USE [WaterInfra_5]
GO
/****** Object:  Table [dbo].[tbInfraValue]    Script Date: 08.04.2021 15:06:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbInfraValue](
	[ValueId] [int] NOT NULL,
	[ObjId] [int] NOT NULL,
	[FieldId] [int] NOT NULL,
	[IntValue] [int] NULL,
	[FloatValue] [float] NULL,
	[StringValue] [nvarchar](max) NULL,
	[BooleanValue] [bit] NULL,
	[DateTimeValue] [datetime] NULL,
 CONSTRAINT [PK_tbInfraObjFielddValue] PRIMARY KEY CLUSTERED 
(
	[ValueId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbInfraField]    Script Date: 08.04.2021 15:06:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbInfraField](
	[FieldId] [int] NOT NULL,
	[CategoryId] [int] NOT NULL,
	[DataTypeId] [int] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Label] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](4000) NOT NULL,
 CONSTRAINT [PK_tbInfraObjFieldType] PRIMARY KEY CLUSTERED 
(
	[FieldId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbInfraCategory]    Script Date: 08.04.2021 15:06:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbInfraCategory](
	[CategoryId] [int] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_tbInfraCategory] PRIMARY KEY CLUSTERED 
(
	[CategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[vwObjectFields]    Script Date: 08.04.2021 15:06:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vwObjectFields]
AS
SELECT        TOP (100) PERCENT dbo.tbInfraValue.ObjId, dbo.tbInfraCategory.Name AS Category, dbo.tbInfraField.Name AS Field, dbo.tbInfraValue.IntValue, dbo.tbInfraValue.FloatValue, dbo.tbInfraValue.StringValue, 
                         dbo.tbInfraValue.BooleanValue, dbo.tbInfraValue.DateTimeValue
FROM            dbo.tbInfraCategory INNER JOIN
                         dbo.tbInfraField ON dbo.tbInfraCategory.CategoryId = dbo.tbInfraField.CategoryId INNER JOIN
                         dbo.tbInfraValue ON dbo.tbInfraField.FieldId = dbo.tbInfraValue.FieldId
WHERE        (dbo.tbInfraValue.ObjId = 3092)
ORDER BY Category, Field
GO
/****** Object:  Table [dbo].[tbInfraConn]    Script Date: 08.04.2021 15:06:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbInfraConn](
	[ParentObjId] [int] NOT NULL,
	[ChildObjId] [int] NOT NULL,
	[ConnTypeId] [int] NULL,
 CONSTRAINT [PK_tbInfraConn_1] PRIMARY KEY CLUSTERED 
(
	[ParentObjId] ASC,
	[ChildObjId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbInfraFieldTemp]    Script Date: 08.04.2021 15:06:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbInfraFieldTemp](
	[ObjTypeId] [int] NOT NULL,
	[FieldId] [int] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[DataTypeId] [int] NOT NULL,
	[Description] [nvarchar](4000) NOT NULL,
	[Label] [nvarchar](100) NULL,
	[Category] [nvarchar](100) NULL,
 CONSTRAINT [PK_tbInfraFieldTemp] PRIMARY KEY CLUSTERED 
(
	[ObjTypeId] ASC,
	[FieldId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbInfraGeometry]    Script Date: 08.04.2021 15:06:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbInfraGeometry](
	[GeometryId] [int] IDENTITY(1,1) NOT NULL,
	[ValueId] [int] NOT NULL,
	[Xp] [float] NOT NULL,
	[Yp] [float] NOT NULL,
 CONSTRAINT [PK_tbInfraGeometry] PRIMARY KEY CLUSTERED 
(
	[GeometryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbInfraObj]    Script Date: 08.04.2021 15:06:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbInfraObj](
	[ObjId] [int] NOT NULL,
	[ObjTypeId] [int] NOT NULL,
 CONSTRAINT [PK_tbInfObject] PRIMARY KEY CLUSTERED 
(
	[ObjId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbInfraObjType]    Script Date: 08.04.2021 15:06:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbInfraObjType](
	[ObjTypeId] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_tbInfraObjType] PRIMARY KEY CLUSTERED 
(
	[ObjTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbInfraObjTypeField]    Script Date: 08.04.2021 15:06:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbInfraObjTypeField](
	[FieldId] [int] NOT NULL,
	[ObjTypeId] [int] NOT NULL,
 CONSTRAINT [PK_tbInfraObjTypeField] PRIMARY KEY CLUSTERED 
(
	[FieldId] ASC,
	[ObjTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbInfraZone]    Script Date: 08.04.2021 15:06:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbInfraZone](
	[ZoneId] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_tbInfraZone] PRIMARY KEY CLUSTERED 
(
	[ZoneId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Index [IX_tbInfraValue]    Script Date: 08.04.2021 15:06:29 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_tbInfraValue] ON [dbo].[tbInfraValue]
(
	[FieldId] ASC,
	[ObjId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[tbInfraConn]  WITH CHECK ADD  CONSTRAINT [FK_tbInfraConn_tbInfraObj] FOREIGN KEY([ChildObjId])
REFERENCES [dbo].[tbInfraObj] ([ObjId])
GO
ALTER TABLE [dbo].[tbInfraConn] CHECK CONSTRAINT [FK_tbInfraConn_tbInfraObj]
GO
ALTER TABLE [dbo].[tbInfraConn]  WITH CHECK ADD  CONSTRAINT [FK_tbInfraConn_tbInfraObj1] FOREIGN KEY([ParentObjId])
REFERENCES [dbo].[tbInfraObj] ([ObjId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[tbInfraConn] CHECK CONSTRAINT [FK_tbInfraConn_tbInfraObj1]
GO
ALTER TABLE [dbo].[tbInfraField]  WITH CHECK ADD  CONSTRAINT [FK_tbInfraField_tbInfraCategory] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[tbInfraCategory] ([CategoryId])
GO
ALTER TABLE [dbo].[tbInfraField] CHECK CONSTRAINT [FK_tbInfraField_tbInfraCategory]
GO
ALTER TABLE [dbo].[tbInfraGeometry]  WITH CHECK ADD  CONSTRAINT [FK_tbInfraGeometry_tbInfraValue] FOREIGN KEY([ValueId])
REFERENCES [dbo].[tbInfraValue] ([ValueId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[tbInfraGeometry] CHECK CONSTRAINT [FK_tbInfraGeometry_tbInfraValue]
GO
ALTER TABLE [dbo].[tbInfraObj]  WITH CHECK ADD  CONSTRAINT [FK_tbInfraObj_tbInfraObjType] FOREIGN KEY([ObjTypeId])
REFERENCES [dbo].[tbInfraObjType] ([ObjTypeId])
GO
ALTER TABLE [dbo].[tbInfraObj] CHECK CONSTRAINT [FK_tbInfraObj_tbInfraObjType]
GO
ALTER TABLE [dbo].[tbInfraObjTypeField]  WITH CHECK ADD  CONSTRAINT [FK_tbInfraObjTypeField_tbInfraField] FOREIGN KEY([FieldId])
REFERENCES [dbo].[tbInfraField] ([FieldId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[tbInfraObjTypeField] CHECK CONSTRAINT [FK_tbInfraObjTypeField_tbInfraField]
GO
ALTER TABLE [dbo].[tbInfraObjTypeField]  WITH CHECK ADD  CONSTRAINT [FK_tbInfraObjTypeField_tbInfraObjType] FOREIGN KEY([ObjTypeId])
REFERENCES [dbo].[tbInfraObjType] ([ObjTypeId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[tbInfraObjTypeField] CHECK CONSTRAINT [FK_tbInfraObjTypeField_tbInfraObjType]
GO
ALTER TABLE [dbo].[tbInfraValue]  WITH CHECK ADD  CONSTRAINT [FK_tbInfraObjFiledValue_tbInfraObj] FOREIGN KEY([ObjId])
REFERENCES [dbo].[tbInfraObj] ([ObjId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[tbInfraValue] CHECK CONSTRAINT [FK_tbInfraObjFiledValue_tbInfraObj]
GO
ALTER TABLE [dbo].[tbInfraValue]  WITH CHECK ADD  CONSTRAINT [FK_tbInfraObjFiledValue_tbInfraObjField] FOREIGN KEY([FieldId])
REFERENCES [dbo].[tbInfraField] ([FieldId])
GO
ALTER TABLE [dbo].[tbInfraValue] CHECK CONSTRAINT [FK_tbInfraObjFiledValue_tbInfraObjField]
GO
