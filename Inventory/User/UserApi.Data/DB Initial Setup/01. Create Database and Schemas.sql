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

DROP TABLE IF EXISTS [hist].[Audit_Users]

DROP TABLE IF EXISTS [app].[Users]
DROP TABLE IF EXISTS [app].[Roles]

DROP VIEW IF EXISTS [app].[vwRoles]
DROP VIEW IF EXISTS [app].[vwUsers]

DROP PROCEDURE IF EXISTS [app].[spAddUser]
DROP PROCEDURE IF EXISTS [app].[spGetAuth]
DROP PROCEDURE IF EXISTS [app].[spGetRole]
DROP PROCEDURE IF EXISTS [app].[spGetRoles]
DROP PROCEDURE IF EXISTS [app].[spGetUser]
DROP PROCEDURE IF EXISTS [app].[spGetUsers]
DROP PROCEDURE IF EXISTS [app].[spRemoveUser]
DROP PROCEDURE IF EXISTS [app].[spUpdateUser]
DROP PROCEDURE IF EXISTS [app].[spGetUserHistory]

DROP FUNCTION IF EXISTS [app].[fnGetUserHistory]

IF EXISTS (SELECT * FROM sys.schemas WHERE name = N'app')
DROP SCHEMA [app]
GO

IF EXISTS (SELECT * FROM sys.schemas WHERE name = N'hist')
DROP SCHEMA [hist]
GO

ALTER DATABASE [StarryEdenUser] SET single_user WITH rollback immediate
GO

DROP DATABASE IF EXISTS [StarryEdenUser]
GO
*/

-----------------------------------------------------------

CREATE DATABASE [StarryEdenUser]
GO

ALTER DATABASE [StarryEdenUser] SET MULTI_USER
GO

-----------------------------------------------------------

USE [StarryEdenUser]
GO

IF NOT EXISTS (SELECT * FROM sys.syslogins WHERE name = N'StarryEdenAppAdmin')
	CREATE LOGIN [StarryEdenAppAdmin] WITH PASSWORD=N'Tomorrow@9', DEFAULT_DATABASE=[StarryEdenUser], DEFAULT_LANGUAGE=[us_english], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
GO

ALTER LOGIN [StarryEdenAppAdmin] ENABLE
GO 

IF NOT EXISTS (SELECT * FROM sys.sysusers WHERE name = N'StarryEdenAppAdmin')
	CREATE USER [StarryEdenAppAdmin] FOR LOGIN [StarryEdenAppAdmin];
GO

-----------------------------------------------------------

USE [StarryEdenUser]
GO

CREATE SCHEMA hist AUTHORIZATION [StarryEdenAppAdmin]
GO

-----------------------------------------------------------

USE [StarryEdenUser]
GO

CREATE SCHEMA app AUTHORIZATION [StarryEdenAppAdmin]
GO

-----------------------------------------------------------

USE [StarryEdenUser]
GO

ALTER USER [StarryEdenAppAdmin]
WITH DEFAULT_SCHEMA = app;

GO

-----------------------------------------------------------

USE [StarryEdenUser]

ALTER ROLE [db_owner] ADD MEMBER [StarryEdenAppAdmin];
GO

-----------------------------------------------------------