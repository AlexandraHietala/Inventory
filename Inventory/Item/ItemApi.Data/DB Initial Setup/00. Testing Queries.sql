USE [StarryEdenItem]
GO

EXEC [app].[spGetBrand] 1
EXEC [app].[spGetBrands] NULL
EXEC [app].[spGetBrands] 'cocoa'
EXEC [app].[spAddBrand] 'MyTest Brand', 'This is just for testing', 'AHIETALA'
EXEC [app].[spAddBrand] 'St. James Infirmary', 'Oh Cocoa', 'AHIETALA'
EXEC [app].[spUpdateBrand] 3, 'St. James Infirmary', NULL, 'AHIETALA'
EXEC [app].[spRemoveBrand] 3, 'AHIETALA'
EXEC [app].[spGetHistory] 3, 'Brand'

EXEC [app].[spGetASeries] 1
EXEC [app].[spGetSeries] NULL
EXEC [app].[spGetSeries] 'uni'
EXEC [app].[spAddSeries] 'un-cornos', 1, 'not unicornos', 'AHIETALA'
EXEC [app].[spUpdateSeries] 3, 'un-cornos', 1, 'def not unicornos', 'AHIETALA'
EXEC [app].[spRemoveSeries] 3, 'AHIETALA'
EXEC [app].[spGetHistory] 3, 'Series'

EXEC [app].[spGetItems] NULL
EXEC [app].[spGetItems] 'min'
EXEC [app].[spSearchItems] 0, 5, 'STATUS', 'ASC', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL
EXEC [app].[spGetItem] 2
EXEC [app].[spAddItem] 'OWNED', 'BLIND', 1, 'My Item', NULL, 'KEYCHAIN', 'LARGE', 2023, NULL, 'AHIETALA'
EXEC [app].[spUpdateItem] 2, 'OWNED', 'BLIND', 1, 'My Item Record', NULL, 'FIGURE', 'REGULAR', 2016, NULL, 'AHIETALA'
EXEC [app].[spRemoveItem] 2, 'AHIETALA'
EXEC [app].[spGetHistory] 2, 'Item'
EXEC [app].[spGetGeneralHistory] 2

EXEC [app].[spGetItemComment] 1 
EXEC [app].[spGetItemComments] 1 
EXEC [app].[spAddItemComment] 1, 'Test Comment :)', 'AHIETALA'
EXEC [app].[spRemoveItemComment] 2, 'AHIETALA'
EXEC [app].[spGetHistory] 1, 'ItemComment' -- Input is ITEM ID not Comment ID

