USE [LOL_WILD_RIFT_DB]
GO
/****** Object:  StoredProcedure [dbo].[CHAMPIONS.SP_GET]    Script Date: 19-Dec-20 1:12:50 PM ******/
DROP PROCEDURE [dbo].[CHAMPIONS.SP_GET]
GO
/****** Object:  StoredProcedure [dbo].[CHAMPIONS.SP_GET]    Script Date: 19-Dec-20 1:12:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Sophon.Ju
-- Create date: 19/12/2020
-- Description:	Get champions
-- =============================================
CREATE PROCEDURE [dbo].[CHAMPIONS.SP_GET]
	@ID INT
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRY
		SELECT  
		    CHAMP.ID
			[NAME], 
			[HISTORY], 
			[STATS_DAMAGE], 
			[STATS_TOUGHNESS],
			[STATS_UTILITY],
			[STATS_DIFFICULITY],
			C.CLASS_NAME AS CLASS,
			R.LANE AS LANE,
			[IMAGE_PATH]
		FROM [CHAMPIONS] AS CHAMP
		JOIN [CLASS] AS C ON (C.ID = CHAMP.CLASS_ID)
		JOIN [RECOMMENDED_LANE] AS R ON (R.ID = CHAMP.RECOMMENDED_LANE_ID)
		WHERE CHAMP.ID = @ID
	END TRY
	BEGIN CATCH
	 SELECT 'ERROR'
	END CATCH
END
GO
