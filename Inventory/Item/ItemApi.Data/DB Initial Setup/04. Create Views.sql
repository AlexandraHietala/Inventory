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
