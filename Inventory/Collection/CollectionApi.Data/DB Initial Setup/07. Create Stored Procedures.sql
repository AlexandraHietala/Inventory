-----------------------------------------------------------

-- Notes:


-----------------------------------------------------------

USE [StarryEdenCollection]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-----------------------------------------------------------

CREATE OR ALTER PROCEDURE [app].[spGetCollections]
	@search varchar(250)
AS

BEGIN TRY
	
	IF (@search IS NOT NULL AND LEN(LTRIM(RTRIM(@search))) > 0) SELECT [ID] as [COLLECTION_ID], [COLLECTION_NAME], [DESCRIPTION] as [COLLECTION_DESCRIPTION], [CREATED_BY] as [COLLECTION_CREATED_BY], [CREATED_DATE] as [COLLECTION_CREATED_DATE], [LAST_MODIFIED_BY] as [COLLECTION_LAST_MODIFIED_BY], [LAST_MODIFIED_DATE] as [COLLECTION_LAST_MODIFIED_DATE] FROM [app].[vwCollections] WHERE ID LIKE '%' + LTRIM(RTRIM(@search)) + '%' OR COLLECTION_NAME LIKE '%' + LTRIM(RTRIM(@search)) + '%' OR DESCRIPTION LIKE '%' + LTRIM(RTRIM(@search)) + '%'
	ELSE SELECT [ID] as [COLLECTION_ID], [COLLECTION_NAME], [DESCRIPTION] as [COLLECTION_DESCRIPTION], [CREATED_BY] as [COLLECTION_CREATED_BY], [CREATED_DATE] as [COLLECTION_CREATED_DATE], [LAST_MODIFIED_BY] as [COLLECTION_LAST_MODIFIED_BY], [LAST_MODIFIED_DATE] as [COLLECTION_LAST_MODIFIED_DATE] FROM [app].[vwCollections]

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

CREATE OR ALTER PROCEDURE [app].[spGetCollection]
	@id int
AS

BEGIN TRY
	
	SELECT [ID] as [COLLECTION_ID], [COLLECTION_NAME], [DESCRIPTION] as [COLLECTION_DESCRIPTION], [CREATED_BY] as [COLLECTION_CREATED_BY], [CREATED_DATE] as [COLLECTION_CREATED_DATE], [LAST_MODIFIED_BY] as [COLLECTION_LAST_MODIFIED_BY], [LAST_MODIFIED_DATE] as [COLLECTION_LAST_MODIFIED_DATE] 
	FROM [app].[vwCollections]
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

CREATE OR ALTER PROCEDURE [app].[spAddCollection]
	@collection_name varchar(50),
	@description varchar(250),
	@lastmodifiedby varchar(100)
AS

BEGIN TRY
	
	INSERT INTO [app].[Collections] ([COLLECTION_NAME],[DESCRIPTION],[LAST_MODIFIED_BY],[CREATED_BY]) 
	VALUES (@collection_name,@description,@lastmodifiedby,@lastmodifiedby);

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

CREATE OR ALTER PROCEDURE [app].[spUpdateCollection]
	@id int,
	@collection_name varchar(50),
	@description varchar(250),
	@lastmodifiedby varchar(100)
AS

BEGIN TRY
	
	UPDATE [app].[Collections] 
	SET [COLLECTION_NAME] = @collection_name, [DESCRIPTION] = @description, [LAST_MODIFIED_BY] = @lastmodifiedby, [LAST_MODIFIED_DATE] = GETDATE()
	WHERE ID = @id

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

CREATE OR ALTER PROCEDURE [app].[spRemoveCollection]
	@id int,
	@lastmodifiedby varchar(100)
AS

BEGIN TRY

	UPDATE [app].[Collections] 
	SET LAST_MODIFIED_BY = @lastmodifiedby, LAST_MODIFIED_DATE = GETDATE()
	WHERE ID = @id

	DELETE FROM [app].[Collections] 
	WHERE ID = @id

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
