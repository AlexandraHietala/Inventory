-----------------------------------------------------------

-- Notes:


-----------------------------------------------------------

USE [StarryEdenCollection]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-----------------------------------------------------------

CREATE OR ALTER FUNCTION [app].[fnGetCollectionHistory]
(
   @id int
)
RETURNS TABLE
AS

   RETURN 
   (
			WITH collectionChangesDraft1 AS (
					SELECT 
						EFFECTIVE_DATE,
						INEFFECTIVE_DATE,
						[ID],
						[COLLECTION_NAME],
						[DESCRIPTION],
						LAST_MODIFIED_BY,
						nextID = LEAD(ID) OVER (PARTITION BY ID ORDER BY EFFECTIVE_DATE),	
						nextCOLLECTION_NAME = LEAD(COLLECTION_NAME) OVER (PARTITION BY ID ORDER BY EFFECTIVE_DATE),	
						nextDESCRIPTION = LEAD([DESCRIPTION]) OVER (PARTITION BY ID ORDER BY EFFECTIVE_DATE),
						nextLAST_MODIFIED_BY = LEAD(LAST_MODIFIED_BY) OVER (PARTITION BY ID ORDER BY EFFECTIVE_DATE
					)
				FROM [hist].[Audit_Collections]
				WHERE ID = @id
			), 
			collectionChangesDraft2 AS (
				SELECT 
					ID,
					COLNAME AS [COLUMN],
					VALUE,
					NEXTVALUE,
					NULL as SNAPSHOT,
					EFFECTIVE_DATE,
					INEFFECTIVE_DATE,
					nextLAST_MODIFIED_BY as LAST_MODIFIED_BY
				FROM collectionChangesDraft1
					CROSS APPLY (VALUES																					
						('COLLECTION ID', CAST([ID] AS NVARCHAR(500)),CAST(nextID AS NVARCHAR(500))),
						('COLLECTION NAME', CAST([COLLECTION_NAME] AS NVARCHAR(500)),CAST(nextCOLLECTION_NAME AS NVARCHAR(500))),
						('DESCRIPTION', CAST(DESCRIPTION AS NVARCHAR(500)),CAST(nextDESCRIPTION AS NVARCHAR(500)))
					) CA(COLNAME, VALUE, NEXTVALUE)
				WHERE EXISTS(SELECT VALUE EXCEPT SELECT NEXTVALUE)
					AND [ID] = @id
					AND INEFFECTIVE_DATE IS NOT NULL
					AND nextID IS NOT NULL
			), 
			collectionChanges AS (
			  SELECT 
				   INEFFECTIVE_DATE as DATEOFCHANGE, 
				   LAST_MODIFIED_BY as CHANGEDBY, 
				   'Changed record value ' + [COLUMN] + ' from ' + CASE WHEN VALUE IS NULL THEN '[]' ELSE '[' + VALUE + ']' END + ' to ' + CASE WHEN NEXTVALUE IS NULL THEN '[]' ELSE '[' + NEXTVALUE + ']' END AS CHANGE 
			  FROM collectionChangesDraft2 nc
			),
			collectionAddsDraft AS (
					SELECT 
						ID as [ID],
						NULL as [COLUMN],
						NULL as VALUE,
						NULL as NEXTVALUE,
						(SELECT 
							'COLLECTION ID: [' + CAST([ID] AS NVARCHAR(500)) + '],' + 
							' COLLECTION NAME: [' + CAST([COLLECTION_NAME] AS NVARCHAR(500)) + '],' + 
							' DESCRIPTION: [' + CASE WHEN DESCRIPTION IS NULL THEN '' ELSE DESCRIPTION END + ']' 
						) as [SNAPSHOT],
						EFFECTIVE_DATE,
						INEFFECTIVE_DATE,
						LAST_MODIFIED_BY 
					FROM [hist].[Audit_Collections]
					WHERE
						ID = @id
						AND ADDED = 1
			),
			collectionAdds AS (
					SELECT 
						EFFECTIVE_DATE as DATEOFCHANGE, 
						LAST_MODIFIED_BY as CHANGEDBY, 
						'Added record with values [' + [SNAPSHOT] + ']' AS CHANGE
					FROM collectionAddsDraft
			),
			collectionDeletesDraft AS (
					SELECT 
						ID as [ID],
						NULL as [COLUMN],
						NULL as VALUE,
						NULL as NEXTVALUE,
						(SELECT 
							'COLLECTION ID: [' + CAST([ID] AS NVARCHAR(500)) + '],' + 
							' COLLECTION NAME: [' + CAST([COLLECTION_NAME] AS NVARCHAR(500)) + '],' + 
							' DESCRIPTION: [' + CASE WHEN DESCRIPTION IS NULL THEN '' ELSE DESCRIPTION END + ']' 
						) as [SNAPSHOT],
						EFFECTIVE_DATE,
						INEFFECTIVE_DATE,
						LAST_MODIFIED_BY 
					FROM [hist].[Audit_Collections]
					WHERE
						ID = @id
						AND DELETED = 1
			),
			collectionDeletes AS (
				SELECT 
					EFFECTIVE_DATE as DATEOFCHANGE, 
					LAST_MODIFIED_BY as CHANGEDBY, 
					'Deleted record with values [' + [SNAPSHOT] + ']' AS CHANGE
				FROM collectionDeletesDraft
			)
			
			SELECT 'Collection' as CHANGED, * FROM collectionChanges
			UNION ALL 
			SELECT 'Collection' as CHANGED, * FROM collectionAdds
			UNION ALL 
			SELECT 'Collection' as CHANGED, * FROM collectionDeletes
		
   );

   GO

-----------------------------------------------------------