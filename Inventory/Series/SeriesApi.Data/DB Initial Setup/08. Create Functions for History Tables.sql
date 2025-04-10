-----------------------------------------------------------

-- Notes:


-----------------------------------------------------------

USE [StarryEdenSeries]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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