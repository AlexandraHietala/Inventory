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

DROP TABLE IF EXISTS [hist].[Audit_Series]

DROP TABLE IF EXISTS [app].[Series]

DROP VIEW IF EXISTS [app].[vwSeries]

DROP PROCEDURE IF EXISTS [app].[spAddSeries]
DROP PROCEDURE IF EXISTS [app].[spGetSeriesHistory]
DROP PROCEDURE IF EXISTS [app].[spGetSeries]
DROP PROCEDURE IF EXISTS [app].[spRemoveSeries]
DROP PROCEDURE IF EXISTS [app].[spUpdateSeries]

DROP FUNCTION IF EXISTS [app].[fnGetSeriesHistory]

IF EXISTS (SELECT * FROM sys.schemas WHERE name = N'app')
DROP SCHEMA [app]
GO

IF EXISTS (SELECT * FROM sys.schemas WHERE name = N'hist')
DROP SCHEMA [hist]
GO

ALTER DATABASE [StarryEdenSeries] SET single_user WITH rollback immediate
GO

DROP DATABASE IF EXISTS [StarryEdenSeries]
GO
*/

-----------------------------------------------------------

CREATE DATABASE [StarryEdenSeries]
GO

ALTER DATABASE [StarryEdenSeries] SET MULTI_USER
GO

-----------------------------------------------------------

USE [StarryEdenSeries]
GO

IF NOT EXISTS (SELECT * FROM sys.syslogins WHERE name = N'StarryEdenAppAdmin')
	CREATE LOGIN [StarryEdenAppAdmin] WITH PASSWORD=N'Tomorrow@9', DEFAULT_DATABASE=[StarryEdenSeries], DEFAULT_LANGUAGE=[us_english], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
GO

ALTER LOGIN [StarryEdenAppAdmin] ENABLE
GO 

IF NOT EXISTS (SELECT * FROM sys.sysusers WHERE name = N'StarryEdenAppAdmin')
	CREATE USER [StarryEdenAppAdmin] FOR LOGIN [StarryEdenAppAdmin];
GO

-----------------------------------------------------------

USE [StarryEdenSeries]
GO

CREATE SCHEMA hist AUTHORIZATION [StarryEdenAppAdmin]
GO

-----------------------------------------------------------

USE [StarryEdenSeries]
GO

CREATE SCHEMA app AUTHORIZATION [StarryEdenAppAdmin]
GO

-----------------------------------------------------------

USE [StarryEdenSeries]
GO

ALTER USER [StarryEdenAppAdmin]
WITH DEFAULT_SCHEMA = app;

GO

-----------------------------------------------------------

USE [StarryEdenSeries]

ALTER ROLE [db_owner] ADD MEMBER [StarryEdenAppAdmin];
GO

-----------------------------------------------------------