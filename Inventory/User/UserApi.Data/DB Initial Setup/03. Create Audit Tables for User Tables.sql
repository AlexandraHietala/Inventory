-----------------------------------------------------------

-- Notes:


-----------------------------------------------------------

USE [StarryEdenUser]
GO

-----------------------------------------------------------

CREATE TABLE [hist].[Audit_Users](
	[EFFECTIVE_DATE] [datetime] NOT NULL DEFAULT GetDate(),
	[INEFFECTIVE_DATE] [datetime] NULL,
	[ID] int NOT NULL,
	[NAME] [varchar](250) NOT NULL,
	[PASS_SALT] [varchar](250) NOT NULL,
	[PASS_HASH] [varchar](250) NOT NULL,
	[ROLE_ID] [int] NULL,
	[ADDED] [bit] NOT NULL DEFAULT 0,
	[DELETED] [bit] NOT NULL DEFAULT 0,
	[CREATED_BY] [varchar](100) NULL,
	[CREATED_DATE] [datetime] NULL,
	[LAST_MODIFIED_BY] [varchar](100) NULL,
	[LAST_MODIFIED_DATE] [datetime] NULL,
 CONSTRAINT [PK_Audit_Users] PRIMARY KEY CLUSTERED 
(
	[EFFECTIVE_DATE] ASC,
	[ID] ASC
))

GO

-----------------------------------------------------------
