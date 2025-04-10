-----------------------------------------------------------

-- Notes:


-----------------------------------------------------------

USE [StarryEdenSeries]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-----------------------------------------------------------

CREATE OR ALTER PROCEDURE [app].[spGetSeries]
	@search varchar(250)
AS

BEGIN TRY
	
	IF (@search IS NOT NULL AND LEN(LTRIM(RTRIM(@search))) > 0) SELECT [ID] as [SERIES_ID], [SERIES_NAME], [DESCRIPTION] as [SERIES_DESCRIPTION], [CREATED_BY] as [SERIES_CREATED_BY], [CREATED_DATE] as [SERIES_CREATED_DATE], [LAST_MODIFIED_BY] as [SERIES_LAST_MODIFIED_BY], [LAST_MODIFIED_DATE] as [SERIES_LAST_MODIFIED_DATE] FROM [app].[vwSeries] WHERE ID LIKE '%' + LTRIM(RTRIM(@search)) + '%' OR SERIES_NAME LIKE '%' + LTRIM(RTRIM(@search)) + '%' OR DESCRIPTION LIKE '%' + LTRIM(RTRIM(@search)) + '%'
	ELSE SELECT [ID] as [SERIES_ID], [SERIES_NAME], [DESCRIPTION] as [SERIES_DESCRIPTION], [CREATED_BY] as [SERIES_CREATED_BY], [CREATED_DATE] as [SERIES_CREATED_DATE], [LAST_MODIFIED_BY] as [SERIES_LAST_MODIFIED_BY], [LAST_MODIFIED_DATE] as [SERIES_LAST_MODIFIED_DATE] FROM [app].[vwSeries]

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

CREATE OR ALTER PROCEDURE [app].[spGetASeries]
	@id int
AS

BEGIN TRY
	
	SELECT [ID] as [SERIES_ID], [SERIES_NAME], [DESCRIPTION] as [SERIES_DESCRIPTION], [CREATED_BY] as [SERIES_CREATED_BY], [CREATED_DATE] as [SERIES_CREATED_DATE], [LAST_MODIFIED_BY] as [SERIES_LAST_MODIFIED_BY], [LAST_MODIFIED_DATE] as [SERIES_LAST_MODIFIED_DATE] 
	FROM [app].[vwSeries] WHERE ID = @id

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

CREATE OR ALTER PROCEDURE [app].[spAddSeries]
	@series_name varchar(250),
	@description varchar(250),
	@lastmodifiedby varchar(100)
AS

BEGIN TRY
	
	INSERT INTO [app].[Series] ([SERIES_NAME],[DESCRIPTION],[LAST_MODIFIED_BY],[CREATED_BY]) 
	VALUES (@series_name,@description,@lastmodifiedby,@lastmodifiedby);

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

CREATE OR ALTER PROCEDURE [app].[spUpdateSeries]
	@id int,
	@series_name varchar(250),
	@description varchar(250),
	@lastmodifiedby varchar(100)
AS

BEGIN TRY
	
	UPDATE [app].[Series] 
	SET [SERIES_NAME] = @series_name, [DESCRIPTION] = @description, [LAST_MODIFIED_BY] = @lastmodifiedby, [LAST_MODIFIED_DATE] = GETDATE()
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

CREATE OR ALTER PROCEDURE [app].[spRemoveSeries]
	@id int,
	@lastmodifiedby varchar(100)
AS

BEGIN TRY

	UPDATE [app].[Series] 
	SET LAST_MODIFIED_BY = @lastmodifiedby, LAST_MODIFIED_DATE = GETDATE()
	WHERE ID = @id

	DELETE FROM [app].[Series] 
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
