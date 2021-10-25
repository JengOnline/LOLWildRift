USE [LOL_WILD_RIFT_DB]
GO
/****** Object:  StoredProcedure [dbo].[CHAMPIONS.SP_UPDATE_IMAGE_PATH]    Script Date: 19-Dec-20 1:12:50 PM ******/
DROP PROCEDURE [dbo].[CHAMPIONS.SP_UPDATE_IMAGE_PATH]
GO
/****** Object:  StoredProcedure [dbo].[CHAMPIONS.SP_UPDATE_IMAGE_PATH]    Script Date: 19-Dec-20 1:12:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Sophon.Ju
-- Create date: 17/12/2020
-- Description:	update image path.
-- =============================================
CREATE PROCEDURE [dbo].[CHAMPIONS.SP_UPDATE_IMAGE_PATH]
	@ID		INT,
	@IMAGE_PATH	VARCHAR(200)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @RETURN BIT
	BEGIN TRY
		IF EXISTS(SELECT TOP 1 [ID] FROM CHAMPIONS WHERE ID = @ID)
		BEGIN
		 UPDATE CHAMPIONS SET IMAGE_PATH = @IMAGE_PATH WHERE ID = @ID
		 SET @RETURN = 1 
		END
		
		ELSE SET @RETURN = 0
		
	END TRY
	BEGIN CATCH
		SET @RETURN = 0
	END CATCH
    
	SELECT @RETURN AS RESULT
END
GO
