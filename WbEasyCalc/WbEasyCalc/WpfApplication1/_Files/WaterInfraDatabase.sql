USE [WaterInfra]
GO
/****** Object:  UserDefinedFunction [dbo].[fnCustomerMeterInZoneCount]    Script Date: 08.06.2021 14:32:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[fnCustomerMeterInZoneCount]
(
	@ZoneId INT
)
RETURNS INT
AS
BEGIN
	-- Declare the return variable here
	DECLARE @ResultVar INT = 7;

	WITH 
	tJunctionList AS (
		-- Junction list for a particular ZoneId
		SELECT        
			dbo.tbInfraObj.ObjId 
		FROM            
			dbo.tbInfraObj 
			INNER JOIN dbo.tbInfraValue AS tbInfraValue_1 ON dbo.tbInfraObj.ObjId = tbInfraValue_1.ObjId
		WHERE        
			dbo.tbInfraObj.ObjTypeId = 55					-- Junction
			AND tbInfraValue_1.FieldId = 647				-- ZoneId
			AND tbInfraValue_1.IntValue = @ZoneId 
	)
	SELECT        
		@ResultVar = COUNT(dbo.tbInfraObj.ObjId)
	FROM            
		dbo.tbInfraObj 
		INNER JOIN dbo.tbInfraValue AS tbInfraValue_1 ON dbo.tbInfraObj.ObjId = tbInfraValue_1.ObjId
	WHERE        
		dbo.tbInfraObj.ObjTypeId = 73					-- CustomerMeter
		AND tbInfraValue_1.FieldId = 769				-- Associated Element
		AND tbInfraValue_1.IntValue IN (SELECT ObjId FROM tJunctionList);
	
	-- Return the result of the function
	RETURN COALESCE(@ResultVar, 0)

END
GO
/****** Object:  UserDefinedFunction [dbo].[fnPipeLenghtInZoneSum]    Script Date: 08.06.2021 14:32:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[fnPipeLenghtInZoneSum]
(
	@ZoneId INT
)
RETURNS FLOAT
AS
BEGIN
	-- Declare the return variable here
	DECLARE @ResultVar FLOAT;

SELECT        
	@ResultVar = SUM(tbInfraValue_2.FloatValue) * 0.3048 / 1000
FROM            
	dbo.tbInfraObj 
	INNER JOIN dbo.tbInfraValue ON dbo.tbInfraObj.ObjId = dbo.tbInfraValue.ObjId 
    INNER JOIN dbo.tbInfraValue AS tbInfraValue_1 ON dbo.tbInfraObj.ObjId = tbInfraValue_1.ObjId 
    INNER JOIN dbo.tbInfraValue AS tbInfraValue_2 ON dbo.tbInfraObj.ObjId = tbInfraValue_2.ObjId
WHERE        
	(dbo.tbInfraObj.ObjTypeId = 69)							-- Pipe
	AND (dbo.tbInfraValue.FieldId = 2)						-- Label
	AND (dbo.tbInfraValue.StringValue NOT LIKE N'%-be%') 
	AND (tbInfraValue_1.FieldId = 647)						-- Physical_Zone
	AND (tbInfraValue_1.IntValue = @ZoneId) 
	AND (tbInfraValue_2.FieldId = 621);						-- HMIGeometryScaledLength

	-- Return the result of the function
	RETURN COALESCE(@ResultVar, 0)

END
GO
/****** Object:  Table [dbo].[tbInfraValue]    Script Date: 08.06.2021 14:32:09 ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbInfraField]    Script Date: 08.06.2021 14:32:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbInfraField](
	[FieldId] [int] NOT NULL,
	[CategoryId] [int] NOT NULL,
	[DataTypeId] [int] NOT NULL,
	[UnitCorrectionId] [int] NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Label] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](4000) NOT NULL,
 CONSTRAINT [PK_tbInfraObjFieldType] PRIMARY KEY CLUSTERED 
(
	[FieldId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbInfraCategory]    Script Date: 08.06.2021 14:32:09 ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbInfraObj]    Script Date: 08.06.2021 14:32:09 ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[__View_1]    Script Date: 08.06.2021 14:32:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[__View_1]
AS
SELECT        TOP (100) PERCENT dbo.tbInfraCategory.Name AS Category, dbo.tbInfraField.Name, dbo.tbInfraField.Label, dbo.tbInfraValue.FloatValue
FROM            dbo.tbInfraObj INNER JOIN
                         dbo.tbInfraValue ON dbo.tbInfraObj.ObjId = dbo.tbInfraValue.ObjId INNER JOIN
                         dbo.tbInfraField ON dbo.tbInfraValue.FieldId = dbo.tbInfraField.FieldId INNER JOIN
                         dbo.tbInfraCategory ON dbo.tbInfraField.CategoryId = dbo.tbInfraCategory.CategoryId
WHERE        (dbo.tbInfraObj.ObjId = 3092) AND (dbo.tbInfraField.DataTypeId = 2)
ORDER BY Category
GO
/****** Object:  View [dbo].[__View_2]    Script Date: 08.06.2021 14:32:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[__View_2]
AS
SELECT        TOP (100) PERCENT dbo.tbInfraCategory.Name AS Category, dbo.tbInfraField.FieldId, dbo.tbInfraField.Name, dbo.tbInfraField.Label
FROM            dbo.tbInfraCategory INNER JOIN
                         dbo.tbInfraField ON dbo.tbInfraCategory.CategoryId = dbo.tbInfraField.CategoryId
ORDER BY Category, dbo.tbInfraField.Name
GO
/****** Object:  View [dbo].[__vwInfraJunctionWithoutSsSuffix_2]    Script Date: 08.06.2021 14:32:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[__vwInfraJunctionWithoutSsSuffix_2]
AS
SELECT        dbo.tbInfraValue.ObjId, dbo.tbInfraValue.StringValue, tbInfraValue_2.FloatValue * 0.3048 AS LenghtInMeters
FROM            dbo.tbInfraObj INNER JOIN
                         dbo.tbInfraValue ON dbo.tbInfraObj.ObjId = dbo.tbInfraValue.ObjId INNER JOIN
                         dbo.tbInfraValue AS tbInfraValue_1 ON dbo.tbInfraObj.ObjId = tbInfraValue_1.ObjId INNER JOIN
                         dbo.tbInfraValue AS tbInfraValue_2 ON dbo.tbInfraObj.ObjId = tbInfraValue_2.ObjId
WHERE        (dbo.tbInfraObj.ObjTypeId = 69) AND (dbo.tbInfraValue.FieldId = 2) AND (dbo.tbInfraValue.StringValue NOT LIKE N'%-be%') AND (tbInfraValue_1.FieldId = 614) AND (tbInfraValue_1.IntValue = 6773) AND (tbInfraValue_2.FieldId = 588)
GO
/****** Object:  View [dbo].[vwInfraJunctionWithoutSsSuffix]    Script Date: 08.06.2021 14:32:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE VIEW [dbo].[vwInfraJunctionWithoutSsSuffix]
AS
SELECT        
	--dbo.tbInfraValue.ObjId, 
	--dbo.tbInfraValue.StringValue,
	tbInfraValue_1.IntValue AS ZoneId,
	CONCAT(dbo.tbInfraValue.StringValue, '.',CAST(dbo.tbInfraValue.ObjId AS NVARCHAR), '.', 'NodePrs') AS ScadaName
FROM            
	dbo.tbInfraObj 
	INNER JOIN dbo.tbInfraValue ON dbo.tbInfraObj.ObjId = dbo.tbInfraValue.ObjId 
	INNER JOIN dbo.tbInfraValue AS tbInfraValue_1 ON dbo.tbInfraObj.ObjId = tbInfraValue_1.ObjId
WHERE        
	(dbo.tbInfraObj.ObjTypeId IN (54, 55))					-- Junction, Hydrant
	AND (dbo.tbInfraValue.FieldId = 2)						-- Label 
	AND (dbo.tbInfraValue.StringValue NOT LIKE N'%-S%')
	AND (tbInfraValue_1.FieldId = 647)						-- Physical_Zone
	--AND (tbInfraValue_1.IntValue = 6773) 
GO
/****** Object:  View [dbo].[__vwObjectFields]    Script Date: 08.06.2021 14:32:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[__vwObjectFields]
AS
SELECT        TOP (100) PERCENT dbo.tbInfraValue.ObjId, dbo.tbInfraCategory.Name AS Category, dbo.tbInfraField.Name AS Field, dbo.tbInfraValue.IntValue, dbo.tbInfraValue.FloatValue, dbo.tbInfraValue.StringValue, 
                         dbo.tbInfraValue.BooleanValue, dbo.tbInfraValue.DateTimeValue
FROM            dbo.tbInfraCategory INNER JOIN
                         dbo.tbInfraField ON dbo.tbInfraCategory.CategoryId = dbo.tbInfraField.CategoryId INNER JOIN
                         dbo.tbInfraValue ON dbo.tbInfraField.FieldId = dbo.tbInfraValue.FieldId
WHERE        (dbo.tbInfraValue.ObjId = 3092)
ORDER BY Category, Field
GO
/****** Object:  Table [dbo].[tbInfraDemandPattern]    Script Date: 08.06.2021 14:32:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbInfraDemandPattern](
	[DemandPatternId] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_tbInfraDemandPattern] PRIMARY KEY CLUSTERED 
(
	[DemandPatternId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbInfraFieldTemp]    Script Date: 08.06.2021 14:32:09 ******/
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
	[FieldTypeId] [int] NOT NULL,
 CONSTRAINT [PK_tbInfraFieldTemp] PRIMARY KEY CLUSTERED 
(
	[ObjTypeId] ASC,
	[FieldId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbInfraGeometry]    Script Date: 08.06.2021 14:32:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbInfraGeometry](
	[GeometryId] [int] IDENTITY(1,1) NOT NULL,
	[ValueId] [int] NOT NULL,
	[OrderNo] [int] NOT NULL,
	[Xp] [float] NOT NULL,
	[Yp] [float] NOT NULL,
 CONSTRAINT [PK_tbInfraGeometry] PRIMARY KEY CLUSTERED 
(
	[GeometryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbInfraObjType]    Script Date: 08.06.2021 14:32:09 ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbInfraObjTypeField]    Script Date: 08.06.2021 14:32:09 ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbInfraUnitCorrection]    Script Date: 08.06.2021 14:32:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbInfraUnitCorrection](
	[UnitCorrectionId] [int] NOT NULL,
	[Value] [float] NOT NULL,
	[Description] [nvarchar](4000) NOT NULL,
 CONSTRAINT [PK_tbInfraUnitCorrection] PRIMARY KEY CLUSTERED 
(
	[UnitCorrectionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbInfraZone]    Script Date: 08.06.2021 14:32:09 ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Index [IX_tbInfraValue]    Script Date: 08.06.2021 14:32:09 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_tbInfraValue] ON [dbo].[tbInfraValue]
(
	[ObjId] ASC,
	[FieldId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[tbInfraFieldTemp] ADD  CONSTRAINT [DF_tbInfraFieldTemp_FieldTypeId]  DEFAULT ((1)) FOR [FieldTypeId]
GO
ALTER TABLE [dbo].[tbInfraField]  WITH CHECK ADD  CONSTRAINT [FK_tbInfraField_tbInfraCategory] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[tbInfraCategory] ([CategoryId])
GO
ALTER TABLE [dbo].[tbInfraField] CHECK CONSTRAINT [FK_tbInfraField_tbInfraCategory]
GO
ALTER TABLE [dbo].[tbInfraField]  WITH CHECK ADD  CONSTRAINT [FK_tbInfraField_tbInfraUnitCorrection] FOREIGN KEY([UnitCorrectionId])
REFERENCES [dbo].[tbInfraUnitCorrection] ([UnitCorrectionId])
GO
ALTER TABLE [dbo].[tbInfraField] CHECK CONSTRAINT [FK_tbInfraField_tbInfraUnitCorrection]
GO
ALTER TABLE [dbo].[tbInfraGeometry]  WITH CHECK ADD  CONSTRAINT [FK_tbInfraGeometry_tbInfraValue] FOREIGN KEY([ValueId])
REFERENCES [dbo].[tbInfraValue] ([ValueId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[tbInfraGeometry] CHECK CONSTRAINT [FK_tbInfraGeometry_tbInfraValue]
GO
ALTER TABLE [dbo].[tbInfraObj]  WITH CHECK ADD  CONSTRAINT [FK_tbInfraObj_tbInfraObjType] FOREIGN KEY([ObjTypeId])
REFERENCES [dbo].[tbInfraObjType] ([ObjTypeId])
ON DELETE CASCADE
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
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[41] 4[3] 2[19] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "tbInfraObj"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 115
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tbInfraField"
            Begin Extent = 
               Top = 5
               Left = 694
               Bottom = 196
               Right = 864
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tbInfraValue"
            Begin Extent = 
               Top = 6
               Left = 454
               Bottom = 277
               Right = 624
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tbInfraCategory"
            Begin Extent = 
               Top = 11
               Left = 954
               Bottom = 107
               Right = 1124
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 10
         Width = 284
         Width = 2790
         Width = 4320
         Width = 3420
         Width = 2925
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 2025
         Width = 2280
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         O' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'__View_1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'r = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'__View_1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'__View_1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[20] 4[17] 2[7] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "tbInfraCategory"
            Begin Extent = 
               Top = 6
               Left = 662
               Bottom = 102
               Right = 832
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tbInfraField"
            Begin Extent = 
               Top = 6
               Left = 454
               Bottom = 136
               Right = 624
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 3480
         Width = 1140
         Width = 5340
         Width = 7860
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 2055
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'__View_2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'__View_2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[14] 4[39] 2[16] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "tbInfraObj"
            Begin Extent = 
               Top = 6
               Left = 246
               Bottom = 102
               Right = 416
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tbInfraValue"
            Begin Extent = 
               Top = 9
               Left = 464
               Bottom = 266
               Right = 634
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tbInfraValue_1"
            Begin Extent = 
               Top = 5
               Left = 33
               Bottom = 246
               Right = 203
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tbInfraValue_2"
            Begin Extent = 
               Top = 6
               Left = 672
               Bottom = 252
               Right = 842
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 3165
         Width = 2355
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1830
         Or = 1350
         Or = 1350
         Or = ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'__vwInfraJunctionWithoutSsSuffix_2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'__vwInfraJunctionWithoutSsSuffix_2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'__vwInfraJunctionWithoutSsSuffix_2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[25] 4[3] 2[27] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "tbInfraCategory"
            Begin Extent = 
               Top = 12
               Left = 40
               Bottom = 121
               Right = 210
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tbInfraField"
            Begin Extent = 
               Top = 9
               Left = 278
               Bottom = 196
               Right = 448
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tbInfraValue"
            Begin Extent = 
               Top = 10
               Left = 540
               Bottom = 199
               Right = 710
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 3045
         Width = 3855
         Width = 1500
         Width = 2175
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'__vwObjectFields'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'__vwObjectFields'
GO
