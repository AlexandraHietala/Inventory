USE [StarryEdenItem]
GO

EXEC [app].[spGetItems] NULL
EXEC [app].[spGetItems] 'min'
EXEC [app].[spGetItemsPerCollection] 1, 'pri'
EXEC [app].[spGetItemsPerCollection] 1, NULL
EXEC [app].[spSearchItems] 0, 5, 'STATUS', 'ASC', NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL
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

