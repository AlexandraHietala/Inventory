-----------------------------------------------------------

-- Notes:


-----------------------------------------------------------

USE [StarryEdenItem]
GO

-----------------------------------------------------------

CREATE OR ALTER VIEW [app].[vwBrands] AS
	
SELECT *
FROM [StarryEdenBrand].[app].[Brands]

GO

-----------------------------------------------------------

CREATE OR ALTER VIEW [app].[vwSeries] AS
	
SELECT *
FROM [StarryEdenSeries].[app].[Series]

GO

-----------------------------------------------------------

CREATE OR ALTER VIEW [app].[vwItemComments] AS
	
SELECT *
FROM [app].[ItemComments]

GO

-----------------------------------------------------------

CREATE OR ALTER VIEW [app].[vwItems] AS
	
SELECT *
FROM [app].[Items]

GO

-----------------------------------------------------------

CREATE OR ALTER VIEW [app].[vwCollections] AS
	
SELECT *
FROM [StarryEdenCollection].[app].[vwCollections]

GO

-----------------------------------------------------------

CREATE OR ALTER VIEW [app].[vwItems_Expanded]
AS

	SELECT
		item.*, 
		
		-- Series in format of   1 SeriesA 2 SeriesB 3 SeriesC
		SUBSTRING (
		(
			SELECT ' ' + CAST(ser.ID as varchar) + ' ' + ser.SERIES_NAME AS [text()]
			FROM [app].[vwSeries] ser
			WHERE item.SERIES_ID = ser.ID
			ORDER BY ser.ID
			FOR XML PATH ('')
		), 1, 10000) [SERIES],

		-- Brand in format of   1 BrandA 2 BrandB 3 BrandC
		SUBSTRING (
		(
			SELECT ' ' + CAST(bra.ID as varchar) + ' ' + bra.BRAND_NAME AS [text()]
			FROM [app].[vwBrands] bra
			WHERE item.BRAND_ID = bra.ID
			ORDER BY bra.ID
			FOR XML PATH ('')
		), 1, 10000) [BRAND],

		-- Collection in format of   1 CollectionA 2 CollectionB 3 CollectionC
		SUBSTRING (
		(
			SELECT ' ' + CAST(col.ID as varchar) + ' ' + col.COLLECTION_NAME AS [text()]
			FROM [app].[vwCollections] col
			WHERE item.COLLECTION_ID = col.ID
			ORDER BY col.ID
			FOR XML PATH ('')
		), 1, 10000) [COLLECTION]

	FROM [app].[vwItems] item;

GO

-----------------------------------------------------------

CREATE OR ALTER VIEW [app].[vwItems_Search]
AS

SELECT
	    item.*,
		(CONCAT([ID],' ',[STATUS],' ',[TYPE],' ',[SERIES],' ',[BRAND],' ',[NAME], ' ', [DESCRIPTION], ' ',[FORMAT],' ',[SIZE],' ',[YEAR])) as [FULLDATA]
FROM [app].[vwItems_Expanded] item

GO

-----------------------------------------------------------