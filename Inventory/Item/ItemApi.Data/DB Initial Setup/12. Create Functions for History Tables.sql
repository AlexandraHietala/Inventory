-----------------------------------------------------------

-- Notes:


-----------------------------------------------------------

USE [StarryEdenItem]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-----------------------------------------------------------

CREATE OR ALTER FUNCTION [app].[fnGetItemHistory]
(
   @id int
)
RETURNS TABLE
AS

   RETURN 
   (
			WITH itemChangesDraft1 AS (
					SELECT 
						EFFECTIVE_DATE,
						INEFFECTIVE_DATE,
						ID,
						STATUS,
						TYPE,
						BRAND_ID,
						SERIES_ID,
						NAME,
						DESCRIPTION,
						FORMAT,
						SIZE,
						YEAR,
						PHOTO,
						LAST_MODIFIED_BY,
						nextID = LEAD(ID) OVER (PARTITION BY ID, CREATED_DATE ORDER BY EFFECTIVE_DATE),	
						nextSTATUS = LEAD(STATUS) OVER (PARTITION BY ID, CREATED_DATE ORDER BY EFFECTIVE_DATE),	
						nextTYPE = LEAD(TYPE) OVER (PARTITION BY ID, CREATED_DATE ORDER BY EFFECTIVE_DATE),
						nextBRAND_ID = LEAD(BRAND_ID) OVER (PARTITION BY ID, CREATED_DATE ORDER BY EFFECTIVE_DATE),
						nextSERIES_ID = LEAD(SERIES_ID) OVER (PARTITION BY ID, CREATED_DATE ORDER BY EFFECTIVE_DATE),
						nextNAME = LEAD(NAME) OVER (PARTITION BY ID, CREATED_DATE ORDER BY EFFECTIVE_DATE),	
						nextDESCRIPTION = LEAD(DESCRIPTION) OVER (PARTITION BY ID, CREATED_DATE ORDER BY EFFECTIVE_DATE),	
						nextFORMAT = LEAD(FORMAT) OVER (PARTITION BY ID, CREATED_DATE ORDER BY EFFECTIVE_DATE),
						nextSIZE = LEAD(SIZE) OVER (PARTITION BY ID, CREATED_DATE ORDER BY EFFECTIVE_DATE),
						nextYEAR = LEAD(YEAR) OVER (PARTITION BY ID, CREATED_DATE ORDER BY EFFECTIVE_DATE),
						nextPHOTO = LEAD(PHOTO) OVER (PARTITION BY ID, CREATED_DATE ORDER BY EFFECTIVE_DATE),
						nextLAST_MODIFIED_BY = LEAD(LAST_MODIFIED_BY) OVER (PARTITION BY ID, CREATED_DATE ORDER BY EFFECTIVE_DATE
					)
				FROM [hist].[Audit_Items]
				WHERE ID = @id
			),
			itemChangesDraft2 AS (
				SELECT 
					ID,
					COLNAME AS [COLUMN],
					VALUE,
					NEXTVALUE,
					NULL as SNAPSHOT,
					EFFECTIVE_DATE,
					INEFFECTIVE_DATE,
					nextLAST_MODIFIED_BY as LAST_MODIFIED_BY
				FROM itemChangesDraft1
					CROSS APPLY (VALUES																					
						('ITEM ID', CAST(ID AS NVARCHAR(500)),CAST(nextID AS NVARCHAR(500))),
						('STATUS', CAST(STATUS AS NVARCHAR(500)),CAST(nextSTATUS AS NVARCHAR(500))),
						('TYPE', CAST(TYPE AS NVARCHAR(500)),CAST(nextTYPE AS NVARCHAR(500))),
						('BRAND', CAST((SELECT TOP 1 BRAND_NAME FROM [app].[vwBrands] WHERE ID = BRAND_ID) AS NVARCHAR(500)),CAST((SELECT TOP 1 BRAND_NAME FROM [app].[vwBrands] WHERE ID = nextBRAND_ID) AS NVARCHAR(500))),
						('SERIES', CAST((SELECT TOP 1 SERIES_NAME FROM [app].[vwSeries] WHERE ID = SERIES_ID) AS NVARCHAR(500)),CAST((SELECT TOP 1 SERIES_NAME FROM [app].[vwSeries] WHERE ID = nextSERIES_ID) AS NVARCHAR(500))),
						('NAME', CAST(NAME AS NVARCHAR(500)),CAST(nextNAME AS NVARCHAR(500))),
						('DESCRIPTION', CAST(DESCRIPTION AS NVARCHAR(500)),CAST(nextDESCRIPTION AS NVARCHAR(500))),
						('FORMAT', CAST(FORMAT AS NVARCHAR(500)),CAST(nextFORMAT AS NVARCHAR(500))),
						('SIZE', CAST(SIZE AS NVARCHAR(500)),CAST(nextSIZE AS NVARCHAR(500))),
						('YEAR', CAST(YEAR AS NVARCHAR(500)),CAST(nextYEAR AS NVARCHAR(500))),
						('PHOTO', CAST(PHOTO AS NVARCHAR(500)),CAST(nextPHOTO AS NVARCHAR(500)))
					) CA(COLNAME, VALUE, NEXTVALUE)
				WHERE EXISTS(SELECT VALUE EXCEPT SELECT NEXTVALUE)
					AND ID = @id
					AND INEFFECTIVE_DATE IS NOT NULL
					AND nextID IS NOT NULL
			), 
			itemChanges AS (
			  SELECT 
				   INEFFECTIVE_DATE as DATEOFCHANGE, 
				   LAST_MODIFIED_BY as CHANGEDBY, 
				   'Changed record value ' + [COLUMN] + ' from ' + CASE WHEN VALUE IS NULL THEN '[]' ELSE '[' + VALUE + ']' END + ' to ' + CASE WHEN NEXTVALUE IS NULL THEN '[]' ELSE '[' + NEXTVALUE + ']' END AS CHANGE 
			  FROM itemChangesDraft2 nc
			),
			itemAddsDraft AS (
					SELECT 
						ID,
						NULL as [COLUMN],
						NULL as VALUE,
						NULL as NEXTVALUE,
						(SELECT 
							'ITEM ID: [' + CAST(ID AS NVARCHAR(500)) + '],' + 
							' STATUS: [' + STATUS + '],' + 
							' TYPE: [' + TYPE + '],' + 	
							' BRAND: [' + CASE WHEN BRAND_ID IS NULL THEN '' ELSE CAST(BRAND_ID AS NVARCHAR(500)) + ' (' + (SELECT TOP 1 BRAND_NAME FROM [app].[vwBrands] WHERE ID = BRAND_ID) + ')' END + '],' +  
							' SERIES: [' + CASE WHEN SERIES_ID IS NULL THEN '' ELSE CAST(SERIES_ID AS NVARCHAR(500)) + ' (' + (SELECT TOP 1 SERIES_NAME FROM [app].[vwSeries] WHERE ID = SERIES_ID) + ')' END + '],' +  
							' NAME: [' + CASE WHEN NAME IS NULL THEN '' ELSE NAME END + '],' + 
							' DESCRIPTION: [' + CASE WHEN DESCRIPTION IS NULL THEN '' ELSE DESCRIPTION END + '],' + 
							' FORMAT: [' + FORMAT + '],' +
							' SIZE: [' + SIZE + '],' + 
							' YEAR: [' + CASE WHEN YEAR IS NULL THEN '' ELSE CAST(YEAR AS NVARCHAR(500)) END + '],' + 
							' PHOTO: [' + CASE WHEN PHOTO IS NULL THEN '' ELSE PHOTO END + ']'
						) as [SNAPSHOT],
						EFFECTIVE_DATE,
						INEFFECTIVE_DATE,
						LAST_MODIFIED_BY 
					FROM [hist].[Audit_Items]
					WHERE
						ID = @id
						AND ADDED = 1
						
			),
			itemAdds AS (
					SELECT 
						EFFECTIVE_DATE as DATEOFCHANGE, 
						LAST_MODIFIED_BY as CHANGEDBY, 
						'Added record with values [' + [SNAPSHOT] + ']' AS CHANGE
					FROM itemAddsDraft
			),
			itemDeletesDraft AS (
					SELECT 
						ID,
						NULL as [COLUMN],
						NULL as VALUE,
						NULL as NEXTVALUE,
						(SELECT 
							'ITEM ID: [' + CAST(ID AS NVARCHAR(500)) + '],' + 
							' STATUS: [' + STATUS + '],' + 
							' TYPE: [' + TYPE + '],' + 	
							' BRAND: [' + CASE WHEN BRAND_ID IS NULL THEN '' ELSE CAST(BRAND_ID AS NVARCHAR(500)) + ' (' + (SELECT TOP 1 BRAND_NAME FROM [app].[vwBrands] WHERE ID = BRAND_ID) + ')' END + '],' +   +  
							' SERIES: [' + CASE WHEN SERIES_ID IS NULL THEN '' ELSE CAST(SERIES_ID AS NVARCHAR(500)) + ' (' + (SELECT TOP 1 SERIES_NAME FROM [app].[vwSeries] WHERE ID = SERIES_ID) + ')' END + '],' +   +  
							' NAME: [' + CASE WHEN NAME IS NULL THEN '' ELSE NAME END + '],' + 
							' DESCRIPTION: [' + CASE WHEN DESCRIPTION IS NULL THEN '' ELSE DESCRIPTION END + '],' + 
							' FORMAT: [' + FORMAT + '],' +
							' SIZE: [' + SIZE + '],' + 
							' YEAR: [' + CASE WHEN YEAR IS NULL THEN '' ELSE CAST(YEAR AS NVARCHAR(500)) END + '],' + 
							' PHOTO: [' + CASE WHEN PHOTO IS NULL THEN '' ELSE PHOTO END + ']'
						) as [SNAPSHOT],
						EFFECTIVE_DATE,
						INEFFECTIVE_DATE,
						LAST_MODIFIED_BY 
					FROM [hist].[Audit_Items]
					WHERE
						ID = @id
						AND DELETED = 1
			),
			itemDeletes AS (
				SELECT 
					EFFECTIVE_DATE as DATEOFCHANGE, 
					LAST_MODIFIED_BY as CHANGEDBY, 
					'Deleted record with values [' + [SNAPSHOT] + ']' AS CHANGE
				FROM itemDeletesDraft
			)
			
			SELECT 'Item' as CHANGED, * FROM itemChanges
			UNION ALL 
			SELECT 'Item' as CHANGED, * FROM itemAdds
			UNION ALL 
			SELECT 'Item' as CHANGED, * FROM itemDeletes
		
   );

   GO

-----------------------------------------------------------

CREATE OR ALTER FUNCTION [app].[fnGetItemCommentHistory]
(
   @id int
)
RETURNS TABLE
AS

   RETURN 
   (
			WITH commentAddsDraft AS (
					SELECT 
						ITEM_ID,
						NULL as [COLUMN],
						NULL as VALUE,
						NULL as NEXTVALUE,
						(SELECT 
							'COMMENT ID: [' + CAST(ID AS NVARCHAR(500)) + '],' + 
							' ITEM ID: [' + CAST(ITEM_ID AS NVARCHAR(500)) + '],' + 
							' COMMENT: [' + CAST(COMMENT AS NVARCHAR(500)) + ']'
						) as [SNAPSHOT],
						EFFECTIVE_DATE,
						INEFFECTIVE_DATE,
						LAST_MODIFIED_BY 
					FROM [hist].[Audit_ItemComments]
					WHERE
						ITEM_ID = @id
						AND ADDED = 1
			),
			commentAdds AS (
					SELECT 
						EFFECTIVE_DATE as DATEOFCHANGE, 
						LAST_MODIFIED_BY as CHANGEDBY, 
						'Added record with values [' + [SNAPSHOT] + ']' AS CHANGE
					FROM commentAddsDraft
			),
			commentDeletesDraft AS (
					SELECT 
						ITEM_ID,
						NULL as [COLUMN],
						NULL as VALUE,
						NULL as NEXTVALUE,
						(SELECT 
							'COMMENT ID: [' + CAST(ID AS NVARCHAR(500)) + '],' + 
							' ITEM ID: [' + CAST(ITEM_ID AS NVARCHAR(500)) + '],' +
							' COMMENT: [' + CAST(COMMENT AS NVARCHAR(500)) + ']'
						) as [SNAPSHOT],
						EFFECTIVE_DATE,
						INEFFECTIVE_DATE,
						LAST_MODIFIED_BY 
					FROM [hist].[Audit_ItemComments]
					WHERE
						ITEM_ID = @id
						AND DELETED = 1
			),
			commentDeletes AS (
				SELECT 
					EFFECTIVE_DATE as DATEOFCHANGE, 
					LAST_MODIFIED_BY as CHANGEDBY, 
					'Deleted record with values [' + [SNAPSHOT] + ']' AS CHANGE
				FROM commentDeletesDraft
			)
			
			SELECT 'Comment' as CHANGED, * FROM commentAdds
			UNION ALL 
			SELECT 'Comment' as CHANGED, * FROM commentDeletes
		
   );

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

CREATE OR ALTER FUNCTION [app].[fnGetSeriesHistory]
(
   @id int
)
RETURNS TABLE
AS

   RETURN 
   (
			WITH seriesChangesDraft1 AS (
					SELECT 
						EFFECTIVE_DATE,
						INEFFECTIVE_DATE,
						[ID],
						[SERIES_NAME],
						[DESCRIPTION],
						LAST_MODIFIED_BY,
						nextID = LEAD(ID) OVER (PARTITION BY ID ORDER BY EFFECTIVE_DATE),	
						nextSERIES_NAME = LEAD(SERIES_NAME) OVER (PARTITION BY ID ORDER BY EFFECTIVE_DATE),	
						nextDESCRIPTION = LEAD([DESCRIPTION]) OVER (PARTITION BY ID ORDER BY EFFECTIVE_DATE),
						nextLAST_MODIFIED_BY = LEAD(LAST_MODIFIED_BY) OVER (PARTITION BY ID ORDER BY EFFECTIVE_DATE
					)
				FROM [hist].[Audit_Series]
				WHERE ID = @id
			), 
			seriesChangesDraft2 AS (
				SELECT 
					ID,
					COLNAME AS [COLUMN],
					VALUE,
					NEXTVALUE,
					NULL as SNAPSHOT,
					EFFECTIVE_DATE,
					INEFFECTIVE_DATE,
					nextLAST_MODIFIED_BY as LAST_MODIFIED_BY
				FROM seriesChangesDraft1
					CROSS APPLY (VALUES																					
						('SERIES ID', CAST([ID] AS NVARCHAR(500)),CAST(nextID AS NVARCHAR(500))),
						('SERIES NAME', CAST([SERIES_NAME] AS NVARCHAR(500)),CAST(nextSERIES_NAME AS NVARCHAR(500))),
						('DESCRIPTION', CAST(DESCRIPTION AS NVARCHAR(500)),CAST(nextDESCRIPTION AS NVARCHAR(500)))
					) CA(COLNAME, VALUE, NEXTVALUE)
				WHERE EXISTS(SELECT VALUE EXCEPT SELECT NEXTVALUE)
					AND [ID] = @id
					AND INEFFECTIVE_DATE IS NOT NULL
					AND nextID IS NOT NULL
			), 
			seriesChanges AS (
			  SELECT 
				   INEFFECTIVE_DATE as DATEOFCHANGE, 
				   LAST_MODIFIED_BY as CHANGEDBY, 
				   'Changed record value ' + [COLUMN] + ' from ' + CASE WHEN VALUE IS NULL THEN '[]' ELSE '[' + VALUE + ']' END + ' to ' + CASE WHEN NEXTVALUE IS NULL THEN '[]' ELSE '[' + NEXTVALUE + ']' END AS CHANGE 
			  FROM seriesChangesDraft2 nc
			),
			seriesAddsDraft AS (
					SELECT 
						ID as [ID],
						NULL as [COLUMN],
						NULL as VALUE,
						NULL as NEXTVALUE,
						(SELECT 
							'SERIES ID: [' + CAST([ID] AS NVARCHAR(500)) + '],' + 
							' SERIES NAME: [' + CAST([SERIES_NAME] AS NVARCHAR(500)) + '],' + 
							' DESCRIPTION: [' + CASE WHEN DESCRIPTION IS NULL THEN '' ELSE DESCRIPTION END + ']' 
						) as [SNAPSHOT],
						EFFECTIVE_DATE,
						INEFFECTIVE_DATE,
						LAST_MODIFIED_BY 
					FROM [hist].[Audit_Series]
					WHERE
						ID = @id
						AND ADDED = 1
			),
			seriesAdds AS (
					SELECT 
						EFFECTIVE_DATE as DATEOFCHANGE, 
						LAST_MODIFIED_BY as CHANGEDBY, 
						'Added record with values [' + [SNAPSHOT] + ']' AS CHANGE
					FROM seriesAddsDraft
			),
			seriesDeletesDraft AS (
					SELECT 
						ID as [ID],
						NULL as [COLUMN],
						NULL as VALUE,
						NULL as NEXTVALUE,
						(SELECT 
							'SERIES ID: [' + CAST([ID] AS NVARCHAR(500)) + '],' + 
							' SERIES NAME: [' + CAST([SERIES_NAME] AS NVARCHAR(500)) + '],' + 
							' DESCRIPTION: [' + CASE WHEN DESCRIPTION IS NULL THEN '' ELSE DESCRIPTION END + ']' 
						) as [SNAPSHOT],
						EFFECTIVE_DATE,
						INEFFECTIVE_DATE,
						LAST_MODIFIED_BY 
					FROM [hist].[Audit_Series]
					WHERE
						ID = @id
						AND DELETED = 1
			),
			seriesDeletes AS (
				SELECT 
					EFFECTIVE_DATE as DATEOFCHANGE, 
					LAST_MODIFIED_BY as CHANGEDBY, 
					'Deleted record with values [' + [SNAPSHOT] + ']' AS CHANGE
				FROM seriesDeletesDraft
			)
			
			SELECT 'Series' as CHANGED, * FROM seriesChanges
			UNION ALL 
			SELECT 'Series' as CHANGED, * FROM seriesAdds
			UNION ALL 
			SELECT 'Series' as CHANGED, * FROM seriesDeletes
		
   );

   GO

-----------------------------------------------------------