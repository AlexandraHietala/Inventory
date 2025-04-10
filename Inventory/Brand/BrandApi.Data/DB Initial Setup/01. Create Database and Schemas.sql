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

DROP TABLE IF EXISTS [hist].[Audit_Brands]

DROP TABLE IF EXISTS [app].[Brands]

DROP VIEW IF EXISTS [app].[vwBrands]

DROP PROCEDURE IF EXISTS [app].[spAddBrand]
DROP PROCEDURE IF EXISTS [app].[spGetBrands]
DROP PROCEDURE IF EXISTS [app].[spGetBrandHistory]
DROP PROCEDURE IF EXISTS [app].[spRemoveBrand]
DROP PROCEDURE IF EXISTS [app].[spUpdateBrand]

DROP FUNCTION IF EXISTS [app].[fnGetBrandHistory]

IF EXISTS (SELECT * FROM sys.schemas WHERE name = N'app')
DROP SCHEMA [app]
GO

IF EXISTS (SELECT * FROM sys.schemas WHERE name = N'hist')
DROP SCHEMA [hist]
GO

ALTER DATABASE [StarryEdenBrand] SET single_user WITH rollback immediate
GO

DROP DATABASE IF EXISTS [StarryEdenBrand]
GO
*/

-----------------------------------------------------------

CREATE DATABASE [StarryEdenBrand]
GO

ALTER DATABASE [StarryEdenBrand] SET MULTI_USER
GO

-----------------------------------------------------------

USE [StarryEdenBrand]
GO

IF NOT EXISTS (SELECT * FROM sys.syslogins WHERE name = N'StarryEdenAppAdmin')
	CREATE LOGIN [StarryEdenAppAdmin] WITH PASSWORD=N'Tomorrow@9', DEFAULT_DATABASE=[StarryEdenBrand], DEFAULT_LANGUAGE=[us_english], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
GO

ALTER LOGIN [StarryEdenAppAdmin] ENABLE
GO 

IF NOT EXISTS (SELECT * FROM sys.sysusers WHERE name = N'StarryEdenAppAdmin')
	CREATE USER [StarryEdenAppAdmin] FOR LOGIN [StarryEdenAppAdmin];
GO

-----------------------------------------------------------

USE [StarryEdenBrand]
GO

CREATE SCHEMA hist AUTHORIZATION [StarryEdenAppAdmin]
GO

-----------------------------------------------------------

USE [StarryEdenBrand]
GO

CREATE SCHEMA app AUTHORIZATION [StarryEdenAppAdmin]
GO

-----------------------------------------------------------

USE [StarryEdenBrand]
GO

ALTER USER [StarryEdenAppAdmin]
WITH DEFAULT_SCHEMA = app;

GO

-----------------------------------------------------------

USE [StarryEdenBrand]

ALTER ROLE [db_owner] ADD MEMBER [StarryEdenAppAdmin];
GO

-----------------------------------------------------------