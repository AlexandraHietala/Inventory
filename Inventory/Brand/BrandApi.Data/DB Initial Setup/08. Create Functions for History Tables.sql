-----------------------------------------------------------

-- Notes:


-----------------------------------------------------------

USE [StarryEdenBrand]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-----------------------------------------------------------

CREATE OR ALTER FUNCTION [app].[fnGetBrandHistory]
(
   @id int
)
RETURNS TABLE
AS

   RETURN 
   (
			WITH brandChangesDraft1 AS (
					SELECT 
						EFFECTIVE_DATE,
						INEFFECTIVE_DATE,
						[ID],
						[BRAND_NAME],
						DESCRIPTION,
						LAST_MODIFIED_BY,
						nextID = LEAD(ID) OVER (PARTITION BY ID ORDER BY EFFECTIVE_DATE),	
						nextBRAND_NAME = LEAD([BRAND_NAME]) OVER (PARTITION BY ID ORDER BY EFFECTIVE_DATE),	
						nextDESCRIPTION = LEAD(DESCRIPTION) OVER (PARTITION BY ID ORDER BY EFFECTIVE_DATE),
						nextLAST_MODIFIED_BY = LEAD(LAST_MODIFIED_BY) OVER (PARTITION BY ID ORDER BY EFFECTIVE_DATE
					)
				FROM [hist].[Audit_Brands]
				WHERE ID = @id
			), 
			brandChangesDraft2 AS (
				SELECT 
					ID,
					COLNAME AS [COLUMN],
					VALUE,
					NEXTVALUE,
					NULL as SNAPSHOT,
					EFFECTIVE_DATE,
					INEFFECTIVE_DATE,
					nextLAST_MODIFIED_BY as LAST_MODIFIED_BY
				FROM brandChangesDraft1
					CROSS APPLY (VALUES																					
						('BRAND ID', CAST([ID] AS NVARCHAR(500)),CAST(nextID AS NVARCHAR(500))),
						('BRAND NAME', CAST([BRAND_NAME] AS NVARCHAR(500)),CAST(nextBRAND_NAME AS NVARCHAR(500))),
						('DESCRIPTION', CAST(DESCRIPTION AS NVARCHAR(500)),CAST(nextDESCRIPTION AS NVARCHAR(500)))
					) CA(COLNAME, VALUE, NEXTVALUE)
				WHERE EXISTS(SELECT VALUE EXCEPT SELECT NEXTVALUE)
					AND [ID] = @id
					AND INEFFECTIVE_DATE IS NOT NULL
					AND nextID IS NOT NULL
			), 
			brandChanges AS (
			  SELECT 
				   INEFFECTIVE_DATE as DATEOFCHANGE, 
				   LAST_MODIFIED_BY as CHANGEDBY, 
				   'Changed record value ' + [COLUMN] + ' from ' + CASE WHEN VALUE IS NULL THEN '[]' ELSE '[' + VALUE + ']' END + ' to ' + CASE WHEN NEXTVALUE IS NULL THEN '[]' ELSE '[' + NEXTVALUE + ']' END AS CHANGE 
			  FROM brandChangesDraft2 nc
			),
			brandAddsDraft AS (
					SELECT 
						ID as [ID],
						NULL as [COLUMN],
						NULL as VALUE,
						NULL as NEXTVALUE,
						(SELECT 
							'BRAND ID: [' + CAST([ID] AS NVARCHAR(500)) + '],' + 
							' BRAND NAME: [' + CAST([BRAND_NAME] AS NVARCHAR(500)) + '],' + 
							' DESCRIPTION: [' + CASE WHEN DESCRIPTION IS NULL THEN '' ELSE DESCRIPTION END + ']' 
						) as [SNAPSHOT],
						EFFECTIVE_DATE,
						INEFFECTIVE_DATE,
						LAST_MODIFIED_BY 
					FROM [hist].[Audit_Brands]
					WHERE
						ID = @id
						AND ADDED = 1
			),
			brandAdds AS (
					SELECT 
						EFFECTIVE_DATE as DATEOFCHANGE, 
						LAST_MODIFIED_BY as CHANGEDBY, 
						'Added record with values [' + [SNAPSHOT] + ']' AS CHANGE
					FROM brandAddsDraft
			),
			brandDeletesDraft AS (
					SELECT 
						ID as [ID],
						NULL as [COLUMN],
						NULL as VALUE,
						NULL as NEXTVALUE,
						(SELECT 
							'BRAND ID: [' + CAST([ID] AS NVARCHAR(500)) + '],' + 
							' BRAND NAME: [' + CAST([BRAND_NAME] AS NVARCHAR(500)) + '],' + 
							' DESCRIPTION: [' + CASE WHEN DESCRIPTION IS NULL THEN '' ELSE DESCRIPTION END + ']' 
						) as [SNAPSHOT],
						EFFECTIVE_DATE,
						INEFFECTIVE_DATE,
						LAST_MODIFIED_BY 
					FROM [hist].[Audit_Brands]
					WHERE
						ID = @id
						AND DELETED = 1
			),
			brandDeletes AS (
				SELECT 
					EFFECTIVE_DATE as DATEOFCHANGE, 
					LAST_MODIFIED_BY as CHANGEDBY, 
					'Deleted record with values [' + [SNAPSHOT] + ']' AS CHANGE
				FROM brandDeletesDraft
			)
			
			SELECT 'Brand' as CHANGED, * FROM brandChanges
			UNION ALL 
			SELECT 'Brand' as CHANGED, * FROM brandAdds
			UNION ALL 
			SELECT 'Brand' as CHANGED, * FROM brandDeletes
		
   );

   GO

-----------------------------------------------------------
