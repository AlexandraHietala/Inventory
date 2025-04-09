USE [StarryEdenCollection]
GO

EXEC [app].[spGetCollection] 1
EXEC [app].[spGetCollections] NULL
EXEC [app].[spGetCollections] 'cocoa'
EXEC [app].[spAddCollection] 'MyTest Brand', 'This is just for testing', 'AHIETALA'
EXEC [app].[spAddCollection] 'St. James Infirmary', 'Oh Cocoa', 'AHIETALA'
EXEC [app].[spUpdateCollection] 2, 'St. James Infirmary', NULL, 'AHIETALA'
EXEC [app].[spRemoveCollection] 2, 'AHIETALA'
EXEC [app].[spGetHistory] 1, 'Collection'