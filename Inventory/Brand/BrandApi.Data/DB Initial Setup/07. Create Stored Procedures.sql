-----------------------------------------------------------

-- Notes:


-----------------------------------------------------------

USE [StarryEdenBrand]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-----------------------------------------------------------

CREATE OR ALTER PROCEDURE [app].[spGetBrands]
	@search varchar(250)
AS

BEGIN TRY
	
	IF (@search IS NOT NULL AND LEN(LTRIM(RTRIM(@search))) > 0) SELECT [ID] as [BRAND_ID], [BRAND_NAME], [DESCRIPTION] as [BRAND_DESCRIPTION], [CREATED_BY] as [BRAND_CREATED_BY], [CREATED_DATE] as [BRAND_CREATED_DATE], [LAST_MODIFIED_BY] as [BRAND_LAST_MODIFIED_BY], [LAST_MODIFIED_DATE] as [BRAND_LAST_MODIFIED_DATE] FROM [app].[vwBrands] WHERE ID LIKE '%' + LTRIM(RTRIM(@search)) + '%' OR BRAND_NAME LIKE '%' + LTRIM(RTRIM(@search)) + '%' OR DESCRIPTION LIKE '%' + LTRIM(RTRIM(@search)) + '%'
	ELSE SELECT [ID] as [BRAND_ID], [BRAND_NAME], [DESCRIPTION] as [BRAND_DESCRIPTION], [CREATED_BY] as [BRAND_CREATED_BY], [CREATED_DATE] as [BRAND_CREATED_DATE], [LAST_MODIFIED_BY] as [BRAND_LAST_MODIFIED_BY], [LAST_MODIFIED_DATE] as [BRAND_LAST_MODIFIED_DATE] FROM [app].[vwBrands]

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

CREATE OR ALTER PROCEDURE [app].[spGetBrand]
	@id int
AS

BEGIN TRY
	
	SELECT [ID] as [BRAND_ID], [BRAND_NAME], [DESCRIPTION] as [BRAND_DESCRIPTION], [CREATED_BY] as [BRAND_CREATED_BY], [CREATED_DATE] as [BRAND_CREATED_DATE], [LAST_MODIFIED_BY] as [BRAND_LAST_MODIFIED_BY], [LAST_MODIFIED_DATE] as [BRAND_LAST_MODIFIED_DATE] 
	FROM [app].[vwBrands]
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

CREATE OR ALTER PROCEDURE [app].[spAddBrand]
	@brand_name varchar(50),
	@description varchar(250),
	@lastmodifiedby varchar(100)
AS

BEGIN TRY
	
	INSERT INTO [app].[Brands] ([BRAND_NAME],[DESCRIPTION],[LAST_MODIFIED_BY],[CREATED_BY]) 
	VALUES (@brand_name,@description,@lastmodifiedby,@lastmodifiedby);

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

CREATE OR ALTER PROCEDURE [app].[spUpdateBrand]
	@id int,
	@brand_name varchar(50),
	@description varchar(250),
	@lastmodifiedby varchar(100)
AS

BEGIN TRY
	
	UPDATE [app].[Brands] 
	SET [BRAND_NAME] = @brand_name, [DESCRIPTION] = @description, [LAST_MODIFIED_BY] = @lastmodifiedby, [LAST_MODIFIED_DATE] = GETDATE()
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

CREATE OR ALTER PROCEDURE [app].[spRemoveBrand]
	@id int,
	@lastmodifiedby varchar(100)
AS

BEGIN TRY

	UPDATE [app].[Brands] 
	SET LAST_MODIFIED_BY = @lastmodifiedby, LAST_MODIFIED_DATE = GETDATE()
	WHERE ID = @id

	DELETE FROM [app].[Brands] 
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
