-----------------------------------------------------------

-- Notes:


-----------------------------------------------------------

USE [StarryEdenBrand]
GO

-----------------------------------------------------------

CREATE TABLE [hist].[Audit_Brands](
	[EFFECTIVE_DATE] [datetime] NOT NULL DEFAULT GetDate(),
	[INEFFECTIVE_DATE] [datetime] NULL,
	[ID] [int] NOT NULL,
	[BRAND_NAME] [varchar](50) NOT NULL,
	[DESCRIPTION] [varchar](250) NULL,
	[ADDED] [bit] NOT NULL DEFAULT 0,
	[DELETED] [bit] NOT NULL DEFAULT 0,
	[CREATED_BY] [varchar](100) NULL,
	[CREATED_DATE] [datetime] NULL,
	[LAST_MODIFIED_BY] [varchar](100) NULL,
	[LAST_MODIFIED_DATE] [datetime] NULL,
 CONSTRAINT [PK_Audit_Brands] PRIMARY KEY CLUSTERED 
(
	[EFFECTIVE_DATE] ASC,
	[ID] ASC
))

GO

-----------------------------------------------------------

