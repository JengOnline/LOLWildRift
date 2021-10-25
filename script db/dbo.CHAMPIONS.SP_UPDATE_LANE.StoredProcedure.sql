USE [LOL_WILD_RIFT_DB]
GO
/****** Object:  StoredProcedure [dbo].[CHAMPIONS.SP_UPDATE_LANE]    Script Date: 19-Dec-20 1:12:50 PM ******/
DROP PROCEDURE [dbo].[CHAMPIONS.SP_UPDATE_LANE]
GO
/****** Object:  StoredProcedure [dbo].[CHAMPIONS.SP_UPDATE_LANE]    Script Date: 19-Dec-20 1:12:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Sophon.Ju
-- Create date: 19/12/2020
-- Description:	Update lane
-- =============================================
CREATE PROCEDURE [dbo].[CHAMPIONS.SP_UPDATE_LANE]
	@ID INT,
	@RECOMMENDED_LANE_ID INT
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @RESULT BIT
	BEGIN TRY
		IF EXISTS(SELECT TOP 1 [ID] FROM CHAMPIONS WHERE ID = @ID)
		BEGIN
			UPDATE CHAMPIONS SET 
			RECOMMENDED_LANE_ID = ISNULL(@RECOMMENDED_LANE_ID, RECOMMENDED_LANE_ID)
			WHERE ID = @ID

			SET @RESULT = 1
		END
		ELSE SET @RESULT = 0
	END TRY
	BEGIN CATCH
		SET @RESULT = 0
	END CATCH
	SELECT @RESULT AS RESULT
END
GO
