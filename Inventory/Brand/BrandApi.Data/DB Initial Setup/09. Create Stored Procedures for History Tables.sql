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

CREATE OR ALTER PROCEDURE [app].[spGetBrandHistory]
	@id int
AS

BEGIN TRY

	DECLARE @sqlfinal nvarchar(1000);

	SET @sqlfinal = 'SELECT DATEOFCHANGE, CHANGEDBY, CHANGED, CHANGE FROM app.fnGetBrandHistory (' + CAST(@id AS NVARCHAR(500)) + ') ORDER BY DATEOFCHANGE DESC';

	EXECUTE sp_executesql @sqlfinal;

END TRY

BEGIN CATCH

	DECLARE @ErrorMessage nvarchar(500);
	DECLARE @ErrorSeverity int;
	DECLARE @ErrorState int;

	SELECT @ErrorMessage  = ERROR_MESSAGE(),
		   @ErrorSeverity  = ERROR_SEVERITY(),
		   @ErrorState  = ERROR_STATE();

	SELECT @ErrorMessage, @ErrorSeverity, @ErrorState;

	IF (@@TRANCOUNT > 0)
		ROLLBACK TRANSACTION;

END CATCH


GO

-----------------------------------------------------------
