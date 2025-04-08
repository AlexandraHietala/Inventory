-----------------------------------------------------------

-- Notes:


-----------------------------------------------------------

USE [StarryEdenUser]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-----------------------------------------------------------

CREATE OR ALTER FUNCTION [app].[fnGetUserHistory]
(
   @id int
)
RETURNS TABLE
AS

   RETURN 
   (
			WITH userChangesDraft1 AS (
					SELECT 
						EFFECTIVE_DATE,
						INEFFECTIVE_DATE,
						[ID],
						[NAME],
						PASS_SALT,
						PASS_HASH,
						ROLE_ID,
						LAST_MODIFIED_BY,
						nextID = LEAD([ID]) OVER (PARTITION BY [ID] ORDER BY EFFECTIVE_DATE),	
						nextNAME = LEAD([NAME]) OVER (PARTITION BY [ID] ORDER BY EFFECTIVE_DATE),	
						nextPASS_SALT = LEAD(PASS_SALT) OVER (PARTITION BY [ID] ORDER BY EFFECTIVE_DATE),	
						nextPASS_HASH = LEAD(PASS_HASH) OVER (PARTITION BY [ID] ORDER BY EFFECTIVE_DATE),	
						nextROLE_ID = LEAD(ROLE_ID) OVER (PARTITION BY [ID] ORDER BY EFFECTIVE_DATE),	
						nextLAST_MODIFIED_BY = LEAD(LAST_MODIFIED_BY) OVER (PARTITION BY [ID] ORDER BY EFFECTIVE_DATE
					)
				FROM [hist].[Audit_Users]
				WHERE [ID] = @id
			), 
			userChangesDraft2 AS (
				SELECT 
					[ID],
					COLNAME AS [COLUMN],
					VALUE,
					NEXTVALUE,
					NULL as SNAPSHOT,
					EFFECTIVE_DATE,
					INEFFECTIVE_DATE,
					nextLAST_MODIFIED_BY as LAST_MODIFIED_BY
				FROM userChangesDraft1
					CROSS APPLY (VALUES																					
						('USER ID', CAST([ID] AS NVARCHAR(500)),CAST(nextID AS NVARCHAR(500))),
						('NAME', CAST([NAME] AS NVARCHAR(500)),CAST(nextNAME AS NVARCHAR(500))),
						('PASS SALT', CAST(PASS_SALT AS NVARCHAR(500)),CAST(nextPASS_SALT AS NVARCHAR(500))),
						('PASS HASH', CAST(PASS_HASH AS NVARCHAR(500)),CAST(nextPASS_HASH AS NVARCHAR(500))),
						('ROLE ID', CAST(ROLE_ID AS NVARCHAR(500)),CAST(nextROLE_ID AS NVARCHAR(500)))
					) CA(COLNAME, VALUE, NEXTVALUE)
				WHERE EXISTS(SELECT VALUE EXCEPT SELECT NEXTVALUE)
					AND [ID] = @id
					AND INEFFECTIVE_DATE IS NOT NULL
					AND nextID IS NOT NULL
			), 
			userChanges AS (
			  SELECT 
				   INEFFECTIVE_DATE as DATEOFCHANGE, 
				   LAST_MODIFIED_BY as CHANGEDBY, 
				   'Changed record value ' + [COLUMN] + ' from ' + CASE WHEN VALUE IS NULL THEN '[]' ELSE '[' + VALUE + ']' END + ' to ' + CASE WHEN NEXTVALUE IS NULL THEN '[]' ELSE '[' + NEXTVALUE + ']' END AS CHANGE 
			  FROM userChangesDraft2 nc
			),
			userAddsDraft AS (
					SELECT 
						[ID],
						NULL as [COLUMN],
						NULL as VALUE,
						NULL as NEXTVALUE,
						(SELECT 
							'USER ID: [' + CAST([ID] AS NVARCHAR(500)) + '],' + 
							' NAME: [' + CAST([NAME] AS NVARCHAR(500)) + '],' + 
							' PASS SALT: [' + CAST(PASS_SALT AS NVARCHAR(500)) + '],' + 
							' PASS HASH: [' + CAST(PASS_HASH AS NVARCHAR(500)) + '],' + 
							' ROLE: [' + CASE WHEN ROLE_ID IS NULL THEN '' ELSE CAST(ROLE_ID AS NVARCHAR(500)) + ' (' + (SELECT TOP 1 [DESCRIPTION] FROM [app].[vwRoles] WHERE ID = ROLE_ID) + ')' END + ']'
						) as [SNAPSHOT],
						EFFECTIVE_DATE,
						INEFFECTIVE_DATE,
						LAST_MODIFIED_BY 
					FROM [hist].[Audit_Users]
					WHERE
						[ID] = @id
						AND ADDED = 1
			),
			userAdds AS (
					SELECT 
						EFFECTIVE_DATE as DATEOFCHANGE, 
						LAST_MODIFIED_BY as CHANGEDBY, 
						'Added record with values [' + [SNAPSHOT] + ']' AS CHANGE
					FROM userAddsDraft
			),
			userDeletesDraft AS (
					SELECT 
						[ID],
						NULL as [COLUMN],
						NULL as VALUE,
						NULL as NEXTVALUE,
						(SELECT 
							'USER ID: [' + CAST([ID] AS NVARCHAR(500)) + '],' + 
							' NAME: [' + CAST([NAME] AS NVARCHAR(500)) + '],' + 
							' PASS SALT: [' + CAST(PASS_SALT AS NVARCHAR(500)) + '],' + 
							' PASS HASH: [' + CAST(PASS_HASH AS NVARCHAR(500)) + '],' + 
							' ROLE: [' + CASE WHEN ROLE_ID IS NULL THEN '' ELSE CAST(ROLE_ID AS NVARCHAR(500)) + ' (' + (SELECT TOP 1 [DESCRIPTION] FROM [app].[vwRoles] WHERE ID = ROLE_ID) + ')' END + ']'
						) as [SNAPSHOT],
						EFFECTIVE_DATE,
						INEFFECTIVE_DATE,
						LAST_MODIFIED_BY 
					FROM [hist].[Audit_Users]
					WHERE
						[ID] = @id
						AND DELETED = 1
			),
			userDeletes AS (
				SELECT 
					EFFECTIVE_DATE as DATEOFCHANGE, 
					LAST_MODIFIED_BY as CHANGEDBY, 
					'Deleted record with values [' + [SNAPSHOT] + ']' AS CHANGE
				FROM userDeletesDraft
			)
			
			SELECT 'User' as CHANGED, * FROM userChanges
			UNION ALL 
			SELECT 'User' as CHANGED, * FROM userAdds
			UNION ALL 
			SELECT 'User' as CHANGED, * FROM userDeletes
		
   );

   GO

-----------------------------------------------------------
