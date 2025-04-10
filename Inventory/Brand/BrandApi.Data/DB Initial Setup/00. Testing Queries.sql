USE [StarryEdenBrand]
GO

EXEC [app].[spGetBrand] 1
EXEC [app].[spGetBrands] NULL
EXEC [app].[spGetBrands] 'cocoa'
EXEC [app].[spAddBrand] 'MyTest Brand', 'This is just for testing', 'AHIETALA'
EXEC [app].[spAddBrand] 'St. James Infirmary', 'Oh Cocoa', 'AHIETALA'
EXEC [app].[spUpdateBrand] 3, 'St. James Infirmary', NULL, 'AHIETALA'
EXEC [app].[spRemoveBrand] 3, 'AHIETALA'
EXEC [app].[spGetBrandHistory] 3