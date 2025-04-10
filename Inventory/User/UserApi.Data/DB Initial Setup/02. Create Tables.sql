-----------------------------------------------------------

-- Notes:


-----------------------------------------------------------

USE [StarryEdenUser]
GO

-----------------------------------------------------------

CREATE TABLE [app].[Roles](
	[ID] [int] PRIMARY KEY NOT NULL,
	[DESCRIPTION] [varchar](250) NULL,
)

GO

-----------------------------------------------------------

CREATE TABLE [app].[Users](
	[ID] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[NAME] [varchar](250) NOT NULL,
	[PASS_SALT] [varchar](250) NOT NULL,
	[PASS_HASH] [varchar](250) NOT NULL,
	[ROLE_ID] [int] NULL,
	[CREATED_BY] [varchar](100) NOT NULL DEFAULT 'SYSTEM',
	[CREATED_DATE] [datetime] NOT NULL DEFAULT GetDate(),
	[LAST_MODIFIED_BY] [varchar](100) NULL DEFAULT 'SYSTEM',
	[LAST_MODIFIED_DATE] [datetime] NULL DEFAULT GetDate(),
)

GO

ALTER TABLE [app].[Users] WITH CHECK ADD CONSTRAINT [FK_Users_Roles] FOREIGN KEY([ROLE_ID]) REFERENCES [app].[Roles] ([ID])
GO

ALTER TABLE [app].[Users] CHECK CONSTRAINT [FK_Users_Roles] 
GO

-----------------------------------------------------------