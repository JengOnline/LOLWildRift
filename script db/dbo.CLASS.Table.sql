USE [LOL_WILD_RIFT_DB]
GO
/****** Object:  Table [dbo].[CLASS]    Script Date: 19-Dec-20 1:12:50 PM ******/
DROP TABLE [dbo].[CLASS]
GO
/****** Object:  Table [dbo].[CLASS]    Script Date: 19-Dec-20 1:12:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CLASS](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CLASS_NAME] [varchar](50) NULL
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[CLASS] ON 

INSERT [dbo].[CLASS] ([ID], [CLASS_NAME]) VALUES (1, N'Fighter')
INSERT [dbo].[CLASS] ([ID], [CLASS_NAME]) VALUES (2, N'MAGE')
INSERT [dbo].[CLASS] ([ID], [CLASS_NAME]) VALUES (3, N'MARKSMAN')
INSERT [dbo].[CLASS] ([ID], [CLASS_NAME]) VALUES (4, N'ASSASSIN')
INSERT [dbo].[CLASS] ([ID], [CLASS_NAME]) VALUES (5, N'TANK')
INSERT [dbo].[CLASS] ([ID], [CLASS_NAME]) VALUES (6, N'SUPPORT')
SET IDENTITY_INSERT [dbo].[CLASS] OFF