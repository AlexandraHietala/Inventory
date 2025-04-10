USE [StarryEdenItem]
GO

EXEC [app].[spGetASeries] 1
EXEC [app].[spGetSeries] NULL
EXEC [app].[spGetSeries] 'uni'
EXEC [app].[spAddSeries] 'un-cornos', 1, 'not unicornos', 'AHIETALA'
EXEC [app].[spUpdateSeries] 3, 'un-cornos', 1, 'def not unicornos', 'AHIETALA'
EXEC [app].[spRemoveSeries] 3, 'AHIETALA'
EXEC [app].[spGetSeriesHistory] 3