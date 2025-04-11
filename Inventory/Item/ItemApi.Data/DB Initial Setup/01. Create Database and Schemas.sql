-----------------------------------------------------------

-- Notes:
-- When the server is setup be sure to enable SQL Authentication, which can be found under
--			(Right-Click Server) > Properties > Security > Server Authentication > SQL Server and Windows Authentication Mode
-- Run this setup under local db admin account

-----------------------------------------------------------

-- Drop Existing, If Needed

/*
USE [Master]
GO

DROP TABLE IF EXISTS [hist].[Audit_ItemComments]
DROP TABLE IF EXISTS [hist].[Audit_Items]

DROP TABLE IF EXISTS [app].[ItemComments]
DROP TABLE IF EXISTS [app].[Items]

DROP VIEW IF EXISTS [app].[vwItemComments]
DROP VIEW IF EXISTS [app].[vwItems]
DROP VIEW IF EXISTS [app].[vwItems_Expanded]
DROP VIEW IF EXISTS [app].[vwItems_Search]

DROP PROCEDURE IF EXISTS [app].[spAddItemComment]
DROP PROCEDURE IF EXISTS [app].[spAddItem]
DROP PROCEDURE IF EXISTS [app].[spGetItemComments]
DROP PROCEDURE IF EXISTS [app].[spGetItem]
DROP PROCEDURE IF EXISTS [app].[spGetItems]
DROP PROCEDURE IF EXISTS [app].[spSearchItems]
DROP PROCEDURE IF EXISTS [app].[spGetGeneralHistory]
DROP PROCEDURE IF EXISTS [app].[spGetHistory]
DROP PROCEDURE IF EXISTS [app].[spRemoveItemComment]
DROP PROCEDURE IF EXISTS [app].[spRemoveItem]
DROP PROCEDURE IF EXISTS [app].[spUpdateItem]

DROP FUNCTION IF EXISTS [app].[fnGetItemCommentHistory]
DROP FUNCTION IF EXISTS [app].[fnGetItemHistory]
DROP FUNCTION IF EXISTS [app].[fnSplit_KeyValuePairs]
DROP FUNCTION IF EXISTS [app].[fnSplit_OnPipe]

IF EXISTS (SELECT * FROM sys.schemas WHERE name = N'app')
DROP SCHEMA [app]
GO

IF EXISTS (SELECT * FROM sys.schemas WHERE name = N'hist')
DROP SCHEMA [hist]
GO

ALTER DATABASE [SEInventory] SET single_user WITH rollback immediate
GO

DROP DATABASE IF EXISTS [SEInventory]
GO
*/

-----------------------------------------------------------

CREATE DATABASE [SEInventory]
GO

ALTER DATABASE [SEInventory] SET MULTI_USER
GO

-----------------------------------------------------------

USE [SEInventory]
GO

IF NOT EXISTS (SELECT * FROM sys.syslogins WHERE name = N'SEAppAdmin')
	CREATE LOGIN [SEAppAdmin] WITH PASSWORD=N'Tomorrow@9', DEFAULT_DATABASE=[SEInventory], DEFAULT_LANGUAGE=[us_english], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
GO

ALTER LOGIN [SEAppAdmin] ENABLE
GO 

IF NOT EXISTS (SELECT * FROM sys.sysusers WHERE name = N'SEAppAdmin')
	CREATE USER [SEAppAdmin] FOR LOGIN [SEAppAdmin];
GO

-----------------------------------------------------------

USE [SEInventory]
GO

CREATE SCHEMA hist AUTHORIZATION [SEAppAdmin]
GO

-----------------------------------------------------------

USE [SEInventory]
GO

CREATE SCHEMA app AUTHORIZATION [SEAppAdmin]
GO

-----------------------------------------------------------

USE [SEInventory]
GO

ALTER USER [SEAppAdmin]
WITH DEFAULT_SCHEMA = app;

GO

-----------------------------------------------------------

USE [SEInventory]

ALTER ROLE [db_owner] ADD MEMBER [SEAppAdmin];
GO

-----------------------------------------------------------