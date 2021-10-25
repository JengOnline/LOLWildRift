USE [LOL_WILD_RIFT_DB]
GO
/****** Object:  StoredProcedure [dbo].[CHAMPIONS.SP_ADD_OR_UPDATE]    Script Date: 19-Dec-20 1:12:50 PM ******/
DROP PROCEDURE [dbo].[CHAMPIONS.SP_ADD_OR_UPDATE]
GO
/****** Object:  StoredProcedure [dbo].[CHAMPIONS.SP_ADD_OR_UPDATE]    Script Date: 19-Dec-20 1:12:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Sophon.Ju
-- Create date: 14/12/2020
-- Description:	ADD OR UPDATE CHAMPIONS
-- =============================================
CREATE PROCEDURE [dbo].[CHAMPIONS.SP_ADD_OR_UPDATE]

	@NAME					AS VARCHAR(50) = NULL,
	@HISTORY				AS VARCHAR(MAX)= NULL,
	@STATS_DAMAGE			AS INT = NULL,
	@STATS_TOUGHNESS		AS INT = NULL,
	@STATS_UTILITY			AS INT = NULL,
	@STATS_DIFFICULITY		AS INT = NULL,
	@CLASS_ID				AS INT = NULL,
	@RECOMMENDED_LANE_ID	AS INT = NULL,
	@IMAGE_PATH				AS VARCHAR(200) = NULL
	AS
	SET NOCOUNT ON
	IF EXISTS(SELECT TOP 1 [ID] FROM CHAMPIONS WHERE NAME = @NAME)
		BEGIN TRY
			UPDATE CHAMPIONS SET 
			NAME	= @NAME,
			HISTORY	= ISNULL(@HISTORY,HISTORY),
			STATS_DAMAGE	= ISNULL(@STATS_DAMAGE,STATS_DAMAGE),
			STATS_TOUGHNESS = ISNULL(@STATS_TOUGHNESS, STATS_TOUGHNESS),
			STATS_UTILITY	= ISNULL(@STATS_UTILITY, STATS_UTILITY),
			STATS_DIFFICULITY	= ISNULL(@STATS_DIFFICULITY, STATS_DIFFICULITY),
			CLASS_ID = ISNULL(@CLASS_ID, CLASS_ID),
			RECOMMENDED_LANE_ID = ISNULL(@RECOMMENDED_LANE_ID , RECOMMENDED_LANE_ID),
			IMAGE_PATH = ISNULL(@IMAGE_PATH, IMAGE_PATH)
			WHERE NAME = @NAME 

			SELECT 1 AS RESULT
		END TRY
		BEGIN CATCH
			SELECT 0 AS RESULT
		END CATCH
	ELSE 
		BEGIN TRY
			INSERT INTO CHAMPIONS 
			([NAME], [HISTORY], [STATS_DAMAGE], [STATS_TOUGHNESS], [STATS_UTILITY], [STATS_DIFFICULITY], [CLASS_ID], [RECOMMENDED_LANE_ID], [IMAGE_PATH])
			VALUES
			(@NAME, @HISTORY, @STATS_DAMAGE, @STATS_TOUGHNESS ,@STATS_UTILITY, @STATS_DIFFICULITY, @CLASS_ID, @RECOMMENDED_LANE_ID, @IMAGE_PATH)
			SELECT 1 AS RESULT
		END TRY
		BEGIN CATCH
			SELECT 0 AS RESULT
		END CATCH


GO
