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
DROP TABLE IF EXISTS [hist].[Audit_Series]
DROP TABLE IF EXISTS [hist].[Audit_Brands]

DROP TABLE IF EXISTS [app].[ItemComments]
DROP TABLE IF EXISTS [app].[Items]
DROP TABLE IF EXISTS [app].[Series]
DROP TABLE IF EXISTS [app].[Brands]

DROP VIEW IF EXISTS [app].[vwBrands]
DROP VIEW IF EXISTS [app].[vwItemComments]
DROP VIEW IF EXISTS [app].[vwItems]
DROP VIEW IF EXISTS [app].[vwSeries]
DROP VIEW IF EXISTS [app].[vwItems_Expanded]
DROP VIEW IF EXISTS [app].[vwItems_Search]

DROP PROCEDURE IF EXISTS [app].[spAddBrand]
DROP PROCEDURE IF EXISTS [app].[spAddItemComment]
DROP PROCEDURE IF EXISTS [app].[spAddItem]
DROP PROCEDURE IF EXISTS [app].[spAddSeries]
DROP PROCEDURE IF EXISTS [app].[spGetBrands]
DROP PROCEDURE IF EXISTS [app].[spGetItemComments]
DROP PROCEDURE IF EXISTS [app].[spGetItem]
DROP PROCEDURE IF EXISTS [app].[spGetItems]
DROP PROCEDURE IF EXISTS [app].[spSearchItems]
DROP PROCEDURE IF EXISTS [app].[spGetGeneralHistory]
DROP PROCEDURE IF EXISTS [app].[spGetHistory]
DROP PROCEDURE IF EXISTS [app].[spGetSeries]
DROP PROCEDURE IF EXISTS [app].[spRemoveBrand]
DROP PROCEDURE IF EXISTS [app].[spRemoveItemComment]
DROP PROCEDURE IF EXISTS [app].[spRemoveItem]
DROP PROCEDURE IF EXISTS [app].[spRemoveSeries]
DROP PROCEDURE IF EXISTS [app].[spUpdateBrand]
DROP PROCEDURE IF EXISTS [app].[spUpdateItem]
DROP PROCEDURE IF EXISTS [app].[spUpdateSeries]

DROP FUNCTION IF EXISTS [app].[fnGetBrandHistory]
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

ALTER DATABASE [StarryEdenItem] SET single_user WITH rollback immediate
GO

DROP DATABASE IF EXISTS [StarryEdenItem]
GO
*/

-----------------------------------------------------------

CREATE DATABASE [StarryEdenItem]
GO

ALTER DATABASE [StarryEdenItem] SET MULTI_USER
GO

-----------------------------------------------------------

USE [StarryEdenItem]
GO

IF NOT EXISTS (SELECT * FROM sys.syslogins WHERE name = N'StarryEdenAppAdmin')
	CREATE LOGIN [StarryEdenAppAdmin] WITH PASSWORD=N'Tomorrow@9', DEFAULT_DATABASE=[StarryEdenItem], DEFAULT_LANGUAGE=[us_english], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
GO

ALTER LOGIN [StarryEdenAppAdmin] ENABLE
GO 

IF NOT EXISTS (SELECT * FROM sys.sysusers WHERE name = N'StarryEdenAppAdmin')
	CREATE USER [StarryEdenAppAdmin] FOR LOGIN [StarryEdenAppAdmin];
GO

-----------------------------------------------------------

USE [StarryEdenItem]
GO

CREATE SCHEMA hist AUTHORIZATION [StarryEdenAppAdmin]
GO

-----------------------------------------------------------

USE [StarryEdenItem]
GO

CREATE SCHEMA app AUTHORIZATION [StarryEdenAppAdmin]
GO

-----------------------------------------------------------

USE [StarryEdenItem]
GO

ALTER USER [StarryEdenAppAdmin]
WITH DEFAULT_SCHEMA = app;

GO

-----------------------------------------------------------

USE [StarryEdenItem]

ALTER ROLE [db_owner] ADD MEMBER [StarryEdenAppAdmin];
GO

-----------------------------------------------------------