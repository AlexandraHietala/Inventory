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

DROP TABLE IF EXISTS [hist].[Audit_Collections]

DROP TABLE IF EXISTS [app].[Collections]

DROP VIEW IF EXISTS [app].[vwCollections]

DROP PROCEDURE IF EXISTS [app].[spAddCollection]
DROP PROCEDURE IF EXISTS [app].[spGetCollections]
DROP PROCEDURE IF EXISTS [app].[spRemoveCollection]
DROP PROCEDURE IF EXISTS [app].[spUpdateCollection]
DROP PROCEDURE IF EXISTS [app].[spGetCollectionHistory]

DROP FUNCTION IF EXISTS [app].[fnGetCollectionHistory]


IF EXISTS (SELECT * FROM sys.schemas WHERE name = N'app')
DROP SCHEMA [app]
GO

IF EXISTS (SELECT * FROM sys.schemas WHERE name = N'hist')
DROP SCHEMA [hist]
GO

ALTER DATABASE [StarryEdenCollection] SET single_user WITH rollback immediate
GO

DROP DATABASE IF EXISTS [StarryEdenCollection]
GO
*/

-----------------------------------------------------------

CREATE DATABASE [StarryEdenCollection]
GO

ALTER DATABASE [StarryEdenCollection] SET MULTI_USER
GO

-----------------------------------------------------------

USE [StarryEdenCollection]
GO

IF NOT EXISTS (SELECT * FROM sys.syslogins WHERE name = N'StarryEdenAppAdmin')
	CREATE LOGIN [StarryEdenAppAdmin] WITH PASSWORD=N'Tomorrow@9', DEFAULT_DATABASE=[StarryEdenCollection], DEFAULT_LANGUAGE=[us_english], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
GO

ALTER LOGIN [StarryEdenAppAdmin] ENABLE
GO 

IF NOT EXISTS (SELECT * FROM sys.sysusers WHERE name = N'StarryEdenAppAdmin')
	CREATE USER [StarryEdenAppAdmin] FOR LOGIN [StarryEdenAppAdmin];
GO

-----------------------------------------------------------

USE [StarryEdenCollection]
GO

CREATE SCHEMA hist AUTHORIZATION [StarryEdenAppAdmin]
GO

-----------------------------------------------------------

USE [StarryEdenCollection]
GO

CREATE SCHEMA app AUTHORIZATION [StarryEdenAppAdmin]
GO

-----------------------------------------------------------

USE [StarryEdenCollection]
GO

ALTER USER [StarryEdenAppAdmin]
WITH DEFAULT_SCHEMA = app;

GO

-----------------------------------------------------------

USE [StarryEdenCollection]

ALTER ROLE [db_owner] ADD MEMBER [StarryEdenAppAdmin];
GO

-----------------------------------------------------------