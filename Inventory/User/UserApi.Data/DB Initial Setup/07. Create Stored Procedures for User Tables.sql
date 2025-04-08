-----------------------------------------------------------

-- Notes:


-----------------------------------------------------------

USE [StarryEdenUser]
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-----------------------------------------------------------

CREATE OR ALTER PROCEDURE [app].[spAddUser]
	@name varchar(250),
	@salt varchar(250),
	@hash varchar(250),
	@role int,
	@lastmodifiedby varchar(100)
AS

BEGIN TRY

	INSERT INTO [app].[Users] ([NAME], [PASS_SALT], [PASS_HASH], [ROLE_ID], [LAST_MODIFIED_BY], [CREATED_BY])
	VALUES (@name, @salt, @hash, @role, @lastmodifiedby, @lastmodifiedby);

    SELECT SCOPE_IDENTITY();

END TRY

BEGIN CATCH

	DECLARE @ErrorMessage nvarchar(4000);
	DECLARE @ErrorSeverity int;
	DECLARE @ErrorState int;

	SELECT @ErrorMessage = ERROR_MESSAGE(),
		   @ErrorSeverity = ERROR_SEVERITY(),
		   @ErrorState = ERROR_STATE();

	SELECT @ErrorMessage, @ErrorSeverity, @ErrorState;

	IF (@@TRANCOUNT > 0)
		ROLLBACK TRANSACTION;

END CATCH

GO

-----------------------------------------------------------

CREATE OR ALTER PROCEDURE [app].[spGetAuth]
	@id varchar(100)
AS

BEGIN TRY

	SELECT [PASS_SALT], [PASS_HASH], [ROLE_ID]
	FROM [app].[vwUsers] 
	WHERE [ID] = @id 

END TRY

BEGIN CATCH

	DECLARE @ErrorMessage nvarchar(4000);
	DECLARE @ErrorSeverity int;
	DECLARE @ErrorState int;

	SELECT @ErrorMessage = ERROR_MESSAGE(),
		   @ErrorSeverity = ERROR_SEVERITY(),
		   @ErrorState = ERROR_STATE();

	SELECT @ErrorMessage, @ErrorSeverity, @ErrorState;

	IF (@@TRANCOUNT > 0)
		ROLLBACK TRANSACTION;

END CATCH


GO

-----------------------------------------------------------

CREATE OR ALTER PROCEDURE [app].[spGetUsers]
AS

BEGIN TRY

	SELECT u.[ID], u.[NAME], u.[PASS_SALT], u.[PASS_HASH], u.[ROLE_ID], u.[CREATED_BY], u.[CREATED_DATE], u.[LAST_MODIFIED_BY], u.[LAST_MODIFIED_DATE]
	FROM [app].[vwUsers] u

END TRY

BEGIN CATCH

	DECLARE @ErrorMessage nvarchar(4000);
	DECLARE @ErrorSeverity int;
	DECLARE @ErrorState int;

	SELECT @ErrorMessage = ERROR_MESSAGE(),
		   @ErrorSeverity = ERROR_SEVERITY(),
		   @ErrorState = ERROR_STATE();

	SELECT @ErrorMessage, @ErrorSeverity, @ErrorState;

	IF (@@TRANCOUNT > 0)
		ROLLBACK TRANSACTION;

END CATCH


GO

-----------------------------------------------------------

CREATE OR ALTER PROCEDURE [app].[spGetUser]
	@id int
AS

BEGIN TRY

	SELECT TOP 1 u.[ID], u.[NAME], u.[PASS_SALT], u.[PASS_HASH], u.[ROLE_ID], u.[CREATED_BY], u.[CREATED_DATE], u.[LAST_MODIFIED_BY], u.[LAST_MODIFIED_DATE]
	FROM [app].[vwUsers] u
	WHERE u.ID = @id

END TRY

BEGIN CATCH

	DECLARE @ErrorMessage nvarchar(4000);
	DECLARE @ErrorSeverity int;
	DECLARE @ErrorState int;

	SELECT @ErrorMessage = ERROR_MESSAGE(),
		   @ErrorSeverity = ERROR_SEVERITY(),
		   @ErrorState = ERROR_STATE();

	SELECT @ErrorMessage, @ErrorSeverity, @ErrorState;

	IF (@@TRANCOUNT > 0)
		ROLLBACK TRANSACTION;

END CATCH


GO

-----------------------------------------------------------

CREATE OR ALTER PROCEDURE [app].[spUpdateUser]
	@id int,
	@name varchar(250),
	@salt varchar(250),
	@hash varchar(250),
	@role int,
	@lastmodifiedby varchar(100)
AS

BEGIN TRY

	UPDATE [app].[Users] 
	SET [NAME] = @name, [PASS_SALT] = @salt, [PASS_HASH] = @hash, [ROLE_ID] = @role, [LAST_MODIFIED_BY] = @lastmodifiedby, [LAST_MODIFIED_DATE] = GETDATE() 
	WHERE [ID] = @id

	SELECT @id

END TRY

BEGIN CATCH

	DECLARE @ErrorMessage nvarchar(4000);
	DECLARE @ErrorSeverity int;
	DECLARE @ErrorState int;

	SELECT @ErrorMessage = ERROR_MESSAGE(),
		   @ErrorSeverity = ERROR_SEVERITY(),
		   @ErrorState = ERROR_STATE();

	SELECT @ErrorMessage, @ErrorSeverity, @ErrorState;

	IF (@@TRANCOUNT > 0)
		ROLLBACK TRANSACTION;

END CATCH



GO

-----------------------------------------------------------

CREATE OR ALTER PROCEDURE [app].[spRemoveUser]
	@id int,
	@lastmodifiedby varchar(100)
AS

BEGIN TRY

	UPDATE [app].[Users]
	SET [LAST_MODIFIED_BY] = @lastmodifiedby, [LAST_MODIFIED_DATE] = GETDATE()
	WHERE [ID] = @id

	DELETE FROM [app].[Users]
	WHERE [ID] = @id

	SELECT @id

END TRY

BEGIN CATCH

	DECLARE @ErrorMessage nvarchar(4000);
	DECLARE @ErrorSeverity int;
	DECLARE @ErrorState int;

	SELECT @ErrorMessage = ERROR_MESSAGE(),
		   @ErrorSeverity = ERROR_SEVERITY(),
		   @ErrorState = ERROR_STATE();

	SELECT @ErrorMessage, @ErrorSeverity, @ErrorState;

	IF (@@TRANCOUNT > 0)
		ROLLBACK TRANSACTION;

END CATCH


GO

-----------------------------------------------------------

CREATE OR ALTER PROCEDURE [app].[spGetRole]
	@id int
AS

BEGIN TRY

	SELECT TOP 1 [ID] as [ROLE_ID], [DESCRIPTION] as [ROLE_DESCRIPTION]
	FROM [app].[vwRoles]
	WHERE ID = @id

END TRY

BEGIN CATCH

	DECLARE @ErrorMessage nvarchar(4000);
	DECLARE @ErrorSeverity int;
	DECLARE @ErrorState int;

	SELECT @ErrorMessage = ERROR_MESSAGE(),
		   @ErrorSeverity = ERROR_SEVERITY(),
		   @ErrorState = ERROR_STATE();

	SELECT @ErrorMessage, @ErrorSeverity, @ErrorState;

	IF (@@TRANCOUNT > 0)
		ROLLBACK TRANSACTION;

END CATCH


GO

-----------------------------------------------------------

CREATE OR ALTER PROCEDURE [app].[spGetRoles]
AS

BEGIN TRY

	SELECT [ID] as [ROLE_ID], [DESCRIPTION] as [ROLE_DESCRIPTION]
	FROM [app].[vwRoles]

END TRY

BEGIN CATCH

	DECLARE @ErrorMessage nvarchar(4000);
	DECLARE @ErrorSeverity int;
	DECLARE @ErrorState int;

	SELECT @ErrorMessage = ERROR_MESSAGE(),
		   @ErrorSeverity = ERROR_SEVERITY(),
		   @ErrorState = ERROR_STATE();

	SELECT @ErrorMessage, @ErrorSeverity, @ErrorState;

	IF (@@TRANCOUNT > 0)
		ROLLBACK TRANSACTION;

END CATCH


GO

-----------------------------------------------------------