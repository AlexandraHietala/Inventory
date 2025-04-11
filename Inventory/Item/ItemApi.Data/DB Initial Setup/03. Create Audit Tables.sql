-----------------------------------------------------------

-- Notes:


-----------------------------------------------------------

USE [StarryEdenItem]
GO

-----------------------------------------------------------

CREATE TABLE [hist].[Audit_Items](
	[EFFECTIVE_DATE] [datetime] NOT NULL DEFAULT GetDate(),
	[INEFFECTIVE_DATE] [datetime] NULL,
	[ID] [int] NOT NULL,
	[STATUS] [varchar](10) NOT NULL,
	[TYPE] [varchar](15) NOT NULL,
	[BRAND] [varchar](100) NULL,
	[SERIES] [varchar](100) NULL,
	[NAME] [varchar](100) NULL,
	[DESCRIPTION] [varchar](250) NULL,
	[FORMAT] [varchar](15) NOT NULL,
	[SIZE] [varchar](10) NOT NULL,
	[YEAR] [int] NULL,
	[PHOTO] [varchar](max) NULL,
	[CREATED_BY] [varchar](100) NOT NULL,
	[CREATED_DATE] [datetime] NOT NULL,
	[ADDED] [bit] NOT NULL DEFAULT 0,
	[DELETED] [bit] NOT NULL DEFAULT 0,
	[LAST_MODIFIED_BY] [varchar](100) NULL,
	[LAST_MODIFIED_DATE] [datetime] NULL,
 CONSTRAINT [PK_Audit_Items] PRIMARY KEY CLUSTERED 
(
	[EFFECTIVE_DATE] ASC,
	[ID] ASC
))

GO

-----------------------------------------------------------

CREATE TABLE [hist].[Audit_ItemComments](
	[EFFECTIVE_DATE] [datetime] NOT NULL DEFAULT GetDate(),
	[INEFFECTIVE_DATE] [datetime] NULL,
	[ID] [int] NOT NULL,
	[ITEM_ID] [int] NOT NULL,
	[COMMENT] [varchar](max) NOT NULL,
	[ADDED] [bit] NOT NULL DEFAULT 0,
	[DELETED] [bit] NOT NULL DEFAULT 0,
	[CREATED_BY] [varchar](100) NOT NULL,
	[CREATED_DATE] [datetime] NOT NULL,
	[LAST_MODIFIED_BY] [varchar](100) NULL,
	[LAST_MODIFIED_DATE] [datetime] NULL,
 CONSTRAINT [PK_Audit_ItemComments] PRIMARY KEY CLUSTERED 
(
	[EFFECTIVE_DATE] ASC,
	[ID] ASC,
	[ITEM_ID] ASC
))

GO

-----------------------------------------------------------
