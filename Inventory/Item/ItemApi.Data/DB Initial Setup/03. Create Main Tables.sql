-----------------------------------------------------------

-- Notes:


-----------------------------------------------------------

USE [StarryEdenItem]
GO

-----------------------------------------------------------

CREATE TABLE [app].[Items](
	[ID] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[COLLECTION_ID] [int] NOT NULL,
	[STATUS] [varchar](10) NOT NULL CHECK ([STATUS] IN ('OWNED','NOT OWNED','WISHLIST','PENDING')) DEFAULT 'OWNED',
	[TYPE] [varchar](15) NOT NULL CHECK ([TYPE] IN ('BLIND','SOLD SEPARATELY','SET')) DEFAULT 'BLIND',
	[BRAND_ID] [int] NULL,
	[SERIES_ID] [int] NULL,
	[NAME] [varchar](100) NULL,
	[DESCRIPTION] [varchar](250) NULL,
	[FORMAT] [varchar](15) NOT NULL CHECK ([FORMAT] IN ('KEYCHAIN','FIGURE','PLUSH','OTHER')) DEFAULT 'FIGURE',
	[SIZE] [varchar](10) NOT NULL CHECK ([SIZE] IN ('MINI','REGULAR','LARGE','GIANT','IRREGULAR')) DEFAULT 'REGULAR',
	[YEAR] [int] NULL,
	[PHOTO] [varchar](max) NULL,
	[CREATED_BY] [varchar](100) NOT NULL DEFAULT 'SYSTEM',
	[CREATED_DATE] [datetime] NOT NULL DEFAULT GetDate(),
	[LAST_MODIFIED_BY] [varchar](100) NULL DEFAULT 'SYSTEM',
	[LAST_MODIFIED_DATE] [datetime] NULL DEFAULT GetDate(),
)

GO

ALTER TABLE [app].[Items]  WITH CHECK ADD  CONSTRAINT [FK_Items_Series] FOREIGN KEY([SERIES_ID])
REFERENCES [app].[Series] ([ID])
GO

ALTER TABLE [app].[Items] CHECK CONSTRAINT [FK_Items_Series]
GO

ALTER TABLE [app].[Items]  WITH CHECK ADD  CONSTRAINT [FK_Items_Brands] FOREIGN KEY([BRAND_ID])
REFERENCES [app].[Brands] ([ID])
GO

ALTER TABLE [app].[Items] CHECK CONSTRAINT [FK_Items_Brands]
GO

-----------------------------------------------------------
CREATE TABLE [app].[ItemComments](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ITEM_ID] [int] NOT NULL,
	[COMMENT] [varchar](max) NOT NULL,
	[CREATED_BY] [varchar](100) NOT NULL DEFAULT 'SYSTEM',
	[CREATED_DATE] [datetime] NOT NULL DEFAULT GetDate(),
	[LAST_MODIFIED_BY] [varchar](100) NULL DEFAULT 'SYSTEM',
	[LAST_MODIFIED_DATE] [datetime] NULL DEFAULT GetDate(),
 CONSTRAINT [PK_ItemComments] PRIMARY KEY CLUSTERED 
(
	[ID] ASC,		
	[ITEM_ID] ASC
	
))

GO

ALTER TABLE [app].[ItemComments]  WITH CHECK ADD  CONSTRAINT [FK_ItemComments_Items] FOREIGN KEY([ITEM_ID])
REFERENCES [app].[Items] ([ID])
GO

ALTER TABLE [app].[ItemComments] CHECK CONSTRAINT [FK_ItemComments_Items]
GO

-----------------------------------------------------------
