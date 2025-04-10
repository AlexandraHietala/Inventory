-----------------------------------------------------------

-- Notes:


-----------------------------------------------------------

USE [StarryEdenSeries]
GO

-----------------------------------------------------------

CREATE TABLE [app].[Series](
	[ID] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[SERIES_NAME] [varchar](250) NOT NULL,
	[DESCRIPTION] [varchar](250) NULL,
	[CREATED_BY] [varchar](100) NOT NULL DEFAULT 'SYSTEM',
	[CREATED_DATE] [datetime] NOT NULL DEFAULT GetDate(),
	[LAST_MODIFIED_BY] [varchar](100) NULL DEFAULT 'SYSTEM',
	[LAST_MODIFIED_DATE] [datetime] NULL DEFAULT GetDate(),
)

GO

-----------------------------------------------------------