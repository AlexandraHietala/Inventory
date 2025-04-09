-----------------------------------------------------------

-- Notes:


-----------------------------------------------------------

USE [StarryEdenCollection]
GO

-----------------------------------------------------------

CREATE TABLE [app].[Collections](
	[ID] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[COLLECTION_NAME] [varchar](50) NOT NULL,
	[DESCRIPTION] [varchar](250) NULL,
	[CREATED_BY] [varchar](100) NOT NULL DEFAULT 'SYSTEM',
	[CREATED_DATE] [datetime] NOT NULL DEFAULT GetDate(),
	[LAST_MODIFIED_BY] [varchar](100) NULL DEFAULT 'SYSTEM',
	[LAST_MODIFIED_DATE] [datetime] NULL DEFAULT GetDate(),
)

GO

-----------------------------------------------------------
