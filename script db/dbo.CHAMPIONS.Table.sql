USE [LOL_WILD_RIFT_DB]
GO
/****** Object:  Table [dbo].[CHAMPIONS]    Script Date: 19-Dec-20 1:12:50 PM ******/
DROP TABLE [dbo].[CHAMPIONS]
GO
/****** Object:  Table [dbo].[CHAMPIONS]    Script Date: 19-Dec-20 1:12:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CHAMPIONS](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[NAME] [varchar](50) NULL,
	[HISTORY] [varchar](max) NULL,
	[STATS_DAMAGE] [int] NULL,
	[STATS_TOUGHNESS] [int] NULL,
	[STATS_UTILITY] [int] NULL,
	[STATS_DIFFICULITY] [int] NULL,
	[CLASS_ID] [int] NULL,
	[RECOMMENDED_LANE_ID] [int] NULL,
	[IMAGE_PATH] [varchar](200) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[CHAMPIONS] ON 

INSERT [dbo].[CHAMPIONS] ([ID], [NAME], [HISTORY], [STATS_DAMAGE], [STATS_TOUGHNESS], [STATS_UTILITY], [STATS_DIFFICULITY], [CLASS_ID], [RECOMMENDED_LANE_ID], [IMAGE_PATH]) VALUES (1, N'DARIUS', N'Darius hesitates once his axe is raised, those who stand against him can expect no mercy. Darius is a Baron lane bruiser who excels in bullying his lane and snowballing through his incredible damage output and infinite execution potential.', 100, 66, 33, 66, 2, 1, N'C:\Users\Jeng\Downloads\DARIUS.JPG')
INSERT [dbo].[CHAMPIONS] ([ID], [NAME], [HISTORY], [STATS_DAMAGE], [STATS_TOUGHNESS], [STATS_UTILITY], [STATS_DIFFICULITY], [CLASS_ID], [RECOMMENDED_LANE_ID], [IMAGE_PATH]) VALUES (2, N'AKALI', N'Akali strikes in silence, with a message loud and clear: fear the assassin with no master. Akali is a mobile burst damage assassin who is great at both killing a champion fast and at doing extended 1v1 trades. She has a lot of mobility abilities which makes her great at killing enemies one by one by going in and out of sight.', 100, 33, 33, 1, 4, 2, NULL)
SET IDENTITY_INSERT [dbo].[CHAMPIONS] OFF
