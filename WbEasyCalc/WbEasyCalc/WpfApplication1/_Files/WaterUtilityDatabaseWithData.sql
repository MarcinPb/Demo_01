USE [WaterUtility]
GO


/****** Object:  Schema [config]    Script Date: 26.05.2021 09:39:29 ******/
CREATE SCHEMA [config]
GO
/****** Object:  UserDefinedFunction [dbo].[fnGetArchivedDate]    Script Date: 26.05.2021 09:39:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[fnGetArchivedDate]
(
	@YearNo [int], 
	@MonthNo [int]
)
RETURNS DATETIME
AS
BEGIN

DECLARE 
	@ArchivedDate DATETIME,
	@DayNo INT = 1;

SELECT
	@DayNo = CASE @MonthNo WHEN 13 THEN 31 ELSE 1 END,
	@MonthNo = CASE @MonthNo WHEN 13 THEN 12 ELSE @MonthNo END;

SET @ArchivedDate = CONCAT(@YearNo, '-', FORMAT(@MonthNo, '00'), '-', FORMAT(@DayNo, '00'))
RETURN @ArchivedDate;

END
GO
/****** Object:  UserDefinedFunction [dbo].[fnScadaPressureForZoneAvg]    Script Date: 26.05.2021 09:39:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[fnScadaPressureForZoneAvg]
(
	@YearNo INT,
	@MonthNo INT,
	@ZoneId INT
)
RETURNS FLOAT
AS
BEGIN
	-- Declare the return variable here
	DECLARE @ResultVar FLOAT = 0;

	DECLARE @InfraTab TABLE (
		ScadaName NVARCHAR(100)
	);

	INSERT INTO @InfraTab (
		ScadaName
	)
	SELECT 
		ScadaName
	FROM
		WaterInfra.[dbo].[vwInfraJunctionWithoutSsSuffix] tInfra
	WHERE        
		tInfra.ZoneId = @ZoneId;

	--SELECT * FROM @InfraTab;


	IF (@YearNo=2019) BEGIN
		SELECT 
			@ResultVar = AVG(tArch.D_VALUE_FLO)
		FROM            
			TWDB.dbo.TELWIN_MAP tMap
			INNER JOIN TWDB.dbo.AR_0000_2019 tArch ON tMap.D_ID = tArch.D_VAR_ID
			--INNER JOIN @InfraTab tInfra ON tMap.D_NAME = tInfra.ScadaName
		WHERE        
			MONTH(tArch.D_TIME) = @MonthNo
			AND YEAR(tArch.D_TIME) = @YearNo
			AND tMap.D_NAME IN (SELECT ScadaName FROM @InfraTab);
	END;

	IF (@YearNo=2020) BEGIN
		SELECT 
			@ResultVar = AVG(tArch.D_VALUE_FLO)
		FROM            
			TWDB.dbo.TELWIN_MAP tMap
			INNER JOIN TWDB.dbo.AR_0000_2020 tArch ON tMap.D_ID = tArch.D_VAR_ID
			--INNER JOIN @InfraTab tInfra ON tMap.D_NAME = tInfra.ScadaName
		WHERE        
			MONTH(tArch.D_TIME) = @MonthNo
			AND YEAR(tArch.D_TIME) = @YearNo
			AND tMap.D_NAME IN (SELECT ScadaName FROM @InfraTab);
	END;

	IF (@YearNo=2021) BEGIN
		SELECT 
			@ResultVar = AVG(tArch.D_VALUE_FLO)
		FROM            
			TWDB.dbo.TELWIN_MAP tMap
			INNER JOIN TWDB.dbo.AR_0000_2021 tArch ON tMap.D_ID = tArch.D_VAR_ID
			--INNER JOIN @InfraTab tInfra ON tMap.D_NAME = tInfra.ScadaName
		WHERE        
			MONTH(tArch.D_TIME) = @MonthNo
			AND YEAR(tArch.D_TIME) = @YearNo
			AND tMap.D_NAME IN (SELECT ScadaName FROM @InfraTab);
	END;


	-- Return the result of the function
	RETURN @ResultVar

END
GO
/****** Object:  UserDefinedFunction [dbo].[STRING_SPLIT_MY]    Script Date: 26.05.2021 09:39:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	How to split a comma-separated value to columns
--              https://stackoverflow.com/questions/10581772/how-to-split-a-comma-separated-value-to-columns
-- =============================================
CREATE FUNCTION [dbo].[STRING_SPLIT_MY] 
(
		@String varchar(max),
        @Delimiter varchar(4000)
)
RETURNS 
@Table_Variable_Name TABLE 
(
	[value] NVARCHAR(4000)
)
AS
BEGIN

	DECLARE @X xml;
	SELECT @X = CONVERT(xml,'<root><myvalue>' + REPLACE(@String, @Delimiter, '</myvalue><myvalue>') + '</myvalue></root> ');

	INSERT INTO @Table_Variable_Name ([value])
	SELECT  T.c.value('.', 'varchar(20)') AS [value] FROM @X.nodes('/root/myvalue') T(c);
	 
RETURN 
END
GO
/****** Object:  Table [dbo].[tb_zuzycia_stref]    Script Date: 26.05.2021 09:39:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tb_zuzycia_stref](
	[id_strefy] [int] NOT NULL,
	[nazwa_strefy] [nvarchar](50) NOT NULL,
	[dlugosc_sieci] [float] NOT NULL,
	[ilosc_przylaczy] [int] NOT NULL,
	[sprzedaz_w_strefie] [float] NOT NULL,
	[year] [int] NOT NULL,
	[month] [int] NOT NULL,
	[ilosc_przylaczy_czynnych] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[vwGisWaterConsumption]    Script Date: 26.05.2021 09:39:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE VIEW [dbo].[vwGisWaterConsumption]
AS
--SELECT * FROM LEGNICA.legnica.gis_data.zuzycia_stref;
SELECT * FROM dbo.tb_zuzycia_stref;
GO
/****** Object:  Table [config].[WaterBalanceConfig]    Script Date: 26.05.2021 09:39:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [config].[WaterBalanceConfig](
	[ID] [int] NOT NULL,
	[ZoneName] [varchar](128) NOT NULL,
	[DestinationName] [varchar](80) NOT NULL,
	[IsActive] [tinyint] NULL,
	[Timestamp] [datetime] NULL,
	[ZoneNo] [int] NULL,
	[ModelZoneId] [int] NOT NULL,
	[GisZoneId] [int] NULL,
	[Var1MinId] [int] NULL,
	[Var10MinId] [int] NULL,
 CONSTRAINT [PK_WaterBalanceConfig_1] PRIMARY KEY CLUSTERED 
(
	[ModelZoneId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbLog]    Script Date: 26.05.2021 09:39:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbLog](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ExecDate] [datetime] NOT NULL,
	[ExecTime] [int] NOT NULL,
	[CommandName] [nvarchar](50) NULL,
	[TestDate] [datetime] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbMonth]    Script Date: 26.05.2021 09:39:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbMonth](
	[MonthId] [int] NOT NULL,
	[MonthName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_tbMonth] PRIMARY KEY CLUSTERED 
(
	[MonthId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbSetting]    Script Date: 26.05.2021 09:39:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbSetting](
	[FinancData_G6] [float] NULL,
	[FinancData_K6] [nvarchar](400) NULL,
	[FinancData_G8] [float] NULL,
	[MatrixOneIn_SelectedOption] [int] NULL,
	[MatrixOneIn_C11] [float] NULL,
	[MatrixOneIn_C12] [float] NULL,
	[MatrixOneIn_C13] [float] NULL,
	[MatrixOneIn_C14] [float] NULL,
	[MatrixOneIn_C21] [float] NULL,
	[MatrixOneIn_C22] [float] NULL,
	[MatrixOneIn_C23] [float] NULL,
	[MatrixOneIn_C24] [float] NULL,
	[MatrixOneIn_D21] [float] NULL,
	[MatrixOneIn_D22] [float] NULL,
	[MatrixOneIn_D23] [float] NULL,
	[MatrixOneIn_D24] [float] NULL,
	[MatrixOneIn_E11] [float] NULL,
	[MatrixOneIn_E12] [float] NULL,
	[MatrixOneIn_E13] [float] NULL,
	[MatrixOneIn_E14] [float] NULL,
	[MatrixOneIn_E21] [float] NULL,
	[MatrixOneIn_E22] [float] NULL,
	[MatrixOneIn_E23] [float] NULL,
	[MatrixOneIn_E24] [float] NULL,
	[MatrixOneIn_F11] [float] NULL,
	[MatrixOneIn_F12] [float] NULL,
	[MatrixOneIn_F13] [float] NULL,
	[MatrixOneIn_F14] [float] NULL,
	[MatrixOneIn_F21] [float] NULL,
	[MatrixOneIn_F22] [float] NULL,
	[MatrixOneIn_F23] [float] NULL,
	[MatrixOneIn_F24] [float] NULL,
	[MatrixOneIn_G11] [float] NULL,
	[MatrixOneIn_G12] [float] NULL,
	[MatrixOneIn_G13] [float] NULL,
	[MatrixOneIn_G14] [float] NULL,
	[MatrixOneIn_G21] [float] NULL,
	[MatrixOneIn_G22] [float] NULL,
	[MatrixOneIn_G23] [float] NULL,
	[MatrixOneIn_G24] [float] NULL,
	[MatrixOneIn_H11] [float] NULL,
	[MatrixOneIn_H12] [float] NULL,
	[MatrixOneIn_H13] [float] NULL,
	[MatrixOneIn_H14] [float] NULL,
	[MatrixOneIn_H21] [float] NULL,
	[MatrixOneIn_H22] [float] NULL,
	[MatrixOneIn_H23] [float] NULL,
	[MatrixOneIn_H24] [float] NULL,
	[MatrixTwoIn_SelectedOption] [int] NULL,
	[MatrixTwoIn_C11] [float] NULL,
	[MatrixTwoIn_C12] [float] NULL,
	[MatrixTwoIn_C13] [float] NULL,
	[MatrixTwoIn_C14] [float] NULL,
	[MatrixTwoIn_C21] [float] NULL,
	[MatrixTwoIn_C22] [float] NULL,
	[MatrixTwoIn_C23] [float] NULL,
	[MatrixTwoIn_C24] [float] NULL,
	[MatrixTwoIn_D21] [float] NULL,
	[MatrixTwoIn_D22] [float] NULL,
	[MatrixTwoIn_D23] [float] NULL,
	[MatrixTwoIn_D24] [float] NULL,
	[MatrixTwoIn_E11] [float] NULL,
	[MatrixTwoIn_E12] [float] NULL,
	[MatrixTwoIn_E13] [float] NULL,
	[MatrixTwoIn_E14] [float] NULL,
	[MatrixTwoIn_E21] [float] NULL,
	[MatrixTwoIn_E22] [float] NULL,
	[MatrixTwoIn_E23] [float] NULL,
	[MatrixTwoIn_E24] [float] NULL,
	[MatrixTwoIn_F11] [float] NULL,
	[MatrixTwoIn_F12] [float] NULL,
	[MatrixTwoIn_F13] [float] NULL,
	[MatrixTwoIn_F14] [float] NULL,
	[MatrixTwoIn_F21] [float] NULL,
	[MatrixTwoIn_F22] [float] NULL,
	[MatrixTwoIn_F23] [float] NULL,
	[MatrixTwoIn_F24] [float] NULL,
	[MatrixTwoIn_G11] [float] NULL,
	[MatrixTwoIn_G12] [float] NULL,
	[MatrixTwoIn_G13] [float] NULL,
	[MatrixTwoIn_G14] [float] NULL,
	[MatrixTwoIn_G21] [float] NULL,
	[MatrixTwoIn_G22] [float] NULL,
	[MatrixTwoIn_G23] [float] NULL,
	[MatrixTwoIn_G24] [float] NULL,
	[MatrixTwoIn_H11] [float] NULL,
	[MatrixTwoIn_H12] [float] NULL,
	[MatrixTwoIn_H13] [float] NULL,
	[MatrixTwoIn_H14] [float] NULL,
	[MatrixTwoIn_H21] [float] NULL,
	[MatrixTwoIn_H22] [float] NULL,
	[MatrixTwoIn_H23] [float] NULL,
	[MatrixTwoIn_H24] [float] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbWaterConsumption]    Script Date: 26.05.2021 09:39:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbWaterConsumption](
	[WaterConsumptionId] [int] IDENTITY(1,1) NOT NULL,
	[WbEasyCalcDataId] [int] NULL,
	[Description] [nvarchar](max) NULL,
	[WaterConsumptionCategoryId] [int] NOT NULL,
	[WaterConsumptionStatusId] [int] NOT NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[Latitude] [float] NULL,
	[Lontitude] [float] NULL,
	[RelatedId] [int] NOT NULL,
	[Value] [float] NOT NULL,
 CONSTRAINT [PK_tbWaterConsumption] PRIMARY KEY CLUSTERED 
(
	[WaterConsumptionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbWaterConsumptionCategory]    Script Date: 26.05.2021 09:39:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbWaterConsumptionCategory](
	[WaterConsumptionCategoryId] [int] NOT NULL,
	[WaterConsumptionCategoryName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_tbWaterConsumptionCategory] PRIMARY KEY CLUSTERED 
(
	[WaterConsumptionCategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbWaterConsumptionCategoryStatus]    Script Date: 26.05.2021 09:39:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbWaterConsumptionCategoryStatus](
	[CategoryId] [int] NOT NULL,
	[StatusId] [int] NOT NULL,
	[ExcelCellId] [int] NOT NULL,
 CONSTRAINT [PK_tbWaterConsumptionCategoryStatus] PRIMARY KEY CLUSTERED 
(
	[CategoryId] ASC,
	[StatusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbWaterConsumptionCategoryStatusExcel]    Script Date: 26.05.2021 09:39:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbWaterConsumptionCategoryStatusExcel](
	[ExcelCellId] [int] NOT NULL,
	[ExcelCellName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_tbWaterConsumptionCategoryStatusExcel] PRIMARY KEY CLUSTERED 
(
	[ExcelCellId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbWaterConsumptionStatus]    Script Date: 26.05.2021 09:39:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbWaterConsumptionStatus](
	[WaterConsumptionStatusId] [int] NOT NULL,
	[WaterConsumptionStatusName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_tbWaterConsumptionStatus] PRIMARY KEY CLUSTERED 
(
	[WaterConsumptionStatusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbWbEasyCalcData]    Script Date: 26.05.2021 09:39:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbWbEasyCalcData](
	[WbEasyCalcDataId] [int] IDENTITY(1,1) NOT NULL,
	[CreateLogin] [nvarchar](50) NULL,
	[CreateDate] [datetime] NOT NULL,
	[ModifyLogin] [nvarchar](50) NULL,
	[ModifyDate] [datetime] NOT NULL,
	[ZoneId] [int] NOT NULL,
	[YearNo] [int] NOT NULL,
	[MonthNo] [int] NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[IsArchive] [bit] NOT NULL,
	[IsAccepted] [bit] NOT NULL,
	[Start_PeriodDays_M21] [int] NOT NULL,
	[SysInput_Desc_B6] [nvarchar](400) NULL,
	[SysInput_Desc_B7] [nvarchar](400) NULL,
	[SysInput_Desc_B8] [nvarchar](400) NULL,
	[SysInput_Desc_B9] [nvarchar](400) NULL,
	[SysInput_SystemInputVolumeM3_D6] [float] NOT NULL,
	[SysInput_SystemInputVolumeError_F6] [float] NOT NULL,
	[SysInput_SystemInputVolumeM3_D7] [float] NOT NULL,
	[SysInput_SystemInputVolumeError_F7] [float] NOT NULL,
	[SysInput_SystemInputVolumeM3_D8] [float] NOT NULL,
	[SysInput_SystemInputVolumeError_F8] [float] NOT NULL,
	[SysInput_SystemInputVolumeM3_D9] [float] NOT NULL,
	[SysInput_SystemInputVolumeError_F9] [float] NOT NULL,
	[BilledCons_Desc_B8] [nvarchar](400) NULL,
	[BilledCons_Desc_B9] [nvarchar](400) NULL,
	[BilledCons_Desc_B10] [nvarchar](400) NULL,
	[BilledCons_Desc_B11] [nvarchar](400) NULL,
	[BilledCons_Desc_F8] [nvarchar](400) NULL,
	[BilledCons_Desc_F9] [nvarchar](400) NULL,
	[BilledCons_Desc_F10] [nvarchar](400) NULL,
	[BilledCons_Desc_F11] [nvarchar](400) NULL,
	[UnbilledCons_Desc_D8] [nvarchar](400) NULL,
	[UnbilledCons_Desc_D9] [nvarchar](400) NULL,
	[UnbilledCons_Desc_D10] [nvarchar](400) NULL,
	[UnbilledCons_Desc_D11] [nvarchar](400) NULL,
	[UnbilledCons_Desc_F6] [nvarchar](400) NULL,
	[UnbilledCons_Desc_F7] [nvarchar](400) NULL,
	[UnbilledCons_Desc_F8] [nvarchar](400) NULL,
	[UnbilledCons_Desc_F9] [nvarchar](400) NULL,
	[UnbilledCons_Desc_F10] [nvarchar](400) NULL,
	[UnbilledCons_Desc_F11] [nvarchar](400) NULL,
	[UnauthCons_Desc_B18] [nvarchar](400) NULL,
	[UnauthCons_Desc_B19] [nvarchar](400) NULL,
	[UnauthCons_Desc_B20] [nvarchar](400) NULL,
	[UnauthCons_Desc_B21] [nvarchar](400) NULL,
	[MetErrors_Desc_D12] [nvarchar](400) NULL,
	[MetErrors_Desc_D13] [nvarchar](400) NULL,
	[MetErrors_Desc_D14] [nvarchar](400) NULL,
	[MetErrors_Desc_D15] [nvarchar](400) NULL,
	[Network_Desc_B7] [nvarchar](400) NULL,
	[Network_Desc_B8] [nvarchar](400) NULL,
	[Network_Desc_B9] [nvarchar](400) NULL,
	[Network_Desc_B10] [nvarchar](400) NULL,
	[Interm_Area_B7] [nvarchar](400) NULL,
	[Interm_Area_B8] [nvarchar](400) NULL,
	[Interm_Area_B9] [nvarchar](400) NULL,
	[Interm_Area_B10] [nvarchar](400) NULL,
	[BilledCons_BilledMetConsBulkWatSupExpM3_D6] [float] NOT NULL,
	[BilledCons_BilledUnmetConsBulkWatSupExpM3_H6] [float] NOT NULL,
	[BilledCons_UnbMetConsM3_D8] [float] NOT NULL,
	[BilledCons_UnbMetConsM3_D9] [float] NOT NULL,
	[BilledCons_UnbMetConsM3_D10] [float] NOT NULL,
	[BilledCons_UnbMetConsM3_D11] [float] NOT NULL,
	[BilledCons_UnbUnmetConsM3_H8] [float] NOT NULL,
	[BilledCons_UnbUnmetConsM3_H9] [float] NOT NULL,
	[BilledCons_UnbUnmetConsM3_H10] [float] NOT NULL,
	[BilledCons_UnbUnmetConsM3_H11] [float] NOT NULL,
	[UnbilledCons_MetConsBulkWatSupExpM3_D6] [float] NOT NULL,
	[UnbilledCons_UnbMetConsM3_D8] [float] NOT NULL,
	[UnbilledCons_UnbMetConsM3_D9] [float] NOT NULL,
	[UnbilledCons_UnbMetConsM3_D10] [float] NOT NULL,
	[UnbilledCons_UnbMetConsM3_D11] [float] NOT NULL,
	[UnbilledCons_UnbUnmetConsM3_H6] [float] NOT NULL,
	[UnbilledCons_UnbUnmetConsM3_H7] [float] NOT NULL,
	[UnbilledCons_UnbUnmetConsM3_H8] [float] NOT NULL,
	[UnbilledCons_UnbUnmetConsM3_H9] [float] NOT NULL,
	[UnbilledCons_UnbUnmetConsM3_H10] [float] NOT NULL,
	[UnbilledCons_UnbUnmetConsM3_H11] [float] NOT NULL,
	[UnbilledCons_UnbUnmetConsError_J6] [float] NOT NULL,
	[UnbilledCons_UnbUnmetConsError_J7] [float] NOT NULL,
	[UnbilledCons_UnbUnmetConsError_J8] [float] NOT NULL,
	[UnbilledCons_UnbUnmetConsError_J9] [float] NOT NULL,
	[UnbilledCons_UnbUnmetConsError_J10] [float] NOT NULL,
	[UnbilledCons_UnbUnmetConsError_J11] [float] NOT NULL,
	[UnauthCons_IllegalConnDomEstNo_D6] [int] NOT NULL,
	[UnauthCons_IllegalConnDomPersPerHouse_H6] [float] NOT NULL,
	[UnauthCons_IllegalConnDomConsLitPerPersDay_J6] [float] NOT NULL,
	[UnauthCons_IllegalConnDomErrorMargin_F6] [float] NOT NULL,
	[UnauthCons_IllegalConnOthersErrorMargin_F10] [float] NOT NULL,
	[IllegalConnectionsOthersEstimatedNumber_D10] [float] NOT NULL,
	[IllegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10] [float] NOT NULL,
	[UnauthCons_MeterTampBypEtcEstNo_D14] [float] NOT NULL,
	[UnauthCons_MeterTampBypEtcErrorMargin_F14] [float] NOT NULL,
	[UnauthCons_MeterTampBypEtcConsLitPerCustDay_J14] [float] NOT NULL,
	[UnauthCons_OthersErrorMargin_F18] [float] NOT NULL,
	[UnauthCons_OthersErrorMargin_F19] [float] NOT NULL,
	[UnauthCons_OthersErrorMargin_F20] [float] NOT NULL,
	[UnauthCons_OthersErrorMargin_F21] [float] NOT NULL,
	[UnauthCons_OthersM3PerDay_J18] [float] NOT NULL,
	[UnauthCons_OthersM3PerDay_J19] [float] NOT NULL,
	[UnauthCons_OthersM3PerDay_J20] [float] NOT NULL,
	[UnauthCons_OthersM3PerDay_J21] [float] NOT NULL,
	[MetErrors_DetailedManualSpec_J6] [bit] NOT NULL,
	[MetErrors_BilledMetConsWoBulkSupMetUndrreg_H8] [float] NOT NULL,
	[MetErrors_BilledMetConsWoBulkSupErrorMargin_N8] [float] NOT NULL,
	[MetErrors_Total_F12] [float] NOT NULL,
	[MetErrors_Total_F13] [float] NOT NULL,
	[MetErrors_Total_F14] [float] NOT NULL,
	[MetErrors_Total_F15] [float] NOT NULL,
	[MetErrors_Meter_H12] [float] NOT NULL,
	[MetErrors_Meter_H13] [float] NOT NULL,
	[MetErrors_Meter_H14] [float] NOT NULL,
	[MetErrors_Meter_H15] [float] NOT NULL,
	[MetErrors_Error_N12] [float] NOT NULL,
	[MetErrors_Error_N13] [float] NOT NULL,
	[MetErrors_Error_N14] [float] NOT NULL,
	[MetErrors_Error_N15] [float] NOT NULL,
	[MeteredBulkSupplyExportErrorMargin_N32] [float] NOT NULL,
	[UnbilledMeteredConsumptionWithoutBulkSupplyErrorMargin_N34] [float] NOT NULL,
	[CorruptMeterReadingPracticessErrorMargin_N38] [float] NOT NULL,
	[DataHandlingErrorsOffice_L40] [float] NOT NULL,
	[DataHandlingErrorsOfficeErrorMargin_N40] [float] NOT NULL,
	[MetErrors_MetBulkSupExpMetUnderreg_H32] [float] NOT NULL,
	[MetErrors_UnbillMetConsWoBulkSupplMetUndrreg_H34] [float] NOT NULL,
	[MetErrors_CorruptMetReadPractMetUndrreg_H38] [float] NOT NULL,
	[Network_DistributionAndTransmissionMains_D7] [float] NOT NULL,
	[Network_DistributionAndTransmissionMains_D8] [float] NOT NULL,
	[Network_DistributionAndTransmissionMains_D9] [float] NOT NULL,
	[Network_DistributionAndTransmissionMains_D10] [float] NOT NULL,
	[Network_NoOfConnOfRegCustomers_H10] [float] NOT NULL,
	[Network_NoOfInactAccountsWSvcConns_H18] [float] NOT NULL,
	[Network_AvgLenOfSvcConnFromBoundaryToMeterM_H32] [float] NOT NULL,
	[Network_PossibleUnd_D30] [float] NOT NULL,
	[Network_NoCustomers_H7] [float] NOT NULL,
	[Network_ErrorMargin_J7] [float] NOT NULL,
	[Network_ErrorMargin_J10] [float] NOT NULL,
	[Network_ErrorMargin_J18] [float] NOT NULL,
	[Network_ErrorMargin_J32] [float] NOT NULL,
	[Network_ErrorMargin_D35] [float] NOT NULL,
	[Prs_Area_B7] [nvarchar](400) NULL,
	[Prs_Area_B8] [nvarchar](400) NULL,
	[Prs_Area_B9] [nvarchar](400) NULL,
	[Prs_Area_B10] [nvarchar](400) NULL,
	[Prs_ApproxNoOfConn_D7] [float] NOT NULL,
	[Prs_DailyAvgPrsM_F7] [float] NOT NULL,
	[Prs_ApproxNoOfConn_D8] [float] NOT NULL,
	[Prs_DailyAvgPrsM_F8] [float] NOT NULL,
	[Prs_ApproxNoOfConn_D9] [float] NOT NULL,
	[Prs_DailyAvgPrsM_F9] [float] NOT NULL,
	[Prs_ApproxNoOfConn_D10] [float] NOT NULL,
	[Prs_DailyAvgPrsM_F10] [float] NOT NULL,
	[Prs_ErrorMarg_F26] [float] NULL,
	[Interm_Conn_D7] [float] NOT NULL,
	[Interm_Conn_D8] [float] NOT NULL,
	[Interm_Conn_D9] [float] NOT NULL,
	[Interm_Conn_D10] [float] NOT NULL,
	[Interm_Days_F7] [float] NOT NULL,
	[Interm_Days_F8] [float] NOT NULL,
	[Interm_Days_F9] [float] NOT NULL,
	[Interm_Days_F10] [float] NOT NULL,
	[Interm_Hour_H7] [float] NOT NULL,
	[Interm_Hour_H8] [float] NOT NULL,
	[Interm_Hour_H9] [float] NOT NULL,
	[Interm_Hour_H10] [float] NOT NULL,
	[Interm_ErrorMarg_H26] [float] NOT NULL,
	[FinancData_G6] [float] NOT NULL,
	[FinancData_K6] [nvarchar](400) NOT NULL,
	[FinancData_G8] [float] NOT NULL,
	[FinancData_D26] [float] NOT NULL,
	[FinancData_G35] [float] NOT NULL,
	[MatrixOneIn_SelectedOption] [int] NOT NULL,
	[MatrixOneIn_C11] [float] NOT NULL,
	[MatrixOneIn_C12] [float] NOT NULL,
	[MatrixOneIn_C13] [float] NULL,
	[MatrixOneIn_C14] [float] NULL,
	[MatrixOneIn_C21] [float] NULL,
	[MatrixOneIn_C22] [float] NULL,
	[MatrixOneIn_C23] [float] NULL,
	[MatrixOneIn_C24] [float] NULL,
	[MatrixOneIn_D21] [float] NULL,
	[MatrixOneIn_D22] [float] NULL,
	[MatrixOneIn_D23] [float] NULL,
	[MatrixOneIn_D24] [float] NULL,
	[MatrixOneIn_E11] [float] NULL,
	[MatrixOneIn_E12] [float] NULL,
	[MatrixOneIn_E13] [float] NULL,
	[MatrixOneIn_E14] [float] NULL,
	[MatrixOneIn_E21] [float] NULL,
	[MatrixOneIn_E22] [float] NULL,
	[MatrixOneIn_E23] [float] NULL,
	[MatrixOneIn_E24] [float] NULL,
	[MatrixOneIn_F11] [float] NULL,
	[MatrixOneIn_F12] [float] NULL,
	[MatrixOneIn_F13] [float] NULL,
	[MatrixOneIn_F14] [float] NULL,
	[MatrixOneIn_F21] [float] NULL,
	[MatrixOneIn_F22] [float] NULL,
	[MatrixOneIn_F23] [float] NULL,
	[MatrixOneIn_F24] [float] NULL,
	[MatrixOneIn_G11] [float] NULL,
	[MatrixOneIn_G12] [float] NULL,
	[MatrixOneIn_G13] [float] NULL,
	[MatrixOneIn_G14] [float] NULL,
	[MatrixOneIn_G21] [float] NULL,
	[MatrixOneIn_G22] [float] NULL,
	[MatrixOneIn_G23] [float] NULL,
	[MatrixOneIn_G24] [float] NULL,
	[MatrixOneIn_H11] [float] NULL,
	[MatrixOneIn_H12] [float] NULL,
	[MatrixOneIn_H13] [float] NULL,
	[MatrixOneIn_H14] [float] NULL,
	[MatrixOneIn_H21] [float] NULL,
	[MatrixOneIn_H22] [float] NULL,
	[MatrixOneIn_H23] [float] NULL,
	[MatrixOneIn_H24] [float] NULL,
	[MatrixTwoIn_SelectedOption] [int] NULL,
	[MatrixTwoIn_D21] [int] NULL,
	[MatrixTwoIn_D22] [int] NULL,
	[MatrixTwoIn_D23] [int] NULL,
	[MatrixTwoIn_D24] [int] NULL,
	[MatrixTwoIn_E11] [int] NULL,
	[MatrixTwoIn_E12] [int] NULL,
	[MatrixTwoIn_E13] [int] NULL,
	[MatrixTwoIn_E14] [int] NULL,
	[MatrixTwoIn_E21] [int] NULL,
	[MatrixTwoIn_E22] [int] NULL,
	[MatrixTwoIn_E23] [int] NULL,
	[MatrixTwoIn_E24] [int] NULL,
	[MatrixTwoIn_F11] [int] NULL,
	[MatrixTwoIn_F12] [int] NULL,
	[MatrixTwoIn_F13] [int] NULL,
	[MatrixTwoIn_F14] [int] NULL,
	[MatrixTwoIn_F21] [int] NULL,
	[MatrixTwoIn_F22] [int] NULL,
	[MatrixTwoIn_F23] [int] NULL,
	[MatrixTwoIn_F24] [int] NULL,
	[MatrixTwoIn_G11] [int] NULL,
	[MatrixTwoIn_G12] [int] NULL,
	[MatrixTwoIn_G13] [int] NULL,
	[MatrixTwoIn_G14] [int] NULL,
	[MatrixTwoIn_G21] [int] NULL,
	[MatrixTwoIn_G22] [int] NULL,
	[MatrixTwoIn_G23] [int] NULL,
	[MatrixTwoIn_G24] [int] NULL,
	[MatrixTwoIn_H11] [int] NULL,
	[MatrixTwoIn_H12] [int] NULL,
	[MatrixTwoIn_H13] [int] NULL,
	[MatrixTwoIn_H14] [int] NULL,
	[MatrixTwoIn_H21] [int] NULL,
	[MatrixTwoIn_H22] [int] NULL,
	[MatrixTwoIn_H23] [int] NULL,
	[MatrixTwoIn_H24] [int] NULL,
	[SystemInputVolume_B19] [float] NOT NULL,
	[SystemInputVolumeErrorMargin_B21] [float] NOT NULL,
	[AuthorizedConsumption_K12] [float] NOT NULL,
	[AuthorizedConsumptionErrorMargin_K15] [float] NOT NULL,
	[WaterLosses_K29] [float] NOT NULL,
	[WaterLossesErrorMargin_K31] [float] NOT NULL,
	[BilledAuthorizedConsumption_T8] [float] NOT NULL,
	[UnbilledAuthorizedConsumption_T16] [float] NOT NULL,
	[UnbilledAuthorizedConsumptionErrorMargin_T20] [float] NOT NULL,
	[CommercialLosses_T26] [float] NOT NULL,
	[CommercialLossesErrorMargin_T29] [float] NOT NULL,
	[PhysicalLossesM3_T34] [float] NOT NULL,
	[PhyscialLossesErrorMargin_AH35] [float] NOT NULL,
	[BilledMeteredConsumption_AC4] [float] NOT NULL,
	[BilledUnmeteredConsumption_AC9] [float] NOT NULL,
	[UnbilledMeteredConsumption_AC14] [float] NOT NULL,
	[UnbilledUnmeteredConsumption_AC19] [float] NOT NULL,
	[UnbilledUnmeteredConsumptionErrorMargin_AO20] [float] NOT NULL,
	[UnauthorizedConsumption_AC24] [float] NOT NULL,
	[UnauthorizedConsumptionErrorMargin_AO25] [float] NOT NULL,
	[CustomerMeterInaccuraciesAndErrorsM3_AC29] [float] NOT NULL,
	[CustomerMeterInaccuraciesAndErrorsErrorMargin_AO30] [float] NOT NULL,
	[RevenueWaterM3_AY8] [float] NOT NULL,
	[NonRevenueWaterM3_AY24] [float] NOT NULL,
	[NonRevenueWaterErrorMargin_AY26] [float] NOT NULL,
 CONSTRAINT [PK_tbWbEasyCalcData] PRIMARY KEY CLUSTERED 
(
	[WbEasyCalcDataId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbYear]    Script Date: 26.05.2021 09:39:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbYear](
	[YearId] [int] NOT NULL,
	[YearName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_tbYear] PRIMARY KEY CLUSTERED 
(
	[YearId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [config].[WaterBalanceConfig] ([ID], [ZoneName], [DestinationName], [IsActive], [Timestamp], [ZoneNo], [ModelZoneId], [GisZoneId], [Var1MinId], [Var10MinId]) VALUES (1, N'Przybków', N'SI', 1, CAST(N'2020-09-18T10:47:30.143' AS DateTime), 1, 6773, 6, 224277, 225759)
GO
INSERT [config].[WaterBalanceConfig] ([ID], [ZoneName], [DestinationName], [IsActive], [Timestamp], [ZoneNo], [ModelZoneId], [GisZoneId], [Var1MinId], [Var10MinId]) VALUES (5, N'Stare Miasto', N'SII', 1, CAST(N'2020-09-18T10:44:28.897' AS DateTime), 2, 6774, 2, 224278, 225760)
GO
INSERT [config].[WaterBalanceConfig] ([ID], [ZoneName], [DestinationName], [IsActive], [Timestamp], [ZoneNo], [ModelZoneId], [GisZoneId], [Var1MinId], [Var10MinId]) VALUES (6, N'Kopernik', N'SIII', 1, CAST(N'2020-09-18T10:45:08.520' AS DateTime), 3, 6775, 4, 224279, 225761)
GO
INSERT [config].[WaterBalanceConfig] ([ID], [ZoneName], [DestinationName], [IsActive], [Timestamp], [ZoneNo], [ModelZoneId], [GisZoneId], [Var1MinId], [Var10MinId]) VALUES (7, N'Piekary', N'SIV', 1, CAST(N'2020-09-18T10:45:03.153' AS DateTime), 4, 6776, 3, 224280, 225762)
GO
INSERT [config].[WaterBalanceConfig] ([ID], [ZoneName], [DestinationName], [IsActive], [Timestamp], [ZoneNo], [ModelZoneId], [GisZoneId], [Var1MinId], [Var10MinId]) VALUES (8, N'Pó³nocna', N'SV', 1, CAST(N'2020-09-18T10:44:45.433' AS DateTime), 5, 6777, 1, 224281, 225763)
GO
INSERT [config].[WaterBalanceConfig] ([ID], [ZoneName], [DestinationName], [IsActive], [Timestamp], [ZoneNo], [ModelZoneId], [GisZoneId], [Var1MinId], [Var10MinId]) VALUES (9, N'ZPW', N'SVI', 1, CAST(N'2020-09-18T10:44:32.813' AS DateTime), 6, 6778, 7, 224282, 225764)
GO
INSERT [config].[WaterBalanceConfig] ([ID], [ZoneName], [DestinationName], [IsActive], [Timestamp], [ZoneNo], [ModelZoneId], [GisZoneId], [Var1MinId], [Var10MinId]) VALUES (11, N'Tranzyt', N'SVII', 1, CAST(N'2020-09-18T10:43:38.753' AS DateTime), 7, 6779, NULL, 224287, 225766)
GO
INSERT [config].[WaterBalanceConfig] ([ID], [ZoneName], [DestinationName], [IsActive], [Timestamp], [ZoneNo], [ModelZoneId], [GisZoneId], [Var1MinId], [Var10MinId]) VALUES (14, N'Zbiorniki', N'SVIII', 1, CAST(N'2020-09-18T10:43:43.133' AS DateTime), 8, 6780, NULL, 224294, 225767)
GO
INSERT [config].[WaterBalanceConfig] ([ID], [ZoneName], [DestinationName], [IsActive], [Timestamp], [ZoneNo], [ModelZoneId], [GisZoneId], [Var1MinId], [Var10MinId]) VALUES (10, N'Huta', N'SIX', 1, CAST(N'2020-09-18T10:47:26.963' AS DateTime), 9, 6781, 5, 224283, 225765)
GO
INSERT [config].[WaterBalanceConfig] ([ID], [ZoneName], [DestinationName], [IsActive], [Timestamp], [ZoneNo], [ModelZoneId], [GisZoneId], [Var1MinId], [Var10MinId]) VALUES (18, N'Pobór - Legnickie Pole', N'QGLP', 1, CAST(N'2020-07-16T12:31:54.300' AS DateTime), NULL, 6783, NULL, 224291, NULL)
GO
INSERT [config].[WaterBalanceConfig] ([ID], [ZoneName], [DestinationName], [IsActive], [Timestamp], [ZoneNo], [ModelZoneId], [GisZoneId], [Var1MinId], [Var10MinId]) VALUES (20, N'Pobór - Krotoszyce', N'QGKR', 1, CAST(N'2020-07-16T12:29:48.963' AS DateTime), NULL, 6784, NULL, 224297, NULL)
GO
INSERT [config].[WaterBalanceConfig] ([ID], [ZoneName], [DestinationName], [IsActive], [Timestamp], [ZoneNo], [ModelZoneId], [GisZoneId], [Var1MinId], [Var10MinId]) VALUES (21, N'Pobór - Mi³kowice', N'QGM', 1, CAST(N'2020-07-16T12:29:21.060' AS DateTime), NULL, 6787, NULL, 224298, NULL)
GO
INSERT [config].[WaterBalanceConfig] ([ID], [ZoneName], [DestinationName], [IsActive], [Timestamp], [ZoneNo], [ModelZoneId], [GisZoneId], [Var1MinId], [Var10MinId]) VALUES (12, N'Magistrala', N'SVIIa', 1, CAST(N'2020-08-05T08:27:10.023' AS DateTime), NULL, 7001, NULL, 224285, NULL)
GO
INSERT [config].[WaterBalanceConfig] ([ID], [ZoneName], [DestinationName], [IsActive], [Timestamp], [ZoneNo], [ModelZoneId], [GisZoneId], [Var1MinId], [Var10MinId]) VALUES (13, N'Magistrala', N'SVIIb', 1, CAST(N'2020-08-05T08:27:29.153' AS DateTime), NULL, 7002, NULL, 224286, NULL)
GO
INSERT [config].[WaterBalanceConfig] ([ID], [ZoneName], [DestinationName], [IsActive], [Timestamp], [ZoneNo], [ModelZoneId], [GisZoneId], [Var1MinId], [Var10MinId]) VALUES (15, N'Ogólny', N'B', 1, CAST(N'2020-07-16T12:38:11.930' AS DateTime), NULL, 7003, NULL, 1173, NULL)
GO
INSERT [config].[WaterBalanceConfig] ([ID], [ZoneName], [DestinationName], [IsActive], [Timestamp], [ZoneNo], [ModelZoneId], [GisZoneId], [Var1MinId], [Var10MinId]) VALUES (16, N'Bilans bez gmin oœciennych', N'BG', 1, CAST(N'2020-07-16T12:31:23.173' AS DateTime), NULL, 7004, NULL, 224292, NULL)
GO
INSERT [config].[WaterBalanceConfig] ([ID], [ZoneName], [DestinationName], [IsActive], [Timestamp], [ZoneNo], [ModelZoneId], [GisZoneId], [Var1MinId], [Var10MinId]) VALUES (17, N'Pobór przez gminy oœcienne', N'QGC', 1, CAST(N'2020-07-16T12:30:08.293' AS DateTime), NULL, 7005, NULL, 224295, NULL)
GO
INSERT [config].[WaterBalanceConfig] ([ID], [ZoneName], [DestinationName], [IsActive], [Timestamp], [ZoneNo], [ModelZoneId], [GisZoneId], [Var1MinId], [Var10MinId]) VALUES (19, N'Pobór - Kunice', N'QGK', 1, CAST(N'2020-07-16T12:30:01.533' AS DateTime), NULL, 7008, NULL, 224296, NULL)
GO
INSERT [config].[WaterBalanceConfig] ([ID], [ZoneName], [DestinationName], [IsActive], [Timestamp], [ZoneNo], [ModelZoneId], [GisZoneId], [Var1MinId], [Var10MinId]) VALUES (22, N'Bilans bez obszarów specjalnych', N'BS', 1, CAST(N'2020-07-16T12:32:37.253' AS DateTime), NULL, 7009, NULL, 224299, NULL)
GO
INSERT [config].[WaterBalanceConfig] ([ID], [ZoneName], [DestinationName], [IsActive], [Timestamp], [ZoneNo], [ModelZoneId], [GisZoneId], [Var1MinId], [Var10MinId]) VALUES (23, N'Pobór przez obszary specjalne', N'QS', 1, CAST(N'2020-07-16T12:32:22.780' AS DateTime), NULL, 7010, NULL, 224300, NULL)
GO
INSERT [config].[WaterBalanceConfig] ([ID], [ZoneName], [DestinationName], [IsActive], [Timestamp], [ZoneNo], [ModelZoneId], [GisZoneId], [Var1MinId], [Var10MinId]) VALUES (24, N'Bilans bez gmin oœciennych i obszarów specjalnych', N'BM', 1, CAST(N'2020-07-16T12:31:04.733' AS DateTime), NULL, 7011, NULL, 224301, NULL)
GO
INSERT [config].[WaterBalanceConfig] ([ID], [ZoneName], [DestinationName], [IsActive], [Timestamp], [ZoneNo], [ModelZoneId], [GisZoneId], [Var1MinId], [Var10MinId]) VALUES (25, N'QZPW', N'QZPW', 1, CAST(N'2020-07-16T12:31:57.637' AS DateTime), NULL, 7012, NULL, 224290, NULL)
GO
INSERT [config].[WaterBalanceConfig] ([ID], [ZoneName], [DestinationName], [IsActive], [Timestamp], [ZoneNo], [ModelZoneId], [GisZoneId], [Var1MinId], [Var10MinId]) VALUES (26, N'Przep³yw do gmin oœciennych', N'QG', 1, CAST(N'2020-07-16T12:30:30.537' AS DateTime), NULL, 7013, NULL, 224293, NULL)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (2, N'SII', 78526.4898099685, 2905, 0, 2015, 10, 4086)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (5, N'SIX', 4337.40698996059, 122, 0, 2015, 10, 174)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (1, N'SV', 28628.2033220978, 1099, 98, 2015, 11, 1629)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (2, N'SII', 78526.4898099685, 2905, 827, 2015, 11, 4086)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (3, N'SIV', 46520.0579671655, 1381, 154, 2015, 11, 2071)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (4, N'SIII', 54602.945564502, 2343, 72, 2015, 11, 3190)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (5, N'SIX', 4337.40698996059, 122, 0, 2015, 11, 174)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (6, N'SI', 19125.3019854906, 453, 400, 2015, 11, 942)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (1, N'SV', 28628.2033220978, 1099, 754, 2015, 12, 1629)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (2, N'SII', 78526.4898099685, 2905, 5463, 2015, 12, 4086)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (3, N'SIV', 46520.0579671655, 1381, 1291, 2015, 12, 2071)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (4, N'SIII', 54602.945564502, 2343, 482, 2015, 12, 3190)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (5, N'SIX', 4337.40698996059, 122, 10, 2015, 12, 174)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (6, N'SI', 19125.3019854906, 453, 1819, 2015, 12, 942)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (7, N'VI', 14686.5557244333, 39, 4, 2015, 12, 161)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (1, N'SV', 28628.2033220978, 1099, 401, 2016, 1, 1629)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (2, N'SII', 78526.4898099685, 2905, 5311, 2016, 1, 4086)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (3, N'SIV', 46520.0579671655, 1381, 50, 2016, 1, 2071)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (4, N'SIII', 54602.945564502, 2343, 238, 2016, 1, 3190)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (6, N'SI', 19125.3019854906, 453, 114, 2016, 1, 942)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (1, N'SV', 28628.2033220978, 1099, 400, 2016, 2, 1629)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (2, N'SII', 78526.4898099685, 2905, 5178, 2016, 2, 4086)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (3, N'SIV', 46520.0579671655, 1381, 60, 2016, 2, 2071)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (4, N'SIII', 54602.945564502, 2343, 252, 2016, 2, 3190)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (5, N'SIX', 4337.40698996059, 122, 16, 2016, 2, 174)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (6, N'SI', 19125.3019854906, 453, 119, 2016, 2, 942)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (1, N'SV', 28628.2033220978, 1099, 462, 2016, 3, 1629)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (2, N'SII', 78526.4898099685, 2905, 7326, 2016, 3, 4086)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (3, N'SIV', 46520.0579671655, 1381, 72, 2016, 3, 2071)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (4, N'SIII', 54602.945564502, 2343, 1176, 2016, 3, 3190)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (5, N'SIX', 4337.40698996059, 122, 28, 2016, 3, 174)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (6, N'SI', 19125.3019854906, 453, 136, 2016, 3, 942)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (1, N'SV', 28628.2033220978, 1099, 520, 2016, 4, 1629)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (2, N'SII', 78526.4898099685, 2905, 10816, 2016, 4, 4086)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (3, N'SIV', 46520.0579671655, 1381, 104, 2016, 4, 2071)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (4, N'SIII', 54602.945564502, 2343, 1652, 2016, 4, 3190)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (5, N'SIX', 4337.40698996059, 122, 40, 2016, 4, 174)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (6, N'SI', 19125.3019854906, 453, 178, 2016, 4, 942)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (1, N'SV', 28628.2033220978, 1099, 434, 2016, 5, 1629)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (2, N'SII', 78526.4898099685, 2905, 10448, 2016, 5, 4086)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (3, N'SIV', 46520.0579671655, 1381, 96, 2016, 5, 2071)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (4, N'SIII', 54602.945564502, 2343, 2638, 2016, 5, 3190)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (5, N'SIX', 4337.40698996059, 122, 68, 2016, 5, 174)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (6, N'SI', 19125.3019854906, 453, 201, 2016, 5, 942)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (1, N'SV', 28628.2033220978, 1099, 516, 2016, 6, 1629)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (2, N'SII', 78526.4898099685, 2905, 10514, 2016, 6, 4086)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (3, N'SIV', 46520.0579671655, 1381, 357, 2016, 6, 2071)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (4, N'SIII', 54602.945564502, 2343, 1680, 2016, 6, 3190)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (5, N'SIX', 4337.40698996059, 122, 1858, 2016, 6, 174)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (6, N'SI', 19125.3019854906, 453, 371, 2016, 6, 942)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (1, N'SV', 28628.2033220978, 1099, 1067, 2016, 7, 1629)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (2, N'SII', 78526.4898099685, 2905, 11301, 2016, 7, 4086)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (3, N'SIV', 46520.0579671655, 1381, 902, 2016, 7, 2071)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (4, N'SIII', 54602.945564502, 2343, 1666, 2016, 7, 3190)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (5, N'SIX', 4337.40698996059, 122, 1962, 2016, 7, 174)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (6, N'SI', 19125.3019854906, 453, 525, 2016, 7, 942)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (1, N'SV', 28628.2033220978, 1099, 1642, 2016, 8, 1629)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (2, N'SII', 78526.4898099685, 2905, 10296, 2016, 8, 4086)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (3, N'SIV', 46520.0579671655, 1381, 1782, 2016, 8, 2071)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (4, N'SIII', 54602.945564502, 2343, 2077, 2016, 8, 3190)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (5, N'SIX', 4337.40698996059, 122, 1940, 2016, 8, 174)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (6, N'SI', 19125.3019854906, 453, 562, 2016, 8, 942)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (1, N'SV', 28628.2033220978, 1099, 2022, 2016, 9, 1629)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (2, N'SII', 78526.4898099685, 2905, 14945, 2016, 9, 4086)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (3, N'SIV', 46520.0579671655, 1381, 3368, 2016, 9, 2071)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (4, N'SIII', 54602.945564502, 2343, 3052, 2016, 9, 3190)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (5, N'SIX', 4337.40698996059, 122, 2376, 2016, 9, 174)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (6, N'SI', 19125.3019854906, 453, 575, 2016, 9, 942)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (1, N'SV', 28628.2033220978, 1099, 2445, 2016, 10, 1629)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (2, N'SII', 78526.4898099685, 2905, 16441, 2016, 10, 4086)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (3, N'SIV', 46520.0579671655, 1381, 4967, 2016, 10, 2071)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (4, N'SIII', 54602.945564502, 2343, 4610, 2016, 10, 3190)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (5, N'SIX', 4337.40698996059, 122, 2568, 2016, 10, 174)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (6, N'SI', 19125.3019854906, 453, 609, 2016, 10, 942)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (1, N'SV', 28628.2033220978, 1099, 2450, 2016, 11, 1629)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (2, N'SII', 78526.4898099685, 2905, 17996, 2016, 11, 4086)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (3, N'SIV', 46520.0579671655, 1381, 5967, 2016, 11, 2071)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (4, N'SIII', 54602.945564502, 2343, 6427, 2016, 11, 3190)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (5, N'SIX', 4337.40698996059, 122, 2414, 2016, 11, 174)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (6, N'SI', 19125.3019854906, 453, 650, 2016, 11, 942)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (1, N'SV', 28628.2033220978, 1099, 2932, 2016, 12, 1629)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (2, N'SII', 78526.4898099685, 2905, 24021, 2016, 12, 4086)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (3, N'SIV', 46520.0579671655, 1381, 7129, 2016, 12, 2071)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (4, N'SIII', 54602.945564502, 2343, 8200, 2016, 12, 3190)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (5, N'SIX', 4337.40698996059, 122, 2292, 2016, 12, 174)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (6, N'SI', 19125.3019854906, 453, 664, 2016, 12, 942)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (1, N'SV', 28628.2033220978, 1099, 3304, 2017, 1, 1629)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (2, N'SII', 78526.4898099685, 2905, 29536, 2017, 1, 4086)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (3, N'SIV', 46520.0579671655, 1381, 7166, 2017, 1, 2071)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (4, N'SIII', 54602.945564502, 2343, 9508, 2017, 1, 3190)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (5, N'SIX', 4337.40698996059, 122, 1595, 2017, 1, 174)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (6, N'SI', 19125.3019854906, 453, 694, 2017, 1, 942)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (7, N'VI', 14686.5557244333, 39, 4, 2017, 1, 161)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (1, N'SV', 28628.2033220978, 1099, 3113, 2017, 2, 1629)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (2, N'SII', 78526.4898099685, 2905, 29299, 2017, 2, 4086)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (3, N'SIV', 46520.0579671655, 1381, 7204, 2017, 2, 2071)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (4, N'SIII', 54602.945564502, 2343, 9706, 2017, 2, 3190)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (5, N'SIX', 4337.40698996059, 122, 2455, 2017, 2, 174)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (6, N'SI', 19125.3019854906, 453, 612, 2017, 2, 942)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (7, N'VI', 14686.5557244333, 39, 7, 2017, 2, 161)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (1, N'SV', 28628.2033220978, 1099, 3345, 2017, 3, 1629)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (2, N'SII', 78526.4898099685, 2905, 32996, 2017, 3, 4086)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (3, N'SIV', 46520.0579671655, 1381, 9022, 2017, 3, 2071)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (4, N'SIII', 54602.945564502, 2343, 10859, 2017, 3, 3190)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (5, N'SIX', 4337.40698996059, 122, 2554, 2017, 3, 174)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (6, N'SI', 19125.3019854906, 453, 720, 2017, 3, 942)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (7, N'VI', 14686.5557244333, 39, 43, 2017, 3, 161)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (1, N'SV', 28628.2033220978, 1099, 3540, 2017, 4, 1629)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (2, N'SII', 78526.4898099685, 2905, 31703, 2017, 4, 4086)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (3, N'SIV', 46520.0579671655, 1381, 9923, 2017, 4, 2071)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (4, N'SIII', 54602.945564502, 2343, 10975, 2017, 4, 3190)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (5, N'SIX', 4337.40698996059, 122, 2350, 2017, 4, 174)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (6, N'SI', 19125.3019854906, 453, 790, 2017, 4, 942)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (7, N'VI', 14686.5557244333, 39, 69, 2017, 4, 161)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (1, N'SV', 28628.2033220978, 1099, 4021, 2017, 5, 1629)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (2, N'SII', 78526.4898099685, 2905, 32932, 2017, 5, 4086)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (3, N'SIV', 46520.0579671655, 1381, 11042, 2017, 5, 2071)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (4, N'SIII', 54602.945564502, 2343, 12402, 2017, 5, 3190)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (5, N'SIX', 4337.40698996059, 122, 1936, 2017, 5, 174)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (6, N'SI', 19125.3019854906, 453, 892, 2017, 5, 942)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (7, N'VI', 14686.5557244333, 39, 49, 2017, 5, 161)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (1, N'SV', 28628.2033220978, 1099, 4104, 2017, 6, 1629)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (2, N'SII', 78526.4898099685, 2905, 33066, 2017, 6, 4086)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (3, N'SIV', 46520.0579671655, 1381, 11244, 2017, 6, 2071)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (4, N'SIII', 54602.945564502, 2343, 14008, 2017, 6, 3190)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (5, N'SIX', 4337.40698996059, 122, 2414, 2017, 6, 174)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (6, N'SI', 19125.3019854906, 453, 990, 2017, 6, 942)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (7, N'VI', 14686.5557244333, 39, 23, 2017, 6, 161)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (1, N'SV', 28628.2033220978, 1099, 4694, 2017, 7, 1629)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (2, N'SII', 78526.4898099685, 2905, 38739, 2017, 7, 4086)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (3, N'SIV', 46520.0579671655, 1381, 11607, 2017, 7, 2071)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (4, N'SIII', 54602.945564502, 2343, 14322, 2017, 7, 3190)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (5, N'SIX', 4337.40698996059, 122, 2610, 2017, 7, 174)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (6, N'SI', 19125.3019854906, 453, 987, 2017, 7, 942)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (7, N'VI', 14686.5557244333, 39, 23, 2017, 7, 161)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (1, N'SV', 28628.2033220978, 1099, 4805, 2017, 8, 1629)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (2, N'SII', 78526.4898099685, 2905, 39911, 2017, 8, 4086)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (3, N'SIV', 46520.0579671655, 1381, 11033, 2017, 8, 2071)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (4, N'SIII', 54602.945564502, 2343, 15931, 2017, 8, 3190)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (5, N'SIX', 4337.40698996059, 122, 2466, 2017, 8, 174)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (6, N'SI', 19125.3019854906, 453, 999, 2017, 8, 942)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (7, N'VI', 14686.5557244333, 39, 23, 2017, 8, 161)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (1, N'SV', 28628.2033220978, 1099, 5093, 2017, 9, 1629)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (2, N'SII', 78526.4898099685, 2905, 44171, 2017, 9, 4086)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (3, N'SIV', 46520.0579671655, 1381, 11149, 2017, 9, 2071)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (4, N'SIII', 54602.945564502, 2343, 18238, 2017, 9, 3190)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (5, N'SIX', 4337.40698996059, 122, 2416, 2017, 9, 174)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (6, N'SI', 19125.3019854906, 453, 1285, 2017, 9, 942)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (7, N'VI', 14686.5557244333, 39, 24, 2017, 9, 161)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (1, N'SV', 28628.2033220978, 1099, 7045, 2017, 10, 1629)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (2, N'SII', 78526.4898099685, 2905, 46906, 2017, 10, 4086)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (3, N'SIV', 46520.0579671655, 1381, 12595, 2017, 10, 2071)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (4, N'SIII', 54602.945564502, 2343, 22788, 2017, 10, 3190)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (5, N'SIX', 4337.40698996059, 122, 2546, 2017, 10, 174)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (6, N'SI', 19125.3019854906, 453, 1783, 2017, 10, 942)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (7, N'VI', 14686.5557244333, 39, 26, 2017, 10, 161)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (1, N'SV', 28628.2033220978, 1099, 8317, 2017, 11, 1629)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (2, N'SII', 78526.4898099685, 2905, 47432, 2017, 11, 4086)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (3, N'SIV', 46520.0579671655, 1381, 11958, 2017, 11, 2071)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (4, N'SIII', 54602.945564502, 2343, 24485, 2017, 11, 3190)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (5, N'SIX', 4337.40698996059, 122, 2396, 2017, 11, 174)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (6, N'SI', 19125.3019854906, 453, 1829, 2017, 11, 942)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (7, N'VI', 14686.5557244333, 39, 23, 2017, 11, 161)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (1, N'SV', 28628.2033220978, 1099, 9476, 2017, 12, 1629)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (2, N'SII', 78526.4898099685, 2905, 52204, 2017, 12, 4086)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (3, N'SIV', 46520.0579671655, 1381, 12782, 2017, 12, 2071)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (4, N'SIII', 54602.945564502, 2343, 26578, 2017, 12, 3190)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (5, N'SIX', 4337.40698996059, 122, 2610, 2017, 12, 174)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (6, N'SI', 19125.3019854906, 453, 2060, 2017, 12, 942)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (7, N'VI', 14686.5557244333, 39, 21, 2017, 12, 161)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (1, N'SV', 28628.2033220978, 1099, 8241, 2018, 1, 1629)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (2, N'SII', 78526.4898099685, 2905, 52914, 2018, 1, 4086)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (3, N'SIV', 46520.0579671655, 1381, 13149, 2018, 1, 2071)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (4, N'SIII', 54602.945564502, 2343, 27128, 2018, 1, 3190)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (5, N'SIX', 4337.40698996059, 122, 2568, 2018, 1, 174)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (6, N'SI', 19125.3019854906, 453, 2022, 2018, 1, 942)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (7, N'VI', 14686.5557244333, 39, 32, 2018, 1, 161)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (1, N'SV', 28628.2033220978, 1099, 7332, 2018, 2, 1629)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (2, N'SII', 78526.4898099685, 2905, 51336, 2018, 2, 4086)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (3, N'SIV', 46520.0579671655, 1381, 12951, 2018, 2, 2071)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (4, N'SIII', 54602.945564502, 2343, 27363, 2018, 2, 3190)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (5, N'SIX', 4337.40698996059, 122, 2408, 2018, 2, 174)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (6, N'SI', 19125.3019854906, 453, 1983, 2018, 2, 942)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (7, N'VI', 14686.5557244333, 39, 36, 2018, 2, 161)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (1, N'SV', 28628.2033220978, 1099, 8871, 2018, 3, 1629)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (2, N'SII', 78526.4898099685, 2905, 58896, 2018, 3, 4086)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (3, N'SIV', 46520.0579671655, 1381, 15427, 2018, 3, 2071)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (4, N'SIII', 54602.945564502, 2343, 33677, 2018, 3, 3190)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (5, N'SIX', 4337.40698996059, 122, 2646, 2018, 3, 174)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (6, N'SI', 19125.3019854906, 453, 2413, 2018, 3, 942)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (7, N'VI', 14686.5557244333, 39, 62, 2018, 3, 161)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (1, N'SV', 28628.2033220978, 1099, 9774, 2018, 4, 1629)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (2, N'SII', 78526.4898099685, 2905, 57373, 2018, 4, 4086)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (3, N'SIV', 46520.0579671655, 1381, 14734, 2018, 4, 2071)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (4, N'SIII', 54602.945564502, 2343, 34372, 2018, 4, 3190)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (5, N'SIX', 4337.40698996059, 122, 2598, 2018, 4, 174)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (6, N'SI', 19125.3019854906, 453, 2343, 2018, 4, 942)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (7, N'VI', 14686.5557244333, 39, 82, 2018, 4, 161)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (1, N'SV', 28628.2033220978, 1099, 11514, 2018, 5, 1629)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (2, N'SII', 78526.4898099685, 2905, 61683, 2018, 5, 4086)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (3, N'SIV', 46520.0579671655, 1381, 17547, 2018, 5, 2071)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (4, N'SIII', 54602.945564502, 2343, 37139, 2018, 5, 3190)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (5, N'SIX', 4337.40698996059, 122, 2728, 2018, 5, 174)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (6, N'SI', 19125.3019854906, 453, 2521, 2018, 5, 942)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (7, N'VI', 14686.5557244333, 39, 90, 2018, 5, 161)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (1, N'SV', 28628.2033220978, 1099, 13175, 2018, 6, 1629)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (2, N'SII', 78526.4898099685, 2905, 64773, 2018, 6, 4086)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (3, N'SIV', 46520.0579671655, 1381, 18233, 2018, 6, 2071)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (4, N'SIII', 54602.945564502, 2343, 40312, 2018, 6, 3190)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (5, N'SIX', 4337.40698996059, 122, 2844, 2018, 6, 174)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (6, N'SI', 19125.3019854906, 453, 2452, 2018, 6, 942)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (7, N'VI', 14686.5557244333, 39, 103, 2018, 6, 161)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (1, N'SV', 28628.2033220978, 1099, 25321, 2018, 7, 1629)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (2, N'SII', 78526.4898099685, 2905, 66609, 2018, 7, 4086)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (3, N'SIV', 46520.0579671655, 1381, 18710, 2018, 7, 2071)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (4, N'SIII', 54602.945564502, 2343, 46085, 2018, 7, 3190)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (5, N'SIX', 4337.40698996059, 122, 2766, 2018, 7, 174)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (6, N'SI', 19125.3019854906, 453, 2672, 2018, 7, 942)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (7, N'VI', 14686.5557244333, 39, 94, 2018, 7, 161)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (1, N'SV', 28628.2033220978, 1099, 29576, 2018, 8, 1629)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (2, N'SII', 78526.4898099685, 2905, 67933, 2018, 8, 4086)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (3, N'SIV', 46520.0579671655, 1381, 16696, 2018, 8, 2071)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (4, N'SIII', 54602.945564502, 2343, 47638, 2018, 8, 3190)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (5, N'SIX', 4337.40698996059, 122, 2830, 2018, 8, 174)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (6, N'SI', 19125.3019854906, 453, 3019, 2018, 8, 942)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (7, N'VI', 14686.5557244333, 39, 82, 2018, 8, 161)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (1, N'SV', 28628.2033220978, 1099, 33041, 2018, 9, 1629)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (2, N'SII', 78526.4898099685, 2905, 71128, 2018, 9, 4086)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (3, N'SIV', 46520.0579671655, 1381, 17990, 2018, 9, 2071)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (4, N'SIII', 54602.945564502, 2343, 48679, 2018, 9, 3190)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (5, N'SIX', 4337.40698996059, 122, 2854, 2018, 9, 174)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (6, N'SI', 19125.3019854906, 453, 3321, 2018, 9, 942)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (7, N'VI', 14686.5557244333, 39, 80, 2018, 9, 161)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (1, N'SV', 28628.2033220978, 1099, 31186, 2018, 10, 1629)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (2, N'SII', 78526.4898099685, 2905, 73859, 2018, 10, 4086)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (3, N'SIV', 46520.0579671655, 1381, 18042, 2018, 10, 2071)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (4, N'SIII', 54602.945564502, 2343, 49154, 2018, 10, 3190)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (5, N'SIX', 4337.40698996059, 122, 3086, 2018, 10, 174)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (6, N'SI', 19125.3019854906, 453, 3338, 2018, 10, 942)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (7, N'VI', 14686.5557244333, 39, 72, 2018, 10, 161)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (1, N'SV', 28628.2033220978, 1099, 32275, 2018, 11, 1629)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (2, N'SII', 78526.4898099685, 2905, 74907, 2018, 11, 4086)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (3, N'SIV', 46520.0579671655, 1381, 18026, 2018, 11, 2071)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (4, N'SIII', 54602.945564502, 2343, 51279, 2018, 11, 3190)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (5, N'SIX', 4337.40698996059, 122, 2850, 2018, 11, 174)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (6, N'SI', 19125.3019854906, 453, 3335, 2018, 11, 942)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (7, N'VI', 14686.5557244333, 39, 91, 2018, 11, 161)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (1, N'SV', 28628.2033220978, 1099, 31353, 2018, 12, 1629)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (2, N'SII', 78526.4898099685, 2905, 80220, 2018, 12, 4086)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (3, N'SIV', 46520.0579671655, 1381, 18736, 2018, 12, 2071)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (4, N'SIII', 54602.945564502, 2343, 52422, 2018, 12, 3190)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (5, N'SIX', 4337.40698996059, 122, 3012, 2018, 12, 174)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (6, N'SI', 19125.3019854906, 453, 13982, 2018, 12, 942)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (7, N'VI', 14686.5557244333, 39, 130, 2018, 12, 161)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (1, N'SV', 28628.2033220978, 1099, 33312, 2019, 1, 1629)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (2, N'SII', 78526.4898099685, 2905, 80491, 2019, 1, 4086)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (3, N'SIV', 46520.0579671655, 1381, 19352, 2019, 1, 2071)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (4, N'SIII', 54602.945564502, 2343, 51927, 2019, 1, 3190)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (5, N'SIX', 4337.40698996059, 122, 3032, 2019, 1, 174)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (6, N'SI', 19125.3019854906, 453, 61439, 2019, 1, 942)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (7, N'VI', 14686.5557244333, 39, 142, 2019, 1, 161)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (1, N'SV', 28628.2033220978, 1099, 33079, 2019, 2, 1629)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (2, N'SII', 78526.4898099685, 2905, 78482, 2019, 2, 4086)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (3, N'SIV', 46520.0579671655, 1381, 17395, 2019, 2, 2071)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (4, N'SIII', 54602.945564502, 2343, 48136, 2019, 2, 3190)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (5, N'SIX', 4337.40698996059, 122, 2648, 2019, 2, 174)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (6, N'SI', 19125.3019854906, 453, 55856, 2019, 2, 942)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (7, N'VI', 14686.5557244333, 39, 120, 2019, 2, 161)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (1, N'SV', 28628.2033220978, 1099, 38988, 2019, 3, 1629)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (2, N'SII', 78526.4898099685, 2905, 88973, 2019, 3, 4086)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (3, N'SIV', 46520.0579671655, 1381, 20424, 2019, 3, 2071)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (4, N'SIII', 54602.945564502, 2343, 54459, 2019, 3, 3190)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (5, N'SIX', 4337.40698996059, 122, 2972, 2019, 3, 174)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (6, N'SI', 19125.3019854906, 453, 63326, 2019, 3, 942)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (7, N'VI', 14686.5557244333, 39, 143, 2019, 3, 161)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (1, N'SV', 28628.2033220978, 1099, 37874, 2019, 4, 1629)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (2, N'SII', 78526.4898099685, 2905, 86546, 2019, 4, 4086)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (3, N'SIV', 46520.0579671655, 1381, 19654, 2019, 4, 2071)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (4, N'SIII', 54602.945564502, 2343, 52886, 2019, 4, 3190)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (5, N'SIX', 4337.40698996059, 122, 2908, 2019, 4, 174)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (6, N'SI', 19125.3019854906, 453, 59598, 2019, 4, 942)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (7, N'VI', 14686.5557244333, 39, 140, 2019, 4, 161)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (1, N'SV', 28628.2033220978, 1099, 41320, 2019, 5, 1629)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (2, N'SII', 78526.4898099685, 2905, 91576, 2019, 5, 4086)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (3, N'SIV', 46520.0579671655, 1381, 21432, 2019, 5, 2071)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (4, N'SIII', 54602.945564502, 2343, 56564, 2019, 5, 3190)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (5, N'SIX', 4337.40698996059, 122, 2930, 2019, 5, 174)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (6, N'SI', 19125.3019854906, 453, 61590, 2019, 5, 942)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (7, N'VI', 14686.5557244333, 39, 166, 2019, 5, 161)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (1, N'SV', 28628.2033220978, 1099, 39752, 2019, 6, 1629)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (2, N'SII', 78526.4898099685, 2905, 94352, 2019, 6, 4086)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (3, N'SIV', 46520.0579671655, 1381, 22733, 2019, 6, 2071)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (4, N'SIII', 54602.945564502, 2343, 59674, 2019, 6, 3190)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (5, N'SIX', 4337.40698996059, 122, 3215, 2019, 6, 174)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (6, N'SI', 19125.3019854906, 453, 69236, 2019, 6, 942)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (7, N'VI', 14686.5557244333, 39, 177, 2019, 6, 161)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (1, N'SV', 28628.2033220978, 1099, 40644, 2019, 7, 1629)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (2, N'SII', 78526.4898099685, 2905, 96606, 2019, 7, 4086)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (3, N'SIV', 46520.0579671655, 1381, 23844, 2019, 7, 2071)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (4, N'SIII', 54602.945564502, 2343, 60313, 2019, 7, 3190)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (5, N'SIX', 4337.40698996059, 122, 3065, 2019, 7, 174)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (6, N'SI', 19125.3019854906, 453, 63915, 2019, 7, 942)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (7, N'VI', 14686.5557244333, 39, 182, 2019, 7, 161)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (1, N'SV', 28628.2033220978, 1099, 36161, 2019, 8, 1629)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (2, N'SII', 78526.4898099685, 2905, 97769, 2019, 8, 4086)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (3, N'SIV', 46520.0579671655, 1381, 23576, 2019, 8, 2071)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (4, N'SIII', 54602.945564502, 2343, 61474, 2019, 8, 3190)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (5, N'SIX', 4337.40698996059, 122, 3163, 2019, 8, 174)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (6, N'SI', 19125.3019854906, 453, 59543, 2019, 8, 942)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (7, N'VI', 14686.5557244333, 39, 192, 2019, 8, 161)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (1, N'SV', 28628.2033220978, 1099, 36699, 2019, 9, 1629)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (2, N'SII', 78526.4898099685, 2905, 102491, 2019, 9, 4086)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (3, N'SIV', 46520.0579671655, 1381, 22141, 2019, 9, 2071)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (4, N'SIII', 54602.945564502, 2343, 60630, 2019, 9, 3190)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (5, N'SIX', 4337.40698996059, 122, 3285, 2019, 9, 174)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (6, N'SI', 19125.3019854906, 453, 65675, 2019, 9, 942)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (7, N'VI', 14686.5557244333, 39, 170, 2019, 9, 161)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (1, N'SV', 28628.2033220978, 1099, 40248, 2019, 10, 1629)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (2, N'SII', 78526.4898099685, 2905, 107030, 2019, 10, 4086)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (3, N'SIV', 46520.0579671655, 1381, 23384, 2019, 10, 2071)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (4, N'SIII', 54602.945564502, 2343, 67201, 2019, 10, 3190)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (5, N'SIX', 4337.40698996059, 122, 3640, 2019, 10, 174)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (6, N'SI', 19125.3019854906, 453, 63610, 2019, 10, 942)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (7, N'VI', 14686.5557244333, 39, 155, 2019, 10, 161)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (1, N'SV', 28628.2033220978, 1099, 39523, 2019, 11, 1629)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (2, N'SII', 78526.4898099685, 2905, 105585, 2019, 11, 4086)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (3, N'SIV', 46520.0579671655, 1381, 22672, 2019, 11, 2071)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (4, N'SIII', 54602.945564502, 2343, 67446, 2019, 11, 3190)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (5, N'SIX', 4337.40698996059, 122, 4402, 2019, 11, 174)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (6, N'SI', 19125.3019854906, 453, 61085, 2019, 11, 942)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (7, N'VI', 14686.5557244333, 39, 153, 2019, 11, 161)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (1, N'SV', 28628.2033220978, 1099, 41651, 2019, 12, 1629)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (2, N'SII', 78526.4898099685, 2905, 112091, 2019, 12, 4086)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (3, N'SIV', 46520.0579671655, 1381, 24723, 2019, 12, 2071)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (4, N'SIII', 54602.945564502, 2343, 70798, 2019, 12, 3190)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (5, N'SIX', 4337.40698996059, 122, 5969, 2019, 12, 174)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (6, N'SI', 19125.3019854906, 453, 52706, 2019, 12, 942)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (7, N'VI', 14686.5557244333, 39, 169, 2019, 12, 161)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (1, N'SV', 28628.2033220978, 1099, 42527, 2020, 1, 1629)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (2, N'SII', 78526.4898099685, 2905, 113366, 2020, 1, 4086)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (3, N'SIV', 46520.0579671655, 1381, 32423, 2020, 1, 2071)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (4, N'SIII', 54602.945564502, 2343, 69878, 2020, 1, 3190)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (5, N'SIX', 4337.40698996059, 122, 6301, 2020, 1, 174)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (6, N'SI', 19125.3019854906, 453, 61456, 2020, 1, 942)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (7, N'VI', 14686.5557244333, 39, 139, 2020, 1, 161)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (1, N'SV', 28628.2033220978, 1099, 42999, 2020, 2, 1629)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (2, N'SII', 78526.4898099685, 2905, 116503, 2020, 2, 4086)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (3, N'SIV', 46520.0579671655, 1381, 44291, 2020, 2, 2071)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (4, N'SIII', 54602.945564502, 2343, 68532, 2020, 2, 3190)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (5, N'SIX', 4337.40698996059, 122, 6126, 2020, 2, 174)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (6, N'SI', 19125.3019854906, 453, 60501, 2020, 2, 942)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (7, N'VI', 14686.5557244333, 39, 138, 2020, 2, 161)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (1, N'SV', 28628.2033220978, 1099, 44085, 2020, 3, 1629)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (2, N'SII', 78526.4898099685, 2905, 127589, 2020, 3, 4086)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (3, N'SIV', 46520.0579671655, 1381, 35644, 2020, 3, 2071)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (4, N'SIII', 54602.945564502, 2343, 74935, 2020, 3, 3190)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (5, N'SIX', 4337.40698996059, 122, 6797, 2020, 3, 174)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (6, N'SI', 19125.3019854906, 453, 63457, 2020, 3, 942)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (7, N'VI', 14686.5557244333, 39, 166, 2020, 3, 161)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (1, N'SV', 28628.2033220978, 1099, 49385, 2020, 4, 1629)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (2, N'SII', 78526.4898099685, 2905, 123776, 2020, 4, 4086)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (3, N'SIV', 46520.0579671655, 1381, 34765, 2020, 4, 2071)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (4, N'SIII', 54602.945564502, 2343, 73735, 2020, 4, 3190)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (5, N'SIX', 4337.40698996059, 122, 6344, 2020, 4, 174)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (6, N'SI', 19125.3019854906, 453, 52562, 2020, 4, 942)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (7, N'VI', 14686.5557244333, 39, 168, 2020, 4, 161)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (1, N'SV', 28628.2033220978, 1099, 49366, 2020, 5, 1629)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (2, N'SII', 78526.4898099685, 2905, 141369, 2020, 5, 4086)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (3, N'SIV', 46520.0579671655, 1381, 37381, 2020, 5, 2071)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (4, N'SIII', 54602.945564502, 2343, 79477, 2020, 5, 3190)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (5, N'SIX', 4337.40698996059, 122, 6702, 2020, 5, 174)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (6, N'SI', 19125.3019854906, 453, 58302, 2020, 5, 942)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (7, N'VI', 14686.5557244333, 39, 175, 2020, 5, 161)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (1, N'SV', 28628.2033220978, 1099, 48513, 2020, 6, 1629)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (2, N'SII', 78526.4898099685, 2905, 151323, 2020, 6, 4086)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (3, N'SIV', 46520.0579671655, 1381, 37460, 2020, 6, 2071)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (4, N'SIII', 54602.945564502, 2343, 85149, 2020, 6, 3190)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (5, N'SIX', 4337.40698996059, 122, 6424, 2020, 6, 174)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (6, N'SI', 19125.3019854906, 453, 59250, 2020, 6, 942)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (7, N'VI', 14686.5557244333, 39, 160, 2020, 6, 161)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (1, N'SV', 28628.2033220978, 1099, 52553, 2020, 7, 1629)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (2, N'SII', 78526.4898099685, 2905, 163951, 2020, 7, 4086)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (3, N'SIV', 46520.0579671655, 1381, 39210, 2020, 7, 2071)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (4, N'SIII', 54602.945564502, 2343, 91277, 2020, 7, 3190)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (5, N'SIX', 4337.40698996059, 122, 6175, 2020, 7, 174)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (6, N'SI', 19125.3019854906, 453, 63811, 2020, 7, 942)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (7, N'VI', 14686.5557244333, 39, 179, 2020, 7, 161)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (1, N'SV', 28628.2033220978, 1099, 48840, 2020, 8, 1629)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (2, N'SII', 78526.4898099685, 2905, 169497, 2020, 8, 4086)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (3, N'SIV', 46520.0579671655, 1381, 37429, 2020, 8, 2071)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (4, N'SIII', 54602.945564502, 2343, 89385, 2020, 8, 3190)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (5, N'SIX', 4337.40698996059, 122, 6283, 2020, 8, 174)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (6, N'SI', 19125.3019854906, 453, 60444, 2020, 8, 942)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (7, N'VI', 14686.5557244333, 39, 137, 2020, 8, 161)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (1, N'SV', 28628.2033220978, 1099, 51634, 2020, 9, 1629)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (2, N'SII', 78526.4898099685, 2905, 170048, 2020, 9, 4086)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (3, N'SIV', 46520.0579671655, 1381, 38587, 2020, 9, 2071)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (4, N'SIII', 54602.945564502, 2343, 81578, 2020, 9, 3190)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (5, N'SIX', 4337.40698996059, 122, 8394, 2020, 9, 174)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (6, N'SI', 19125.3019854906, 453, 63722, 2020, 9, 942)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (7, N'VI', 14686.5557244333, 39, 114, 2020, 9, 161)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (1, N'SV', 28628.2033220978, 1099, 51624, 2020, 10, 1629)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (2, N'SII', 78526.4898099685, 2905, 128633, 2020, 10, 4086)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (3, N'SIV', 46520.0579671655, 1381, 41113, 2020, 10, 2071)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (4, N'SIII', 54602.945564502, 2343, 67522, 2020, 10, 3190)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (5, N'SIX', 4337.40698996059, 122, 10326, 2020, 10, 174)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (6, N'SI', 19125.3019854906, 453, 68677, 2020, 10, 942)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (7, N'VI', 14686.5557244333, 39, 125, 2020, 10, 161)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (1, N'SV', 28628.2033220978, 1099, 44212, 2020, 11, 1629)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (2, N'SII', 78526.4898099685, 2905, 110523, 2020, 11, 4086)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (3, N'SIV', 46520.0579671655, 1381, 40994, 2020, 11, 2071)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (4, N'SIII', 54602.945564502, 2343, 63955, 2020, 11, 3190)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (5, N'SIX', 4337.40698996059, 122, 7500, 2020, 11, 174)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (6, N'SI', 19125.3019854906, 453, 63411, 2020, 11, 942)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (7, N'VI', 14686.5557244333, 39, 138, 2020, 11, 161)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (1, N'SV', 28628.2033220978, 1099, 45338, 2020, 12, 1629)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (2, N'SII', 78526.4898099685, 2905, 118837, 2020, 12, 4086)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (3, N'SIV', 46520.0579671655, 1381, 41857, 2020, 12, 2071)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (4, N'SIII', 54602.945564502, 2343, 68148, 2020, 12, 3190)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (5, N'SIX', 4337.40698996059, 122, 8725, 2020, 12, 174)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (6, N'SI', 19125.3019854906, 453, 66561, 2020, 12, 942)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (7, N'VI', 14686.5557244333, 39, 153, 2020, 12, 161)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (1, N'SV', 28628.2033220978, 1099, 66990, 2021, 1, 1629)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (2, N'SII', 78526.4898099685, 2905, 142176, 2021, 1, 4086)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (3, N'SIV', 46520.0579671655, 1381, 43344, 2021, 1, 2071)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (4, N'SIII', 54602.945564502, 2343, 72555, 2021, 1, 3190)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (5, N'SIX', 4337.40698996059, 122, 10324, 2021, 1, 174)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (6, N'SI', 19125.3019854906, 453, 64508, 2021, 1, 942)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (7, N'VI', 14686.5557244333, 39, 270, 2021, 1, 161)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (1, N'SV', 28628.2033220978, 1099, 62622, 2021, 2, 1629)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (2, N'SII', 78526.4898099685, 2905, 159018, 2021, 2, 4086)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (3, N'SIV', 46520.0579671655, 1381, 44248, 2021, 2, 2071)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (4, N'SIII', 54602.945564502, 2343, 78226, 2021, 2, 3190)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (5, N'SIX', 4337.40698996059, 122, 9359, 2021, 2, 174)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (6, N'SI', 19125.3019854906, 453, 65211, 2021, 2, 942)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (7, N'VI', 14686.5557244333, 39, 308, 2021, 2, 161)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (1, N'SV', 28628.2033220978, 1099, 63549, 2021, 3, 1629)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (2, N'SII', 78526.4898099685, 2905, 182230, 2021, 3, 4086)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (3, N'SIV', 46520.0579671655, 1381, 50282, 2021, 3, 2071)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (4, N'SIII', 54602.945564502, 2343, 92913, 2021, 3, 3190)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (5, N'SIX', 4337.40698996059, 122, 10757, 2021, 3, 174)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (6, N'SI', 19125.3019854906, 453, 70625, 2021, 3, 942)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (7, N'VI', 14686.5557244333, 39, 335, 2021, 3, 161)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (1, N'SV', 28628.2033220978, 1099, 59780, 2021, 4, 1629)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (2, N'SII', 78526.4898099685, 2905, 140948, 2021, 4, 4086)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (3, N'SIV', 46520.0579671655, 1381, 36516, 2021, 4, 2071)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (4, N'SIII', 54602.945564502, 2343, 56173, 2021, 4, 3190)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (5, N'SIX', 4337.40698996059, 122, 8211, 2021, 4, 174)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (6, N'SI', 19125.3019854906, 453, 66727, 2021, 4, 942)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (7, N'VI', 14686.5557244333, 39, 334, 2021, 4, 161)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (1, N'SV', 28628.2033220978, 1099, 10816, 2021, 5, 1629)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (2, N'SII', 78526.4898099685, 2905, 44666, 2021, 5, 4086)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (3, N'SIV', 46520.0579671655, 1381, 6041, 2021, 5, 2071)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (4, N'SIII', 54602.945564502, 2343, 9763, 2021, 5, 3190)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (6, N'SI', 19125.3019854906, 453, 3089, 2021, 5, 942)
GO
INSERT [dbo].[tb_zuzycia_stref] ([id_strefy], [nazwa_strefy], [dlugosc_sieci], [ilosc_przylaczy], [sprzedaz_w_strefie], [year], [month], [ilosc_przylaczy_czynnych]) VALUES (7, N'VI', 14686.5557244333, 39, 138, 2021, 5, 161)
GO
SET IDENTITY_INSERT [dbo].[tbLog] ON 
GO
INSERT [dbo].[tbLog] ([Id], [ExecDate], [ExecTime], [CommandName], [TestDate]) VALUES (1325, CAST(N'2020-12-23T12:31:17.237' AS DateTime), 1, N'77-2020-4-1-0', CAST(N'2020-12-23T12:31:17.237' AS DateTime))
GO
INSERT [dbo].[tbLog] ([Id], [ExecDate], [ExecTime], [CommandName], [TestDate]) VALUES (1326, CAST(N'2020-12-23T12:31:17.237' AS DateTime), 1, N'77-2020-4-1-0', CAST(N'2020-12-23T12:31:17.237' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[tbLog] OFF
GO
INSERT [dbo].[tbMonth] ([MonthId], [MonthName]) VALUES (1, N'1 - January')
GO
INSERT [dbo].[tbMonth] ([MonthId], [MonthName]) VALUES (2, N'2 - February')
GO
INSERT [dbo].[tbMonth] ([MonthId], [MonthName]) VALUES (3, N'3 - March')
GO
INSERT [dbo].[tbMonth] ([MonthId], [MonthName]) VALUES (4, N'4 - April')
GO
INSERT [dbo].[tbMonth] ([MonthId], [MonthName]) VALUES (5, N'5 - May')
GO
INSERT [dbo].[tbMonth] ([MonthId], [MonthName]) VALUES (6, N'6 - June')
GO
INSERT [dbo].[tbMonth] ([MonthId], [MonthName]) VALUES (7, N'7 - July')
GO
INSERT [dbo].[tbMonth] ([MonthId], [MonthName]) VALUES (8, N'8 - August')
GO
INSERT [dbo].[tbMonth] ([MonthId], [MonthName]) VALUES (9, N'9 - September')
GO
INSERT [dbo].[tbMonth] ([MonthId], [MonthName]) VALUES (10, N'10 - October')
GO
INSERT [dbo].[tbMonth] ([MonthId], [MonthName]) VALUES (11, N'11- November')
GO
INSERT [dbo].[tbMonth] ([MonthId], [MonthName]) VALUES (12, N'12 -December')
GO
INSERT [dbo].[tbMonth] ([MonthId], [MonthName]) VALUES (13, N'<Whole Year>')
GO
INSERT [dbo].[tbSetting] ([FinancData_G6], [FinancData_K6], [FinancData_G8], [MatrixOneIn_SelectedOption], [MatrixOneIn_C11], [MatrixOneIn_C12], [MatrixOneIn_C13], [MatrixOneIn_C14], [MatrixOneIn_C21], [MatrixOneIn_C22], [MatrixOneIn_C23], [MatrixOneIn_C24], [MatrixOneIn_D21], [MatrixOneIn_D22], [MatrixOneIn_D23], [MatrixOneIn_D24], [MatrixOneIn_E11], [MatrixOneIn_E12], [MatrixOneIn_E13], [MatrixOneIn_E14], [MatrixOneIn_E21], [MatrixOneIn_E22], [MatrixOneIn_E23], [MatrixOneIn_E24], [MatrixOneIn_F11], [MatrixOneIn_F12], [MatrixOneIn_F13], [MatrixOneIn_F14], [MatrixOneIn_F21], [MatrixOneIn_F22], [MatrixOneIn_F23], [MatrixOneIn_F24], [MatrixOneIn_G11], [MatrixOneIn_G12], [MatrixOneIn_G13], [MatrixOneIn_G14], [MatrixOneIn_G21], [MatrixOneIn_G22], [MatrixOneIn_G23], [MatrixOneIn_G24], [MatrixOneIn_H11], [MatrixOneIn_H12], [MatrixOneIn_H13], [MatrixOneIn_H14], [MatrixOneIn_H21], [MatrixOneIn_H22], [MatrixOneIn_H23], [MatrixOneIn_H24], [MatrixTwoIn_SelectedOption], [MatrixTwoIn_C11], [MatrixTwoIn_C12], [MatrixTwoIn_C13], [MatrixTwoIn_C14], [MatrixTwoIn_C21], [MatrixTwoIn_C22], [MatrixTwoIn_C23], [MatrixTwoIn_C24], [MatrixTwoIn_D21], [MatrixTwoIn_D22], [MatrixTwoIn_D23], [MatrixTwoIn_D24], [MatrixTwoIn_E11], [MatrixTwoIn_E12], [MatrixTwoIn_E13], [MatrixTwoIn_E14], [MatrixTwoIn_E21], [MatrixTwoIn_E22], [MatrixTwoIn_E23], [MatrixTwoIn_E24], [MatrixTwoIn_F11], [MatrixTwoIn_F12], [MatrixTwoIn_F13], [MatrixTwoIn_F14], [MatrixTwoIn_F21], [MatrixTwoIn_F22], [MatrixTwoIn_F23], [MatrixTwoIn_F24], [MatrixTwoIn_G11], [MatrixTwoIn_G12], [MatrixTwoIn_G13], [MatrixTwoIn_G14], [MatrixTwoIn_G21], [MatrixTwoIn_G22], [MatrixTwoIn_G23], [MatrixTwoIn_G24], [MatrixTwoIn_H11], [MatrixTwoIn_H12], [MatrixTwoIn_H13], [MatrixTwoIn_H14], [MatrixTwoIn_H21], [MatrixTwoIn_H22], [MatrixTwoIn_H23], [MatrixTwoIn_H24]) VALUES (222, N'PLNNN', 888, 3, 1.5, 2, 4, 8, 2, 4, 8, 999, 25, 50, 100, 200, 25, 50, 100, 200, 50, 100, 200, 400, 40, 75, 150, 300, 75, 150, 300, 600, 50, 100, 200, 400, 100, 200, 400, 800, 60, 125, 250, 500, 125, 250, 500, 1000, 4, 0, 0, 0, 0, 0, 0, 0, 0, 55, 110, 220, 400, 50, 100, 200, 350, 80, 160, 320, 600, 65, 125, 250, 450, 105, 210, 420, 800, 75, 150, 300, 550, 130, 260, 520, 1000, 85, 175, 350, 650, 155, 310, 620, 1200)
GO
SET IDENTITY_INSERT [dbo].[tbWaterConsumption] ON 
GO
INSERT [dbo].[tbWaterConsumption] ([WaterConsumptionId], [WbEasyCalcDataId], [Description], [WaterConsumptionCategoryId], [WaterConsumptionStatusId], [StartDate], [EndDate], [Latitude], [Lontitude], [RelatedId], [Value]) VALUES (1045, 1104, N'', 1, 1, CAST(N'2021-05-21T12:54:09.857' AS DateTime), CAST(N'2021-05-21T12:54:09.857' AS DateTime), 5671725.89566259, 5581504.3556976, 5450, 1)
GO
INSERT [dbo].[tbWaterConsumption] ([WaterConsumptionId], [WbEasyCalcDataId], [Description], [WaterConsumptionCategoryId], [WaterConsumptionStatusId], [StartDate], [EndDate], [Latitude], [Lontitude], [RelatedId], [Value]) VALUES (1046, 1108, N'', 1, 1, CAST(N'2021-05-25T17:18:34.120' AS DateTime), CAST(N'2021-05-25T17:18:34.120' AS DateTime), 5671925.5026895674, 5580998.4526182469, 5433, 2)
GO
INSERT [dbo].[tbWaterConsumption] ([WaterConsumptionId], [WbEasyCalcDataId], [Description], [WaterConsumptionCategoryId], [WaterConsumptionStatusId], [StartDate], [EndDate], [Latitude], [Lontitude], [RelatedId], [Value]) VALUES (1047, 1109, N'', 1, 1, CAST(N'2021-05-25T17:26:19.743' AS DateTime), CAST(N'2021-05-25T17:26:19.743' AS DateTime), 5671962.0024741329, 5581258.2876476431, 5552, 4)
GO
INSERT [dbo].[tbWaterConsumption] ([WaterConsumptionId], [WbEasyCalcDataId], [Description], [WaterConsumptionCategoryId], [WaterConsumptionStatusId], [StartDate], [EndDate], [Latitude], [Lontitude], [RelatedId], [Value]) VALUES (1048, 1109, N'', 1, 1, CAST(N'2021-05-25T17:46:54.280' AS DateTime), CAST(N'2021-05-25T17:46:54.280' AS DateTime), 5671997.3791884044, 5581057.7531024935, 5546, 3)
GO
INSERT [dbo].[tbWaterConsumption] ([WaterConsumptionId], [WbEasyCalcDataId], [Description], [WaterConsumptionCategoryId], [WaterConsumptionStatusId], [StartDate], [EndDate], [Latitude], [Lontitude], [RelatedId], [Value]) VALUES (1049, 1101, N'', 1, 1, CAST(N'2021-05-25T17:50:10.170' AS DateTime), CAST(N'2021-05-25T17:50:10.170' AS DateTime), 5671726.9438615311, 5581739.9448039858, 5479, 0)
GO
SET IDENTITY_INSERT [dbo].[tbWaterConsumption] OFF
GO
INSERT [dbo].[tbWaterConsumptionCategory] ([WaterConsumptionCategoryId], [WaterConsumptionCategoryName]) VALUES (1, N'Obiekty ZPW')
GO
INSERT [dbo].[tbWaterConsumptionCategory] ([WaterConsumptionCategoryId], [WaterConsumptionCategoryName]) VALUES (2, N'Obiekty Oczyszczalnia')
GO
INSERT [dbo].[tbWaterConsumptionCategory] ([WaterConsumptionCategoryId], [WaterConsumptionCategoryName]) VALUES (3, N'P³ukanie')
GO
INSERT [dbo].[tbWaterConsumptionCategory] ([WaterConsumptionCategoryId], [WaterConsumptionCategoryName]) VALUES (4, N'Badanie Wydajnoœci Hydrandów')
GO
INSERT [dbo].[tbWaterConsumptionCategory] ([WaterConsumptionCategoryId], [WaterConsumptionCategoryName]) VALUES (5, N'Inne Potrzeby W³asne')
GO
INSERT [dbo].[tbWaterConsumptionCategory] ([WaterConsumptionCategoryId], [WaterConsumptionCategoryName]) VALUES (6, N'Star¿ Po¿arna')
GO
INSERT [dbo].[tbWaterConsumptionCategory] ([WaterConsumptionCategoryId], [WaterConsumptionCategoryName]) VALUES (7, N'Wodomierze HH')
GO
INSERT [dbo].[tbWaterConsumptionCategoryStatus] ([CategoryId], [StatusId], [ExcelCellId]) VALUES (1, 1, 6)
GO
INSERT [dbo].[tbWaterConsumptionCategoryStatus] ([CategoryId], [StatusId], [ExcelCellId]) VALUES (1, 2, 5)
GO
INSERT [dbo].[tbWaterConsumptionCategoryStatus] ([CategoryId], [StatusId], [ExcelCellId]) VALUES (1, 3, 6)
GO
INSERT [dbo].[tbWaterConsumptionCategoryStatus] ([CategoryId], [StatusId], [ExcelCellId]) VALUES (1, 4, 6)
GO
INSERT [dbo].[tbWaterConsumptionCategoryStatus] ([CategoryId], [StatusId], [ExcelCellId]) VALUES (1, 5, 7)
GO
INSERT [dbo].[tbWaterConsumptionCategoryStatus] ([CategoryId], [StatusId], [ExcelCellId]) VALUES (1, 6, 6)
GO
INSERT [dbo].[tbWaterConsumptionCategoryStatus] ([CategoryId], [StatusId], [ExcelCellId]) VALUES (2, 1, 6)
GO
INSERT [dbo].[tbWaterConsumptionCategoryStatus] ([CategoryId], [StatusId], [ExcelCellId]) VALUES (2, 2, 5)
GO
INSERT [dbo].[tbWaterConsumptionCategoryStatus] ([CategoryId], [StatusId], [ExcelCellId]) VALUES (2, 3, 6)
GO
INSERT [dbo].[tbWaterConsumptionCategoryStatus] ([CategoryId], [StatusId], [ExcelCellId]) VALUES (2, 4, 6)
GO
INSERT [dbo].[tbWaterConsumptionCategoryStatus] ([CategoryId], [StatusId], [ExcelCellId]) VALUES (2, 5, 7)
GO
INSERT [dbo].[tbWaterConsumptionCategoryStatus] ([CategoryId], [StatusId], [ExcelCellId]) VALUES (2, 6, 6)
GO
INSERT [dbo].[tbWaterConsumptionCategoryStatus] ([CategoryId], [StatusId], [ExcelCellId]) VALUES (3, 1, 6)
GO
INSERT [dbo].[tbWaterConsumptionCategoryStatus] ([CategoryId], [StatusId], [ExcelCellId]) VALUES (3, 2, 5)
GO
INSERT [dbo].[tbWaterConsumptionCategoryStatus] ([CategoryId], [StatusId], [ExcelCellId]) VALUES (3, 3, 6)
GO
INSERT [dbo].[tbWaterConsumptionCategoryStatus] ([CategoryId], [StatusId], [ExcelCellId]) VALUES (3, 4, 6)
GO
INSERT [dbo].[tbWaterConsumptionCategoryStatus] ([CategoryId], [StatusId], [ExcelCellId]) VALUES (3, 5, 7)
GO
INSERT [dbo].[tbWaterConsumptionCategoryStatus] ([CategoryId], [StatusId], [ExcelCellId]) VALUES (3, 6, 6)
GO
INSERT [dbo].[tbWaterConsumptionCategoryStatus] ([CategoryId], [StatusId], [ExcelCellId]) VALUES (4, 1, 6)
GO
INSERT [dbo].[tbWaterConsumptionCategoryStatus] ([CategoryId], [StatusId], [ExcelCellId]) VALUES (4, 2, 5)
GO
INSERT [dbo].[tbWaterConsumptionCategoryStatus] ([CategoryId], [StatusId], [ExcelCellId]) VALUES (4, 3, 6)
GO
INSERT [dbo].[tbWaterConsumptionCategoryStatus] ([CategoryId], [StatusId], [ExcelCellId]) VALUES (4, 4, 6)
GO
INSERT [dbo].[tbWaterConsumptionCategoryStatus] ([CategoryId], [StatusId], [ExcelCellId]) VALUES (4, 5, 7)
GO
INSERT [dbo].[tbWaterConsumptionCategoryStatus] ([CategoryId], [StatusId], [ExcelCellId]) VALUES (4, 6, 6)
GO
INSERT [dbo].[tbWaterConsumptionCategoryStatus] ([CategoryId], [StatusId], [ExcelCellId]) VALUES (5, 1, 6)
GO
INSERT [dbo].[tbWaterConsumptionCategoryStatus] ([CategoryId], [StatusId], [ExcelCellId]) VALUES (5, 2, 5)
GO
INSERT [dbo].[tbWaterConsumptionCategoryStatus] ([CategoryId], [StatusId], [ExcelCellId]) VALUES (5, 3, 6)
GO
INSERT [dbo].[tbWaterConsumptionCategoryStatus] ([CategoryId], [StatusId], [ExcelCellId]) VALUES (5, 4, 6)
GO
INSERT [dbo].[tbWaterConsumptionCategoryStatus] ([CategoryId], [StatusId], [ExcelCellId]) VALUES (5, 5, 7)
GO
INSERT [dbo].[tbWaterConsumptionCategoryStatus] ([CategoryId], [StatusId], [ExcelCellId]) VALUES (5, 6, 6)
GO
INSERT [dbo].[tbWaterConsumptionCategoryStatus] ([CategoryId], [StatusId], [ExcelCellId]) VALUES (6, 2, 1)
GO
INSERT [dbo].[tbWaterConsumptionCategoryStatus] ([CategoryId], [StatusId], [ExcelCellId]) VALUES (6, 3, 4)
GO
INSERT [dbo].[tbWaterConsumptionCategoryStatus] ([CategoryId], [StatusId], [ExcelCellId]) VALUES (7, 2, 1)
GO
INSERT [dbo].[tbWaterConsumptionCategoryStatus] ([CategoryId], [StatusId], [ExcelCellId]) VALUES (7, 3, 4)
GO
INSERT [dbo].[tbWaterConsumptionCategoryStatus] ([CategoryId], [StatusId], [ExcelCellId]) VALUES (7, 4, 3)
GO
INSERT [dbo].[tbWaterConsumptionCategoryStatus] ([CategoryId], [StatusId], [ExcelCellId]) VALUES (7, 6, 2)
GO
INSERT [dbo].[tbWaterConsumptionCategoryStatusExcel] ([ExcelCellId], [ExcelCellName]) VALUES (1, N'BilledCons_BilledMetConsBulkWatSupExpM3_D6')
GO
INSERT [dbo].[tbWaterConsumptionCategoryStatusExcel] ([ExcelCellId], [ExcelCellName]) VALUES (2, N'BilledCons_UnbMetConsM3_D9')
GO
INSERT [dbo].[tbWaterConsumptionCategoryStatusExcel] ([ExcelCellId], [ExcelCellName]) VALUES (3, N'BilledCons_BilledUnmetConsBulkWatSupExpM3_H6')
GO
INSERT [dbo].[tbWaterConsumptionCategoryStatusExcel] ([ExcelCellId], [ExcelCellName]) VALUES (4, N'BilledCons_UnbUnmetConsM3_H8')
GO
INSERT [dbo].[tbWaterConsumptionCategoryStatusExcel] ([ExcelCellId], [ExcelCellName]) VALUES (5, N'UnbilledCons_MetConsBulkWatSupExpM3_D6')
GO
INSERT [dbo].[tbWaterConsumptionCategoryStatusExcel] ([ExcelCellId], [ExcelCellName]) VALUES (6, N'UnbilledCons_UnbMetConsM3_D8')
GO
INSERT [dbo].[tbWaterConsumptionCategoryStatusExcel] ([ExcelCellId], [ExcelCellName]) VALUES (7, N'UnbilledCons_UnbUnmetConsM3_H6')
GO
INSERT [dbo].[tbWaterConsumptionStatus] ([WaterConsumptionStatusId], [WaterConsumptionStatusName]) VALUES (1, N'<Brak>')
GO
INSERT [dbo].[tbWaterConsumptionStatus] ([WaterConsumptionStatusId], [WaterConsumptionStatusName]) VALUES (2, N'Przesy³')
GO
INSERT [dbo].[tbWaterConsumptionStatus] ([WaterConsumptionStatusId], [WaterConsumptionStatusName]) VALUES (3, N'Rycza³t')
GO
INSERT [dbo].[tbWaterConsumptionStatus] ([WaterConsumptionStatusId], [WaterConsumptionStatusName]) VALUES (4, N'Przesy³-Rycza³t')
GO
INSERT [dbo].[tbWaterConsumptionStatus] ([WaterConsumptionStatusId], [WaterConsumptionStatusName]) VALUES (5, N'Szacunek')
GO
INSERT [dbo].[tbWaterConsumptionStatus] ([WaterConsumptionStatusId], [WaterConsumptionStatusName]) VALUES (6, N'Przystawka hydr.')
GO
SET IDENTITY_INSERT [dbo].[tbWbEasyCalcData] ON 
GO
INSERT [dbo].[tbWbEasyCalcData] ([WbEasyCalcDataId], [CreateLogin], [CreateDate], [ModifyLogin], [ModifyDate], [ZoneId], [YearNo], [MonthNo], [Description], [IsArchive], [IsAccepted], [Start_PeriodDays_M21], [SysInput_Desc_B6], [SysInput_Desc_B7], [SysInput_Desc_B8], [SysInput_Desc_B9], [SysInput_SystemInputVolumeM3_D6], [SysInput_SystemInputVolumeError_F6], [SysInput_SystemInputVolumeM3_D7], [SysInput_SystemInputVolumeError_F7], [SysInput_SystemInputVolumeM3_D8], [SysInput_SystemInputVolumeError_F8], [SysInput_SystemInputVolumeM3_D9], [SysInput_SystemInputVolumeError_F9], [BilledCons_Desc_B8], [BilledCons_Desc_B9], [BilledCons_Desc_B10], [BilledCons_Desc_B11], [BilledCons_Desc_F8], [BilledCons_Desc_F9], [BilledCons_Desc_F10], [BilledCons_Desc_F11], [UnbilledCons_Desc_D8], [UnbilledCons_Desc_D9], [UnbilledCons_Desc_D10], [UnbilledCons_Desc_D11], [UnbilledCons_Desc_F6], [UnbilledCons_Desc_F7], [UnbilledCons_Desc_F8], [UnbilledCons_Desc_F9], [UnbilledCons_Desc_F10], [UnbilledCons_Desc_F11], [UnauthCons_Desc_B18], [UnauthCons_Desc_B19], [UnauthCons_Desc_B20], [UnauthCons_Desc_B21], [MetErrors_Desc_D12], [MetErrors_Desc_D13], [MetErrors_Desc_D14], [MetErrors_Desc_D15], [Network_Desc_B7], [Network_Desc_B8], [Network_Desc_B9], [Network_Desc_B10], [Interm_Area_B7], [Interm_Area_B8], [Interm_Area_B9], [Interm_Area_B10], [BilledCons_BilledMetConsBulkWatSupExpM3_D6], [BilledCons_BilledUnmetConsBulkWatSupExpM3_H6], [BilledCons_UnbMetConsM3_D8], [BilledCons_UnbMetConsM3_D9], [BilledCons_UnbMetConsM3_D10], [BilledCons_UnbMetConsM3_D11], [BilledCons_UnbUnmetConsM3_H8], [BilledCons_UnbUnmetConsM3_H9], [BilledCons_UnbUnmetConsM3_H10], [BilledCons_UnbUnmetConsM3_H11], [UnbilledCons_MetConsBulkWatSupExpM3_D6], [UnbilledCons_UnbMetConsM3_D8], [UnbilledCons_UnbMetConsM3_D9], [UnbilledCons_UnbMetConsM3_D10], [UnbilledCons_UnbMetConsM3_D11], [UnbilledCons_UnbUnmetConsM3_H6], [UnbilledCons_UnbUnmetConsM3_H7], [UnbilledCons_UnbUnmetConsM3_H8], [UnbilledCons_UnbUnmetConsM3_H9], [UnbilledCons_UnbUnmetConsM3_H10], [UnbilledCons_UnbUnmetConsM3_H11], [UnbilledCons_UnbUnmetConsError_J6], [UnbilledCons_UnbUnmetConsError_J7], [UnbilledCons_UnbUnmetConsError_J8], [UnbilledCons_UnbUnmetConsError_J9], [UnbilledCons_UnbUnmetConsError_J10], [UnbilledCons_UnbUnmetConsError_J11], [UnauthCons_IllegalConnDomEstNo_D6], [UnauthCons_IllegalConnDomPersPerHouse_H6], [UnauthCons_IllegalConnDomConsLitPerPersDay_J6], [UnauthCons_IllegalConnDomErrorMargin_F6], [UnauthCons_IllegalConnOthersErrorMargin_F10], [IllegalConnectionsOthersEstimatedNumber_D10], [IllegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10], [UnauthCons_MeterTampBypEtcEstNo_D14], [UnauthCons_MeterTampBypEtcErrorMargin_F14], [UnauthCons_MeterTampBypEtcConsLitPerCustDay_J14], [UnauthCons_OthersErrorMargin_F18], [UnauthCons_OthersErrorMargin_F19], [UnauthCons_OthersErrorMargin_F20], [UnauthCons_OthersErrorMargin_F21], [UnauthCons_OthersM3PerDay_J18], [UnauthCons_OthersM3PerDay_J19], [UnauthCons_OthersM3PerDay_J20], [UnauthCons_OthersM3PerDay_J21], [MetErrors_DetailedManualSpec_J6], [MetErrors_BilledMetConsWoBulkSupMetUndrreg_H8], [MetErrors_BilledMetConsWoBulkSupErrorMargin_N8], [MetErrors_Total_F12], [MetErrors_Total_F13], [MetErrors_Total_F14], [MetErrors_Total_F15], [MetErrors_Meter_H12], [MetErrors_Meter_H13], [MetErrors_Meter_H14], [MetErrors_Meter_H15], [MetErrors_Error_N12], [MetErrors_Error_N13], [MetErrors_Error_N14], [MetErrors_Error_N15], [MeteredBulkSupplyExportErrorMargin_N32], [UnbilledMeteredConsumptionWithoutBulkSupplyErrorMargin_N34], [CorruptMeterReadingPracticessErrorMargin_N38], [DataHandlingErrorsOffice_L40], [DataHandlingErrorsOfficeErrorMargin_N40], [MetErrors_MetBulkSupExpMetUnderreg_H32], [MetErrors_UnbillMetConsWoBulkSupplMetUndrreg_H34], [MetErrors_CorruptMetReadPractMetUndrreg_H38], [Network_DistributionAndTransmissionMains_D7], [Network_DistributionAndTransmissionMains_D8], [Network_DistributionAndTransmissionMains_D9], [Network_DistributionAndTransmissionMains_D10], [Network_NoOfConnOfRegCustomers_H10], [Network_NoOfInactAccountsWSvcConns_H18], [Network_AvgLenOfSvcConnFromBoundaryToMeterM_H32], [Network_PossibleUnd_D30], [Network_NoCustomers_H7], [Network_ErrorMargin_J7], [Network_ErrorMargin_J10], [Network_ErrorMargin_J18], [Network_ErrorMargin_J32], [Network_ErrorMargin_D35], [Prs_Area_B7], [Prs_Area_B8], [Prs_Area_B9], [Prs_Area_B10], [Prs_ApproxNoOfConn_D7], [Prs_DailyAvgPrsM_F7], [Prs_ApproxNoOfConn_D8], [Prs_DailyAvgPrsM_F8], [Prs_ApproxNoOfConn_D9], [Prs_DailyAvgPrsM_F9], [Prs_ApproxNoOfConn_D10], [Prs_DailyAvgPrsM_F10], [Prs_ErrorMarg_F26], [Interm_Conn_D7], [Interm_Conn_D8], [Interm_Conn_D9], [Interm_Conn_D10], [Interm_Days_F7], [Interm_Days_F8], [Interm_Days_F9], [Interm_Days_F10], [Interm_Hour_H7], [Interm_Hour_H8], [Interm_Hour_H9], [Interm_Hour_H10], [Interm_ErrorMarg_H26], [FinancData_G6], [FinancData_K6], [FinancData_G8], [FinancData_D26], [FinancData_G35], [MatrixOneIn_SelectedOption], [MatrixOneIn_C11], [MatrixOneIn_C12], [MatrixOneIn_C13], [MatrixOneIn_C14], [MatrixOneIn_C21], [MatrixOneIn_C22], [MatrixOneIn_C23], [MatrixOneIn_C24], [MatrixOneIn_D21], [MatrixOneIn_D22], [MatrixOneIn_D23], [MatrixOneIn_D24], [MatrixOneIn_E11], [MatrixOneIn_E12], [MatrixOneIn_E13], [MatrixOneIn_E14], [MatrixOneIn_E21], [MatrixOneIn_E22], [MatrixOneIn_E23], [MatrixOneIn_E24], [MatrixOneIn_F11], [MatrixOneIn_F12], [MatrixOneIn_F13], [MatrixOneIn_F14], [MatrixOneIn_F21], [MatrixOneIn_F22], [MatrixOneIn_F23], [MatrixOneIn_F24], [MatrixOneIn_G11], [MatrixOneIn_G12], [MatrixOneIn_G13], [MatrixOneIn_G14], [MatrixOneIn_G21], [MatrixOneIn_G22], [MatrixOneIn_G23], [MatrixOneIn_G24], [MatrixOneIn_H11], [MatrixOneIn_H12], [MatrixOneIn_H13], [MatrixOneIn_H14], [MatrixOneIn_H21], [MatrixOneIn_H22], [MatrixOneIn_H23], [MatrixOneIn_H24], [MatrixTwoIn_SelectedOption], [MatrixTwoIn_D21], [MatrixTwoIn_D22], [MatrixTwoIn_D23], [MatrixTwoIn_D24], [MatrixTwoIn_E11], [MatrixTwoIn_E12], [MatrixTwoIn_E13], [MatrixTwoIn_E14], [MatrixTwoIn_E21], [MatrixTwoIn_E22], [MatrixTwoIn_E23], [MatrixTwoIn_E24], [MatrixTwoIn_F11], [MatrixTwoIn_F12], [MatrixTwoIn_F13], [MatrixTwoIn_F14], [MatrixTwoIn_F21], [MatrixTwoIn_F22], [MatrixTwoIn_F23], [MatrixTwoIn_F24], [MatrixTwoIn_G11], [MatrixTwoIn_G12], [MatrixTwoIn_G13], [MatrixTwoIn_G14], [MatrixTwoIn_G21], [MatrixTwoIn_G22], [MatrixTwoIn_G23], [MatrixTwoIn_G24], [MatrixTwoIn_H11], [MatrixTwoIn_H12], [MatrixTwoIn_H13], [MatrixTwoIn_H14], [MatrixTwoIn_H21], [MatrixTwoIn_H22], [MatrixTwoIn_H23], [MatrixTwoIn_H24], [SystemInputVolume_B19], [SystemInputVolumeErrorMargin_B21], [AuthorizedConsumption_K12], [AuthorizedConsumptionErrorMargin_K15], [WaterLosses_K29], [WaterLossesErrorMargin_K31], [BilledAuthorizedConsumption_T8], [UnbilledAuthorizedConsumption_T16], [UnbilledAuthorizedConsumptionErrorMargin_T20], [CommercialLosses_T26], [CommercialLossesErrorMargin_T29], [PhysicalLossesM3_T34], [PhyscialLossesErrorMargin_AH35], [BilledMeteredConsumption_AC4], [BilledUnmeteredConsumption_AC9], [UnbilledMeteredConsumption_AC14], [UnbilledUnmeteredConsumption_AC19], [UnbilledUnmeteredConsumptionErrorMargin_AO20], [UnauthorizedConsumption_AC24], [UnauthorizedConsumptionErrorMargin_AO25], [CustomerMeterInaccuraciesAndErrorsM3_AC29], [CustomerMeterInaccuraciesAndErrorsErrorMargin_AO30], [RevenueWaterM3_AY8], [NonRevenueWaterM3_AY24], [NonRevenueWaterErrorMargin_AY26]) VALUES (1, N'none\none', CAST(N'2021-01-14T09:15:40.203' AS DateTime), N'EMEA\79749', CAST(N'2021-03-24T20:12:48.470' AS DateTime), 6773, 2019, 1, N'Old - first', 1, 1, 30, N'aaaaaa', N'dddddd', N'xxxxxxxxxx', N'nnnnnnnnn', 18000, 0.01, 0, 0, 0, 0, 0, 0, N'qq', N'ww', N'ee', N'rrr', N'tt', N'yy', N'uu', N'ii', N'aa', N'ss', N'dd', N'ff', N'gg', N'hh', N'jj', N'kk', N'll', N'zz', N'zz', N'xx', N'cc', N'vv', N'qqqq', N'www', N'eeee', N'rrrrrr', N'tt', N'yy', N'uu', N'iii', N'uuuuu', N'iiiiiiiii', N'ooooooooop', N'ppppppp', 500, 250, 14000, 220, 0, 0, 75, 0, 0, 0, 150, 300, 0, 0, 0, 100, 0, 0, 0, 0, 0, 0.05, 0, 0, 0, 0, 0, 20, 3, 120, 0.2, 0.1, 5, 500, 30, 0.2, 160, 0.4, 0, 0, 0, 3, 0, 0, 0, 1, 0.02, 0.05, 10000, 2000, 1000, 0, 0.03, 0.04, 0.05, 0, 0.1, 0.1, 0.1, 0, 0.05, 0.1, 0.02, 50, 0.02, 0.02, 0.05, 0.03, 24.2, 0, 0, 0, 700, 15, 10, 0.01, 758, 0, 0.01, 0.05, 0.05, 0.22, N'adddddddddd', N'gzzzzzzzzzzzzz', N'eggggggggg', N'jjjjjjjjjjjj', 740, 37.5, 0, 0, 0, 0, 0, 0, 0.03, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, N'PLN', 1, 10, 20000, 1, 1.5, 2, 4, 8, 2, 4, 8, 16, 25, 50, 100, 200, 25, 50, 100, 200, 50, 100, 200, 400, 40, 75, 150, 300, 75, 150, 300, 600, 50, 100, 200, 400, 100, 200, 400, 800, 60, 125, 250, 500, 125, 250, 500, 1000, 2, 55, 110, 220, 400, 50, 100, 200, 350, 80, 160, 320, 600, 65, 125, 250, 450, 105, 210, 420, 800, 75, 150, 300, 550, 130, 260, 520, 1000, 85, 175, 350, 650, 155, 310, 620, 1200, 18000, 0.01, 15595, 0.00032061558191728119, 2405, 0.074872944305069064, 15045, 550, 0.00909090909090909, 1359.5681397898293, 0.048505884633143863, 1045.4318602101707, 0.18343186759183736, 14720, 325, 450, 100, 0.05, 525, 0.12118732337558567, 834.56813978982927, 0.020791322514394753, 15045, 2955, 0.060913705583756347)
GO
INSERT [dbo].[tbWbEasyCalcData] ([WbEasyCalcDataId], [CreateLogin], [CreateDate], [ModifyLogin], [ModifyDate], [ZoneId], [YearNo], [MonthNo], [Description], [IsArchive], [IsAccepted], [Start_PeriodDays_M21], [SysInput_Desc_B6], [SysInput_Desc_B7], [SysInput_Desc_B8], [SysInput_Desc_B9], [SysInput_SystemInputVolumeM3_D6], [SysInput_SystemInputVolumeError_F6], [SysInput_SystemInputVolumeM3_D7], [SysInput_SystemInputVolumeError_F7], [SysInput_SystemInputVolumeM3_D8], [SysInput_SystemInputVolumeError_F8], [SysInput_SystemInputVolumeM3_D9], [SysInput_SystemInputVolumeError_F9], [BilledCons_Desc_B8], [BilledCons_Desc_B9], [BilledCons_Desc_B10], [BilledCons_Desc_B11], [BilledCons_Desc_F8], [BilledCons_Desc_F9], [BilledCons_Desc_F10], [BilledCons_Desc_F11], [UnbilledCons_Desc_D8], [UnbilledCons_Desc_D9], [UnbilledCons_Desc_D10], [UnbilledCons_Desc_D11], [UnbilledCons_Desc_F6], [UnbilledCons_Desc_F7], [UnbilledCons_Desc_F8], [UnbilledCons_Desc_F9], [UnbilledCons_Desc_F10], [UnbilledCons_Desc_F11], [UnauthCons_Desc_B18], [UnauthCons_Desc_B19], [UnauthCons_Desc_B20], [UnauthCons_Desc_B21], [MetErrors_Desc_D12], [MetErrors_Desc_D13], [MetErrors_Desc_D14], [MetErrors_Desc_D15], [Network_Desc_B7], [Network_Desc_B8], [Network_Desc_B9], [Network_Desc_B10], [Interm_Area_B7], [Interm_Area_B8], [Interm_Area_B9], [Interm_Area_B10], [BilledCons_BilledMetConsBulkWatSupExpM3_D6], [BilledCons_BilledUnmetConsBulkWatSupExpM3_H6], [BilledCons_UnbMetConsM3_D8], [BilledCons_UnbMetConsM3_D9], [BilledCons_UnbMetConsM3_D10], [BilledCons_UnbMetConsM3_D11], [BilledCons_UnbUnmetConsM3_H8], [BilledCons_UnbUnmetConsM3_H9], [BilledCons_UnbUnmetConsM3_H10], [BilledCons_UnbUnmetConsM3_H11], [UnbilledCons_MetConsBulkWatSupExpM3_D6], [UnbilledCons_UnbMetConsM3_D8], [UnbilledCons_UnbMetConsM3_D9], [UnbilledCons_UnbMetConsM3_D10], [UnbilledCons_UnbMetConsM3_D11], [UnbilledCons_UnbUnmetConsM3_H6], [UnbilledCons_UnbUnmetConsM3_H7], [UnbilledCons_UnbUnmetConsM3_H8], [UnbilledCons_UnbUnmetConsM3_H9], [UnbilledCons_UnbUnmetConsM3_H10], [UnbilledCons_UnbUnmetConsM3_H11], [UnbilledCons_UnbUnmetConsError_J6], [UnbilledCons_UnbUnmetConsError_J7], [UnbilledCons_UnbUnmetConsError_J8], [UnbilledCons_UnbUnmetConsError_J9], [UnbilledCons_UnbUnmetConsError_J10], [UnbilledCons_UnbUnmetConsError_J11], [UnauthCons_IllegalConnDomEstNo_D6], [UnauthCons_IllegalConnDomPersPerHouse_H6], [UnauthCons_IllegalConnDomConsLitPerPersDay_J6], [UnauthCons_IllegalConnDomErrorMargin_F6], [UnauthCons_IllegalConnOthersErrorMargin_F10], [IllegalConnectionsOthersEstimatedNumber_D10], [IllegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10], [UnauthCons_MeterTampBypEtcEstNo_D14], [UnauthCons_MeterTampBypEtcErrorMargin_F14], [UnauthCons_MeterTampBypEtcConsLitPerCustDay_J14], [UnauthCons_OthersErrorMargin_F18], [UnauthCons_OthersErrorMargin_F19], [UnauthCons_OthersErrorMargin_F20], [UnauthCons_OthersErrorMargin_F21], [UnauthCons_OthersM3PerDay_J18], [UnauthCons_OthersM3PerDay_J19], [UnauthCons_OthersM3PerDay_J20], [UnauthCons_OthersM3PerDay_J21], [MetErrors_DetailedManualSpec_J6], [MetErrors_BilledMetConsWoBulkSupMetUndrreg_H8], [MetErrors_BilledMetConsWoBulkSupErrorMargin_N8], [MetErrors_Total_F12], [MetErrors_Total_F13], [MetErrors_Total_F14], [MetErrors_Total_F15], [MetErrors_Meter_H12], [MetErrors_Meter_H13], [MetErrors_Meter_H14], [MetErrors_Meter_H15], [MetErrors_Error_N12], [MetErrors_Error_N13], [MetErrors_Error_N14], [MetErrors_Error_N15], [MeteredBulkSupplyExportErrorMargin_N32], [UnbilledMeteredConsumptionWithoutBulkSupplyErrorMargin_N34], [CorruptMeterReadingPracticessErrorMargin_N38], [DataHandlingErrorsOffice_L40], [DataHandlingErrorsOfficeErrorMargin_N40], [MetErrors_MetBulkSupExpMetUnderreg_H32], [MetErrors_UnbillMetConsWoBulkSupplMetUndrreg_H34], [MetErrors_CorruptMetReadPractMetUndrreg_H38], [Network_DistributionAndTransmissionMains_D7], [Network_DistributionAndTransmissionMains_D8], [Network_DistributionAndTransmissionMains_D9], [Network_DistributionAndTransmissionMains_D10], [Network_NoOfConnOfRegCustomers_H10], [Network_NoOfInactAccountsWSvcConns_H18], [Network_AvgLenOfSvcConnFromBoundaryToMeterM_H32], [Network_PossibleUnd_D30], [Network_NoCustomers_H7], [Network_ErrorMargin_J7], [Network_ErrorMargin_J10], [Network_ErrorMargin_J18], [Network_ErrorMargin_J32], [Network_ErrorMargin_D35], [Prs_Area_B7], [Prs_Area_B8], [Prs_Area_B9], [Prs_Area_B10], [Prs_ApproxNoOfConn_D7], [Prs_DailyAvgPrsM_F7], [Prs_ApproxNoOfConn_D8], [Prs_DailyAvgPrsM_F8], [Prs_ApproxNoOfConn_D9], [Prs_DailyAvgPrsM_F9], [Prs_ApproxNoOfConn_D10], [Prs_DailyAvgPrsM_F10], [Prs_ErrorMarg_F26], [Interm_Conn_D7], [Interm_Conn_D8], [Interm_Conn_D9], [Interm_Conn_D10], [Interm_Days_F7], [Interm_Days_F8], [Interm_Days_F9], [Interm_Days_F10], [Interm_Hour_H7], [Interm_Hour_H8], [Interm_Hour_H9], [Interm_Hour_H10], [Interm_ErrorMarg_H26], [FinancData_G6], [FinancData_K6], [FinancData_G8], [FinancData_D26], [FinancData_G35], [MatrixOneIn_SelectedOption], [MatrixOneIn_C11], [MatrixOneIn_C12], [MatrixOneIn_C13], [MatrixOneIn_C14], [MatrixOneIn_C21], [MatrixOneIn_C22], [MatrixOneIn_C23], [MatrixOneIn_C24], [MatrixOneIn_D21], [MatrixOneIn_D22], [MatrixOneIn_D23], [MatrixOneIn_D24], [MatrixOneIn_E11], [MatrixOneIn_E12], [MatrixOneIn_E13], [MatrixOneIn_E14], [MatrixOneIn_E21], [MatrixOneIn_E22], [MatrixOneIn_E23], [MatrixOneIn_E24], [MatrixOneIn_F11], [MatrixOneIn_F12], [MatrixOneIn_F13], [MatrixOneIn_F14], [MatrixOneIn_F21], [MatrixOneIn_F22], [MatrixOneIn_F23], [MatrixOneIn_F24], [MatrixOneIn_G11], [MatrixOneIn_G12], [MatrixOneIn_G13], [MatrixOneIn_G14], [MatrixOneIn_G21], [MatrixOneIn_G22], [MatrixOneIn_G23], [MatrixOneIn_G24], [MatrixOneIn_H11], [MatrixOneIn_H12], [MatrixOneIn_H13], [MatrixOneIn_H14], [MatrixOneIn_H21], [MatrixOneIn_H22], [MatrixOneIn_H23], [MatrixOneIn_H24], [MatrixTwoIn_SelectedOption], [MatrixTwoIn_D21], [MatrixTwoIn_D22], [MatrixTwoIn_D23], [MatrixTwoIn_D24], [MatrixTwoIn_E11], [MatrixTwoIn_E12], [MatrixTwoIn_E13], [MatrixTwoIn_E14], [MatrixTwoIn_E21], [MatrixTwoIn_E22], [MatrixTwoIn_E23], [MatrixTwoIn_E24], [MatrixTwoIn_F11], [MatrixTwoIn_F12], [MatrixTwoIn_F13], [MatrixTwoIn_F14], [MatrixTwoIn_F21], [MatrixTwoIn_F22], [MatrixTwoIn_F23], [MatrixTwoIn_F24], [MatrixTwoIn_G11], [MatrixTwoIn_G12], [MatrixTwoIn_G13], [MatrixTwoIn_G14], [MatrixTwoIn_G21], [MatrixTwoIn_G22], [MatrixTwoIn_G23], [MatrixTwoIn_G24], [MatrixTwoIn_H11], [MatrixTwoIn_H12], [MatrixTwoIn_H13], [MatrixTwoIn_H14], [MatrixTwoIn_H21], [MatrixTwoIn_H22], [MatrixTwoIn_H23], [MatrixTwoIn_H24], [SystemInputVolume_B19], [SystemInputVolumeErrorMargin_B21], [AuthorizedConsumption_K12], [AuthorizedConsumptionErrorMargin_K15], [WaterLosses_K29], [WaterLossesErrorMargin_K31], [BilledAuthorizedConsumption_T8], [UnbilledAuthorizedConsumption_T16], [UnbilledAuthorizedConsumptionErrorMargin_T20], [CommercialLosses_T26], [CommercialLossesErrorMargin_T29], [PhysicalLossesM3_T34], [PhyscialLossesErrorMargin_AH35], [BilledMeteredConsumption_AC4], [BilledUnmeteredConsumption_AC9], [UnbilledMeteredConsumption_AC14], [UnbilledUnmeteredConsumption_AC19], [UnbilledUnmeteredConsumptionErrorMargin_AO20], [UnauthorizedConsumption_AC24], [UnauthorizedConsumptionErrorMargin_AO25], [CustomerMeterInaccuraciesAndErrorsM3_AC29], [CustomerMeterInaccuraciesAndErrorsErrorMargin_AO30], [RevenueWaterM3_AY8], [NonRevenueWaterM3_AY24], [NonRevenueWaterErrorMargin_AY26]) VALUES (9, N'none\none', CAST(N'2021-01-14T09:15:40.203' AS DateTime), N'EMEA\79749', CAST(N'2021-02-04T19:11:14.307' AS DateTime), 6773, 2019, 1, N'Old', 0, 0, 30, NULL, NULL, NULL, NULL, 6593339, 0.05, 10, 0.1, 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 5332026, 1000, 0, 0, 0, 0, 0, 0, 0, 0, 309349, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 3, 120, 0.05, 0, 0, 0, 1000, 0.1, 160, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0.03, 0.02, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0.03, 0.03, 0.03, 260, 0, 0, 0, 8000, 1500, 7, 0, 0, 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, 9500, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, N'0', 0, 0, 0, 1, 1.5, 2, 4, 8, 2, 4, 8, 16, 25, 50, 100, 200, 25, 50, 100, 200, 50, 100, 200, 400, 40, 75, 150, 300, 75, 150, 300, 600, 50, 100, 200, 400, 100, 200, 400, 800, 60, 125, 250, 500, 125, 250, 500, 1000, 2, 55, 110, 220, 400, 50, 100, 200, 350, 80, 160, 320, 600, 65, 125, 250, 450, 105, 210, 420, 800, 75, 150, 300, 550, 130, 260, 520, 1000, 85, 175, 350, 650, 155, 310, 620, 1200, 6593349, 0.049999924166234284, 5642375, 0, 950974, 0.3466624218974616, 5333026, 309349, 0, 345294.46391752549, 0.001398887036941496, 605679.53608247451, 0.54429328420081236, 5332026, 1000, 309349, 0, 0, 5880, 0.082147610459505027, 339414.46391752549, 0, 5333026, 1260323, 0.261573382380165)
GO
INSERT [dbo].[tbWbEasyCalcData] ([WbEasyCalcDataId], [CreateLogin], [CreateDate], [ModifyLogin], [ModifyDate], [ZoneId], [YearNo], [MonthNo], [Description], [IsArchive], [IsAccepted], [Start_PeriodDays_M21], [SysInput_Desc_B6], [SysInput_Desc_B7], [SysInput_Desc_B8], [SysInput_Desc_B9], [SysInput_SystemInputVolumeM3_D6], [SysInput_SystemInputVolumeError_F6], [SysInput_SystemInputVolumeM3_D7], [SysInput_SystemInputVolumeError_F7], [SysInput_SystemInputVolumeM3_D8], [SysInput_SystemInputVolumeError_F8], [SysInput_SystemInputVolumeM3_D9], [SysInput_SystemInputVolumeError_F9], [BilledCons_Desc_B8], [BilledCons_Desc_B9], [BilledCons_Desc_B10], [BilledCons_Desc_B11], [BilledCons_Desc_F8], [BilledCons_Desc_F9], [BilledCons_Desc_F10], [BilledCons_Desc_F11], [UnbilledCons_Desc_D8], [UnbilledCons_Desc_D9], [UnbilledCons_Desc_D10], [UnbilledCons_Desc_D11], [UnbilledCons_Desc_F6], [UnbilledCons_Desc_F7], [UnbilledCons_Desc_F8], [UnbilledCons_Desc_F9], [UnbilledCons_Desc_F10], [UnbilledCons_Desc_F11], [UnauthCons_Desc_B18], [UnauthCons_Desc_B19], [UnauthCons_Desc_B20], [UnauthCons_Desc_B21], [MetErrors_Desc_D12], [MetErrors_Desc_D13], [MetErrors_Desc_D14], [MetErrors_Desc_D15], [Network_Desc_B7], [Network_Desc_B8], [Network_Desc_B9], [Network_Desc_B10], [Interm_Area_B7], [Interm_Area_B8], [Interm_Area_B9], [Interm_Area_B10], [BilledCons_BilledMetConsBulkWatSupExpM3_D6], [BilledCons_BilledUnmetConsBulkWatSupExpM3_H6], [BilledCons_UnbMetConsM3_D8], [BilledCons_UnbMetConsM3_D9], [BilledCons_UnbMetConsM3_D10], [BilledCons_UnbMetConsM3_D11], [BilledCons_UnbUnmetConsM3_H8], [BilledCons_UnbUnmetConsM3_H9], [BilledCons_UnbUnmetConsM3_H10], [BilledCons_UnbUnmetConsM3_H11], [UnbilledCons_MetConsBulkWatSupExpM3_D6], [UnbilledCons_UnbMetConsM3_D8], [UnbilledCons_UnbMetConsM3_D9], [UnbilledCons_UnbMetConsM3_D10], [UnbilledCons_UnbMetConsM3_D11], [UnbilledCons_UnbUnmetConsM3_H6], [UnbilledCons_UnbUnmetConsM3_H7], [UnbilledCons_UnbUnmetConsM3_H8], [UnbilledCons_UnbUnmetConsM3_H9], [UnbilledCons_UnbUnmetConsM3_H10], [UnbilledCons_UnbUnmetConsM3_H11], [UnbilledCons_UnbUnmetConsError_J6], [UnbilledCons_UnbUnmetConsError_J7], [UnbilledCons_UnbUnmetConsError_J8], [UnbilledCons_UnbUnmetConsError_J9], [UnbilledCons_UnbUnmetConsError_J10], [UnbilledCons_UnbUnmetConsError_J11], [UnauthCons_IllegalConnDomEstNo_D6], [UnauthCons_IllegalConnDomPersPerHouse_H6], [UnauthCons_IllegalConnDomConsLitPerPersDay_J6], [UnauthCons_IllegalConnDomErrorMargin_F6], [UnauthCons_IllegalConnOthersErrorMargin_F10], [IllegalConnectionsOthersEstimatedNumber_D10], [IllegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10], [UnauthCons_MeterTampBypEtcEstNo_D14], [UnauthCons_MeterTampBypEtcErrorMargin_F14], [UnauthCons_MeterTampBypEtcConsLitPerCustDay_J14], [UnauthCons_OthersErrorMargin_F18], [UnauthCons_OthersErrorMargin_F19], [UnauthCons_OthersErrorMargin_F20], [UnauthCons_OthersErrorMargin_F21], [UnauthCons_OthersM3PerDay_J18], [UnauthCons_OthersM3PerDay_J19], [UnauthCons_OthersM3PerDay_J20], [UnauthCons_OthersM3PerDay_J21], [MetErrors_DetailedManualSpec_J6], [MetErrors_BilledMetConsWoBulkSupMetUndrreg_H8], [MetErrors_BilledMetConsWoBulkSupErrorMargin_N8], [MetErrors_Total_F12], [MetErrors_Total_F13], [MetErrors_Total_F14], [MetErrors_Total_F15], [MetErrors_Meter_H12], [MetErrors_Meter_H13], [MetErrors_Meter_H14], [MetErrors_Meter_H15], [MetErrors_Error_N12], [MetErrors_Error_N13], [MetErrors_Error_N14], [MetErrors_Error_N15], [MeteredBulkSupplyExportErrorMargin_N32], [UnbilledMeteredConsumptionWithoutBulkSupplyErrorMargin_N34], [CorruptMeterReadingPracticessErrorMargin_N38], [DataHandlingErrorsOffice_L40], [DataHandlingErrorsOfficeErrorMargin_N40], [MetErrors_MetBulkSupExpMetUnderreg_H32], [MetErrors_UnbillMetConsWoBulkSupplMetUndrreg_H34], [MetErrors_CorruptMetReadPractMetUndrreg_H38], [Network_DistributionAndTransmissionMains_D7], [Network_DistributionAndTransmissionMains_D8], [Network_DistributionAndTransmissionMains_D9], [Network_DistributionAndTransmissionMains_D10], [Network_NoOfConnOfRegCustomers_H10], [Network_NoOfInactAccountsWSvcConns_H18], [Network_AvgLenOfSvcConnFromBoundaryToMeterM_H32], [Network_PossibleUnd_D30], [Network_NoCustomers_H7], [Network_ErrorMargin_J7], [Network_ErrorMargin_J10], [Network_ErrorMargin_J18], [Network_ErrorMargin_J32], [Network_ErrorMargin_D35], [Prs_Area_B7], [Prs_Area_B8], [Prs_Area_B9], [Prs_Area_B10], [Prs_ApproxNoOfConn_D7], [Prs_DailyAvgPrsM_F7], [Prs_ApproxNoOfConn_D8], [Prs_DailyAvgPrsM_F8], [Prs_ApproxNoOfConn_D9], [Prs_DailyAvgPrsM_F9], [Prs_ApproxNoOfConn_D10], [Prs_DailyAvgPrsM_F10], [Prs_ErrorMarg_F26], [Interm_Conn_D7], [Interm_Conn_D8], [Interm_Conn_D9], [Interm_Conn_D10], [Interm_Days_F7], [Interm_Days_F8], [Interm_Days_F9], [Interm_Days_F10], [Interm_Hour_H7], [Interm_Hour_H8], [Interm_Hour_H9], [Interm_Hour_H10], [Interm_ErrorMarg_H26], [FinancData_G6], [FinancData_K6], [FinancData_G8], [FinancData_D26], [FinancData_G35], [MatrixOneIn_SelectedOption], [MatrixOneIn_C11], [MatrixOneIn_C12], [MatrixOneIn_C13], [MatrixOneIn_C14], [MatrixOneIn_C21], [MatrixOneIn_C22], [MatrixOneIn_C23], [MatrixOneIn_C24], [MatrixOneIn_D21], [MatrixOneIn_D22], [MatrixOneIn_D23], [MatrixOneIn_D24], [MatrixOneIn_E11], [MatrixOneIn_E12], [MatrixOneIn_E13], [MatrixOneIn_E14], [MatrixOneIn_E21], [MatrixOneIn_E22], [MatrixOneIn_E23], [MatrixOneIn_E24], [MatrixOneIn_F11], [MatrixOneIn_F12], [MatrixOneIn_F13], [MatrixOneIn_F14], [MatrixOneIn_F21], [MatrixOneIn_F22], [MatrixOneIn_F23], [MatrixOneIn_F24], [MatrixOneIn_G11], [MatrixOneIn_G12], [MatrixOneIn_G13], [MatrixOneIn_G14], [MatrixOneIn_G21], [MatrixOneIn_G22], [MatrixOneIn_G23], [MatrixOneIn_G24], [MatrixOneIn_H11], [MatrixOneIn_H12], [MatrixOneIn_H13], [MatrixOneIn_H14], [MatrixOneIn_H21], [MatrixOneIn_H22], [MatrixOneIn_H23], [MatrixOneIn_H24], [MatrixTwoIn_SelectedOption], [MatrixTwoIn_D21], [MatrixTwoIn_D22], [MatrixTwoIn_D23], [MatrixTwoIn_D24], [MatrixTwoIn_E11], [MatrixTwoIn_E12], [MatrixTwoIn_E13], [MatrixTwoIn_E14], [MatrixTwoIn_E21], [MatrixTwoIn_E22], [MatrixTwoIn_E23], [MatrixTwoIn_E24], [MatrixTwoIn_F11], [MatrixTwoIn_F12], [MatrixTwoIn_F13], [MatrixTwoIn_F14], [MatrixTwoIn_F21], [MatrixTwoIn_F22], [MatrixTwoIn_F23], [MatrixTwoIn_F24], [MatrixTwoIn_G11], [MatrixTwoIn_G12], [MatrixTwoIn_G13], [MatrixTwoIn_G14], [MatrixTwoIn_G21], [MatrixTwoIn_G22], [MatrixTwoIn_G23], [MatrixTwoIn_G24], [MatrixTwoIn_H11], [MatrixTwoIn_H12], [MatrixTwoIn_H13], [MatrixTwoIn_H14], [MatrixTwoIn_H21], [MatrixTwoIn_H22], [MatrixTwoIn_H23], [MatrixTwoIn_H24], [SystemInputVolume_B19], [SystemInputVolumeErrorMargin_B21], [AuthorizedConsumption_K12], [AuthorizedConsumptionErrorMargin_K15], [WaterLosses_K29], [WaterLossesErrorMargin_K31], [BilledAuthorizedConsumption_T8], [UnbilledAuthorizedConsumption_T16], [UnbilledAuthorizedConsumptionErrorMargin_T20], [CommercialLosses_T26], [CommercialLossesErrorMargin_T29], [PhysicalLossesM3_T34], [PhyscialLossesErrorMargin_AH35], [BilledMeteredConsumption_AC4], [BilledUnmeteredConsumption_AC9], [UnbilledMeteredConsumption_AC14], [UnbilledUnmeteredConsumption_AC19], [UnbilledUnmeteredConsumptionErrorMargin_AO20], [UnauthorizedConsumption_AC24], [UnauthorizedConsumptionErrorMargin_AO25], [CustomerMeterInaccuraciesAndErrorsM3_AC29], [CustomerMeterInaccuraciesAndErrorsErrorMargin_AO30], [RevenueWaterM3_AY8], [NonRevenueWaterM3_AY24], [NonRevenueWaterErrorMargin_AY26]) VALUES (10, N'none\none', CAST(N'2021-01-14T09:15:40.203' AS DateTime), N'EMEA\79749', CAST(N'2021-03-15T13:50:51.860' AS DateTime), 6773, 2019, 1, N'Old', 0, 0, 10, NULL, NULL, NULL, NULL, 6593339, 0.05, 0, 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 5332026, 1000, 0, 0, 0, 0, 0, 0, 0, 0, 309349, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 3, 120, 0.05, 0, 0, 0, 1000, 0.1, 160, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0.03, 0.02, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0.03, 0.03, 0.03, 260, 0, 0, 0, 8000, 1500, 7, 0, 0, 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, 9500, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, N'0', 0, 0, 0, 1, 1.5, 2, 4, 8, 2, 4, 8, 16, 25, 50, 100, 200, 25, 50, 100, 200, 50, 100, 200, 400, 40, 75, 150, 300, 75, 150, 300, 600, 50, 100, 200, 400, 100, 200, 400, 800, 60, 125, 250, 500, 125, 250, 500, 1000, 2, 55, 110, 220, 400, 50, 100, 200, 350, 80, 160, 320, 600, 65, 125, 250, 450, 105, 210, 420, 800, 75, 150, 300, 550, 130, 260, 520, 1000, 85, 175, 350, 650, 155, 310, 620, 1200, 6593339, 0.05, 5642375, 0, 950964, 0.34666606727489158, 5333026, 309349, 0, 341374.46391752549, 0.00047165014820654242, 609589.53608247451, 0.54080158829016556, 5332026, 1000, 309349, 0, 0, 1960, 0.082147610459505027, 339414.46391752549, 0, 5333026, 1260313, 0.26157545784261532)
GO
INSERT [dbo].[tbWbEasyCalcData] ([WbEasyCalcDataId], [CreateLogin], [CreateDate], [ModifyLogin], [ModifyDate], [ZoneId], [YearNo], [MonthNo], [Description], [IsArchive], [IsAccepted], [Start_PeriodDays_M21], [SysInput_Desc_B6], [SysInput_Desc_B7], [SysInput_Desc_B8], [SysInput_Desc_B9], [SysInput_SystemInputVolumeM3_D6], [SysInput_SystemInputVolumeError_F6], [SysInput_SystemInputVolumeM3_D7], [SysInput_SystemInputVolumeError_F7], [SysInput_SystemInputVolumeM3_D8], [SysInput_SystemInputVolumeError_F8], [SysInput_SystemInputVolumeM3_D9], [SysInput_SystemInputVolumeError_F9], [BilledCons_Desc_B8], [BilledCons_Desc_B9], [BilledCons_Desc_B10], [BilledCons_Desc_B11], [BilledCons_Desc_F8], [BilledCons_Desc_F9], [BilledCons_Desc_F10], [BilledCons_Desc_F11], [UnbilledCons_Desc_D8], [UnbilledCons_Desc_D9], [UnbilledCons_Desc_D10], [UnbilledCons_Desc_D11], [UnbilledCons_Desc_F6], [UnbilledCons_Desc_F7], [UnbilledCons_Desc_F8], [UnbilledCons_Desc_F9], [UnbilledCons_Desc_F10], [UnbilledCons_Desc_F11], [UnauthCons_Desc_B18], [UnauthCons_Desc_B19], [UnauthCons_Desc_B20], [UnauthCons_Desc_B21], [MetErrors_Desc_D12], [MetErrors_Desc_D13], [MetErrors_Desc_D14], [MetErrors_Desc_D15], [Network_Desc_B7], [Network_Desc_B8], [Network_Desc_B9], [Network_Desc_B10], [Interm_Area_B7], [Interm_Area_B8], [Interm_Area_B9], [Interm_Area_B10], [BilledCons_BilledMetConsBulkWatSupExpM3_D6], [BilledCons_BilledUnmetConsBulkWatSupExpM3_H6], [BilledCons_UnbMetConsM3_D8], [BilledCons_UnbMetConsM3_D9], [BilledCons_UnbMetConsM3_D10], [BilledCons_UnbMetConsM3_D11], [BilledCons_UnbUnmetConsM3_H8], [BilledCons_UnbUnmetConsM3_H9], [BilledCons_UnbUnmetConsM3_H10], [BilledCons_UnbUnmetConsM3_H11], [UnbilledCons_MetConsBulkWatSupExpM3_D6], [UnbilledCons_UnbMetConsM3_D8], [UnbilledCons_UnbMetConsM3_D9], [UnbilledCons_UnbMetConsM3_D10], [UnbilledCons_UnbMetConsM3_D11], [UnbilledCons_UnbUnmetConsM3_H6], [UnbilledCons_UnbUnmetConsM3_H7], [UnbilledCons_UnbUnmetConsM3_H8], [UnbilledCons_UnbUnmetConsM3_H9], [UnbilledCons_UnbUnmetConsM3_H10], [UnbilledCons_UnbUnmetConsM3_H11], [UnbilledCons_UnbUnmetConsError_J6], [UnbilledCons_UnbUnmetConsError_J7], [UnbilledCons_UnbUnmetConsError_J8], [UnbilledCons_UnbUnmetConsError_J9], [UnbilledCons_UnbUnmetConsError_J10], [UnbilledCons_UnbUnmetConsError_J11], [UnauthCons_IllegalConnDomEstNo_D6], [UnauthCons_IllegalConnDomPersPerHouse_H6], [UnauthCons_IllegalConnDomConsLitPerPersDay_J6], [UnauthCons_IllegalConnDomErrorMargin_F6], [UnauthCons_IllegalConnOthersErrorMargin_F10], [IllegalConnectionsOthersEstimatedNumber_D10], [IllegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10], [UnauthCons_MeterTampBypEtcEstNo_D14], [UnauthCons_MeterTampBypEtcErrorMargin_F14], [UnauthCons_MeterTampBypEtcConsLitPerCustDay_J14], [UnauthCons_OthersErrorMargin_F18], [UnauthCons_OthersErrorMargin_F19], [UnauthCons_OthersErrorMargin_F20], [UnauthCons_OthersErrorMargin_F21], [UnauthCons_OthersM3PerDay_J18], [UnauthCons_OthersM3PerDay_J19], [UnauthCons_OthersM3PerDay_J20], [UnauthCons_OthersM3PerDay_J21], [MetErrors_DetailedManualSpec_J6], [MetErrors_BilledMetConsWoBulkSupMetUndrreg_H8], [MetErrors_BilledMetConsWoBulkSupErrorMargin_N8], [MetErrors_Total_F12], [MetErrors_Total_F13], [MetErrors_Total_F14], [MetErrors_Total_F15], [MetErrors_Meter_H12], [MetErrors_Meter_H13], [MetErrors_Meter_H14], [MetErrors_Meter_H15], [MetErrors_Error_N12], [MetErrors_Error_N13], [MetErrors_Error_N14], [MetErrors_Error_N15], [MeteredBulkSupplyExportErrorMargin_N32], [UnbilledMeteredConsumptionWithoutBulkSupplyErrorMargin_N34], [CorruptMeterReadingPracticessErrorMargin_N38], [DataHandlingErrorsOffice_L40], [DataHandlingErrorsOfficeErrorMargin_N40], [MetErrors_MetBulkSupExpMetUnderreg_H32], [MetErrors_UnbillMetConsWoBulkSupplMetUndrreg_H34], [MetErrors_CorruptMetReadPractMetUndrreg_H38], [Network_DistributionAndTransmissionMains_D7], [Network_DistributionAndTransmissionMains_D8], [Network_DistributionAndTransmissionMains_D9], [Network_DistributionAndTransmissionMains_D10], [Network_NoOfConnOfRegCustomers_H10], [Network_NoOfInactAccountsWSvcConns_H18], [Network_AvgLenOfSvcConnFromBoundaryToMeterM_H32], [Network_PossibleUnd_D30], [Network_NoCustomers_H7], [Network_ErrorMargin_J7], [Network_ErrorMargin_J10], [Network_ErrorMargin_J18], [Network_ErrorMargin_J32], [Network_ErrorMargin_D35], [Prs_Area_B7], [Prs_Area_B8], [Prs_Area_B9], [Prs_Area_B10], [Prs_ApproxNoOfConn_D7], [Prs_DailyAvgPrsM_F7], [Prs_ApproxNoOfConn_D8], [Prs_DailyAvgPrsM_F8], [Prs_ApproxNoOfConn_D9], [Prs_DailyAvgPrsM_F9], [Prs_ApproxNoOfConn_D10], [Prs_DailyAvgPrsM_F10], [Prs_ErrorMarg_F26], [Interm_Conn_D7], [Interm_Conn_D8], [Interm_Conn_D9], [Interm_Conn_D10], [Interm_Days_F7], [Interm_Days_F8], [Interm_Days_F9], [Interm_Days_F10], [Interm_Hour_H7], [Interm_Hour_H8], [Interm_Hour_H9], [Interm_Hour_H10], [Interm_ErrorMarg_H26], [FinancData_G6], [FinancData_K6], [FinancData_G8], [FinancData_D26], [FinancData_G35], [MatrixOneIn_SelectedOption], [MatrixOneIn_C11], [MatrixOneIn_C12], [MatrixOneIn_C13], [MatrixOneIn_C14], [MatrixOneIn_C21], [MatrixOneIn_C22], [MatrixOneIn_C23], [MatrixOneIn_C24], [MatrixOneIn_D21], [MatrixOneIn_D22], [MatrixOneIn_D23], [MatrixOneIn_D24], [MatrixOneIn_E11], [MatrixOneIn_E12], [MatrixOneIn_E13], [MatrixOneIn_E14], [MatrixOneIn_E21], [MatrixOneIn_E22], [MatrixOneIn_E23], [MatrixOneIn_E24], [MatrixOneIn_F11], [MatrixOneIn_F12], [MatrixOneIn_F13], [MatrixOneIn_F14], [MatrixOneIn_F21], [MatrixOneIn_F22], [MatrixOneIn_F23], [MatrixOneIn_F24], [MatrixOneIn_G11], [MatrixOneIn_G12], [MatrixOneIn_G13], [MatrixOneIn_G14], [MatrixOneIn_G21], [MatrixOneIn_G22], [MatrixOneIn_G23], [MatrixOneIn_G24], [MatrixOneIn_H11], [MatrixOneIn_H12], [MatrixOneIn_H13], [MatrixOneIn_H14], [MatrixOneIn_H21], [MatrixOneIn_H22], [MatrixOneIn_H23], [MatrixOneIn_H24], [MatrixTwoIn_SelectedOption], [MatrixTwoIn_D21], [MatrixTwoIn_D22], [MatrixTwoIn_D23], [MatrixTwoIn_D24], [MatrixTwoIn_E11], [MatrixTwoIn_E12], [MatrixTwoIn_E13], [MatrixTwoIn_E14], [MatrixTwoIn_E21], [MatrixTwoIn_E22], [MatrixTwoIn_E23], [MatrixTwoIn_E24], [MatrixTwoIn_F11], [MatrixTwoIn_F12], [MatrixTwoIn_F13], [MatrixTwoIn_F14], [MatrixTwoIn_F21], [MatrixTwoIn_F22], [MatrixTwoIn_F23], [MatrixTwoIn_F24], [MatrixTwoIn_G11], [MatrixTwoIn_G12], [MatrixTwoIn_G13], [MatrixTwoIn_G14], [MatrixTwoIn_G21], [MatrixTwoIn_G22], [MatrixTwoIn_G23], [MatrixTwoIn_G24], [MatrixTwoIn_H11], [MatrixTwoIn_H12], [MatrixTwoIn_H13], [MatrixTwoIn_H14], [MatrixTwoIn_H21], [MatrixTwoIn_H22], [MatrixTwoIn_H23], [MatrixTwoIn_H24], [SystemInputVolume_B19], [SystemInputVolumeErrorMargin_B21], [AuthorizedConsumption_K12], [AuthorizedConsumptionErrorMargin_K15], [WaterLosses_K29], [WaterLossesErrorMargin_K31], [BilledAuthorizedConsumption_T8], [UnbilledAuthorizedConsumption_T16], [UnbilledAuthorizedConsumptionErrorMargin_T20], [CommercialLosses_T26], [CommercialLossesErrorMargin_T29], [PhysicalLossesM3_T34], [PhyscialLossesErrorMargin_AH35], [BilledMeteredConsumption_AC4], [BilledUnmeteredConsumption_AC9], [UnbilledMeteredConsumption_AC14], [UnbilledUnmeteredConsumption_AC19], [UnbilledUnmeteredConsumptionErrorMargin_AO20], [UnauthorizedConsumption_AC24], [UnauthorizedConsumptionErrorMargin_AO25], [CustomerMeterInaccuraciesAndErrorsM3_AC29], [CustomerMeterInaccuraciesAndErrorsErrorMargin_AO30], [RevenueWaterM3_AY8], [NonRevenueWaterM3_AY24], [NonRevenueWaterErrorMargin_AY26]) VALUES (11, N'none\none', CAST(N'2021-01-14T09:15:40.203' AS DateTime), N'EMEA\79749', CAST(N'2021-05-12T12:00:33.680' AS DateTime), 6773, 2019, 1, N'Old', 0, 0, 11, NULL, NULL, NULL, NULL, 6593339, 0.05, 0, 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 3243, 1000, 0, 0, 0, 0, 0, 0, 0, 0, 309349, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 3, 120, 0.05, 0, 0, 0, 1000, 0.1, 160, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0.03, 0.02, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0.03, 0.03, 0.03, 260, 0, 0, 0, 8000, 1500, 7, 0, 0, 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, 9500, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, N'0', 0, 0, 0, 1, 1.5, 2, 4, 8, 2, 4, 8, 16, 25, 50, 100, 200, 25, 50, 100, 200, 50, 100, 200, 400, 40, 75, 150, 300, 75, 150, 300, 600, 50, 100, 200, 400, 100, 200, 400, 800, 60, 125, 250, 500, 125, 250, 500, 1000, 2, 55, 110, 220, 400, 50, 100, 200, 350, 80, 160, 320, 600, 65, 125, 250, 450, 105, 210, 420, 800, 75, 150, 300, 550, 130, 260, 520, 1000, 85, 175, 350, 650, 155, 310, 620, 1200, 6593339, 0.05, 313592, 0, 6279747, 0.05249685218210224, 4243, 309349, 0, 11955.020618556693, 0.014814717080101118, 6267791.979381443, 0.052596990879694, 3243, 1000, 309349, 0, 0, 2156, 0.082147610459505041, 9799.0206185566931, 0, 4243, 6589096, 0.050032197132960275)
GO
INSERT [dbo].[tbWbEasyCalcData] ([WbEasyCalcDataId], [CreateLogin], [CreateDate], [ModifyLogin], [ModifyDate], [ZoneId], [YearNo], [MonthNo], [Description], [IsArchive], [IsAccepted], [Start_PeriodDays_M21], [SysInput_Desc_B6], [SysInput_Desc_B7], [SysInput_Desc_B8], [SysInput_Desc_B9], [SysInput_SystemInputVolumeM3_D6], [SysInput_SystemInputVolumeError_F6], [SysInput_SystemInputVolumeM3_D7], [SysInput_SystemInputVolumeError_F7], [SysInput_SystemInputVolumeM3_D8], [SysInput_SystemInputVolumeError_F8], [SysInput_SystemInputVolumeM3_D9], [SysInput_SystemInputVolumeError_F9], [BilledCons_Desc_B8], [BilledCons_Desc_B9], [BilledCons_Desc_B10], [BilledCons_Desc_B11], [BilledCons_Desc_F8], [BilledCons_Desc_F9], [BilledCons_Desc_F10], [BilledCons_Desc_F11], [UnbilledCons_Desc_D8], [UnbilledCons_Desc_D9], [UnbilledCons_Desc_D10], [UnbilledCons_Desc_D11], [UnbilledCons_Desc_F6], [UnbilledCons_Desc_F7], [UnbilledCons_Desc_F8], [UnbilledCons_Desc_F9], [UnbilledCons_Desc_F10], [UnbilledCons_Desc_F11], [UnauthCons_Desc_B18], [UnauthCons_Desc_B19], [UnauthCons_Desc_B20], [UnauthCons_Desc_B21], [MetErrors_Desc_D12], [MetErrors_Desc_D13], [MetErrors_Desc_D14], [MetErrors_Desc_D15], [Network_Desc_B7], [Network_Desc_B8], [Network_Desc_B9], [Network_Desc_B10], [Interm_Area_B7], [Interm_Area_B8], [Interm_Area_B9], [Interm_Area_B10], [BilledCons_BilledMetConsBulkWatSupExpM3_D6], [BilledCons_BilledUnmetConsBulkWatSupExpM3_H6], [BilledCons_UnbMetConsM3_D8], [BilledCons_UnbMetConsM3_D9], [BilledCons_UnbMetConsM3_D10], [BilledCons_UnbMetConsM3_D11], [BilledCons_UnbUnmetConsM3_H8], [BilledCons_UnbUnmetConsM3_H9], [BilledCons_UnbUnmetConsM3_H10], [BilledCons_UnbUnmetConsM3_H11], [UnbilledCons_MetConsBulkWatSupExpM3_D6], [UnbilledCons_UnbMetConsM3_D8], [UnbilledCons_UnbMetConsM3_D9], [UnbilledCons_UnbMetConsM3_D10], [UnbilledCons_UnbMetConsM3_D11], [UnbilledCons_UnbUnmetConsM3_H6], [UnbilledCons_UnbUnmetConsM3_H7], [UnbilledCons_UnbUnmetConsM3_H8], [UnbilledCons_UnbUnmetConsM3_H9], [UnbilledCons_UnbUnmetConsM3_H10], [UnbilledCons_UnbUnmetConsM3_H11], [UnbilledCons_UnbUnmetConsError_J6], [UnbilledCons_UnbUnmetConsError_J7], [UnbilledCons_UnbUnmetConsError_J8], [UnbilledCons_UnbUnmetConsError_J9], [UnbilledCons_UnbUnmetConsError_J10], [UnbilledCons_UnbUnmetConsError_J11], [UnauthCons_IllegalConnDomEstNo_D6], [UnauthCons_IllegalConnDomPersPerHouse_H6], [UnauthCons_IllegalConnDomConsLitPerPersDay_J6], [UnauthCons_IllegalConnDomErrorMargin_F6], [UnauthCons_IllegalConnOthersErrorMargin_F10], [IllegalConnectionsOthersEstimatedNumber_D10], [IllegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10], [UnauthCons_MeterTampBypEtcEstNo_D14], [UnauthCons_MeterTampBypEtcErrorMargin_F14], [UnauthCons_MeterTampBypEtcConsLitPerCustDay_J14], [UnauthCons_OthersErrorMargin_F18], [UnauthCons_OthersErrorMargin_F19], [UnauthCons_OthersErrorMargin_F20], [UnauthCons_OthersErrorMargin_F21], [UnauthCons_OthersM3PerDay_J18], [UnauthCons_OthersM3PerDay_J19], [UnauthCons_OthersM3PerDay_J20], [UnauthCons_OthersM3PerDay_J21], [MetErrors_DetailedManualSpec_J6], [MetErrors_BilledMetConsWoBulkSupMetUndrreg_H8], [MetErrors_BilledMetConsWoBulkSupErrorMargin_N8], [MetErrors_Total_F12], [MetErrors_Total_F13], [MetErrors_Total_F14], [MetErrors_Total_F15], [MetErrors_Meter_H12], [MetErrors_Meter_H13], [MetErrors_Meter_H14], [MetErrors_Meter_H15], [MetErrors_Error_N12], [MetErrors_Error_N13], [MetErrors_Error_N14], [MetErrors_Error_N15], [MeteredBulkSupplyExportErrorMargin_N32], [UnbilledMeteredConsumptionWithoutBulkSupplyErrorMargin_N34], [CorruptMeterReadingPracticessErrorMargin_N38], [DataHandlingErrorsOffice_L40], [DataHandlingErrorsOfficeErrorMargin_N40], [MetErrors_MetBulkSupExpMetUnderreg_H32], [MetErrors_UnbillMetConsWoBulkSupplMetUndrreg_H34], [MetErrors_CorruptMetReadPractMetUndrreg_H38], [Network_DistributionAndTransmissionMains_D7], [Network_DistributionAndTransmissionMains_D8], [Network_DistributionAndTransmissionMains_D9], [Network_DistributionAndTransmissionMains_D10], [Network_NoOfConnOfRegCustomers_H10], [Network_NoOfInactAccountsWSvcConns_H18], [Network_AvgLenOfSvcConnFromBoundaryToMeterM_H32], [Network_PossibleUnd_D30], [Network_NoCustomers_H7], [Network_ErrorMargin_J7], [Network_ErrorMargin_J10], [Network_ErrorMargin_J18], [Network_ErrorMargin_J32], [Network_ErrorMargin_D35], [Prs_Area_B7], [Prs_Area_B8], [Prs_Area_B9], [Prs_Area_B10], [Prs_ApproxNoOfConn_D7], [Prs_DailyAvgPrsM_F7], [Prs_ApproxNoOfConn_D8], [Prs_DailyAvgPrsM_F8], [Prs_ApproxNoOfConn_D9], [Prs_DailyAvgPrsM_F9], [Prs_ApproxNoOfConn_D10], [Prs_DailyAvgPrsM_F10], [Prs_ErrorMarg_F26], [Interm_Conn_D7], [Interm_Conn_D8], [Interm_Conn_D9], [Interm_Conn_D10], [Interm_Days_F7], [Interm_Days_F8], [Interm_Days_F9], [Interm_Days_F10], [Interm_Hour_H7], [Interm_Hour_H8], [Interm_Hour_H9], [Interm_Hour_H10], [Interm_ErrorMarg_H26], [FinancData_G6], [FinancData_K6], [FinancData_G8], [FinancData_D26], [FinancData_G35], [MatrixOneIn_SelectedOption], [MatrixOneIn_C11], [MatrixOneIn_C12], [MatrixOneIn_C13], [MatrixOneIn_C14], [MatrixOneIn_C21], [MatrixOneIn_C22], [MatrixOneIn_C23], [MatrixOneIn_C24], [MatrixOneIn_D21], [MatrixOneIn_D22], [MatrixOneIn_D23], [MatrixOneIn_D24], [MatrixOneIn_E11], [MatrixOneIn_E12], [MatrixOneIn_E13], [MatrixOneIn_E14], [MatrixOneIn_E21], [MatrixOneIn_E22], [MatrixOneIn_E23], [MatrixOneIn_E24], [MatrixOneIn_F11], [MatrixOneIn_F12], [MatrixOneIn_F13], [MatrixOneIn_F14], [MatrixOneIn_F21], [MatrixOneIn_F22], [MatrixOneIn_F23], [MatrixOneIn_F24], [MatrixOneIn_G11], [MatrixOneIn_G12], [MatrixOneIn_G13], [MatrixOneIn_G14], [MatrixOneIn_G21], [MatrixOneIn_G22], [MatrixOneIn_G23], [MatrixOneIn_G24], [MatrixOneIn_H11], [MatrixOneIn_H12], [MatrixOneIn_H13], [MatrixOneIn_H14], [MatrixOneIn_H21], [MatrixOneIn_H22], [MatrixOneIn_H23], [MatrixOneIn_H24], [MatrixTwoIn_SelectedOption], [MatrixTwoIn_D21], [MatrixTwoIn_D22], [MatrixTwoIn_D23], [MatrixTwoIn_D24], [MatrixTwoIn_E11], [MatrixTwoIn_E12], [MatrixTwoIn_E13], [MatrixTwoIn_E14], [MatrixTwoIn_E21], [MatrixTwoIn_E22], [MatrixTwoIn_E23], [MatrixTwoIn_E24], [MatrixTwoIn_F11], [MatrixTwoIn_F12], [MatrixTwoIn_F13], [MatrixTwoIn_F14], [MatrixTwoIn_F21], [MatrixTwoIn_F22], [MatrixTwoIn_F23], [MatrixTwoIn_F24], [MatrixTwoIn_G11], [MatrixTwoIn_G12], [MatrixTwoIn_G13], [MatrixTwoIn_G14], [MatrixTwoIn_G21], [MatrixTwoIn_G22], [MatrixTwoIn_G23], [MatrixTwoIn_G24], [MatrixTwoIn_H11], [MatrixTwoIn_H12], [MatrixTwoIn_H13], [MatrixTwoIn_H14], [MatrixTwoIn_H21], [MatrixTwoIn_H22], [MatrixTwoIn_H23], [MatrixTwoIn_H24], [SystemInputVolume_B19], [SystemInputVolumeErrorMargin_B21], [AuthorizedConsumption_K12], [AuthorizedConsumptionErrorMargin_K15], [WaterLosses_K29], [WaterLossesErrorMargin_K31], [BilledAuthorizedConsumption_T8], [UnbilledAuthorizedConsumption_T16], [UnbilledAuthorizedConsumptionErrorMargin_T20], [CommercialLosses_T26], [CommercialLossesErrorMargin_T29], [PhysicalLossesM3_T34], [PhyscialLossesErrorMargin_AH35], [BilledMeteredConsumption_AC4], [BilledUnmeteredConsumption_AC9], [UnbilledMeteredConsumption_AC14], [UnbilledUnmeteredConsumption_AC19], [UnbilledUnmeteredConsumptionErrorMargin_AO20], [UnauthorizedConsumption_AC24], [UnauthorizedConsumptionErrorMargin_AO25], [CustomerMeterInaccuraciesAndErrorsM3_AC29], [CustomerMeterInaccuraciesAndErrorsErrorMargin_AO30], [RevenueWaterM3_AY8], [NonRevenueWaterM3_AY24], [NonRevenueWaterErrorMargin_AY26]) VALUES (12, N'none\none', CAST(N'2021-01-14T09:15:40.203' AS DateTime), N'EMEA\79749', CAST(N'2021-05-12T13:36:02.650' AS DateTime), 6774, 2019, 1, N'Old', 1, 0, 12, NULL, NULL, NULL, NULL, 6593339, 0.05, 0, 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 5332026, 1000, 0, 0, 0, 0, 0, 0, 0, 0, 309349, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 3, 120, 0.05, 0, 0, 0, 1000, 0.1, 160, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0.03, 0.02, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0.03, 0.03, 0.03, 260, 0, 0, 0, 8000, 1500, 7, 0, 0, 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, 9500, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, N'0', 0, 0, 0, 1, 1.5, 2, 4, 8, 2, 4, 8, 16, 25, 50, 100, 200, 25, 50, 100, 200, 50, 100, 200, 400, 40, 75, 150, 300, 75, 150, 300, 600, 50, 100, 200, 400, 100, 200, 400, 800, 60, 125, 250, 500, 125, 250, 500, 1000, 2, 55, 110, 220, 400, 50, 100, 200, 350, 80, 160, 320, 600, 65, 125, 250, 450, 105, 210, 420, 800, 75, 150, 300, 550, 130, 260, 520, 1000, 85, 175, 350, 650, 155, 310, 620, 1200, 6593339, 0.05, 5642375, 0, 950964, 0.34666606727489158, 5333026, 309349, 0, 341766.46391752549, 0.00056533100874221894, 609197.53608247451, 0.54114960598586614, 5332026, 1000, 309349, 0, 0, 2352, 0.082147610459505041, 339414.46391752549, 0, 5333026, 1260313, 0.26157545784261532)
GO
INSERT [dbo].[tbWbEasyCalcData] ([WbEasyCalcDataId], [CreateLogin], [CreateDate], [ModifyLogin], [ModifyDate], [ZoneId], [YearNo], [MonthNo], [Description], [IsArchive], [IsAccepted], [Start_PeriodDays_M21], [SysInput_Desc_B6], [SysInput_Desc_B7], [SysInput_Desc_B8], [SysInput_Desc_B9], [SysInput_SystemInputVolumeM3_D6], [SysInput_SystemInputVolumeError_F6], [SysInput_SystemInputVolumeM3_D7], [SysInput_SystemInputVolumeError_F7], [SysInput_SystemInputVolumeM3_D8], [SysInput_SystemInputVolumeError_F8], [SysInput_SystemInputVolumeM3_D9], [SysInput_SystemInputVolumeError_F9], [BilledCons_Desc_B8], [BilledCons_Desc_B9], [BilledCons_Desc_B10], [BilledCons_Desc_B11], [BilledCons_Desc_F8], [BilledCons_Desc_F9], [BilledCons_Desc_F10], [BilledCons_Desc_F11], [UnbilledCons_Desc_D8], [UnbilledCons_Desc_D9], [UnbilledCons_Desc_D10], [UnbilledCons_Desc_D11], [UnbilledCons_Desc_F6], [UnbilledCons_Desc_F7], [UnbilledCons_Desc_F8], [UnbilledCons_Desc_F9], [UnbilledCons_Desc_F10], [UnbilledCons_Desc_F11], [UnauthCons_Desc_B18], [UnauthCons_Desc_B19], [UnauthCons_Desc_B20], [UnauthCons_Desc_B21], [MetErrors_Desc_D12], [MetErrors_Desc_D13], [MetErrors_Desc_D14], [MetErrors_Desc_D15], [Network_Desc_B7], [Network_Desc_B8], [Network_Desc_B9], [Network_Desc_B10], [Interm_Area_B7], [Interm_Area_B8], [Interm_Area_B9], [Interm_Area_B10], [BilledCons_BilledMetConsBulkWatSupExpM3_D6], [BilledCons_BilledUnmetConsBulkWatSupExpM3_H6], [BilledCons_UnbMetConsM3_D8], [BilledCons_UnbMetConsM3_D9], [BilledCons_UnbMetConsM3_D10], [BilledCons_UnbMetConsM3_D11], [BilledCons_UnbUnmetConsM3_H8], [BilledCons_UnbUnmetConsM3_H9], [BilledCons_UnbUnmetConsM3_H10], [BilledCons_UnbUnmetConsM3_H11], [UnbilledCons_MetConsBulkWatSupExpM3_D6], [UnbilledCons_UnbMetConsM3_D8], [UnbilledCons_UnbMetConsM3_D9], [UnbilledCons_UnbMetConsM3_D10], [UnbilledCons_UnbMetConsM3_D11], [UnbilledCons_UnbUnmetConsM3_H6], [UnbilledCons_UnbUnmetConsM3_H7], [UnbilledCons_UnbUnmetConsM3_H8], [UnbilledCons_UnbUnmetConsM3_H9], [UnbilledCons_UnbUnmetConsM3_H10], [UnbilledCons_UnbUnmetConsM3_H11], [UnbilledCons_UnbUnmetConsError_J6], [UnbilledCons_UnbUnmetConsError_J7], [UnbilledCons_UnbUnmetConsError_J8], [UnbilledCons_UnbUnmetConsError_J9], [UnbilledCons_UnbUnmetConsError_J10], [UnbilledCons_UnbUnmetConsError_J11], [UnauthCons_IllegalConnDomEstNo_D6], [UnauthCons_IllegalConnDomPersPerHouse_H6], [UnauthCons_IllegalConnDomConsLitPerPersDay_J6], [UnauthCons_IllegalConnDomErrorMargin_F6], [UnauthCons_IllegalConnOthersErrorMargin_F10], [IllegalConnectionsOthersEstimatedNumber_D10], [IllegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10], [UnauthCons_MeterTampBypEtcEstNo_D14], [UnauthCons_MeterTampBypEtcErrorMargin_F14], [UnauthCons_MeterTampBypEtcConsLitPerCustDay_J14], [UnauthCons_OthersErrorMargin_F18], [UnauthCons_OthersErrorMargin_F19], [UnauthCons_OthersErrorMargin_F20], [UnauthCons_OthersErrorMargin_F21], [UnauthCons_OthersM3PerDay_J18], [UnauthCons_OthersM3PerDay_J19], [UnauthCons_OthersM3PerDay_J20], [UnauthCons_OthersM3PerDay_J21], [MetErrors_DetailedManualSpec_J6], [MetErrors_BilledMetConsWoBulkSupMetUndrreg_H8], [MetErrors_BilledMetConsWoBulkSupErrorMargin_N8], [MetErrors_Total_F12], [MetErrors_Total_F13], [MetErrors_Total_F14], [MetErrors_Total_F15], [MetErrors_Meter_H12], [MetErrors_Meter_H13], [MetErrors_Meter_H14], [MetErrors_Meter_H15], [MetErrors_Error_N12], [MetErrors_Error_N13], [MetErrors_Error_N14], [MetErrors_Error_N15], [MeteredBulkSupplyExportErrorMargin_N32], [UnbilledMeteredConsumptionWithoutBulkSupplyErrorMargin_N34], [CorruptMeterReadingPracticessErrorMargin_N38], [DataHandlingErrorsOffice_L40], [DataHandlingErrorsOfficeErrorMargin_N40], [MetErrors_MetBulkSupExpMetUnderreg_H32], [MetErrors_UnbillMetConsWoBulkSupplMetUndrreg_H34], [MetErrors_CorruptMetReadPractMetUndrreg_H38], [Network_DistributionAndTransmissionMains_D7], [Network_DistributionAndTransmissionMains_D8], [Network_DistributionAndTransmissionMains_D9], [Network_DistributionAndTransmissionMains_D10], [Network_NoOfConnOfRegCustomers_H10], [Network_NoOfInactAccountsWSvcConns_H18], [Network_AvgLenOfSvcConnFromBoundaryToMeterM_H32], [Network_PossibleUnd_D30], [Network_NoCustomers_H7], [Network_ErrorMargin_J7], [Network_ErrorMargin_J10], [Network_ErrorMargin_J18], [Network_ErrorMargin_J32], [Network_ErrorMargin_D35], [Prs_Area_B7], [Prs_Area_B8], [Prs_Area_B9], [Prs_Area_B10], [Prs_ApproxNoOfConn_D7], [Prs_DailyAvgPrsM_F7], [Prs_ApproxNoOfConn_D8], [Prs_DailyAvgPrsM_F8], [Prs_ApproxNoOfConn_D9], [Prs_DailyAvgPrsM_F9], [Prs_ApproxNoOfConn_D10], [Prs_DailyAvgPrsM_F10], [Prs_ErrorMarg_F26], [Interm_Conn_D7], [Interm_Conn_D8], [Interm_Conn_D9], [Interm_Conn_D10], [Interm_Days_F7], [Interm_Days_F8], [Interm_Days_F9], [Interm_Days_F10], [Interm_Hour_H7], [Interm_Hour_H8], [Interm_Hour_H9], [Interm_Hour_H10], [Interm_ErrorMarg_H26], [FinancData_G6], [FinancData_K6], [FinancData_G8], [FinancData_D26], [FinancData_G35], [MatrixOneIn_SelectedOption], [MatrixOneIn_C11], [MatrixOneIn_C12], [MatrixOneIn_C13], [MatrixOneIn_C14], [MatrixOneIn_C21], [MatrixOneIn_C22], [MatrixOneIn_C23], [MatrixOneIn_C24], [MatrixOneIn_D21], [MatrixOneIn_D22], [MatrixOneIn_D23], [MatrixOneIn_D24], [MatrixOneIn_E11], [MatrixOneIn_E12], [MatrixOneIn_E13], [MatrixOneIn_E14], [MatrixOneIn_E21], [MatrixOneIn_E22], [MatrixOneIn_E23], [MatrixOneIn_E24], [MatrixOneIn_F11], [MatrixOneIn_F12], [MatrixOneIn_F13], [MatrixOneIn_F14], [MatrixOneIn_F21], [MatrixOneIn_F22], [MatrixOneIn_F23], [MatrixOneIn_F24], [MatrixOneIn_G11], [MatrixOneIn_G12], [MatrixOneIn_G13], [MatrixOneIn_G14], [MatrixOneIn_G21], [MatrixOneIn_G22], [MatrixOneIn_G23], [MatrixOneIn_G24], [MatrixOneIn_H11], [MatrixOneIn_H12], [MatrixOneIn_H13], [MatrixOneIn_H14], [MatrixOneIn_H21], [MatrixOneIn_H22], [MatrixOneIn_H23], [MatrixOneIn_H24], [MatrixTwoIn_SelectedOption], [MatrixTwoIn_D21], [MatrixTwoIn_D22], [MatrixTwoIn_D23], [MatrixTwoIn_D24], [MatrixTwoIn_E11], [MatrixTwoIn_E12], [MatrixTwoIn_E13], [MatrixTwoIn_E14], [MatrixTwoIn_E21], [MatrixTwoIn_E22], [MatrixTwoIn_E23], [MatrixTwoIn_E24], [MatrixTwoIn_F11], [MatrixTwoIn_F12], [MatrixTwoIn_F13], [MatrixTwoIn_F14], [MatrixTwoIn_F21], [MatrixTwoIn_F22], [MatrixTwoIn_F23], [MatrixTwoIn_F24], [MatrixTwoIn_G11], [MatrixTwoIn_G12], [MatrixTwoIn_G13], [MatrixTwoIn_G14], [MatrixTwoIn_G21], [MatrixTwoIn_G22], [MatrixTwoIn_G23], [MatrixTwoIn_G24], [MatrixTwoIn_H11], [MatrixTwoIn_H12], [MatrixTwoIn_H13], [MatrixTwoIn_H14], [MatrixTwoIn_H21], [MatrixTwoIn_H22], [MatrixTwoIn_H23], [MatrixTwoIn_H24], [SystemInputVolume_B19], [SystemInputVolumeErrorMargin_B21], [AuthorizedConsumption_K12], [AuthorizedConsumptionErrorMargin_K15], [WaterLosses_K29], [WaterLossesErrorMargin_K31], [BilledAuthorizedConsumption_T8], [UnbilledAuthorizedConsumption_T16], [UnbilledAuthorizedConsumptionErrorMargin_T20], [CommercialLosses_T26], [CommercialLossesErrorMargin_T29], [PhysicalLossesM3_T34], [PhyscialLossesErrorMargin_AH35], [BilledMeteredConsumption_AC4], [BilledUnmeteredConsumption_AC9], [UnbilledMeteredConsumption_AC14], [UnbilledUnmeteredConsumption_AC19], [UnbilledUnmeteredConsumptionErrorMargin_AO20], [UnauthorizedConsumption_AC24], [UnauthorizedConsumptionErrorMargin_AO25], [CustomerMeterInaccuraciesAndErrorsM3_AC29], [CustomerMeterInaccuraciesAndErrorsErrorMargin_AO30], [RevenueWaterM3_AY8], [NonRevenueWaterM3_AY24], [NonRevenueWaterErrorMargin_AY26]) VALUES (15, N'none\none', CAST(N'2021-01-14T09:15:40.203' AS DateTime), N'EMEA\79749', CAST(N'2021-05-14T15:17:42.323' AS DateTime), 6773, 2019, 2, N'Very old', 1, 0, 15, NULL, NULL, NULL, NULL, 102537, 0.12, 0, 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 3740.999, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5056.876, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20342.3603638975, 0, 0, 0, 333, 0, 0, 0, 0, 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, N'0', 0, 0, 0, 1, 1.5, 2, 4, 8, 2, 4, 8, 16, 25, 50, 100, 200, 25, 50, 100, 200, 50, 100, 200, 400, 40, 75, 150, 300, 75, 150, 300, 600, 50, 100, 200, 400, 100, 200, 400, 800, 60, 125, 250, 500, 125, 250, 500, 1000, 2, 55, 110, 220, 400, 50, 100, 200, 350, 80, 160, 320, 600, 65, 125, 250, 450, 105, 210, 420, 800, 75, 150, 300, 550, 130, 260, 520, 1000, 85, 175, 350, 650, 155, 310, 620, 1200, 102537, 0.11999999999999998, 8797.875, 0, 93739.125, 0.13126258646002936, 3740.999, 5056.876, 0, 0, 0, 93739.125, 0.13126258646002936, 3740.999, 0, 5056.876, 0, 0, 0, 0, 0, 0, 3740.999, 98796.001, 0.12454390739965272)
GO
INSERT [dbo].[tbWbEasyCalcData] ([WbEasyCalcDataId], [CreateLogin], [CreateDate], [ModifyLogin], [ModifyDate], [ZoneId], [YearNo], [MonthNo], [Description], [IsArchive], [IsAccepted], [Start_PeriodDays_M21], [SysInput_Desc_B6], [SysInput_Desc_B7], [SysInput_Desc_B8], [SysInput_Desc_B9], [SysInput_SystemInputVolumeM3_D6], [SysInput_SystemInputVolumeError_F6], [SysInput_SystemInputVolumeM3_D7], [SysInput_SystemInputVolumeError_F7], [SysInput_SystemInputVolumeM3_D8], [SysInput_SystemInputVolumeError_F8], [SysInput_SystemInputVolumeM3_D9], [SysInput_SystemInputVolumeError_F9], [BilledCons_Desc_B8], [BilledCons_Desc_B9], [BilledCons_Desc_B10], [BilledCons_Desc_B11], [BilledCons_Desc_F8], [BilledCons_Desc_F9], [BilledCons_Desc_F10], [BilledCons_Desc_F11], [UnbilledCons_Desc_D8], [UnbilledCons_Desc_D9], [UnbilledCons_Desc_D10], [UnbilledCons_Desc_D11], [UnbilledCons_Desc_F6], [UnbilledCons_Desc_F7], [UnbilledCons_Desc_F8], [UnbilledCons_Desc_F9], [UnbilledCons_Desc_F10], [UnbilledCons_Desc_F11], [UnauthCons_Desc_B18], [UnauthCons_Desc_B19], [UnauthCons_Desc_B20], [UnauthCons_Desc_B21], [MetErrors_Desc_D12], [MetErrors_Desc_D13], [MetErrors_Desc_D14], [MetErrors_Desc_D15], [Network_Desc_B7], [Network_Desc_B8], [Network_Desc_B9], [Network_Desc_B10], [Interm_Area_B7], [Interm_Area_B8], [Interm_Area_B9], [Interm_Area_B10], [BilledCons_BilledMetConsBulkWatSupExpM3_D6], [BilledCons_BilledUnmetConsBulkWatSupExpM3_H6], [BilledCons_UnbMetConsM3_D8], [BilledCons_UnbMetConsM3_D9], [BilledCons_UnbMetConsM3_D10], [BilledCons_UnbMetConsM3_D11], [BilledCons_UnbUnmetConsM3_H8], [BilledCons_UnbUnmetConsM3_H9], [BilledCons_UnbUnmetConsM3_H10], [BilledCons_UnbUnmetConsM3_H11], [UnbilledCons_MetConsBulkWatSupExpM3_D6], [UnbilledCons_UnbMetConsM3_D8], [UnbilledCons_UnbMetConsM3_D9], [UnbilledCons_UnbMetConsM3_D10], [UnbilledCons_UnbMetConsM3_D11], [UnbilledCons_UnbUnmetConsM3_H6], [UnbilledCons_UnbUnmetConsM3_H7], [UnbilledCons_UnbUnmetConsM3_H8], [UnbilledCons_UnbUnmetConsM3_H9], [UnbilledCons_UnbUnmetConsM3_H10], [UnbilledCons_UnbUnmetConsM3_H11], [UnbilledCons_UnbUnmetConsError_J6], [UnbilledCons_UnbUnmetConsError_J7], [UnbilledCons_UnbUnmetConsError_J8], [UnbilledCons_UnbUnmetConsError_J9], [UnbilledCons_UnbUnmetConsError_J10], [UnbilledCons_UnbUnmetConsError_J11], [UnauthCons_IllegalConnDomEstNo_D6], [UnauthCons_IllegalConnDomPersPerHouse_H6], [UnauthCons_IllegalConnDomConsLitPerPersDay_J6], [UnauthCons_IllegalConnDomErrorMargin_F6], [UnauthCons_IllegalConnOthersErrorMargin_F10], [IllegalConnectionsOthersEstimatedNumber_D10], [IllegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10], [UnauthCons_MeterTampBypEtcEstNo_D14], [UnauthCons_MeterTampBypEtcErrorMargin_F14], [UnauthCons_MeterTampBypEtcConsLitPerCustDay_J14], [UnauthCons_OthersErrorMargin_F18], [UnauthCons_OthersErrorMargin_F19], [UnauthCons_OthersErrorMargin_F20], [UnauthCons_OthersErrorMargin_F21], [UnauthCons_OthersM3PerDay_J18], [UnauthCons_OthersM3PerDay_J19], [UnauthCons_OthersM3PerDay_J20], [UnauthCons_OthersM3PerDay_J21], [MetErrors_DetailedManualSpec_J6], [MetErrors_BilledMetConsWoBulkSupMetUndrreg_H8], [MetErrors_BilledMetConsWoBulkSupErrorMargin_N8], [MetErrors_Total_F12], [MetErrors_Total_F13], [MetErrors_Total_F14], [MetErrors_Total_F15], [MetErrors_Meter_H12], [MetErrors_Meter_H13], [MetErrors_Meter_H14], [MetErrors_Meter_H15], [MetErrors_Error_N12], [MetErrors_Error_N13], [MetErrors_Error_N14], [MetErrors_Error_N15], [MeteredBulkSupplyExportErrorMargin_N32], [UnbilledMeteredConsumptionWithoutBulkSupplyErrorMargin_N34], [CorruptMeterReadingPracticessErrorMargin_N38], [DataHandlingErrorsOffice_L40], [DataHandlingErrorsOfficeErrorMargin_N40], [MetErrors_MetBulkSupExpMetUnderreg_H32], [MetErrors_UnbillMetConsWoBulkSupplMetUndrreg_H34], [MetErrors_CorruptMetReadPractMetUndrreg_H38], [Network_DistributionAndTransmissionMains_D7], [Network_DistributionAndTransmissionMains_D8], [Network_DistributionAndTransmissionMains_D9], [Network_DistributionAndTransmissionMains_D10], [Network_NoOfConnOfRegCustomers_H10], [Network_NoOfInactAccountsWSvcConns_H18], [Network_AvgLenOfSvcConnFromBoundaryToMeterM_H32], [Network_PossibleUnd_D30], [Network_NoCustomers_H7], [Network_ErrorMargin_J7], [Network_ErrorMargin_J10], [Network_ErrorMargin_J18], [Network_ErrorMargin_J32], [Network_ErrorMargin_D35], [Prs_Area_B7], [Prs_Area_B8], [Prs_Area_B9], [Prs_Area_B10], [Prs_ApproxNoOfConn_D7], [Prs_DailyAvgPrsM_F7], [Prs_ApproxNoOfConn_D8], [Prs_DailyAvgPrsM_F8], [Prs_ApproxNoOfConn_D9], [Prs_DailyAvgPrsM_F9], [Prs_ApproxNoOfConn_D10], [Prs_DailyAvgPrsM_F10], [Prs_ErrorMarg_F26], [Interm_Conn_D7], [Interm_Conn_D8], [Interm_Conn_D9], [Interm_Conn_D10], [Interm_Days_F7], [Interm_Days_F8], [Interm_Days_F9], [Interm_Days_F10], [Interm_Hour_H7], [Interm_Hour_H8], [Interm_Hour_H9], [Interm_Hour_H10], [Interm_ErrorMarg_H26], [FinancData_G6], [FinancData_K6], [FinancData_G8], [FinancData_D26], [FinancData_G35], [MatrixOneIn_SelectedOption], [MatrixOneIn_C11], [MatrixOneIn_C12], [MatrixOneIn_C13], [MatrixOneIn_C14], [MatrixOneIn_C21], [MatrixOneIn_C22], [MatrixOneIn_C23], [MatrixOneIn_C24], [MatrixOneIn_D21], [MatrixOneIn_D22], [MatrixOneIn_D23], [MatrixOneIn_D24], [MatrixOneIn_E11], [MatrixOneIn_E12], [MatrixOneIn_E13], [MatrixOneIn_E14], [MatrixOneIn_E21], [MatrixOneIn_E22], [MatrixOneIn_E23], [MatrixOneIn_E24], [MatrixOneIn_F11], [MatrixOneIn_F12], [MatrixOneIn_F13], [MatrixOneIn_F14], [MatrixOneIn_F21], [MatrixOneIn_F22], [MatrixOneIn_F23], [MatrixOneIn_F24], [MatrixOneIn_G11], [MatrixOneIn_G12], [MatrixOneIn_G13], [MatrixOneIn_G14], [MatrixOneIn_G21], [MatrixOneIn_G22], [MatrixOneIn_G23], [MatrixOneIn_G24], [MatrixOneIn_H11], [MatrixOneIn_H12], [MatrixOneIn_H13], [MatrixOneIn_H14], [MatrixOneIn_H21], [MatrixOneIn_H22], [MatrixOneIn_H23], [MatrixOneIn_H24], [MatrixTwoIn_SelectedOption], [MatrixTwoIn_D21], [MatrixTwoIn_D22], [MatrixTwoIn_D23], [MatrixTwoIn_D24], [MatrixTwoIn_E11], [MatrixTwoIn_E12], [MatrixTwoIn_E13], [MatrixTwoIn_E14], [MatrixTwoIn_E21], [MatrixTwoIn_E22], [MatrixTwoIn_E23], [MatrixTwoIn_E24], [MatrixTwoIn_F11], [MatrixTwoIn_F12], [MatrixTwoIn_F13], [MatrixTwoIn_F14], [MatrixTwoIn_F21], [MatrixTwoIn_F22], [MatrixTwoIn_F23], [MatrixTwoIn_F24], [MatrixTwoIn_G11], [MatrixTwoIn_G12], [MatrixTwoIn_G13], [MatrixTwoIn_G14], [MatrixTwoIn_G21], [MatrixTwoIn_G22], [MatrixTwoIn_G23], [MatrixTwoIn_G24], [MatrixTwoIn_H11], [MatrixTwoIn_H12], [MatrixTwoIn_H13], [MatrixTwoIn_H14], [MatrixTwoIn_H21], [MatrixTwoIn_H22], [MatrixTwoIn_H23], [MatrixTwoIn_H24], [SystemInputVolume_B19], [SystemInputVolumeErrorMargin_B21], [AuthorizedConsumption_K12], [AuthorizedConsumptionErrorMargin_K15], [WaterLosses_K29], [WaterLossesErrorMargin_K31], [BilledAuthorizedConsumption_T8], [UnbilledAuthorizedConsumption_T16], [UnbilledAuthorizedConsumptionErrorMargin_T20], [CommercialLosses_T26], [CommercialLossesErrorMargin_T29], [PhysicalLossesM3_T34], [PhyscialLossesErrorMargin_AH35], [BilledMeteredConsumption_AC4], [BilledUnmeteredConsumption_AC9], [UnbilledMeteredConsumption_AC14], [UnbilledUnmeteredConsumption_AC19], [UnbilledUnmeteredConsumptionErrorMargin_AO20], [UnauthorizedConsumption_AC24], [UnauthorizedConsumptionErrorMargin_AO25], [CustomerMeterInaccuraciesAndErrorsM3_AC29], [CustomerMeterInaccuraciesAndErrorsErrorMargin_AO30], [RevenueWaterM3_AY8], [NonRevenueWaterM3_AY24], [NonRevenueWaterErrorMargin_AY26]) VALUES (20, N'none\none', CAST(N'2021-01-14T09:15:40.203' AS DateTime), N'EMEA\79749', CAST(N'2021-05-14T09:20:44.220' AS DateTime), 6774, 2019, 2, N'Desc 1', 0, 0, 28, NULL, NULL, NULL, NULL, 2020202, 0.2, 0, 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 12032, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, N'0', 0, 0, 0, 1, 1.5, 2, 4, 8, 2, 4, 8, 16, 25, 50, 100, 200, 25, 50, 100, 200, 50, 100, 200, 400, 40, 75, 150, 300, 75, 150, 300, 600, 50, 100, 200, 400, 100, 200, 400, 800, 60, 125, 250, 500, 125, 250, 500, 1000, 2, 55, 110, 220, 400, 50, 100, 200, 350, 80, 160, 320, 600, 65, 125, 250, 450, 105, 210, 420, 800, 75, 150, 300, 550, 130, 260, 520, 1000, 85, 175, 350, 650, 155, 310, 620, 1200, 2020202, 0.2, 12032, 0, 2008170, 0.2011983049243839, 0, 12032, 0, 0, 0, 2008170, 0.2011983049243839, 0, 0, 12032, 0, 0, 0, 0, 0, 0, 0, 2020202, 0.2)
GO
INSERT [dbo].[tbWbEasyCalcData] ([WbEasyCalcDataId], [CreateLogin], [CreateDate], [ModifyLogin], [ModifyDate], [ZoneId], [YearNo], [MonthNo], [Description], [IsArchive], [IsAccepted], [Start_PeriodDays_M21], [SysInput_Desc_B6], [SysInput_Desc_B7], [SysInput_Desc_B8], [SysInput_Desc_B9], [SysInput_SystemInputVolumeM3_D6], [SysInput_SystemInputVolumeError_F6], [SysInput_SystemInputVolumeM3_D7], [SysInput_SystemInputVolumeError_F7], [SysInput_SystemInputVolumeM3_D8], [SysInput_SystemInputVolumeError_F8], [SysInput_SystemInputVolumeM3_D9], [SysInput_SystemInputVolumeError_F9], [BilledCons_Desc_B8], [BilledCons_Desc_B9], [BilledCons_Desc_B10], [BilledCons_Desc_B11], [BilledCons_Desc_F8], [BilledCons_Desc_F9], [BilledCons_Desc_F10], [BilledCons_Desc_F11], [UnbilledCons_Desc_D8], [UnbilledCons_Desc_D9], [UnbilledCons_Desc_D10], [UnbilledCons_Desc_D11], [UnbilledCons_Desc_F6], [UnbilledCons_Desc_F7], [UnbilledCons_Desc_F8], [UnbilledCons_Desc_F9], [UnbilledCons_Desc_F10], [UnbilledCons_Desc_F11], [UnauthCons_Desc_B18], [UnauthCons_Desc_B19], [UnauthCons_Desc_B20], [UnauthCons_Desc_B21], [MetErrors_Desc_D12], [MetErrors_Desc_D13], [MetErrors_Desc_D14], [MetErrors_Desc_D15], [Network_Desc_B7], [Network_Desc_B8], [Network_Desc_B9], [Network_Desc_B10], [Interm_Area_B7], [Interm_Area_B8], [Interm_Area_B9], [Interm_Area_B10], [BilledCons_BilledMetConsBulkWatSupExpM3_D6], [BilledCons_BilledUnmetConsBulkWatSupExpM3_H6], [BilledCons_UnbMetConsM3_D8], [BilledCons_UnbMetConsM3_D9], [BilledCons_UnbMetConsM3_D10], [BilledCons_UnbMetConsM3_D11], [BilledCons_UnbUnmetConsM3_H8], [BilledCons_UnbUnmetConsM3_H9], [BilledCons_UnbUnmetConsM3_H10], [BilledCons_UnbUnmetConsM3_H11], [UnbilledCons_MetConsBulkWatSupExpM3_D6], [UnbilledCons_UnbMetConsM3_D8], [UnbilledCons_UnbMetConsM3_D9], [UnbilledCons_UnbMetConsM3_D10], [UnbilledCons_UnbMetConsM3_D11], [UnbilledCons_UnbUnmetConsM3_H6], [UnbilledCons_UnbUnmetConsM3_H7], [UnbilledCons_UnbUnmetConsM3_H8], [UnbilledCons_UnbUnmetConsM3_H9], [UnbilledCons_UnbUnmetConsM3_H10], [UnbilledCons_UnbUnmetConsM3_H11], [UnbilledCons_UnbUnmetConsError_J6], [UnbilledCons_UnbUnmetConsError_J7], [UnbilledCons_UnbUnmetConsError_J8], [UnbilledCons_UnbUnmetConsError_J9], [UnbilledCons_UnbUnmetConsError_J10], [UnbilledCons_UnbUnmetConsError_J11], [UnauthCons_IllegalConnDomEstNo_D6], [UnauthCons_IllegalConnDomPersPerHouse_H6], [UnauthCons_IllegalConnDomConsLitPerPersDay_J6], [UnauthCons_IllegalConnDomErrorMargin_F6], [UnauthCons_IllegalConnOthersErrorMargin_F10], [IllegalConnectionsOthersEstimatedNumber_D10], [IllegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10], [UnauthCons_MeterTampBypEtcEstNo_D14], [UnauthCons_MeterTampBypEtcErrorMargin_F14], [UnauthCons_MeterTampBypEtcConsLitPerCustDay_J14], [UnauthCons_OthersErrorMargin_F18], [UnauthCons_OthersErrorMargin_F19], [UnauthCons_OthersErrorMargin_F20], [UnauthCons_OthersErrorMargin_F21], [UnauthCons_OthersM3PerDay_J18], [UnauthCons_OthersM3PerDay_J19], [UnauthCons_OthersM3PerDay_J20], [UnauthCons_OthersM3PerDay_J21], [MetErrors_DetailedManualSpec_J6], [MetErrors_BilledMetConsWoBulkSupMetUndrreg_H8], [MetErrors_BilledMetConsWoBulkSupErrorMargin_N8], [MetErrors_Total_F12], [MetErrors_Total_F13], [MetErrors_Total_F14], [MetErrors_Total_F15], [MetErrors_Meter_H12], [MetErrors_Meter_H13], [MetErrors_Meter_H14], [MetErrors_Meter_H15], [MetErrors_Error_N12], [MetErrors_Error_N13], [MetErrors_Error_N14], [MetErrors_Error_N15], [MeteredBulkSupplyExportErrorMargin_N32], [UnbilledMeteredConsumptionWithoutBulkSupplyErrorMargin_N34], [CorruptMeterReadingPracticessErrorMargin_N38], [DataHandlingErrorsOffice_L40], [DataHandlingErrorsOfficeErrorMargin_N40], [MetErrors_MetBulkSupExpMetUnderreg_H32], [MetErrors_UnbillMetConsWoBulkSupplMetUndrreg_H34], [MetErrors_CorruptMetReadPractMetUndrreg_H38], [Network_DistributionAndTransmissionMains_D7], [Network_DistributionAndTransmissionMains_D8], [Network_DistributionAndTransmissionMains_D9], [Network_DistributionAndTransmissionMains_D10], [Network_NoOfConnOfRegCustomers_H10], [Network_NoOfInactAccountsWSvcConns_H18], [Network_AvgLenOfSvcConnFromBoundaryToMeterM_H32], [Network_PossibleUnd_D30], [Network_NoCustomers_H7], [Network_ErrorMargin_J7], [Network_ErrorMargin_J10], [Network_ErrorMargin_J18], [Network_ErrorMargin_J32], [Network_ErrorMargin_D35], [Prs_Area_B7], [Prs_Area_B8], [Prs_Area_B9], [Prs_Area_B10], [Prs_ApproxNoOfConn_D7], [Prs_DailyAvgPrsM_F7], [Prs_ApproxNoOfConn_D8], [Prs_DailyAvgPrsM_F8], [Prs_ApproxNoOfConn_D9], [Prs_DailyAvgPrsM_F9], [Prs_ApproxNoOfConn_D10], [Prs_DailyAvgPrsM_F10], [Prs_ErrorMarg_F26], [Interm_Conn_D7], [Interm_Conn_D8], [Interm_Conn_D9], [Interm_Conn_D10], [Interm_Days_F7], [Interm_Days_F8], [Interm_Days_F9], [Interm_Days_F10], [Interm_Hour_H7], [Interm_Hour_H8], [Interm_Hour_H9], [Interm_Hour_H10], [Interm_ErrorMarg_H26], [FinancData_G6], [FinancData_K6], [FinancData_G8], [FinancData_D26], [FinancData_G35], [MatrixOneIn_SelectedOption], [MatrixOneIn_C11], [MatrixOneIn_C12], [MatrixOneIn_C13], [MatrixOneIn_C14], [MatrixOneIn_C21], [MatrixOneIn_C22], [MatrixOneIn_C23], [MatrixOneIn_C24], [MatrixOneIn_D21], [MatrixOneIn_D22], [MatrixOneIn_D23], [MatrixOneIn_D24], [MatrixOneIn_E11], [MatrixOneIn_E12], [MatrixOneIn_E13], [MatrixOneIn_E14], [MatrixOneIn_E21], [MatrixOneIn_E22], [MatrixOneIn_E23], [MatrixOneIn_E24], [MatrixOneIn_F11], [MatrixOneIn_F12], [MatrixOneIn_F13], [MatrixOneIn_F14], [MatrixOneIn_F21], [MatrixOneIn_F22], [MatrixOneIn_F23], [MatrixOneIn_F24], [MatrixOneIn_G11], [MatrixOneIn_G12], [MatrixOneIn_G13], [MatrixOneIn_G14], [MatrixOneIn_G21], [MatrixOneIn_G22], [MatrixOneIn_G23], [MatrixOneIn_G24], [MatrixOneIn_H11], [MatrixOneIn_H12], [MatrixOneIn_H13], [MatrixOneIn_H14], [MatrixOneIn_H21], [MatrixOneIn_H22], [MatrixOneIn_H23], [MatrixOneIn_H24], [MatrixTwoIn_SelectedOption], [MatrixTwoIn_D21], [MatrixTwoIn_D22], [MatrixTwoIn_D23], [MatrixTwoIn_D24], [MatrixTwoIn_E11], [MatrixTwoIn_E12], [MatrixTwoIn_E13], [MatrixTwoIn_E14], [MatrixTwoIn_E21], [MatrixTwoIn_E22], [MatrixTwoIn_E23], [MatrixTwoIn_E24], [MatrixTwoIn_F11], [MatrixTwoIn_F12], [MatrixTwoIn_F13], [MatrixTwoIn_F14], [MatrixTwoIn_F21], [MatrixTwoIn_F22], [MatrixTwoIn_F23], [MatrixTwoIn_F24], [MatrixTwoIn_G11], [MatrixTwoIn_G12], [MatrixTwoIn_G13], [MatrixTwoIn_G14], [MatrixTwoIn_G21], [MatrixTwoIn_G22], [MatrixTwoIn_G23], [MatrixTwoIn_G24], [MatrixTwoIn_H11], [MatrixTwoIn_H12], [MatrixTwoIn_H13], [MatrixTwoIn_H14], [MatrixTwoIn_H21], [MatrixTwoIn_H22], [MatrixTwoIn_H23], [MatrixTwoIn_H24], [SystemInputVolume_B19], [SystemInputVolumeErrorMargin_B21], [AuthorizedConsumption_K12], [AuthorizedConsumptionErrorMargin_K15], [WaterLosses_K29], [WaterLossesErrorMargin_K31], [BilledAuthorizedConsumption_T8], [UnbilledAuthorizedConsumption_T16], [UnbilledAuthorizedConsumptionErrorMargin_T20], [CommercialLosses_T26], [CommercialLossesErrorMargin_T29], [PhysicalLossesM3_T34], [PhyscialLossesErrorMargin_AH35], [BilledMeteredConsumption_AC4], [BilledUnmeteredConsumption_AC9], [UnbilledMeteredConsumption_AC14], [UnbilledUnmeteredConsumption_AC19], [UnbilledUnmeteredConsumptionErrorMargin_AO20], [UnauthorizedConsumption_AC24], [UnauthorizedConsumptionErrorMargin_AO25], [CustomerMeterInaccuraciesAndErrorsM3_AC29], [CustomerMeterInaccuraciesAndErrorsErrorMargin_AO30], [RevenueWaterM3_AY8], [NonRevenueWaterM3_AY24], [NonRevenueWaterErrorMargin_AY26]) VALUES (21, N'none\none', CAST(N'2021-01-14T09:15:40.203' AS DateTime), N'EMEA\79749', CAST(N'2021-03-15T14:08:52.997' AS DateTime), 6773, 2019, 11, N'', 0, 0, 30, NULL, NULL, NULL, NULL, 2120000, 0.21, 0, 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, N'0', 0, 0, 0, 1, 1.5, 2, 4, 8, 2, 4, 8, 16, 25, 50, 100, 200, 25, 50, 100, 200, 50, 100, 200, 400, 40, 75, 150, 300, 75, 150, 300, 600, 50, 100, 200, 400, 100, 200, 400, 800, 60, 125, 250, 500, 125, 250, 500, 1000, 2, 55, 110, 220, 400, 50, 100, 200, 350, 80, 160, 320, 600, 65, 125, 250, 450, 105, 210, 420, 800, 75, 150, 300, 550, 130, 260, 520, 1000, 85, 175, 350, 650, 155, 310, 620, 1200, 2120000, 0.21, 0, 0, 2120000, 0.21, 0, 0, 0, 0, 0, 2120000, 0.21, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2120000, 0.21)
GO
INSERT [dbo].[tbWbEasyCalcData] ([WbEasyCalcDataId], [CreateLogin], [CreateDate], [ModifyLogin], [ModifyDate], [ZoneId], [YearNo], [MonthNo], [Description], [IsArchive], [IsAccepted], [Start_PeriodDays_M21], [SysInput_Desc_B6], [SysInput_Desc_B7], [SysInput_Desc_B8], [SysInput_Desc_B9], [SysInput_SystemInputVolumeM3_D6], [SysInput_SystemInputVolumeError_F6], [SysInput_SystemInputVolumeM3_D7], [SysInput_SystemInputVolumeError_F7], [SysInput_SystemInputVolumeM3_D8], [SysInput_SystemInputVolumeError_F8], [SysInput_SystemInputVolumeM3_D9], [SysInput_SystemInputVolumeError_F9], [BilledCons_Desc_B8], [BilledCons_Desc_B9], [BilledCons_Desc_B10], [BilledCons_Desc_B11], [BilledCons_Desc_F8], [BilledCons_Desc_F9], [BilledCons_Desc_F10], [BilledCons_Desc_F11], [UnbilledCons_Desc_D8], [UnbilledCons_Desc_D9], [UnbilledCons_Desc_D10], [UnbilledCons_Desc_D11], [UnbilledCons_Desc_F6], [UnbilledCons_Desc_F7], [UnbilledCons_Desc_F8], [UnbilledCons_Desc_F9], [UnbilledCons_Desc_F10], [UnbilledCons_Desc_F11], [UnauthCons_Desc_B18], [UnauthCons_Desc_B19], [UnauthCons_Desc_B20], [UnauthCons_Desc_B21], [MetErrors_Desc_D12], [MetErrors_Desc_D13], [MetErrors_Desc_D14], [MetErrors_Desc_D15], [Network_Desc_B7], [Network_Desc_B8], [Network_Desc_B9], [Network_Desc_B10], [Interm_Area_B7], [Interm_Area_B8], [Interm_Area_B9], [Interm_Area_B10], [BilledCons_BilledMetConsBulkWatSupExpM3_D6], [BilledCons_BilledUnmetConsBulkWatSupExpM3_H6], [BilledCons_UnbMetConsM3_D8], [BilledCons_UnbMetConsM3_D9], [BilledCons_UnbMetConsM3_D10], [BilledCons_UnbMetConsM3_D11], [BilledCons_UnbUnmetConsM3_H8], [BilledCons_UnbUnmetConsM3_H9], [BilledCons_UnbUnmetConsM3_H10], [BilledCons_UnbUnmetConsM3_H11], [UnbilledCons_MetConsBulkWatSupExpM3_D6], [UnbilledCons_UnbMetConsM3_D8], [UnbilledCons_UnbMetConsM3_D9], [UnbilledCons_UnbMetConsM3_D10], [UnbilledCons_UnbMetConsM3_D11], [UnbilledCons_UnbUnmetConsM3_H6], [UnbilledCons_UnbUnmetConsM3_H7], [UnbilledCons_UnbUnmetConsM3_H8], [UnbilledCons_UnbUnmetConsM3_H9], [UnbilledCons_UnbUnmetConsM3_H10], [UnbilledCons_UnbUnmetConsM3_H11], [UnbilledCons_UnbUnmetConsError_J6], [UnbilledCons_UnbUnmetConsError_J7], [UnbilledCons_UnbUnmetConsError_J8], [UnbilledCons_UnbUnmetConsError_J9], [UnbilledCons_UnbUnmetConsError_J10], [UnbilledCons_UnbUnmetConsError_J11], [UnauthCons_IllegalConnDomEstNo_D6], [UnauthCons_IllegalConnDomPersPerHouse_H6], [UnauthCons_IllegalConnDomConsLitPerPersDay_J6], [UnauthCons_IllegalConnDomErrorMargin_F6], [UnauthCons_IllegalConnOthersErrorMargin_F10], [IllegalConnectionsOthersEstimatedNumber_D10], [IllegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10], [UnauthCons_MeterTampBypEtcEstNo_D14], [UnauthCons_MeterTampBypEtcErrorMargin_F14], [UnauthCons_MeterTampBypEtcConsLitPerCustDay_J14], [UnauthCons_OthersErrorMargin_F18], [UnauthCons_OthersErrorMargin_F19], [UnauthCons_OthersErrorMargin_F20], [UnauthCons_OthersErrorMargin_F21], [UnauthCons_OthersM3PerDay_J18], [UnauthCons_OthersM3PerDay_J19], [UnauthCons_OthersM3PerDay_J20], [UnauthCons_OthersM3PerDay_J21], [MetErrors_DetailedManualSpec_J6], [MetErrors_BilledMetConsWoBulkSupMetUndrreg_H8], [MetErrors_BilledMetConsWoBulkSupErrorMargin_N8], [MetErrors_Total_F12], [MetErrors_Total_F13], [MetErrors_Total_F14], [MetErrors_Total_F15], [MetErrors_Meter_H12], [MetErrors_Meter_H13], [MetErrors_Meter_H14], [MetErrors_Meter_H15], [MetErrors_Error_N12], [MetErrors_Error_N13], [MetErrors_Error_N14], [MetErrors_Error_N15], [MeteredBulkSupplyExportErrorMargin_N32], [UnbilledMeteredConsumptionWithoutBulkSupplyErrorMargin_N34], [CorruptMeterReadingPracticessErrorMargin_N38], [DataHandlingErrorsOffice_L40], [DataHandlingErrorsOfficeErrorMargin_N40], [MetErrors_MetBulkSupExpMetUnderreg_H32], [MetErrors_UnbillMetConsWoBulkSupplMetUndrreg_H34], [MetErrors_CorruptMetReadPractMetUndrreg_H38], [Network_DistributionAndTransmissionMains_D7], [Network_DistributionAndTransmissionMains_D8], [Network_DistributionAndTransmissionMains_D9], [Network_DistributionAndTransmissionMains_D10], [Network_NoOfConnOfRegCustomers_H10], [Network_NoOfInactAccountsWSvcConns_H18], [Network_AvgLenOfSvcConnFromBoundaryToMeterM_H32], [Network_PossibleUnd_D30], [Network_NoCustomers_H7], [Network_ErrorMargin_J7], [Network_ErrorMargin_J10], [Network_ErrorMargin_J18], [Network_ErrorMargin_J32], [Network_ErrorMargin_D35], [Prs_Area_B7], [Prs_Area_B8], [Prs_Area_B9], [Prs_Area_B10], [Prs_ApproxNoOfConn_D7], [Prs_DailyAvgPrsM_F7], [Prs_ApproxNoOfConn_D8], [Prs_DailyAvgPrsM_F8], [Prs_ApproxNoOfConn_D9], [Prs_DailyAvgPrsM_F9], [Prs_ApproxNoOfConn_D10], [Prs_DailyAvgPrsM_F10], [Prs_ErrorMarg_F26], [Interm_Conn_D7], [Interm_Conn_D8], [Interm_Conn_D9], [Interm_Conn_D10], [Interm_Days_F7], [Interm_Days_F8], [Interm_Days_F9], [Interm_Days_F10], [Interm_Hour_H7], [Interm_Hour_H8], [Interm_Hour_H9], [Interm_Hour_H10], [Interm_ErrorMarg_H26], [FinancData_G6], [FinancData_K6], [FinancData_G8], [FinancData_D26], [FinancData_G35], [MatrixOneIn_SelectedOption], [MatrixOneIn_C11], [MatrixOneIn_C12], [MatrixOneIn_C13], [MatrixOneIn_C14], [MatrixOneIn_C21], [MatrixOneIn_C22], [MatrixOneIn_C23], [MatrixOneIn_C24], [MatrixOneIn_D21], [MatrixOneIn_D22], [MatrixOneIn_D23], [MatrixOneIn_D24], [MatrixOneIn_E11], [MatrixOneIn_E12], [MatrixOneIn_E13], [MatrixOneIn_E14], [MatrixOneIn_E21], [MatrixOneIn_E22], [MatrixOneIn_E23], [MatrixOneIn_E24], [MatrixOneIn_F11], [MatrixOneIn_F12], [MatrixOneIn_F13], [MatrixOneIn_F14], [MatrixOneIn_F21], [MatrixOneIn_F22], [MatrixOneIn_F23], [MatrixOneIn_F24], [MatrixOneIn_G11], [MatrixOneIn_G12], [MatrixOneIn_G13], [MatrixOneIn_G14], [MatrixOneIn_G21], [MatrixOneIn_G22], [MatrixOneIn_G23], [MatrixOneIn_G24], [MatrixOneIn_H11], [MatrixOneIn_H12], [MatrixOneIn_H13], [MatrixOneIn_H14], [MatrixOneIn_H21], [MatrixOneIn_H22], [MatrixOneIn_H23], [MatrixOneIn_H24], [MatrixTwoIn_SelectedOption], [MatrixTwoIn_D21], [MatrixTwoIn_D22], [MatrixTwoIn_D23], [MatrixTwoIn_D24], [MatrixTwoIn_E11], [MatrixTwoIn_E12], [MatrixTwoIn_E13], [MatrixTwoIn_E14], [MatrixTwoIn_E21], [MatrixTwoIn_E22], [MatrixTwoIn_E23], [MatrixTwoIn_E24], [MatrixTwoIn_F11], [MatrixTwoIn_F12], [MatrixTwoIn_F13], [MatrixTwoIn_F14], [MatrixTwoIn_F21], [MatrixTwoIn_F22], [MatrixTwoIn_F23], [MatrixTwoIn_F24], [MatrixTwoIn_G11], [MatrixTwoIn_G12], [MatrixTwoIn_G13], [MatrixTwoIn_G14], [MatrixTwoIn_G21], [MatrixTwoIn_G22], [MatrixTwoIn_G23], [MatrixTwoIn_G24], [MatrixTwoIn_H11], [MatrixTwoIn_H12], [MatrixTwoIn_H13], [MatrixTwoIn_H14], [MatrixTwoIn_H21], [MatrixTwoIn_H22], [MatrixTwoIn_H23], [MatrixTwoIn_H24], [SystemInputVolume_B19], [SystemInputVolumeErrorMargin_B21], [AuthorizedConsumption_K12], [AuthorizedConsumptionErrorMargin_K15], [WaterLosses_K29], [WaterLossesErrorMargin_K31], [BilledAuthorizedConsumption_T8], [UnbilledAuthorizedConsumption_T16], [UnbilledAuthorizedConsumptionErrorMargin_T20], [CommercialLosses_T26], [CommercialLossesErrorMargin_T29], [PhysicalLossesM3_T34], [PhyscialLossesErrorMargin_AH35], [BilledMeteredConsumption_AC4], [BilledUnmeteredConsumption_AC9], [UnbilledMeteredConsumption_AC14], [UnbilledUnmeteredConsumption_AC19], [UnbilledUnmeteredConsumptionErrorMargin_AO20], [UnauthorizedConsumption_AC24], [UnauthorizedConsumptionErrorMargin_AO25], [CustomerMeterInaccuraciesAndErrorsM3_AC29], [CustomerMeterInaccuraciesAndErrorsErrorMargin_AO30], [RevenueWaterM3_AY8], [NonRevenueWaterM3_AY24], [NonRevenueWaterErrorMargin_AY26]) VALUES (22, N'none\none', CAST(N'2021-01-14T09:15:40.203' AS DateTime), N'EMEA\79749', CAST(N'2021-03-15T14:15:00.200' AS DateTime), 6774, 2020, 1, N'22 SSSSSSS', 0, 0, 31, NULL, NULL, NULL, NULL, 2222220, 0.22, 0, 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, N'0', 0, 0, 0, 1, 1.5, 2, 4, 8, 2, 4, 8, 16, 25, 50, 100, 200, 25, 50, 100, 200, 50, 100, 200, 400, 40, 75, 150, 300, 75, 150, 300, 600, 50, 100, 200, 400, 100, 200, 400, 800, 60, 125, 250, 500, 125, 250, 500, 1000, 2, 55, 110, 220, 400, 50, 100, 200, 350, 80, 160, 320, 600, 65, 125, 250, 450, 105, 210, 420, 800, 75, 150, 300, 550, 130, 260, 520, 1000, 85, 175, 350, 650, 155, 310, 620, 1200, 2222220, 0.22, 0, 0, 2222220, 0.22, 0, 0, 0, 0, 0, 2222220, 0.22, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2222220, 0.22)
GO
INSERT [dbo].[tbWbEasyCalcData] ([WbEasyCalcDataId], [CreateLogin], [CreateDate], [ModifyLogin], [ModifyDate], [ZoneId], [YearNo], [MonthNo], [Description], [IsArchive], [IsAccepted], [Start_PeriodDays_M21], [SysInput_Desc_B6], [SysInput_Desc_B7], [SysInput_Desc_B8], [SysInput_Desc_B9], [SysInput_SystemInputVolumeM3_D6], [SysInput_SystemInputVolumeError_F6], [SysInput_SystemInputVolumeM3_D7], [SysInput_SystemInputVolumeError_F7], [SysInput_SystemInputVolumeM3_D8], [SysInput_SystemInputVolumeError_F8], [SysInput_SystemInputVolumeM3_D9], [SysInput_SystemInputVolumeError_F9], [BilledCons_Desc_B8], [BilledCons_Desc_B9], [BilledCons_Desc_B10], [BilledCons_Desc_B11], [BilledCons_Desc_F8], [BilledCons_Desc_F9], [BilledCons_Desc_F10], [BilledCons_Desc_F11], [UnbilledCons_Desc_D8], [UnbilledCons_Desc_D9], [UnbilledCons_Desc_D10], [UnbilledCons_Desc_D11], [UnbilledCons_Desc_F6], [UnbilledCons_Desc_F7], [UnbilledCons_Desc_F8], [UnbilledCons_Desc_F9], [UnbilledCons_Desc_F10], [UnbilledCons_Desc_F11], [UnauthCons_Desc_B18], [UnauthCons_Desc_B19], [UnauthCons_Desc_B20], [UnauthCons_Desc_B21], [MetErrors_Desc_D12], [MetErrors_Desc_D13], [MetErrors_Desc_D14], [MetErrors_Desc_D15], [Network_Desc_B7], [Network_Desc_B8], [Network_Desc_B9], [Network_Desc_B10], [Interm_Area_B7], [Interm_Area_B8], [Interm_Area_B9], [Interm_Area_B10], [BilledCons_BilledMetConsBulkWatSupExpM3_D6], [BilledCons_BilledUnmetConsBulkWatSupExpM3_H6], [BilledCons_UnbMetConsM3_D8], [BilledCons_UnbMetConsM3_D9], [BilledCons_UnbMetConsM3_D10], [BilledCons_UnbMetConsM3_D11], [BilledCons_UnbUnmetConsM3_H8], [BilledCons_UnbUnmetConsM3_H9], [BilledCons_UnbUnmetConsM3_H10], [BilledCons_UnbUnmetConsM3_H11], [UnbilledCons_MetConsBulkWatSupExpM3_D6], [UnbilledCons_UnbMetConsM3_D8], [UnbilledCons_UnbMetConsM3_D9], [UnbilledCons_UnbMetConsM3_D10], [UnbilledCons_UnbMetConsM3_D11], [UnbilledCons_UnbUnmetConsM3_H6], [UnbilledCons_UnbUnmetConsM3_H7], [UnbilledCons_UnbUnmetConsM3_H8], [UnbilledCons_UnbUnmetConsM3_H9], [UnbilledCons_UnbUnmetConsM3_H10], [UnbilledCons_UnbUnmetConsM3_H11], [UnbilledCons_UnbUnmetConsError_J6], [UnbilledCons_UnbUnmetConsError_J7], [UnbilledCons_UnbUnmetConsError_J8], [UnbilledCons_UnbUnmetConsError_J9], [UnbilledCons_UnbUnmetConsError_J10], [UnbilledCons_UnbUnmetConsError_J11], [UnauthCons_IllegalConnDomEstNo_D6], [UnauthCons_IllegalConnDomPersPerHouse_H6], [UnauthCons_IllegalConnDomConsLitPerPersDay_J6], [UnauthCons_IllegalConnDomErrorMargin_F6], [UnauthCons_IllegalConnOthersErrorMargin_F10], [IllegalConnectionsOthersEstimatedNumber_D10], [IllegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10], [UnauthCons_MeterTampBypEtcEstNo_D14], [UnauthCons_MeterTampBypEtcErrorMargin_F14], [UnauthCons_MeterTampBypEtcConsLitPerCustDay_J14], [UnauthCons_OthersErrorMargin_F18], [UnauthCons_OthersErrorMargin_F19], [UnauthCons_OthersErrorMargin_F20], [UnauthCons_OthersErrorMargin_F21], [UnauthCons_OthersM3PerDay_J18], [UnauthCons_OthersM3PerDay_J19], [UnauthCons_OthersM3PerDay_J20], [UnauthCons_OthersM3PerDay_J21], [MetErrors_DetailedManualSpec_J6], [MetErrors_BilledMetConsWoBulkSupMetUndrreg_H8], [MetErrors_BilledMetConsWoBulkSupErrorMargin_N8], [MetErrors_Total_F12], [MetErrors_Total_F13], [MetErrors_Total_F14], [MetErrors_Total_F15], [MetErrors_Meter_H12], [MetErrors_Meter_H13], [MetErrors_Meter_H14], [MetErrors_Meter_H15], [MetErrors_Error_N12], [MetErrors_Error_N13], [MetErrors_Error_N14], [MetErrors_Error_N15], [MeteredBulkSupplyExportErrorMargin_N32], [UnbilledMeteredConsumptionWithoutBulkSupplyErrorMargin_N34], [CorruptMeterReadingPracticessErrorMargin_N38], [DataHandlingErrorsOffice_L40], [DataHandlingErrorsOfficeErrorMargin_N40], [MetErrors_MetBulkSupExpMetUnderreg_H32], [MetErrors_UnbillMetConsWoBulkSupplMetUndrreg_H34], [MetErrors_CorruptMetReadPractMetUndrreg_H38], [Network_DistributionAndTransmissionMains_D7], [Network_DistributionAndTransmissionMains_D8], [Network_DistributionAndTransmissionMains_D9], [Network_DistributionAndTransmissionMains_D10], [Network_NoOfConnOfRegCustomers_H10], [Network_NoOfInactAccountsWSvcConns_H18], [Network_AvgLenOfSvcConnFromBoundaryToMeterM_H32], [Network_PossibleUnd_D30], [Network_NoCustomers_H7], [Network_ErrorMargin_J7], [Network_ErrorMargin_J10], [Network_ErrorMargin_J18], [Network_ErrorMargin_J32], [Network_ErrorMargin_D35], [Prs_Area_B7], [Prs_Area_B8], [Prs_Area_B9], [Prs_Area_B10], [Prs_ApproxNoOfConn_D7], [Prs_DailyAvgPrsM_F7], [Prs_ApproxNoOfConn_D8], [Prs_DailyAvgPrsM_F8], [Prs_ApproxNoOfConn_D9], [Prs_DailyAvgPrsM_F9], [Prs_ApproxNoOfConn_D10], [Prs_DailyAvgPrsM_F10], [Prs_ErrorMarg_F26], [Interm_Conn_D7], [Interm_Conn_D8], [Interm_Conn_D9], [Interm_Conn_D10], [Interm_Days_F7], [Interm_Days_F8], [Interm_Days_F9], [Interm_Days_F10], [Interm_Hour_H7], [Interm_Hour_H8], [Interm_Hour_H9], [Interm_Hour_H10], [Interm_ErrorMarg_H26], [FinancData_G6], [FinancData_K6], [FinancData_G8], [FinancData_D26], [FinancData_G35], [MatrixOneIn_SelectedOption], [MatrixOneIn_C11], [MatrixOneIn_C12], [MatrixOneIn_C13], [MatrixOneIn_C14], [MatrixOneIn_C21], [MatrixOneIn_C22], [MatrixOneIn_C23], [MatrixOneIn_C24], [MatrixOneIn_D21], [MatrixOneIn_D22], [MatrixOneIn_D23], [MatrixOneIn_D24], [MatrixOneIn_E11], [MatrixOneIn_E12], [MatrixOneIn_E13], [MatrixOneIn_E14], [MatrixOneIn_E21], [MatrixOneIn_E22], [MatrixOneIn_E23], [MatrixOneIn_E24], [MatrixOneIn_F11], [MatrixOneIn_F12], [MatrixOneIn_F13], [MatrixOneIn_F14], [MatrixOneIn_F21], [MatrixOneIn_F22], [MatrixOneIn_F23], [MatrixOneIn_F24], [MatrixOneIn_G11], [MatrixOneIn_G12], [MatrixOneIn_G13], [MatrixOneIn_G14], [MatrixOneIn_G21], [MatrixOneIn_G22], [MatrixOneIn_G23], [MatrixOneIn_G24], [MatrixOneIn_H11], [MatrixOneIn_H12], [MatrixOneIn_H13], [MatrixOneIn_H14], [MatrixOneIn_H21], [MatrixOneIn_H22], [MatrixOneIn_H23], [MatrixOneIn_H24], [MatrixTwoIn_SelectedOption], [MatrixTwoIn_D21], [MatrixTwoIn_D22], [MatrixTwoIn_D23], [MatrixTwoIn_D24], [MatrixTwoIn_E11], [MatrixTwoIn_E12], [MatrixTwoIn_E13], [MatrixTwoIn_E14], [MatrixTwoIn_E21], [MatrixTwoIn_E22], [MatrixTwoIn_E23], [MatrixTwoIn_E24], [MatrixTwoIn_F11], [MatrixTwoIn_F12], [MatrixTwoIn_F13], [MatrixTwoIn_F14], [MatrixTwoIn_F21], [MatrixTwoIn_F22], [MatrixTwoIn_F23], [MatrixTwoIn_F24], [MatrixTwoIn_G11], [MatrixTwoIn_G12], [MatrixTwoIn_G13], [MatrixTwoIn_G14], [MatrixTwoIn_G21], [MatrixTwoIn_G22], [MatrixTwoIn_G23], [MatrixTwoIn_G24], [MatrixTwoIn_H11], [MatrixTwoIn_H12], [MatrixTwoIn_H13], [MatrixTwoIn_H14], [MatrixTwoIn_H21], [MatrixTwoIn_H22], [MatrixTwoIn_H23], [MatrixTwoIn_H24], [SystemInputVolume_B19], [SystemInputVolumeErrorMargin_B21], [AuthorizedConsumption_K12], [AuthorizedConsumptionErrorMargin_K15], [WaterLosses_K29], [WaterLossesErrorMargin_K31], [BilledAuthorizedConsumption_T8], [UnbilledAuthorizedConsumption_T16], [UnbilledAuthorizedConsumptionErrorMargin_T20], [CommercialLosses_T26], [CommercialLossesErrorMargin_T29], [PhysicalLossesM3_T34], [PhyscialLossesErrorMargin_AH35], [BilledMeteredConsumption_AC4], [BilledUnmeteredConsumption_AC9], [UnbilledMeteredConsumption_AC14], [UnbilledUnmeteredConsumption_AC19], [UnbilledUnmeteredConsumptionErrorMargin_AO20], [UnauthorizedConsumption_AC24], [UnauthorizedConsumptionErrorMargin_AO25], [CustomerMeterInaccuraciesAndErrorsM3_AC29], [CustomerMeterInaccuraciesAndErrorsErrorMargin_AO30], [RevenueWaterM3_AY8], [NonRevenueWaterM3_AY24], [NonRevenueWaterErrorMargin_AY26]) VALUES (26, N'none\none', CAST(N'2021-01-14T09:15:40.203' AS DateTime), N'EMEA\79749', CAST(N'2021-03-15T14:09:32.997' AS DateTime), 6773, 2019, 1, N'', 0, 0, 100, NULL, NULL, NULL, NULL, 1001001, 0.88, 0, 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0.11, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, N'0', 0, 0, 0, 1, 1.5, 2, 4, 8, 2, 4, 8, 16, 25, 50, 100, 200, 25, 50, 100, 200, 50, 100, 200, 400, 40, 75, 150, 300, 75, 150, 300, 600, 50, 100, 200, 400, 100, 200, 400, 800, 60, 125, 250, 500, 125, 250, 500, 1000, 2, 55, 110, 220, 400, 50, 100, 200, 350, 80, 160, 320, 600, 65, 125, 250, 450, 105, 210, 420, 800, 75, 150, 300, 550, 130, 260, 520, 1000, 85, 175, 350, 650, 155, 310, 620, 1200, 1001001, 0.88, 0, 0, 1001001, 0.88, 0, 0, 0, 0, 0, 1001001, 0.88, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1001001, 0.88)
GO
INSERT [dbo].[tbWbEasyCalcData] ([WbEasyCalcDataId], [CreateLogin], [CreateDate], [ModifyLogin], [ModifyDate], [ZoneId], [YearNo], [MonthNo], [Description], [IsArchive], [IsAccepted], [Start_PeriodDays_M21], [SysInput_Desc_B6], [SysInput_Desc_B7], [SysInput_Desc_B8], [SysInput_Desc_B9], [SysInput_SystemInputVolumeM3_D6], [SysInput_SystemInputVolumeError_F6], [SysInput_SystemInputVolumeM3_D7], [SysInput_SystemInputVolumeError_F7], [SysInput_SystemInputVolumeM3_D8], [SysInput_SystemInputVolumeError_F8], [SysInput_SystemInputVolumeM3_D9], [SysInput_SystemInputVolumeError_F9], [BilledCons_Desc_B8], [BilledCons_Desc_B9], [BilledCons_Desc_B10], [BilledCons_Desc_B11], [BilledCons_Desc_F8], [BilledCons_Desc_F9], [BilledCons_Desc_F10], [BilledCons_Desc_F11], [UnbilledCons_Desc_D8], [UnbilledCons_Desc_D9], [UnbilledCons_Desc_D10], [UnbilledCons_Desc_D11], [UnbilledCons_Desc_F6], [UnbilledCons_Desc_F7], [UnbilledCons_Desc_F8], [UnbilledCons_Desc_F9], [UnbilledCons_Desc_F10], [UnbilledCons_Desc_F11], [UnauthCons_Desc_B18], [UnauthCons_Desc_B19], [UnauthCons_Desc_B20], [UnauthCons_Desc_B21], [MetErrors_Desc_D12], [MetErrors_Desc_D13], [MetErrors_Desc_D14], [MetErrors_Desc_D15], [Network_Desc_B7], [Network_Desc_B8], [Network_Desc_B9], [Network_Desc_B10], [Interm_Area_B7], [Interm_Area_B8], [Interm_Area_B9], [Interm_Area_B10], [BilledCons_BilledMetConsBulkWatSupExpM3_D6], [BilledCons_BilledUnmetConsBulkWatSupExpM3_H6], [BilledCons_UnbMetConsM3_D8], [BilledCons_UnbMetConsM3_D9], [BilledCons_UnbMetConsM3_D10], [BilledCons_UnbMetConsM3_D11], [BilledCons_UnbUnmetConsM3_H8], [BilledCons_UnbUnmetConsM3_H9], [BilledCons_UnbUnmetConsM3_H10], [BilledCons_UnbUnmetConsM3_H11], [UnbilledCons_MetConsBulkWatSupExpM3_D6], [UnbilledCons_UnbMetConsM3_D8], [UnbilledCons_UnbMetConsM3_D9], [UnbilledCons_UnbMetConsM3_D10], [UnbilledCons_UnbMetConsM3_D11], [UnbilledCons_UnbUnmetConsM3_H6], [UnbilledCons_UnbUnmetConsM3_H7], [UnbilledCons_UnbUnmetConsM3_H8], [UnbilledCons_UnbUnmetConsM3_H9], [UnbilledCons_UnbUnmetConsM3_H10], [UnbilledCons_UnbUnmetConsM3_H11], [UnbilledCons_UnbUnmetConsError_J6], [UnbilledCons_UnbUnmetConsError_J7], [UnbilledCons_UnbUnmetConsError_J8], [UnbilledCons_UnbUnmetConsError_J9], [UnbilledCons_UnbUnmetConsError_J10], [UnbilledCons_UnbUnmetConsError_J11], [UnauthCons_IllegalConnDomEstNo_D6], [UnauthCons_IllegalConnDomPersPerHouse_H6], [UnauthCons_IllegalConnDomConsLitPerPersDay_J6], [UnauthCons_IllegalConnDomErrorMargin_F6], [UnauthCons_IllegalConnOthersErrorMargin_F10], [IllegalConnectionsOthersEstimatedNumber_D10], [IllegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10], [UnauthCons_MeterTampBypEtcEstNo_D14], [UnauthCons_MeterTampBypEtcErrorMargin_F14], [UnauthCons_MeterTampBypEtcConsLitPerCustDay_J14], [UnauthCons_OthersErrorMargin_F18], [UnauthCons_OthersErrorMargin_F19], [UnauthCons_OthersErrorMargin_F20], [UnauthCons_OthersErrorMargin_F21], [UnauthCons_OthersM3PerDay_J18], [UnauthCons_OthersM3PerDay_J19], [UnauthCons_OthersM3PerDay_J20], [UnauthCons_OthersM3PerDay_J21], [MetErrors_DetailedManualSpec_J6], [MetErrors_BilledMetConsWoBulkSupMetUndrreg_H8], [MetErrors_BilledMetConsWoBulkSupErrorMargin_N8], [MetErrors_Total_F12], [MetErrors_Total_F13], [MetErrors_Total_F14], [MetErrors_Total_F15], [MetErrors_Meter_H12], [MetErrors_Meter_H13], [MetErrors_Meter_H14], [MetErrors_Meter_H15], [MetErrors_Error_N12], [MetErrors_Error_N13], [MetErrors_Error_N14], [MetErrors_Error_N15], [MeteredBulkSupplyExportErrorMargin_N32], [UnbilledMeteredConsumptionWithoutBulkSupplyErrorMargin_N34], [CorruptMeterReadingPracticessErrorMargin_N38], [DataHandlingErrorsOffice_L40], [DataHandlingErrorsOfficeErrorMargin_N40], [MetErrors_MetBulkSupExpMetUnderreg_H32], [MetErrors_UnbillMetConsWoBulkSupplMetUndrreg_H34], [MetErrors_CorruptMetReadPractMetUndrreg_H38], [Network_DistributionAndTransmissionMains_D7], [Network_DistributionAndTransmissionMains_D8], [Network_DistributionAndTransmissionMains_D9], [Network_DistributionAndTransmissionMains_D10], [Network_NoOfConnOfRegCustomers_H10], [Network_NoOfInactAccountsWSvcConns_H18], [Network_AvgLenOfSvcConnFromBoundaryToMeterM_H32], [Network_PossibleUnd_D30], [Network_NoCustomers_H7], [Network_ErrorMargin_J7], [Network_ErrorMargin_J10], [Network_ErrorMargin_J18], [Network_ErrorMargin_J32], [Network_ErrorMargin_D35], [Prs_Area_B7], [Prs_Area_B8], [Prs_Area_B9], [Prs_Area_B10], [Prs_ApproxNoOfConn_D7], [Prs_DailyAvgPrsM_F7], [Prs_ApproxNoOfConn_D8], [Prs_DailyAvgPrsM_F8], [Prs_ApproxNoOfConn_D9], [Prs_DailyAvgPrsM_F9], [Prs_ApproxNoOfConn_D10], [Prs_DailyAvgPrsM_F10], [Prs_ErrorMarg_F26], [Interm_Conn_D7], [Interm_Conn_D8], [Interm_Conn_D9], [Interm_Conn_D10], [Interm_Days_F7], [Interm_Days_F8], [Interm_Days_F9], [Interm_Days_F10], [Interm_Hour_H7], [Interm_Hour_H8], [Interm_Hour_H9], [Interm_Hour_H10], [Interm_ErrorMarg_H26], [FinancData_G6], [FinancData_K6], [FinancData_G8], [FinancData_D26], [FinancData_G35], [MatrixOneIn_SelectedOption], [MatrixOneIn_C11], [MatrixOneIn_C12], [MatrixOneIn_C13], [MatrixOneIn_C14], [MatrixOneIn_C21], [MatrixOneIn_C22], [MatrixOneIn_C23], [MatrixOneIn_C24], [MatrixOneIn_D21], [MatrixOneIn_D22], [MatrixOneIn_D23], [MatrixOneIn_D24], [MatrixOneIn_E11], [MatrixOneIn_E12], [MatrixOneIn_E13], [MatrixOneIn_E14], [MatrixOneIn_E21], [MatrixOneIn_E22], [MatrixOneIn_E23], [MatrixOneIn_E24], [MatrixOneIn_F11], [MatrixOneIn_F12], [MatrixOneIn_F13], [MatrixOneIn_F14], [MatrixOneIn_F21], [MatrixOneIn_F22], [MatrixOneIn_F23], [MatrixOneIn_F24], [MatrixOneIn_G11], [MatrixOneIn_G12], [MatrixOneIn_G13], [MatrixOneIn_G14], [MatrixOneIn_G21], [MatrixOneIn_G22], [MatrixOneIn_G23], [MatrixOneIn_G24], [MatrixOneIn_H11], [MatrixOneIn_H12], [MatrixOneIn_H13], [MatrixOneIn_H14], [MatrixOneIn_H21], [MatrixOneIn_H22], [MatrixOneIn_H23], [MatrixOneIn_H24], [MatrixTwoIn_SelectedOption], [MatrixTwoIn_D21], [MatrixTwoIn_D22], [MatrixTwoIn_D23], [MatrixTwoIn_D24], [MatrixTwoIn_E11], [MatrixTwoIn_E12], [MatrixTwoIn_E13], [MatrixTwoIn_E14], [MatrixTwoIn_E21], [MatrixTwoIn_E22], [MatrixTwoIn_E23], [MatrixTwoIn_E24], [MatrixTwoIn_F11], [MatrixTwoIn_F12], [MatrixTwoIn_F13], [MatrixTwoIn_F14], [MatrixTwoIn_F21], [MatrixTwoIn_F22], [MatrixTwoIn_F23], [MatrixTwoIn_F24], [MatrixTwoIn_G11], [MatrixTwoIn_G12], [MatrixTwoIn_G13], [MatrixTwoIn_G14], [MatrixTwoIn_G21], [MatrixTwoIn_G22], [MatrixTwoIn_G23], [MatrixTwoIn_G24], [MatrixTwoIn_H11], [MatrixTwoIn_H12], [MatrixTwoIn_H13], [MatrixTwoIn_H14], [MatrixTwoIn_H21], [MatrixTwoIn_H22], [MatrixTwoIn_H23], [MatrixTwoIn_H24], [SystemInputVolume_B19], [SystemInputVolumeErrorMargin_B21], [AuthorizedConsumption_K12], [AuthorizedConsumptionErrorMargin_K15], [WaterLosses_K29], [WaterLossesErrorMargin_K31], [BilledAuthorizedConsumption_T8], [UnbilledAuthorizedConsumption_T16], [UnbilledAuthorizedConsumptionErrorMargin_T20], [CommercialLosses_T26], [CommercialLossesErrorMargin_T29], [PhysicalLossesM3_T34], [PhyscialLossesErrorMargin_AH35], [BilledMeteredConsumption_AC4], [BilledUnmeteredConsumption_AC9], [UnbilledMeteredConsumption_AC14], [UnbilledUnmeteredConsumption_AC19], [UnbilledUnmeteredConsumptionErrorMargin_AO20], [UnauthorizedConsumption_AC24], [UnauthorizedConsumptionErrorMargin_AO25], [CustomerMeterInaccuraciesAndErrorsM3_AC29], [CustomerMeterInaccuraciesAndErrorsErrorMargin_AO30], [RevenueWaterM3_AY8], [NonRevenueWaterM3_AY24], [NonRevenueWaterErrorMargin_AY26]) VALUES (31, N'none\none', CAST(N'2021-01-14T09:15:40.203' AS DateTime), N'none\none', CAST(N'2021-01-14T09:15:40.203' AS DateTime), 6773, 2019, 3, N'Generated', 0, 0, 1, NULL, NULL, NULL, NULL, 0, 0.12, 0, 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 70420, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20342.3603638975, 0, 0, 0, 911, 0, 0, 0, 0, 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, 0, 0, 0, 0, 0, 0, 0, 0, NULL, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, N'0', 0, 0, 0, 1, 1.5, 2, 4, 8, 2, 4, 8, 16, 25, 50, 100, 200, 25, 50, 100, 200, 50, 100, 200, 400, 40, 75, 150, 300, 75, 150, 300, 600, 50, 100, 200, 400, 100, 200, 400, 800, 60, 125, 250, 500, 125, 250, 500, 1000, 2, 55, 110, 220, 400, 50, 100, 200, 350, 80, 160, 320, 600, 65, 125, 250, 450, 105, 210, 420, 800, 75, 150, 300, 550, 130, 260, 520, 1000, 85, 175, 350, 650, 155, 310, 620, 1200, 102537, 0.11999999999999998, 68358, 0, 34179, 0.36, 68358, 0, 0, 0, 0, 34179, 0.36, 68358, 0, 0, 0, 0, 0, 0, 0, 0, 68358, 34179, 0.36)
GO
INSERT [dbo].[tbWbEasyCalcData] ([WbEasyCalcDataId], [CreateLogin], [CreateDate], [ModifyLogin], [ModifyDate], [ZoneId], [YearNo], [MonthNo], [Description], [IsArchive], [IsAccepted], [Start_PeriodDays_M21], [SysInput_Desc_B6], [SysInput_Desc_B7], [SysInput_Desc_B8], [SysInput_Desc_B9], [SysInput_SystemInputVolumeM3_D6], [SysInput_SystemInputVolumeError_F6], [SysInput_SystemInputVolumeM3_D7], [SysInput_SystemInputVolumeError_F7], [SysInput_SystemInputVolumeM3_D8], [SysInput_SystemInputVolumeError_F8], [SysInput_SystemInputVolumeM3_D9], [SysInput_SystemInputVolumeError_F9], [BilledCons_Desc_B8], [BilledCons_Desc_B9], [BilledCons_Desc_B10], [BilledCons_Desc_B11], [BilledCons_Desc_F8], [BilledCons_Desc_F9], [BilledCons_Desc_F10], [BilledCons_Desc_F11], [UnbilledCons_Desc_D8], [UnbilledCons_Desc_D9], [UnbilledCons_Desc_D10], [UnbilledCons_Desc_D11], [UnbilledCons_Desc_F6], [UnbilledCons_Desc_F7], [UnbilledCons_Desc_F8], [UnbilledCons_Desc_F9], [UnbilledCons_Desc_F10], [UnbilledCons_Desc_F11], [UnauthCons_Desc_B18], [UnauthCons_Desc_B19], [UnauthCons_Desc_B20], [UnauthCons_Desc_B21], [MetErrors_Desc_D12], [MetErrors_Desc_D13], [MetErrors_Desc_D14], [MetErrors_Desc_D15], [Network_Desc_B7], [Network_Desc_B8], [Network_Desc_B9], [Network_Desc_B10], [Interm_Area_B7], [Interm_Area_B8], [Interm_Area_B9], [Interm_Area_B10], [BilledCons_BilledMetConsBulkWatSupExpM3_D6], [BilledCons_BilledUnmetConsBulkWatSupExpM3_H6], [BilledCons_UnbMetConsM3_D8], [BilledCons_UnbMetConsM3_D9], [BilledCons_UnbMetConsM3_D10], [BilledCons_UnbMetConsM3_D11], [BilledCons_UnbUnmetConsM3_H8], [BilledCons_UnbUnmetConsM3_H9], [BilledCons_UnbUnmetConsM3_H10], [BilledCons_UnbUnmetConsM3_H11], [UnbilledCons_MetConsBulkWatSupExpM3_D6], [UnbilledCons_UnbMetConsM3_D8], [UnbilledCons_UnbMetConsM3_D9], [UnbilledCons_UnbMetConsM3_D10], [UnbilledCons_UnbMetConsM3_D11], [UnbilledCons_UnbUnmetConsM3_H6], [UnbilledCons_UnbUnmetConsM3_H7], [UnbilledCons_UnbUnmetConsM3_H8], [UnbilledCons_UnbUnmetConsM3_H9], [UnbilledCons_UnbUnmetConsM3_H10], [UnbilledCons_UnbUnmetConsM3_H11], [UnbilledCons_UnbUnmetConsError_J6], [UnbilledCons_UnbUnmetConsError_J7], [UnbilledCons_UnbUnmetConsError_J8], [UnbilledCons_UnbUnmetConsError_J9], [UnbilledCons_UnbUnmetConsError_J10], [UnbilledCons_UnbUnmetConsError_J11], [UnauthCons_IllegalConnDomEstNo_D6], [UnauthCons_IllegalConnDomPersPerHouse_H6], [UnauthCons_IllegalConnDomConsLitPerPersDay_J6], [UnauthCons_IllegalConnDomErrorMargin_F6], [UnauthCons_IllegalConnOthersErrorMargin_F10], [IllegalConnectionsOthersEstimatedNumber_D10], [IllegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10], [UnauthCons_MeterTampBypEtcEstNo_D14], [UnauthCons_MeterTampBypEtcErrorMargin_F14], [UnauthCons_MeterTampBypEtcConsLitPerCustDay_J14], [UnauthCons_OthersErrorMargin_F18], [UnauthCons_OthersErrorMargin_F19], [UnauthCons_OthersErrorMargin_F20], [UnauthCons_OthersErrorMargin_F21], [UnauthCons_OthersM3PerDay_J18], [UnauthCons_OthersM3PerDay_J19], [UnauthCons_OthersM3PerDay_J20], [UnauthCons_OthersM3PerDay_J21], [MetErrors_DetailedManualSpec_J6], [MetErrors_BilledMetConsWoBulkSupMetUndrreg_H8], [MetErrors_BilledMetConsWoBulkSupErrorMargin_N8], [MetErrors_Total_F12], [MetErrors_Total_F13], [MetErrors_Total_F14], [MetErrors_Total_F15], [MetErrors_Meter_H12], [MetErrors_Meter_H13], [MetErrors_Meter_H14], [MetErrors_Meter_H15], [MetErrors_Error_N12], [MetErrors_Error_N13], [MetErrors_Error_N14], [MetErrors_Error_N15], [MeteredBulkSupplyExportErrorMargin_N32], [UnbilledMeteredConsumptionWithoutBulkSupplyErrorMargin_N34], [CorruptMeterReadingPracticessErrorMargin_N38], [DataHandlingErrorsOffice_L40], [DataHandlingErrorsOfficeErrorMargin_N40], [MetErrors_MetBulkSupExpMetUnderreg_H32], [MetErrors_UnbillMetConsWoBulkSupplMetUndrreg_H34], [MetErrors_CorruptMetReadPractMetUndrreg_H38], [Network_DistributionAndTransmissionMains_D7], [Network_DistributionAndTransmissionMains_D8], [Network_DistributionAndTransmissionMains_D9], [Network_DistributionAndTransmissionMains_D10], [Network_NoOfConnOfRegCustomers_H10], [Network_NoOfInactAccountsWSvcConns_H18], [Network_AvgLenOfSvcConnFromBoundaryToMeterM_H32], [Network_PossibleUnd_D30], [Network_NoCustomers_H7], [Network_ErrorMargin_J7], [Network_ErrorMargin_J10], [Network_ErrorMargin_J18], [Network_ErrorMargin_J32], [Network_ErrorMargin_D35], [Prs_Area_B7], [Prs_Area_B8], [Prs_Area_B9], [Prs_Area_B10], [Prs_ApproxNoOfConn_D7], [Prs_DailyAvgPrsM_F7], [Prs_ApproxNoOfConn_D8], [Prs_DailyAvgPrsM_F8], [Prs_ApproxNoOfConn_D9], [Prs_DailyAvgPrsM_F9], [Prs_ApproxNoOfConn_D10], [Prs_DailyAvgPrsM_F10], [Prs_ErrorMarg_F26], [Interm_Conn_D7], [Interm_Conn_D8], [Interm_Conn_D9], [Interm_Conn_D10], [Interm_Days_F7], [Interm_Days_F8], [Interm_Days_F9], [Interm_Days_F10], [Interm_Hour_H7], [Interm_Hour_H8], [Interm_Hour_H9], [Interm_Hour_H10], [Interm_ErrorMarg_H26], [FinancData_G6], [FinancData_K6], [FinancData_G8], [FinancData_D26], [FinancData_G35], [MatrixOneIn_SelectedOption], [MatrixOneIn_C11], [MatrixOneIn_C12], [MatrixOneIn_C13], [MatrixOneIn_C14], [MatrixOneIn_C21], [MatrixOneIn_C22], [MatrixOneIn_C23], [MatrixOneIn_C24], [MatrixOneIn_D21], [MatrixOneIn_D22], [MatrixOneIn_D23], [MatrixOneIn_D24], [MatrixOneIn_E11], [MatrixOneIn_E12], [MatrixOneIn_E13], [MatrixOneIn_E14], [MatrixOneIn_E21], [MatrixOneIn_E22], [MatrixOneIn_E23], [MatrixOneIn_E24], [MatrixOneIn_F11], [MatrixOneIn_F12], [MatrixOneIn_F13], [MatrixOneIn_F14], [MatrixOneIn_F21], [MatrixOneIn_F22], [MatrixOneIn_F23], [MatrixOneIn_F24], [MatrixOneIn_G11], [MatrixOneIn_G12], [MatrixOneIn_G13], [MatrixOneIn_G14], [MatrixOneIn_G21], [MatrixOneIn_G22], [MatrixOneIn_G23], [MatrixOneIn_G24], [MatrixOneIn_H11], [MatrixOneIn_H12], [MatrixOneIn_H13], [MatrixOneIn_H14], [MatrixOneIn_H21], [MatrixOneIn_H22], [MatrixOneIn_H23], [MatrixOneIn_H24], [MatrixTwoIn_SelectedOption], [MatrixTwoIn_D21], [MatrixTwoIn_D22], [MatrixTwoIn_D23], [MatrixTwoIn_D24], [MatrixTwoIn_E11], [MatrixTwoIn_E12], [MatrixTwoIn_E13], [MatrixTwoIn_E14], [MatrixTwoIn_E21], [MatrixTwoIn_E22], [MatrixTwoIn_E23], [MatrixTwoIn_E24], [MatrixTwoIn_F11], [MatrixTwoIn_F12], [MatrixTwoIn_F13], [MatrixTwoIn_F14], [MatrixTwoIn_F21], [MatrixTwoIn_F22], [MatrixTwoIn_F23], [MatrixTwoIn_F24], [MatrixTwoIn_G11], [MatrixTwoIn_G12], [MatrixTwoIn_G13], [MatrixTwoIn_G14], [MatrixTwoIn_G21], [MatrixTwoIn_G22], [MatrixTwoIn_G23], [MatrixTwoIn_G24], [MatrixTwoIn_H11], [MatrixTwoIn_H12], [MatrixTwoIn_H13], [MatrixTwoIn_H14], [MatrixTwoIn_H21], [MatrixTwoIn_H22], [MatrixTwoIn_H23], [MatrixTwoIn_H24], [SystemInputVolume_B19], [SystemInputVolumeErrorMargin_B21], [AuthorizedConsumption_K12], [AuthorizedConsumptionErrorMargin_K15], [WaterLosses_K29], [WaterLossesErrorMargin_K31], [BilledAuthorizedConsumption_T8], [UnbilledAuthorizedConsumption_T16], [UnbilledAuthorizedConsumptionErrorMargin_T20], [CommercialLosses_T26], [CommercialLossesErrorMargin_T29], [PhysicalLossesM3_T34], [PhyscialLossesErrorMargin_AH35], [BilledMeteredConsumption_AC4], [BilledUnmeteredConsumption_AC9], [UnbilledMeteredConsumption_AC14], [UnbilledUnmeteredConsumption_AC19], [UnbilledUnmeteredConsumptionErrorMargin_AO20], [UnauthorizedConsumption_AC24], [UnauthorizedConsumptionErrorMargin_AO25], [CustomerMeterInaccuraciesAndErrorsM3_AC29], [CustomerMeterInaccuraciesAndErrorsErrorMargin_AO30], [RevenueWaterM3_AY8], [NonRevenueWaterM3_AY24], [NonRevenueWaterErrorMargin_AY26]) VALUES (32, N'none\none', CAST(N'2021-01-14T09:15:40.203' AS DateTime), N'none\none', CAST(N'2021-01-14T09:15:40.203' AS DateTime), 6774, 2019, 2, N'Generated', 0, 0, 1, NULL, NULL, NULL, NULL, 0, 0.05, 0, 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, 1000, 0, 0, 0, 0, 0, 0, 0, 0, 309349, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 3, 120, 0.05, 0, 0, 0, 1000, 0.1, 160, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0.03, 0.02, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0.03, 0.03, 0.03, 0, 0, 0, 0, 0, 1500, 7, 0, 0, 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, 9500, 30, 0, 0, 0, 0, 0, 0, NULL, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, N'0', 0, 0, 0, 1, 1.5, 2, 4, 8, 2, 4, 8, 16, 25, 50, 100, 200, 25, 50, 100, 200, 50, 100, 200, 400, 40, 75, 150, 300, 75, 150, 300, 600, 50, 100, 200, 400, 100, 200, 400, 800, 60, 125, 250, 500, 125, 250, 500, 1000, 2, 55, 110, 220, 400, 50, 100, 200, 350, 80, 160, 320, 600, 65, 125, 250, 450, 105, 210, 420, 800, 75, 150, 300, 550, 130, 260, 520, 1000, 85, 175, 350, 650, 155, 310, 620, 1200, 6593339, 0.05, 5642375, 0, 950964, 0.34666606727489158, 5333026, 309349, 0, 341766.46391752549, 0.00056533100874221894, 609197.53608247451, 0.54114960598586614, 5332026, 1000, 309349, 0, 0, 2352, 0.082147610459505041, 339414.46391752549, 0, 5333026, 1260313, 0.26157545784261532)
GO
INSERT [dbo].[tbWbEasyCalcData] ([WbEasyCalcDataId], [CreateLogin], [CreateDate], [ModifyLogin], [ModifyDate], [ZoneId], [YearNo], [MonthNo], [Description], [IsArchive], [IsAccepted], [Start_PeriodDays_M21], [SysInput_Desc_B6], [SysInput_Desc_B7], [SysInput_Desc_B8], [SysInput_Desc_B9], [SysInput_SystemInputVolumeM3_D6], [SysInput_SystemInputVolumeError_F6], [SysInput_SystemInputVolumeM3_D7], [SysInput_SystemInputVolumeError_F7], [SysInput_SystemInputVolumeM3_D8], [SysInput_SystemInputVolumeError_F8], [SysInput_SystemInputVolumeM3_D9], [SysInput_SystemInputVolumeError_F9], [BilledCons_Desc_B8], [BilledCons_Desc_B9], [BilledCons_Desc_B10], [BilledCons_Desc_B11], [BilledCons_Desc_F8], [BilledCons_Desc_F9], [BilledCons_Desc_F10], [BilledCons_Desc_F11], [UnbilledCons_Desc_D8], [UnbilledCons_Desc_D9], [UnbilledCons_Desc_D10], [UnbilledCons_Desc_D11], [UnbilledCons_Desc_F6], [UnbilledCons_Desc_F7], [UnbilledCons_Desc_F8], [UnbilledCons_Desc_F9], [UnbilledCons_Desc_F10], [UnbilledCons_Desc_F11], [UnauthCons_Desc_B18], [UnauthCons_Desc_B19], [UnauthCons_Desc_B20], [UnauthCons_Desc_B21], [MetErrors_Desc_D12], [MetErrors_Desc_D13], [MetErrors_Desc_D14], [MetErrors_Desc_D15], [Network_Desc_B7], [Network_Desc_B8], [Network_Desc_B9], [Network_Desc_B10], [Interm_Area_B7], [Interm_Area_B8], [Interm_Area_B9], [Interm_Area_B10], [BilledCons_BilledMetConsBulkWatSupExpM3_D6], [BilledCons_BilledUnmetConsBulkWatSupExpM3_H6], [BilledCons_UnbMetConsM3_D8], [BilledCons_UnbMetConsM3_D9], [BilledCons_UnbMetConsM3_D10], [BilledCons_UnbMetConsM3_D11], [BilledCons_UnbUnmetConsM3_H8], [BilledCons_UnbUnmetConsM3_H9], [BilledCons_UnbUnmetConsM3_H10], [BilledCons_UnbUnmetConsM3_H11], [UnbilledCons_MetConsBulkWatSupExpM3_D6], [UnbilledCons_UnbMetConsM3_D8], [UnbilledCons_UnbMetConsM3_D9], [UnbilledCons_UnbMetConsM3_D10], [UnbilledCons_UnbMetConsM3_D11], [UnbilledCons_UnbUnmetConsM3_H6], [UnbilledCons_UnbUnmetConsM3_H7], [UnbilledCons_UnbUnmetConsM3_H8], [UnbilledCons_UnbUnmetConsM3_H9], [UnbilledCons_UnbUnmetConsM3_H10], [UnbilledCons_UnbUnmetConsM3_H11], [UnbilledCons_UnbUnmetConsError_J6], [UnbilledCons_UnbUnmetConsError_J7], [UnbilledCons_UnbUnmetConsError_J8], [UnbilledCons_UnbUnmetConsError_J9], [UnbilledCons_UnbUnmetConsError_J10], [UnbilledCons_UnbUnmetConsError_J11], [UnauthCons_IllegalConnDomEstNo_D6], [UnauthCons_IllegalConnDomPersPerHouse_H6], [UnauthCons_IllegalConnDomConsLitPerPersDay_J6], [UnauthCons_IllegalConnDomErrorMargin_F6], [UnauthCons_IllegalConnOthersErrorMargin_F10], [IllegalConnectionsOthersEstimatedNumber_D10], [IllegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10], [UnauthCons_MeterTampBypEtcEstNo_D14], [UnauthCons_MeterTampBypEtcErrorMargin_F14], [UnauthCons_MeterTampBypEtcConsLitPerCustDay_J14], [UnauthCons_OthersErrorMargin_F18], [UnauthCons_OthersErrorMargin_F19], [UnauthCons_OthersErrorMargin_F20], [UnauthCons_OthersErrorMargin_F21], [UnauthCons_OthersM3PerDay_J18], [UnauthCons_OthersM3PerDay_J19], [UnauthCons_OthersM3PerDay_J20], [UnauthCons_OthersM3PerDay_J21], [MetErrors_DetailedManualSpec_J6], [MetErrors_BilledMetConsWoBulkSupMetUndrreg_H8], [MetErrors_BilledMetConsWoBulkSupErrorMargin_N8], [MetErrors_Total_F12], [MetErrors_Total_F13], [MetErrors_Total_F14], [MetErrors_Total_F15], [MetErrors_Meter_H12], [MetErrors_Meter_H13], [MetErrors_Meter_H14], [MetErrors_Meter_H15], [MetErrors_Error_N12], [MetErrors_Error_N13], [MetErrors_Error_N14], [MetErrors_Error_N15], [MeteredBulkSupplyExportErrorMargin_N32], [UnbilledMeteredConsumptionWithoutBulkSupplyErrorMargin_N34], [CorruptMeterReadingPracticessErrorMargin_N38], [DataHandlingErrorsOffice_L40], [DataHandlingErrorsOfficeErrorMargin_N40], [MetErrors_MetBulkSupExpMetUnderreg_H32], [MetErrors_UnbillMetConsWoBulkSupplMetUndrreg_H34], [MetErrors_CorruptMetReadPractMetUndrreg_H38], [Network_DistributionAndTransmissionMains_D7], [Network_DistributionAndTransmissionMains_D8], [Network_DistributionAndTransmissionMains_D9], [Network_DistributionAndTransmissionMains_D10], [Network_NoOfConnOfRegCustomers_H10], [Network_NoOfInactAccountsWSvcConns_H18], [Network_AvgLenOfSvcConnFromBoundaryToMeterM_H32], [Network_PossibleUnd_D30], [Network_NoCustomers_H7], [Network_ErrorMargin_J7], [Network_ErrorMargin_J10], [Network_ErrorMargin_J18], [Network_ErrorMargin_J32], [Network_ErrorMargin_D35], [Prs_Area_B7], [Prs_Area_B8], [Prs_Area_B9], [Prs_Area_B10], [Prs_ApproxNoOfConn_D7], [Prs_DailyAvgPrsM_F7], [Prs_ApproxNoOfConn_D8], [Prs_DailyAvgPrsM_F8], [Prs_ApproxNoOfConn_D9], [Prs_DailyAvgPrsM_F9], [Prs_ApproxNoOfConn_D10], [Prs_DailyAvgPrsM_F10], [Prs_ErrorMarg_F26], [Interm_Conn_D7], [Interm_Conn_D8], [Interm_Conn_D9], [Interm_Conn_D10], [Interm_Days_F7], [Interm_Days_F8], [Interm_Days_F9], [Interm_Days_F10], [Interm_Hour_H7], [Interm_Hour_H8], [Interm_Hour_H9], [Interm_Hour_H10], [Interm_ErrorMarg_H26], [FinancData_G6], [FinancData_K6], [FinancData_G8], [FinancData_D26], [FinancData_G35], [MatrixOneIn_SelectedOption], [MatrixOneIn_C11], [MatrixOneIn_C12], [MatrixOneIn_C13], [MatrixOneIn_C14], [MatrixOneIn_C21], [MatrixOneIn_C22], [MatrixOneIn_C23], [MatrixOneIn_C24], [MatrixOneIn_D21], [MatrixOneIn_D22], [MatrixOneIn_D23], [MatrixOneIn_D24], [MatrixOneIn_E11], [MatrixOneIn_E12], [MatrixOneIn_E13], [MatrixOneIn_E14], [MatrixOneIn_E21], [MatrixOneIn_E22], [MatrixOneIn_E23], [MatrixOneIn_E24], [MatrixOneIn_F11], [MatrixOneIn_F12], [MatrixOneIn_F13], [MatrixOneIn_F14], [MatrixOneIn_F21], [MatrixOneIn_F22], [MatrixOneIn_F23], [MatrixOneIn_F24], [MatrixOneIn_G11], [MatrixOneIn_G12], [MatrixOneIn_G13], [MatrixOneIn_G14], [MatrixOneIn_G21], [MatrixOneIn_G22], [MatrixOneIn_G23], [MatrixOneIn_G24], [MatrixOneIn_H11], [MatrixOneIn_H12], [MatrixOneIn_H13], [MatrixOneIn_H14], [MatrixOneIn_H21], [MatrixOneIn_H22], [MatrixOneIn_H23], [MatrixOneIn_H24], [MatrixTwoIn_SelectedOption], [MatrixTwoIn_D21], [MatrixTwoIn_D22], [MatrixTwoIn_D23], [MatrixTwoIn_D24], [MatrixTwoIn_E11], [MatrixTwoIn_E12], [MatrixTwoIn_E13], [MatrixTwoIn_E14], [MatrixTwoIn_E21], [MatrixTwoIn_E22], [MatrixTwoIn_E23], [MatrixTwoIn_E24], [MatrixTwoIn_F11], [MatrixTwoIn_F12], [MatrixTwoIn_F13], [MatrixTwoIn_F14], [MatrixTwoIn_F21], [MatrixTwoIn_F22], [MatrixTwoIn_F23], [MatrixTwoIn_F24], [MatrixTwoIn_G11], [MatrixTwoIn_G12], [MatrixTwoIn_G13], [MatrixTwoIn_G14], [MatrixTwoIn_G21], [MatrixTwoIn_G22], [MatrixTwoIn_G23], [MatrixTwoIn_G24], [MatrixTwoIn_H11], [MatrixTwoIn_H12], [MatrixTwoIn_H13], [MatrixTwoIn_H14], [MatrixTwoIn_H21], [MatrixTwoIn_H22], [MatrixTwoIn_H23], [MatrixTwoIn_H24], [SystemInputVolume_B19], [SystemInputVolumeErrorMargin_B21], [AuthorizedConsumption_K12], [AuthorizedConsumptionErrorMargin_K15], [WaterLosses_K29], [WaterLossesErrorMargin_K31], [BilledAuthorizedConsumption_T8], [UnbilledAuthorizedConsumption_T16], [UnbilledAuthorizedConsumptionErrorMargin_T20], [CommercialLosses_T26], [CommercialLossesErrorMargin_T29], [PhysicalLossesM3_T34], [PhyscialLossesErrorMargin_AH35], [BilledMeteredConsumption_AC4], [BilledUnmeteredConsumption_AC9], [UnbilledMeteredConsumption_AC14], [UnbilledUnmeteredConsumption_AC19], [UnbilledUnmeteredConsumptionErrorMargin_AO20], [UnauthorizedConsumption_AC24], [UnauthorizedConsumptionErrorMargin_AO25], [CustomerMeterInaccuraciesAndErrorsM3_AC29], [CustomerMeterInaccuraciesAndErrorsErrorMargin_AO30], [RevenueWaterM3_AY8], [NonRevenueWaterM3_AY24], [NonRevenueWaterErrorMargin_AY26]) VALUES (33, N'none\none', CAST(N'2021-01-14T09:15:40.203' AS DateTime), N'none\none', CAST(N'2021-01-14T09:15:40.203' AS DateTime), 6773, 2019, 3, N'Generated', 1, 0, 1, NULL, NULL, NULL, NULL, 0, 0.12, 0, 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 70420, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20342.3603638975, 0, 0, 0, 911, 0, 0, 0, 0, 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, 0, 0, 0, 0, 0, 0, 0, 0, NULL, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, N'0', 0, 0, 0, 1, 1.5, 2, 4, 8, 2, 4, 8, 16, 25, 50, 100, 200, 25, 50, 100, 200, 50, 100, 200, 400, 40, 75, 150, 300, 75, 150, 300, 600, 50, 100, 200, 400, 100, 200, 400, 800, 60, 125, 250, 500, 125, 250, 500, 1000, 2, 55, 110, 220, 400, 50, 100, 200, 350, 80, 160, 320, 600, 65, 125, 250, 450, 105, 210, 420, 800, 75, 150, 300, 550, 130, 260, 520, 1000, 85, 175, 350, 650, 155, 310, 620, 1200, 102537, 0.11999999999999998, 68358, 0, 34179, 0.36, 68358, 0, 0, 0, 0, 34179, 0.36, 68358, 0, 0, 0, 0, 0, 0, 0, 0, 68358, 34179, 0.36)
GO
INSERT [dbo].[tbWbEasyCalcData] ([WbEasyCalcDataId], [CreateLogin], [CreateDate], [ModifyLogin], [ModifyDate], [ZoneId], [YearNo], [MonthNo], [Description], [IsArchive], [IsAccepted], [Start_PeriodDays_M21], [SysInput_Desc_B6], [SysInput_Desc_B7], [SysInput_Desc_B8], [SysInput_Desc_B9], [SysInput_SystemInputVolumeM3_D6], [SysInput_SystemInputVolumeError_F6], [SysInput_SystemInputVolumeM3_D7], [SysInput_SystemInputVolumeError_F7], [SysInput_SystemInputVolumeM3_D8], [SysInput_SystemInputVolumeError_F8], [SysInput_SystemInputVolumeM3_D9], [SysInput_SystemInputVolumeError_F9], [BilledCons_Desc_B8], [BilledCons_Desc_B9], [BilledCons_Desc_B10], [BilledCons_Desc_B11], [BilledCons_Desc_F8], [BilledCons_Desc_F9], [BilledCons_Desc_F10], [BilledCons_Desc_F11], [UnbilledCons_Desc_D8], [UnbilledCons_Desc_D9], [UnbilledCons_Desc_D10], [UnbilledCons_Desc_D11], [UnbilledCons_Desc_F6], [UnbilledCons_Desc_F7], [UnbilledCons_Desc_F8], [UnbilledCons_Desc_F9], [UnbilledCons_Desc_F10], [UnbilledCons_Desc_F11], [UnauthCons_Desc_B18], [UnauthCons_Desc_B19], [UnauthCons_Desc_B20], [UnauthCons_Desc_B21], [MetErrors_Desc_D12], [MetErrors_Desc_D13], [MetErrors_Desc_D14], [MetErrors_Desc_D15], [Network_Desc_B7], [Network_Desc_B8], [Network_Desc_B9], [Network_Desc_B10], [Interm_Area_B7], [Interm_Area_B8], [Interm_Area_B9], [Interm_Area_B10], [BilledCons_BilledMetConsBulkWatSupExpM3_D6], [BilledCons_BilledUnmetConsBulkWatSupExpM3_H6], [BilledCons_UnbMetConsM3_D8], [BilledCons_UnbMetConsM3_D9], [BilledCons_UnbMetConsM3_D10], [BilledCons_UnbMetConsM3_D11], [BilledCons_UnbUnmetConsM3_H8], [BilledCons_UnbUnmetConsM3_H9], [BilledCons_UnbUnmetConsM3_H10], [BilledCons_UnbUnmetConsM3_H11], [UnbilledCons_MetConsBulkWatSupExpM3_D6], [UnbilledCons_UnbMetConsM3_D8], [UnbilledCons_UnbMetConsM3_D9], [UnbilledCons_UnbMetConsM3_D10], [UnbilledCons_UnbMetConsM3_D11], [UnbilledCons_UnbUnmetConsM3_H6], [UnbilledCons_UnbUnmetConsM3_H7], [UnbilledCons_UnbUnmetConsM3_H8], [UnbilledCons_UnbUnmetConsM3_H9], [UnbilledCons_UnbUnmetConsM3_H10], [UnbilledCons_UnbUnmetConsM3_H11], [UnbilledCons_UnbUnmetConsError_J6], [UnbilledCons_UnbUnmetConsError_J7], [UnbilledCons_UnbUnmetConsError_J8], [UnbilledCons_UnbUnmetConsError_J9], [UnbilledCons_UnbUnmetConsError_J10], [UnbilledCons_UnbUnmetConsError_J11], [UnauthCons_IllegalConnDomEstNo_D6], [UnauthCons_IllegalConnDomPersPerHouse_H6], [UnauthCons_IllegalConnDomConsLitPerPersDay_J6], [UnauthCons_IllegalConnDomErrorMargin_F6], [UnauthCons_IllegalConnOthersErrorMargin_F10], [IllegalConnectionsOthersEstimatedNumber_D10], [IllegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10], [UnauthCons_MeterTampBypEtcEstNo_D14], [UnauthCons_MeterTampBypEtcErrorMargin_F14], [UnauthCons_MeterTampBypEtcConsLitPerCustDay_J14], [UnauthCons_OthersErrorMargin_F18], [UnauthCons_OthersErrorMargin_F19], [UnauthCons_OthersErrorMargin_F20], [UnauthCons_OthersErrorMargin_F21], [UnauthCons_OthersM3PerDay_J18], [UnauthCons_OthersM3PerDay_J19], [UnauthCons_OthersM3PerDay_J20], [UnauthCons_OthersM3PerDay_J21], [MetErrors_DetailedManualSpec_J6], [MetErrors_BilledMetConsWoBulkSupMetUndrreg_H8], [MetErrors_BilledMetConsWoBulkSupErrorMargin_N8], [MetErrors_Total_F12], [MetErrors_Total_F13], [MetErrors_Total_F14], [MetErrors_Total_F15], [MetErrors_Meter_H12], [MetErrors_Meter_H13], [MetErrors_Meter_H14], [MetErrors_Meter_H15], [MetErrors_Error_N12], [MetErrors_Error_N13], [MetErrors_Error_N14], [MetErrors_Error_N15], [MeteredBulkSupplyExportErrorMargin_N32], [UnbilledMeteredConsumptionWithoutBulkSupplyErrorMargin_N34], [CorruptMeterReadingPracticessErrorMargin_N38], [DataHandlingErrorsOffice_L40], [DataHandlingErrorsOfficeErrorMargin_N40], [MetErrors_MetBulkSupExpMetUnderreg_H32], [MetErrors_UnbillMetConsWoBulkSupplMetUndrreg_H34], [MetErrors_CorruptMetReadPractMetUndrreg_H38], [Network_DistributionAndTransmissionMains_D7], [Network_DistributionAndTransmissionMains_D8], [Network_DistributionAndTransmissionMains_D9], [Network_DistributionAndTransmissionMains_D10], [Network_NoOfConnOfRegCustomers_H10], [Network_NoOfInactAccountsWSvcConns_H18], [Network_AvgLenOfSvcConnFromBoundaryToMeterM_H32], [Network_PossibleUnd_D30], [Network_NoCustomers_H7], [Network_ErrorMargin_J7], [Network_ErrorMargin_J10], [Network_ErrorMargin_J18], [Network_ErrorMargin_J32], [Network_ErrorMargin_D35], [Prs_Area_B7], [Prs_Area_B8], [Prs_Area_B9], [Prs_Area_B10], [Prs_ApproxNoOfConn_D7], [Prs_DailyAvgPrsM_F7], [Prs_ApproxNoOfConn_D8], [Prs_DailyAvgPrsM_F8], [Prs_ApproxNoOfConn_D9], [Prs_DailyAvgPrsM_F9], [Prs_ApproxNoOfConn_D10], [Prs_DailyAvgPrsM_F10], [Prs_ErrorMarg_F26], [Interm_Conn_D7], [Interm_Conn_D8], [Interm_Conn_D9], [Interm_Conn_D10], [Interm_Days_F7], [Interm_Days_F8], [Interm_Days_F9], [Interm_Days_F10], [Interm_Hour_H7], [Interm_Hour_H8], [Interm_Hour_H9], [Interm_Hour_H10], [Interm_ErrorMarg_H26], [FinancData_G6], [FinancData_K6], [FinancData_G8], [FinancData_D26], [FinancData_G35], [MatrixOneIn_SelectedOption], [MatrixOneIn_C11], [MatrixOneIn_C12], [MatrixOneIn_C13], [MatrixOneIn_C14], [MatrixOneIn_C21], [MatrixOneIn_C22], [MatrixOneIn_C23], [MatrixOneIn_C24], [MatrixOneIn_D21], [MatrixOneIn_D22], [MatrixOneIn_D23], [MatrixOneIn_D24], [MatrixOneIn_E11], [MatrixOneIn_E12], [MatrixOneIn_E13], [MatrixOneIn_E14], [MatrixOneIn_E21], [MatrixOneIn_E22], [MatrixOneIn_E23], [MatrixOneIn_E24], [MatrixOneIn_F11], [MatrixOneIn_F12], [MatrixOneIn_F13], [MatrixOneIn_F14], [MatrixOneIn_F21], [MatrixOneIn_F22], [MatrixOneIn_F23], [MatrixOneIn_F24], [MatrixOneIn_G11], [MatrixOneIn_G12], [MatrixOneIn_G13], [MatrixOneIn_G14], [MatrixOneIn_G21], [MatrixOneIn_G22], [MatrixOneIn_G23], [MatrixOneIn_G24], [MatrixOneIn_H11], [MatrixOneIn_H12], [MatrixOneIn_H13], [MatrixOneIn_H14], [MatrixOneIn_H21], [MatrixOneIn_H22], [MatrixOneIn_H23], [MatrixOneIn_H24], [MatrixTwoIn_SelectedOption], [MatrixTwoIn_D21], [MatrixTwoIn_D22], [MatrixTwoIn_D23], [MatrixTwoIn_D24], [MatrixTwoIn_E11], [MatrixTwoIn_E12], [MatrixTwoIn_E13], [MatrixTwoIn_E14], [MatrixTwoIn_E21], [MatrixTwoIn_E22], [MatrixTwoIn_E23], [MatrixTwoIn_E24], [MatrixTwoIn_F11], [MatrixTwoIn_F12], [MatrixTwoIn_F13], [MatrixTwoIn_F14], [MatrixTwoIn_F21], [MatrixTwoIn_F22], [MatrixTwoIn_F23], [MatrixTwoIn_F24], [MatrixTwoIn_G11], [MatrixTwoIn_G12], [MatrixTwoIn_G13], [MatrixTwoIn_G14], [MatrixTwoIn_G21], [MatrixTwoIn_G22], [MatrixTwoIn_G23], [MatrixTwoIn_G24], [MatrixTwoIn_H11], [MatrixTwoIn_H12], [MatrixTwoIn_H13], [MatrixTwoIn_H14], [MatrixTwoIn_H21], [MatrixTwoIn_H22], [MatrixTwoIn_H23], [MatrixTwoIn_H24], [SystemInputVolume_B19], [SystemInputVolumeErrorMargin_B21], [AuthorizedConsumption_K12], [AuthorizedConsumptionErrorMargin_K15], [WaterLosses_K29], [WaterLossesErrorMargin_K31], [BilledAuthorizedConsumption_T8], [UnbilledAuthorizedConsumption_T16], [UnbilledAuthorizedConsumptionErrorMargin_T20], [CommercialLosses_T26], [CommercialLossesErrorMargin_T29], [PhysicalLossesM3_T34], [PhyscialLossesErrorMargin_AH35], [BilledMeteredConsumption_AC4], [BilledUnmeteredConsumption_AC9], [UnbilledMeteredConsumption_AC14], [UnbilledUnmeteredConsumption_AC19], [UnbilledUnmeteredConsumptionErrorMargin_AO20], [UnauthorizedConsumption_AC24], [UnauthorizedConsumptionErrorMargin_AO25], [CustomerMeterInaccuraciesAndErrorsM3_AC29], [CustomerMeterInaccuraciesAndErrorsErrorMargin_AO30], [RevenueWaterM3_AY8], [NonRevenueWaterM3_AY24], [NonRevenueWaterErrorMargin_AY26]) VALUES (34, N'none\none', CAST(N'2021-01-14T09:15:40.203' AS DateTime), N'none\none', CAST(N'2021-01-14T09:15:40.203' AS DateTime), 6774, 2019, 2, N'Generated', 1, 0, 1, NULL, NULL, NULL, NULL, 0, 0.05, 0, 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, 1000, 0, 0, 0, 0, 0, 0, 0, 0, 309349, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 3, 120, 0.05, 0, 0, 0, 1000, 0.1, 160, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0.03, 0.02, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0.03, 0.03, 0.03, 0, 0, 0, 0, 0, 1500, 7, 0, 0, 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, 9500, 30, 0, 0, 0, 0, 0, 0, NULL, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, N'0', 0, 0, 0, 1, 1.5, 2, 4, 8, 2, 4, 8, 16, 25, 50, 100, 200, 25, 50, 100, 200, 50, 100, 200, 400, 40, 75, 150, 300, 75, 150, 300, 600, 50, 100, 200, 400, 100, 200, 400, 800, 60, 125, 250, 500, 125, 250, 500, 1000, 2, 55, 110, 220, 400, 50, 100, 200, 350, 80, 160, 320, 600, 65, 125, 250, 450, 105, 210, 420, 800, 75, 150, 300, 550, 130, 260, 520, 1000, 85, 175, 350, 650, 155, 310, 620, 1200, 6593339, 0.05, 5642375, 0, 950964, 0.34666606727489158, 5333026, 309349, 0, 341766.46391752549, 0.00056533100874221894, 609197.53608247451, 0.54114960598586614, 5332026, 1000, 309349, 0, 0, 2352, 0.082147610459505041, 339414.46391752549, 0, 5333026, 1260313, 0.26157545784261532)
GO
INSERT [dbo].[tbWbEasyCalcData] ([WbEasyCalcDataId], [CreateLogin], [CreateDate], [ModifyLogin], [ModifyDate], [ZoneId], [YearNo], [MonthNo], [Description], [IsArchive], [IsAccepted], [Start_PeriodDays_M21], [SysInput_Desc_B6], [SysInput_Desc_B7], [SysInput_Desc_B8], [SysInput_Desc_B9], [SysInput_SystemInputVolumeM3_D6], [SysInput_SystemInputVolumeError_F6], [SysInput_SystemInputVolumeM3_D7], [SysInput_SystemInputVolumeError_F7], [SysInput_SystemInputVolumeM3_D8], [SysInput_SystemInputVolumeError_F8], [SysInput_SystemInputVolumeM3_D9], [SysInput_SystemInputVolumeError_F9], [BilledCons_Desc_B8], [BilledCons_Desc_B9], [BilledCons_Desc_B10], [BilledCons_Desc_B11], [BilledCons_Desc_F8], [BilledCons_Desc_F9], [BilledCons_Desc_F10], [BilledCons_Desc_F11], [UnbilledCons_Desc_D8], [UnbilledCons_Desc_D9], [UnbilledCons_Desc_D10], [UnbilledCons_Desc_D11], [UnbilledCons_Desc_F6], [UnbilledCons_Desc_F7], [UnbilledCons_Desc_F8], [UnbilledCons_Desc_F9], [UnbilledCons_Desc_F10], [UnbilledCons_Desc_F11], [UnauthCons_Desc_B18], [UnauthCons_Desc_B19], [UnauthCons_Desc_B20], [UnauthCons_Desc_B21], [MetErrors_Desc_D12], [MetErrors_Desc_D13], [MetErrors_Desc_D14], [MetErrors_Desc_D15], [Network_Desc_B7], [Network_Desc_B8], [Network_Desc_B9], [Network_Desc_B10], [Interm_Area_B7], [Interm_Area_B8], [Interm_Area_B9], [Interm_Area_B10], [BilledCons_BilledMetConsBulkWatSupExpM3_D6], [BilledCons_BilledUnmetConsBulkWatSupExpM3_H6], [BilledCons_UnbMetConsM3_D8], [BilledCons_UnbMetConsM3_D9], [BilledCons_UnbMetConsM3_D10], [BilledCons_UnbMetConsM3_D11], [BilledCons_UnbUnmetConsM3_H8], [BilledCons_UnbUnmetConsM3_H9], [BilledCons_UnbUnmetConsM3_H10], [BilledCons_UnbUnmetConsM3_H11], [UnbilledCons_MetConsBulkWatSupExpM3_D6], [UnbilledCons_UnbMetConsM3_D8], [UnbilledCons_UnbMetConsM3_D9], [UnbilledCons_UnbMetConsM3_D10], [UnbilledCons_UnbMetConsM3_D11], [UnbilledCons_UnbUnmetConsM3_H6], [UnbilledCons_UnbUnmetConsM3_H7], [UnbilledCons_UnbUnmetConsM3_H8], [UnbilledCons_UnbUnmetConsM3_H9], [UnbilledCons_UnbUnmetConsM3_H10], [UnbilledCons_UnbUnmetConsM3_H11], [UnbilledCons_UnbUnmetConsError_J6], [UnbilledCons_UnbUnmetConsError_J7], [UnbilledCons_UnbUnmetConsError_J8], [UnbilledCons_UnbUnmetConsError_J9], [UnbilledCons_UnbUnmetConsError_J10], [UnbilledCons_UnbUnmetConsError_J11], [UnauthCons_IllegalConnDomEstNo_D6], [UnauthCons_IllegalConnDomPersPerHouse_H6], [UnauthCons_IllegalConnDomConsLitPerPersDay_J6], [UnauthCons_IllegalConnDomErrorMargin_F6], [UnauthCons_IllegalConnOthersErrorMargin_F10], [IllegalConnectionsOthersEstimatedNumber_D10], [IllegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10], [UnauthCons_MeterTampBypEtcEstNo_D14], [UnauthCons_MeterTampBypEtcErrorMargin_F14], [UnauthCons_MeterTampBypEtcConsLitPerCustDay_J14], [UnauthCons_OthersErrorMargin_F18], [UnauthCons_OthersErrorMargin_F19], [UnauthCons_OthersErrorMargin_F20], [UnauthCons_OthersErrorMargin_F21], [UnauthCons_OthersM3PerDay_J18], [UnauthCons_OthersM3PerDay_J19], [UnauthCons_OthersM3PerDay_J20], [UnauthCons_OthersM3PerDay_J21], [MetErrors_DetailedManualSpec_J6], [MetErrors_BilledMetConsWoBulkSupMetUndrreg_H8], [MetErrors_BilledMetConsWoBulkSupErrorMargin_N8], [MetErrors_Total_F12], [MetErrors_Total_F13], [MetErrors_Total_F14], [MetErrors_Total_F15], [MetErrors_Meter_H12], [MetErrors_Meter_H13], [MetErrors_Meter_H14], [MetErrors_Meter_H15], [MetErrors_Error_N12], [MetErrors_Error_N13], [MetErrors_Error_N14], [MetErrors_Error_N15], [MeteredBulkSupplyExportErrorMargin_N32], [UnbilledMeteredConsumptionWithoutBulkSupplyErrorMargin_N34], [CorruptMeterReadingPracticessErrorMargin_N38], [DataHandlingErrorsOffice_L40], [DataHandlingErrorsOfficeErrorMargin_N40], [MetErrors_MetBulkSupExpMetUnderreg_H32], [MetErrors_UnbillMetConsWoBulkSupplMetUndrreg_H34], [MetErrors_CorruptMetReadPractMetUndrreg_H38], [Network_DistributionAndTransmissionMains_D7], [Network_DistributionAndTransmissionMains_D8], [Network_DistributionAndTransmissionMains_D9], [Network_DistributionAndTransmissionMains_D10], [Network_NoOfConnOfRegCustomers_H10], [Network_NoOfInactAccountsWSvcConns_H18], [Network_AvgLenOfSvcConnFromBoundaryToMeterM_H32], [Network_PossibleUnd_D30], [Network_NoCustomers_H7], [Network_ErrorMargin_J7], [Network_ErrorMargin_J10], [Network_ErrorMargin_J18], [Network_ErrorMargin_J32], [Network_ErrorMargin_D35], [Prs_Area_B7], [Prs_Area_B8], [Prs_Area_B9], [Prs_Area_B10], [Prs_ApproxNoOfConn_D7], [Prs_DailyAvgPrsM_F7], [Prs_ApproxNoOfConn_D8], [Prs_DailyAvgPrsM_F8], [Prs_ApproxNoOfConn_D9], [Prs_DailyAvgPrsM_F9], [Prs_ApproxNoOfConn_D10], [Prs_DailyAvgPrsM_F10], [Prs_ErrorMarg_F26], [Interm_Conn_D7], [Interm_Conn_D8], [Interm_Conn_D9], [Interm_Conn_D10], [Interm_Days_F7], [Interm_Days_F8], [Interm_Days_F9], [Interm_Days_F10], [Interm_Hour_H7], [Interm_Hour_H8], [Interm_Hour_H9], [Interm_Hour_H10], [Interm_ErrorMarg_H26], [FinancData_G6], [FinancData_K6], [FinancData_G8], [FinancData_D26], [FinancData_G35], [MatrixOneIn_SelectedOption], [MatrixOneIn_C11], [MatrixOneIn_C12], [MatrixOneIn_C13], [MatrixOneIn_C14], [MatrixOneIn_C21], [MatrixOneIn_C22], [MatrixOneIn_C23], [MatrixOneIn_C24], [MatrixOneIn_D21], [MatrixOneIn_D22], [MatrixOneIn_D23], [MatrixOneIn_D24], [MatrixOneIn_E11], [MatrixOneIn_E12], [MatrixOneIn_E13], [MatrixOneIn_E14], [MatrixOneIn_E21], [MatrixOneIn_E22], [MatrixOneIn_E23], [MatrixOneIn_E24], [MatrixOneIn_F11], [MatrixOneIn_F12], [MatrixOneIn_F13], [MatrixOneIn_F14], [MatrixOneIn_F21], [MatrixOneIn_F22], [MatrixOneIn_F23], [MatrixOneIn_F24], [MatrixOneIn_G11], [MatrixOneIn_G12], [MatrixOneIn_G13], [MatrixOneIn_G14], [MatrixOneIn_G21], [MatrixOneIn_G22], [MatrixOneIn_G23], [MatrixOneIn_G24], [MatrixOneIn_H11], [MatrixOneIn_H12], [MatrixOneIn_H13], [MatrixOneIn_H14], [MatrixOneIn_H21], [MatrixOneIn_H22], [MatrixOneIn_H23], [MatrixOneIn_H24], [MatrixTwoIn_SelectedOption], [MatrixTwoIn_D21], [MatrixTwoIn_D22], [MatrixTwoIn_D23], [MatrixTwoIn_D24], [MatrixTwoIn_E11], [MatrixTwoIn_E12], [MatrixTwoIn_E13], [MatrixTwoIn_E14], [MatrixTwoIn_E21], [MatrixTwoIn_E22], [MatrixTwoIn_E23], [MatrixTwoIn_E24], [MatrixTwoIn_F11], [MatrixTwoIn_F12], [MatrixTwoIn_F13], [MatrixTwoIn_F14], [MatrixTwoIn_F21], [MatrixTwoIn_F22], [MatrixTwoIn_F23], [MatrixTwoIn_F24], [MatrixTwoIn_G11], [MatrixTwoIn_G12], [MatrixTwoIn_G13], [MatrixTwoIn_G14], [MatrixTwoIn_G21], [MatrixTwoIn_G22], [MatrixTwoIn_G23], [MatrixTwoIn_G24], [MatrixTwoIn_H11], [MatrixTwoIn_H12], [MatrixTwoIn_H13], [MatrixTwoIn_H14], [MatrixTwoIn_H21], [MatrixTwoIn_H22], [MatrixTwoIn_H23], [MatrixTwoIn_H24], [SystemInputVolume_B19], [SystemInputVolumeErrorMargin_B21], [AuthorizedConsumption_K12], [AuthorizedConsumptionErrorMargin_K15], [WaterLosses_K29], [WaterLossesErrorMargin_K31], [BilledAuthorizedConsumption_T8], [UnbilledAuthorizedConsumption_T16], [UnbilledAuthorizedConsumptionErrorMargin_T20], [CommercialLosses_T26], [CommercialLossesErrorMargin_T29], [PhysicalLossesM3_T34], [PhyscialLossesErrorMargin_AH35], [BilledMeteredConsumption_AC4], [BilledUnmeteredConsumption_AC9], [UnbilledMeteredConsumption_AC14], [UnbilledUnmeteredConsumption_AC19], [UnbilledUnmeteredConsumptionErrorMargin_AO20], [UnauthorizedConsumption_AC24], [UnauthorizedConsumptionErrorMargin_AO25], [CustomerMeterInaccuraciesAndErrorsM3_AC29], [CustomerMeterInaccuraciesAndErrorsErrorMargin_AO30], [RevenueWaterM3_AY8], [NonRevenueWaterM3_AY24], [NonRevenueWaterErrorMargin_AY26]) VALUES (77, N'none\none', CAST(N'2021-01-14T09:15:40.203' AS DateTime), N'none\none', CAST(N'2021-01-14T09:15:40.203' AS DateTime), 6773, 2020, 4, N'1. Orygin', 0, 0, 30, NULL, NULL, NULL, NULL, 6593339, 0.05, 0, 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 5332026, 1000, 0, 0, 0, 0, 0, 0, 0, 0, 309349, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 3, 120, 0.05, 0, 0, 0, 1000, 0.1, 160, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0.03, 0.02, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0.03, 0.03, 0.03, 260, 0, 0, 0, 8000, 1500, 7, 0, 0, 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, 9500, 30, 0, 0, 0, 0, 0, 0, NULL, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, N'0', 0, 0, 0, 1, 1.5, 2, 4, 8, 2, 4, 8, 16, 25, 50, 100, 200, 25, 50, 100, 200, 50, 100, 200, 400, 40, 75, 150, 300, 75, 150, 300, 600, 50, 100, 200, 400, 100, 200, 400, 800, 60, 125, 250, 500, 125, 250, 500, 1000, 2, 55, 110, 220, 400, 50, 100, 200, 350, 80, 160, 320, 600, 65, 125, 250, 450, 105, 210, 420, 800, 75, 150, 300, 550, 130, 260, 520, 1000, 85, 175, 350, 650, 155, 310, 620, 1200, 6593339, 0.05, 5642375, 0, 950964, 0.34666606727489158, 5333026, 309349, 0, 345294.46391752549, 0.001398887036941496, 605669.53608247451, 0.54430227083626459, 5332026, 1000, 309349, 0, 0, 5880, 0.082147610459505027, 339414.46391752549, 0, 5333026, 1260313, 0.26157545784261532)
GO
INSERT [dbo].[tbWbEasyCalcData] ([WbEasyCalcDataId], [CreateLogin], [CreateDate], [ModifyLogin], [ModifyDate], [ZoneId], [YearNo], [MonthNo], [Description], [IsArchive], [IsAccepted], [Start_PeriodDays_M21], [SysInput_Desc_B6], [SysInput_Desc_B7], [SysInput_Desc_B8], [SysInput_Desc_B9], [SysInput_SystemInputVolumeM3_D6], [SysInput_SystemInputVolumeError_F6], [SysInput_SystemInputVolumeM3_D7], [SysInput_SystemInputVolumeError_F7], [SysInput_SystemInputVolumeM3_D8], [SysInput_SystemInputVolumeError_F8], [SysInput_SystemInputVolumeM3_D9], [SysInput_SystemInputVolumeError_F9], [BilledCons_Desc_B8], [BilledCons_Desc_B9], [BilledCons_Desc_B10], [BilledCons_Desc_B11], [BilledCons_Desc_F8], [BilledCons_Desc_F9], [BilledCons_Desc_F10], [BilledCons_Desc_F11], [UnbilledCons_Desc_D8], [UnbilledCons_Desc_D9], [UnbilledCons_Desc_D10], [UnbilledCons_Desc_D11], [UnbilledCons_Desc_F6], [UnbilledCons_Desc_F7], [UnbilledCons_Desc_F8], [UnbilledCons_Desc_F9], [UnbilledCons_Desc_F10], [UnbilledCons_Desc_F11], [UnauthCons_Desc_B18], [UnauthCons_Desc_B19], [UnauthCons_Desc_B20], [UnauthCons_Desc_B21], [MetErrors_Desc_D12], [MetErrors_Desc_D13], [MetErrors_Desc_D14], [MetErrors_Desc_D15], [Network_Desc_B7], [Network_Desc_B8], [Network_Desc_B9], [Network_Desc_B10], [Interm_Area_B7], [Interm_Area_B8], [Interm_Area_B9], [Interm_Area_B10], [BilledCons_BilledMetConsBulkWatSupExpM3_D6], [BilledCons_BilledUnmetConsBulkWatSupExpM3_H6], [BilledCons_UnbMetConsM3_D8], [BilledCons_UnbMetConsM3_D9], [BilledCons_UnbMetConsM3_D10], [BilledCons_UnbMetConsM3_D11], [BilledCons_UnbUnmetConsM3_H8], [BilledCons_UnbUnmetConsM3_H9], [BilledCons_UnbUnmetConsM3_H10], [BilledCons_UnbUnmetConsM3_H11], [UnbilledCons_MetConsBulkWatSupExpM3_D6], [UnbilledCons_UnbMetConsM3_D8], [UnbilledCons_UnbMetConsM3_D9], [UnbilledCons_UnbMetConsM3_D10], [UnbilledCons_UnbMetConsM3_D11], [UnbilledCons_UnbUnmetConsM3_H6], [UnbilledCons_UnbUnmetConsM3_H7], [UnbilledCons_UnbUnmetConsM3_H8], [UnbilledCons_UnbUnmetConsM3_H9], [UnbilledCons_UnbUnmetConsM3_H10], [UnbilledCons_UnbUnmetConsM3_H11], [UnbilledCons_UnbUnmetConsError_J6], [UnbilledCons_UnbUnmetConsError_J7], [UnbilledCons_UnbUnmetConsError_J8], [UnbilledCons_UnbUnmetConsError_J9], [UnbilledCons_UnbUnmetConsError_J10], [UnbilledCons_UnbUnmetConsError_J11], [UnauthCons_IllegalConnDomEstNo_D6], [UnauthCons_IllegalConnDomPersPerHouse_H6], [UnauthCons_IllegalConnDomConsLitPerPersDay_J6], [UnauthCons_IllegalConnDomErrorMargin_F6], [UnauthCons_IllegalConnOthersErrorMargin_F10], [IllegalConnectionsOthersEstimatedNumber_D10], [IllegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10], [UnauthCons_MeterTampBypEtcEstNo_D14], [UnauthCons_MeterTampBypEtcErrorMargin_F14], [UnauthCons_MeterTampBypEtcConsLitPerCustDay_J14], [UnauthCons_OthersErrorMargin_F18], [UnauthCons_OthersErrorMargin_F19], [UnauthCons_OthersErrorMargin_F20], [UnauthCons_OthersErrorMargin_F21], [UnauthCons_OthersM3PerDay_J18], [UnauthCons_OthersM3PerDay_J19], [UnauthCons_OthersM3PerDay_J20], [UnauthCons_OthersM3PerDay_J21], [MetErrors_DetailedManualSpec_J6], [MetErrors_BilledMetConsWoBulkSupMetUndrreg_H8], [MetErrors_BilledMetConsWoBulkSupErrorMargin_N8], [MetErrors_Total_F12], [MetErrors_Total_F13], [MetErrors_Total_F14], [MetErrors_Total_F15], [MetErrors_Meter_H12], [MetErrors_Meter_H13], [MetErrors_Meter_H14], [MetErrors_Meter_H15], [MetErrors_Error_N12], [MetErrors_Error_N13], [MetErrors_Error_N14], [MetErrors_Error_N15], [MeteredBulkSupplyExportErrorMargin_N32], [UnbilledMeteredConsumptionWithoutBulkSupplyErrorMargin_N34], [CorruptMeterReadingPracticessErrorMargin_N38], [DataHandlingErrorsOffice_L40], [DataHandlingErrorsOfficeErrorMargin_N40], [MetErrors_MetBulkSupExpMetUnderreg_H32], [MetErrors_UnbillMetConsWoBulkSupplMetUndrreg_H34], [MetErrors_CorruptMetReadPractMetUndrreg_H38], [Network_DistributionAndTransmissionMains_D7], [Network_DistributionAndTransmissionMains_D8], [Network_DistributionAndTransmissionMains_D9], [Network_DistributionAndTransmissionMains_D10], [Network_NoOfConnOfRegCustomers_H10], [Network_NoOfInactAccountsWSvcConns_H18], [Network_AvgLenOfSvcConnFromBoundaryToMeterM_H32], [Network_PossibleUnd_D30], [Network_NoCustomers_H7], [Network_ErrorMargin_J7], [Network_ErrorMargin_J10], [Network_ErrorMargin_J18], [Network_ErrorMargin_J32], [Network_ErrorMargin_D35], [Prs_Area_B7], [Prs_Area_B8], [Prs_Area_B9], [Prs_Area_B10], [Prs_ApproxNoOfConn_D7], [Prs_DailyAvgPrsM_F7], [Prs_ApproxNoOfConn_D8], [Prs_DailyAvgPrsM_F8], [Prs_ApproxNoOfConn_D9], [Prs_DailyAvgPrsM_F9], [Prs_ApproxNoOfConn_D10], [Prs_DailyAvgPrsM_F10], [Prs_ErrorMarg_F26], [Interm_Conn_D7], [Interm_Conn_D8], [Interm_Conn_D9], [Interm_Conn_D10], [Interm_Days_F7], [Interm_Days_F8], [Interm_Days_F9], [Interm_Days_F10], [Interm_Hour_H7], [Interm_Hour_H8], [Interm_Hour_H9], [Interm_Hour_H10], [Interm_ErrorMarg_H26], [FinancData_G6], [FinancData_K6], [FinancData_G8], [FinancData_D26], [FinancData_G35], [MatrixOneIn_SelectedOption], [MatrixOneIn_C11], [MatrixOneIn_C12], [MatrixOneIn_C13], [MatrixOneIn_C14], [MatrixOneIn_C21], [MatrixOneIn_C22], [MatrixOneIn_C23], [MatrixOneIn_C24], [MatrixOneIn_D21], [MatrixOneIn_D22], [MatrixOneIn_D23], [MatrixOneIn_D24], [MatrixOneIn_E11], [MatrixOneIn_E12], [MatrixOneIn_E13], [MatrixOneIn_E14], [MatrixOneIn_E21], [MatrixOneIn_E22], [MatrixOneIn_E23], [MatrixOneIn_E24], [MatrixOneIn_F11], [MatrixOneIn_F12], [MatrixOneIn_F13], [MatrixOneIn_F14], [MatrixOneIn_F21], [MatrixOneIn_F22], [MatrixOneIn_F23], [MatrixOneIn_F24], [MatrixOneIn_G11], [MatrixOneIn_G12], [MatrixOneIn_G13], [MatrixOneIn_G14], [MatrixOneIn_G21], [MatrixOneIn_G22], [MatrixOneIn_G23], [MatrixOneIn_G24], [MatrixOneIn_H11], [MatrixOneIn_H12], [MatrixOneIn_H13], [MatrixOneIn_H14], [MatrixOneIn_H21], [MatrixOneIn_H22], [MatrixOneIn_H23], [MatrixOneIn_H24], [MatrixTwoIn_SelectedOption], [MatrixTwoIn_D21], [MatrixTwoIn_D22], [MatrixTwoIn_D23], [MatrixTwoIn_D24], [MatrixTwoIn_E11], [MatrixTwoIn_E12], [MatrixTwoIn_E13], [MatrixTwoIn_E14], [MatrixTwoIn_E21], [MatrixTwoIn_E22], [MatrixTwoIn_E23], [MatrixTwoIn_E24], [MatrixTwoIn_F11], [MatrixTwoIn_F12], [MatrixTwoIn_F13], [MatrixTwoIn_F14], [MatrixTwoIn_F21], [MatrixTwoIn_F22], [MatrixTwoIn_F23], [MatrixTwoIn_F24], [MatrixTwoIn_G11], [MatrixTwoIn_G12], [MatrixTwoIn_G13], [MatrixTwoIn_G14], [MatrixTwoIn_G21], [MatrixTwoIn_G22], [MatrixTwoIn_G23], [MatrixTwoIn_G24], [MatrixTwoIn_H11], [MatrixTwoIn_H12], [MatrixTwoIn_H13], [MatrixTwoIn_H14], [MatrixTwoIn_H21], [MatrixTwoIn_H22], [MatrixTwoIn_H23], [MatrixTwoIn_H24], [SystemInputVolume_B19], [SystemInputVolumeErrorMargin_B21], [AuthorizedConsumption_K12], [AuthorizedConsumptionErrorMargin_K15], [WaterLosses_K29], [WaterLossesErrorMargin_K31], [BilledAuthorizedConsumption_T8], [UnbilledAuthorizedConsumption_T16], [UnbilledAuthorizedConsumptionErrorMargin_T20], [CommercialLosses_T26], [CommercialLossesErrorMargin_T29], [PhysicalLossesM3_T34], [PhyscialLossesErrorMargin_AH35], [BilledMeteredConsumption_AC4], [BilledUnmeteredConsumption_AC9], [UnbilledMeteredConsumption_AC14], [UnbilledUnmeteredConsumption_AC19], [UnbilledUnmeteredConsumptionErrorMargin_AO20], [UnauthorizedConsumption_AC24], [UnauthorizedConsumptionErrorMargin_AO25], [CustomerMeterInaccuraciesAndErrorsM3_AC29], [CustomerMeterInaccuraciesAndErrorsErrorMargin_AO30], [RevenueWaterM3_AY8], [NonRevenueWaterM3_AY24], [NonRevenueWaterErrorMargin_AY26]) VALUES (78, N'none\none', CAST(N'2021-01-14T09:15:40.203' AS DateTime), N'none\none', CAST(N'2021-01-14T09:15:40.203' AS DateTime), 6773, 2020, 4, N'2. Orygin', 0, 0, 30, NULL, NULL, NULL, NULL, 6593339, 0.05, 0, 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 5332026, 1000, 0, 0, 0, 0, 0, 0, 0, 0, 309349, 0, 0, 0, 0, 2000, 0, 0, 0, 0, 0, 0.05, 0, 0, 0, 0, 0, 100, 3, 120, 0.05, 0, 0, 0, 1000, 0.1, 160, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0.03, 0.02, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0.03, 0.03, 0.03, 260, 0, 0, 0, 8000, 1500, 7, 0, 0, 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, 9500, 30, 0, 0, 0, 0, 0, 0, NULL, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, N'0', 0, 0, 0, 1, 1.5, 2, 4, 8, 2, 4, 8, 16, 25, 50, 100, 200, 25, 50, 100, 200, 50, 100, 200, 400, 40, 75, 150, 300, 75, 150, 300, 600, 50, 100, 200, 400, 100, 200, 400, 800, 60, 125, 250, 500, 125, 250, 500, 1000, 2, 55, 110, 220, 400, 50, 100, 200, 350, 80, 160, 320, 600, 65, 125, 250, 450, 105, 210, 420, 800, 75, 150, 300, 550, 130, 260, 520, 1000, 85, 175, 350, 650, 155, 310, 620, 1200, 6593339, 0.05, 5644375, 1.7716753404938545E-05, 948964, 0.34739674873320675, 5333026, 311349, 0.00032118298115619447, 345294.46391752549, 0.001398887036941496, 603669.53608247451, 0.5461056079990817, 5332026, 1000, 309349, 2000, 0.05, 5880, 0.082147610459505027, 339414.46391752549, 0, 5333026, 1260313, 0.26157545784261532)
GO
INSERT [dbo].[tbWbEasyCalcData] ([WbEasyCalcDataId], [CreateLogin], [CreateDate], [ModifyLogin], [ModifyDate], [ZoneId], [YearNo], [MonthNo], [Description], [IsArchive], [IsAccepted], [Start_PeriodDays_M21], [SysInput_Desc_B6], [SysInput_Desc_B7], [SysInput_Desc_B8], [SysInput_Desc_B9], [SysInput_SystemInputVolumeM3_D6], [SysInput_SystemInputVolumeError_F6], [SysInput_SystemInputVolumeM3_D7], [SysInput_SystemInputVolumeError_F7], [SysInput_SystemInputVolumeM3_D8], [SysInput_SystemInputVolumeError_F8], [SysInput_SystemInputVolumeM3_D9], [SysInput_SystemInputVolumeError_F9], [BilledCons_Desc_B8], [BilledCons_Desc_B9], [BilledCons_Desc_B10], [BilledCons_Desc_B11], [BilledCons_Desc_F8], [BilledCons_Desc_F9], [BilledCons_Desc_F10], [BilledCons_Desc_F11], [UnbilledCons_Desc_D8], [UnbilledCons_Desc_D9], [UnbilledCons_Desc_D10], [UnbilledCons_Desc_D11], [UnbilledCons_Desc_F6], [UnbilledCons_Desc_F7], [UnbilledCons_Desc_F8], [UnbilledCons_Desc_F9], [UnbilledCons_Desc_F10], [UnbilledCons_Desc_F11], [UnauthCons_Desc_B18], [UnauthCons_Desc_B19], [UnauthCons_Desc_B20], [UnauthCons_Desc_B21], [MetErrors_Desc_D12], [MetErrors_Desc_D13], [MetErrors_Desc_D14], [MetErrors_Desc_D15], [Network_Desc_B7], [Network_Desc_B8], [Network_Desc_B9], [Network_Desc_B10], [Interm_Area_B7], [Interm_Area_B8], [Interm_Area_B9], [Interm_Area_B10], [BilledCons_BilledMetConsBulkWatSupExpM3_D6], [BilledCons_BilledUnmetConsBulkWatSupExpM3_H6], [BilledCons_UnbMetConsM3_D8], [BilledCons_UnbMetConsM3_D9], [BilledCons_UnbMetConsM3_D10], [BilledCons_UnbMetConsM3_D11], [BilledCons_UnbUnmetConsM3_H8], [BilledCons_UnbUnmetConsM3_H9], [BilledCons_UnbUnmetConsM3_H10], [BilledCons_UnbUnmetConsM3_H11], [UnbilledCons_MetConsBulkWatSupExpM3_D6], [UnbilledCons_UnbMetConsM3_D8], [UnbilledCons_UnbMetConsM3_D9], [UnbilledCons_UnbMetConsM3_D10], [UnbilledCons_UnbMetConsM3_D11], [UnbilledCons_UnbUnmetConsM3_H6], [UnbilledCons_UnbUnmetConsM3_H7], [UnbilledCons_UnbUnmetConsM3_H8], [UnbilledCons_UnbUnmetConsM3_H9], [UnbilledCons_UnbUnmetConsM3_H10], [UnbilledCons_UnbUnmetConsM3_H11], [UnbilledCons_UnbUnmetConsError_J6], [UnbilledCons_UnbUnmetConsError_J7], [UnbilledCons_UnbUnmetConsError_J8], [UnbilledCons_UnbUnmetConsError_J9], [UnbilledCons_UnbUnmetConsError_J10], [UnbilledCons_UnbUnmetConsError_J11], [UnauthCons_IllegalConnDomEstNo_D6], [UnauthCons_IllegalConnDomPersPerHouse_H6], [UnauthCons_IllegalConnDomConsLitPerPersDay_J6], [UnauthCons_IllegalConnDomErrorMargin_F6], [UnauthCons_IllegalConnOthersErrorMargin_F10], [IllegalConnectionsOthersEstimatedNumber_D10], [IllegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10], [UnauthCons_MeterTampBypEtcEstNo_D14], [UnauthCons_MeterTampBypEtcErrorMargin_F14], [UnauthCons_MeterTampBypEtcConsLitPerCustDay_J14], [UnauthCons_OthersErrorMargin_F18], [UnauthCons_OthersErrorMargin_F19], [UnauthCons_OthersErrorMargin_F20], [UnauthCons_OthersErrorMargin_F21], [UnauthCons_OthersM3PerDay_J18], [UnauthCons_OthersM3PerDay_J19], [UnauthCons_OthersM3PerDay_J20], [UnauthCons_OthersM3PerDay_J21], [MetErrors_DetailedManualSpec_J6], [MetErrors_BilledMetConsWoBulkSupMetUndrreg_H8], [MetErrors_BilledMetConsWoBulkSupErrorMargin_N8], [MetErrors_Total_F12], [MetErrors_Total_F13], [MetErrors_Total_F14], [MetErrors_Total_F15], [MetErrors_Meter_H12], [MetErrors_Meter_H13], [MetErrors_Meter_H14], [MetErrors_Meter_H15], [MetErrors_Error_N12], [MetErrors_Error_N13], [MetErrors_Error_N14], [MetErrors_Error_N15], [MeteredBulkSupplyExportErrorMargin_N32], [UnbilledMeteredConsumptionWithoutBulkSupplyErrorMargin_N34], [CorruptMeterReadingPracticessErrorMargin_N38], [DataHandlingErrorsOffice_L40], [DataHandlingErrorsOfficeErrorMargin_N40], [MetErrors_MetBulkSupExpMetUnderreg_H32], [MetErrors_UnbillMetConsWoBulkSupplMetUndrreg_H34], [MetErrors_CorruptMetReadPractMetUndrreg_H38], [Network_DistributionAndTransmissionMains_D7], [Network_DistributionAndTransmissionMains_D8], [Network_DistributionAndTransmissionMains_D9], [Network_DistributionAndTransmissionMains_D10], [Network_NoOfConnOfRegCustomers_H10], [Network_NoOfInactAccountsWSvcConns_H18], [Network_AvgLenOfSvcConnFromBoundaryToMeterM_H32], [Network_PossibleUnd_D30], [Network_NoCustomers_H7], [Network_ErrorMargin_J7], [Network_ErrorMargin_J10], [Network_ErrorMargin_J18], [Network_ErrorMargin_J32], [Network_ErrorMargin_D35], [Prs_Area_B7], [Prs_Area_B8], [Prs_Area_B9], [Prs_Area_B10], [Prs_ApproxNoOfConn_D7], [Prs_DailyAvgPrsM_F7], [Prs_ApproxNoOfConn_D8], [Prs_DailyAvgPrsM_F8], [Prs_ApproxNoOfConn_D9], [Prs_DailyAvgPrsM_F9], [Prs_ApproxNoOfConn_D10], [Prs_DailyAvgPrsM_F10], [Prs_ErrorMarg_F26], [Interm_Conn_D7], [Interm_Conn_D8], [Interm_Conn_D9], [Interm_Conn_D10], [Interm_Days_F7], [Interm_Days_F8], [Interm_Days_F9], [Interm_Days_F10], [Interm_Hour_H7], [Interm_Hour_H8], [Interm_Hour_H9], [Interm_Hour_H10], [Interm_ErrorMarg_H26], [FinancData_G6], [FinancData_K6], [FinancData_G8], [FinancData_D26], [FinancData_G35], [MatrixOneIn_SelectedOption], [MatrixOneIn_C11], [MatrixOneIn_C12], [MatrixOneIn_C13], [MatrixOneIn_C14], [MatrixOneIn_C21], [MatrixOneIn_C22], [MatrixOneIn_C23], [MatrixOneIn_C24], [MatrixOneIn_D21], [MatrixOneIn_D22], [MatrixOneIn_D23], [MatrixOneIn_D24], [MatrixOneIn_E11], [MatrixOneIn_E12], [MatrixOneIn_E13], [MatrixOneIn_E14], [MatrixOneIn_E21], [MatrixOneIn_E22], [MatrixOneIn_E23], [MatrixOneIn_E24], [MatrixOneIn_F11], [MatrixOneIn_F12], [MatrixOneIn_F13], [MatrixOneIn_F14], [MatrixOneIn_F21], [MatrixOneIn_F22], [MatrixOneIn_F23], [MatrixOneIn_F24], [MatrixOneIn_G11], [MatrixOneIn_G12], [MatrixOneIn_G13], [MatrixOneIn_G14], [MatrixOneIn_G21], [MatrixOneIn_G22], [MatrixOneIn_G23], [MatrixOneIn_G24], [MatrixOneIn_H11], [MatrixOneIn_H12], [MatrixOneIn_H13], [MatrixOneIn_H14], [MatrixOneIn_H21], [MatrixOneIn_H22], [MatrixOneIn_H23], [MatrixOneIn_H24], [MatrixTwoIn_SelectedOption], [MatrixTwoIn_D21], [MatrixTwoIn_D22], [MatrixTwoIn_D23], [MatrixTwoIn_D24], [MatrixTwoIn_E11], [MatrixTwoIn_E12], [MatrixTwoIn_E13], [MatrixTwoIn_E14], [MatrixTwoIn_E21], [MatrixTwoIn_E22], [MatrixTwoIn_E23], [MatrixTwoIn_E24], [MatrixTwoIn_F11], [MatrixTwoIn_F12], [MatrixTwoIn_F13], [MatrixTwoIn_F14], [MatrixTwoIn_F21], [MatrixTwoIn_F22], [MatrixTwoIn_F23], [MatrixTwoIn_F24], [MatrixTwoIn_G11], [MatrixTwoIn_G12], [MatrixTwoIn_G13], [MatrixTwoIn_G14], [MatrixTwoIn_G21], [MatrixTwoIn_G22], [MatrixTwoIn_G23], [MatrixTwoIn_G24], [MatrixTwoIn_H11], [MatrixTwoIn_H12], [MatrixTwoIn_H13], [MatrixTwoIn_H14], [MatrixTwoIn_H21], [MatrixTwoIn_H22], [MatrixTwoIn_H23], [MatrixTwoIn_H24], [SystemInputVolume_B19], [SystemInputVolumeErrorMargin_B21], [AuthorizedConsumption_K12], [AuthorizedConsumptionErrorMargin_K15], [WaterLosses_K29], [WaterLossesErrorMargin_K31], [BilledAuthorizedConsumption_T8], [UnbilledAuthorizedConsumption_T16], [UnbilledAuthorizedConsumptionErrorMargin_T20], [CommercialLosses_T26], [CommercialLossesErrorMargin_T29], [PhysicalLossesM3_T34], [PhyscialLossesErrorMargin_AH35], [BilledMeteredConsumption_AC4], [BilledUnmeteredConsumption_AC9], [UnbilledMeteredConsumption_AC14], [UnbilledUnmeteredConsumption_AC19], [UnbilledUnmeteredConsumptionErrorMargin_AO20], [UnauthorizedConsumption_AC24], [UnauthorizedConsumptionErrorMargin_AO25], [CustomerMeterInaccuraciesAndErrorsM3_AC29], [CustomerMeterInaccuraciesAndErrorsErrorMargin_AO30], [RevenueWaterM3_AY8], [NonRevenueWaterM3_AY24], [NonRevenueWaterErrorMargin_AY26]) VALUES (81, N'none\none', CAST(N'2021-01-14T09:15:40.203' AS DateTime), N'EMEA\79749', CAST(N'2021-02-03T21:30:41.280' AS DateTime), 6773, 2020, 4, N'4. Orygin', 0, 0, 30, NULL, NULL, NULL, NULL, 6593339, 0.05, 0, 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 5332026, 1000, 3000, 0, 0, 0, 2000, 0, 0, 0, 309349, 3000, 0, 0, 0, 2000, 0, 0, 0, 0, 0, 0.05, 0, 0, 0, 0, 0, 100, 3, 120, 0.05, 0.05, 500, 200, 1000, 0.1, 160, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0.1, 0.02, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0.02, 0.02, 0.02, 100, 0.02, 0.03, 0.03, 0.03, 260, 0, 0, 0, 8000, 1500, 7, 0, 0, 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, 9500, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, N'0', 0, 0, 0, 1, 1.5, 2, 4, 8, 2, 4, 8, 16, 25, 50, 100, 200, 25, 50, 100, 200, 50, 100, 200, 400, 40, 75, 150, 300, 75, 150, 300, 600, 50, 100, 200, 400, 100, 200, 400, 800, 60, 125, 250, 500, 125, 250, 500, 1000, 2, 55, 110, 220, 400, 50, 100, 200, 350, 80, 160, 320, 600, 65, 125, 250, 450, 105, 210, 420, 800, 75, 150, 300, 550, 130, 260, 520, 1000, 85, 175, 350, 650, 155, 310, 620, 1200, 6593339, 0.05, 5652375, 1.7691678276830535E-05, 940964, 0.3503502878588966, 5338026, 314349, 0.00031811776083270503, 348975.21993127203, 0.013842306109372472, 591988.78006872791, 0.55694020901215513, 5335026, 3000, 312349, 2000, 0.05, 8880, 0.056957495718128637, 340095.21993127203, 0.014125662426983876, 5338026, 1255313, 0.26261733129506348)
GO
INSERT [dbo].[tbWbEasyCalcData] ([WbEasyCalcDataId], [CreateLogin], [CreateDate], [ModifyLogin], [ModifyDate], [ZoneId], [YearNo], [MonthNo], [Description], [IsArchive], [IsAccepted], [Start_PeriodDays_M21], [SysInput_Desc_B6], [SysInput_Desc_B7], [SysInput_Desc_B8], [SysInput_Desc_B9], [SysInput_SystemInputVolumeM3_D6], [SysInput_SystemInputVolumeError_F6], [SysInput_SystemInputVolumeM3_D7], [SysInput_SystemInputVolumeError_F7], [SysInput_SystemInputVolumeM3_D8], [SysInput_SystemInputVolumeError_F8], [SysInput_SystemInputVolumeM3_D9], [SysInput_SystemInputVolumeError_F9], [BilledCons_Desc_B8], [BilledCons_Desc_B9], [BilledCons_Desc_B10], [BilledCons_Desc_B11], [BilledCons_Desc_F8], [BilledCons_Desc_F9], [BilledCons_Desc_F10], [BilledCons_Desc_F11], [UnbilledCons_Desc_D8], [UnbilledCons_Desc_D9], [UnbilledCons_Desc_D10], [UnbilledCons_Desc_D11], [UnbilledCons_Desc_F6], [UnbilledCons_Desc_F7], [UnbilledCons_Desc_F8], [UnbilledCons_Desc_F9], [UnbilledCons_Desc_F10], [UnbilledCons_Desc_F11], [UnauthCons_Desc_B18], [UnauthCons_Desc_B19], [UnauthCons_Desc_B20], [UnauthCons_Desc_B21], [MetErrors_Desc_D12], [MetErrors_Desc_D13], [MetErrors_Desc_D14], [MetErrors_Desc_D15], [Network_Desc_B7], [Network_Desc_B8], [Network_Desc_B9], [Network_Desc_B10], [Interm_Area_B7], [Interm_Area_B8], [Interm_Area_B9], [Interm_Area_B10], [BilledCons_BilledMetConsBulkWatSupExpM3_D6], [BilledCons_BilledUnmetConsBulkWatSupExpM3_H6], [BilledCons_UnbMetConsM3_D8], [BilledCons_UnbMetConsM3_D9], [BilledCons_UnbMetConsM3_D10], [BilledCons_UnbMetConsM3_D11], [BilledCons_UnbUnmetConsM3_H8], [BilledCons_UnbUnmetConsM3_H9], [BilledCons_UnbUnmetConsM3_H10], [BilledCons_UnbUnmetConsM3_H11], [UnbilledCons_MetConsBulkWatSupExpM3_D6], [UnbilledCons_UnbMetConsM3_D8], [UnbilledCons_UnbMetConsM3_D9], [UnbilledCons_UnbMetConsM3_D10], [UnbilledCons_UnbMetConsM3_D11], [UnbilledCons_UnbUnmetConsM3_H6], [UnbilledCons_UnbUnmetConsM3_H7], [UnbilledCons_UnbUnmetConsM3_H8], [UnbilledCons_UnbUnmetConsM3_H9], [UnbilledCons_UnbUnmetConsM3_H10], [UnbilledCons_UnbUnmetConsM3_H11], [UnbilledCons_UnbUnmetConsError_J6], [UnbilledCons_UnbUnmetConsError_J7], [UnbilledCons_UnbUnmetConsError_J8], [UnbilledCons_UnbUnmetConsError_J9], [UnbilledCons_UnbUnmetConsError_J10], [UnbilledCons_UnbUnmetConsError_J11], [UnauthCons_IllegalConnDomEstNo_D6], [UnauthCons_IllegalConnDomPersPerHouse_H6], [UnauthCons_IllegalConnDomConsLitPerPersDay_J6], [UnauthCons_IllegalConnDomErrorMargin_F6], [UnauthCons_IllegalConnOthersErrorMargin_F10], [IllegalConnectionsOthersEstimatedNumber_D10], [IllegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10], [UnauthCons_MeterTampBypEtcEstNo_D14], [UnauthCons_MeterTampBypEtcErrorMargin_F14], [UnauthCons_MeterTampBypEtcConsLitPerCustDay_J14], [UnauthCons_OthersErrorMargin_F18], [UnauthCons_OthersErrorMargin_F19], [UnauthCons_OthersErrorMargin_F20], [UnauthCons_OthersErrorMargin_F21], [UnauthCons_OthersM3PerDay_J18], [UnauthCons_OthersM3PerDay_J19], [UnauthCons_OthersM3PerDay_J20], [UnauthCons_OthersM3PerDay_J21], [MetErrors_DetailedManualSpec_J6], [MetErrors_BilledMetConsWoBulkSupMetUndrreg_H8], [MetErrors_BilledMetConsWoBulkSupErrorMargin_N8], [MetErrors_Total_F12], [MetErrors_Total_F13], [MetErrors_Total_F14], [MetErrors_Total_F15], [MetErrors_Meter_H12], [MetErrors_Meter_H13], [MetErrors_Meter_H14], [MetErrors_Meter_H15], [MetErrors_Error_N12], [MetErrors_Error_N13], [MetErrors_Error_N14], [MetErrors_Error_N15], [MeteredBulkSupplyExportErrorMargin_N32], [UnbilledMeteredConsumptionWithoutBulkSupplyErrorMargin_N34], [CorruptMeterReadingPracticessErrorMargin_N38], [DataHandlingErrorsOffice_L40], [DataHandlingErrorsOfficeErrorMargin_N40], [MetErrors_MetBulkSupExpMetUnderreg_H32], [MetErrors_UnbillMetConsWoBulkSupplMetUndrreg_H34], [MetErrors_CorruptMetReadPractMetUndrreg_H38], [Network_DistributionAndTransmissionMains_D7], [Network_DistributionAndTransmissionMains_D8], [Network_DistributionAndTransmissionMains_D9], [Network_DistributionAndTransmissionMains_D10], [Network_NoOfConnOfRegCustomers_H10], [Network_NoOfInactAccountsWSvcConns_H18], [Network_AvgLenOfSvcConnFromBoundaryToMeterM_H32], [Network_PossibleUnd_D30], [Network_NoCustomers_H7], [Network_ErrorMargin_J7], [Network_ErrorMargin_J10], [Network_ErrorMargin_J18], [Network_ErrorMargin_J32], [Network_ErrorMargin_D35], [Prs_Area_B7], [Prs_Area_B8], [Prs_Area_B9], [Prs_Area_B10], [Prs_ApproxNoOfConn_D7], [Prs_DailyAvgPrsM_F7], [Prs_ApproxNoOfConn_D8], [Prs_DailyAvgPrsM_F8], [Prs_ApproxNoOfConn_D9], [Prs_DailyAvgPrsM_F9], [Prs_ApproxNoOfConn_D10], [Prs_DailyAvgPrsM_F10], [Prs_ErrorMarg_F26], [Interm_Conn_D7], [Interm_Conn_D8], [Interm_Conn_D9], [Interm_Conn_D10], [Interm_Days_F7], [Interm_Days_F8], [Interm_Days_F9], [Interm_Days_F10], [Interm_Hour_H7], [Interm_Hour_H8], [Interm_Hour_H9], [Interm_Hour_H10], [Interm_ErrorMarg_H26], [FinancData_G6], [FinancData_K6], [FinancData_G8], [FinancData_D26], [FinancData_G35], [MatrixOneIn_SelectedOption], [MatrixOneIn_C11], [MatrixOneIn_C12], [MatrixOneIn_C13], [MatrixOneIn_C14], [MatrixOneIn_C21], [MatrixOneIn_C22], [MatrixOneIn_C23], [MatrixOneIn_C24], [MatrixOneIn_D21], [MatrixOneIn_D22], [MatrixOneIn_D23], [MatrixOneIn_D24], [MatrixOneIn_E11], [MatrixOneIn_E12], [MatrixOneIn_E13], [MatrixOneIn_E14], [MatrixOneIn_E21], [MatrixOneIn_E22], [MatrixOneIn_E23], [MatrixOneIn_E24], [MatrixOneIn_F11], [MatrixOneIn_F12], [MatrixOneIn_F13], [MatrixOneIn_F14], [MatrixOneIn_F21], [MatrixOneIn_F22], [MatrixOneIn_F23], [MatrixOneIn_F24], [MatrixOneIn_G11], [MatrixOneIn_G12], [MatrixOneIn_G13], [MatrixOneIn_G14], [MatrixOneIn_G21], [MatrixOneIn_G22], [MatrixOneIn_G23], [MatrixOneIn_G24], [MatrixOneIn_H11], [MatrixOneIn_H12], [MatrixOneIn_H13], [MatrixOneIn_H14], [MatrixOneIn_H21], [MatrixOneIn_H22], [MatrixOneIn_H23], [MatrixOneIn_H24], [MatrixTwoIn_SelectedOption], [MatrixTwoIn_D21], [MatrixTwoIn_D22], [MatrixTwoIn_D23], [MatrixTwoIn_D24], [MatrixTwoIn_E11], [MatrixTwoIn_E12], [MatrixTwoIn_E13], [MatrixTwoIn_E14], [MatrixTwoIn_E21], [MatrixTwoIn_E22], [MatrixTwoIn_E23], [MatrixTwoIn_E24], [MatrixTwoIn_F11], [MatrixTwoIn_F12], [MatrixTwoIn_F13], [MatrixTwoIn_F14], [MatrixTwoIn_F21], [MatrixTwoIn_F22], [MatrixTwoIn_F23], [MatrixTwoIn_F24], [MatrixTwoIn_G11], [MatrixTwoIn_G12], [MatrixTwoIn_G13], [MatrixTwoIn_G14], [MatrixTwoIn_G21], [MatrixTwoIn_G22], [MatrixTwoIn_G23], [MatrixTwoIn_G24], [MatrixTwoIn_H11], [MatrixTwoIn_H12], [MatrixTwoIn_H13], [MatrixTwoIn_H14], [MatrixTwoIn_H21], [MatrixTwoIn_H22], [MatrixTwoIn_H23], [MatrixTwoIn_H24], [SystemInputVolume_B19], [SystemInputVolumeErrorMargin_B21], [AuthorizedConsumption_K12], [AuthorizedConsumptionErrorMargin_K15], [WaterLosses_K29], [WaterLossesErrorMargin_K31], [BilledAuthorizedConsumption_T8], [UnbilledAuthorizedConsumption_T16], [UnbilledAuthorizedConsumptionErrorMargin_T20], [CommercialLosses_T26], [CommercialLossesErrorMargin_T29], [PhysicalLossesM3_T34], [PhyscialLossesErrorMargin_AH35], [BilledMeteredConsumption_AC4], [BilledUnmeteredConsumption_AC9], [UnbilledMeteredConsumption_AC14], [UnbilledUnmeteredConsumption_AC19], [UnbilledUnmeteredConsumptionErrorMargin_AO20], [UnauthorizedConsumption_AC24], [UnauthorizedConsumptionErrorMargin_AO25], [CustomerMeterInaccuraciesAndErrorsM3_AC29], [CustomerMeterInaccuraciesAndErrorsErrorMargin_AO30], [RevenueWaterM3_AY8], [NonRevenueWaterM3_AY24], [NonRevenueWaterErrorMargin_AY26]) VALUES (1081, N'none\none', CAST(N'2021-01-14T09:15:40.203' AS DateTime), N'none\none', CAST(N'2021-01-14T09:15:40.203' AS DateTime), 6773, 2019, 1, N'Old', 0, 0, 10, NULL, NULL, NULL, NULL, 6593339, 0.05, 0, 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 5332026, 1000, 0, 0, 0, 0, 0, 0, 0, 0, 309349, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 3, 120, 0.05, 0, 0, 0, 1000, 0.1, 160, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0.03, 0.02, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0.03, 0.03, 0.03, 260, 0, 0, 0, 8000, 1500, 7, 0, 0, 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, 9500, 30, 0, 0, 0, 0, 0, 0, NULL, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, N'0', 0, 0, 0, 1, 1.5, 2, 4, 8, 2, 4, 8, 16, 25, 50, 100, 200, 25, 50, 100, 200, 50, 100, 200, 400, 40, 75, 150, 300, 75, 150, 300, 600, 50, 100, 200, 400, 100, 200, 400, 800, 60, 125, 250, 500, 125, 250, 500, 1000, 2, 55, 110, 220, 400, 50, 100, 200, 350, 80, 160, 320, 600, 65, 125, 250, 450, 105, 210, 420, 800, 75, 150, 300, 550, 130, 260, 520, 1000, 85, 175, 350, 650, 155, 310, 620, 1200, 6593339, 0.05, 5642375, 0, 950964, 0.34666606727489158, 5333026, 309349, 0, 341374.46391752549, 0.00047165014820654242, 609589.53608247451, 0.54080158829016556, 5332026, 1000, 309349, 0, 0, 1960, 0.082147610459505027, 339414.46391752549, 0, 5333026, 1260313, 0.26157545784261532)
GO
INSERT [dbo].[tbWbEasyCalcData] ([WbEasyCalcDataId], [CreateLogin], [CreateDate], [ModifyLogin], [ModifyDate], [ZoneId], [YearNo], [MonthNo], [Description], [IsArchive], [IsAccepted], [Start_PeriodDays_M21], [SysInput_Desc_B6], [SysInput_Desc_B7], [SysInput_Desc_B8], [SysInput_Desc_B9], [SysInput_SystemInputVolumeM3_D6], [SysInput_SystemInputVolumeError_F6], [SysInput_SystemInputVolumeM3_D7], [SysInput_SystemInputVolumeError_F7], [SysInput_SystemInputVolumeM3_D8], [SysInput_SystemInputVolumeError_F8], [SysInput_SystemInputVolumeM3_D9], [SysInput_SystemInputVolumeError_F9], [BilledCons_Desc_B8], [BilledCons_Desc_B9], [BilledCons_Desc_B10], [BilledCons_Desc_B11], [BilledCons_Desc_F8], [BilledCons_Desc_F9], [BilledCons_Desc_F10], [BilledCons_Desc_F11], [UnbilledCons_Desc_D8], [UnbilledCons_Desc_D9], [UnbilledCons_Desc_D10], [UnbilledCons_Desc_D11], [UnbilledCons_Desc_F6], [UnbilledCons_Desc_F7], [UnbilledCons_Desc_F8], [UnbilledCons_Desc_F9], [UnbilledCons_Desc_F10], [UnbilledCons_Desc_F11], [UnauthCons_Desc_B18], [UnauthCons_Desc_B19], [UnauthCons_Desc_B20], [UnauthCons_Desc_B21], [MetErrors_Desc_D12], [MetErrors_Desc_D13], [MetErrors_Desc_D14], [MetErrors_Desc_D15], [Network_Desc_B7], [Network_Desc_B8], [Network_Desc_B9], [Network_Desc_B10], [Interm_Area_B7], [Interm_Area_B8], [Interm_Area_B9], [Interm_Area_B10], [BilledCons_BilledMetConsBulkWatSupExpM3_D6], [BilledCons_BilledUnmetConsBulkWatSupExpM3_H6], [BilledCons_UnbMetConsM3_D8], [BilledCons_UnbMetConsM3_D9], [BilledCons_UnbMetConsM3_D10], [BilledCons_UnbMetConsM3_D11], [BilledCons_UnbUnmetConsM3_H8], [BilledCons_UnbUnmetConsM3_H9], [BilledCons_UnbUnmetConsM3_H10], [BilledCons_UnbUnmetConsM3_H11], [UnbilledCons_MetConsBulkWatSupExpM3_D6], [UnbilledCons_UnbMetConsM3_D8], [UnbilledCons_UnbMetConsM3_D9], [UnbilledCons_UnbMetConsM3_D10], [UnbilledCons_UnbMetConsM3_D11], [UnbilledCons_UnbUnmetConsM3_H6], [UnbilledCons_UnbUnmetConsM3_H7], [UnbilledCons_UnbUnmetConsM3_H8], [UnbilledCons_UnbUnmetConsM3_H9], [UnbilledCons_UnbUnmetConsM3_H10], [UnbilledCons_UnbUnmetConsM3_H11], [UnbilledCons_UnbUnmetConsError_J6], [UnbilledCons_UnbUnmetConsError_J7], [UnbilledCons_UnbUnmetConsError_J8], [UnbilledCons_UnbUnmetConsError_J9], [UnbilledCons_UnbUnmetConsError_J10], [UnbilledCons_UnbUnmetConsError_J11], [UnauthCons_IllegalConnDomEstNo_D6], [UnauthCons_IllegalConnDomPersPerHouse_H6], [UnauthCons_IllegalConnDomConsLitPerPersDay_J6], [UnauthCons_IllegalConnDomErrorMargin_F6], [UnauthCons_IllegalConnOthersErrorMargin_F10], [IllegalConnectionsOthersEstimatedNumber_D10], [IllegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10], [UnauthCons_MeterTampBypEtcEstNo_D14], [UnauthCons_MeterTampBypEtcErrorMargin_F14], [UnauthCons_MeterTampBypEtcConsLitPerCustDay_J14], [UnauthCons_OthersErrorMargin_F18], [UnauthCons_OthersErrorMargin_F19], [UnauthCons_OthersErrorMargin_F20], [UnauthCons_OthersErrorMargin_F21], [UnauthCons_OthersM3PerDay_J18], [UnauthCons_OthersM3PerDay_J19], [UnauthCons_OthersM3PerDay_J20], [UnauthCons_OthersM3PerDay_J21], [MetErrors_DetailedManualSpec_J6], [MetErrors_BilledMetConsWoBulkSupMetUndrreg_H8], [MetErrors_BilledMetConsWoBulkSupErrorMargin_N8], [MetErrors_Total_F12], [MetErrors_Total_F13], [MetErrors_Total_F14], [MetErrors_Total_F15], [MetErrors_Meter_H12], [MetErrors_Meter_H13], [MetErrors_Meter_H14], [MetErrors_Meter_H15], [MetErrors_Error_N12], [MetErrors_Error_N13], [MetErrors_Error_N14], [MetErrors_Error_N15], [MeteredBulkSupplyExportErrorMargin_N32], [UnbilledMeteredConsumptionWithoutBulkSupplyErrorMargin_N34], [CorruptMeterReadingPracticessErrorMargin_N38], [DataHandlingErrorsOffice_L40], [DataHandlingErrorsOfficeErrorMargin_N40], [MetErrors_MetBulkSupExpMetUnderreg_H32], [MetErrors_UnbillMetConsWoBulkSupplMetUndrreg_H34], [MetErrors_CorruptMetReadPractMetUndrreg_H38], [Network_DistributionAndTransmissionMains_D7], [Network_DistributionAndTransmissionMains_D8], [Network_DistributionAndTransmissionMains_D9], [Network_DistributionAndTransmissionMains_D10], [Network_NoOfConnOfRegCustomers_H10], [Network_NoOfInactAccountsWSvcConns_H18], [Network_AvgLenOfSvcConnFromBoundaryToMeterM_H32], [Network_PossibleUnd_D30], [Network_NoCustomers_H7], [Network_ErrorMargin_J7], [Network_ErrorMargin_J10], [Network_ErrorMargin_J18], [Network_ErrorMargin_J32], [Network_ErrorMargin_D35], [Prs_Area_B7], [Prs_Area_B8], [Prs_Area_B9], [Prs_Area_B10], [Prs_ApproxNoOfConn_D7], [Prs_DailyAvgPrsM_F7], [Prs_ApproxNoOfConn_D8], [Prs_DailyAvgPrsM_F8], [Prs_ApproxNoOfConn_D9], [Prs_DailyAvgPrsM_F9], [Prs_ApproxNoOfConn_D10], [Prs_DailyAvgPrsM_F10], [Prs_ErrorMarg_F26], [Interm_Conn_D7], [Interm_Conn_D8], [Interm_Conn_D9], [Interm_Conn_D10], [Interm_Days_F7], [Interm_Days_F8], [Interm_Days_F9], [Interm_Days_F10], [Interm_Hour_H7], [Interm_Hour_H8], [Interm_Hour_H9], [Interm_Hour_H10], [Interm_ErrorMarg_H26], [FinancData_G6], [FinancData_K6], [FinancData_G8], [FinancData_D26], [FinancData_G35], [MatrixOneIn_SelectedOption], [MatrixOneIn_C11], [MatrixOneIn_C12], [MatrixOneIn_C13], [MatrixOneIn_C14], [MatrixOneIn_C21], [MatrixOneIn_C22], [MatrixOneIn_C23], [MatrixOneIn_C24], [MatrixOneIn_D21], [MatrixOneIn_D22], [MatrixOneIn_D23], [MatrixOneIn_D24], [MatrixOneIn_E11], [MatrixOneIn_E12], [MatrixOneIn_E13], [MatrixOneIn_E14], [MatrixOneIn_E21], [MatrixOneIn_E22], [MatrixOneIn_E23], [MatrixOneIn_E24], [MatrixOneIn_F11], [MatrixOneIn_F12], [MatrixOneIn_F13], [MatrixOneIn_F14], [MatrixOneIn_F21], [MatrixOneIn_F22], [MatrixOneIn_F23], [MatrixOneIn_F24], [MatrixOneIn_G11], [MatrixOneIn_G12], [MatrixOneIn_G13], [MatrixOneIn_G14], [MatrixOneIn_G21], [MatrixOneIn_G22], [MatrixOneIn_G23], [MatrixOneIn_G24], [MatrixOneIn_H11], [MatrixOneIn_H12], [MatrixOneIn_H13], [MatrixOneIn_H14], [MatrixOneIn_H21], [MatrixOneIn_H22], [MatrixOneIn_H23], [MatrixOneIn_H24], [MatrixTwoIn_SelectedOption], [MatrixTwoIn_D21], [MatrixTwoIn_D22], [MatrixTwoIn_D23], [MatrixTwoIn_D24], [MatrixTwoIn_E11], [MatrixTwoIn_E12], [MatrixTwoIn_E13], [MatrixTwoIn_E14], [MatrixTwoIn_E21], [MatrixTwoIn_E22], [MatrixTwoIn_E23], [MatrixTwoIn_E24], [MatrixTwoIn_F11], [MatrixTwoIn_F12], [MatrixTwoIn_F13], [MatrixTwoIn_F14], [MatrixTwoIn_F21], [MatrixTwoIn_F22], [MatrixTwoIn_F23], [MatrixTwoIn_F24], [MatrixTwoIn_G11], [MatrixTwoIn_G12], [MatrixTwoIn_G13], [MatrixTwoIn_G14], [MatrixTwoIn_G21], [MatrixTwoIn_G22], [MatrixTwoIn_G23], [MatrixTwoIn_G24], [MatrixTwoIn_H11], [MatrixTwoIn_H12], [MatrixTwoIn_H13], [MatrixTwoIn_H14], [MatrixTwoIn_H21], [MatrixTwoIn_H22], [MatrixTwoIn_H23], [MatrixTwoIn_H24], [SystemInputVolume_B19], [SystemInputVolumeErrorMargin_B21], [AuthorizedConsumption_K12], [AuthorizedConsumptionErrorMargin_K15], [WaterLosses_K29], [WaterLossesErrorMargin_K31], [BilledAuthorizedConsumption_T8], [UnbilledAuthorizedConsumption_T16], [UnbilledAuthorizedConsumptionErrorMargin_T20], [CommercialLosses_T26], [CommercialLossesErrorMargin_T29], [PhysicalLossesM3_T34], [PhyscialLossesErrorMargin_AH35], [BilledMeteredConsumption_AC4], [BilledUnmeteredConsumption_AC9], [UnbilledMeteredConsumption_AC14], [UnbilledUnmeteredConsumption_AC19], [UnbilledUnmeteredConsumptionErrorMargin_AO20], [UnauthorizedConsumption_AC24], [UnauthorizedConsumptionErrorMargin_AO25], [CustomerMeterInaccuraciesAndErrorsM3_AC29], [CustomerMeterInaccuraciesAndErrorsErrorMargin_AO30], [RevenueWaterM3_AY8], [NonRevenueWaterM3_AY24], [NonRevenueWaterErrorMargin_AY26]) VALUES (1082, N'none\none', CAST(N'2021-01-14T09:15:40.203' AS DateTime), N'none\none', CAST(N'2021-01-14T09:15:40.203' AS DateTime), 6773, 2019, 3, N'Generated', 0, 0, 1, NULL, NULL, NULL, NULL, 0, 0.12, 0, 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 70420, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20342.3603638975, 0, 0, 0, 911, 0, 0, 0, 0, 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, 0, 0, 0, 0, 0, 0, 0, 0, NULL, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, N'0', 0, 0, 0, 1, 1.5, 2, 4, 8, 2, 4, 8, 16, 25, 50, 100, 200, 25, 50, 100, 200, 50, 100, 200, 400, 40, 75, 150, 300, 75, 150, 300, 600, 50, 100, 200, 400, 100, 200, 400, 800, 60, 125, 250, 500, 125, 250, 500, 1000, 2, 55, 110, 220, 400, 50, 100, 200, 350, 80, 160, 320, 600, 65, 125, 250, 450, 105, 210, 420, 800, 75, 150, 300, 550, 130, 260, 520, 1000, 85, 175, 350, 650, 155, 310, 620, 1200, 102537, 0.11999999999999998, 68358, 0, 34179, 0.36, 68358, 0, 0, 0, 0, 34179, 0.36, 68358, 0, 0, 0, 0, 0, 0, 0, 0, 68358, 34179, 0.36)
GO
INSERT [dbo].[tbWbEasyCalcData] ([WbEasyCalcDataId], [CreateLogin], [CreateDate], [ModifyLogin], [ModifyDate], [ZoneId], [YearNo], [MonthNo], [Description], [IsArchive], [IsAccepted], [Start_PeriodDays_M21], [SysInput_Desc_B6], [SysInput_Desc_B7], [SysInput_Desc_B8], [SysInput_Desc_B9], [SysInput_SystemInputVolumeM3_D6], [SysInput_SystemInputVolumeError_F6], [SysInput_SystemInputVolumeM3_D7], [SysInput_SystemInputVolumeError_F7], [SysInput_SystemInputVolumeM3_D8], [SysInput_SystemInputVolumeError_F8], [SysInput_SystemInputVolumeM3_D9], [SysInput_SystemInputVolumeError_F9], [BilledCons_Desc_B8], [BilledCons_Desc_B9], [BilledCons_Desc_B10], [BilledCons_Desc_B11], [BilledCons_Desc_F8], [BilledCons_Desc_F9], [BilledCons_Desc_F10], [BilledCons_Desc_F11], [UnbilledCons_Desc_D8], [UnbilledCons_Desc_D9], [UnbilledCons_Desc_D10], [UnbilledCons_Desc_D11], [UnbilledCons_Desc_F6], [UnbilledCons_Desc_F7], [UnbilledCons_Desc_F8], [UnbilledCons_Desc_F9], [UnbilledCons_Desc_F10], [UnbilledCons_Desc_F11], [UnauthCons_Desc_B18], [UnauthCons_Desc_B19], [UnauthCons_Desc_B20], [UnauthCons_Desc_B21], [MetErrors_Desc_D12], [MetErrors_Desc_D13], [MetErrors_Desc_D14], [MetErrors_Desc_D15], [Network_Desc_B7], [Network_Desc_B8], [Network_Desc_B9], [Network_Desc_B10], [Interm_Area_B7], [Interm_Area_B8], [Interm_Area_B9], [Interm_Area_B10], [BilledCons_BilledMetConsBulkWatSupExpM3_D6], [BilledCons_BilledUnmetConsBulkWatSupExpM3_H6], [BilledCons_UnbMetConsM3_D8], [BilledCons_UnbMetConsM3_D9], [BilledCons_UnbMetConsM3_D10], [BilledCons_UnbMetConsM3_D11], [BilledCons_UnbUnmetConsM3_H8], [BilledCons_UnbUnmetConsM3_H9], [BilledCons_UnbUnmetConsM3_H10], [BilledCons_UnbUnmetConsM3_H11], [UnbilledCons_MetConsBulkWatSupExpM3_D6], [UnbilledCons_UnbMetConsM3_D8], [UnbilledCons_UnbMetConsM3_D9], [UnbilledCons_UnbMetConsM3_D10], [UnbilledCons_UnbMetConsM3_D11], [UnbilledCons_UnbUnmetConsM3_H6], [UnbilledCons_UnbUnmetConsM3_H7], [UnbilledCons_UnbUnmetConsM3_H8], [UnbilledCons_UnbUnmetConsM3_H9], [UnbilledCons_UnbUnmetConsM3_H10], [UnbilledCons_UnbUnmetConsM3_H11], [UnbilledCons_UnbUnmetConsError_J6], [UnbilledCons_UnbUnmetConsError_J7], [UnbilledCons_UnbUnmetConsError_J8], [UnbilledCons_UnbUnmetConsError_J9], [UnbilledCons_UnbUnmetConsError_J10], [UnbilledCons_UnbUnmetConsError_J11], [UnauthCons_IllegalConnDomEstNo_D6], [UnauthCons_IllegalConnDomPersPerHouse_H6], [UnauthCons_IllegalConnDomConsLitPerPersDay_J6], [UnauthCons_IllegalConnDomErrorMargin_F6], [UnauthCons_IllegalConnOthersErrorMargin_F10], [IllegalConnectionsOthersEstimatedNumber_D10], [IllegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10], [UnauthCons_MeterTampBypEtcEstNo_D14], [UnauthCons_MeterTampBypEtcErrorMargin_F14], [UnauthCons_MeterTampBypEtcConsLitPerCustDay_J14], [UnauthCons_OthersErrorMargin_F18], [UnauthCons_OthersErrorMargin_F19], [UnauthCons_OthersErrorMargin_F20], [UnauthCons_OthersErrorMargin_F21], [UnauthCons_OthersM3PerDay_J18], [UnauthCons_OthersM3PerDay_J19], [UnauthCons_OthersM3PerDay_J20], [UnauthCons_OthersM3PerDay_J21], [MetErrors_DetailedManualSpec_J6], [MetErrors_BilledMetConsWoBulkSupMetUndrreg_H8], [MetErrors_BilledMetConsWoBulkSupErrorMargin_N8], [MetErrors_Total_F12], [MetErrors_Total_F13], [MetErrors_Total_F14], [MetErrors_Total_F15], [MetErrors_Meter_H12], [MetErrors_Meter_H13], [MetErrors_Meter_H14], [MetErrors_Meter_H15], [MetErrors_Error_N12], [MetErrors_Error_N13], [MetErrors_Error_N14], [MetErrors_Error_N15], [MeteredBulkSupplyExportErrorMargin_N32], [UnbilledMeteredConsumptionWithoutBulkSupplyErrorMargin_N34], [CorruptMeterReadingPracticessErrorMargin_N38], [DataHandlingErrorsOffice_L40], [DataHandlingErrorsOfficeErrorMargin_N40], [MetErrors_MetBulkSupExpMetUnderreg_H32], [MetErrors_UnbillMetConsWoBulkSupplMetUndrreg_H34], [MetErrors_CorruptMetReadPractMetUndrreg_H38], [Network_DistributionAndTransmissionMains_D7], [Network_DistributionAndTransmissionMains_D8], [Network_DistributionAndTransmissionMains_D9], [Network_DistributionAndTransmissionMains_D10], [Network_NoOfConnOfRegCustomers_H10], [Network_NoOfInactAccountsWSvcConns_H18], [Network_AvgLenOfSvcConnFromBoundaryToMeterM_H32], [Network_PossibleUnd_D30], [Network_NoCustomers_H7], [Network_ErrorMargin_J7], [Network_ErrorMargin_J10], [Network_ErrorMargin_J18], [Network_ErrorMargin_J32], [Network_ErrorMargin_D35], [Prs_Area_B7], [Prs_Area_B8], [Prs_Area_B9], [Prs_Area_B10], [Prs_ApproxNoOfConn_D7], [Prs_DailyAvgPrsM_F7], [Prs_ApproxNoOfConn_D8], [Prs_DailyAvgPrsM_F8], [Prs_ApproxNoOfConn_D9], [Prs_DailyAvgPrsM_F9], [Prs_ApproxNoOfConn_D10], [Prs_DailyAvgPrsM_F10], [Prs_ErrorMarg_F26], [Interm_Conn_D7], [Interm_Conn_D8], [Interm_Conn_D9], [Interm_Conn_D10], [Interm_Days_F7], [Interm_Days_F8], [Interm_Days_F9], [Interm_Days_F10], [Interm_Hour_H7], [Interm_Hour_H8], [Interm_Hour_H9], [Interm_Hour_H10], [Interm_ErrorMarg_H26], [FinancData_G6], [FinancData_K6], [FinancData_G8], [FinancData_D26], [FinancData_G35], [MatrixOneIn_SelectedOption], [MatrixOneIn_C11], [MatrixOneIn_C12], [MatrixOneIn_C13], [MatrixOneIn_C14], [MatrixOneIn_C21], [MatrixOneIn_C22], [MatrixOneIn_C23], [MatrixOneIn_C24], [MatrixOneIn_D21], [MatrixOneIn_D22], [MatrixOneIn_D23], [MatrixOneIn_D24], [MatrixOneIn_E11], [MatrixOneIn_E12], [MatrixOneIn_E13], [MatrixOneIn_E14], [MatrixOneIn_E21], [MatrixOneIn_E22], [MatrixOneIn_E23], [MatrixOneIn_E24], [MatrixOneIn_F11], [MatrixOneIn_F12], [MatrixOneIn_F13], [MatrixOneIn_F14], [MatrixOneIn_F21], [MatrixOneIn_F22], [MatrixOneIn_F23], [MatrixOneIn_F24], [MatrixOneIn_G11], [MatrixOneIn_G12], [MatrixOneIn_G13], [MatrixOneIn_G14], [MatrixOneIn_G21], [MatrixOneIn_G22], [MatrixOneIn_G23], [MatrixOneIn_G24], [MatrixOneIn_H11], [MatrixOneIn_H12], [MatrixOneIn_H13], [MatrixOneIn_H14], [MatrixOneIn_H21], [MatrixOneIn_H22], [MatrixOneIn_H23], [MatrixOneIn_H24], [MatrixTwoIn_SelectedOption], [MatrixTwoIn_D21], [MatrixTwoIn_D22], [MatrixTwoIn_D23], [MatrixTwoIn_D24], [MatrixTwoIn_E11], [MatrixTwoIn_E12], [MatrixTwoIn_E13], [MatrixTwoIn_E14], [MatrixTwoIn_E21], [MatrixTwoIn_E22], [MatrixTwoIn_E23], [MatrixTwoIn_E24], [MatrixTwoIn_F11], [MatrixTwoIn_F12], [MatrixTwoIn_F13], [MatrixTwoIn_F14], [MatrixTwoIn_F21], [MatrixTwoIn_F22], [MatrixTwoIn_F23], [MatrixTwoIn_F24], [MatrixTwoIn_G11], [MatrixTwoIn_G12], [MatrixTwoIn_G13], [MatrixTwoIn_G14], [MatrixTwoIn_G21], [MatrixTwoIn_G22], [MatrixTwoIn_G23], [MatrixTwoIn_G24], [MatrixTwoIn_H11], [MatrixTwoIn_H12], [MatrixTwoIn_H13], [MatrixTwoIn_H14], [MatrixTwoIn_H21], [MatrixTwoIn_H22], [MatrixTwoIn_H23], [MatrixTwoIn_H24], [SystemInputVolume_B19], [SystemInputVolumeErrorMargin_B21], [AuthorizedConsumption_K12], [AuthorizedConsumptionErrorMargin_K15], [WaterLosses_K29], [WaterLossesErrorMargin_K31], [BilledAuthorizedConsumption_T8], [UnbilledAuthorizedConsumption_T16], [UnbilledAuthorizedConsumptionErrorMargin_T20], [CommercialLosses_T26], [CommercialLossesErrorMargin_T29], [PhysicalLossesM3_T34], [PhyscialLossesErrorMargin_AH35], [BilledMeteredConsumption_AC4], [BilledUnmeteredConsumption_AC9], [UnbilledMeteredConsumption_AC14], [UnbilledUnmeteredConsumption_AC19], [UnbilledUnmeteredConsumptionErrorMargin_AO20], [UnauthorizedConsumption_AC24], [UnauthorizedConsumptionErrorMargin_AO25], [CustomerMeterInaccuraciesAndErrorsM3_AC29], [CustomerMeterInaccuraciesAndErrorsErrorMargin_AO30], [RevenueWaterM3_AY8], [NonRevenueWaterM3_AY24], [NonRevenueWaterErrorMargin_AY26]) VALUES (1083, N'EMEA\79749', CAST(N'2021-01-29T09:12:18.097' AS DateTime), N'EMEA\79749', CAST(N'2021-01-29T09:12:18.097' AS DateTime), 6773, 2021, 1, N'', 0, 0, 31, NULL, NULL, NULL, NULL, 0, 0, 0, 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, N'0', 0, 0, 0, 1, 1.5, 2, 4, 8, 2, 4, 8, 16, 25, 50, 100, 200, 25, 50, 100, 200, 50, 100, 200, 400, 40, 75, 150, 300, 75, 150, 300, 600, 50, 100, 200, 400, 100, 200, 400, 800, 60, 125, 250, 500, 125, 250, 500, 1000, 2, 55, 110, 220, 400, 50, 100, 200, 350, 80, 160, 320, 600, 65, 125, 250, 450, 105, 210, 420, 800, 75, 150, 300, 550, 130, 260, 520, 1000, 85, 175, 350, 650, 155, 310, 620, 1200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)
GO
INSERT [dbo].[tbWbEasyCalcData] ([WbEasyCalcDataId], [CreateLogin], [CreateDate], [ModifyLogin], [ModifyDate], [ZoneId], [YearNo], [MonthNo], [Description], [IsArchive], [IsAccepted], [Start_PeriodDays_M21], [SysInput_Desc_B6], [SysInput_Desc_B7], [SysInput_Desc_B8], [SysInput_Desc_B9], [SysInput_SystemInputVolumeM3_D6], [SysInput_SystemInputVolumeError_F6], [SysInput_SystemInputVolumeM3_D7], [SysInput_SystemInputVolumeError_F7], [SysInput_SystemInputVolumeM3_D8], [SysInput_SystemInputVolumeError_F8], [SysInput_SystemInputVolumeM3_D9], [SysInput_SystemInputVolumeError_F9], [BilledCons_Desc_B8], [BilledCons_Desc_B9], [BilledCons_Desc_B10], [BilledCons_Desc_B11], [BilledCons_Desc_F8], [BilledCons_Desc_F9], [BilledCons_Desc_F10], [BilledCons_Desc_F11], [UnbilledCons_Desc_D8], [UnbilledCons_Desc_D9], [UnbilledCons_Desc_D10], [UnbilledCons_Desc_D11], [UnbilledCons_Desc_F6], [UnbilledCons_Desc_F7], [UnbilledCons_Desc_F8], [UnbilledCons_Desc_F9], [UnbilledCons_Desc_F10], [UnbilledCons_Desc_F11], [UnauthCons_Desc_B18], [UnauthCons_Desc_B19], [UnauthCons_Desc_B20], [UnauthCons_Desc_B21], [MetErrors_Desc_D12], [MetErrors_Desc_D13], [MetErrors_Desc_D14], [MetErrors_Desc_D15], [Network_Desc_B7], [Network_Desc_B8], [Network_Desc_B9], [Network_Desc_B10], [Interm_Area_B7], [Interm_Area_B8], [Interm_Area_B9], [Interm_Area_B10], [BilledCons_BilledMetConsBulkWatSupExpM3_D6], [BilledCons_BilledUnmetConsBulkWatSupExpM3_H6], [BilledCons_UnbMetConsM3_D8], [BilledCons_UnbMetConsM3_D9], [BilledCons_UnbMetConsM3_D10], [BilledCons_UnbMetConsM3_D11], [BilledCons_UnbUnmetConsM3_H8], [BilledCons_UnbUnmetConsM3_H9], [BilledCons_UnbUnmetConsM3_H10], [BilledCons_UnbUnmetConsM3_H11], [UnbilledCons_MetConsBulkWatSupExpM3_D6], [UnbilledCons_UnbMetConsM3_D8], [UnbilledCons_UnbMetConsM3_D9], [UnbilledCons_UnbMetConsM3_D10], [UnbilledCons_UnbMetConsM3_D11], [UnbilledCons_UnbUnmetConsM3_H6], [UnbilledCons_UnbUnmetConsM3_H7], [UnbilledCons_UnbUnmetConsM3_H8], [UnbilledCons_UnbUnmetConsM3_H9], [UnbilledCons_UnbUnmetConsM3_H10], [UnbilledCons_UnbUnmetConsM3_H11], [UnbilledCons_UnbUnmetConsError_J6], [UnbilledCons_UnbUnmetConsError_J7], [UnbilledCons_UnbUnmetConsError_J8], [UnbilledCons_UnbUnmetConsError_J9], [UnbilledCons_UnbUnmetConsError_J10], [UnbilledCons_UnbUnmetConsError_J11], [UnauthCons_IllegalConnDomEstNo_D6], [UnauthCons_IllegalConnDomPersPerHouse_H6], [UnauthCons_IllegalConnDomConsLitPerPersDay_J6], [UnauthCons_IllegalConnDomErrorMargin_F6], [UnauthCons_IllegalConnOthersErrorMargin_F10], [IllegalConnectionsOthersEstimatedNumber_D10], [IllegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10], [UnauthCons_MeterTampBypEtcEstNo_D14], [UnauthCons_MeterTampBypEtcErrorMargin_F14], [UnauthCons_MeterTampBypEtcConsLitPerCustDay_J14], [UnauthCons_OthersErrorMargin_F18], [UnauthCons_OthersErrorMargin_F19], [UnauthCons_OthersErrorMargin_F20], [UnauthCons_OthersErrorMargin_F21], [UnauthCons_OthersM3PerDay_J18], [UnauthCons_OthersM3PerDay_J19], [UnauthCons_OthersM3PerDay_J20], [UnauthCons_OthersM3PerDay_J21], [MetErrors_DetailedManualSpec_J6], [MetErrors_BilledMetConsWoBulkSupMetUndrreg_H8], [MetErrors_BilledMetConsWoBulkSupErrorMargin_N8], [MetErrors_Total_F12], [MetErrors_Total_F13], [MetErrors_Total_F14], [MetErrors_Total_F15], [MetErrors_Meter_H12], [MetErrors_Meter_H13], [MetErrors_Meter_H14], [MetErrors_Meter_H15], [MetErrors_Error_N12], [MetErrors_Error_N13], [MetErrors_Error_N14], [MetErrors_Error_N15], [MeteredBulkSupplyExportErrorMargin_N32], [UnbilledMeteredConsumptionWithoutBulkSupplyErrorMargin_N34], [CorruptMeterReadingPracticessErrorMargin_N38], [DataHandlingErrorsOffice_L40], [DataHandlingErrorsOfficeErrorMargin_N40], [MetErrors_MetBulkSupExpMetUnderreg_H32], [MetErrors_UnbillMetConsWoBulkSupplMetUndrreg_H34], [MetErrors_CorruptMetReadPractMetUndrreg_H38], [Network_DistributionAndTransmissionMains_D7], [Network_DistributionAndTransmissionMains_D8], [Network_DistributionAndTransmissionMains_D9], [Network_DistributionAndTransmissionMains_D10], [Network_NoOfConnOfRegCustomers_H10], [Network_NoOfInactAccountsWSvcConns_H18], [Network_AvgLenOfSvcConnFromBoundaryToMeterM_H32], [Network_PossibleUnd_D30], [Network_NoCustomers_H7], [Network_ErrorMargin_J7], [Network_ErrorMargin_J10], [Network_ErrorMargin_J18], [Network_ErrorMargin_J32], [Network_ErrorMargin_D35], [Prs_Area_B7], [Prs_Area_B8], [Prs_Area_B9], [Prs_Area_B10], [Prs_ApproxNoOfConn_D7], [Prs_DailyAvgPrsM_F7], [Prs_ApproxNoOfConn_D8], [Prs_DailyAvgPrsM_F8], [Prs_ApproxNoOfConn_D9], [Prs_DailyAvgPrsM_F9], [Prs_ApproxNoOfConn_D10], [Prs_DailyAvgPrsM_F10], [Prs_ErrorMarg_F26], [Interm_Conn_D7], [Interm_Conn_D8], [Interm_Conn_D9], [Interm_Conn_D10], [Interm_Days_F7], [Interm_Days_F8], [Interm_Days_F9], [Interm_Days_F10], [Interm_Hour_H7], [Interm_Hour_H8], [Interm_Hour_H9], [Interm_Hour_H10], [Interm_ErrorMarg_H26], [FinancData_G6], [FinancData_K6], [FinancData_G8], [FinancData_D26], [FinancData_G35], [MatrixOneIn_SelectedOption], [MatrixOneIn_C11], [MatrixOneIn_C12], [MatrixOneIn_C13], [MatrixOneIn_C14], [MatrixOneIn_C21], [MatrixOneIn_C22], [MatrixOneIn_C23], [MatrixOneIn_C24], [MatrixOneIn_D21], [MatrixOneIn_D22], [MatrixOneIn_D23], [MatrixOneIn_D24], [MatrixOneIn_E11], [MatrixOneIn_E12], [MatrixOneIn_E13], [MatrixOneIn_E14], [MatrixOneIn_E21], [MatrixOneIn_E22], [MatrixOneIn_E23], [MatrixOneIn_E24], [MatrixOneIn_F11], [MatrixOneIn_F12], [MatrixOneIn_F13], [MatrixOneIn_F14], [MatrixOneIn_F21], [MatrixOneIn_F22], [MatrixOneIn_F23], [MatrixOneIn_F24], [MatrixOneIn_G11], [MatrixOneIn_G12], [MatrixOneIn_G13], [MatrixOneIn_G14], [MatrixOneIn_G21], [MatrixOneIn_G22], [MatrixOneIn_G23], [MatrixOneIn_G24], [MatrixOneIn_H11], [MatrixOneIn_H12], [MatrixOneIn_H13], [MatrixOneIn_H14], [MatrixOneIn_H21], [MatrixOneIn_H22], [MatrixOneIn_H23], [MatrixOneIn_H24], [MatrixTwoIn_SelectedOption], [MatrixTwoIn_D21], [MatrixTwoIn_D22], [MatrixTwoIn_D23], [MatrixTwoIn_D24], [MatrixTwoIn_E11], [MatrixTwoIn_E12], [MatrixTwoIn_E13], [MatrixTwoIn_E14], [MatrixTwoIn_E21], [MatrixTwoIn_E22], [MatrixTwoIn_E23], [MatrixTwoIn_E24], [MatrixTwoIn_F11], [MatrixTwoIn_F12], [MatrixTwoIn_F13], [MatrixTwoIn_F14], [MatrixTwoIn_F21], [MatrixTwoIn_F22], [MatrixTwoIn_F23], [MatrixTwoIn_F24], [MatrixTwoIn_G11], [MatrixTwoIn_G12], [MatrixTwoIn_G13], [MatrixTwoIn_G14], [MatrixTwoIn_G21], [MatrixTwoIn_G22], [MatrixTwoIn_G23], [MatrixTwoIn_G24], [MatrixTwoIn_H11], [MatrixTwoIn_H12], [MatrixTwoIn_H13], [MatrixTwoIn_H14], [MatrixTwoIn_H21], [MatrixTwoIn_H22], [MatrixTwoIn_H23], [MatrixTwoIn_H24], [SystemInputVolume_B19], [SystemInputVolumeErrorMargin_B21], [AuthorizedConsumption_K12], [AuthorizedConsumptionErrorMargin_K15], [WaterLosses_K29], [WaterLossesErrorMargin_K31], [BilledAuthorizedConsumption_T8], [UnbilledAuthorizedConsumption_T16], [UnbilledAuthorizedConsumptionErrorMargin_T20], [CommercialLosses_T26], [CommercialLossesErrorMargin_T29], [PhysicalLossesM3_T34], [PhyscialLossesErrorMargin_AH35], [BilledMeteredConsumption_AC4], [BilledUnmeteredConsumption_AC9], [UnbilledMeteredConsumption_AC14], [UnbilledUnmeteredConsumption_AC19], [UnbilledUnmeteredConsumptionErrorMargin_AO20], [UnauthorizedConsumption_AC24], [UnauthorizedConsumptionErrorMargin_AO25], [CustomerMeterInaccuraciesAndErrorsM3_AC29], [CustomerMeterInaccuraciesAndErrorsErrorMargin_AO30], [RevenueWaterM3_AY8], [NonRevenueWaterM3_AY24], [NonRevenueWaterErrorMargin_AY26]) VALUES (1084, NULL, CAST(N'2021-02-04T20:24:18.240' AS DateTime), N'EMEA\79749', CAST(N'2021-02-09T10:14:54.167' AS DateTime), 6773, 2020, 4, N'4.1', 0, 0, 30, NULL, NULL, NULL, NULL, 6593339, 0.05, 0, 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 5332026, 1000, 3000, 0, 0, 0, 2000, 0, 0, 0, 309349, 3000, 0, 0, 0, 2000, 0, 0, 0, 0, 0, 0.05, 0, 0, 0, 0, 0, 100, 3, 120, 0.05, 0.05, 500, 200, 1000, 0.1, 160, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0.03, 0.02, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0.02, 0.02, 0.02, 100, 0.02, 0.03, 0.03, 0.03, 260, 0, 0, 0, 8000, 1500, 7, 0, 8700, 0.01, 0, 0, 0.02, 0, NULL, NULL, NULL, NULL, 9500, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, N'PLN', 1, 0, 0, 1, 1.5, 2, 4, 8, 2, 4, 8, 16, 25, 50, 100, 200, 25, 50, 100, 200, 50, 100, 200, 400, 40, 75, 150, 300, 75, 150, 300, 600, 50, 100, 200, 400, 100, 200, 400, 800, 60, 125, 250, 500, 125, 250, 500, 1000, 2, 55, 110, 220, 400, 50, 100, 200, 350, 80, 160, 320, 600, 65, 125, 250, 450, 105, 210, 420, 800, 75, 150, 300, 550, 130, 260, 520, 1000, 85, 175, 350, 650, 155, 310, 620, 1200, 6593339, 0.05, 5652375, 1.7691678276830535E-05, 940964, 0.35035024205689252, 5338026, 314349, 0.00031811776083270503, 348734.67010309332, 0.013851842071187582, 592229.32989690662, 0.55671399270202115, 5335026, 3000, 312349, 2000, 0.05, 8880, 0.056957495718128637, 339854.67010309332, 0.014135648043349982, 5338026, 1255313, 0.26261733129506348)
GO
INSERT [dbo].[tbWbEasyCalcData] ([WbEasyCalcDataId], [CreateLogin], [CreateDate], [ModifyLogin], [ModifyDate], [ZoneId], [YearNo], [MonthNo], [Description], [IsArchive], [IsAccepted], [Start_PeriodDays_M21], [SysInput_Desc_B6], [SysInput_Desc_B7], [SysInput_Desc_B8], [SysInput_Desc_B9], [SysInput_SystemInputVolumeM3_D6], [SysInput_SystemInputVolumeError_F6], [SysInput_SystemInputVolumeM3_D7], [SysInput_SystemInputVolumeError_F7], [SysInput_SystemInputVolumeM3_D8], [SysInput_SystemInputVolumeError_F8], [SysInput_SystemInputVolumeM3_D9], [SysInput_SystemInputVolumeError_F9], [BilledCons_Desc_B8], [BilledCons_Desc_B9], [BilledCons_Desc_B10], [BilledCons_Desc_B11], [BilledCons_Desc_F8], [BilledCons_Desc_F9], [BilledCons_Desc_F10], [BilledCons_Desc_F11], [UnbilledCons_Desc_D8], [UnbilledCons_Desc_D9], [UnbilledCons_Desc_D10], [UnbilledCons_Desc_D11], [UnbilledCons_Desc_F6], [UnbilledCons_Desc_F7], [UnbilledCons_Desc_F8], [UnbilledCons_Desc_F9], [UnbilledCons_Desc_F10], [UnbilledCons_Desc_F11], [UnauthCons_Desc_B18], [UnauthCons_Desc_B19], [UnauthCons_Desc_B20], [UnauthCons_Desc_B21], [MetErrors_Desc_D12], [MetErrors_Desc_D13], [MetErrors_Desc_D14], [MetErrors_Desc_D15], [Network_Desc_B7], [Network_Desc_B8], [Network_Desc_B9], [Network_Desc_B10], [Interm_Area_B7], [Interm_Area_B8], [Interm_Area_B9], [Interm_Area_B10], [BilledCons_BilledMetConsBulkWatSupExpM3_D6], [BilledCons_BilledUnmetConsBulkWatSupExpM3_H6], [BilledCons_UnbMetConsM3_D8], [BilledCons_UnbMetConsM3_D9], [BilledCons_UnbMetConsM3_D10], [BilledCons_UnbMetConsM3_D11], [BilledCons_UnbUnmetConsM3_H8], [BilledCons_UnbUnmetConsM3_H9], [BilledCons_UnbUnmetConsM3_H10], [BilledCons_UnbUnmetConsM3_H11], [UnbilledCons_MetConsBulkWatSupExpM3_D6], [UnbilledCons_UnbMetConsM3_D8], [UnbilledCons_UnbMetConsM3_D9], [UnbilledCons_UnbMetConsM3_D10], [UnbilledCons_UnbMetConsM3_D11], [UnbilledCons_UnbUnmetConsM3_H6], [UnbilledCons_UnbUnmetConsM3_H7], [UnbilledCons_UnbUnmetConsM3_H8], [UnbilledCons_UnbUnmetConsM3_H9], [UnbilledCons_UnbUnmetConsM3_H10], [UnbilledCons_UnbUnmetConsM3_H11], [UnbilledCons_UnbUnmetConsError_J6], [UnbilledCons_UnbUnmetConsError_J7], [UnbilledCons_UnbUnmetConsError_J8], [UnbilledCons_UnbUnmetConsError_J9], [UnbilledCons_UnbUnmetConsError_J10], [UnbilledCons_UnbUnmetConsError_J11], [UnauthCons_IllegalConnDomEstNo_D6], [UnauthCons_IllegalConnDomPersPerHouse_H6], [UnauthCons_IllegalConnDomConsLitPerPersDay_J6], [UnauthCons_IllegalConnDomErrorMargin_F6], [UnauthCons_IllegalConnOthersErrorMargin_F10], [IllegalConnectionsOthersEstimatedNumber_D10], [IllegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10], [UnauthCons_MeterTampBypEtcEstNo_D14], [UnauthCons_MeterTampBypEtcErrorMargin_F14], [UnauthCons_MeterTampBypEtcConsLitPerCustDay_J14], [UnauthCons_OthersErrorMargin_F18], [UnauthCons_OthersErrorMargin_F19], [UnauthCons_OthersErrorMargin_F20], [UnauthCons_OthersErrorMargin_F21], [UnauthCons_OthersM3PerDay_J18], [UnauthCons_OthersM3PerDay_J19], [UnauthCons_OthersM3PerDay_J20], [UnauthCons_OthersM3PerDay_J21], [MetErrors_DetailedManualSpec_J6], [MetErrors_BilledMetConsWoBulkSupMetUndrreg_H8], [MetErrors_BilledMetConsWoBulkSupErrorMargin_N8], [MetErrors_Total_F12], [MetErrors_Total_F13], [MetErrors_Total_F14], [MetErrors_Total_F15], [MetErrors_Meter_H12], [MetErrors_Meter_H13], [MetErrors_Meter_H14], [MetErrors_Meter_H15], [MetErrors_Error_N12], [MetErrors_Error_N13], [MetErrors_Error_N14], [MetErrors_Error_N15], [MeteredBulkSupplyExportErrorMargin_N32], [UnbilledMeteredConsumptionWithoutBulkSupplyErrorMargin_N34], [CorruptMeterReadingPracticessErrorMargin_N38], [DataHandlingErrorsOffice_L40], [DataHandlingErrorsOfficeErrorMargin_N40], [MetErrors_MetBulkSupExpMetUnderreg_H32], [MetErrors_UnbillMetConsWoBulkSupplMetUndrreg_H34], [MetErrors_CorruptMetReadPractMetUndrreg_H38], [Network_DistributionAndTransmissionMains_D7], [Network_DistributionAndTransmissionMains_D8], [Network_DistributionAndTransmissionMains_D9], [Network_DistributionAndTransmissionMains_D10], [Network_NoOfConnOfRegCustomers_H10], [Network_NoOfInactAccountsWSvcConns_H18], [Network_AvgLenOfSvcConnFromBoundaryToMeterM_H32], [Network_PossibleUnd_D30], [Network_NoCustomers_H7], [Network_ErrorMargin_J7], [Network_ErrorMargin_J10], [Network_ErrorMargin_J18], [Network_ErrorMargin_J32], [Network_ErrorMargin_D35], [Prs_Area_B7], [Prs_Area_B8], [Prs_Area_B9], [Prs_Area_B10], [Prs_ApproxNoOfConn_D7], [Prs_DailyAvgPrsM_F7], [Prs_ApproxNoOfConn_D8], [Prs_DailyAvgPrsM_F8], [Prs_ApproxNoOfConn_D9], [Prs_DailyAvgPrsM_F9], [Prs_ApproxNoOfConn_D10], [Prs_DailyAvgPrsM_F10], [Prs_ErrorMarg_F26], [Interm_Conn_D7], [Interm_Conn_D8], [Interm_Conn_D9], [Interm_Conn_D10], [Interm_Days_F7], [Interm_Days_F8], [Interm_Days_F9], [Interm_Days_F10], [Interm_Hour_H7], [Interm_Hour_H8], [Interm_Hour_H9], [Interm_Hour_H10], [Interm_ErrorMarg_H26], [FinancData_G6], [FinancData_K6], [FinancData_G8], [FinancData_D26], [FinancData_G35], [MatrixOneIn_SelectedOption], [MatrixOneIn_C11], [MatrixOneIn_C12], [MatrixOneIn_C13], [MatrixOneIn_C14], [MatrixOneIn_C21], [MatrixOneIn_C22], [MatrixOneIn_C23], [MatrixOneIn_C24], [MatrixOneIn_D21], [MatrixOneIn_D22], [MatrixOneIn_D23], [MatrixOneIn_D24], [MatrixOneIn_E11], [MatrixOneIn_E12], [MatrixOneIn_E13], [MatrixOneIn_E14], [MatrixOneIn_E21], [MatrixOneIn_E22], [MatrixOneIn_E23], [MatrixOneIn_E24], [MatrixOneIn_F11], [MatrixOneIn_F12], [MatrixOneIn_F13], [MatrixOneIn_F14], [MatrixOneIn_F21], [MatrixOneIn_F22], [MatrixOneIn_F23], [MatrixOneIn_F24], [MatrixOneIn_G11], [MatrixOneIn_G12], [MatrixOneIn_G13], [MatrixOneIn_G14], [MatrixOneIn_G21], [MatrixOneIn_G22], [MatrixOneIn_G23], [MatrixOneIn_G24], [MatrixOneIn_H11], [MatrixOneIn_H12], [MatrixOneIn_H13], [MatrixOneIn_H14], [MatrixOneIn_H21], [MatrixOneIn_H22], [MatrixOneIn_H23], [MatrixOneIn_H24], [MatrixTwoIn_SelectedOption], [MatrixTwoIn_D21], [MatrixTwoIn_D22], [MatrixTwoIn_D23], [MatrixTwoIn_D24], [MatrixTwoIn_E11], [MatrixTwoIn_E12], [MatrixTwoIn_E13], [MatrixTwoIn_E14], [MatrixTwoIn_E21], [MatrixTwoIn_E22], [MatrixTwoIn_E23], [MatrixTwoIn_E24], [MatrixTwoIn_F11], [MatrixTwoIn_F12], [MatrixTwoIn_F13], [MatrixTwoIn_F14], [MatrixTwoIn_F21], [MatrixTwoIn_F22], [MatrixTwoIn_F23], [MatrixTwoIn_F24], [MatrixTwoIn_G11], [MatrixTwoIn_G12], [MatrixTwoIn_G13], [MatrixTwoIn_G14], [MatrixTwoIn_G21], [MatrixTwoIn_G22], [MatrixTwoIn_G23], [MatrixTwoIn_G24], [MatrixTwoIn_H11], [MatrixTwoIn_H12], [MatrixTwoIn_H13], [MatrixTwoIn_H14], [MatrixTwoIn_H21], [MatrixTwoIn_H22], [MatrixTwoIn_H23], [MatrixTwoIn_H24], [SystemInputVolume_B19], [SystemInputVolumeErrorMargin_B21], [AuthorizedConsumption_K12], [AuthorizedConsumptionErrorMargin_K15], [WaterLosses_K29], [WaterLossesErrorMargin_K31], [BilledAuthorizedConsumption_T8], [UnbilledAuthorizedConsumption_T16], [UnbilledAuthorizedConsumptionErrorMargin_T20], [CommercialLosses_T26], [CommercialLossesErrorMargin_T29], [PhysicalLossesM3_T34], [PhyscialLossesErrorMargin_AH35], [BilledMeteredConsumption_AC4], [BilledUnmeteredConsumption_AC9], [UnbilledMeteredConsumption_AC14], [UnbilledUnmeteredConsumption_AC19], [UnbilledUnmeteredConsumptionErrorMargin_AO20], [UnauthorizedConsumption_AC24], [UnauthorizedConsumptionErrorMargin_AO25], [CustomerMeterInaccuraciesAndErrorsM3_AC29], [CustomerMeterInaccuraciesAndErrorsErrorMargin_AO30], [RevenueWaterM3_AY8], [NonRevenueWaterM3_AY24], [NonRevenueWaterErrorMargin_AY26]) VALUES (1088, N'EMEA\79749', CAST(N'2021-02-10T09:55:53.470' AS DateTime), N'EMEA\79749', CAST(N'2021-02-10T15:28:18.577' AS DateTime), 6773, 2020, 4, N'4.1', 0, 0, 30, NULL, NULL, NULL, NULL, 7593339, 0.05, 0, 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 5332026, 1000, 3000, 0, 0, 0, 2000, 0, 0, 0, 309349, 3000, 0, 0, 0, 2000, 0, 0, 0, 0, 0, 0.05, 0, 0, 0, 0, 0, 100, 3, 120, 0.05, 0.05, 500, 200, 1000, 0.1, 160, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0.03, 0.02, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0.02, 0.02, 0.02, 100, 0.02, 0.03, 0.03, 0.03, 260, 0, 0, 0, 8000, 1500, 7, 0, 8700, 0.01, 0, 0, 0.02, 0, NULL, NULL, NULL, NULL, 9500, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, N'PLN', 1, 0, 0, 1, 1.5, 2, 4, 8, 2, 4, 8, 16, 25, 50, 100, 200, 25, 50, 100, 200, 50, 100, 200, 400, 40, 75, 150, 300, 75, 150, 300, 600, 50, 100, 200, 400, 100, 200, 400, 800, 60, 125, 250, 500, 125, 250, 500, 1000, 2, 55, 110, 220, 400, 50, 100, 200, 350, 80, 160, 320, 600, 65, 125, 250, 450, 105, 210, 420, 800, 75, 150, 300, 550, 130, 260, 520, 1000, 85, 175, 350, 650, 155, 310, 620, 1200, 7593339, 0.05, 5652375, 1.7691678276830535E-05, 1940964, 0.19560742145111232, 5338026, 314349, 0.00031811776083270503, 348734.67010309332, 0.013851842071187582, 1592229.3298969066, 0.23846922393677389, 5335026, 3000, 312349, 2000, 0.05, 8880, 0.056957495718128637, 339854.67010309332, 0.014135648043349982, 5338026, 2255313, 0.16834335189838395)
GO
INSERT [dbo].[tbWbEasyCalcData] ([WbEasyCalcDataId], [CreateLogin], [CreateDate], [ModifyLogin], [ModifyDate], [ZoneId], [YearNo], [MonthNo], [Description], [IsArchive], [IsAccepted], [Start_PeriodDays_M21], [SysInput_Desc_B6], [SysInput_Desc_B7], [SysInput_Desc_B8], [SysInput_Desc_B9], [SysInput_SystemInputVolumeM3_D6], [SysInput_SystemInputVolumeError_F6], [SysInput_SystemInputVolumeM3_D7], [SysInput_SystemInputVolumeError_F7], [SysInput_SystemInputVolumeM3_D8], [SysInput_SystemInputVolumeError_F8], [SysInput_SystemInputVolumeM3_D9], [SysInput_SystemInputVolumeError_F9], [BilledCons_Desc_B8], [BilledCons_Desc_B9], [BilledCons_Desc_B10], [BilledCons_Desc_B11], [BilledCons_Desc_F8], [BilledCons_Desc_F9], [BilledCons_Desc_F10], [BilledCons_Desc_F11], [UnbilledCons_Desc_D8], [UnbilledCons_Desc_D9], [UnbilledCons_Desc_D10], [UnbilledCons_Desc_D11], [UnbilledCons_Desc_F6], [UnbilledCons_Desc_F7], [UnbilledCons_Desc_F8], [UnbilledCons_Desc_F9], [UnbilledCons_Desc_F10], [UnbilledCons_Desc_F11], [UnauthCons_Desc_B18], [UnauthCons_Desc_B19], [UnauthCons_Desc_B20], [UnauthCons_Desc_B21], [MetErrors_Desc_D12], [MetErrors_Desc_D13], [MetErrors_Desc_D14], [MetErrors_Desc_D15], [Network_Desc_B7], [Network_Desc_B8], [Network_Desc_B9], [Network_Desc_B10], [Interm_Area_B7], [Interm_Area_B8], [Interm_Area_B9], [Interm_Area_B10], [BilledCons_BilledMetConsBulkWatSupExpM3_D6], [BilledCons_BilledUnmetConsBulkWatSupExpM3_H6], [BilledCons_UnbMetConsM3_D8], [BilledCons_UnbMetConsM3_D9], [BilledCons_UnbMetConsM3_D10], [BilledCons_UnbMetConsM3_D11], [BilledCons_UnbUnmetConsM3_H8], [BilledCons_UnbUnmetConsM3_H9], [BilledCons_UnbUnmetConsM3_H10], [BilledCons_UnbUnmetConsM3_H11], [UnbilledCons_MetConsBulkWatSupExpM3_D6], [UnbilledCons_UnbMetConsM3_D8], [UnbilledCons_UnbMetConsM3_D9], [UnbilledCons_UnbMetConsM3_D10], [UnbilledCons_UnbMetConsM3_D11], [UnbilledCons_UnbUnmetConsM3_H6], [UnbilledCons_UnbUnmetConsM3_H7], [UnbilledCons_UnbUnmetConsM3_H8], [UnbilledCons_UnbUnmetConsM3_H9], [UnbilledCons_UnbUnmetConsM3_H10], [UnbilledCons_UnbUnmetConsM3_H11], [UnbilledCons_UnbUnmetConsError_J6], [UnbilledCons_UnbUnmetConsError_J7], [UnbilledCons_UnbUnmetConsError_J8], [UnbilledCons_UnbUnmetConsError_J9], [UnbilledCons_UnbUnmetConsError_J10], [UnbilledCons_UnbUnmetConsError_J11], [UnauthCons_IllegalConnDomEstNo_D6], [UnauthCons_IllegalConnDomPersPerHouse_H6], [UnauthCons_IllegalConnDomConsLitPerPersDay_J6], [UnauthCons_IllegalConnDomErrorMargin_F6], [UnauthCons_IllegalConnOthersErrorMargin_F10], [IllegalConnectionsOthersEstimatedNumber_D10], [IllegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10], [UnauthCons_MeterTampBypEtcEstNo_D14], [UnauthCons_MeterTampBypEtcErrorMargin_F14], [UnauthCons_MeterTampBypEtcConsLitPerCustDay_J14], [UnauthCons_OthersErrorMargin_F18], [UnauthCons_OthersErrorMargin_F19], [UnauthCons_OthersErrorMargin_F20], [UnauthCons_OthersErrorMargin_F21], [UnauthCons_OthersM3PerDay_J18], [UnauthCons_OthersM3PerDay_J19], [UnauthCons_OthersM3PerDay_J20], [UnauthCons_OthersM3PerDay_J21], [MetErrors_DetailedManualSpec_J6], [MetErrors_BilledMetConsWoBulkSupMetUndrreg_H8], [MetErrors_BilledMetConsWoBulkSupErrorMargin_N8], [MetErrors_Total_F12], [MetErrors_Total_F13], [MetErrors_Total_F14], [MetErrors_Total_F15], [MetErrors_Meter_H12], [MetErrors_Meter_H13], [MetErrors_Meter_H14], [MetErrors_Meter_H15], [MetErrors_Error_N12], [MetErrors_Error_N13], [MetErrors_Error_N14], [MetErrors_Error_N15], [MeteredBulkSupplyExportErrorMargin_N32], [UnbilledMeteredConsumptionWithoutBulkSupplyErrorMargin_N34], [CorruptMeterReadingPracticessErrorMargin_N38], [DataHandlingErrorsOffice_L40], [DataHandlingErrorsOfficeErrorMargin_N40], [MetErrors_MetBulkSupExpMetUnderreg_H32], [MetErrors_UnbillMetConsWoBulkSupplMetUndrreg_H34], [MetErrors_CorruptMetReadPractMetUndrreg_H38], [Network_DistributionAndTransmissionMains_D7], [Network_DistributionAndTransmissionMains_D8], [Network_DistributionAndTransmissionMains_D9], [Network_DistributionAndTransmissionMains_D10], [Network_NoOfConnOfRegCustomers_H10], [Network_NoOfInactAccountsWSvcConns_H18], [Network_AvgLenOfSvcConnFromBoundaryToMeterM_H32], [Network_PossibleUnd_D30], [Network_NoCustomers_H7], [Network_ErrorMargin_J7], [Network_ErrorMargin_J10], [Network_ErrorMargin_J18], [Network_ErrorMargin_J32], [Network_ErrorMargin_D35], [Prs_Area_B7], [Prs_Area_B8], [Prs_Area_B9], [Prs_Area_B10], [Prs_ApproxNoOfConn_D7], [Prs_DailyAvgPrsM_F7], [Prs_ApproxNoOfConn_D8], [Prs_DailyAvgPrsM_F8], [Prs_ApproxNoOfConn_D9], [Prs_DailyAvgPrsM_F9], [Prs_ApproxNoOfConn_D10], [Prs_DailyAvgPrsM_F10], [Prs_ErrorMarg_F26], [Interm_Conn_D7], [Interm_Conn_D8], [Interm_Conn_D9], [Interm_Conn_D10], [Interm_Days_F7], [Interm_Days_F8], [Interm_Days_F9], [Interm_Days_F10], [Interm_Hour_H7], [Interm_Hour_H8], [Interm_Hour_H9], [Interm_Hour_H10], [Interm_ErrorMarg_H26], [FinancData_G6], [FinancData_K6], [FinancData_G8], [FinancData_D26], [FinancData_G35], [MatrixOneIn_SelectedOption], [MatrixOneIn_C11], [MatrixOneIn_C12], [MatrixOneIn_C13], [MatrixOneIn_C14], [MatrixOneIn_C21], [MatrixOneIn_C22], [MatrixOneIn_C23], [MatrixOneIn_C24], [MatrixOneIn_D21], [MatrixOneIn_D22], [MatrixOneIn_D23], [MatrixOneIn_D24], [MatrixOneIn_E11], [MatrixOneIn_E12], [MatrixOneIn_E13], [MatrixOneIn_E14], [MatrixOneIn_E21], [MatrixOneIn_E22], [MatrixOneIn_E23], [MatrixOneIn_E24], [MatrixOneIn_F11], [MatrixOneIn_F12], [MatrixOneIn_F13], [MatrixOneIn_F14], [MatrixOneIn_F21], [MatrixOneIn_F22], [MatrixOneIn_F23], [MatrixOneIn_F24], [MatrixOneIn_G11], [MatrixOneIn_G12], [MatrixOneIn_G13], [MatrixOneIn_G14], [MatrixOneIn_G21], [MatrixOneIn_G22], [MatrixOneIn_G23], [MatrixOneIn_G24], [MatrixOneIn_H11], [MatrixOneIn_H12], [MatrixOneIn_H13], [MatrixOneIn_H14], [MatrixOneIn_H21], [MatrixOneIn_H22], [MatrixOneIn_H23], [MatrixOneIn_H24], [MatrixTwoIn_SelectedOption], [MatrixTwoIn_D21], [MatrixTwoIn_D22], [MatrixTwoIn_D23], [MatrixTwoIn_D24], [MatrixTwoIn_E11], [MatrixTwoIn_E12], [MatrixTwoIn_E13], [MatrixTwoIn_E14], [MatrixTwoIn_E21], [MatrixTwoIn_E22], [MatrixTwoIn_E23], [MatrixTwoIn_E24], [MatrixTwoIn_F11], [MatrixTwoIn_F12], [MatrixTwoIn_F13], [MatrixTwoIn_F14], [MatrixTwoIn_F21], [MatrixTwoIn_F22], [MatrixTwoIn_F23], [MatrixTwoIn_F24], [MatrixTwoIn_G11], [MatrixTwoIn_G12], [MatrixTwoIn_G13], [MatrixTwoIn_G14], [MatrixTwoIn_G21], [MatrixTwoIn_G22], [MatrixTwoIn_G23], [MatrixTwoIn_G24], [MatrixTwoIn_H11], [MatrixTwoIn_H12], [MatrixTwoIn_H13], [MatrixTwoIn_H14], [MatrixTwoIn_H21], [MatrixTwoIn_H22], [MatrixTwoIn_H23], [MatrixTwoIn_H24], [SystemInputVolume_B19], [SystemInputVolumeErrorMargin_B21], [AuthorizedConsumption_K12], [AuthorizedConsumptionErrorMargin_K15], [WaterLosses_K29], [WaterLossesErrorMargin_K31], [BilledAuthorizedConsumption_T8], [UnbilledAuthorizedConsumption_T16], [UnbilledAuthorizedConsumptionErrorMargin_T20], [CommercialLosses_T26], [CommercialLossesErrorMargin_T29], [PhysicalLossesM3_T34], [PhyscialLossesErrorMargin_AH35], [BilledMeteredConsumption_AC4], [BilledUnmeteredConsumption_AC9], [UnbilledMeteredConsumption_AC14], [UnbilledUnmeteredConsumption_AC19], [UnbilledUnmeteredConsumptionErrorMargin_AO20], [UnauthorizedConsumption_AC24], [UnauthorizedConsumptionErrorMargin_AO25], [CustomerMeterInaccuraciesAndErrorsM3_AC29], [CustomerMeterInaccuraciesAndErrorsErrorMargin_AO30], [RevenueWaterM3_AY8], [NonRevenueWaterM3_AY24], [NonRevenueWaterErrorMargin_AY26]) VALUES (1089, N'EMEA\79749', CAST(N'2021-02-10T15:29:07.530' AS DateTime), N'EMEA\79749', CAST(N'2021-02-15T10:25:50.970' AS DateTime), 6773, 2019, 1, N'Old - first', 0, 0, 30, N'aaaaaa', N'dddddd', N'xxxxxxxxxx', N'nnnnnnnnn', 58000, 0.01, 0, 0, 0, 0, 0, 0, N'qq', N'ww', N'ee', N'rrr', N'tt', N'yy', N'uu', N'ii', N'aa', N'ss', N'dd', N'ff', N'gg', N'hh', N'jj', N'kk', N'll', N'zz', N'zz', N'xx', N'cc', N'vv', N'qqqq', N'www', N'eeee', N'rrrrrr', N'tt', N'yy', N'uu', N'iii', N'uuuuu', N'iiiiiiiii', N'ooooooooop', N'ppppppp', 500, 250, 14000, 220, 0, 0, 75, 0, 0, 0, 150, 300, 0, 0, 0, 100, 0, 0, 0, 0, 0, 0.05, 0, 0, 0, 0, 0, 20, 3, 120, 0.2, 0.1, 5, 500, 30, 0.2, 160, 0.4, 0, 0, 0, 3, 0, 0, 0, 1, 0.02, 0.05, 10000, 2000, 1000, 0, 0.03, 0.04, 0.05, 0, 0.1, 0.1, 0.1, 0, 0.05, 0.1, 0.02, 50, 0.02, 0.02, 0.05, 0.03, 24.2, 0, 0, 0, 700, 15, 10, 0.01, 758, 0, 0.01, 0.05, 0.05, 0.22, N'adddddddddd', N'gzzzzzzzzzzzzz', N'eggggggggg', N'jjjjjjjjjjjj', 740, 37.5, 0, 0, 0, 0, 0, 0, 0.03, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, N'PLN', 1, 10, 20000, 1, 1.5, 2, 4, 8, 2, 4, 8, 16, 25, 50, 100, 200, 25, 50, 100, 200, 50, 100, 200, 400, 40, 75, 150, 300, 75, 150, 300, 600, 50, 100, 200, 400, 100, 200, 400, 800, 60, 125, 250, 500, 125, 250, 500, 1000, 2, 55, 110, 220, 400, 50, 100, 200, 350, 80, 160, 320, 600, 65, 125, 250, 450, 105, 210, 420, 800, 75, 150, 300, 550, 130, 260, 520, 1000, 85, 175, 350, 650, 155, 310, 620, 1200, 58000, 0.01, 15595, 0.00032061558191728119, 42405, 0.013678140580680152, 15045, 550, 0.00909090909090909, 1359.5681397898293, 0.048505884633143863, 41045.43186021017, 0.014222253180015259, 14720, 325, 450, 100, 0.05, 525, 0.12118732337558567, 834.56813978982927, 0.020791322514394753, 15045, 42955, 0.013502502619019904)
GO
INSERT [dbo].[tbWbEasyCalcData] ([WbEasyCalcDataId], [CreateLogin], [CreateDate], [ModifyLogin], [ModifyDate], [ZoneId], [YearNo], [MonthNo], [Description], [IsArchive], [IsAccepted], [Start_PeriodDays_M21], [SysInput_Desc_B6], [SysInput_Desc_B7], [SysInput_Desc_B8], [SysInput_Desc_B9], [SysInput_SystemInputVolumeM3_D6], [SysInput_SystemInputVolumeError_F6], [SysInput_SystemInputVolumeM3_D7], [SysInput_SystemInputVolumeError_F7], [SysInput_SystemInputVolumeM3_D8], [SysInput_SystemInputVolumeError_F8], [SysInput_SystemInputVolumeM3_D9], [SysInput_SystemInputVolumeError_F9], [BilledCons_Desc_B8], [BilledCons_Desc_B9], [BilledCons_Desc_B10], [BilledCons_Desc_B11], [BilledCons_Desc_F8], [BilledCons_Desc_F9], [BilledCons_Desc_F10], [BilledCons_Desc_F11], [UnbilledCons_Desc_D8], [UnbilledCons_Desc_D9], [UnbilledCons_Desc_D10], [UnbilledCons_Desc_D11], [UnbilledCons_Desc_F6], [UnbilledCons_Desc_F7], [UnbilledCons_Desc_F8], [UnbilledCons_Desc_F9], [UnbilledCons_Desc_F10], [UnbilledCons_Desc_F11], [UnauthCons_Desc_B18], [UnauthCons_Desc_B19], [UnauthCons_Desc_B20], [UnauthCons_Desc_B21], [MetErrors_Desc_D12], [MetErrors_Desc_D13], [MetErrors_Desc_D14], [MetErrors_Desc_D15], [Network_Desc_B7], [Network_Desc_B8], [Network_Desc_B9], [Network_Desc_B10], [Interm_Area_B7], [Interm_Area_B8], [Interm_Area_B9], [Interm_Area_B10], [BilledCons_BilledMetConsBulkWatSupExpM3_D6], [BilledCons_BilledUnmetConsBulkWatSupExpM3_H6], [BilledCons_UnbMetConsM3_D8], [BilledCons_UnbMetConsM3_D9], [BilledCons_UnbMetConsM3_D10], [BilledCons_UnbMetConsM3_D11], [BilledCons_UnbUnmetConsM3_H8], [BilledCons_UnbUnmetConsM3_H9], [BilledCons_UnbUnmetConsM3_H10], [BilledCons_UnbUnmetConsM3_H11], [UnbilledCons_MetConsBulkWatSupExpM3_D6], [UnbilledCons_UnbMetConsM3_D8], [UnbilledCons_UnbMetConsM3_D9], [UnbilledCons_UnbMetConsM3_D10], [UnbilledCons_UnbMetConsM3_D11], [UnbilledCons_UnbUnmetConsM3_H6], [UnbilledCons_UnbUnmetConsM3_H7], [UnbilledCons_UnbUnmetConsM3_H8], [UnbilledCons_UnbUnmetConsM3_H9], [UnbilledCons_UnbUnmetConsM3_H10], [UnbilledCons_UnbUnmetConsM3_H11], [UnbilledCons_UnbUnmetConsError_J6], [UnbilledCons_UnbUnmetConsError_J7], [UnbilledCons_UnbUnmetConsError_J8], [UnbilledCons_UnbUnmetConsError_J9], [UnbilledCons_UnbUnmetConsError_J10], [UnbilledCons_UnbUnmetConsError_J11], [UnauthCons_IllegalConnDomEstNo_D6], [UnauthCons_IllegalConnDomPersPerHouse_H6], [UnauthCons_IllegalConnDomConsLitPerPersDay_J6], [UnauthCons_IllegalConnDomErrorMargin_F6], [UnauthCons_IllegalConnOthersErrorMargin_F10], [IllegalConnectionsOthersEstimatedNumber_D10], [IllegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10], [UnauthCons_MeterTampBypEtcEstNo_D14], [UnauthCons_MeterTampBypEtcErrorMargin_F14], [UnauthCons_MeterTampBypEtcConsLitPerCustDay_J14], [UnauthCons_OthersErrorMargin_F18], [UnauthCons_OthersErrorMargin_F19], [UnauthCons_OthersErrorMargin_F20], [UnauthCons_OthersErrorMargin_F21], [UnauthCons_OthersM3PerDay_J18], [UnauthCons_OthersM3PerDay_J19], [UnauthCons_OthersM3PerDay_J20], [UnauthCons_OthersM3PerDay_J21], [MetErrors_DetailedManualSpec_J6], [MetErrors_BilledMetConsWoBulkSupMetUndrreg_H8], [MetErrors_BilledMetConsWoBulkSupErrorMargin_N8], [MetErrors_Total_F12], [MetErrors_Total_F13], [MetErrors_Total_F14], [MetErrors_Total_F15], [MetErrors_Meter_H12], [MetErrors_Meter_H13], [MetErrors_Meter_H14], [MetErrors_Meter_H15], [MetErrors_Error_N12], [MetErrors_Error_N13], [MetErrors_Error_N14], [MetErrors_Error_N15], [MeteredBulkSupplyExportErrorMargin_N32], [UnbilledMeteredConsumptionWithoutBulkSupplyErrorMargin_N34], [CorruptMeterReadingPracticessErrorMargin_N38], [DataHandlingErrorsOffice_L40], [DataHandlingErrorsOfficeErrorMargin_N40], [MetErrors_MetBulkSupExpMetUnderreg_H32], [MetErrors_UnbillMetConsWoBulkSupplMetUndrreg_H34], [MetErrors_CorruptMetReadPractMetUndrreg_H38], [Network_DistributionAndTransmissionMains_D7], [Network_DistributionAndTransmissionMains_D8], [Network_DistributionAndTransmissionMains_D9], [Network_DistributionAndTransmissionMains_D10], [Network_NoOfConnOfRegCustomers_H10], [Network_NoOfInactAccountsWSvcConns_H18], [Network_AvgLenOfSvcConnFromBoundaryToMeterM_H32], [Network_PossibleUnd_D30], [Network_NoCustomers_H7], [Network_ErrorMargin_J7], [Network_ErrorMargin_J10], [Network_ErrorMargin_J18], [Network_ErrorMargin_J32], [Network_ErrorMargin_D35], [Prs_Area_B7], [Prs_Area_B8], [Prs_Area_B9], [Prs_Area_B10], [Prs_ApproxNoOfConn_D7], [Prs_DailyAvgPrsM_F7], [Prs_ApproxNoOfConn_D8], [Prs_DailyAvgPrsM_F8], [Prs_ApproxNoOfConn_D9], [Prs_DailyAvgPrsM_F9], [Prs_ApproxNoOfConn_D10], [Prs_DailyAvgPrsM_F10], [Prs_ErrorMarg_F26], [Interm_Conn_D7], [Interm_Conn_D8], [Interm_Conn_D9], [Interm_Conn_D10], [Interm_Days_F7], [Interm_Days_F8], [Interm_Days_F9], [Interm_Days_F10], [Interm_Hour_H7], [Interm_Hour_H8], [Interm_Hour_H9], [Interm_Hour_H10], [Interm_ErrorMarg_H26], [FinancData_G6], [FinancData_K6], [FinancData_G8], [FinancData_D26], [FinancData_G35], [MatrixOneIn_SelectedOption], [MatrixOneIn_C11], [MatrixOneIn_C12], [MatrixOneIn_C13], [MatrixOneIn_C14], [MatrixOneIn_C21], [MatrixOneIn_C22], [MatrixOneIn_C23], [MatrixOneIn_C24], [MatrixOneIn_D21], [MatrixOneIn_D22], [MatrixOneIn_D23], [MatrixOneIn_D24], [MatrixOneIn_E11], [MatrixOneIn_E12], [MatrixOneIn_E13], [MatrixOneIn_E14], [MatrixOneIn_E21], [MatrixOneIn_E22], [MatrixOneIn_E23], [MatrixOneIn_E24], [MatrixOneIn_F11], [MatrixOneIn_F12], [MatrixOneIn_F13], [MatrixOneIn_F14], [MatrixOneIn_F21], [MatrixOneIn_F22], [MatrixOneIn_F23], [MatrixOneIn_F24], [MatrixOneIn_G11], [MatrixOneIn_G12], [MatrixOneIn_G13], [MatrixOneIn_G14], [MatrixOneIn_G21], [MatrixOneIn_G22], [MatrixOneIn_G23], [MatrixOneIn_G24], [MatrixOneIn_H11], [MatrixOneIn_H12], [MatrixOneIn_H13], [MatrixOneIn_H14], [MatrixOneIn_H21], [MatrixOneIn_H22], [MatrixOneIn_H23], [MatrixOneIn_H24], [MatrixTwoIn_SelectedOption], [MatrixTwoIn_D21], [MatrixTwoIn_D22], [MatrixTwoIn_D23], [MatrixTwoIn_D24], [MatrixTwoIn_E11], [MatrixTwoIn_E12], [MatrixTwoIn_E13], [MatrixTwoIn_E14], [MatrixTwoIn_E21], [MatrixTwoIn_E22], [MatrixTwoIn_E23], [MatrixTwoIn_E24], [MatrixTwoIn_F11], [MatrixTwoIn_F12], [MatrixTwoIn_F13], [MatrixTwoIn_F14], [MatrixTwoIn_F21], [MatrixTwoIn_F22], [MatrixTwoIn_F23], [MatrixTwoIn_F24], [MatrixTwoIn_G11], [MatrixTwoIn_G12], [MatrixTwoIn_G13], [MatrixTwoIn_G14], [MatrixTwoIn_G21], [MatrixTwoIn_G22], [MatrixTwoIn_G23], [MatrixTwoIn_G24], [MatrixTwoIn_H11], [MatrixTwoIn_H12], [MatrixTwoIn_H13], [MatrixTwoIn_H14], [MatrixTwoIn_H21], [MatrixTwoIn_H22], [MatrixTwoIn_H23], [MatrixTwoIn_H24], [SystemInputVolume_B19], [SystemInputVolumeErrorMargin_B21], [AuthorizedConsumption_K12], [AuthorizedConsumptionErrorMargin_K15], [WaterLosses_K29], [WaterLossesErrorMargin_K31], [BilledAuthorizedConsumption_T8], [UnbilledAuthorizedConsumption_T16], [UnbilledAuthorizedConsumptionErrorMargin_T20], [CommercialLosses_T26], [CommercialLossesErrorMargin_T29], [PhysicalLossesM3_T34], [PhyscialLossesErrorMargin_AH35], [BilledMeteredConsumption_AC4], [BilledUnmeteredConsumption_AC9], [UnbilledMeteredConsumption_AC14], [UnbilledUnmeteredConsumption_AC19], [UnbilledUnmeteredConsumptionErrorMargin_AO20], [UnauthorizedConsumption_AC24], [UnauthorizedConsumptionErrorMargin_AO25], [CustomerMeterInaccuraciesAndErrorsM3_AC29], [CustomerMeterInaccuraciesAndErrorsErrorMargin_AO30], [RevenueWaterM3_AY8], [NonRevenueWaterM3_AY24], [NonRevenueWaterErrorMargin_AY26]) VALUES (1091, N'EMEA\79749', CAST(N'2021-02-14T09:43:34.353' AS DateTime), N'EMEA\79749', CAST(N'2021-02-14T10:56:56.070' AS DateTime), 6773, 2019, 1, N'Old - 3', 0, 0, 30, N'aaaaaa', N'dddddd', N'xxxxxxxxxx', N'nnnnnnnnn', 18000, 0.01, 0, 0, 0, 0, 0, 0, N'qq', N'ww', N'ee', N'rrr', N'tt', N'yy', N'uu', N'ii', N'aa', N'ss', N'dd', N'ff', N'gg', N'hh', N'jj', N'kk', N'll', N'zz', N'zz', N'xx', N'cc', N'vv', N'qqqq', N'www', N'eeee', N'rrrrrr', N'tt', N'yy', N'uu', N'iii', N'uuuuu', N'iiiiiiiii', N'ooooooooop', N'ppppppp', 500, 250, 14000, 220, 0, 0, 75, 0, 0, 0, 150, 300, 0, 0, 0, 100, 0, 0, 0, 0, 0, 0.05, 0, 0, 0, 0, 0, 20, 3, 120, 0.2, 0.1, 5, 500, 30, 0.2, 160, 0.4, 0, 0, 0, 3, 0, 0, 0, 1, 0.02, 0.05, 10000, 2000, 1000, 0, 0.03, 0.04, 0.05, 0, 0.1, 0.1, 0.1, 0, 0.05, 0.1, 0.02, 50, 0.02, 0.02, 0.05, 0.03, 24.2, 0, 0, 0, 700, 15, 10, 0.01, 758, 0, 0.01, 0.05, 0.05, 0.22, N'adddddddddd', N'gzzzzzzzzzzzzz', N'eggggggggg', N'jjjjjjjjjjjj', 740, 37.5, 0, 0, 0, 0, 0, 0, 0.03, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, N'PLN', 1, 10, 20000, 1, 1.5, 2, 4, 8, 2, 4, 8, 16, 25, 50, 100, 200, 25, 50, 100, 200, 50, 100, 200, 400, 40, 75, 150, 300, 75, 150, 300, 600, 50, 100, 200, 400, 100, 200, 400, 800, 60, 125, 250, 500, 125, 250, 500, 1000, 2, 55, 110, 220, 400, 50, 100, 200, 350, 80, 160, 320, 600, 65, 125, 250, 450, 105, 210, 420, 800, 75, 150, 300, 550, 130, 260, 520, 1000, 85, 175, 350, 650, 155, 310, 620, 1200, 18000, 0.01, 15595, 0.00032061558191728119, 2405, 0.074872944305069064, 15045, 550, 0.00909090909090909, 1359.5681397898293, 0.048505884633143863, 1045.4318602101707, 0.18343186759183736, 14720, 325, 450, 100, 0.05, 525, 0.12118732337558567, 834.56813978982927, 0.020791322514394753, 15045, 2955, 0.060913705583756347)
GO
INSERT [dbo].[tbWbEasyCalcData] ([WbEasyCalcDataId], [CreateLogin], [CreateDate], [ModifyLogin], [ModifyDate], [ZoneId], [YearNo], [MonthNo], [Description], [IsArchive], [IsAccepted], [Start_PeriodDays_M21], [SysInput_Desc_B6], [SysInput_Desc_B7], [SysInput_Desc_B8], [SysInput_Desc_B9], [SysInput_SystemInputVolumeM3_D6], [SysInput_SystemInputVolumeError_F6], [SysInput_SystemInputVolumeM3_D7], [SysInput_SystemInputVolumeError_F7], [SysInput_SystemInputVolumeM3_D8], [SysInput_SystemInputVolumeError_F8], [SysInput_SystemInputVolumeM3_D9], [SysInput_SystemInputVolumeError_F9], [BilledCons_Desc_B8], [BilledCons_Desc_B9], [BilledCons_Desc_B10], [BilledCons_Desc_B11], [BilledCons_Desc_F8], [BilledCons_Desc_F9], [BilledCons_Desc_F10], [BilledCons_Desc_F11], [UnbilledCons_Desc_D8], [UnbilledCons_Desc_D9], [UnbilledCons_Desc_D10], [UnbilledCons_Desc_D11], [UnbilledCons_Desc_F6], [UnbilledCons_Desc_F7], [UnbilledCons_Desc_F8], [UnbilledCons_Desc_F9], [UnbilledCons_Desc_F10], [UnbilledCons_Desc_F11], [UnauthCons_Desc_B18], [UnauthCons_Desc_B19], [UnauthCons_Desc_B20], [UnauthCons_Desc_B21], [MetErrors_Desc_D12], [MetErrors_Desc_D13], [MetErrors_Desc_D14], [MetErrors_Desc_D15], [Network_Desc_B7], [Network_Desc_B8], [Network_Desc_B9], [Network_Desc_B10], [Interm_Area_B7], [Interm_Area_B8], [Interm_Area_B9], [Interm_Area_B10], [BilledCons_BilledMetConsBulkWatSupExpM3_D6], [BilledCons_BilledUnmetConsBulkWatSupExpM3_H6], [BilledCons_UnbMetConsM3_D8], [BilledCons_UnbMetConsM3_D9], [BilledCons_UnbMetConsM3_D10], [BilledCons_UnbMetConsM3_D11], [BilledCons_UnbUnmetConsM3_H8], [BilledCons_UnbUnmetConsM3_H9], [BilledCons_UnbUnmetConsM3_H10], [BilledCons_UnbUnmetConsM3_H11], [UnbilledCons_MetConsBulkWatSupExpM3_D6], [UnbilledCons_UnbMetConsM3_D8], [UnbilledCons_UnbMetConsM3_D9], [UnbilledCons_UnbMetConsM3_D10], [UnbilledCons_UnbMetConsM3_D11], [UnbilledCons_UnbUnmetConsM3_H6], [UnbilledCons_UnbUnmetConsM3_H7], [UnbilledCons_UnbUnmetConsM3_H8], [UnbilledCons_UnbUnmetConsM3_H9], [UnbilledCons_UnbUnmetConsM3_H10], [UnbilledCons_UnbUnmetConsM3_H11], [UnbilledCons_UnbUnmetConsError_J6], [UnbilledCons_UnbUnmetConsError_J7], [UnbilledCons_UnbUnmetConsError_J8], [UnbilledCons_UnbUnmetConsError_J9], [UnbilledCons_UnbUnmetConsError_J10], [UnbilledCons_UnbUnmetConsError_J11], [UnauthCons_IllegalConnDomEstNo_D6], [UnauthCons_IllegalConnDomPersPerHouse_H6], [UnauthCons_IllegalConnDomConsLitPerPersDay_J6], [UnauthCons_IllegalConnDomErrorMargin_F6], [UnauthCons_IllegalConnOthersErrorMargin_F10], [IllegalConnectionsOthersEstimatedNumber_D10], [IllegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10], [UnauthCons_MeterTampBypEtcEstNo_D14], [UnauthCons_MeterTampBypEtcErrorMargin_F14], [UnauthCons_MeterTampBypEtcConsLitPerCustDay_J14], [UnauthCons_OthersErrorMargin_F18], [UnauthCons_OthersErrorMargin_F19], [UnauthCons_OthersErrorMargin_F20], [UnauthCons_OthersErrorMargin_F21], [UnauthCons_OthersM3PerDay_J18], [UnauthCons_OthersM3PerDay_J19], [UnauthCons_OthersM3PerDay_J20], [UnauthCons_OthersM3PerDay_J21], [MetErrors_DetailedManualSpec_J6], [MetErrors_BilledMetConsWoBulkSupMetUndrreg_H8], [MetErrors_BilledMetConsWoBulkSupErrorMargin_N8], [MetErrors_Total_F12], [MetErrors_Total_F13], [MetErrors_Total_F14], [MetErrors_Total_F15], [MetErrors_Meter_H12], [MetErrors_Meter_H13], [MetErrors_Meter_H14], [MetErrors_Meter_H15], [MetErrors_Error_N12], [MetErrors_Error_N13], [MetErrors_Error_N14], [MetErrors_Error_N15], [MeteredBulkSupplyExportErrorMargin_N32], [UnbilledMeteredConsumptionWithoutBulkSupplyErrorMargin_N34], [CorruptMeterReadingPracticessErrorMargin_N38], [DataHandlingErrorsOffice_L40], [DataHandlingErrorsOfficeErrorMargin_N40], [MetErrors_MetBulkSupExpMetUnderreg_H32], [MetErrors_UnbillMetConsWoBulkSupplMetUndrreg_H34], [MetErrors_CorruptMetReadPractMetUndrreg_H38], [Network_DistributionAndTransmissionMains_D7], [Network_DistributionAndTransmissionMains_D8], [Network_DistributionAndTransmissionMains_D9], [Network_DistributionAndTransmissionMains_D10], [Network_NoOfConnOfRegCustomers_H10], [Network_NoOfInactAccountsWSvcConns_H18], [Network_AvgLenOfSvcConnFromBoundaryToMeterM_H32], [Network_PossibleUnd_D30], [Network_NoCustomers_H7], [Network_ErrorMargin_J7], [Network_ErrorMargin_J10], [Network_ErrorMargin_J18], [Network_ErrorMargin_J32], [Network_ErrorMargin_D35], [Prs_Area_B7], [Prs_Area_B8], [Prs_Area_B9], [Prs_Area_B10], [Prs_ApproxNoOfConn_D7], [Prs_DailyAvgPrsM_F7], [Prs_ApproxNoOfConn_D8], [Prs_DailyAvgPrsM_F8], [Prs_ApproxNoOfConn_D9], [Prs_DailyAvgPrsM_F9], [Prs_ApproxNoOfConn_D10], [Prs_DailyAvgPrsM_F10], [Prs_ErrorMarg_F26], [Interm_Conn_D7], [Interm_Conn_D8], [Interm_Conn_D9], [Interm_Conn_D10], [Interm_Days_F7], [Interm_Days_F8], [Interm_Days_F9], [Interm_Days_F10], [Interm_Hour_H7], [Interm_Hour_H8], [Interm_Hour_H9], [Interm_Hour_H10], [Interm_ErrorMarg_H26], [FinancData_G6], [FinancData_K6], [FinancData_G8], [FinancData_D26], [FinancData_G35], [MatrixOneIn_SelectedOption], [MatrixOneIn_C11], [MatrixOneIn_C12], [MatrixOneIn_C13], [MatrixOneIn_C14], [MatrixOneIn_C21], [MatrixOneIn_C22], [MatrixOneIn_C23], [MatrixOneIn_C24], [MatrixOneIn_D21], [MatrixOneIn_D22], [MatrixOneIn_D23], [MatrixOneIn_D24], [MatrixOneIn_E11], [MatrixOneIn_E12], [MatrixOneIn_E13], [MatrixOneIn_E14], [MatrixOneIn_E21], [MatrixOneIn_E22], [MatrixOneIn_E23], [MatrixOneIn_E24], [MatrixOneIn_F11], [MatrixOneIn_F12], [MatrixOneIn_F13], [MatrixOneIn_F14], [MatrixOneIn_F21], [MatrixOneIn_F22], [MatrixOneIn_F23], [MatrixOneIn_F24], [MatrixOneIn_G11], [MatrixOneIn_G12], [MatrixOneIn_G13], [MatrixOneIn_G14], [MatrixOneIn_G21], [MatrixOneIn_G22], [MatrixOneIn_G23], [MatrixOneIn_G24], [MatrixOneIn_H11], [MatrixOneIn_H12], [MatrixOneIn_H13], [MatrixOneIn_H14], [MatrixOneIn_H21], [MatrixOneIn_H22], [MatrixOneIn_H23], [MatrixOneIn_H24], [MatrixTwoIn_SelectedOption], [MatrixTwoIn_D21], [MatrixTwoIn_D22], [MatrixTwoIn_D23], [MatrixTwoIn_D24], [MatrixTwoIn_E11], [MatrixTwoIn_E12], [MatrixTwoIn_E13], [MatrixTwoIn_E14], [MatrixTwoIn_E21], [MatrixTwoIn_E22], [MatrixTwoIn_E23], [MatrixTwoIn_E24], [MatrixTwoIn_F11], [MatrixTwoIn_F12], [MatrixTwoIn_F13], [MatrixTwoIn_F14], [MatrixTwoIn_F21], [MatrixTwoIn_F22], [MatrixTwoIn_F23], [MatrixTwoIn_F24], [MatrixTwoIn_G11], [MatrixTwoIn_G12], [MatrixTwoIn_G13], [MatrixTwoIn_G14], [MatrixTwoIn_G21], [MatrixTwoIn_G22], [MatrixTwoIn_G23], [MatrixTwoIn_G24], [MatrixTwoIn_H11], [MatrixTwoIn_H12], [MatrixTwoIn_H13], [MatrixTwoIn_H14], [MatrixTwoIn_H21], [MatrixTwoIn_H22], [MatrixTwoIn_H23], [MatrixTwoIn_H24], [SystemInputVolume_B19], [SystemInputVolumeErrorMargin_B21], [AuthorizedConsumption_K12], [AuthorizedConsumptionErrorMargin_K15], [WaterLosses_K29], [WaterLossesErrorMargin_K31], [BilledAuthorizedConsumption_T8], [UnbilledAuthorizedConsumption_T16], [UnbilledAuthorizedConsumptionErrorMargin_T20], [CommercialLosses_T26], [CommercialLossesErrorMargin_T29], [PhysicalLossesM3_T34], [PhyscialLossesErrorMargin_AH35], [BilledMeteredConsumption_AC4], [BilledUnmeteredConsumption_AC9], [UnbilledMeteredConsumption_AC14], [UnbilledUnmeteredConsumption_AC19], [UnbilledUnmeteredConsumptionErrorMargin_AO20], [UnauthorizedConsumption_AC24], [UnauthorizedConsumptionErrorMargin_AO25], [CustomerMeterInaccuraciesAndErrorsM3_AC29], [CustomerMeterInaccuraciesAndErrorsErrorMargin_AO30], [RevenueWaterM3_AY8], [NonRevenueWaterM3_AY24], [NonRevenueWaterErrorMargin_AY26]) VALUES (1092, N'EMEA\79749', CAST(N'2021-02-14T11:08:03.020' AS DateTime), N'EMEA\79749', CAST(N'2021-03-12T12:27:27.633' AS DateTime), 6773, 2019, 1, N'Old - 4 vvvvvvvvvvvvvv', 0, 0, 30, N'aaaaaa', N'dddddd', N'xxxxxxxxxx', N'nnnnnnnnn', 28000, 0.01, 0, 0, 0, 0, 0, 0, N'qq', N'ww', N'ee', N'rrr', N'tt', N'yy', N'uu', N'ii', N'', N'', N'', N'', N'gg', N'hh', N'jj', N'kk', N'll', N'zz', N'zz', N'xx', N'cc', N'vv', N'qqqq', N'www', N'eeee', N'rrrrrr', N'tt', N'yy', N'uu', N'iii', N'uuuuu', N'iiiiiiiii', N'ooooooooop', N'ppppppp', 500, 250, 14000, 220, 0, 0, 75, 0, 0, 0, 150, 300, 0, 0, 0, 100, 0, 0, 0, 0, 0, 0.05, 0, 0, 0, 0, 0, 20, 3, 120, 0.2, 0.1, 5, 500, 30, 0.2, 160, 0.4, 0, 0, 0, 3, 0, 0, 0, 1, 0.02, 0.05, 10000, 2000, 1000, 0, 0.03, 0.04, 0.05, 0, 0.1, 0.1, 0.1, 0, 0.05, 0.1, 0.02, 50, 0.02, 0.02, 0.05, 0.03, 24.2, 0, 0, 0, 700, 15, 10, 0.01, 758, 0, 0.01, 0.05, 0.05, 0, N'adddddddddd', N'gzzzzzzzzzzzzz', N'eggggggggg', N'jjjjjjjjjjjj', 740, 37.5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, N'PLN', 1, 10, 20000, 1, 1.5, 2, 4, 8, 2, 4, 8, 16, 25, 50, 100, 200, 25, 50, 100, 200, 50, 100, 200, 400, 40, 75, 150, 300, 75, 150, 300, 600, 50, 100, 200, 400, 100, 200, 400, 800, 60, 125, 250, 500, 125, 250, 500, 1000, 2, 55, 110, 220, 400, 50, 100, 200, 350, 80, 160, 320, 600, 65, 125, 250, 450, 105, 210, 420, 800, 75, 150, 300, 550, 130, 260, 520, 1000, 85, 175, 350, 650, 155, 310, 620, 1200, 28000, 0.01, 15595, 0.00032061558191728119, 12405, 0.022575142224813296, 15045, 550, 0.00909090909090909, 1359.5681397898293, 0.048505884633143863, 11045.43186021017, 0.02604739569152642, 14720, 325, 450, 100, 0.05, 525, 0.12118732337558567, 834.56813978982927, 0.020791322514394753, 15045, 12955, 0.021613276727132383)
GO
INSERT [dbo].[tbWbEasyCalcData] ([WbEasyCalcDataId], [CreateLogin], [CreateDate], [ModifyLogin], [ModifyDate], [ZoneId], [YearNo], [MonthNo], [Description], [IsArchive], [IsAccepted], [Start_PeriodDays_M21], [SysInput_Desc_B6], [SysInput_Desc_B7], [SysInput_Desc_B8], [SysInput_Desc_B9], [SysInput_SystemInputVolumeM3_D6], [SysInput_SystemInputVolumeError_F6], [SysInput_SystemInputVolumeM3_D7], [SysInput_SystemInputVolumeError_F7], [SysInput_SystemInputVolumeM3_D8], [SysInput_SystemInputVolumeError_F8], [SysInput_SystemInputVolumeM3_D9], [SysInput_SystemInputVolumeError_F9], [BilledCons_Desc_B8], [BilledCons_Desc_B9], [BilledCons_Desc_B10], [BilledCons_Desc_B11], [BilledCons_Desc_F8], [BilledCons_Desc_F9], [BilledCons_Desc_F10], [BilledCons_Desc_F11], [UnbilledCons_Desc_D8], [UnbilledCons_Desc_D9], [UnbilledCons_Desc_D10], [UnbilledCons_Desc_D11], [UnbilledCons_Desc_F6], [UnbilledCons_Desc_F7], [UnbilledCons_Desc_F8], [UnbilledCons_Desc_F9], [UnbilledCons_Desc_F10], [UnbilledCons_Desc_F11], [UnauthCons_Desc_B18], [UnauthCons_Desc_B19], [UnauthCons_Desc_B20], [UnauthCons_Desc_B21], [MetErrors_Desc_D12], [MetErrors_Desc_D13], [MetErrors_Desc_D14], [MetErrors_Desc_D15], [Network_Desc_B7], [Network_Desc_B8], [Network_Desc_B9], [Network_Desc_B10], [Interm_Area_B7], [Interm_Area_B8], [Interm_Area_B9], [Interm_Area_B10], [BilledCons_BilledMetConsBulkWatSupExpM3_D6], [BilledCons_BilledUnmetConsBulkWatSupExpM3_H6], [BilledCons_UnbMetConsM3_D8], [BilledCons_UnbMetConsM3_D9], [BilledCons_UnbMetConsM3_D10], [BilledCons_UnbMetConsM3_D11], [BilledCons_UnbUnmetConsM3_H8], [BilledCons_UnbUnmetConsM3_H9], [BilledCons_UnbUnmetConsM3_H10], [BilledCons_UnbUnmetConsM3_H11], [UnbilledCons_MetConsBulkWatSupExpM3_D6], [UnbilledCons_UnbMetConsM3_D8], [UnbilledCons_UnbMetConsM3_D9], [UnbilledCons_UnbMetConsM3_D10], [UnbilledCons_UnbMetConsM3_D11], [UnbilledCons_UnbUnmetConsM3_H6], [UnbilledCons_UnbUnmetConsM3_H7], [UnbilledCons_UnbUnmetConsM3_H8], [UnbilledCons_UnbUnmetConsM3_H9], [UnbilledCons_UnbUnmetConsM3_H10], [UnbilledCons_UnbUnmetConsM3_H11], [UnbilledCons_UnbUnmetConsError_J6], [UnbilledCons_UnbUnmetConsError_J7], [UnbilledCons_UnbUnmetConsError_J8], [UnbilledCons_UnbUnmetConsError_J9], [UnbilledCons_UnbUnmetConsError_J10], [UnbilledCons_UnbUnmetConsError_J11], [UnauthCons_IllegalConnDomEstNo_D6], [UnauthCons_IllegalConnDomPersPerHouse_H6], [UnauthCons_IllegalConnDomConsLitPerPersDay_J6], [UnauthCons_IllegalConnDomErrorMargin_F6], [UnauthCons_IllegalConnOthersErrorMargin_F10], [IllegalConnectionsOthersEstimatedNumber_D10], [IllegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10], [UnauthCons_MeterTampBypEtcEstNo_D14], [UnauthCons_MeterTampBypEtcErrorMargin_F14], [UnauthCons_MeterTampBypEtcConsLitPerCustDay_J14], [UnauthCons_OthersErrorMargin_F18], [UnauthCons_OthersErrorMargin_F19], [UnauthCons_OthersErrorMargin_F20], [UnauthCons_OthersErrorMargin_F21], [UnauthCons_OthersM3PerDay_J18], [UnauthCons_OthersM3PerDay_J19], [UnauthCons_OthersM3PerDay_J20], [UnauthCons_OthersM3PerDay_J21], [MetErrors_DetailedManualSpec_J6], [MetErrors_BilledMetConsWoBulkSupMetUndrreg_H8], [MetErrors_BilledMetConsWoBulkSupErrorMargin_N8], [MetErrors_Total_F12], [MetErrors_Total_F13], [MetErrors_Total_F14], [MetErrors_Total_F15], [MetErrors_Meter_H12], [MetErrors_Meter_H13], [MetErrors_Meter_H14], [MetErrors_Meter_H15], [MetErrors_Error_N12], [MetErrors_Error_N13], [MetErrors_Error_N14], [MetErrors_Error_N15], [MeteredBulkSupplyExportErrorMargin_N32], [UnbilledMeteredConsumptionWithoutBulkSupplyErrorMargin_N34], [CorruptMeterReadingPracticessErrorMargin_N38], [DataHandlingErrorsOffice_L40], [DataHandlingErrorsOfficeErrorMargin_N40], [MetErrors_MetBulkSupExpMetUnderreg_H32], [MetErrors_UnbillMetConsWoBulkSupplMetUndrreg_H34], [MetErrors_CorruptMetReadPractMetUndrreg_H38], [Network_DistributionAndTransmissionMains_D7], [Network_DistributionAndTransmissionMains_D8], [Network_DistributionAndTransmissionMains_D9], [Network_DistributionAndTransmissionMains_D10], [Network_NoOfConnOfRegCustomers_H10], [Network_NoOfInactAccountsWSvcConns_H18], [Network_AvgLenOfSvcConnFromBoundaryToMeterM_H32], [Network_PossibleUnd_D30], [Network_NoCustomers_H7], [Network_ErrorMargin_J7], [Network_ErrorMargin_J10], [Network_ErrorMargin_J18], [Network_ErrorMargin_J32], [Network_ErrorMargin_D35], [Prs_Area_B7], [Prs_Area_B8], [Prs_Area_B9], [Prs_Area_B10], [Prs_ApproxNoOfConn_D7], [Prs_DailyAvgPrsM_F7], [Prs_ApproxNoOfConn_D8], [Prs_DailyAvgPrsM_F8], [Prs_ApproxNoOfConn_D9], [Prs_DailyAvgPrsM_F9], [Prs_ApproxNoOfConn_D10], [Prs_DailyAvgPrsM_F10], [Prs_ErrorMarg_F26], [Interm_Conn_D7], [Interm_Conn_D8], [Interm_Conn_D9], [Interm_Conn_D10], [Interm_Days_F7], [Interm_Days_F8], [Interm_Days_F9], [Interm_Days_F10], [Interm_Hour_H7], [Interm_Hour_H8], [Interm_Hour_H9], [Interm_Hour_H10], [Interm_ErrorMarg_H26], [FinancData_G6], [FinancData_K6], [FinancData_G8], [FinancData_D26], [FinancData_G35], [MatrixOneIn_SelectedOption], [MatrixOneIn_C11], [MatrixOneIn_C12], [MatrixOneIn_C13], [MatrixOneIn_C14], [MatrixOneIn_C21], [MatrixOneIn_C22], [MatrixOneIn_C23], [MatrixOneIn_C24], [MatrixOneIn_D21], [MatrixOneIn_D22], [MatrixOneIn_D23], [MatrixOneIn_D24], [MatrixOneIn_E11], [MatrixOneIn_E12], [MatrixOneIn_E13], [MatrixOneIn_E14], [MatrixOneIn_E21], [MatrixOneIn_E22], [MatrixOneIn_E23], [MatrixOneIn_E24], [MatrixOneIn_F11], [MatrixOneIn_F12], [MatrixOneIn_F13], [MatrixOneIn_F14], [MatrixOneIn_F21], [MatrixOneIn_F22], [MatrixOneIn_F23], [MatrixOneIn_F24], [MatrixOneIn_G11], [MatrixOneIn_G12], [MatrixOneIn_G13], [MatrixOneIn_G14], [MatrixOneIn_G21], [MatrixOneIn_G22], [MatrixOneIn_G23], [MatrixOneIn_G24], [MatrixOneIn_H11], [MatrixOneIn_H12], [MatrixOneIn_H13], [MatrixOneIn_H14], [MatrixOneIn_H21], [MatrixOneIn_H22], [MatrixOneIn_H23], [MatrixOneIn_H24], [MatrixTwoIn_SelectedOption], [MatrixTwoIn_D21], [MatrixTwoIn_D22], [MatrixTwoIn_D23], [MatrixTwoIn_D24], [MatrixTwoIn_E11], [MatrixTwoIn_E12], [MatrixTwoIn_E13], [MatrixTwoIn_E14], [MatrixTwoIn_E21], [MatrixTwoIn_E22], [MatrixTwoIn_E23], [MatrixTwoIn_E24], [MatrixTwoIn_F11], [MatrixTwoIn_F12], [MatrixTwoIn_F13], [MatrixTwoIn_F14], [MatrixTwoIn_F21], [MatrixTwoIn_F22], [MatrixTwoIn_F23], [MatrixTwoIn_F24], [MatrixTwoIn_G11], [MatrixTwoIn_G12], [MatrixTwoIn_G13], [MatrixTwoIn_G14], [MatrixTwoIn_G21], [MatrixTwoIn_G22], [MatrixTwoIn_G23], [MatrixTwoIn_G24], [MatrixTwoIn_H11], [MatrixTwoIn_H12], [MatrixTwoIn_H13], [MatrixTwoIn_H14], [MatrixTwoIn_H21], [MatrixTwoIn_H22], [MatrixTwoIn_H23], [MatrixTwoIn_H24], [SystemInputVolume_B19], [SystemInputVolumeErrorMargin_B21], [AuthorizedConsumption_K12], [AuthorizedConsumptionErrorMargin_K15], [WaterLosses_K29], [WaterLossesErrorMargin_K31], [BilledAuthorizedConsumption_T8], [UnbilledAuthorizedConsumption_T16], [UnbilledAuthorizedConsumptionErrorMargin_T20], [CommercialLosses_T26], [CommercialLossesErrorMargin_T29], [PhysicalLossesM3_T34], [PhyscialLossesErrorMargin_AH35], [BilledMeteredConsumption_AC4], [BilledUnmeteredConsumption_AC9], [UnbilledMeteredConsumption_AC14], [UnbilledUnmeteredConsumption_AC19], [UnbilledUnmeteredConsumptionErrorMargin_AO20], [UnauthorizedConsumption_AC24], [UnauthorizedConsumptionErrorMargin_AO25], [CustomerMeterInaccuraciesAndErrorsM3_AC29], [CustomerMeterInaccuraciesAndErrorsErrorMargin_AO30], [RevenueWaterM3_AY8], [NonRevenueWaterM3_AY24], [NonRevenueWaterErrorMargin_AY26]) VALUES (1098, N'EMEA\79749', CAST(N'2021-03-25T20:43:55.260' AS DateTime), N'EMEA\79749', CAST(N'2021-03-30T13:55:32.707' AS DateTime), 6773, 2020, 1, N'', 0, 0, 31, NULL, NULL, NULL, NULL, 0, 0, 0, 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 691, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, N'PLN', 0, 0, 0, 1, 1.5, 2, 4, 8, 2, 4, 8, 16, 25, 50, 100, 200, 25, 50, 100, 200, 50, 100, 200, 400, 40, 75, 150, 300, 75, 150, 300, 600, 50, 100, 200, 400, 100, 200, 400, 800, 60, 125, 250, 500, 125, 250, 500, 1000, 2, 55, 110, 220, 400, 50, 100, 200, 350, 80, 160, 320, 600, 65, 125, 250, 450, 105, 210, 420, 800, 75, 150, 300, 550, 130, 260, 520, 1000, 85, 175, 350, 650, 155, 310, 620, 1200, 0, 0, 691, 0, -691, 0, 691, 0, 0, 0, 0, -691, 0, 691, 0, 0, 0, 0, 0, 0, 0, 0, 691, -691, 0)
GO
INSERT [dbo].[tbWbEasyCalcData] ([WbEasyCalcDataId], [CreateLogin], [CreateDate], [ModifyLogin], [ModifyDate], [ZoneId], [YearNo], [MonthNo], [Description], [IsArchive], [IsAccepted], [Start_PeriodDays_M21], [SysInput_Desc_B6], [SysInput_Desc_B7], [SysInput_Desc_B8], [SysInput_Desc_B9], [SysInput_SystemInputVolumeM3_D6], [SysInput_SystemInputVolumeError_F6], [SysInput_SystemInputVolumeM3_D7], [SysInput_SystemInputVolumeError_F7], [SysInput_SystemInputVolumeM3_D8], [SysInput_SystemInputVolumeError_F8], [SysInput_SystemInputVolumeM3_D9], [SysInput_SystemInputVolumeError_F9], [BilledCons_Desc_B8], [BilledCons_Desc_B9], [BilledCons_Desc_B10], [BilledCons_Desc_B11], [BilledCons_Desc_F8], [BilledCons_Desc_F9], [BilledCons_Desc_F10], [BilledCons_Desc_F11], [UnbilledCons_Desc_D8], [UnbilledCons_Desc_D9], [UnbilledCons_Desc_D10], [UnbilledCons_Desc_D11], [UnbilledCons_Desc_F6], [UnbilledCons_Desc_F7], [UnbilledCons_Desc_F8], [UnbilledCons_Desc_F9], [UnbilledCons_Desc_F10], [UnbilledCons_Desc_F11], [UnauthCons_Desc_B18], [UnauthCons_Desc_B19], [UnauthCons_Desc_B20], [UnauthCons_Desc_B21], [MetErrors_Desc_D12], [MetErrors_Desc_D13], [MetErrors_Desc_D14], [MetErrors_Desc_D15], [Network_Desc_B7], [Network_Desc_B8], [Network_Desc_B9], [Network_Desc_B10], [Interm_Area_B7], [Interm_Area_B8], [Interm_Area_B9], [Interm_Area_B10], [BilledCons_BilledMetConsBulkWatSupExpM3_D6], [BilledCons_BilledUnmetConsBulkWatSupExpM3_H6], [BilledCons_UnbMetConsM3_D8], [BilledCons_UnbMetConsM3_D9], [BilledCons_UnbMetConsM3_D10], [BilledCons_UnbMetConsM3_D11], [BilledCons_UnbUnmetConsM3_H8], [BilledCons_UnbUnmetConsM3_H9], [BilledCons_UnbUnmetConsM3_H10], [BilledCons_UnbUnmetConsM3_H11], [UnbilledCons_MetConsBulkWatSupExpM3_D6], [UnbilledCons_UnbMetConsM3_D8], [UnbilledCons_UnbMetConsM3_D9], [UnbilledCons_UnbMetConsM3_D10], [UnbilledCons_UnbMetConsM3_D11], [UnbilledCons_UnbUnmetConsM3_H6], [UnbilledCons_UnbUnmetConsM3_H7], [UnbilledCons_UnbUnmetConsM3_H8], [UnbilledCons_UnbUnmetConsM3_H9], [UnbilledCons_UnbUnmetConsM3_H10], [UnbilledCons_UnbUnmetConsM3_H11], [UnbilledCons_UnbUnmetConsError_J6], [UnbilledCons_UnbUnmetConsError_J7], [UnbilledCons_UnbUnmetConsError_J8], [UnbilledCons_UnbUnmetConsError_J9], [UnbilledCons_UnbUnmetConsError_J10], [UnbilledCons_UnbUnmetConsError_J11], [UnauthCons_IllegalConnDomEstNo_D6], [UnauthCons_IllegalConnDomPersPerHouse_H6], [UnauthCons_IllegalConnDomConsLitPerPersDay_J6], [UnauthCons_IllegalConnDomErrorMargin_F6], [UnauthCons_IllegalConnOthersErrorMargin_F10], [IllegalConnectionsOthersEstimatedNumber_D10], [IllegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10], [UnauthCons_MeterTampBypEtcEstNo_D14], [UnauthCons_MeterTampBypEtcErrorMargin_F14], [UnauthCons_MeterTampBypEtcConsLitPerCustDay_J14], [UnauthCons_OthersErrorMargin_F18], [UnauthCons_OthersErrorMargin_F19], [UnauthCons_OthersErrorMargin_F20], [UnauthCons_OthersErrorMargin_F21], [UnauthCons_OthersM3PerDay_J18], [UnauthCons_OthersM3PerDay_J19], [UnauthCons_OthersM3PerDay_J20], [UnauthCons_OthersM3PerDay_J21], [MetErrors_DetailedManualSpec_J6], [MetErrors_BilledMetConsWoBulkSupMetUndrreg_H8], [MetErrors_BilledMetConsWoBulkSupErrorMargin_N8], [MetErrors_Total_F12], [MetErrors_Total_F13], [MetErrors_Total_F14], [MetErrors_Total_F15], [MetErrors_Meter_H12], [MetErrors_Meter_H13], [MetErrors_Meter_H14], [MetErrors_Meter_H15], [MetErrors_Error_N12], [MetErrors_Error_N13], [MetErrors_Error_N14], [MetErrors_Error_N15], [MeteredBulkSupplyExportErrorMargin_N32], [UnbilledMeteredConsumptionWithoutBulkSupplyErrorMargin_N34], [CorruptMeterReadingPracticessErrorMargin_N38], [DataHandlingErrorsOffice_L40], [DataHandlingErrorsOfficeErrorMargin_N40], [MetErrors_MetBulkSupExpMetUnderreg_H32], [MetErrors_UnbillMetConsWoBulkSupplMetUndrreg_H34], [MetErrors_CorruptMetReadPractMetUndrreg_H38], [Network_DistributionAndTransmissionMains_D7], [Network_DistributionAndTransmissionMains_D8], [Network_DistributionAndTransmissionMains_D9], [Network_DistributionAndTransmissionMains_D10], [Network_NoOfConnOfRegCustomers_H10], [Network_NoOfInactAccountsWSvcConns_H18], [Network_AvgLenOfSvcConnFromBoundaryToMeterM_H32], [Network_PossibleUnd_D30], [Network_NoCustomers_H7], [Network_ErrorMargin_J7], [Network_ErrorMargin_J10], [Network_ErrorMargin_J18], [Network_ErrorMargin_J32], [Network_ErrorMargin_D35], [Prs_Area_B7], [Prs_Area_B8], [Prs_Area_B9], [Prs_Area_B10], [Prs_ApproxNoOfConn_D7], [Prs_DailyAvgPrsM_F7], [Prs_ApproxNoOfConn_D8], [Prs_DailyAvgPrsM_F8], [Prs_ApproxNoOfConn_D9], [Prs_DailyAvgPrsM_F9], [Prs_ApproxNoOfConn_D10], [Prs_DailyAvgPrsM_F10], [Prs_ErrorMarg_F26], [Interm_Conn_D7], [Interm_Conn_D8], [Interm_Conn_D9], [Interm_Conn_D10], [Interm_Days_F7], [Interm_Days_F8], [Interm_Days_F9], [Interm_Days_F10], [Interm_Hour_H7], [Interm_Hour_H8], [Interm_Hour_H9], [Interm_Hour_H10], [Interm_ErrorMarg_H26], [FinancData_G6], [FinancData_K6], [FinancData_G8], [FinancData_D26], [FinancData_G35], [MatrixOneIn_SelectedOption], [MatrixOneIn_C11], [MatrixOneIn_C12], [MatrixOneIn_C13], [MatrixOneIn_C14], [MatrixOneIn_C21], [MatrixOneIn_C22], [MatrixOneIn_C23], [MatrixOneIn_C24], [MatrixOneIn_D21], [MatrixOneIn_D22], [MatrixOneIn_D23], [MatrixOneIn_D24], [MatrixOneIn_E11], [MatrixOneIn_E12], [MatrixOneIn_E13], [MatrixOneIn_E14], [MatrixOneIn_E21], [MatrixOneIn_E22], [MatrixOneIn_E23], [MatrixOneIn_E24], [MatrixOneIn_F11], [MatrixOneIn_F12], [MatrixOneIn_F13], [MatrixOneIn_F14], [MatrixOneIn_F21], [MatrixOneIn_F22], [MatrixOneIn_F23], [MatrixOneIn_F24], [MatrixOneIn_G11], [MatrixOneIn_G12], [MatrixOneIn_G13], [MatrixOneIn_G14], [MatrixOneIn_G21], [MatrixOneIn_G22], [MatrixOneIn_G23], [MatrixOneIn_G24], [MatrixOneIn_H11], [MatrixOneIn_H12], [MatrixOneIn_H13], [MatrixOneIn_H14], [MatrixOneIn_H21], [MatrixOneIn_H22], [MatrixOneIn_H23], [MatrixOneIn_H24], [MatrixTwoIn_SelectedOption], [MatrixTwoIn_D21], [MatrixTwoIn_D22], [MatrixTwoIn_D23], [MatrixTwoIn_D24], [MatrixTwoIn_E11], [MatrixTwoIn_E12], [MatrixTwoIn_E13], [MatrixTwoIn_E14], [MatrixTwoIn_E21], [MatrixTwoIn_E22], [MatrixTwoIn_E23], [MatrixTwoIn_E24], [MatrixTwoIn_F11], [MatrixTwoIn_F12], [MatrixTwoIn_F13], [MatrixTwoIn_F14], [MatrixTwoIn_F21], [MatrixTwoIn_F22], [MatrixTwoIn_F23], [MatrixTwoIn_F24], [MatrixTwoIn_G11], [MatrixTwoIn_G12], [MatrixTwoIn_G13], [MatrixTwoIn_G14], [MatrixTwoIn_G21], [MatrixTwoIn_G22], [MatrixTwoIn_G23], [MatrixTwoIn_G24], [MatrixTwoIn_H11], [MatrixTwoIn_H12], [MatrixTwoIn_H13], [MatrixTwoIn_H14], [MatrixTwoIn_H21], [MatrixTwoIn_H22], [MatrixTwoIn_H23], [MatrixTwoIn_H24], [SystemInputVolume_B19], [SystemInputVolumeErrorMargin_B21], [AuthorizedConsumption_K12], [AuthorizedConsumptionErrorMargin_K15], [WaterLosses_K29], [WaterLossesErrorMargin_K31], [BilledAuthorizedConsumption_T8], [UnbilledAuthorizedConsumption_T16], [UnbilledAuthorizedConsumptionErrorMargin_T20], [CommercialLosses_T26], [CommercialLossesErrorMargin_T29], [PhysicalLossesM3_T34], [PhyscialLossesErrorMargin_AH35], [BilledMeteredConsumption_AC4], [BilledUnmeteredConsumption_AC9], [UnbilledMeteredConsumption_AC14], [UnbilledUnmeteredConsumption_AC19], [UnbilledUnmeteredConsumptionErrorMargin_AO20], [UnauthorizedConsumption_AC24], [UnauthorizedConsumptionErrorMargin_AO25], [CustomerMeterInaccuraciesAndErrorsM3_AC29], [CustomerMeterInaccuraciesAndErrorsErrorMargin_AO30], [RevenueWaterM3_AY8], [NonRevenueWaterM3_AY24], [NonRevenueWaterErrorMargin_AY26]) VALUES (1099, N'EMEA\79749', CAST(N'2021-04-29T10:27:08.877' AS DateTime), N'EMEA\79749', CAST(N'2021-04-29T12:19:11.450' AS DateTime), 6773, 2019, 1, N'Old - 4 vvvvvvvvvvvvvv', 0, 0, 30, N'aaaaaa', N'dddddd', N'xxxxxxxxxx', N'nnnnnnnnn', 28000, 0.01, 232, 0, 0, 0, 0, 0, N'qq', N'ww', N'ee', N'rrr', N'tt', N'yy', N'uu', N'ii', N'', N'', N'', N'', N'gg', N'hh', N'jj', N'kk', N'll', N'zz', N'zz', N'xx', N'cc', N'vv', N'qqqq', N'www', N'eeee', N'rrrrrr', N'tt', N'yy', N'uu', N'iii', N'uuuuu', N'iiiiiiiii', N'ooooooooop', N'ppppppp', 500, 250, 14000, 220, 0, 0, 75, 0, 0, 0, 150, 300, 0, 0, 0, 100, 0, 0, 0, 0, 0, 0.05, 0, 0, 0, 0, 0, 20, 3, 120, 0.2, 0.1, 5, 500, 30, 0.2, 160, 0.4, 0, 0, 0, 3, 0, 0, 0, 1, 0.02, 0.05, 10000, 2000, 1000, 0, 0.03, 0.04, 0.05, 0, 0.1, 0.1, 0.1, 0, 0.05, 0.1, 0.02, 50, 0.02, 0.02, 0.05, 0.03, 24.2, 0, 0, 0, 700, 15, 10, 0.01, 758, 0, 0.01, 0.05, 0.05, 0, N'adddddddddd', N'gzzzzzzzzzzzzz', N'eggggggggg', N'jjjjjjjjjjjj', 740, 37.5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, N'PLN', 1, 10, 20000, 1, 1.5, 2, 4, 8, 2, 4, 8, 16, 25, 50, 100, 200, 25, 50, 100, 200, 50, 100, 200, 400, 40, 75, 150, 300, 75, 150, 300, 600, 50, 100, 200, 400, 100, 200, 400, 800, 60, 125, 250, 500, 125, 250, 500, 1000, 2, 55, 110, 220, 400, 50, 100, 200, 350, 80, 160, 320, 600, 65, 125, 250, 450, 105, 210, 420, 800, 75, 150, 300, 550, 130, 260, 520, 1000, 85, 175, 350, 650, 155, 310, 620, 1200, 28232, 0.0099178237461037128, 15595, 0.00032061558191728119, 12637, 0.022160689981705225, 15045, 550, 0.00909090909090909, 1359.5681397898293, 0.048505884633143863, 11277.43186021017, 0.025511547115774396, 14720, 325, 450, 100, 0.05, 525, 0.12118732337558567, 834.56813978982927, 0.020791322514394753, 15045, 13187, 0.02123303253203913)
GO
INSERT [dbo].[tbWbEasyCalcData] ([WbEasyCalcDataId], [CreateLogin], [CreateDate], [ModifyLogin], [ModifyDate], [ZoneId], [YearNo], [MonthNo], [Description], [IsArchive], [IsAccepted], [Start_PeriodDays_M21], [SysInput_Desc_B6], [SysInput_Desc_B7], [SysInput_Desc_B8], [SysInput_Desc_B9], [SysInput_SystemInputVolumeM3_D6], [SysInput_SystemInputVolumeError_F6], [SysInput_SystemInputVolumeM3_D7], [SysInput_SystemInputVolumeError_F7], [SysInput_SystemInputVolumeM3_D8], [SysInput_SystemInputVolumeError_F8], [SysInput_SystemInputVolumeM3_D9], [SysInput_SystemInputVolumeError_F9], [BilledCons_Desc_B8], [BilledCons_Desc_B9], [BilledCons_Desc_B10], [BilledCons_Desc_B11], [BilledCons_Desc_F8], [BilledCons_Desc_F9], [BilledCons_Desc_F10], [BilledCons_Desc_F11], [UnbilledCons_Desc_D8], [UnbilledCons_Desc_D9], [UnbilledCons_Desc_D10], [UnbilledCons_Desc_D11], [UnbilledCons_Desc_F6], [UnbilledCons_Desc_F7], [UnbilledCons_Desc_F8], [UnbilledCons_Desc_F9], [UnbilledCons_Desc_F10], [UnbilledCons_Desc_F11], [UnauthCons_Desc_B18], [UnauthCons_Desc_B19], [UnauthCons_Desc_B20], [UnauthCons_Desc_B21], [MetErrors_Desc_D12], [MetErrors_Desc_D13], [MetErrors_Desc_D14], [MetErrors_Desc_D15], [Network_Desc_B7], [Network_Desc_B8], [Network_Desc_B9], [Network_Desc_B10], [Interm_Area_B7], [Interm_Area_B8], [Interm_Area_B9], [Interm_Area_B10], [BilledCons_BilledMetConsBulkWatSupExpM3_D6], [BilledCons_BilledUnmetConsBulkWatSupExpM3_H6], [BilledCons_UnbMetConsM3_D8], [BilledCons_UnbMetConsM3_D9], [BilledCons_UnbMetConsM3_D10], [BilledCons_UnbMetConsM3_D11], [BilledCons_UnbUnmetConsM3_H8], [BilledCons_UnbUnmetConsM3_H9], [BilledCons_UnbUnmetConsM3_H10], [BilledCons_UnbUnmetConsM3_H11], [UnbilledCons_MetConsBulkWatSupExpM3_D6], [UnbilledCons_UnbMetConsM3_D8], [UnbilledCons_UnbMetConsM3_D9], [UnbilledCons_UnbMetConsM3_D10], [UnbilledCons_UnbMetConsM3_D11], [UnbilledCons_UnbUnmetConsM3_H6], [UnbilledCons_UnbUnmetConsM3_H7], [UnbilledCons_UnbUnmetConsM3_H8], [UnbilledCons_UnbUnmetConsM3_H9], [UnbilledCons_UnbUnmetConsM3_H10], [UnbilledCons_UnbUnmetConsM3_H11], [UnbilledCons_UnbUnmetConsError_J6], [UnbilledCons_UnbUnmetConsError_J7], [UnbilledCons_UnbUnmetConsError_J8], [UnbilledCons_UnbUnmetConsError_J9], [UnbilledCons_UnbUnmetConsError_J10], [UnbilledCons_UnbUnmetConsError_J11], [UnauthCons_IllegalConnDomEstNo_D6], [UnauthCons_IllegalConnDomPersPerHouse_H6], [UnauthCons_IllegalConnDomConsLitPerPersDay_J6], [UnauthCons_IllegalConnDomErrorMargin_F6], [UnauthCons_IllegalConnOthersErrorMargin_F10], [IllegalConnectionsOthersEstimatedNumber_D10], [IllegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10], [UnauthCons_MeterTampBypEtcEstNo_D14], [UnauthCons_MeterTampBypEtcErrorMargin_F14], [UnauthCons_MeterTampBypEtcConsLitPerCustDay_J14], [UnauthCons_OthersErrorMargin_F18], [UnauthCons_OthersErrorMargin_F19], [UnauthCons_OthersErrorMargin_F20], [UnauthCons_OthersErrorMargin_F21], [UnauthCons_OthersM3PerDay_J18], [UnauthCons_OthersM3PerDay_J19], [UnauthCons_OthersM3PerDay_J20], [UnauthCons_OthersM3PerDay_J21], [MetErrors_DetailedManualSpec_J6], [MetErrors_BilledMetConsWoBulkSupMetUndrreg_H8], [MetErrors_BilledMetConsWoBulkSupErrorMargin_N8], [MetErrors_Total_F12], [MetErrors_Total_F13], [MetErrors_Total_F14], [MetErrors_Total_F15], [MetErrors_Meter_H12], [MetErrors_Meter_H13], [MetErrors_Meter_H14], [MetErrors_Meter_H15], [MetErrors_Error_N12], [MetErrors_Error_N13], [MetErrors_Error_N14], [MetErrors_Error_N15], [MeteredBulkSupplyExportErrorMargin_N32], [UnbilledMeteredConsumptionWithoutBulkSupplyErrorMargin_N34], [CorruptMeterReadingPracticessErrorMargin_N38], [DataHandlingErrorsOffice_L40], [DataHandlingErrorsOfficeErrorMargin_N40], [MetErrors_MetBulkSupExpMetUnderreg_H32], [MetErrors_UnbillMetConsWoBulkSupplMetUndrreg_H34], [MetErrors_CorruptMetReadPractMetUndrreg_H38], [Network_DistributionAndTransmissionMains_D7], [Network_DistributionAndTransmissionMains_D8], [Network_DistributionAndTransmissionMains_D9], [Network_DistributionAndTransmissionMains_D10], [Network_NoOfConnOfRegCustomers_H10], [Network_NoOfInactAccountsWSvcConns_H18], [Network_AvgLenOfSvcConnFromBoundaryToMeterM_H32], [Network_PossibleUnd_D30], [Network_NoCustomers_H7], [Network_ErrorMargin_J7], [Network_ErrorMargin_J10], [Network_ErrorMargin_J18], [Network_ErrorMargin_J32], [Network_ErrorMargin_D35], [Prs_Area_B7], [Prs_Area_B8], [Prs_Area_B9], [Prs_Area_B10], [Prs_ApproxNoOfConn_D7], [Prs_DailyAvgPrsM_F7], [Prs_ApproxNoOfConn_D8], [Prs_DailyAvgPrsM_F8], [Prs_ApproxNoOfConn_D9], [Prs_DailyAvgPrsM_F9], [Prs_ApproxNoOfConn_D10], [Prs_DailyAvgPrsM_F10], [Prs_ErrorMarg_F26], [Interm_Conn_D7], [Interm_Conn_D8], [Interm_Conn_D9], [Interm_Conn_D10], [Interm_Days_F7], [Interm_Days_F8], [Interm_Days_F9], [Interm_Days_F10], [Interm_Hour_H7], [Interm_Hour_H8], [Interm_Hour_H9], [Interm_Hour_H10], [Interm_ErrorMarg_H26], [FinancData_G6], [FinancData_K6], [FinancData_G8], [FinancData_D26], [FinancData_G35], [MatrixOneIn_SelectedOption], [MatrixOneIn_C11], [MatrixOneIn_C12], [MatrixOneIn_C13], [MatrixOneIn_C14], [MatrixOneIn_C21], [MatrixOneIn_C22], [MatrixOneIn_C23], [MatrixOneIn_C24], [MatrixOneIn_D21], [MatrixOneIn_D22], [MatrixOneIn_D23], [MatrixOneIn_D24], [MatrixOneIn_E11], [MatrixOneIn_E12], [MatrixOneIn_E13], [MatrixOneIn_E14], [MatrixOneIn_E21], [MatrixOneIn_E22], [MatrixOneIn_E23], [MatrixOneIn_E24], [MatrixOneIn_F11], [MatrixOneIn_F12], [MatrixOneIn_F13], [MatrixOneIn_F14], [MatrixOneIn_F21], [MatrixOneIn_F22], [MatrixOneIn_F23], [MatrixOneIn_F24], [MatrixOneIn_G11], [MatrixOneIn_G12], [MatrixOneIn_G13], [MatrixOneIn_G14], [MatrixOneIn_G21], [MatrixOneIn_G22], [MatrixOneIn_G23], [MatrixOneIn_G24], [MatrixOneIn_H11], [MatrixOneIn_H12], [MatrixOneIn_H13], [MatrixOneIn_H14], [MatrixOneIn_H21], [MatrixOneIn_H22], [MatrixOneIn_H23], [MatrixOneIn_H24], [MatrixTwoIn_SelectedOption], [MatrixTwoIn_D21], [MatrixTwoIn_D22], [MatrixTwoIn_D23], [MatrixTwoIn_D24], [MatrixTwoIn_E11], [MatrixTwoIn_E12], [MatrixTwoIn_E13], [MatrixTwoIn_E14], [MatrixTwoIn_E21], [MatrixTwoIn_E22], [MatrixTwoIn_E23], [MatrixTwoIn_E24], [MatrixTwoIn_F11], [MatrixTwoIn_F12], [MatrixTwoIn_F13], [MatrixTwoIn_F14], [MatrixTwoIn_F21], [MatrixTwoIn_F22], [MatrixTwoIn_F23], [MatrixTwoIn_F24], [MatrixTwoIn_G11], [MatrixTwoIn_G12], [MatrixTwoIn_G13], [MatrixTwoIn_G14], [MatrixTwoIn_G21], [MatrixTwoIn_G22], [MatrixTwoIn_G23], [MatrixTwoIn_G24], [MatrixTwoIn_H11], [MatrixTwoIn_H12], [MatrixTwoIn_H13], [MatrixTwoIn_H14], [MatrixTwoIn_H21], [MatrixTwoIn_H22], [MatrixTwoIn_H23], [MatrixTwoIn_H24], [SystemInputVolume_B19], [SystemInputVolumeErrorMargin_B21], [AuthorizedConsumption_K12], [AuthorizedConsumptionErrorMargin_K15], [WaterLosses_K29], [WaterLossesErrorMargin_K31], [BilledAuthorizedConsumption_T8], [UnbilledAuthorizedConsumption_T16], [UnbilledAuthorizedConsumptionErrorMargin_T20], [CommercialLosses_T26], [CommercialLossesErrorMargin_T29], [PhysicalLossesM3_T34], [PhyscialLossesErrorMargin_AH35], [BilledMeteredConsumption_AC4], [BilledUnmeteredConsumption_AC9], [UnbilledMeteredConsumption_AC14], [UnbilledUnmeteredConsumption_AC19], [UnbilledUnmeteredConsumptionErrorMargin_AO20], [UnauthorizedConsumption_AC24], [UnauthorizedConsumptionErrorMargin_AO25], [CustomerMeterInaccuraciesAndErrorsM3_AC29], [CustomerMeterInaccuraciesAndErrorsErrorMargin_AO30], [RevenueWaterM3_AY8], [NonRevenueWaterM3_AY24], [NonRevenueWaterErrorMargin_AY26]) VALUES (1100, N'EMEA\79749', CAST(N'2021-04-29T13:52:14.727' AS DateTime), N'EMEA\79749', CAST(N'2021-05-18T14:12:12.543' AS DateTime), 6773, 2020, 4, N'4.1', 0, 0, 30, NULL, NULL, NULL, NULL, 0, 0.05, 0, 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 5332026, 1000, 52572, 0, 0, 0, 2000, 0, 0, 0, 309349, 1111, 0, 0, 0, 3242, 0, 0, 0, 0, 0, 0.05, 0, 0, 0, 0, 0, 100, 3, 120, 0.05, 0.05, 500, 200, 1000, 0.1, 160, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0.03, 0.02, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0.02, 0.02, 0.02, 100, 0.02, 0.03, 0.03, 0.03, 19539.125613145392, 0, 0, 0, 362, 91, 7, 0, 758, 0.01, 0, 0, 0.02, 0, NULL, NULL, NULL, NULL, 9500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, N'PLN', 1, 0, 0, 1, 1.5, 2, 4, 8, 2, 4, 8, 16, 25, 50, 100, 200, 25, 50, 100, 200, 50, 100, 200, 400, 40, 75, 150, 300, 75, 150, 300, 600, 50, 100, 200, 400, 100, 200, 400, 800, 60, 125, 250, 500, 125, 250, 500, 1000, 4, 55, 110, 220, 400, 50, 100, 200, 350, 80, 160, 320, 600, 65, 125, 250, 450, 105, 210, 420, 800, 75, 150, 300, 550, 130, 260, 520, 1000, 85, 175, 350, 650, 155, 310, 620, 1200, 0, 0, 5701300, 2.8432111974461968E-05, -5701300, -2.8432111974461968E-05, 5387598, 313702, 0.00051673244034147064, 351742.5567010314, 0.013793430932532911, -6053042.5567010315, -0.0008019840930801348, 5384598, 3000, 310460, 3242, 0.05000000000000001, 8880, 0.056957495718128637, 342862.5567010314, 0.01407357323145182, 5387598, -5387598, 0)
GO
INSERT [dbo].[tbWbEasyCalcData] ([WbEasyCalcDataId], [CreateLogin], [CreateDate], [ModifyLogin], [ModifyDate], [ZoneId], [YearNo], [MonthNo], [Description], [IsArchive], [IsAccepted], [Start_PeriodDays_M21], [SysInput_Desc_B6], [SysInput_Desc_B7], [SysInput_Desc_B8], [SysInput_Desc_B9], [SysInput_SystemInputVolumeM3_D6], [SysInput_SystemInputVolumeError_F6], [SysInput_SystemInputVolumeM3_D7], [SysInput_SystemInputVolumeError_F7], [SysInput_SystemInputVolumeM3_D8], [SysInput_SystemInputVolumeError_F8], [SysInput_SystemInputVolumeM3_D9], [SysInput_SystemInputVolumeError_F9], [BilledCons_Desc_B8], [BilledCons_Desc_B9], [BilledCons_Desc_B10], [BilledCons_Desc_B11], [BilledCons_Desc_F8], [BilledCons_Desc_F9], [BilledCons_Desc_F10], [BilledCons_Desc_F11], [UnbilledCons_Desc_D8], [UnbilledCons_Desc_D9], [UnbilledCons_Desc_D10], [UnbilledCons_Desc_D11], [UnbilledCons_Desc_F6], [UnbilledCons_Desc_F7], [UnbilledCons_Desc_F8], [UnbilledCons_Desc_F9], [UnbilledCons_Desc_F10], [UnbilledCons_Desc_F11], [UnauthCons_Desc_B18], [UnauthCons_Desc_B19], [UnauthCons_Desc_B20], [UnauthCons_Desc_B21], [MetErrors_Desc_D12], [MetErrors_Desc_D13], [MetErrors_Desc_D14], [MetErrors_Desc_D15], [Network_Desc_B7], [Network_Desc_B8], [Network_Desc_B9], [Network_Desc_B10], [Interm_Area_B7], [Interm_Area_B8], [Interm_Area_B9], [Interm_Area_B10], [BilledCons_BilledMetConsBulkWatSupExpM3_D6], [BilledCons_BilledUnmetConsBulkWatSupExpM3_H6], [BilledCons_UnbMetConsM3_D8], [BilledCons_UnbMetConsM3_D9], [BilledCons_UnbMetConsM3_D10], [BilledCons_UnbMetConsM3_D11], [BilledCons_UnbUnmetConsM3_H8], [BilledCons_UnbUnmetConsM3_H9], [BilledCons_UnbUnmetConsM3_H10], [BilledCons_UnbUnmetConsM3_H11], [UnbilledCons_MetConsBulkWatSupExpM3_D6], [UnbilledCons_UnbMetConsM3_D8], [UnbilledCons_UnbMetConsM3_D9], [UnbilledCons_UnbMetConsM3_D10], [UnbilledCons_UnbMetConsM3_D11], [UnbilledCons_UnbUnmetConsM3_H6], [UnbilledCons_UnbUnmetConsM3_H7], [UnbilledCons_UnbUnmetConsM3_H8], [UnbilledCons_UnbUnmetConsM3_H9], [UnbilledCons_UnbUnmetConsM3_H10], [UnbilledCons_UnbUnmetConsM3_H11], [UnbilledCons_UnbUnmetConsError_J6], [UnbilledCons_UnbUnmetConsError_J7], [UnbilledCons_UnbUnmetConsError_J8], [UnbilledCons_UnbUnmetConsError_J9], [UnbilledCons_UnbUnmetConsError_J10], [UnbilledCons_UnbUnmetConsError_J11], [UnauthCons_IllegalConnDomEstNo_D6], [UnauthCons_IllegalConnDomPersPerHouse_H6], [UnauthCons_IllegalConnDomConsLitPerPersDay_J6], [UnauthCons_IllegalConnDomErrorMargin_F6], [UnauthCons_IllegalConnOthersErrorMargin_F10], [IllegalConnectionsOthersEstimatedNumber_D10], [IllegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10], [UnauthCons_MeterTampBypEtcEstNo_D14], [UnauthCons_MeterTampBypEtcErrorMargin_F14], [UnauthCons_MeterTampBypEtcConsLitPerCustDay_J14], [UnauthCons_OthersErrorMargin_F18], [UnauthCons_OthersErrorMargin_F19], [UnauthCons_OthersErrorMargin_F20], [UnauthCons_OthersErrorMargin_F21], [UnauthCons_OthersM3PerDay_J18], [UnauthCons_OthersM3PerDay_J19], [UnauthCons_OthersM3PerDay_J20], [UnauthCons_OthersM3PerDay_J21], [MetErrors_DetailedManualSpec_J6], [MetErrors_BilledMetConsWoBulkSupMetUndrreg_H8], [MetErrors_BilledMetConsWoBulkSupErrorMargin_N8], [MetErrors_Total_F12], [MetErrors_Total_F13], [MetErrors_Total_F14], [MetErrors_Total_F15], [MetErrors_Meter_H12], [MetErrors_Meter_H13], [MetErrors_Meter_H14], [MetErrors_Meter_H15], [MetErrors_Error_N12], [MetErrors_Error_N13], [MetErrors_Error_N14], [MetErrors_Error_N15], [MeteredBulkSupplyExportErrorMargin_N32], [UnbilledMeteredConsumptionWithoutBulkSupplyErrorMargin_N34], [CorruptMeterReadingPracticessErrorMargin_N38], [DataHandlingErrorsOffice_L40], [DataHandlingErrorsOfficeErrorMargin_N40], [MetErrors_MetBulkSupExpMetUnderreg_H32], [MetErrors_UnbillMetConsWoBulkSupplMetUndrreg_H34], [MetErrors_CorruptMetReadPractMetUndrreg_H38], [Network_DistributionAndTransmissionMains_D7], [Network_DistributionAndTransmissionMains_D8], [Network_DistributionAndTransmissionMains_D9], [Network_DistributionAndTransmissionMains_D10], [Network_NoOfConnOfRegCustomers_H10], [Network_NoOfInactAccountsWSvcConns_H18], [Network_AvgLenOfSvcConnFromBoundaryToMeterM_H32], [Network_PossibleUnd_D30], [Network_NoCustomers_H7], [Network_ErrorMargin_J7], [Network_ErrorMargin_J10], [Network_ErrorMargin_J18], [Network_ErrorMargin_J32], [Network_ErrorMargin_D35], [Prs_Area_B7], [Prs_Area_B8], [Prs_Area_B9], [Prs_Area_B10], [Prs_ApproxNoOfConn_D7], [Prs_DailyAvgPrsM_F7], [Prs_ApproxNoOfConn_D8], [Prs_DailyAvgPrsM_F8], [Prs_ApproxNoOfConn_D9], [Prs_DailyAvgPrsM_F9], [Prs_ApproxNoOfConn_D10], [Prs_DailyAvgPrsM_F10], [Prs_ErrorMarg_F26], [Interm_Conn_D7], [Interm_Conn_D8], [Interm_Conn_D9], [Interm_Conn_D10], [Interm_Days_F7], [Interm_Days_F8], [Interm_Days_F9], [Interm_Days_F10], [Interm_Hour_H7], [Interm_Hour_H8], [Interm_Hour_H9], [Interm_Hour_H10], [Interm_ErrorMarg_H26], [FinancData_G6], [FinancData_K6], [FinancData_G8], [FinancData_D26], [FinancData_G35], [MatrixOneIn_SelectedOption], [MatrixOneIn_C11], [MatrixOneIn_C12], [MatrixOneIn_C13], [MatrixOneIn_C14], [MatrixOneIn_C21], [MatrixOneIn_C22], [MatrixOneIn_C23], [MatrixOneIn_C24], [MatrixOneIn_D21], [MatrixOneIn_D22], [MatrixOneIn_D23], [MatrixOneIn_D24], [MatrixOneIn_E11], [MatrixOneIn_E12], [MatrixOneIn_E13], [MatrixOneIn_E14], [MatrixOneIn_E21], [MatrixOneIn_E22], [MatrixOneIn_E23], [MatrixOneIn_E24], [MatrixOneIn_F11], [MatrixOneIn_F12], [MatrixOneIn_F13], [MatrixOneIn_F14], [MatrixOneIn_F21], [MatrixOneIn_F22], [MatrixOneIn_F23], [MatrixOneIn_F24], [MatrixOneIn_G11], [MatrixOneIn_G12], [MatrixOneIn_G13], [MatrixOneIn_G14], [MatrixOneIn_G21], [MatrixOneIn_G22], [MatrixOneIn_G23], [MatrixOneIn_G24], [MatrixOneIn_H11], [MatrixOneIn_H12], [MatrixOneIn_H13], [MatrixOneIn_H14], [MatrixOneIn_H21], [MatrixOneIn_H22], [MatrixOneIn_H23], [MatrixOneIn_H24], [MatrixTwoIn_SelectedOption], [MatrixTwoIn_D21], [MatrixTwoIn_D22], [MatrixTwoIn_D23], [MatrixTwoIn_D24], [MatrixTwoIn_E11], [MatrixTwoIn_E12], [MatrixTwoIn_E13], [MatrixTwoIn_E14], [MatrixTwoIn_E21], [MatrixTwoIn_E22], [MatrixTwoIn_E23], [MatrixTwoIn_E24], [MatrixTwoIn_F11], [MatrixTwoIn_F12], [MatrixTwoIn_F13], [MatrixTwoIn_F14], [MatrixTwoIn_F21], [MatrixTwoIn_F22], [MatrixTwoIn_F23], [MatrixTwoIn_F24], [MatrixTwoIn_G11], [MatrixTwoIn_G12], [MatrixTwoIn_G13], [MatrixTwoIn_G14], [MatrixTwoIn_G21], [MatrixTwoIn_G22], [MatrixTwoIn_G23], [MatrixTwoIn_G24], [MatrixTwoIn_H11], [MatrixTwoIn_H12], [MatrixTwoIn_H13], [MatrixTwoIn_H14], [MatrixTwoIn_H21], [MatrixTwoIn_H22], [MatrixTwoIn_H23], [MatrixTwoIn_H24], [SystemInputVolume_B19], [SystemInputVolumeErrorMargin_B21], [AuthorizedConsumption_K12], [AuthorizedConsumptionErrorMargin_K15], [WaterLosses_K29], [WaterLossesErrorMargin_K31], [BilledAuthorizedConsumption_T8], [UnbilledAuthorizedConsumption_T16], [UnbilledAuthorizedConsumptionErrorMargin_T20], [CommercialLosses_T26], [CommercialLossesErrorMargin_T29], [PhysicalLossesM3_T34], [PhyscialLossesErrorMargin_AH35], [BilledMeteredConsumption_AC4], [BilledUnmeteredConsumption_AC9], [UnbilledMeteredConsumption_AC14], [UnbilledUnmeteredConsumption_AC19], [UnbilledUnmeteredConsumptionErrorMargin_AO20], [UnauthorizedConsumption_AC24], [UnauthorizedConsumptionErrorMargin_AO25], [CustomerMeterInaccuraciesAndErrorsM3_AC29], [CustomerMeterInaccuraciesAndErrorsErrorMargin_AO30], [RevenueWaterM3_AY8], [NonRevenueWaterM3_AY24], [NonRevenueWaterErrorMargin_AY26]) VALUES (1101, N'EMEA\79749', CAST(N'2021-05-11T10:56:23.767' AS DateTime), N'EMEA\79749', CAST(N'2021-05-25T17:50:20.400' AS DateTime), 6773, 2021, 5, N'Desc 1', 0, 0, 31, NULL, NULL, NULL, NULL, 111111, 0, 0, 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 5551.878, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, N'PLN', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 111111, 0, 5551.878, 0, 105559.122, 0, 5551.878, 0, 0, 0, 0, 105559.122, 0, 5551.878, 0, 0, 0, 0, 0, 0, 0, 0, 5551.878, 105559.122, 0)
GO
INSERT [dbo].[tbWbEasyCalcData] ([WbEasyCalcDataId], [CreateLogin], [CreateDate], [ModifyLogin], [ModifyDate], [ZoneId], [YearNo], [MonthNo], [Description], [IsArchive], [IsAccepted], [Start_PeriodDays_M21], [SysInput_Desc_B6], [SysInput_Desc_B7], [SysInput_Desc_B8], [SysInput_Desc_B9], [SysInput_SystemInputVolumeM3_D6], [SysInput_SystemInputVolumeError_F6], [SysInput_SystemInputVolumeM3_D7], [SysInput_SystemInputVolumeError_F7], [SysInput_SystemInputVolumeM3_D8], [SysInput_SystemInputVolumeError_F8], [SysInput_SystemInputVolumeM3_D9], [SysInput_SystemInputVolumeError_F9], [BilledCons_Desc_B8], [BilledCons_Desc_B9], [BilledCons_Desc_B10], [BilledCons_Desc_B11], [BilledCons_Desc_F8], [BilledCons_Desc_F9], [BilledCons_Desc_F10], [BilledCons_Desc_F11], [UnbilledCons_Desc_D8], [UnbilledCons_Desc_D9], [UnbilledCons_Desc_D10], [UnbilledCons_Desc_D11], [UnbilledCons_Desc_F6], [UnbilledCons_Desc_F7], [UnbilledCons_Desc_F8], [UnbilledCons_Desc_F9], [UnbilledCons_Desc_F10], [UnbilledCons_Desc_F11], [UnauthCons_Desc_B18], [UnauthCons_Desc_B19], [UnauthCons_Desc_B20], [UnauthCons_Desc_B21], [MetErrors_Desc_D12], [MetErrors_Desc_D13], [MetErrors_Desc_D14], [MetErrors_Desc_D15], [Network_Desc_B7], [Network_Desc_B8], [Network_Desc_B9], [Network_Desc_B10], [Interm_Area_B7], [Interm_Area_B8], [Interm_Area_B9], [Interm_Area_B10], [BilledCons_BilledMetConsBulkWatSupExpM3_D6], [BilledCons_BilledUnmetConsBulkWatSupExpM3_H6], [BilledCons_UnbMetConsM3_D8], [BilledCons_UnbMetConsM3_D9], [BilledCons_UnbMetConsM3_D10], [BilledCons_UnbMetConsM3_D11], [BilledCons_UnbUnmetConsM3_H8], [BilledCons_UnbUnmetConsM3_H9], [BilledCons_UnbUnmetConsM3_H10], [BilledCons_UnbUnmetConsM3_H11], [UnbilledCons_MetConsBulkWatSupExpM3_D6], [UnbilledCons_UnbMetConsM3_D8], [UnbilledCons_UnbMetConsM3_D9], [UnbilledCons_UnbMetConsM3_D10], [UnbilledCons_UnbMetConsM3_D11], [UnbilledCons_UnbUnmetConsM3_H6], [UnbilledCons_UnbUnmetConsM3_H7], [UnbilledCons_UnbUnmetConsM3_H8], [UnbilledCons_UnbUnmetConsM3_H9], [UnbilledCons_UnbUnmetConsM3_H10], [UnbilledCons_UnbUnmetConsM3_H11], [UnbilledCons_UnbUnmetConsError_J6], [UnbilledCons_UnbUnmetConsError_J7], [UnbilledCons_UnbUnmetConsError_J8], [UnbilledCons_UnbUnmetConsError_J9], [UnbilledCons_UnbUnmetConsError_J10], [UnbilledCons_UnbUnmetConsError_J11], [UnauthCons_IllegalConnDomEstNo_D6], [UnauthCons_IllegalConnDomPersPerHouse_H6], [UnauthCons_IllegalConnDomConsLitPerPersDay_J6], [UnauthCons_IllegalConnDomErrorMargin_F6], [UnauthCons_IllegalConnOthersErrorMargin_F10], [IllegalConnectionsOthersEstimatedNumber_D10], [IllegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10], [UnauthCons_MeterTampBypEtcEstNo_D14], [UnauthCons_MeterTampBypEtcErrorMargin_F14], [UnauthCons_MeterTampBypEtcConsLitPerCustDay_J14], [UnauthCons_OthersErrorMargin_F18], [UnauthCons_OthersErrorMargin_F19], [UnauthCons_OthersErrorMargin_F20], [UnauthCons_OthersErrorMargin_F21], [UnauthCons_OthersM3PerDay_J18], [UnauthCons_OthersM3PerDay_J19], [UnauthCons_OthersM3PerDay_J20], [UnauthCons_OthersM3PerDay_J21], [MetErrors_DetailedManualSpec_J6], [MetErrors_BilledMetConsWoBulkSupMetUndrreg_H8], [MetErrors_BilledMetConsWoBulkSupErrorMargin_N8], [MetErrors_Total_F12], [MetErrors_Total_F13], [MetErrors_Total_F14], [MetErrors_Total_F15], [MetErrors_Meter_H12], [MetErrors_Meter_H13], [MetErrors_Meter_H14], [MetErrors_Meter_H15], [MetErrors_Error_N12], [MetErrors_Error_N13], [MetErrors_Error_N14], [MetErrors_Error_N15], [MeteredBulkSupplyExportErrorMargin_N32], [UnbilledMeteredConsumptionWithoutBulkSupplyErrorMargin_N34], [CorruptMeterReadingPracticessErrorMargin_N38], [DataHandlingErrorsOffice_L40], [DataHandlingErrorsOfficeErrorMargin_N40], [MetErrors_MetBulkSupExpMetUnderreg_H32], [MetErrors_UnbillMetConsWoBulkSupplMetUndrreg_H34], [MetErrors_CorruptMetReadPractMetUndrreg_H38], [Network_DistributionAndTransmissionMains_D7], [Network_DistributionAndTransmissionMains_D8], [Network_DistributionAndTransmissionMains_D9], [Network_DistributionAndTransmissionMains_D10], [Network_NoOfConnOfRegCustomers_H10], [Network_NoOfInactAccountsWSvcConns_H18], [Network_AvgLenOfSvcConnFromBoundaryToMeterM_H32], [Network_PossibleUnd_D30], [Network_NoCustomers_H7], [Network_ErrorMargin_J7], [Network_ErrorMargin_J10], [Network_ErrorMargin_J18], [Network_ErrorMargin_J32], [Network_ErrorMargin_D35], [Prs_Area_B7], [Prs_Area_B8], [Prs_Area_B9], [Prs_Area_B10], [Prs_ApproxNoOfConn_D7], [Prs_DailyAvgPrsM_F7], [Prs_ApproxNoOfConn_D8], [Prs_DailyAvgPrsM_F8], [Prs_ApproxNoOfConn_D9], [Prs_DailyAvgPrsM_F9], [Prs_ApproxNoOfConn_D10], [Prs_DailyAvgPrsM_F10], [Prs_ErrorMarg_F26], [Interm_Conn_D7], [Interm_Conn_D8], [Interm_Conn_D9], [Interm_Conn_D10], [Interm_Days_F7], [Interm_Days_F8], [Interm_Days_F9], [Interm_Days_F10], [Interm_Hour_H7], [Interm_Hour_H8], [Interm_Hour_H9], [Interm_Hour_H10], [Interm_ErrorMarg_H26], [FinancData_G6], [FinancData_K6], [FinancData_G8], [FinancData_D26], [FinancData_G35], [MatrixOneIn_SelectedOption], [MatrixOneIn_C11], [MatrixOneIn_C12], [MatrixOneIn_C13], [MatrixOneIn_C14], [MatrixOneIn_C21], [MatrixOneIn_C22], [MatrixOneIn_C23], [MatrixOneIn_C24], [MatrixOneIn_D21], [MatrixOneIn_D22], [MatrixOneIn_D23], [MatrixOneIn_D24], [MatrixOneIn_E11], [MatrixOneIn_E12], [MatrixOneIn_E13], [MatrixOneIn_E14], [MatrixOneIn_E21], [MatrixOneIn_E22], [MatrixOneIn_E23], [MatrixOneIn_E24], [MatrixOneIn_F11], [MatrixOneIn_F12], [MatrixOneIn_F13], [MatrixOneIn_F14], [MatrixOneIn_F21], [MatrixOneIn_F22], [MatrixOneIn_F23], [MatrixOneIn_F24], [MatrixOneIn_G11], [MatrixOneIn_G12], [MatrixOneIn_G13], [MatrixOneIn_G14], [MatrixOneIn_G21], [MatrixOneIn_G22], [MatrixOneIn_G23], [MatrixOneIn_G24], [MatrixOneIn_H11], [MatrixOneIn_H12], [MatrixOneIn_H13], [MatrixOneIn_H14], [MatrixOneIn_H21], [MatrixOneIn_H22], [MatrixOneIn_H23], [MatrixOneIn_H24], [MatrixTwoIn_SelectedOption], [MatrixTwoIn_D21], [MatrixTwoIn_D22], [MatrixTwoIn_D23], [MatrixTwoIn_D24], [MatrixTwoIn_E11], [MatrixTwoIn_E12], [MatrixTwoIn_E13], [MatrixTwoIn_E14], [MatrixTwoIn_E21], [MatrixTwoIn_E22], [MatrixTwoIn_E23], [MatrixTwoIn_E24], [MatrixTwoIn_F11], [MatrixTwoIn_F12], [MatrixTwoIn_F13], [MatrixTwoIn_F14], [MatrixTwoIn_F21], [MatrixTwoIn_F22], [MatrixTwoIn_F23], [MatrixTwoIn_F24], [MatrixTwoIn_G11], [MatrixTwoIn_G12], [MatrixTwoIn_G13], [MatrixTwoIn_G14], [MatrixTwoIn_G21], [MatrixTwoIn_G22], [MatrixTwoIn_G23], [MatrixTwoIn_G24], [MatrixTwoIn_H11], [MatrixTwoIn_H12], [MatrixTwoIn_H13], [MatrixTwoIn_H14], [MatrixTwoIn_H21], [MatrixTwoIn_H22], [MatrixTwoIn_H23], [MatrixTwoIn_H24], [SystemInputVolume_B19], [SystemInputVolumeErrorMargin_B21], [AuthorizedConsumption_K12], [AuthorizedConsumptionErrorMargin_K15], [WaterLosses_K29], [WaterLossesErrorMargin_K31], [BilledAuthorizedConsumption_T8], [UnbilledAuthorizedConsumption_T16], [UnbilledAuthorizedConsumptionErrorMargin_T20], [CommercialLosses_T26], [CommercialLossesErrorMargin_T29], [PhysicalLossesM3_T34], [PhyscialLossesErrorMargin_AH35], [BilledMeteredConsumption_AC4], [BilledUnmeteredConsumption_AC9], [UnbilledMeteredConsumption_AC14], [UnbilledUnmeteredConsumption_AC19], [UnbilledUnmeteredConsumptionErrorMargin_AO20], [UnauthorizedConsumption_AC24], [UnauthorizedConsumptionErrorMargin_AO25], [CustomerMeterInaccuraciesAndErrorsM3_AC29], [CustomerMeterInaccuraciesAndErrorsErrorMargin_AO30], [RevenueWaterM3_AY8], [NonRevenueWaterM3_AY24], [NonRevenueWaterErrorMargin_AY26]) VALUES (1104, N'EMEA\79749', CAST(N'2021-05-15T12:18:00.277' AS DateTime), N'EMEA\79749', CAST(N'2021-05-25T17:48:48.860' AS DateTime), 6773, 2021, 5, N'xxxxxxxxxxxxxxxxxxxxx', 0, 0, 31, NULL, NULL, NULL, NULL, 0, 0, 0, 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 600, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, N'PLN', 0, 0, 0, 1, 333, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 601, 0, -601, 0, 600, 1, 0, 0, 0, -601, 0, 600, 0, 1, 0, 0, 0, 0, 0, 0, 600, -600, 0)
GO
INSERT [dbo].[tbWbEasyCalcData] ([WbEasyCalcDataId], [CreateLogin], [CreateDate], [ModifyLogin], [ModifyDate], [ZoneId], [YearNo], [MonthNo], [Description], [IsArchive], [IsAccepted], [Start_PeriodDays_M21], [SysInput_Desc_B6], [SysInput_Desc_B7], [SysInput_Desc_B8], [SysInput_Desc_B9], [SysInput_SystemInputVolumeM3_D6], [SysInput_SystemInputVolumeError_F6], [SysInput_SystemInputVolumeM3_D7], [SysInput_SystemInputVolumeError_F7], [SysInput_SystemInputVolumeM3_D8], [SysInput_SystemInputVolumeError_F8], [SysInput_SystemInputVolumeM3_D9], [SysInput_SystemInputVolumeError_F9], [BilledCons_Desc_B8], [BilledCons_Desc_B9], [BilledCons_Desc_B10], [BilledCons_Desc_B11], [BilledCons_Desc_F8], [BilledCons_Desc_F9], [BilledCons_Desc_F10], [BilledCons_Desc_F11], [UnbilledCons_Desc_D8], [UnbilledCons_Desc_D9], [UnbilledCons_Desc_D10], [UnbilledCons_Desc_D11], [UnbilledCons_Desc_F6], [UnbilledCons_Desc_F7], [UnbilledCons_Desc_F8], [UnbilledCons_Desc_F9], [UnbilledCons_Desc_F10], [UnbilledCons_Desc_F11], [UnauthCons_Desc_B18], [UnauthCons_Desc_B19], [UnauthCons_Desc_B20], [UnauthCons_Desc_B21], [MetErrors_Desc_D12], [MetErrors_Desc_D13], [MetErrors_Desc_D14], [MetErrors_Desc_D15], [Network_Desc_B7], [Network_Desc_B8], [Network_Desc_B9], [Network_Desc_B10], [Interm_Area_B7], [Interm_Area_B8], [Interm_Area_B9], [Interm_Area_B10], [BilledCons_BilledMetConsBulkWatSupExpM3_D6], [BilledCons_BilledUnmetConsBulkWatSupExpM3_H6], [BilledCons_UnbMetConsM3_D8], [BilledCons_UnbMetConsM3_D9], [BilledCons_UnbMetConsM3_D10], [BilledCons_UnbMetConsM3_D11], [BilledCons_UnbUnmetConsM3_H8], [BilledCons_UnbUnmetConsM3_H9], [BilledCons_UnbUnmetConsM3_H10], [BilledCons_UnbUnmetConsM3_H11], [UnbilledCons_MetConsBulkWatSupExpM3_D6], [UnbilledCons_UnbMetConsM3_D8], [UnbilledCons_UnbMetConsM3_D9], [UnbilledCons_UnbMetConsM3_D10], [UnbilledCons_UnbMetConsM3_D11], [UnbilledCons_UnbUnmetConsM3_H6], [UnbilledCons_UnbUnmetConsM3_H7], [UnbilledCons_UnbUnmetConsM3_H8], [UnbilledCons_UnbUnmetConsM3_H9], [UnbilledCons_UnbUnmetConsM3_H10], [UnbilledCons_UnbUnmetConsM3_H11], [UnbilledCons_UnbUnmetConsError_J6], [UnbilledCons_UnbUnmetConsError_J7], [UnbilledCons_UnbUnmetConsError_J8], [UnbilledCons_UnbUnmetConsError_J9], [UnbilledCons_UnbUnmetConsError_J10], [UnbilledCons_UnbUnmetConsError_J11], [UnauthCons_IllegalConnDomEstNo_D6], [UnauthCons_IllegalConnDomPersPerHouse_H6], [UnauthCons_IllegalConnDomConsLitPerPersDay_J6], [UnauthCons_IllegalConnDomErrorMargin_F6], [UnauthCons_IllegalConnOthersErrorMargin_F10], [IllegalConnectionsOthersEstimatedNumber_D10], [IllegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10], [UnauthCons_MeterTampBypEtcEstNo_D14], [UnauthCons_MeterTampBypEtcErrorMargin_F14], [UnauthCons_MeterTampBypEtcConsLitPerCustDay_J14], [UnauthCons_OthersErrorMargin_F18], [UnauthCons_OthersErrorMargin_F19], [UnauthCons_OthersErrorMargin_F20], [UnauthCons_OthersErrorMargin_F21], [UnauthCons_OthersM3PerDay_J18], [UnauthCons_OthersM3PerDay_J19], [UnauthCons_OthersM3PerDay_J20], [UnauthCons_OthersM3PerDay_J21], [MetErrors_DetailedManualSpec_J6], [MetErrors_BilledMetConsWoBulkSupMetUndrreg_H8], [MetErrors_BilledMetConsWoBulkSupErrorMargin_N8], [MetErrors_Total_F12], [MetErrors_Total_F13], [MetErrors_Total_F14], [MetErrors_Total_F15], [MetErrors_Meter_H12], [MetErrors_Meter_H13], [MetErrors_Meter_H14], [MetErrors_Meter_H15], [MetErrors_Error_N12], [MetErrors_Error_N13], [MetErrors_Error_N14], [MetErrors_Error_N15], [MeteredBulkSupplyExportErrorMargin_N32], [UnbilledMeteredConsumptionWithoutBulkSupplyErrorMargin_N34], [CorruptMeterReadingPracticessErrorMargin_N38], [DataHandlingErrorsOffice_L40], [DataHandlingErrorsOfficeErrorMargin_N40], [MetErrors_MetBulkSupExpMetUnderreg_H32], [MetErrors_UnbillMetConsWoBulkSupplMetUndrreg_H34], [MetErrors_CorruptMetReadPractMetUndrreg_H38], [Network_DistributionAndTransmissionMains_D7], [Network_DistributionAndTransmissionMains_D8], [Network_DistributionAndTransmissionMains_D9], [Network_DistributionAndTransmissionMains_D10], [Network_NoOfConnOfRegCustomers_H10], [Network_NoOfInactAccountsWSvcConns_H18], [Network_AvgLenOfSvcConnFromBoundaryToMeterM_H32], [Network_PossibleUnd_D30], [Network_NoCustomers_H7], [Network_ErrorMargin_J7], [Network_ErrorMargin_J10], [Network_ErrorMargin_J18], [Network_ErrorMargin_J32], [Network_ErrorMargin_D35], [Prs_Area_B7], [Prs_Area_B8], [Prs_Area_B9], [Prs_Area_B10], [Prs_ApproxNoOfConn_D7], [Prs_DailyAvgPrsM_F7], [Prs_ApproxNoOfConn_D8], [Prs_DailyAvgPrsM_F8], [Prs_ApproxNoOfConn_D9], [Prs_DailyAvgPrsM_F9], [Prs_ApproxNoOfConn_D10], [Prs_DailyAvgPrsM_F10], [Prs_ErrorMarg_F26], [Interm_Conn_D7], [Interm_Conn_D8], [Interm_Conn_D9], [Interm_Conn_D10], [Interm_Days_F7], [Interm_Days_F8], [Interm_Days_F9], [Interm_Days_F10], [Interm_Hour_H7], [Interm_Hour_H8], [Interm_Hour_H9], [Interm_Hour_H10], [Interm_ErrorMarg_H26], [FinancData_G6], [FinancData_K6], [FinancData_G8], [FinancData_D26], [FinancData_G35], [MatrixOneIn_SelectedOption], [MatrixOneIn_C11], [MatrixOneIn_C12], [MatrixOneIn_C13], [MatrixOneIn_C14], [MatrixOneIn_C21], [MatrixOneIn_C22], [MatrixOneIn_C23], [MatrixOneIn_C24], [MatrixOneIn_D21], [MatrixOneIn_D22], [MatrixOneIn_D23], [MatrixOneIn_D24], [MatrixOneIn_E11], [MatrixOneIn_E12], [MatrixOneIn_E13], [MatrixOneIn_E14], [MatrixOneIn_E21], [MatrixOneIn_E22], [MatrixOneIn_E23], [MatrixOneIn_E24], [MatrixOneIn_F11], [MatrixOneIn_F12], [MatrixOneIn_F13], [MatrixOneIn_F14], [MatrixOneIn_F21], [MatrixOneIn_F22], [MatrixOneIn_F23], [MatrixOneIn_F24], [MatrixOneIn_G11], [MatrixOneIn_G12], [MatrixOneIn_G13], [MatrixOneIn_G14], [MatrixOneIn_G21], [MatrixOneIn_G22], [MatrixOneIn_G23], [MatrixOneIn_G24], [MatrixOneIn_H11], [MatrixOneIn_H12], [MatrixOneIn_H13], [MatrixOneIn_H14], [MatrixOneIn_H21], [MatrixOneIn_H22], [MatrixOneIn_H23], [MatrixOneIn_H24], [MatrixTwoIn_SelectedOption], [MatrixTwoIn_D21], [MatrixTwoIn_D22], [MatrixTwoIn_D23], [MatrixTwoIn_D24], [MatrixTwoIn_E11], [MatrixTwoIn_E12], [MatrixTwoIn_E13], [MatrixTwoIn_E14], [MatrixTwoIn_E21], [MatrixTwoIn_E22], [MatrixTwoIn_E23], [MatrixTwoIn_E24], [MatrixTwoIn_F11], [MatrixTwoIn_F12], [MatrixTwoIn_F13], [MatrixTwoIn_F14], [MatrixTwoIn_F21], [MatrixTwoIn_F22], [MatrixTwoIn_F23], [MatrixTwoIn_F24], [MatrixTwoIn_G11], [MatrixTwoIn_G12], [MatrixTwoIn_G13], [MatrixTwoIn_G14], [MatrixTwoIn_G21], [MatrixTwoIn_G22], [MatrixTwoIn_G23], [MatrixTwoIn_G24], [MatrixTwoIn_H11], [MatrixTwoIn_H12], [MatrixTwoIn_H13], [MatrixTwoIn_H14], [MatrixTwoIn_H21], [MatrixTwoIn_H22], [MatrixTwoIn_H23], [MatrixTwoIn_H24], [SystemInputVolume_B19], [SystemInputVolumeErrorMargin_B21], [AuthorizedConsumption_K12], [AuthorizedConsumptionErrorMargin_K15], [WaterLosses_K29], [WaterLossesErrorMargin_K31], [BilledAuthorizedConsumption_T8], [UnbilledAuthorizedConsumption_T16], [UnbilledAuthorizedConsumptionErrorMargin_T20], [CommercialLosses_T26], [CommercialLossesErrorMargin_T29], [PhysicalLossesM3_T34], [PhyscialLossesErrorMargin_AH35], [BilledMeteredConsumption_AC4], [BilledUnmeteredConsumption_AC9], [UnbilledMeteredConsumption_AC14], [UnbilledUnmeteredConsumption_AC19], [UnbilledUnmeteredConsumptionErrorMargin_AO20], [UnauthorizedConsumption_AC24], [UnauthorizedConsumptionErrorMargin_AO25], [CustomerMeterInaccuraciesAndErrorsM3_AC29], [CustomerMeterInaccuraciesAndErrorsErrorMargin_AO30], [RevenueWaterM3_AY8], [NonRevenueWaterM3_AY24], [NonRevenueWaterErrorMargin_AY26]) VALUES (1108, N'EMEA\79749', CAST(N'2021-05-25T17:18:50.280' AS DateTime), N'EMEA\79749', CAST(N'2021-05-25T17:49:00.660' AS DateTime), 6773, 2021, 5, N'', 0, 0, 31, NULL, NULL, NULL, NULL, 0, 0, 0, 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, N'PLN', 0, 0, 0, 1, 333, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, -2, 0, 0, 2, 0, 0, 0, -2, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0)
GO
INSERT [dbo].[tbWbEasyCalcData] ([WbEasyCalcDataId], [CreateLogin], [CreateDate], [ModifyLogin], [ModifyDate], [ZoneId], [YearNo], [MonthNo], [Description], [IsArchive], [IsAccepted], [Start_PeriodDays_M21], [SysInput_Desc_B6], [SysInput_Desc_B7], [SysInput_Desc_B8], [SysInput_Desc_B9], [SysInput_SystemInputVolumeM3_D6], [SysInput_SystemInputVolumeError_F6], [SysInput_SystemInputVolumeM3_D7], [SysInput_SystemInputVolumeError_F7], [SysInput_SystemInputVolumeM3_D8], [SysInput_SystemInputVolumeError_F8], [SysInput_SystemInputVolumeM3_D9], [SysInput_SystemInputVolumeError_F9], [BilledCons_Desc_B8], [BilledCons_Desc_B9], [BilledCons_Desc_B10], [BilledCons_Desc_B11], [BilledCons_Desc_F8], [BilledCons_Desc_F9], [BilledCons_Desc_F10], [BilledCons_Desc_F11], [UnbilledCons_Desc_D8], [UnbilledCons_Desc_D9], [UnbilledCons_Desc_D10], [UnbilledCons_Desc_D11], [UnbilledCons_Desc_F6], [UnbilledCons_Desc_F7], [UnbilledCons_Desc_F8], [UnbilledCons_Desc_F9], [UnbilledCons_Desc_F10], [UnbilledCons_Desc_F11], [UnauthCons_Desc_B18], [UnauthCons_Desc_B19], [UnauthCons_Desc_B20], [UnauthCons_Desc_B21], [MetErrors_Desc_D12], [MetErrors_Desc_D13], [MetErrors_Desc_D14], [MetErrors_Desc_D15], [Network_Desc_B7], [Network_Desc_B8], [Network_Desc_B9], [Network_Desc_B10], [Interm_Area_B7], [Interm_Area_B8], [Interm_Area_B9], [Interm_Area_B10], [BilledCons_BilledMetConsBulkWatSupExpM3_D6], [BilledCons_BilledUnmetConsBulkWatSupExpM3_H6], [BilledCons_UnbMetConsM3_D8], [BilledCons_UnbMetConsM3_D9], [BilledCons_UnbMetConsM3_D10], [BilledCons_UnbMetConsM3_D11], [BilledCons_UnbUnmetConsM3_H8], [BilledCons_UnbUnmetConsM3_H9], [BilledCons_UnbUnmetConsM3_H10], [BilledCons_UnbUnmetConsM3_H11], [UnbilledCons_MetConsBulkWatSupExpM3_D6], [UnbilledCons_UnbMetConsM3_D8], [UnbilledCons_UnbMetConsM3_D9], [UnbilledCons_UnbMetConsM3_D10], [UnbilledCons_UnbMetConsM3_D11], [UnbilledCons_UnbUnmetConsM3_H6], [UnbilledCons_UnbUnmetConsM3_H7], [UnbilledCons_UnbUnmetConsM3_H8], [UnbilledCons_UnbUnmetConsM3_H9], [UnbilledCons_UnbUnmetConsM3_H10], [UnbilledCons_UnbUnmetConsM3_H11], [UnbilledCons_UnbUnmetConsError_J6], [UnbilledCons_UnbUnmetConsError_J7], [UnbilledCons_UnbUnmetConsError_J8], [UnbilledCons_UnbUnmetConsError_J9], [UnbilledCons_UnbUnmetConsError_J10], [UnbilledCons_UnbUnmetConsError_J11], [UnauthCons_IllegalConnDomEstNo_D6], [UnauthCons_IllegalConnDomPersPerHouse_H6], [UnauthCons_IllegalConnDomConsLitPerPersDay_J6], [UnauthCons_IllegalConnDomErrorMargin_F6], [UnauthCons_IllegalConnOthersErrorMargin_F10], [IllegalConnectionsOthersEstimatedNumber_D10], [IllegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10], [UnauthCons_MeterTampBypEtcEstNo_D14], [UnauthCons_MeterTampBypEtcErrorMargin_F14], [UnauthCons_MeterTampBypEtcConsLitPerCustDay_J14], [UnauthCons_OthersErrorMargin_F18], [UnauthCons_OthersErrorMargin_F19], [UnauthCons_OthersErrorMargin_F20], [UnauthCons_OthersErrorMargin_F21], [UnauthCons_OthersM3PerDay_J18], [UnauthCons_OthersM3PerDay_J19], [UnauthCons_OthersM3PerDay_J20], [UnauthCons_OthersM3PerDay_J21], [MetErrors_DetailedManualSpec_J6], [MetErrors_BilledMetConsWoBulkSupMetUndrreg_H8], [MetErrors_BilledMetConsWoBulkSupErrorMargin_N8], [MetErrors_Total_F12], [MetErrors_Total_F13], [MetErrors_Total_F14], [MetErrors_Total_F15], [MetErrors_Meter_H12], [MetErrors_Meter_H13], [MetErrors_Meter_H14], [MetErrors_Meter_H15], [MetErrors_Error_N12], [MetErrors_Error_N13], [MetErrors_Error_N14], [MetErrors_Error_N15], [MeteredBulkSupplyExportErrorMargin_N32], [UnbilledMeteredConsumptionWithoutBulkSupplyErrorMargin_N34], [CorruptMeterReadingPracticessErrorMargin_N38], [DataHandlingErrorsOffice_L40], [DataHandlingErrorsOfficeErrorMargin_N40], [MetErrors_MetBulkSupExpMetUnderreg_H32], [MetErrors_UnbillMetConsWoBulkSupplMetUndrreg_H34], [MetErrors_CorruptMetReadPractMetUndrreg_H38], [Network_DistributionAndTransmissionMains_D7], [Network_DistributionAndTransmissionMains_D8], [Network_DistributionAndTransmissionMains_D9], [Network_DistributionAndTransmissionMains_D10], [Network_NoOfConnOfRegCustomers_H10], [Network_NoOfInactAccountsWSvcConns_H18], [Network_AvgLenOfSvcConnFromBoundaryToMeterM_H32], [Network_PossibleUnd_D30], [Network_NoCustomers_H7], [Network_ErrorMargin_J7], [Network_ErrorMargin_J10], [Network_ErrorMargin_J18], [Network_ErrorMargin_J32], [Network_ErrorMargin_D35], [Prs_Area_B7], [Prs_Area_B8], [Prs_Area_B9], [Prs_Area_B10], [Prs_ApproxNoOfConn_D7], [Prs_DailyAvgPrsM_F7], [Prs_ApproxNoOfConn_D8], [Prs_DailyAvgPrsM_F8], [Prs_ApproxNoOfConn_D9], [Prs_DailyAvgPrsM_F9], [Prs_ApproxNoOfConn_D10], [Prs_DailyAvgPrsM_F10], [Prs_ErrorMarg_F26], [Interm_Conn_D7], [Interm_Conn_D8], [Interm_Conn_D9], [Interm_Conn_D10], [Interm_Days_F7], [Interm_Days_F8], [Interm_Days_F9], [Interm_Days_F10], [Interm_Hour_H7], [Interm_Hour_H8], [Interm_Hour_H9], [Interm_Hour_H10], [Interm_ErrorMarg_H26], [FinancData_G6], [FinancData_K6], [FinancData_G8], [FinancData_D26], [FinancData_G35], [MatrixOneIn_SelectedOption], [MatrixOneIn_C11], [MatrixOneIn_C12], [MatrixOneIn_C13], [MatrixOneIn_C14], [MatrixOneIn_C21], [MatrixOneIn_C22], [MatrixOneIn_C23], [MatrixOneIn_C24], [MatrixOneIn_D21], [MatrixOneIn_D22], [MatrixOneIn_D23], [MatrixOneIn_D24], [MatrixOneIn_E11], [MatrixOneIn_E12], [MatrixOneIn_E13], [MatrixOneIn_E14], [MatrixOneIn_E21], [MatrixOneIn_E22], [MatrixOneIn_E23], [MatrixOneIn_E24], [MatrixOneIn_F11], [MatrixOneIn_F12], [MatrixOneIn_F13], [MatrixOneIn_F14], [MatrixOneIn_F21], [MatrixOneIn_F22], [MatrixOneIn_F23], [MatrixOneIn_F24], [MatrixOneIn_G11], [MatrixOneIn_G12], [MatrixOneIn_G13], [MatrixOneIn_G14], [MatrixOneIn_G21], [MatrixOneIn_G22], [MatrixOneIn_G23], [MatrixOneIn_G24], [MatrixOneIn_H11], [MatrixOneIn_H12], [MatrixOneIn_H13], [MatrixOneIn_H14], [MatrixOneIn_H21], [MatrixOneIn_H22], [MatrixOneIn_H23], [MatrixOneIn_H24], [MatrixTwoIn_SelectedOption], [MatrixTwoIn_D21], [MatrixTwoIn_D22], [MatrixTwoIn_D23], [MatrixTwoIn_D24], [MatrixTwoIn_E11], [MatrixTwoIn_E12], [MatrixTwoIn_E13], [MatrixTwoIn_E14], [MatrixTwoIn_E21], [MatrixTwoIn_E22], [MatrixTwoIn_E23], [MatrixTwoIn_E24], [MatrixTwoIn_F11], [MatrixTwoIn_F12], [MatrixTwoIn_F13], [MatrixTwoIn_F14], [MatrixTwoIn_F21], [MatrixTwoIn_F22], [MatrixTwoIn_F23], [MatrixTwoIn_F24], [MatrixTwoIn_G11], [MatrixTwoIn_G12], [MatrixTwoIn_G13], [MatrixTwoIn_G14], [MatrixTwoIn_G21], [MatrixTwoIn_G22], [MatrixTwoIn_G23], [MatrixTwoIn_G24], [MatrixTwoIn_H11], [MatrixTwoIn_H12], [MatrixTwoIn_H13], [MatrixTwoIn_H14], [MatrixTwoIn_H21], [MatrixTwoIn_H22], [MatrixTwoIn_H23], [MatrixTwoIn_H24], [SystemInputVolume_B19], [SystemInputVolumeErrorMargin_B21], [AuthorizedConsumption_K12], [AuthorizedConsumptionErrorMargin_K15], [WaterLosses_K29], [WaterLossesErrorMargin_K31], [BilledAuthorizedConsumption_T8], [UnbilledAuthorizedConsumption_T16], [UnbilledAuthorizedConsumptionErrorMargin_T20], [CommercialLosses_T26], [CommercialLossesErrorMargin_T29], [PhysicalLossesM3_T34], [PhyscialLossesErrorMargin_AH35], [BilledMeteredConsumption_AC4], [BilledUnmeteredConsumption_AC9], [UnbilledMeteredConsumption_AC14], [UnbilledUnmeteredConsumption_AC19], [UnbilledUnmeteredConsumptionErrorMargin_AO20], [UnauthorizedConsumption_AC24], [UnauthorizedConsumptionErrorMargin_AO25], [CustomerMeterInaccuraciesAndErrorsM3_AC29], [CustomerMeterInaccuraciesAndErrorsErrorMargin_AO30], [RevenueWaterM3_AY8], [NonRevenueWaterM3_AY24], [NonRevenueWaterErrorMargin_AY26]) VALUES (1109, N'EMEA\79749', CAST(N'2021-05-25T17:31:53.067' AS DateTime), N'EMEA\79749', CAST(N'2021-05-26T09:23:53.953' AS DateTime), 6773, 2021, 3, N'', 0, 0, 31, NULL, NULL, NULL, NULL, 0, 0, 0, 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, 0, 70625, 0, 0, 0, 0, 0, 0, 0, 0, 7, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 19539.125613145381, 0, 0, 0, 453, 489, 0, 0, 758, 0, 0, 0, 0, 0, NULL, NULL, NULL, NULL, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, N'PLN', 0, 0, 0, 1, 333, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 70632, 0, -70632, 0, 70625, 7, 0, 0, 0, -70632, 0, 70625, 0, 7, 0, 0, 0, 0, 0, 0, 70625, -70625, 0)
GO
SET IDENTITY_INSERT [dbo].[tbWbEasyCalcData] OFF
GO
INSERT [dbo].[tbYear] ([YearId], [YearName]) VALUES (2015, N'2015 ')
GO
INSERT [dbo].[tbYear] ([YearId], [YearName]) VALUES (2016, N'2016')
GO
INSERT [dbo].[tbYear] ([YearId], [YearName]) VALUES (2017, N'2017')
GO
INSERT [dbo].[tbYear] ([YearId], [YearName]) VALUES (2019, N'2019')
GO
INSERT [dbo].[tbYear] ([YearId], [YearName]) VALUES (2020, N'2020')
GO
INSERT [dbo].[tbYear] ([YearId], [YearName]) VALUES (2021, N'2021')
GO
INSERT [dbo].[tbYear] ([YearId], [YearName]) VALUES (2022, N'2022')
GO
INSERT [dbo].[tbYear] ([YearId], [YearName]) VALUES (2023, N'2023')
GO
INSERT [dbo].[tbYear] ([YearId], [YearName]) VALUES (2024, N'2024')
GO
INSERT [dbo].[tbYear] ([YearId], [YearName]) VALUES (2025, N'2025')
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_ModifyDate]  DEFAULT (getdate()) FOR [ModifyDate]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_IsArchive]  DEFAULT ((0)) FOR [IsArchive]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_IsAccepted]  DEFAULT ((0)) FOR [IsAccepted]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_SysInput_SystemInputVolumeM3_D7]  DEFAULT ((0)) FOR [SysInput_SystemInputVolumeM3_D7]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_SysInput_SystemInputVolumeError_F7]  DEFAULT ((0)) FOR [SysInput_SystemInputVolumeError_F7]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_SysInput_SystemInputVolumeM3_D8]  DEFAULT ((0)) FOR [SysInput_SystemInputVolumeM3_D8]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_SysInput_SystemInputVolumeError_F8]  DEFAULT ((0)) FOR [SysInput_SystemInputVolumeError_F8]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_SysInput_SystemInputVolumeM3_D9]  DEFAULT ((0)) FOR [SysInput_SystemInputVolumeM3_D9]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_SysInput_SystemInputVolumeError_F9]  DEFAULT ((0)) FOR [SysInput_SystemInputVolumeError_F9]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_BilledCons_UnbMetConsM3_D8]  DEFAULT ((0)) FOR [BilledCons_UnbMetConsM3_D8]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_BilledCons_UnbMetConsM3_D9]  DEFAULT ((0)) FOR [BilledCons_UnbMetConsM3_D9]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_BilledCons_UnbMetConsM3_D10]  DEFAULT ((0)) FOR [BilledCons_UnbMetConsM3_D10]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_BilledCons_UnbMetConsM3_D11]  DEFAULT ((0)) FOR [BilledCons_UnbMetConsM3_D11]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_BilledCons_UnbUnmetConsM3_H8]  DEFAULT ((0)) FOR [BilledCons_UnbUnmetConsM3_H8]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_BilledCons_UnbUnmetConsM3_H9]  DEFAULT ((0)) FOR [BilledCons_UnbUnmetConsM3_H9]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_BilledCons_UnbUnmetConsM3_H10]  DEFAULT ((0)) FOR [BilledCons_UnbUnmetConsM3_H10]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_BilledCons_UnbUnmetConsM3_H11]  DEFAULT ((0)) FOR [BilledCons_UnbUnmetConsM3_H11]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_UnbilledCons_UnbMetConsM3_D8]  DEFAULT ((0)) FOR [UnbilledCons_UnbMetConsM3_D8]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_UnbilledCons_UnbMetConsM3_D9]  DEFAULT ((0)) FOR [UnbilledCons_UnbMetConsM3_D9]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_UnbilledCons_UnbMetConsM3_D10]  DEFAULT ((0)) FOR [UnbilledCons_UnbMetConsM3_D10]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_UnbilledCons_UnbMetConsM3_D11]  DEFAULT ((0)) FOR [UnbilledCons_UnbMetConsM3_D11]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_UnbilledCons_UnbUnmetConsM3_H6]  DEFAULT ((0)) FOR [UnbilledCons_UnbUnmetConsM3_H6]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_UnbilledCons_UnbUnmetConsM3_H7]  DEFAULT ((0)) FOR [UnbilledCons_UnbUnmetConsM3_H7]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_UnbilledCons_UnbUnmetConsM3_H8]  DEFAULT ((0)) FOR [UnbilledCons_UnbUnmetConsM3_H8]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_UnbilledCons_UnbUnmetConsM3_H9]  DEFAULT ((0)) FOR [UnbilledCons_UnbUnmetConsM3_H9]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_UnbilledCons_UnbUnmetConsM3_H10]  DEFAULT ((0)) FOR [UnbilledCons_UnbUnmetConsM3_H10]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_UnbilledCons_UnbUnmetConsM3_H11]  DEFAULT ((0)) FOR [UnbilledCons_UnbUnmetConsM3_H11]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_UnbilledCons_UnbUnmetConsError_J6]  DEFAULT ((0)) FOR [UnbilledCons_UnbUnmetConsError_J6]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_UnbilledCons_UnbUnmetConsError_J7]  DEFAULT ((0)) FOR [UnbilledCons_UnbUnmetConsError_J7]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_UnbilledCons_UnbUnmetConsError_J8]  DEFAULT ((0)) FOR [UnbilledCons_UnbUnmetConsError_J8]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_UnbilledCons_UnbUnmetConsError_J9]  DEFAULT ((0)) FOR [UnbilledCons_UnbUnmetConsError_J9]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_UnbilledCons_UnbUnmetConsError_J10]  DEFAULT ((0)) FOR [UnbilledCons_UnbUnmetConsError_J10]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_UnbilledCons_UnbUnmetConsError_J11]  DEFAULT ((0)) FOR [UnbilledCons_UnbUnmetConsError_J11]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_IllegalConnectionsOthersEstimatedNumber_D10]  DEFAULT ((0)) FOR [IllegalConnectionsOthersEstimatedNumber_D10]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_IllegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10]  DEFAULT ((0)) FOR [IllegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_UnauthCons_OthersErrorMargin_F18]  DEFAULT ((0)) FOR [UnauthCons_OthersErrorMargin_F18]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_UnauthCons_OthersErrorMargin_F19]  DEFAULT ((0)) FOR [UnauthCons_OthersErrorMargin_F19]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_UnauthCons_OthersErrorMargin_F20]  DEFAULT ((0)) FOR [UnauthCons_OthersErrorMargin_F20]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_UnauthCons_OthersErrorMargin_F21]  DEFAULT ((0)) FOR [UnauthCons_OthersErrorMargin_F21]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_UnauthCons_OthersM3PerDay_J18]  DEFAULT ((0)) FOR [UnauthCons_OthersM3PerDay_J18]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_UnauthCons_OthersM3PerDay_J19]  DEFAULT ((0)) FOR [UnauthCons_OthersM3PerDay_J19]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_UnauthCons_OthersM3PerDay_J20]  DEFAULT ((0)) FOR [UnauthCons_OthersM3PerDay_J20]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_UnauthCons_OthersM3PerDay_J21]  DEFAULT ((0)) FOR [UnauthCons_OthersM3PerDay_J21]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_MetErrors_Total_F12]  DEFAULT ((0)) FOR [MetErrors_Total_F12]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_MetErrors_Total_F13]  DEFAULT ((0)) FOR [MetErrors_Total_F13]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_MetErrors_Total_F14]  DEFAULT ((0)) FOR [MetErrors_Total_F14]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_MetErrors_Total_F15]  DEFAULT ((0)) FOR [MetErrors_Total_F15]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_MetErrors_Meter_H12]  DEFAULT ((0)) FOR [MetErrors_Meter_H12]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_MetErrors_Meter_H13]  DEFAULT ((0)) FOR [MetErrors_Meter_H13]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_MetErrors_Meter_H14]  DEFAULT ((0)) FOR [MetErrors_Meter_H14]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_MetErrors_Meter_H15]  DEFAULT ((0)) FOR [MetErrors_Meter_H15]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_MetErrors_Error_N12]  DEFAULT ((0)) FOR [MetErrors_Error_N12]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_MetErrors_Error_N13]  DEFAULT ((0)) FOR [MetErrors_Error_N13]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_MetErrors_Error_N14]  DEFAULT ((0)) FOR [MetErrors_Error_N14]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_MetErrors_Error_N15]  DEFAULT ((0)) FOR [MetErrors_Error_N15]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_MeteredBulkSupplyExportErrorMargin_N32]  DEFAULT ((0)) FOR [MeteredBulkSupplyExportErrorMargin_N32]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_UnbilledMeteredConsumptionWithoutBulkSupplyErrorMargin_N34]  DEFAULT ((0)) FOR [UnbilledMeteredConsumptionWithoutBulkSupplyErrorMargin_N34]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_CorruptMeterReadingPracticessErrorMargin_N38]  DEFAULT ((0)) FOR [CorruptMeterReadingPracticessErrorMargin_N38]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_DataHandlingErrorsOffice_L40]  DEFAULT ((0)) FOR [DataHandlingErrorsOffice_L40]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_DataHandlingErrorsOfficeErrorMargin_N40]  DEFAULT ((0)) FOR [DataHandlingErrorsOfficeErrorMargin_N40]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_Network_DistributionAndTransmissionMains_D8]  DEFAULT ((0)) FOR [Network_DistributionAndTransmissionMains_D8]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_Network_DistributionAndTransmissionMains_D9]  DEFAULT ((0)) FOR [Network_DistributionAndTransmissionMains_D9]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_Network_DistributionAndTransmissionMains_D10]  DEFAULT ((0)) FOR [Network_DistributionAndTransmissionMains_D10]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_Network_PossibleUnd_D30]  DEFAULT ((0)) FOR [Network_PossibleUnd_D30]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_Network_NoCustomers_H7]  DEFAULT ((0)) FOR [Network_NoCustomers_H7]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_Network_ErrorMargin_J7]  DEFAULT ((0)) FOR [Network_ErrorMargin_J7]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_Network_ErrorMargin_J10]  DEFAULT ((0)) FOR [Network_ErrorMargin_J10]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_Network_ErrorMargin_J18]  DEFAULT ((0)) FOR [Network_ErrorMargin_J18]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_Network_ErrorMargin_J32]  DEFAULT ((0)) FOR [Network_ErrorMargin_J32]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_Network_ErrorMargin_D35]  DEFAULT ((0)) FOR [Network_ErrorMargin_D35]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_Prs_ApproxNoOfConn_D8]  DEFAULT ((0)) FOR [Prs_ApproxNoOfConn_D8]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_Prs_DailyAvgPrsM_F8]  DEFAULT ((0)) FOR [Prs_DailyAvgPrsM_F8]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_Prs_ApproxNoOfConn_D9]  DEFAULT ((0)) FOR [Prs_ApproxNoOfConn_D9]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_Prs_DailyAvgPrsM_F9]  DEFAULT ((0)) FOR [Prs_DailyAvgPrsM_F9]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_Prs_ApproxNoOfConn_D10]  DEFAULT ((0)) FOR [Prs_ApproxNoOfConn_D10]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_Prs_DailyAvgPrsM_F10]  DEFAULT ((0)) FOR [Prs_DailyAvgPrsM_F10]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_Prs_ErrorMarg_F26]  DEFAULT ((0)) FOR [Prs_ErrorMarg_F26]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_Interm_Conn_D7]  DEFAULT ((0)) FOR [Interm_Conn_D7]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_Interm_Conn_D8]  DEFAULT ((0)) FOR [Interm_Conn_D8]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_Interm_Conn_D9]  DEFAULT ((0)) FOR [Interm_Conn_D9]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_Interm_Conn_D10]  DEFAULT ((0)) FOR [Interm_Conn_D10]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_Interm_Days_F7]  DEFAULT ((0)) FOR [Interm_Days_F7]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_Interm_Days_F8]  DEFAULT ((0)) FOR [Interm_Days_F8]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_Interm_Days_F9]  DEFAULT ((0)) FOR [Interm_Days_F9]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_Interm_Days_F10]  DEFAULT ((0)) FOR [Interm_Days_F10]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_Interm_Hour_H7]  DEFAULT ((0)) FOR [Interm_Hour_H7]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_Interm_Hour_H8]  DEFAULT ((0)) FOR [Interm_Hour_H8]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_Interm_Hour_H9]  DEFAULT ((0)) FOR [Interm_Hour_H9]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_Interm_Hour_H10]  DEFAULT ((0)) FOR [Interm_Hour_H10]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_Interm_ErrorMarg_H26]  DEFAULT ((0)) FOR [Interm_ErrorMarg_H26]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_FinancData_G6]  DEFAULT ((0)) FOR [FinancData_G6]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_FinancData_K6]  DEFAULT ((0)) FOR [FinancData_K6]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_FinancData_G8]  DEFAULT ((0)) FOR [FinancData_G8]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_FinancData_D26]  DEFAULT ((0)) FOR [FinancData_D26]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_FinancData_G35]  DEFAULT ((0)) FOR [FinancData_G35]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_MatrixOneIn_SelectedOption]  DEFAULT ((1)) FOR [MatrixOneIn_SelectedOption]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_MatrixOneIn_C11]  DEFAULT ((0)) FOR [MatrixOneIn_C11]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] ADD  CONSTRAINT [DF_tbWbEasyCalcData_MatrixOneIn_C12]  DEFAULT ((0)) FOR [MatrixOneIn_C12]
GO
ALTER TABLE [dbo].[tbWaterConsumption]  WITH CHECK ADD  CONSTRAINT [FK_tbWaterConsumption_tbWaterConsumptionCategory] FOREIGN KEY([WaterConsumptionCategoryId])
REFERENCES [dbo].[tbWaterConsumptionCategory] ([WaterConsumptionCategoryId])
GO
ALTER TABLE [dbo].[tbWaterConsumption] CHECK CONSTRAINT [FK_tbWaterConsumption_tbWaterConsumptionCategory]
GO
ALTER TABLE [dbo].[tbWaterConsumption]  WITH CHECK ADD  CONSTRAINT [FK_tbWaterConsumption_tbWaterConsumptionStatus] FOREIGN KEY([WaterConsumptionStatusId])
REFERENCES [dbo].[tbWaterConsumptionStatus] ([WaterConsumptionStatusId])
GO
ALTER TABLE [dbo].[tbWaterConsumption] CHECK CONSTRAINT [FK_tbWaterConsumption_tbWaterConsumptionStatus]
GO
ALTER TABLE [dbo].[tbWaterConsumption]  WITH CHECK ADD  CONSTRAINT [FK_tbWaterConsumption_tbWbEasyCalcData] FOREIGN KEY([WbEasyCalcDataId])
REFERENCES [dbo].[tbWbEasyCalcData] ([WbEasyCalcDataId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[tbWaterConsumption] CHECK CONSTRAINT [FK_tbWaterConsumption_tbWbEasyCalcData]
GO
ALTER TABLE [dbo].[tbWaterConsumptionCategoryStatus]  WITH CHECK ADD  CONSTRAINT [FK_tbWaterConsumptionCategoryStatus_tbWaterConsumptionCategory] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[tbWaterConsumptionCategory] ([WaterConsumptionCategoryId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[tbWaterConsumptionCategoryStatus] CHECK CONSTRAINT [FK_tbWaterConsumptionCategoryStatus_tbWaterConsumptionCategory]
GO
ALTER TABLE [dbo].[tbWaterConsumptionCategoryStatus]  WITH CHECK ADD  CONSTRAINT [FK_tbWaterConsumptionCategoryStatus_tbWaterConsumptionCategoryStatusExcel] FOREIGN KEY([ExcelCellId])
REFERENCES [dbo].[tbWaterConsumptionCategoryStatusExcel] ([ExcelCellId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[tbWaterConsumptionCategoryStatus] CHECK CONSTRAINT [FK_tbWaterConsumptionCategoryStatus_tbWaterConsumptionCategoryStatusExcel]
GO
ALTER TABLE [dbo].[tbWaterConsumptionCategoryStatus]  WITH CHECK ADD  CONSTRAINT [FK_tbWaterConsumptionCategoryStatus_tbWaterConsumptionStatus] FOREIGN KEY([StatusId])
REFERENCES [dbo].[tbWaterConsumptionStatus] ([WaterConsumptionStatusId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[tbWaterConsumptionCategoryStatus] CHECK CONSTRAINT [FK_tbWaterConsumptionCategoryStatus_tbWaterConsumptionStatus]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData]  WITH CHECK ADD  CONSTRAINT [FK_tbWbEasyCalcData_tbMonth] FOREIGN KEY([MonthNo])
REFERENCES [dbo].[tbMonth] ([MonthId])
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] CHECK CONSTRAINT [FK_tbWbEasyCalcData_tbMonth]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData]  WITH CHECK ADD  CONSTRAINT [FK_tbWbEasyCalcData_tbYear] FOREIGN KEY([YearNo])
REFERENCES [dbo].[tbYear] ([YearId])
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] CHECK CONSTRAINT [FK_tbWbEasyCalcData_tbYear]
GO
ALTER TABLE [dbo].[tbWbEasyCalcData]  WITH CHECK ADD  CONSTRAINT [FK_tbWbEasyCalcData_WaterBalanceConfig] FOREIGN KEY([ZoneId])
REFERENCES [config].[WaterBalanceConfig] ([ModelZoneId])
GO
ALTER TABLE [dbo].[tbWbEasyCalcData] CHECK CONSTRAINT [FK_tbWbEasyCalcData_WaterBalanceConfig]
GO
/****** Object:  StoredProcedure [dbo].[spGisModelScadaData]    Script Date: 26.05.2021 09:39:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spGisModelScadaData] 
	@YearNo INT,
	@MonthNo INT,
	@ZoneId INT,

	@Start_PeriodDays_M21 INT OUTPUT,

	@SysInput_SystemInputVolumeM3_D6 FLOAT OUTPUT,						-- SCADA - [SystemInputVolume SUM(S(n)_SREDN_10M)]
	@SysInput_SystemInputVolumeError_F6 FLOAT OUTPUT,					-- percent
	@SysInput_SystemInputVolumeM3_D7 FLOAT OUTPUT,
	@SysInput_SystemInputVolumeError_F7 FLOAT OUTPUT,
	@SysInput_SystemInputVolumeM3_D8 FLOAT OUTPUT,
	@SysInput_SystemInputVolumeError_F8 FLOAT OUTPUT,
	@SysInput_SystemInputVolumeM3_D9 FLOAT OUTPUT,
	@SysInput_SystemInputVolumeError_F9 FLOAT OUTPUT,

	@BilledCons_BilledMetConsBulkWatSupExpM3_D6 FLOAT OUTPUT,			
	@BilledCons_BilledUnmetConsBulkWatSupExpM3_H6 FLOAT OUTPUT,
	@BilledCons_UnbMetConsM3_D8 FLOAT OUTPUT,							-- GIS - [sprzedaz_w_strefie]
	@BilledCons_UnbUnmetConsM3_H8 FLOAT OUTPUT,

	@UnbilledCons_MetConsBulkWatSupExpM3_D6 FLOAT OUTPUT,	
	@UnbilledCons_UnbMetConsM3_D8 FLOAT OUTPUT,
	@UnbilledCons_UnbUnmetConsM3_H6 FLOAT OUTPUT,
	@UnbilledCons_UnbUnmetConsError_J6 FLOAT OUTPUT,

	@UnauthCons_IllegalConnDomEstNo_D6 INT OUTPUT,
	@UnauthCons_IllegalConnDomPersPerHouse_H6 FLOAT OUTPUT,
	@UnauthCons_IllegalConnDomConsLitPerPersDay_J6 FLOAT OUTPUT, 
	@UnauthCons_IllegalConnDomErrorMargin_F6 FLOAT OUTPUT,				-- percent
	@UnauthCons_IllegalConnOthersErrorMargin_F10 FLOAT OUTPUT,			-- percent
	@IllegalConnectionsOthersEstimatedNumber_D10 FLOAT OUTPUT,							
	@IllegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10 FLOAT OUTPUT,		
	@UnauthCons_MeterTampBypEtcEstNo_D14 FLOAT OUTPUT,
	@UnauthCons_MeterTampBypEtcErrorMargin_F14 FLOAT OUTPUT,			-- percent
	@UnauthCons_MeterTampBypEtcConsLitPerCustDay_J14 FLOAT OUTPUT,	
	
	@MetErrors_DetailedManualSpec_J6 INT OUTPUT,
	@MetErrors_BilledMetConsWoBulkSupMetUndrreg_H8 FLOAT OUTPUT,		-- percent
	@MetErrors_BilledMetConsWoBulkSupErrorMargin_N8 FLOAT OUTPUT,		-- percent		
	@MeteredBulkSupplyExportErrorMargin_N32 FLOAT OUTPUT,
	@UnbilledMeteredConsumptionWithoutBulkSupplyErrorMargin_N34 FLOAT OUTPUT,
	@CorruptMeterReadingPracticessErrorMargin_N38 FLOAT OUTPUT,
	@DataHandlingErrorsOffice_L40 FLOAT OUTPUT,
	@DataHandlingErrorsOfficeErrorMargin_N40 FLOAT OUTPUT,		
	@MetErrors_MetBulkSupExpMetUnderreg_H32 FLOAT OUTPUT,				-- percent
	@MetErrors_UnbillMetConsWoBulkSupplMetUndrreg_H34 FLOAT OUTPUT,		-- percent
	@MetErrors_CorruptMetReadPractMetUndrreg_H38 FLOAT OUTPUT,			-- percent
	
	@Network_DistributionAndTransmissionMains_D7 FLOAT OUTPUT,			-- Model - [dlugosc_sieci]
	@Network_NoCustomers_H7 FLOAT OUTPUT,								-- Model - CustomerMeterQuantity
	@Network_NoOfConnOfRegCustomers_H10 FLOAT OUTPUT,					-- GIS - [ilosc_przylaczy]
	@Network_NoOfInactAccountsWSvcConns_H18 FLOAT OUTPUT,				-- GIS
	@Network_AvgLenOfSvcConnFromBoundaryToMeterM_H32 FLOAT OUTPUT,
	
	@Prs_ApproxNoOfConn_D7 FLOAT OUTPUT,
	@Prs_DailyAvgPrsM_F7 FLOAT OUTPUT,									-- SCADA - AveragePressure
	@Prs_ApproxNoOfConn_D8 FLOAT OUTPUT,
	@Prs_DailyAvgPrsM_F8 FLOAT OUTPUT,
	@Prs_ApproxNoOfConn_D9 FLOAT OUTPUT,
	@Prs_DailyAvgPrsM_F9 FLOAT OUTPUT,
	@Prs_ApproxNoOfConn_D10 FLOAT OUTPUT,
	@Prs_DailyAvgPrsM_F10 FLOAT OUTPUT,
	@Prs_ErrorMarg_F26 FLOAT OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE
		@StartDate DATETIME,
		@EndDate DATETIME,
		@ZonePrefix NVARCHAR(50);
 
	SELECT
		@ZonePrefix = [DestinationName]
	FROM
		[config].[WaterBalanceConfig]
	WHERE
		[ZoneNo] = @ZoneId;		-- 'SI'

	IF (@MonthNo < 13) BEGIN
		SELECT @StartDate = CONCAT(@YearNo, '-', FORMAT(@MonthNo, '00'), '-01');
		SELECT @EndDate = EOMONTH(@StartDate);
	END ELSE BEGIN
		SELECT @StartDate = CONCAT(@YearNo, '-01-01');
		SELECT @EndDate = CONCAT(@YearNo, '-12-31');
	END;

	SELECT 
		 @Start_PeriodDays_M21 = 0

		,@SysInput_SystemInputVolumeM3_D6 = 0								-- SystemInputVolume
		,@SysInput_SystemInputVolumeError_F6 = 0
		,@SysInput_SystemInputVolumeM3_D7 = 0
		,@SysInput_SystemInputVolumeError_F7 = 0
		,@SysInput_SystemInputVolumeM3_D8 = 0
		,@SysInput_SystemInputVolumeError_F8 = 0
		,@SysInput_SystemInputVolumeM3_D9 = 0
		,@SysInput_SystemInputVolumeError_F9 = 0

		,@BilledCons_BilledMetConsBulkWatSupExpM3_D6 = 0					-- sprzedaz_w_strefie
		,@BilledCons_BilledUnmetConsBulkWatSupExpM3_H6 = 0
		,@BilledCons_UnbMetConsM3_D8 = 0
		,@BilledCons_UnbUnmetConsM3_H8 = 0

		,@UnbilledCons_MetConsBulkWatSupExpM3_D6 = 0
		,@UnbilledCons_UnbMetConsM3_D8 = 0
		,@UnbilledCons_UnbUnmetConsM3_H6 = 0
		,@UnbilledCons_UnbUnmetConsError_J6 = 0

		,@UnauthCons_IllegalConnDomEstNo_D6 = 0
		,@UnauthCons_IllegalConnDomPersPerHouse_H6 = 0
		,@UnauthCons_IllegalConnDomConsLitPerPersDay_J6 = 0
		,@UnauthCons_IllegalConnDomErrorMargin_F6 = 0
		,@UnauthCons_IllegalConnOthersErrorMargin_F10 = 0
		,@IllegalConnectionsOthersEstimatedNumber_D10 = 0
		,@IllegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10 = 0
		,@UnauthCons_MeterTampBypEtcEstNo_D14 = 0
		,@UnauthCons_MeterTampBypEtcErrorMargin_F14 = 0
		,@UnauthCons_MeterTampBypEtcConsLitPerCustDay_J14 = 0

		,@MetErrors_DetailedManualSpec_J6 = 0								-- (t02.[MetErrors_DetailedManualSpec_J6])
		,@MetErrors_BilledMetConsWoBulkSupMetUndrreg_H8 = 0
		,@MetErrors_BilledMetConsWoBulkSupErrorMargin_N8 = 0
		,@MeteredBulkSupplyExportErrorMargin_N32 = 0
		,@UnbilledMeteredConsumptionWithoutBulkSupplyErrorMargin_N34 = 0
		,@CorruptMeterReadingPracticessErrorMargin_N38 = 0
		,@DataHandlingErrorsOffice_L40 = 0
		,@DataHandlingErrorsOfficeErrorMargin_N40 = 0
		,@MetErrors_MetBulkSupExpMetUnderreg_H32 = 0
		,@MetErrors_UnbillMetConsWoBulkSupplMetUndrreg_H34 = 0
		,@MetErrors_CorruptMetReadPractMetUndrreg_H38 = 0

		,@Network_DistributionAndTransmissionMains_D7 = 0					-- Model: dlugosc_sieci
		,@Network_NoCustomers_H7 = 0										-- Model: CustomerMetersQuantity
		,@Network_NoOfConnOfRegCustomers_H10 = 0							-- ilosc przylaczy aktywnych
		,@Network_NoOfInactAccountsWSvcConns_H18 = 0						-- ilosc przylaczy nieaktywnych
		,@Network_AvgLenOfSvcConnFromBoundaryToMeterM_H32 = 0

		,@Prs_ApproxNoOfConn_D7 = 0
		,@Prs_DailyAvgPrsM_F7 = 0											-- 
		,@Prs_ApproxNoOfConn_D8 = 0
		,@Prs_DailyAvgPrsM_F8 = 0
		,@Prs_ApproxNoOfConn_D9 = 0
		,@Prs_DailyAvgPrsM_F9 = 0
		,@Prs_ApproxNoOfConn_D10 = 0
		,@Prs_DailyAvgPrsM_F10 = 0
		,@Prs_ErrorMarg_F26 = 0
		;

	IF (@MonthNo < 13) BEGIN

		-- GIS -----------------------------------------------------------------------
		SELECT 
			@BilledCons_UnbMetConsM3_D8 = sprzedaz_w_strefie,
			@Network_NoOfConnOfRegCustomers_H10 = ilosc_przylaczy,
			@Network_NoOfInactAccountsWSvcConns_H18 = ilosc_przylaczy_czynnych - ilosc_przylaczy
		FROM 
			[dbo].[vwGisWaterConsumption] vCons
			INNER JOIN config.WaterBalanceConfig tConf ON vCons.id_strefy = tConf.GisZoneId
		WHERE 
			[year] = @YearNo 
			AND [month] = @MonthNo 
			AND tConf.ModelZoneId = @ZoneId;

		-- WaterGEMS ------------------------------------------------------------------
		SELECT 
			@Network_DistributionAndTransmissionMains_D7 = WaterInfra.[dbo].[fnPipeLenghtInZoneSum](@ZoneId),
			@Network_NoCustomers_H7 = WaterInfra.[dbo].[fnCustomerMeterInZoneCount](@ZoneId);

		-- SCADA ----------------------------------------------------------------------
		SELECT 
			@SysInput_SystemInputVolumeM3_D6 = TWDB.[dbo].[fnWaterConsumptionForZone](@YearNo, @MonthNo, @ZoneId);

		-- SCADA & WaterGEMS ----------------------------------------------------------
		SELECT 
			@Prs_DailyAvgPrsM_F7 = COALESCE([dbo].[fnScadaPressureForZoneAvg](@YearNo, @MonthNo, @ZoneId), 0);


	END ELSE BEGIN

		-- GIS
		--SELECT 
		--	@BilledCons_BilledMetConsBulkWatSupExpM3_D6 = SUM([sprzedaz_w_strefie]),
		--	@Network_DistributionAndTransmissionMains_D7 = AVG([dlugosc_sieci]),     
		--	@Network_NoOfConnOfRegCustomers_H10 = AVG([ilosc_przylaczy])  
		--FROM 
		--	[dbo].[vw_zuzycia_stref_2] 
		--WHERE 
		--	[year] = @YearNo 
		--	AND ZoneId = @ZoneId;

		-- @SysInput_SystemInputVolumeM3_D6
		SELECT 
			 @Start_PeriodDays_M21 = SUM(t02.[Start_PeriodDays_M21])

			,@SysInput_SystemInputVolumeM3_D6 = SUM(t02.[SysInput_SystemInputVolumeM3_D6])
			,@SysInput_SystemInputVolumeError_F6 = AVG(t02.[SysInput_SystemInputVolumeError_F6])
			,@SysInput_SystemInputVolumeM3_D7 = 0
			,@SysInput_SystemInputVolumeError_F7 = 0
			,@SysInput_SystemInputVolumeM3_D8 = 0
			,@SysInput_SystemInputVolumeError_F8 = 0
			,@SysInput_SystemInputVolumeM3_D9 = 0
			,@SysInput_SystemInputVolumeError_F9 = 0

			,@BilledCons_BilledMetConsBulkWatSupExpM3_D6 = SUM(t02.[BilledCons_BilledMetConsBulkWatSupExpM3_D6])					-- sprzedaz_w_strefie
			,@BilledCons_BilledUnmetConsBulkWatSupExpM3_H6 = SUM(t02.[BilledCons_BilledUnmetConsBulkWatSupExpM3_H6])			
			,@BilledCons_UnbMetConsM3_D8 = SUM(t02.[BilledCons_UnbMetConsM3_D8])
			,@BilledCons_UnbUnmetConsM3_H8 = SUM(t02.[BilledCons_UnbUnmetConsM3_H8])
			
			,@UnbilledCons_MetConsBulkWatSupExpM3_D6 = SUM(t02.[UnbilledCons_MetConsBulkWatSupExpM3_D6])
			,@UnbilledCons_UnbMetConsM3_D8 = SUM(t02.[UnbilledCons_UnbMetConsM3_D8])
			,@UnbilledCons_UnbUnmetConsM3_H6 = SUM(t02.[UnbilledCons_UnbUnmetConsM3_H6])
			,@UnbilledCons_UnbUnmetConsError_J6 = AVG(t02.[UnbilledCons_UnbUnmetConsError_J6])

			,@UnauthCons_IllegalConnDomEstNo_D6 = SUM(t02.[UnauthCons_IllegalConnDomEstNo_D6])
			,@UnauthCons_IllegalConnDomPersPerHouse_H6 = SUM(t02.[UnauthCons_IllegalConnDomPersPerHouse_H6])
			,@UnauthCons_IllegalConnDomConsLitPerPersDay_J6 = SUM(t02.[UnauthCons_IllegalConnDomConsLitPerPersDay_J6])
			,@UnauthCons_IllegalConnDomErrorMargin_F6 = AVG(t02.[UnauthCons_IllegalConnDomErrorMargin_F6])
			,@UnauthCons_IllegalConnOthersErrorMargin_F10 = AVG(t02.[UnauthCons_IllegalConnOthersErrorMargin_F10])
			,@IllegalConnectionsOthersEstimatedNumber_D10 = SUM(t02.[IllegalConnectionsOthersEstimatedNumber_D10])
			,@IllegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10 = SUM(t02.[IllegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10])
			,@UnauthCons_MeterTampBypEtcEstNo_D14 = SUM(t02.[UnauthCons_MeterTampBypEtcEstNo_D14])
			,@UnauthCons_MeterTampBypEtcErrorMargin_F14 = AVG(t02.[UnauthCons_MeterTampBypEtcErrorMargin_F14])
			,@UnauthCons_MeterTampBypEtcConsLitPerCustDay_J14 = SUM(t02.[UnauthCons_MeterTampBypEtcConsLitPerCustDay_J14])

			,@MetErrors_DetailedManualSpec_J6 = 2																					-- (t02.[MetErrors_DetailedManualSpec_J6])
			,@MetErrors_BilledMetConsWoBulkSupMetUndrreg_H8 = AVG(t02.[MetErrors_BilledMetConsWoBulkSupMetUndrreg_H8])
			,@MetErrors_BilledMetConsWoBulkSupErrorMargin_N8 = AVG(t02.[MetErrors_BilledMetConsWoBulkSupErrorMargin_N8])
			,@MeteredBulkSupplyExportErrorMargin_N32 = AVG(t02.[MeteredBulkSupplyExportErrorMargin_N32])
			,@UnbilledMeteredConsumptionWithoutBulkSupplyErrorMargin_N34 = AVG(t02.[UnbilledMeteredConsumptionWithoutBulkSupplyErrorMargin_N34])
			,@CorruptMeterReadingPracticessErrorMargin_N38 = AVG(t02.[CorruptMeterReadingPracticessErrorMargin_N38])
			,@DataHandlingErrorsOffice_L40 = SUM(t02.[DataHandlingErrorsOffice_L40])
			,@DataHandlingErrorsOfficeErrorMargin_N40 = AVG(t02.[DataHandlingErrorsOfficeErrorMargin_N40])
			,@MetErrors_MetBulkSupExpMetUnderreg_H32 = AVG(t02.[MetErrors_MetBulkSupExpMetUnderreg_H32])
			,@MetErrors_UnbillMetConsWoBulkSupplMetUndrreg_H34 = AVG(t02.[MetErrors_UnbillMetConsWoBulkSupplMetUndrreg_H34])
			,@MetErrors_CorruptMetReadPractMetUndrreg_H38 = AVG(t02.[MetErrors_CorruptMetReadPractMetUndrreg_H38])

			,@Network_DistributionAndTransmissionMains_D7 = AVG(t02.[Network_DistributionAndTransmissionMains_D7])					-- dlugosc_sieci
			,@Network_NoCustomers_H7 = AVG(t02.[Network_NoCustomers_H7])															-- CustomerMetersQuantity
			,@Network_NoOfConnOfRegCustomers_H10 = AVG(t02.[Network_NoOfConnOfRegCustomers_H10])									-- ilosc_przylaczy
			,@Network_NoOfInactAccountsWSvcConns_H18 = AVG(t02.[Network_NoOfInactAccountsWSvcConns_H18])
			,@Network_AvgLenOfSvcConnFromBoundaryToMeterM_H32 = AVG(t02.[Network_AvgLenOfSvcConnFromBoundaryToMeterM_H32])

			,@Prs_ApproxNoOfConn_D7 = AVG(t02.[Prs_ApproxNoOfConn_D7])
			,@Prs_DailyAvgPrsM_F7 = AVG(t02.[Prs_DailyAvgPrsM_F7])
			,@Prs_ApproxNoOfConn_D8 = 0
			,@Prs_DailyAvgPrsM_F8 = 0
			,@Prs_ApproxNoOfConn_D9 = 0
			,@Prs_DailyAvgPrsM_F9 = 0
			,@Prs_ApproxNoOfConn_D10 = 0
			,@Prs_DailyAvgPrsM_F10 = 0
			,@Prs_ErrorMarg_F26 = AVG(t02.[Prs_ErrorMarg_F26])
		FROM 
			dbo.tbWbEasyCalcData t02 
		WHERE
			t02.YearNo = @YearNo
			AND t02.ZoneId = @ZoneId
			AND t02.IsArchive = 1
			;	

	END;
END
GO
/****** Object:  StoredProcedure [dbo].[spMonthList]    Script Date: 26.05.2021 09:39:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create PROCEDURE [dbo].[spMonthList] 
AS
BEGIN
	SET NOCOUNT ON;

SELECT 
    MonthId AS Id,
	[MonthName] AS [Name]
FROM
	tbMonth;


END
GO
/****** Object:  StoredProcedure [dbo].[spOptionGet]    Script Date: 26.05.2021 09:39:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create PROCEDURE [dbo].[spOptionGet] 
AS
BEGIN
	SET NOCOUNT ON;

WITH 
t01 AS (
SELECT
       [Ili]	AS MatrixOneIn_C11
      ,[Lcd10]	AS MatrixOneIn_D11
      ,[Lcd20]	AS MatrixOneIn_E11 
      ,[Lcd30]	AS MatrixOneIn_F11 
      ,[Lcd40]	AS MatrixOneIn_G11 
      ,[Lcd50]	AS MatrixOneIn_H11 
FROM 
	[dbo].[tbConfigMatrix] 
WHERE
	ConfigId = 1
)
,t02 AS (
SELECT
       [Ili]	AS MatrixOneIn_C12
      ,[Lcd10]	AS MatrixOneIn_D12
      ,[Lcd20]	AS MatrixOneIn_E12 
      ,[Lcd30]	AS MatrixOneIn_F12 
      ,[Lcd40]	AS MatrixOneIn_G12 
      ,[Lcd50]	AS MatrixOneIn_H12 
FROM 
	[dbo].[tbConfigMatrix] 
WHERE
	ConfigId = 2
)
,t03 AS (
SELECT
       [Ili]	AS MatrixOneIn_C13
      ,[Lcd10]	AS MatrixOneIn_D13
      ,[Lcd20]	AS MatrixOneIn_E13 
      ,[Lcd30]	AS MatrixOneIn_F13 
      ,[Lcd40]	AS MatrixOneIn_G13 
      ,[Lcd50]	AS MatrixOneIn_H13 
FROM 
	[dbo].[tbConfigMatrix] 
WHERE
	ConfigId = 3
)
,t04 AS (
SELECT
       [Ili]	AS MatrixOneIn_C14
      ,[Lcd10]	AS MatrixOneIn_D14
      ,[Lcd20]	AS MatrixOneIn_E14 
      ,[Lcd30]	AS MatrixOneIn_F14 
      ,[Lcd40]	AS MatrixOneIn_G14 
      ,[Lcd50]	AS MatrixOneIn_H14 
FROM 
	[dbo].[tbConfigMatrix] 
WHERE
	ConfigId = 4
)
,t06 AS (
SELECT
       [Ili]	AS MatrixOneIn_C21
      ,[Lcd10]	AS MatrixOneIn_D21
      ,[Lcd20]	AS MatrixOneIn_E21 
      ,[Lcd30]	AS MatrixOneIn_F21 
      ,[Lcd40]	AS MatrixOneIn_G21 
      ,[Lcd50]	AS MatrixOneIn_H21 
FROM 
	[dbo].[tbConfigMatrix] 
WHERE
	ConfigId = 6
)
,t07 AS (
SELECT
       [Ili]	AS MatrixOneIn_C22
      ,[Lcd10]	AS MatrixOneIn_D22
      ,[Lcd20]	AS MatrixOneIn_E22 
      ,[Lcd30]	AS MatrixOneIn_F22 
      ,[Lcd40]	AS MatrixOneIn_G22 
      ,[Lcd50]	AS MatrixOneIn_H22 
FROM 
	[dbo].[tbConfigMatrix] 
WHERE
	ConfigId = 7
)
,t08 AS (
SELECT
       [Ili]	AS MatrixOneIn_C23
      ,[Lcd10]	AS MatrixOneIn_D23
      ,[Lcd20]	AS MatrixOneIn_E23 
      ,[Lcd30]	AS MatrixOneIn_F23 
      ,[Lcd40]	AS MatrixOneIn_G23 
      ,[Lcd50]	AS MatrixOneIn_H23 
FROM 
	[dbo].[tbConfigMatrix] 
WHERE
	ConfigId = 8
)
,t09 AS (
SELECT
       [Ili]	AS MatrixOneIn_C24
      ,[Lcd10]	AS MatrixOneIn_D24
      ,[Lcd20]	AS MatrixOneIn_E24 
      ,[Lcd30]	AS MatrixOneIn_F24 
      ,[Lcd40]	AS MatrixOneIn_G24 
      ,[Lcd50]	AS MatrixOneIn_H24 
FROM 
	[dbo].[tbConfigMatrix] 
WHERE
	ConfigId = 9
)
SELECT 
	* 
FROM
	t01 
	CROSS JOIN t02
	CROSS JOIN t03
	CROSS JOIN t04
	CROSS JOIN t06
	CROSS JOIN t07
	CROSS JOIN t08
	CROSS JOIN t09
	;
END
GO
/****** Object:  StoredProcedure [dbo].[spSettingGet]    Script Date: 26.05.2021 09:39:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spSettingGet] 
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * FROM dbo.tbSetting;
END
GO
/****** Object:  StoredProcedure [dbo].[spSettingSave]    Script Date: 26.05.2021 09:39:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spSettingSave]

	@FinancData_G6 [float],
	@FinancData_K6 nvarchar(400),
	@FinancData_G8 [float],
	--@FinancData_D26 [float], 
	--@FinancData_G35 [float], 

	@MatrixOneIn_SelectedOption [int], 
	@MatrixOneIn_C11 [float], 
	@MatrixOneIn_C12 [float],
	@MatrixOneIn_C13 [float],
	@MatrixOneIn_C14 [float],
	@MatrixOneIn_C21 [float],
	@MatrixOneIn_C22 [float],
	@MatrixOneIn_C23 [float],
	@MatrixOneIn_C24 [float],
	@MatrixOneIn_D21 [float],
	@MatrixOneIn_D22 [float],
	@MatrixOneIn_D23 [float],
	@MatrixOneIn_D24 [float],
	@MatrixOneIn_E11 [float],
	@MatrixOneIn_E12 [float],
	@MatrixOneIn_E13 [float],
	@MatrixOneIn_E14 [float],
	@MatrixOneIn_E21 [float],
	@MatrixOneIn_E22 [float],
	@MatrixOneIn_E23 [float],
	@MatrixOneIn_E24 [float],
	@MatrixOneIn_F11 [float],
	@MatrixOneIn_F12 [float],
	@MatrixOneIn_F13 [float],
	@MatrixOneIn_F14 [float],
	@MatrixOneIn_F21 [float],
	@MatrixOneIn_F22 [float],
	@MatrixOneIn_F23 [float],
	@MatrixOneIn_F24 [float],
	@MatrixOneIn_G11 [float],
	@MatrixOneIn_G12 [float],
	@MatrixOneIn_G13 [float],
	@MatrixOneIn_G14 [float],
	@MatrixOneIn_G21 [float],
	@MatrixOneIn_G22 [float],
	@MatrixOneIn_G23 [float],
	@MatrixOneIn_G24 [float],
	@MatrixOneIn_H11 [float],
	@MatrixOneIn_H12 [float],
	@MatrixOneIn_H13 [float],
	@MatrixOneIn_H14 [float],
	@MatrixOneIn_H21 [float],
	@MatrixOneIn_H22 [float],
	@MatrixOneIn_H23 [float],
	@MatrixOneIn_H24 [float],

	@MatrixTwoIn_SelectedOption [int], 
	@MatrixTwoIn_C11 [float], 
	@MatrixTwoIn_C12 [float],
	@MatrixTwoIn_C13 [float],
	@MatrixTwoIn_C14 [float],
	@MatrixTwoIn_C21 [float],
	@MatrixTwoIn_C22 [float],
	@MatrixTwoIn_C23 [float],
	@MatrixTwoIn_C24 [float],
	@MatrixTwoIn_D21 [float],
	@MatrixTwoIn_D22 [float],
	@MatrixTwoIn_D23 [float],
	@MatrixTwoIn_D24 [float],
	@MatrixTwoIn_E11 [float],
	@MatrixTwoIn_E12 [float],
	@MatrixTwoIn_E13 [float],
	@MatrixTwoIn_E14 [float],
	@MatrixTwoIn_E21 [float],
	@MatrixTwoIn_E22 [float],
	@MatrixTwoIn_E23 [float],
	@MatrixTwoIn_E24 [float],
	@MatrixTwoIn_F11 [float],
	@MatrixTwoIn_F12 [float],
	@MatrixTwoIn_F13 [float],
	@MatrixTwoIn_F14 [float],
	@MatrixTwoIn_F21 [float],
	@MatrixTwoIn_F22 [float],
	@MatrixTwoIn_F23 [float],
	@MatrixTwoIn_F24 [float],
	@MatrixTwoIn_G11 [float],
	@MatrixTwoIn_G12 [float],
	@MatrixTwoIn_G13 [float],
	@MatrixTwoIn_G14 [float],
	@MatrixTwoIn_G21 [float],
	@MatrixTwoIn_G22 [float],
	@MatrixTwoIn_G23 [float],
	@MatrixTwoIn_G24 [float],
	@MatrixTwoIn_H11 [float],
	@MatrixTwoIn_H12 [float],
	@MatrixTwoIn_H13 [float],
	@MatrixTwoIn_H14 [float],
	@MatrixTwoIn_H21 [float],
	@MatrixTwoIn_H22 [float],
	@MatrixTwoIn_H23 [float],
	@MatrixTwoIn_H24 [float]

AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[tbSetting] SET

		FinancData_G6 = @FinancData_G6,
		FinancData_K6 = @FinancData_K6,
		FinancData_G8 = @FinancData_G8,
		--FinancData_D26 = @FinancData_D26,
		--FinancData_G35 = @FinancData_G35,

		MatrixOneIn_SelectedOption = @MatrixOneIn_SelectedOption,
		MatrixOneIn_C11 = @MatrixOneIn_C11,
        MatrixOneIn_C12 = @MatrixOneIn_C12,
        MatrixOneIn_C13 = @MatrixOneIn_C13,
        MatrixOneIn_C14 = @MatrixOneIn_C14,
        MatrixOneIn_C21 = @MatrixOneIn_C21,
        MatrixOneIn_C22 = @MatrixOneIn_C22,
        MatrixOneIn_C23 = @MatrixOneIn_C23,
        MatrixOneIn_C24 = @MatrixOneIn_C24,
        MatrixOneIn_D21 = @MatrixOneIn_D21,
        MatrixOneIn_D22 = @MatrixOneIn_D22,
        MatrixOneIn_D23 = @MatrixOneIn_D23,
        MatrixOneIn_D24 = @MatrixOneIn_D24,
        MatrixOneIn_E11 = @MatrixOneIn_E11,
        MatrixOneIn_E12 = @MatrixOneIn_E12,
        MatrixOneIn_E13 = @MatrixOneIn_E13,
        MatrixOneIn_E14 = @MatrixOneIn_E14,
        MatrixOneIn_E21 = @MatrixOneIn_E21,
        MatrixOneIn_E22 = @MatrixOneIn_E22,
        MatrixOneIn_E23 = @MatrixOneIn_E23,
        MatrixOneIn_E24 = @MatrixOneIn_E24,
        MatrixOneIn_F11 = @MatrixOneIn_F11,
        MatrixOneIn_F12 = @MatrixOneIn_F12,
        MatrixOneIn_F13 = @MatrixOneIn_F13,
        MatrixOneIn_F14 = @MatrixOneIn_F14,
        MatrixOneIn_F21 = @MatrixOneIn_F21,
        MatrixOneIn_F22 = @MatrixOneIn_F22,
        MatrixOneIn_F23 = @MatrixOneIn_F23,
        MatrixOneIn_F24 = @MatrixOneIn_F24,
        MatrixOneIn_G11 = @MatrixOneIn_G11,
        MatrixOneIn_G12 = @MatrixOneIn_G12,
        MatrixOneIn_G13 = @MatrixOneIn_G13,
        MatrixOneIn_G14 = @MatrixOneIn_G14,
        MatrixOneIn_G21 = @MatrixOneIn_G21,
        MatrixOneIn_G22 = @MatrixOneIn_G22,
        MatrixOneIn_G23 = @MatrixOneIn_G23,
        MatrixOneIn_G24 = @MatrixOneIn_G24,
        MatrixOneIn_H11 = @MatrixOneIn_H11,
        MatrixOneIn_H12 = @MatrixOneIn_H12,
        MatrixOneIn_H13 = @MatrixOneIn_H13,
        MatrixOneIn_H14 = @MatrixOneIn_H14,
        MatrixOneIn_H21 = @MatrixOneIn_H21,
        MatrixOneIn_H22 = @MatrixOneIn_H22,
        MatrixOneIn_H23 = @MatrixOneIn_H23,
        MatrixOneIn_H24 = @MatrixOneIn_H24,

		MatrixTwoIn_SelectedOption = @MatrixTwoIn_SelectedOption,
		MatrixTwoIn_C11 = @MatrixTwoIn_C11,
        MatrixTwoIn_C12 = @MatrixTwoIn_C12,
        MatrixTwoIn_C13 = @MatrixTwoIn_C13,
        MatrixTwoIn_C14 = @MatrixTwoIn_C14,
        MatrixTwoIn_C21 = @MatrixTwoIn_C21,
        MatrixTwoIn_C22 = @MatrixTwoIn_C22,
        MatrixTwoIn_C23 = @MatrixTwoIn_C23,
        MatrixTwoIn_C24 = @MatrixTwoIn_C24,
        MatrixTwoIn_D21 = @MatrixTwoIn_D21,
        MatrixTwoIn_D22 = @MatrixTwoIn_D22,
        MatrixTwoIn_D23 = @MatrixTwoIn_D23,
        MatrixTwoIn_D24 = @MatrixTwoIn_D24,
        MatrixTwoIn_E11 = @MatrixTwoIn_E11,
        MatrixTwoIn_E12 = @MatrixTwoIn_E12,
        MatrixTwoIn_E13 = @MatrixTwoIn_E13,
        MatrixTwoIn_E14 = @MatrixTwoIn_E14,
        MatrixTwoIn_E21 = @MatrixTwoIn_E21,
        MatrixTwoIn_E22 = @MatrixTwoIn_E22,
        MatrixTwoIn_E23 = @MatrixTwoIn_E23,
        MatrixTwoIn_E24 = @MatrixTwoIn_E24,
        MatrixTwoIn_F11 = @MatrixTwoIn_F11,
        MatrixTwoIn_F12 = @MatrixTwoIn_F12,
        MatrixTwoIn_F13 = @MatrixTwoIn_F13,
        MatrixTwoIn_F14 = @MatrixTwoIn_F14,
        MatrixTwoIn_F21 = @MatrixTwoIn_F21,
        MatrixTwoIn_F22 = @MatrixTwoIn_F22,
        MatrixTwoIn_F23 = @MatrixTwoIn_F23,
        MatrixTwoIn_F24 = @MatrixTwoIn_F24,
        MatrixTwoIn_G11 = @MatrixTwoIn_G11,
        MatrixTwoIn_G12 = @MatrixTwoIn_G12,
        MatrixTwoIn_G13 = @MatrixTwoIn_G13,
        MatrixTwoIn_G14 = @MatrixTwoIn_G14,
        MatrixTwoIn_G21 = @MatrixTwoIn_G21,
        MatrixTwoIn_G22 = @MatrixTwoIn_G22,
        MatrixTwoIn_G23 = @MatrixTwoIn_G23,
        MatrixTwoIn_G24 = @MatrixTwoIn_G24,
        MatrixTwoIn_H11 = @MatrixTwoIn_H11,
        MatrixTwoIn_H12 = @MatrixTwoIn_H12,
        MatrixTwoIn_H13 = @MatrixTwoIn_H13,
        MatrixTwoIn_H14 = @MatrixTwoIn_H14,
        MatrixTwoIn_H21 = @MatrixTwoIn_H21,
        MatrixTwoIn_H22 = @MatrixTwoIn_H22,
        MatrixTwoIn_H23 = @MatrixTwoIn_H23,
        MatrixTwoIn_H24 = @MatrixTwoIn_H24

END;



GO
/****** Object:  StoredProcedure [dbo].[spUnpivotTable]    Script Date: 26.05.2021 09:39:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[spUnpivotTable] 
	@IdValue INT = 1,
	@OrdinalPosition INT = 8,
	@TableName NVARCHAR(4000) = 'tbWbEasyCalcData',
	@IdColumnName NVARCHAR(4000) = 'WbEasyCalcDataId',
	@CastType NVARCHAR(4000) = 'FLOAT'
AS
BEGIN
SET NOCOUNT ON;

DECLARE
	@Part1 NVARCHAR(MAX),
	@Part2 NVARCHAR(MAX),
	@SqlCmd NVARCHAR(MAX);

SELECT 
	@Part1 = STRING_AGG(CONVERT(NVARCHAR(max), CONCAT('CAST(', [COLUMN_NAME], ' AS ', @CastType, ') AS ', [COLUMN_NAME])), ','), 
	@Part2 = STRING_AGG([COLUMN_NAME], ',') 
FROM
	[INFORMATION_SCHEMA].[COLUMNS]
WHERE 
	TABLE_NAME = @TableName AND
	[ORDINAL_POSITION] >= @OrdinalPosition; 

SELECT 
	@SqlCmd = CONCAT(
		'SELECT VarName, VarValue FROM (SELECT ', @IdColumnName, ',',
		@Part1,
		' FROM ', @TableName, ' WHERE ', @IdColumnName, ' = ', @IdValue, 
		') p UNPIVOT (VarValue FOR VarName IN (',
		@Part2,
		')) AS unpvt'
	);

--SELECT @SqlCmd;
EXEC (@SqlCmd);


END
GO
/****** Object:  StoredProcedure [dbo].[spWaterConsumptionCategoryList]    Script Date: 26.05.2021 09:39:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[spWaterConsumptionCategoryList] 
AS
BEGIN
	SET NOCOUNT ON;

SELECT 
    WaterConsumptionCategoryId AS Id,
	WaterConsumptionCategoryName AS [Name]
FROM
	tbWaterConsumptionCategory;


END
GO
/****** Object:  StoredProcedure [dbo].[spWaterConsumptionCategoryStatusExcelList]    Script Date: 26.05.2021 09:39:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[spWaterConsumptionCategoryStatusExcelList] 
AS
BEGIN
	SET NOCOUNT ON;

SELECT 
    CategoryId,
	StatusId,
	tCS.ExcelCellId,
	tCSE.ExcelCellName
FROM
	tbWaterConsumptionCategoryStatus tCS
	INNER JOIN tbWaterConsumptionCategoryStatusExcel tCSE ON tCS.ExcelCellId = tCSE.ExcelCellId
	;


END
GO
/****** Object:  StoredProcedure [dbo].[spWaterConsumptionClone]    Script Date: 26.05.2021 09:39:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spWaterConsumptionClone]
	@UserName nvarchar(50), 
	@id [int] OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [dbo].[tbWaterConsumption] (
		[Description],
		WaterConsumptionCategoryId,
		WaterConsumptionStatusId, 
		StartDate,
		EndDate,
		Latitude,
		Lontitude,
		RelatedId,
		[Value]
	) 
	SELECT 
		[Description],
		WaterConsumptionCategoryId,
		WaterConsumptionStatusId, 
		StartDate,
		EndDate,
		Latitude,
		Lontitude,
		RelatedId,
		[Value]
	FROM
		tbWaterConsumption
	WHERE
		WaterConsumptionId = @id;

	SELECT @id = SCOPE_IDENTITY();

END;

GO
/****** Object:  StoredProcedure [dbo].[spWaterConsumptionDelete]    Script Date: 26.05.2021 09:39:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[spWaterConsumptionDelete]
	@id int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[tbWaterConsumption] WHERE WaterConsumptionId = @id;
END
GO
/****** Object:  StoredProcedure [dbo].[spWaterConsumptionList]    Script Date: 26.05.2021 09:39:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create PROCEDURE [dbo].[spWaterConsumptionList] 
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * FROM dbo.tbWaterConsumption ORDER BY WaterConsumptionId;
END
GO
/****** Object:  StoredProcedure [dbo].[spWaterConsumptionSave]    Script Date: 26.05.2021 09:39:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spWaterConsumptionSave]
	@Description nvarchar(MAX),

	@WbEasyCalcDataId [int],
	@WaterConsumptionCategoryId [int],
	@WaterConsumptionStatusId [int], 
	@StartDate [datetime],
	@EndDate [datetime],
	@Latitude [float],
	@Lontitude [float],
	@RelatedId [int],
	@Value [float],

	@id [int] OUTPUT
AS
BEGIN
	SET NOCOUNT ON;


/*
DECLARE 
	@OldYearNo [int], 
	@OldMonthNo [int],
	@OldRelatedId [int], 
	@OldIsArchive BIT;
SELECT 
	@OldYearNo = YearNo, 
	@OldMonthNo = MonthNo,
	@OldRelatedId = RelatedId, 
	@OldIsArchive = IsArchive 
FROM 
	[dbo].[tbWbEasyCalcData] 
WHERE 
	WbEasyCalcDataId = @id;

DECLARE 
	@ChangedId [int];


IF (@IsArchive=1) BEGIN
	SELECT
		@ChangedId = WbEasyCalcDataId
	FROM
		[dbo].[tbWbEasyCalcData]
	WHERE
		IsArchive = @IsArchive AND
		RelatedId = @RelatedId AND 
		YearNo = @YearNo AND 
		MonthNo = @MonthNo;

	UPDATE [dbo].[tbWbEasyCalcData] SET
		IsArchive = 0,
		IsAccepted = 0
	WHERE
		RelatedId = @RelatedId AND 
		YearNo = @YearNo AND 
		MonthNo = @MonthNo;
END;
*/



IF (@id = 0) BEGIN

	INSERT INTO [dbo].[tbWaterConsumption] (
		[Description],

		WbEasyCalcDataId,
		WaterConsumptionCategoryId,
		WaterConsumptionStatusId, 
		StartDate,
		EndDate,
		Latitude,
		Lontitude,
		RelatedId,
		[Value]

	) VALUES (
		@Description,

		@WbEasyCalcDataId,
		@WaterConsumptionCategoryId,
		@WaterConsumptionStatusId, 
		@StartDate,
		@EndDate,
		@Latitude,
		@Lontitude,
		@RelatedId,
		@Value
	);

	SELECT @id = SCOPE_IDENTITY();

END ELSE BEGIN
	UPDATE [dbo].[tbWaterConsumption] SET
		[Description] = @Description,

		WbEasyCalcDataId = @WbEasyCalcDataId,
		WaterConsumptionCategoryId = @WaterConsumptionCategoryId,
		WaterConsumptionStatusId = @WaterConsumptionStatusId, 
		StartDate = @StartDate,
		EndDate = @EndDate,
		Latitude = @Latitude,
		Lontitude = @Lontitude,
		RelatedId = @RelatedId,
		[Value] = @Value

	WHERE 
		[WaterConsumptionId] = @id;
END;

--EXEC dbo.spWbEasyCalcDataSaveArchive @id, @OldYearNo, @OldMonthNo, @OldRelatedId, @OldIsArchive, @ChangedId;	

END;



GO
/****** Object:  StoredProcedure [dbo].[spWaterConsumptionStatusList]    Script Date: 26.05.2021 09:39:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[spWaterConsumptionStatusList] 
AS
BEGIN
	SET NOCOUNT ON;

SELECT 
    WaterConsumptionStatusId AS Id,
	WaterConsumptionStatusName AS [Name]
FROM
	tbWaterConsumptionStatus;


END
GO
/****** Object:  StoredProcedure [dbo].[spWbEasyCalcDataClone]    Script Date: 26.05.2021 09:39:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spWbEasyCalcDataClone]
	@id [int] OUTPUT,
	@UserName nvarchar(50) 
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [dbo].[tbWbEasyCalcData] (
		CreateLogin,
		CreateDate,
		ModifyLogin,
		ModifyDate,

		ZoneId, 
		YearNo, 
		MonthNo,
		[Description],
		IsArchive,
		IsAccepted,

		-- Input
		Start_PeriodDays_M21,

		SysInput_Desc_B6,
		SysInput_Desc_B7,
		SysInput_Desc_B8,
		SysInput_Desc_B9,
		SysInput_SystemInputVolumeM3_D6,
		SysInput_SystemInputVolumeError_F6,
		SysInput_SystemInputVolumeM3_D7,
		SysInput_SystemInputVolumeError_F7,
		SysInput_SystemInputVolumeM3_D8,
		SysInput_SystemInputVolumeError_F8,
		SysInput_SystemInputVolumeM3_D9,
		SysInput_SystemInputVolumeError_F9,

		BilledCons_Desc_B8   ,
		BilledCons_Desc_B9   ,
		BilledCons_Desc_B10  ,
		BilledCons_Desc_B11  ,
		BilledCons_Desc_F8   ,
		BilledCons_Desc_F9   ,
		BilledCons_Desc_F10  ,
		BilledCons_Desc_F11  ,
		UnbilledCons_Desc_D8 ,
		UnbilledCons_Desc_D9 ,
		UnbilledCons_Desc_D10,
		UnbilledCons_Desc_D11,
		UnbilledCons_Desc_F6 ,
		UnbilledCons_Desc_F7 ,
		UnbilledCons_Desc_F8 ,
		UnbilledCons_Desc_F9 ,
		UnbilledCons_Desc_F10,
		UnbilledCons_Desc_F11,
		UnauthCons_Desc_B18  ,
		UnauthCons_Desc_B19  ,
		UnauthCons_Desc_B20  ,
		UnauthCons_Desc_B21  ,
		MetErrors_Desc_D12   ,
		MetErrors_Desc_D13   ,
		MetErrors_Desc_D14   ,
		MetErrors_Desc_D15   ,
		Network_Desc_B7      ,
		Network_Desc_B8      ,
		Network_Desc_B9      ,
		Network_Desc_B10     ,
		Interm_Area_B7       ,
		Interm_Area_B8       ,
		Interm_Area_B9       ,
		Interm_Area_B10      ,

		BilledCons_BilledMetConsBulkWatSupExpM3_D6,
		BilledCons_BilledUnmetConsBulkWatSupExpM3_H6,

		BilledCons_UnbMetConsM3_D8,
		BilledCons_UnbMetConsM3_D9,
		BilledCons_UnbMetConsM3_D10,
		BilledCons_UnbMetConsM3_D11,
		BilledCons_UnbUnmetConsM3_H8, 
		BilledCons_UnbUnmetConsM3_H9, 
		BilledCons_UnbUnmetConsM3_H10, 
		BilledCons_UnbUnmetConsM3_H11, 

		UnbilledCons_MetConsBulkWatSupExpM3_D6,

		UnbilledCons_UnbMetConsM3_D8,
		UnbilledCons_UnbMetConsM3_D9,
		UnbilledCons_UnbMetConsM3_D10,
		UnbilledCons_UnbMetConsM3_D11,
		UnbilledCons_UnbUnmetConsM3_H6,
		UnbilledCons_UnbUnmetConsM3_H7,
		UnbilledCons_UnbUnmetConsM3_H8,
		UnbilledCons_UnbUnmetConsM3_H9,
		UnbilledCons_UnbUnmetConsM3_H10,
		UnbilledCons_UnbUnmetConsM3_H11,
		UnbilledCons_UnbUnmetConsError_J6,
		UnbilledCons_UnbUnmetConsError_J7,
		UnbilledCons_UnbUnmetConsError_J8,
		UnbilledCons_UnbUnmetConsError_J9,
		UnbilledCons_UnbUnmetConsError_J10,
		UnbilledCons_UnbUnmetConsError_J11,

		UnauthCons_IllegalConnDomEstNo_D6,
		UnauthCons_IllegalConnDomPersPerHouse_H6,
		UnauthCons_IllegalConnDomConsLitPerPersDay_J6,
		UnauthCons_IllegalConnDomErrorMargin_F6,
		UnauthCons_IllegalConnOthersErrorMargin_F10,

		IllegalConnectionsOthersEstimatedNumber_D10,
		IllegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10,

		UnauthCons_MeterTampBypEtcEstNo_D14,
		UnauthCons_MeterTampBypEtcErrorMargin_F14,
		UnauthCons_MeterTampBypEtcConsLitPerCustDay_J14,

		UnauthCons_OthersErrorMargin_F18, 
		UnauthCons_OthersErrorMargin_F19, 
		UnauthCons_OthersErrorMargin_F20, 
		UnauthCons_OthersErrorMargin_F21, 
		UnauthCons_OthersM3PerDay_J18,
		UnauthCons_OthersM3PerDay_J19,
		UnauthCons_OthersM3PerDay_J20,
		UnauthCons_OthersM3PerDay_J21,

		MetErrors_DetailedManualSpec_J6,
		MetErrors_BilledMetConsWoBulkSupMetUndrreg_H8,
		MetErrors_BilledMetConsWoBulkSupErrorMargin_N8,

		MetErrors_Total_F12,
		MetErrors_Total_F13,
		MetErrors_Total_F14,
		MetErrors_Total_F15,
		MetErrors_Meter_H12,
		MetErrors_Meter_H13,
		MetErrors_Meter_H14,
		MetErrors_Meter_H15,
		MetErrors_Error_N12,
		MetErrors_Error_N13,
		MetErrors_Error_N14,
		MetErrors_Error_N15,

		MeteredBulkSupplyExportErrorMargin_N32,
		UnbilledMeteredConsumptionWithoutBulkSupplyErrorMargin_N34,
		CorruptMeterReadingPracticessErrorMargin_N38,
		DataHandlingErrorsOffice_L40,
		DataHandlingErrorsOfficeErrorMargin_N40,

		MetErrors_MetBulkSupExpMetUnderreg_H32,
		MetErrors_UnbillMetConsWoBulkSupplMetUndrreg_H34,
		MetErrors_CorruptMetReadPractMetUndrreg_H38,
		Network_DistributionAndTransmissionMains_D7,
		Network_DistributionAndTransmissionMains_D8,
		Network_DistributionAndTransmissionMains_D9,
		Network_DistributionAndTransmissionMains_D10,
		Network_NoOfConnOfRegCustomers_H10,
		Network_NoOfInactAccountsWSvcConns_H18,
		Network_AvgLenOfSvcConnFromBoundaryToMeterM_H32,

		Network_PossibleUnd_D30,
		Network_NoCustomers_H7,
		Network_ErrorMargin_J7,
		Network_ErrorMargin_J10,
		Network_ErrorMargin_J18,
		Network_ErrorMargin_J32,
		Network_ErrorMargin_D35,

		Prs_Area_B7,
		Prs_Area_B8,
		Prs_Area_B9,
		Prs_Area_B10,
		Prs_ApproxNoOfConn_D7,
		Prs_DailyAvgPrsM_F7,
		Prs_ApproxNoOfConn_D8,
		Prs_DailyAvgPrsM_F8,
		Prs_ApproxNoOfConn_D9,
		Prs_DailyAvgPrsM_F9,
		Prs_ApproxNoOfConn_D10,
		Prs_DailyAvgPrsM_F10,

		Prs_ErrorMarg_F26,

		Interm_Conn_D7,
		Interm_Conn_D8,
		Interm_Conn_D9,
		Interm_Conn_D10,
		Interm_Days_F7,
		Interm_Days_F8,
		Interm_Days_F9,
		Interm_Days_F10,
		Interm_Hour_H7,
		Interm_Hour_H8,
		Interm_Hour_H9,
		Interm_Hour_H10,
		Interm_ErrorMarg_H26,

		FinancData_G6,
		FinancData_K6,
		FinancData_G8,
		FinancData_D26,
		FinancData_G35,

		MatrixOneIn_SelectedOption,
		MatrixOneIn_C11,
		MatrixOneIn_C12,
		MatrixOneIn_C13,
		MatrixOneIn_C14,
		MatrixOneIn_C21,
		MatrixOneIn_C22,
		MatrixOneIn_C23,
		MatrixOneIn_C24,
		MatrixOneIn_D21,
		MatrixOneIn_D22,
		MatrixOneIn_D23,
		MatrixOneIn_D24,
		MatrixOneIn_E11,
		MatrixOneIn_E12,
		MatrixOneIn_E13,
		MatrixOneIn_E14,
		MatrixOneIn_E21,
		MatrixOneIn_E22,
		MatrixOneIn_E23,
		MatrixOneIn_E24,
		MatrixOneIn_F11,
		MatrixOneIn_F12,
		MatrixOneIn_F13,
		MatrixOneIn_F14,
		MatrixOneIn_F21,
		MatrixOneIn_F22,
		MatrixOneIn_F23,
		MatrixOneIn_F24,
		MatrixOneIn_G11,
		MatrixOneIn_G12,
		MatrixOneIn_G13,
		MatrixOneIn_G14,
		MatrixOneIn_G21,
		MatrixOneIn_G22,
		MatrixOneIn_G23,
		MatrixOneIn_G24,
		MatrixOneIn_H11,
		MatrixOneIn_H12,
		MatrixOneIn_H13,
		MatrixOneIn_H14,
		MatrixOneIn_H21,
		MatrixOneIn_H22,
		MatrixOneIn_H23,
		MatrixOneIn_H24,

		-- Output
		SystemInputVolume_B19,
		SystemInputVolumeErrorMargin_B21,	
		AuthorizedConsumption_K12,
		AuthorizedConsumptionErrorMargin_K15,
		WaterLosses_K29, 
		WaterLossesErrorMargin_K31,
		BilledAuthorizedConsumption_T8, 
		UnbilledAuthorizedConsumption_T16, 
		UnbilledAuthorizedConsumptionErrorMargin_T20,
		CommercialLosses_T26, 
		CommercialLossesErrorMargin_T29, 
		PhysicalLossesM3_T34, 
		PhyscialLossesErrorMargin_AH35, 
		BilledMeteredConsumption_AC4,
		BilledUnmeteredConsumption_AC9,
		UnbilledMeteredConsumption_AC14,
		UnbilledUnmeteredConsumption_AC19,
		UnbilledUnmeteredConsumptionErrorMargin_AO20,
		UnauthorizedConsumption_AC24,
		UnauthorizedConsumptionErrorMargin_AO25,
		CustomerMeterInaccuraciesAndErrorsM3_AC29,
		CustomerMeterInaccuraciesAndErrorsErrorMargin_AO30,
		RevenueWaterM3_AY8,
		NonRevenueWaterM3_AY24,
		NonRevenueWaterErrorMargin_AY26
	) 
	SELECT 
		@UserName,
		GETDATE(),
		@UserName,
		GETDATE(),

		ZoneId, 
		YearNo, 
		MonthNo,
		[Description],
		0,
		0,

		-- Input
		Start_PeriodDays_M21,

		SysInput_Desc_B6,
		SysInput_Desc_B7,
		SysInput_Desc_B8,
		SysInput_Desc_B9,
		SysInput_SystemInputVolumeM3_D6,
		SysInput_SystemInputVolumeError_F6,
		SysInput_SystemInputVolumeM3_D7,
		SysInput_SystemInputVolumeError_F7,
		SysInput_SystemInputVolumeM3_D8,
		SysInput_SystemInputVolumeError_F8,
		SysInput_SystemInputVolumeM3_D9,
		SysInput_SystemInputVolumeError_F9,

		BilledCons_Desc_B8   ,
		BilledCons_Desc_B9   ,
		BilledCons_Desc_B10  ,
		BilledCons_Desc_B11  ,
		BilledCons_Desc_F8   ,
		BilledCons_Desc_F9   ,
		BilledCons_Desc_F10  ,
		BilledCons_Desc_F11  ,
		UnbilledCons_Desc_D8 ,
		UnbilledCons_Desc_D9 ,
		UnbilledCons_Desc_D10,
		UnbilledCons_Desc_D11,
		UnbilledCons_Desc_F6 ,
		UnbilledCons_Desc_F7 ,
		UnbilledCons_Desc_F8 ,
		UnbilledCons_Desc_F9 ,
		UnbilledCons_Desc_F10,
		UnbilledCons_Desc_F11,
		UnauthCons_Desc_B18  ,
		UnauthCons_Desc_B19  ,
		UnauthCons_Desc_B20  ,
		UnauthCons_Desc_B21  ,
		MetErrors_Desc_D12   ,
		MetErrors_Desc_D13   ,
		MetErrors_Desc_D14   ,
		MetErrors_Desc_D15   ,
		Network_Desc_B7      ,
		Network_Desc_B8      ,
		Network_Desc_B9      ,
		Network_Desc_B10     ,
		Interm_Area_B7       ,
		Interm_Area_B8       ,
		Interm_Area_B9       ,
		Interm_Area_B10      ,

		BilledCons_BilledMetConsBulkWatSupExpM3_D6,
		BilledCons_BilledUnmetConsBulkWatSupExpM3_H6,

		BilledCons_UnbMetConsM3_D8,
		BilledCons_UnbMetConsM3_D9,
		BilledCons_UnbMetConsM3_D10,
		BilledCons_UnbMetConsM3_D11,
		BilledCons_UnbUnmetConsM3_H8, 
		BilledCons_UnbUnmetConsM3_H9, 
		BilledCons_UnbUnmetConsM3_H10, 
		BilledCons_UnbUnmetConsM3_H11, 

		UnbilledCons_MetConsBulkWatSupExpM3_D6,

		UnbilledCons_UnbMetConsM3_D8,
		UnbilledCons_UnbMetConsM3_D9,
		UnbilledCons_UnbMetConsM3_D10,
		UnbilledCons_UnbMetConsM3_D11,
		UnbilledCons_UnbUnmetConsM3_H6,
		UnbilledCons_UnbUnmetConsM3_H7,
		UnbilledCons_UnbUnmetConsM3_H8,
		UnbilledCons_UnbUnmetConsM3_H9,
		UnbilledCons_UnbUnmetConsM3_H10,
		UnbilledCons_UnbUnmetConsM3_H11,
		UnbilledCons_UnbUnmetConsError_J6,
		UnbilledCons_UnbUnmetConsError_J7,
		UnbilledCons_UnbUnmetConsError_J8,
		UnbilledCons_UnbUnmetConsError_J9,
		UnbilledCons_UnbUnmetConsError_J10,
		UnbilledCons_UnbUnmetConsError_J11,

		UnauthCons_IllegalConnDomEstNo_D6,
		UnauthCons_IllegalConnDomPersPerHouse_H6,
		UnauthCons_IllegalConnDomConsLitPerPersDay_J6,
		UnauthCons_IllegalConnDomErrorMargin_F6,
		UnauthCons_IllegalConnOthersErrorMargin_F10,

		IllegalConnectionsOthersEstimatedNumber_D10,
		IllegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10,

		UnauthCons_MeterTampBypEtcEstNo_D14,
		UnauthCons_MeterTampBypEtcErrorMargin_F14,
		UnauthCons_MeterTampBypEtcConsLitPerCustDay_J14,

		UnauthCons_OthersErrorMargin_F18, 
		UnauthCons_OthersErrorMargin_F19, 
		UnauthCons_OthersErrorMargin_F20, 
		UnauthCons_OthersErrorMargin_F21, 
		UnauthCons_OthersM3PerDay_J18,
		UnauthCons_OthersM3PerDay_J19,
		UnauthCons_OthersM3PerDay_J20,
		UnauthCons_OthersM3PerDay_J21,

		MetErrors_DetailedManualSpec_J6,
		MetErrors_BilledMetConsWoBulkSupMetUndrreg_H8,
		MetErrors_BilledMetConsWoBulkSupErrorMargin_N8,

		MetErrors_Total_F12,
		MetErrors_Total_F13,
		MetErrors_Total_F14,
		MetErrors_Total_F15,
		MetErrors_Meter_H12,
		MetErrors_Meter_H13,
		MetErrors_Meter_H14,
		MetErrors_Meter_H15,
		MetErrors_Error_N12,
		MetErrors_Error_N13,
		MetErrors_Error_N14,
		MetErrors_Error_N15,

		MeteredBulkSupplyExportErrorMargin_N32,
		UnbilledMeteredConsumptionWithoutBulkSupplyErrorMargin_N34,
		CorruptMeterReadingPracticessErrorMargin_N38,
		DataHandlingErrorsOffice_L40,
		DataHandlingErrorsOfficeErrorMargin_N40,

		MetErrors_MetBulkSupExpMetUnderreg_H32,
		MetErrors_UnbillMetConsWoBulkSupplMetUndrreg_H34,
		MetErrors_CorruptMetReadPractMetUndrreg_H38,
		Network_DistributionAndTransmissionMains_D7,
		Network_DistributionAndTransmissionMains_D8,
		Network_DistributionAndTransmissionMains_D9,
		Network_DistributionAndTransmissionMains_D10,
		Network_NoOfConnOfRegCustomers_H10,
		Network_NoOfInactAccountsWSvcConns_H18,
		Network_AvgLenOfSvcConnFromBoundaryToMeterM_H32,

		Network_PossibleUnd_D30,
		Network_NoCustomers_H7,
		Network_ErrorMargin_J7,
		Network_ErrorMargin_J10,
		Network_ErrorMargin_J18,
		Network_ErrorMargin_J32,
		Network_ErrorMargin_D35,

		Prs_Area_B7,
		Prs_Area_B8,
		Prs_Area_B9,
		Prs_Area_B10,
		Prs_ApproxNoOfConn_D7,
		Prs_DailyAvgPrsM_F7,
		Prs_ApproxNoOfConn_D8,
		Prs_DailyAvgPrsM_F8,
		Prs_ApproxNoOfConn_D9,
		Prs_DailyAvgPrsM_F9,
		Prs_ApproxNoOfConn_D10,
		Prs_DailyAvgPrsM_F10,

		Prs_ErrorMarg_F26,

		Interm_Conn_D7,
		Interm_Conn_D8,
		Interm_Conn_D9,
		Interm_Conn_D10,
		Interm_Days_F7,
		Interm_Days_F8,
		Interm_Days_F9,
		Interm_Days_F10,
		Interm_Hour_H7,
		Interm_Hour_H8,
		Interm_Hour_H9,
		Interm_Hour_H10,
		Interm_ErrorMarg_H26,

		FinancData_G6,
		FinancData_K6,
		FinancData_G8,
		FinancData_D26,
		FinancData_G35,

		MatrixOneIn_SelectedOption,
		MatrixOneIn_C11,
		MatrixOneIn_C12,
		MatrixOneIn_C13,
		MatrixOneIn_C14,
		MatrixOneIn_C21,
		MatrixOneIn_C22,
		MatrixOneIn_C23,
		MatrixOneIn_C24,
		MatrixOneIn_D21,
		MatrixOneIn_D22,
		MatrixOneIn_D23,
		MatrixOneIn_D24,
		MatrixOneIn_E11,
		MatrixOneIn_E12,
		MatrixOneIn_E13,
		MatrixOneIn_E14,
		MatrixOneIn_E21,
		MatrixOneIn_E22,
		MatrixOneIn_E23,
		MatrixOneIn_E24,
		MatrixOneIn_F11,
		MatrixOneIn_F12,
		MatrixOneIn_F13,
		MatrixOneIn_F14,
		MatrixOneIn_F21,
		MatrixOneIn_F22,
		MatrixOneIn_F23,
		MatrixOneIn_F24,
		MatrixOneIn_G11,
		MatrixOneIn_G12,
		MatrixOneIn_G13,
		MatrixOneIn_G14,
		MatrixOneIn_G21,
		MatrixOneIn_G22,
		MatrixOneIn_G23,
		MatrixOneIn_G24,
		MatrixOneIn_H11,
		MatrixOneIn_H12,
		MatrixOneIn_H13,
		MatrixOneIn_H14,
		MatrixOneIn_H21,
		MatrixOneIn_H22,
		MatrixOneIn_H23,
		MatrixOneIn_H24,

		-- Output
		SystemInputVolume_B19,
		SystemInputVolumeErrorMargin_B21,	
		AuthorizedConsumption_K12,
		AuthorizedConsumptionErrorMargin_K15,
		WaterLosses_K29, 
		WaterLossesErrorMargin_K31,
		BilledAuthorizedConsumption_T8, 
		UnbilledAuthorizedConsumption_T16, 
		UnbilledAuthorizedConsumptionErrorMargin_T20,
		CommercialLosses_T26, 
		CommercialLossesErrorMargin_T29, 
		PhysicalLossesM3_T34, 
		PhyscialLossesErrorMargin_AH35, 
		BilledMeteredConsumption_AC4,
		BilledUnmeteredConsumption_AC9,
		UnbilledMeteredConsumption_AC14,
		UnbilledUnmeteredConsumption_AC19,
		UnbilledUnmeteredConsumptionErrorMargin_AO20,
		UnauthorizedConsumption_AC24,
		UnauthorizedConsumptionErrorMargin_AO25,
		CustomerMeterInaccuraciesAndErrorsM3_AC29,
		CustomerMeterInaccuraciesAndErrorsErrorMargin_AO30,
		RevenueWaterM3_AY8,
		NonRevenueWaterM3_AY24,
		NonRevenueWaterErrorMargin_AY26
	FROM
		tbWbEasyCalcData
	WHERE
		WbEasyCalcDataId = @id;

	SELECT @id = SCOPE_IDENTITY();

END;

GO
/****** Object:  StoredProcedure [dbo].[spWbEasyCalcDataCreateAll]    Script Date: 26.05.2021 09:39:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spWbEasyCalcDataCreateAll]
	@RecordQty int OUTPUT
AS
BEGIN
SET NOCOUNT ON;

SELECT @RecordQty = COUNT(*) FROM tbWbEasyCalcData;

WITH 
t001 AS (
SELECT
	ZoneId,
	MAX(CAST(CONCAT(YearNo, '-', FORMAT(MonthNo, '00'), '-01')  AS DATETIME)) AS MaxDate
FROM
	[dbo].[tbWbEasyCalcData]
WHERE
	MonthNo < 13
	AND IsArchive = 1
GROUP BY
	ZoneId
)
,t002 AS (
SELECT
	t001.ZoneId,
	tConf.DestinationName AS ZonePrefix,
	t001.MaxDate,
	YEAR(MaxDate) AS YearNo,
	MONTH(MaxDate) AS MonthNo,
    YEAR(DATEADD(month, 1, t001.MaxDate)) AS NextYearNo, 
    MONTH(DATEADD(month, 1, t001.MaxDate)) AS NextMonthNo,
	DATEADD(month, 1, t001.MaxDate) AS StartDate,
	DATEADD(month, 2, t001.MaxDate) AS EndDate,
	tMap.D_ID
FROM
	t001
	INNER JOIN [config].[WaterBalanceConfig] tConf ON t001.ZoneId= tConf.ZoneNo 
	INNER JOIN dbo.TELWIN_MAP tMap ON CONCAT(tConf.DestinationName, '_SREDN_10M') = tMap.D_NAME
)
,tMonthFlowSum AS (
SELECT 
	t002.ZoneId,
	SUM(tArch.D_VALUE_FLO) AS MonthFlowSum 
FROM 
	t002
	INNER JOIN dbo.AR_0000_2020 tArch ON t002.D_ID = tArch.D_VAR_ID
WHERE        
    tArch.D_STATUS = 16
	AND tArch.T_TYPE = 1
	AND tArch.D_TIME >= StartDate
	AND tArch.D_TIME < EndDate
GROUP BY
	t002.ZoneId
)
INSERT INTO [dbo].[tbWbEasyCalcData](
     [ZoneId]
    ,[YearNo]
    ,[MonthNo]
    ,[Description]
    ,[IsArchive]
    ,[IsAccepted]
    ,[Start_PeriodDays_M21]
    ,[SysInput_SystemInputVolumeM3_D6]
    ,[SysInput_SystemInputVolumeError_F6]
    ,[BilledCons_BilledMetConsBulkWatSupExpM3_D6]
    ,[BilledCons_BilledUnmetConsBulkWatSupExpM3_H6]

	,BilledCons_UnbMetConsM3_D8
	,BilledCons_UnbUnmetConsM3_H8

    ,[UnbilledCons_MetConsBulkWatSupExpM3_D6]

    ,UnbilledCons_UnbMetConsM3_D8
	,UnbilledCons_UnbUnmetConsM3_H6
    ,UnbilledCons_UnbUnmetConsError_J6

    ,[UnauthCons_IllegalConnDomEstNo_D6]
    ,[UnauthCons_IllegalConnDomPersPerHouse_H6]
    ,[UnauthCons_IllegalConnDomConsLitPerPersDay_J6]
    ,[UnauthCons_IllegalConnDomErrorMargin_F6]
    ,[UnauthCons_IllegalConnOthersErrorMargin_F10]

	,IllegalConnectionsOthersEstimatedNumber_D10
	,IllegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10

    ,[UnauthCons_MeterTampBypEtcEstNo_D14]
    ,[UnauthCons_MeterTampBypEtcErrorMargin_F14]
    ,[UnauthCons_MeterTampBypEtcConsLitPerCustDay_J14]
    ,[MetErrors_DetailedManualSpec_J6]
    ,[MetErrors_BilledMetConsWoBulkSupMetUndrreg_H8]
    ,[MetErrors_BilledMetConsWoBulkSupErrorMargin_N8]

	,MeteredBulkSupplyExportErrorMargin_N32
	,UnbilledMeteredConsumptionWithoutBulkSupplyErrorMargin_N34
	,CorruptMeterReadingPracticessErrorMargin_N38
	,DataHandlingErrorsOffice_L40
	,DataHandlingErrorsOfficeErrorMargin_N40

    ,[MetErrors_MetBulkSupExpMetUnderreg_H32]
    ,[MetErrors_UnbillMetConsWoBulkSupplMetUndrreg_H34]
    ,[MetErrors_CorruptMetReadPractMetUndrreg_H38]
    ,[Network_DistributionAndTransmissionMains_D7]
    ,[Network_NoOfConnOfRegCustomers_H10]
    ,[Network_NoOfInactAccountsWSvcConns_H18]
    ,[Network_AvgLenOfSvcConnFromBoundaryToMeterM_H32]
    ,[Prs_ApproxNoOfConn_D7]
    ,[Prs_DailyAvgPrsM_F7]
    ,[Prs_ErrorMarg_F26]
    ,[PIs_IliBestEstimate_F25]
    ,[SystemInputVolume_B19]
    ,[SystemInputVolumeErrorMargin_B21]
    ,[AuthorizedConsumption_K12]
    ,[AuthorizedConsumptionErrorMargin_K15]
    ,[WaterLosses_K29]
    ,[WaterLossesErrorMargin_K31]
    ,[BilledAuthorizedConsumption_T8]
    ,[UnbilledAuthorizedConsumption_T16]
    ,[UnbilledAuthorizedConsumptionErrorMargin_T20]
    ,[CommercialLosses_T26]
    ,[CommercialLossesErrorMargin_T29]
    ,[PhysicalLossesM3_T34]
    ,[PhyscialLossesErrorMargin_AH35]
    ,[BilledMeteredConsumption_AC4]
    ,[BilledUnmeteredConsumption_AC9]
    ,[UnbilledMeteredConsumption_AC14]
    ,[UnbilledUnmeteredConsumption_AC19]
    ,[UnbilledUnmeteredConsumptionErrorMargin_AO20]
    ,[UnauthorizedConsumption_AC24]
    ,[UnauthorizedConsumptionErrorMargin_AO25]
    ,[CustomerMeterInaccuraciesAndErrorsM3_AC29]
    ,[CustomerMeterInaccuraciesAndErrorsErrorMargin_AO30]
    ,[RevenueWaterM3_AY8]
    ,[NonRevenueWaterM3_AY24]
    ,[NonRevenueWaterErrorMargin_AY26]
)
SELECT 
     tWbEasyCalc.[ZoneId]

    ,t002.NextYearNo AS [YearNo]
    ,t002.NextMonthNo AS [MonthNo] 
    ,'Generated' AS [Description]
    ,0 AS [IsArchive]
    ,0 AS [IsAccepted]

    ,DAY(EndDate) AS [Start_PeriodDays_M21]
    ,COALESCE(tMonthFlowSum.MonthFlowSum, 0) AS [SysInput_SystemInputVolumeM3_D6]
    
	,tWbEasyCalc.[SysInput_SystemInputVolumeError_F6]

    ,COALESCE(tAut.sprzedaz_w_strefie, 0) AS [BilledCons_BilledMetConsBulkWatSupExpM3_D6]

    ,tWbEasyCalc.[BilledCons_BilledUnmetConsBulkWatSupExpM3_H6]

	,tWbEasyCalc.BilledCons_UnbMetConsM3_D8
	,tWbEasyCalc.BilledCons_UnbUnmetConsM3_H8

    ,tWbEasyCalc.[UnbilledCons_MetConsBulkWatSupExpM3_D6]

    ,tWbEasyCalc.UnbilledCons_UnbMetConsM3_D8
	,tWbEasyCalc.UnbilledCons_UnbUnmetConsM3_H6
    ,tWbEasyCalc.UnbilledCons_UnbUnmetConsError_J6

    ,tWbEasyCalc.[UnauthCons_IllegalConnDomEstNo_D6]
    ,tWbEasyCalc.[UnauthCons_IllegalConnDomPersPerHouse_H6]
    ,tWbEasyCalc.[UnauthCons_IllegalConnDomConsLitPerPersDay_J6]
    ,tWbEasyCalc.[UnauthCons_IllegalConnDomErrorMargin_F6]
    ,tWbEasyCalc.[UnauthCons_IllegalConnOthersErrorMargin_F10]

	,tWbEasyCalc.IllegalConnectionsOthersEstimatedNumber_D10
	,tWbEasyCalc.IllegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10

    ,tWbEasyCalc.[UnauthCons_MeterTampBypEtcEstNo_D14]
    ,tWbEasyCalc.[UnauthCons_MeterTampBypEtcErrorMargin_F14]
    ,tWbEasyCalc.[UnauthCons_MeterTampBypEtcConsLitPerCustDay_J14]
    ,tWbEasyCalc.[MetErrors_DetailedManualSpec_J6]
    ,tWbEasyCalc.[MetErrors_BilledMetConsWoBulkSupMetUndrreg_H8]
    ,tWbEasyCalc.[MetErrors_BilledMetConsWoBulkSupErrorMargin_N8]

	,tWbEasyCalc.MeteredBulkSupplyExportErrorMargin_N32
	,tWbEasyCalc.UnbilledMeteredConsumptionWithoutBulkSupplyErrorMargin_N34
	,tWbEasyCalc.CorruptMeterReadingPracticessErrorMargin_N38
	,tWbEasyCalc.DataHandlingErrorsOffice_L40
	,tWbEasyCalc.DataHandlingErrorsOfficeErrorMargin_N40

    ,tWbEasyCalc.[MetErrors_MetBulkSupExpMetUnderreg_H32]
    ,tWbEasyCalc.[MetErrors_UnbillMetConsWoBulkSupplMetUndrreg_H34]
    ,tWbEasyCalc.[MetErrors_CorruptMetReadPractMetUndrreg_H38]

    ,COALESCE(tAut.dlugosc_sieci, 0) AS [Network_DistributionAndTransmissionMains_D7]
    ,COALESCE(tAut.ilosc_przylaczy, 0) AS [Network_NoOfConnOfRegCustomers_H10]

    ,tWbEasyCalc.[Network_NoOfInactAccountsWSvcConns_H18]
    ,tWbEasyCalc.[Network_AvgLenOfSvcConnFromBoundaryToMeterM_H32]
    ,tWbEasyCalc.[Prs_ApproxNoOfConn_D7]
    ,tWbEasyCalc.[Prs_DailyAvgPrsM_F7]
    ,tWbEasyCalc.[Prs_ErrorMarg_F26]
    ,tWbEasyCalc.[PIs_IliBestEstimate_F25]
    ,tWbEasyCalc.[SystemInputVolume_B19]
    ,tWbEasyCalc.[SystemInputVolumeErrorMargin_B21]
    ,tWbEasyCalc.[AuthorizedConsumption_K12]
    ,tWbEasyCalc.[AuthorizedConsumptionErrorMargin_K15]
    ,tWbEasyCalc.[WaterLosses_K29]
    ,tWbEasyCalc.[WaterLossesErrorMargin_K31]
    ,tWbEasyCalc.[BilledAuthorizedConsumption_T8]
    ,tWbEasyCalc.[UnbilledAuthorizedConsumption_T16]
    ,tWbEasyCalc.[UnbilledAuthorizedConsumptionErrorMargin_T20]
    ,tWbEasyCalc.[CommercialLosses_T26]
    ,tWbEasyCalc.[CommercialLossesErrorMargin_T29]
    ,tWbEasyCalc.[PhysicalLossesM3_T34]
    ,tWbEasyCalc.[PhyscialLossesErrorMargin_AH35]
    ,tWbEasyCalc.[BilledMeteredConsumption_AC4]
    ,tWbEasyCalc.[BilledUnmeteredConsumption_AC9]
    ,tWbEasyCalc.[UnbilledMeteredConsumption_AC14]
    ,tWbEasyCalc.[UnbilledUnmeteredConsumption_AC19]
    ,tWbEasyCalc.[UnbilledUnmeteredConsumptionErrorMargin_AO20]
    ,tWbEasyCalc.[UnauthorizedConsumption_AC24]
    ,tWbEasyCalc.[UnauthorizedConsumptionErrorMargin_AO25]
    ,tWbEasyCalc.[CustomerMeterInaccuraciesAndErrorsM3_AC29]
    ,tWbEasyCalc.[CustomerMeterInaccuraciesAndErrorsErrorMargin_AO30]
    ,tWbEasyCalc.[RevenueWaterM3_AY8]
    ,tWbEasyCalc.[NonRevenueWaterM3_AY24]
    ,tWbEasyCalc.[NonRevenueWaterErrorMargin_AY26]
FROM 
	t002
	INNER JOIN dbo.tbWbEasyCalcData tWbEasyCalc ON 
		t002.ZoneId = tWbEasyCalc.ZoneId
		AND t002.YearNo = tWbEasyCalc.[YearNo] 
		AND t002.MonthNo = tWbEasyCalc.[MonthNo]
	LEFT OUTER JOIN tMonthFlowSum ON 
		t002.ZoneId = tMonthFlowSum.ZoneId
	LEFT OUTER JOIN [dbo].[vw_zuzycia_stref_2] tAut ON
		t002.ZoneId = tAut.ZoneId
		AND t002.NextYearNo = tAut.[year] 
		AND t002.NextMonthNo = tAut.[month] 
WHERE
	tWbEasyCalc.IsArchive = 1
	;

SELECT @RecordQty = COUNT(*) - @RecordQty FROM tbWbEasyCalcData;

 
END
GO
/****** Object:  StoredProcedure [dbo].[spWbEasyCalcDataDelete]    Script Date: 26.05.2021 09:39:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spWbEasyCalcDataDelete]
	@id int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[tbWbEasyCalcData] WHERE WbEasyCalcDataId = @id;
END
GO
/****** Object:  StoredProcedure [dbo].[spWbEasyCalcDataList]    Script Date: 26.05.2021 09:39:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spWbEasyCalcDataList] 
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * FROM dbo.tbWbEasyCalcData ORDER BY WbEasyCalcDataId;
END
GO
/****** Object:  StoredProcedure [dbo].[spWbEasyCalcDataSave]    Script Date: 26.05.2021 09:39:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spWbEasyCalcDataSave]
	@UserName nvarchar(50), 

	@ZoneId [int], 
	@YearNo [int], 
	@MonthNo [int],
	@Description nvarchar(MAX),
	@IsArchive [bit],
	@IsAccepted [bit],

	-- Input
	@Start_PeriodDays_M21 [int],

	@SysInput_Desc_B6 nvarchar(400),
	@SysInput_Desc_B7 nvarchar(400),
	@SysInput_Desc_B8 nvarchar(400),
	@SysInput_Desc_B9 nvarchar(400),
	@SysInput_SystemInputVolumeM3_D6 [float],
	@SysInput_SystemInputVolumeError_F6 [float],
	@SysInput_SystemInputVolumeM3_D7 [float],
	@SysInput_SystemInputVolumeError_F7 [float],
	@SysInput_SystemInputVolumeM3_D8 [float],
	@SysInput_SystemInputVolumeError_F8 [float],
	@SysInput_SystemInputVolumeM3_D9 [float],
	@SysInput_SystemInputVolumeError_F9 [float],

	@BilledCons_Desc_B8      nvarchar(400),
	@BilledCons_Desc_B9   	 nvarchar(400),
	@BilledCons_Desc_B10  	 nvarchar(400),
	@BilledCons_Desc_B11  	 nvarchar(400),
	@BilledCons_Desc_F8   	 nvarchar(400),
	@BilledCons_Desc_F9   	 nvarchar(400),
	@BilledCons_Desc_F10  	 nvarchar(400),
	@BilledCons_Desc_F11  	 nvarchar(400),
	@UnbilledCons_Desc_D8 	 nvarchar(400),
	@UnbilledCons_Desc_D9 	 nvarchar(400),
	@UnbilledCons_Desc_D10	 nvarchar(400),
	@UnbilledCons_Desc_D11	 nvarchar(400),
	@UnbilledCons_Desc_F6 	 nvarchar(400),
	@UnbilledCons_Desc_F7 	 nvarchar(400),
	@UnbilledCons_Desc_F8 	 nvarchar(400),
	@UnbilledCons_Desc_F9 	 nvarchar(400),
	@UnbilledCons_Desc_F10	 nvarchar(400),
	@UnbilledCons_Desc_F11	 nvarchar(400),
	@UnauthCons_Desc_B18  	 nvarchar(400),
	@UnauthCons_Desc_B19  	 nvarchar(400),
	@UnauthCons_Desc_B20  	 nvarchar(400),
	@UnauthCons_Desc_B21  	 nvarchar(400),
	@MetErrors_Desc_D12   	 nvarchar(400),
	@MetErrors_Desc_D13   	 nvarchar(400),
	@MetErrors_Desc_D14   	 nvarchar(400),
	@MetErrors_Desc_D15   	 nvarchar(400),
	@Network_Desc_B7      	 nvarchar(400),
	@Network_Desc_B8      	 nvarchar(400),
	@Network_Desc_B9      	 nvarchar(400),
	@Network_Desc_B10     	 nvarchar(400),
	@Interm_Area_B7       	 nvarchar(400),
	@Interm_Area_B8       	 nvarchar(400),
	@Interm_Area_B9       	 nvarchar(400),
	@Interm_Area_B10      	 nvarchar(400),

	@BilledCons_BilledMetConsBulkWatSupExpM3_D6 [float],
	@BilledCons_BilledUnmetConsBulkWatSupExpM3_H6 [float],

	@BilledCons_UnbMetConsM3_D8 [float],
	@BilledCons_UnbMetConsM3_D9 [float],
	@BilledCons_UnbMetConsM3_D10 [float],
	@BilledCons_UnbMetConsM3_D11 [float],
	@BilledCons_UnbUnmetConsM3_H8 [float], 
	@BilledCons_UnbUnmetConsM3_H9 [float], 
	@BilledCons_UnbUnmetConsM3_H10 [float], 
	@BilledCons_UnbUnmetConsM3_H11 [float], 

	@UnbilledCons_MetConsBulkWatSupExpM3_D6 [float],

	@UnbilledCons_UnbMetConsM3_D8 [float],
	@UnbilledCons_UnbMetConsM3_D9 [float],
	@UnbilledCons_UnbMetConsM3_D10 [float],
	@UnbilledCons_UnbMetConsM3_D11 [float],
	@UnbilledCons_UnbUnmetConsM3_H6 [float],
	@UnbilledCons_UnbUnmetConsM3_H7 [float],
	@UnbilledCons_UnbUnmetConsM3_H8 [float],
	@UnbilledCons_UnbUnmetConsM3_H9 [float],
	@UnbilledCons_UnbUnmetConsM3_H10 [float],
	@UnbilledCons_UnbUnmetConsM3_H11 [float],
	@UnbilledCons_UnbUnmetConsError_J6 [float],
	@UnbilledCons_UnbUnmetConsError_J7 [float],
	@UnbilledCons_UnbUnmetConsError_J8 [float],
	@UnbilledCons_UnbUnmetConsError_J9 [float],
	@UnbilledCons_UnbUnmetConsError_J10 [float],
	@UnbilledCons_UnbUnmetConsError_J11 [float],

	@UnauthCons_IllegalConnDomEstNo_D6 [int],
	@UnauthCons_IllegalConnDomPersPerHouse_H6 [float],
	@UnauthCons_IllegalConnDomConsLitPerPersDay_J6 [float],
	@UnauthCons_IllegalConnDomErrorMargin_F6 [float],
	@UnauthCons_IllegalConnOthersErrorMargin_F10 [float],

	@IllegalConnectionsOthersEstimatedNumber_D10 [float],
	@IllegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10 [float],

	@UnauthCons_MeterTampBypEtcEstNo_D14 [float],
	@UnauthCons_MeterTampBypEtcErrorMargin_F14 [float],
	@UnauthCons_MeterTampBypEtcConsLitPerCustDay_J14 [float],

    @UnauthCons_OthersErrorMargin_F18 [float], 
    @UnauthCons_OthersErrorMargin_F19 [float], 
    @UnauthCons_OthersErrorMargin_F20 [float], 
    @UnauthCons_OthersErrorMargin_F21 [float], 
	@UnauthCons_OthersM3PerDay_J18 [float],
	@UnauthCons_OthersM3PerDay_J19 [float],
	@UnauthCons_OthersM3PerDay_J20 [float],
	@UnauthCons_OthersM3PerDay_J21 [float],

	@MetErrors_DetailedManualSpec_J6 [bit],
	@MetErrors_BilledMetConsWoBulkSupMetUndrreg_H8 [float],
	@MetErrors_BilledMetConsWoBulkSupErrorMargin_N8 [float],

	@MetErrors_Total_F12 [float],
	@MetErrors_Total_F13 [float],
	@MetErrors_Total_F14 [float],
	@MetErrors_Total_F15 [float],
	@MetErrors_Meter_H12 [float],
	@MetErrors_Meter_H13 [float],
	@MetErrors_Meter_H14 [float],
	@MetErrors_Meter_H15 [float],
	@MetErrors_Error_N12 [float],
	@MetErrors_Error_N13 [float],
	@MetErrors_Error_N14 [float],
	@MetErrors_Error_N15 [float],

	@MeteredBulkSupplyExportErrorMargin_N32 [float],
	@UnbilledMeteredConsumptionWithoutBulkSupplyErrorMargin_N34 [float],
	@CorruptMeterReadingPracticessErrorMargin_N38 [float],
	@DataHandlingErrorsOffice_L40 [float],
	@DataHandlingErrorsOfficeErrorMargin_N40 [float],

	@MetErrors_MetBulkSupExpMetUnderreg_H32 [float],
	@MetErrors_UnbillMetConsWoBulkSupplMetUndrreg_H34 [float],
	@MetErrors_CorruptMetReadPractMetUndrreg_H38 [float],
	@Network_DistributionAndTransmissionMains_D7 [float],
	@Network_DistributionAndTransmissionMains_D8 [float],
	@Network_DistributionAndTransmissionMains_D9 [float],
	@Network_DistributionAndTransmissionMains_D10 [float],
	@Network_NoOfConnOfRegCustomers_H10 [float],
	@Network_NoOfInactAccountsWSvcConns_H18 [float],
	@Network_AvgLenOfSvcConnFromBoundaryToMeterM_H32 [float],

	@Network_PossibleUnd_D30 [float],
	@Network_NoCustomers_H7  [float],
	@Network_ErrorMargin_J7  [float],
	@Network_ErrorMargin_J10 [float],
	@Network_ErrorMargin_J18 [float],
	@Network_ErrorMargin_J32 [float],
	@Network_ErrorMargin_D35 [float],

	@Prs_Area_B7 nvarchar(400),
	@Prs_Area_B8 nvarchar(400),
	@Prs_Area_B9 nvarchar(400),
	@Prs_Area_B10 nvarchar(400),
	@Prs_ApproxNoOfConn_D7 [float],
	@Prs_DailyAvgPrsM_F7 [float],
	@Prs_ApproxNoOfConn_D8 [float],
	@Prs_DailyAvgPrsM_F8 [float],
	@Prs_ApproxNoOfConn_D9 [float],
	@Prs_DailyAvgPrsM_F9 [float],
	@Prs_ApproxNoOfConn_D10 [float],
	@Prs_DailyAvgPrsM_F10 [float],

	@Prs_ErrorMarg_F26 [float],

	@Interm_Conn_D7  [float],
	@Interm_Conn_D8  [float],
	@Interm_Conn_D9  [float],
	@Interm_Conn_D10 [float],
	@Interm_Days_F7  [float],
	@Interm_Days_F8  [float],
	@Interm_Days_F9  [float],
	@Interm_Days_F10 [float],
	@Interm_Hour_H7  [float],
	@Interm_Hour_H8  [float],
	@Interm_Hour_H9  [float],
	@Interm_Hour_H10 [float],
	@Interm_ErrorMarg_H26 [float],

	@FinancData_G6 [float],
	@FinancData_K6 nvarchar(400),
	@FinancData_G8 [float],
	@FinancData_D26 [float], 
	@FinancData_G35 [float], 

	@MatrixOneIn_SelectedOption [int], 
	@MatrixOneIn_C11 [float], 
	@MatrixOneIn_C12 [float],
	@MatrixOneIn_C13 [float],
	@MatrixOneIn_C14 [float],
	@MatrixOneIn_C21 [float],
	@MatrixOneIn_C22 [float],
	@MatrixOneIn_C23 [float],
	@MatrixOneIn_C24 [float],
	@MatrixOneIn_D21 [float],
	@MatrixOneIn_D22 [float],
	@MatrixOneIn_D23 [float],
	@MatrixOneIn_D24 [float],
	@MatrixOneIn_E11 [float],
	@MatrixOneIn_E12 [float],
	@MatrixOneIn_E13 [float],
	@MatrixOneIn_E14 [float],
	@MatrixOneIn_E21 [float],
	@MatrixOneIn_E22 [float],
	@MatrixOneIn_E23 [float],
	@MatrixOneIn_E24 [float],
	@MatrixOneIn_F11 [float],
	@MatrixOneIn_F12 [float],
	@MatrixOneIn_F13 [float],
	@MatrixOneIn_F14 [float],
	@MatrixOneIn_F21 [float],
	@MatrixOneIn_F22 [float],
	@MatrixOneIn_F23 [float],
	@MatrixOneIn_F24 [float],
	@MatrixOneIn_G11 [float],
	@MatrixOneIn_G12 [float],
	@MatrixOneIn_G13 [float],
	@MatrixOneIn_G14 [float],
	@MatrixOneIn_G21 [float],
	@MatrixOneIn_G22 [float],
	@MatrixOneIn_G23 [float],
	@MatrixOneIn_G24 [float],
	@MatrixOneIn_H11 [float],
	@MatrixOneIn_H12 [float],
	@MatrixOneIn_H13 [float],
	@MatrixOneIn_H14 [float],
	@MatrixOneIn_H21 [float],
	@MatrixOneIn_H22 [float],
	@MatrixOneIn_H23 [float],
	@MatrixOneIn_H24 [float],

	@MatrixTwoIn_SelectedOption [int], 
	@MatrixTwoIn_D21 [float],
	@MatrixTwoIn_D22 [float],
	@MatrixTwoIn_D23 [float],
	@MatrixTwoIn_D24 [float],
	@MatrixTwoIn_E11 [float],
	@MatrixTwoIn_E12 [float],
	@MatrixTwoIn_E13 [float],
	@MatrixTwoIn_E14 [float],
	@MatrixTwoIn_E21 [float],
	@MatrixTwoIn_E22 [float],
	@MatrixTwoIn_E23 [float],
	@MatrixTwoIn_E24 [float],
	@MatrixTwoIn_F11 [float],
	@MatrixTwoIn_F12 [float],
	@MatrixTwoIn_F13 [float],
	@MatrixTwoIn_F14 [float],
	@MatrixTwoIn_F21 [float],
	@MatrixTwoIn_F22 [float],
	@MatrixTwoIn_F23 [float],
	@MatrixTwoIn_F24 [float],
	@MatrixTwoIn_G11 [float],
	@MatrixTwoIn_G12 [float],
	@MatrixTwoIn_G13 [float],
	@MatrixTwoIn_G14 [float],
	@MatrixTwoIn_G21 [float],
	@MatrixTwoIn_G22 [float],
	@MatrixTwoIn_G23 [float],
	@MatrixTwoIn_G24 [float],
	@MatrixTwoIn_H11 [float],
	@MatrixTwoIn_H12 [float],
	@MatrixTwoIn_H13 [float],
	@MatrixTwoIn_H14 [float],
	@MatrixTwoIn_H21 [float],
	@MatrixTwoIn_H22 [float],
	@MatrixTwoIn_H23 [float],
	@MatrixTwoIn_H24 [float],

	-- Output
	@SystemInputVolume_B19 [float],
	@SystemInputVolumeErrorMargin_B21 [float],
	@AuthorizedConsumption_K12 [float],
	@AuthorizedConsumptionErrorMargin_K15 [float],
	@WaterLosses_K29 [float], 
	@WaterLossesErrorMargin_K31 [float],
	@BilledAuthorizedConsumption_T8 [float], 
	@UnbilledAuthorizedConsumption_T16 [float], 
	@UnbilledAuthorizedConsumptionErrorMargin_T20 [float],
	@CommercialLosses_T26 [float], 
	@CommercialLossesErrorMargin_T29 [float], 
	@PhysicalLossesM3_T34 [float], 
	@PhyscialLossesErrorMargin_AH35 [float], 
	@BilledMeteredConsumption_AC4 [float],
	@BilledUnmeteredConsumption_AC9 [float],
	@UnbilledMeteredConsumption_AC14 [float],
	@UnbilledUnmeteredConsumption_AC19 [float],
	@UnbilledUnmeteredConsumptionErrorMargin_AO20 [float],
	@UnauthorizedConsumption_AC24 [float],
	@UnauthorizedConsumptionErrorMargin_AO25 [float],
	@CustomerMeterInaccuraciesAndErrorsM3_AC29 [float],
	@CustomerMeterInaccuraciesAndErrorsErrorMargin_AO30 [float],
	@RevenueWaterM3_AY8 [float],
	@NonRevenueWaterM3_AY24 [float],
	@NonRevenueWaterErrorMargin_AY26 [float], 

	@id [int] OUTPUT
AS
BEGIN
	SET NOCOUNT ON;



DECLARE 
	@OldYearNo [int], 
	@OldMonthNo [int],
	@OldZoneId [int], 
	@OldIsArchive BIT;
SELECT 
	@OldYearNo = YearNo, 
	@OldMonthNo = MonthNo,
	@OldZoneId = ZoneId, 
	@OldIsArchive = IsArchive 
FROM 
	[dbo].[tbWbEasyCalcData] 
WHERE 
	WbEasyCalcDataId = @id;
--SELECT 
--	@IsArchiveOld = COALESCE(@IsArchiveOld, 0);



DECLARE 
	@ChangedId [int];


IF (@IsArchive=1) BEGIN
	SELECT
		@ChangedId = WbEasyCalcDataId
	FROM
		[dbo].[tbWbEasyCalcData]
	WHERE
		IsArchive = @IsArchive AND
		ZoneId = @ZoneId AND 
		YearNo = @YearNo AND 
		MonthNo = @MonthNo;

	UPDATE [dbo].[tbWbEasyCalcData] SET
		IsArchive = 0,
		IsAccepted = 0
	WHERE
		ZoneId = @ZoneId AND 
		YearNo = @YearNo AND 
		MonthNo = @MonthNo;
END;

IF (@id = 0) BEGIN

	INSERT INTO [dbo].[tbWbEasyCalcData] (
		CreateLogin,
		CreateDate,
		ModifyLogin,
		ModifyDate,

		ZoneId, 
		YearNo, 
		MonthNo,
		[Description],
		IsArchive,
		IsAccepted,

		-- Input
		Start_PeriodDays_M21,

		SysInput_Desc_B6,
		SysInput_Desc_B7,
		SysInput_Desc_B8,
		SysInput_Desc_B9,
		SysInput_SystemInputVolumeM3_D6,
		SysInput_SystemInputVolumeError_F6,
		SysInput_SystemInputVolumeM3_D7,
		SysInput_SystemInputVolumeError_F7,
		SysInput_SystemInputVolumeM3_D8,
		SysInput_SystemInputVolumeError_F8,
		SysInput_SystemInputVolumeM3_D9,
		SysInput_SystemInputVolumeError_F9,

		BilledCons_Desc_B8   ,
		BilledCons_Desc_B9   ,
		BilledCons_Desc_B10  ,
		BilledCons_Desc_B11  ,
		BilledCons_Desc_F8   ,
		BilledCons_Desc_F9   ,
		BilledCons_Desc_F10  ,
		BilledCons_Desc_F11  ,
		UnbilledCons_Desc_D8 ,
		UnbilledCons_Desc_D9 ,
		UnbilledCons_Desc_D10,
		UnbilledCons_Desc_D11,
		UnbilledCons_Desc_F6 ,
		UnbilledCons_Desc_F7 ,
		UnbilledCons_Desc_F8 ,
		UnbilledCons_Desc_F9 ,
		UnbilledCons_Desc_F10,
		UnbilledCons_Desc_F11,
		UnauthCons_Desc_B18  ,
		UnauthCons_Desc_B19  ,
		UnauthCons_Desc_B20  ,
		UnauthCons_Desc_B21  ,
		MetErrors_Desc_D12   ,
		MetErrors_Desc_D13   ,
		MetErrors_Desc_D14   ,
		MetErrors_Desc_D15   ,
		Network_Desc_B7      ,
		Network_Desc_B8      ,
		Network_Desc_B9      ,
		Network_Desc_B10     ,
		Interm_Area_B7       ,
		Interm_Area_B8       ,
		Interm_Area_B9       ,
		Interm_Area_B10      ,

		BilledCons_BilledMetConsBulkWatSupExpM3_D6,
		BilledCons_BilledUnmetConsBulkWatSupExpM3_H6,

		BilledCons_UnbMetConsM3_D8,
		BilledCons_UnbMetConsM3_D9,
		BilledCons_UnbMetConsM3_D10,
		BilledCons_UnbMetConsM3_D11,
		BilledCons_UnbUnmetConsM3_H8, 
		BilledCons_UnbUnmetConsM3_H9, 
		BilledCons_UnbUnmetConsM3_H10, 
		BilledCons_UnbUnmetConsM3_H11, 

		UnbilledCons_MetConsBulkWatSupExpM3_D6,

		UnbilledCons_UnbMetConsM3_D8,
		UnbilledCons_UnbMetConsM3_D9,
		UnbilledCons_UnbMetConsM3_D10,
		UnbilledCons_UnbMetConsM3_D11,
		UnbilledCons_UnbUnmetConsM3_H6,
		UnbilledCons_UnbUnmetConsM3_H7,
		UnbilledCons_UnbUnmetConsM3_H8,
		UnbilledCons_UnbUnmetConsM3_H9,
		UnbilledCons_UnbUnmetConsM3_H10,
		UnbilledCons_UnbUnmetConsM3_H11,
		UnbilledCons_UnbUnmetConsError_J6,
		UnbilledCons_UnbUnmetConsError_J7,
		UnbilledCons_UnbUnmetConsError_J8,
		UnbilledCons_UnbUnmetConsError_J9,
		UnbilledCons_UnbUnmetConsError_J10,
		UnbilledCons_UnbUnmetConsError_J11,

		UnauthCons_IllegalConnDomEstNo_D6,
		UnauthCons_IllegalConnDomPersPerHouse_H6,
		UnauthCons_IllegalConnDomConsLitPerPersDay_J6,
		UnauthCons_IllegalConnDomErrorMargin_F6,
		UnauthCons_IllegalConnOthersErrorMargin_F10,

		IllegalConnectionsOthersEstimatedNumber_D10,
		IllegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10,

		UnauthCons_MeterTampBypEtcEstNo_D14,
		UnauthCons_MeterTampBypEtcErrorMargin_F14,
		UnauthCons_MeterTampBypEtcConsLitPerCustDay_J14,

		UnauthCons_OthersErrorMargin_F18, 
		UnauthCons_OthersErrorMargin_F19, 
		UnauthCons_OthersErrorMargin_F20, 
		UnauthCons_OthersErrorMargin_F21, 
		UnauthCons_OthersM3PerDay_J18,
		UnauthCons_OthersM3PerDay_J19,
		UnauthCons_OthersM3PerDay_J20,
		UnauthCons_OthersM3PerDay_J21,

		MetErrors_DetailedManualSpec_J6,
		MetErrors_BilledMetConsWoBulkSupMetUndrreg_H8,
		MetErrors_BilledMetConsWoBulkSupErrorMargin_N8,

		MetErrors_Total_F12,
		MetErrors_Total_F13,
		MetErrors_Total_F14,
		MetErrors_Total_F15,
		MetErrors_Meter_H12,
		MetErrors_Meter_H13,
		MetErrors_Meter_H14,
		MetErrors_Meter_H15,
		MetErrors_Error_N12,
		MetErrors_Error_N13,
		MetErrors_Error_N14,
		MetErrors_Error_N15,

		MeteredBulkSupplyExportErrorMargin_N32,
		UnbilledMeteredConsumptionWithoutBulkSupplyErrorMargin_N34,
		CorruptMeterReadingPracticessErrorMargin_N38,
		DataHandlingErrorsOffice_L40,
		DataHandlingErrorsOfficeErrorMargin_N40,

		MetErrors_MetBulkSupExpMetUnderreg_H32,
		MetErrors_UnbillMetConsWoBulkSupplMetUndrreg_H34,
		MetErrors_CorruptMetReadPractMetUndrreg_H38,
		Network_DistributionAndTransmissionMains_D7,
		Network_DistributionAndTransmissionMains_D8,
		Network_DistributionAndTransmissionMains_D9,
		Network_DistributionAndTransmissionMains_D10,
		Network_NoOfConnOfRegCustomers_H10,
		Network_NoOfInactAccountsWSvcConns_H18,
		Network_AvgLenOfSvcConnFromBoundaryToMeterM_H32,

		Network_PossibleUnd_D30,
		Network_NoCustomers_H7,
		Network_ErrorMargin_J7,
		Network_ErrorMargin_J10,
		Network_ErrorMargin_J18,
		Network_ErrorMargin_J32,
		Network_ErrorMargin_D35,

		Prs_Area_B7,
		Prs_Area_B8,
		Prs_Area_B9,
		Prs_Area_B10,
		Prs_ApproxNoOfConn_D7,
		Prs_DailyAvgPrsM_F7,
		Prs_ApproxNoOfConn_D8,
		Prs_DailyAvgPrsM_F8,
		Prs_ApproxNoOfConn_D9,
		Prs_DailyAvgPrsM_F9,
		Prs_ApproxNoOfConn_D10,
		Prs_DailyAvgPrsM_F10,

		Prs_ErrorMarg_F26,

		Interm_Conn_D7,
		Interm_Conn_D8,
		Interm_Conn_D9,
		Interm_Conn_D10,
		Interm_Days_F7,
		Interm_Days_F8,
		Interm_Days_F9,
		Interm_Days_F10,
		Interm_Hour_H7,
		Interm_Hour_H8,
		Interm_Hour_H9,
		Interm_Hour_H10,
		Interm_ErrorMarg_H26,

		FinancData_G6,
		FinancData_K6,
		FinancData_G8,
		FinancData_D26,
		FinancData_G35,

		MatrixOneIn_SelectedOption,
		MatrixOneIn_C11,
		MatrixOneIn_C12,
		MatrixOneIn_C13,
		MatrixOneIn_C14,
		MatrixOneIn_C21,
		MatrixOneIn_C22,
		MatrixOneIn_C23,
		MatrixOneIn_C24,
		MatrixOneIn_D21,
		MatrixOneIn_D22,
		MatrixOneIn_D23,
		MatrixOneIn_D24,
		MatrixOneIn_E11,
		MatrixOneIn_E12,
		MatrixOneIn_E13,
		MatrixOneIn_E14,
		MatrixOneIn_E21,
		MatrixOneIn_E22,
		MatrixOneIn_E23,
		MatrixOneIn_E24,
		MatrixOneIn_F11,
		MatrixOneIn_F12,
		MatrixOneIn_F13,
		MatrixOneIn_F14,
		MatrixOneIn_F21,
		MatrixOneIn_F22,
		MatrixOneIn_F23,
		MatrixOneIn_F24,
		MatrixOneIn_G11,
		MatrixOneIn_G12,
		MatrixOneIn_G13,
		MatrixOneIn_G14,
		MatrixOneIn_G21,
		MatrixOneIn_G22,
		MatrixOneIn_G23,
		MatrixOneIn_G24,
		MatrixOneIn_H11,
		MatrixOneIn_H12,
		MatrixOneIn_H13,
		MatrixOneIn_H14,
		MatrixOneIn_H21,
		MatrixOneIn_H22,
		MatrixOneIn_H23,
		MatrixOneIn_H24,

		MatrixTwoIn_SelectedOption,
		MatrixTwoIn_D21,
		MatrixTwoIn_D22,
		MatrixTwoIn_D23,
		MatrixTwoIn_D24,
		MatrixTwoIn_E11,
		MatrixTwoIn_E12,
		MatrixTwoIn_E13,
		MatrixTwoIn_E14,
		MatrixTwoIn_E21,
		MatrixTwoIn_E22,
		MatrixTwoIn_E23,
		MatrixTwoIn_E24,
		MatrixTwoIn_F11,
		MatrixTwoIn_F12,
		MatrixTwoIn_F13,
		MatrixTwoIn_F14,
		MatrixTwoIn_F21,
		MatrixTwoIn_F22,
		MatrixTwoIn_F23,
		MatrixTwoIn_F24,
		MatrixTwoIn_G11,
		MatrixTwoIn_G12,
		MatrixTwoIn_G13,
		MatrixTwoIn_G14,
		MatrixTwoIn_G21,
		MatrixTwoIn_G22,
		MatrixTwoIn_G23,
		MatrixTwoIn_G24,
		MatrixTwoIn_H11,
		MatrixTwoIn_H12,
		MatrixTwoIn_H13,
		MatrixTwoIn_H14,
		MatrixTwoIn_H21,
		MatrixTwoIn_H22,
		MatrixTwoIn_H23,
		MatrixTwoIn_H24,

		-- Output
		SystemInputVolume_B19,
		SystemInputVolumeErrorMargin_B21,	
		AuthorizedConsumption_K12,
		AuthorizedConsumptionErrorMargin_K15,
		WaterLosses_K29, 
		WaterLossesErrorMargin_K31,
		BilledAuthorizedConsumption_T8, 
		UnbilledAuthorizedConsumption_T16, 
		UnbilledAuthorizedConsumptionErrorMargin_T20,
		CommercialLosses_T26, 
		CommercialLossesErrorMargin_T29, 
		PhysicalLossesM3_T34, 
		PhyscialLossesErrorMargin_AH35, 
		BilledMeteredConsumption_AC4,
		BilledUnmeteredConsumption_AC9,
		UnbilledMeteredConsumption_AC14,
		UnbilledUnmeteredConsumption_AC19,
		UnbilledUnmeteredConsumptionErrorMargin_AO20,
		UnauthorizedConsumption_AC24,
		UnauthorizedConsumptionErrorMargin_AO25,
		CustomerMeterInaccuraciesAndErrorsM3_AC29,
		CustomerMeterInaccuraciesAndErrorsErrorMargin_AO30,
		RevenueWaterM3_AY8,
		NonRevenueWaterM3_AY24,
		NonRevenueWaterErrorMargin_AY26
	) VALUES (
		@UserName,
		GETDATE(),
		@UserName,
		GETDATE(),

		@ZoneId, 
		@YearNo, 
		@MonthNo,
		@Description,
		@IsArchive,
		@IsAccepted,

		-- input
		@Start_PeriodDays_M21,

		@SysInput_Desc_B6,
		@SysInput_Desc_B7,
		@SysInput_Desc_B8,
		@SysInput_Desc_B9,
		@SysInput_SystemInputVolumeM3_D6,
		@SysInput_SystemInputVolumeError_F6,
		@SysInput_SystemInputVolumeM3_D7,
		@SysInput_SystemInputVolumeError_F7,
		@SysInput_SystemInputVolumeM3_D8,
		@SysInput_SystemInputVolumeError_F8,
		@SysInput_SystemInputVolumeM3_D9,
		@SysInput_SystemInputVolumeError_F9,

		@BilledCons_Desc_B8   ,
		@BilledCons_Desc_B9   ,
		@BilledCons_Desc_B10  ,
		@BilledCons_Desc_B11  ,
		@BilledCons_Desc_F8   ,
		@BilledCons_Desc_F9   ,
		@BilledCons_Desc_F10  ,
		@BilledCons_Desc_F11  ,
		@UnbilledCons_Desc_D8 ,
		@UnbilledCons_Desc_D9 ,
		@UnbilledCons_Desc_D10,
		@UnbilledCons_Desc_D11,
		@UnbilledCons_Desc_F6 ,
		@UnbilledCons_Desc_F7 ,
		@UnbilledCons_Desc_F8 ,
		@UnbilledCons_Desc_F9 ,
		@UnbilledCons_Desc_F10,
		@UnbilledCons_Desc_F11,
		@UnauthCons_Desc_B18  ,
		@UnauthCons_Desc_B19  ,
		@UnauthCons_Desc_B20  ,
		@UnauthCons_Desc_B21  ,
		@MetErrors_Desc_D12   ,
		@MetErrors_Desc_D13   ,
		@MetErrors_Desc_D14   ,
		@MetErrors_Desc_D15   ,
		@Network_Desc_B7      ,
		@Network_Desc_B8      ,
		@Network_Desc_B9      ,
		@Network_Desc_B10     ,
		@Interm_Area_B7       ,
		@Interm_Area_B8       ,
		@Interm_Area_B9       ,
		@Interm_Area_B10      ,

		@BilledCons_BilledMetConsBulkWatSupExpM3_D6,
		@BilledCons_BilledUnmetConsBulkWatSupExpM3_H6,

		@BilledCons_UnbMetConsM3_D8,
		@BilledCons_UnbMetConsM3_D9,
		@BilledCons_UnbMetConsM3_D10,
		@BilledCons_UnbMetConsM3_D11,
		@BilledCons_UnbUnmetConsM3_H8, 
		@BilledCons_UnbUnmetConsM3_H9, 
		@BilledCons_UnbUnmetConsM3_H10, 
		@BilledCons_UnbUnmetConsM3_H11, 

		@UnbilledCons_MetConsBulkWatSupExpM3_D6,

		@UnbilledCons_UnbMetConsM3_D8,
		@UnbilledCons_UnbMetConsM3_D9,
		@UnbilledCons_UnbMetConsM3_D10,
		@UnbilledCons_UnbMetConsM3_D11,
		@UnbilledCons_UnbUnmetConsM3_H6,
		@UnbilledCons_UnbUnmetConsM3_H7,
		@UnbilledCons_UnbUnmetConsM3_H8,
		@UnbilledCons_UnbUnmetConsM3_H9,
		@UnbilledCons_UnbUnmetConsM3_H10,
		@UnbilledCons_UnbUnmetConsM3_H11,
		@UnbilledCons_UnbUnmetConsError_J6,
		@UnbilledCons_UnbUnmetConsError_J7,
		@UnbilledCons_UnbUnmetConsError_J8,
		@UnbilledCons_UnbUnmetConsError_J9,
		@UnbilledCons_UnbUnmetConsError_J10,
		@UnbilledCons_UnbUnmetConsError_J11,

		@UnauthCons_IllegalConnDomEstNo_D6,
		@UnauthCons_IllegalConnDomPersPerHouse_H6,
		@UnauthCons_IllegalConnDomConsLitPerPersDay_J6,
		@UnauthCons_IllegalConnDomErrorMargin_F6,
		@UnauthCons_IllegalConnOthersErrorMargin_F10,

		@IllegalConnectionsOthersEstimatedNumber_D10,
		@IllegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10,

		@UnauthCons_MeterTampBypEtcEstNo_D14,
		@UnauthCons_MeterTampBypEtcErrorMargin_F14,
		@UnauthCons_MeterTampBypEtcConsLitPerCustDay_J14,

		@UnauthCons_OthersErrorMargin_F18, 
		@UnauthCons_OthersErrorMargin_F19, 
		@UnauthCons_OthersErrorMargin_F20, 
		@UnauthCons_OthersErrorMargin_F21, 
		@UnauthCons_OthersM3PerDay_J18,
		@UnauthCons_OthersM3PerDay_J19,
		@UnauthCons_OthersM3PerDay_J20,
		@UnauthCons_OthersM3PerDay_J21,

		@MetErrors_DetailedManualSpec_J6,
		@MetErrors_BilledMetConsWoBulkSupMetUndrreg_H8,
		@MetErrors_BilledMetConsWoBulkSupErrorMargin_N8,

		@MetErrors_Total_F12,
		@MetErrors_Total_F13,
		@MetErrors_Total_F14,
		@MetErrors_Total_F15,
		@MetErrors_Meter_H12,
		@MetErrors_Meter_H13,
		@MetErrors_Meter_H14,
		@MetErrors_Meter_H15,
		@MetErrors_Error_N12,
		@MetErrors_Error_N13,
		@MetErrors_Error_N14,
		@MetErrors_Error_N15,

		@MeteredBulkSupplyExportErrorMargin_N32,
		@UnbilledMeteredConsumptionWithoutBulkSupplyErrorMargin_N34,
		@CorruptMeterReadingPracticessErrorMargin_N38,
		@DataHandlingErrorsOffice_L40,
		@DataHandlingErrorsOfficeErrorMargin_N40,

		@MetErrors_MetBulkSupExpMetUnderreg_H32,
		@MetErrors_UnbillMetConsWoBulkSupplMetUndrreg_H34,
		@MetErrors_CorruptMetReadPractMetUndrreg_H38,
		@Network_DistributionAndTransmissionMains_D7,
		@Network_DistributionAndTransmissionMains_D8,
		@Network_DistributionAndTransmissionMains_D9,
		@Network_DistributionAndTransmissionMains_D10,
		@Network_NoOfConnOfRegCustomers_H10,
		@Network_NoOfInactAccountsWSvcConns_H18,
		@Network_AvgLenOfSvcConnFromBoundaryToMeterM_H32,

		@Network_PossibleUnd_D30,
		@Network_NoCustomers_H7,
		@Network_ErrorMargin_J7,
		@Network_ErrorMargin_J10,
		@Network_ErrorMargin_J18,
		@Network_ErrorMargin_J32,
		@Network_ErrorMargin_D35,

		@Prs_Area_B7,
		@Prs_Area_B8,
		@Prs_Area_B9,
		@Prs_Area_B10,
		@Prs_ApproxNoOfConn_D7,
		@Prs_DailyAvgPrsM_F7,
		@Prs_ApproxNoOfConn_D8,
		@Prs_DailyAvgPrsM_F8,
		@Prs_ApproxNoOfConn_D9,
		@Prs_DailyAvgPrsM_F9,
		@Prs_ApproxNoOfConn_D10,
		@Prs_DailyAvgPrsM_F10,

		@Prs_ErrorMarg_F26,

		@Interm_Conn_D7,
		@Interm_Conn_D8,
		@Interm_Conn_D9,
		@Interm_Conn_D10,
		@Interm_Days_F7,
		@Interm_Days_F8,
		@Interm_Days_F9,
		@Interm_Days_F10,
		@Interm_Hour_H7,
		@Interm_Hour_H8,
		@Interm_Hour_H9,
		@Interm_Hour_H10,
		@Interm_ErrorMarg_H26,

		@FinancData_G6,
		@FinancData_K6,
		@FinancData_G8,
		@FinancData_D26,
		@FinancData_G35,

		@MatrixOneIn_SelectedOption,
		@MatrixOneIn_C11,
		@MatrixOneIn_C12,
		@MatrixOneIn_C13,
		@MatrixOneIn_C14,
		@MatrixOneIn_C21,
		@MatrixOneIn_C22,
		@MatrixOneIn_C23,
		@MatrixOneIn_C24,
		@MatrixOneIn_D21,
		@MatrixOneIn_D22,
		@MatrixOneIn_D23,
		@MatrixOneIn_D24,
		@MatrixOneIn_E11,
		@MatrixOneIn_E12,
		@MatrixOneIn_E13,
		@MatrixOneIn_E14,
		@MatrixOneIn_E21,
		@MatrixOneIn_E22,
		@MatrixOneIn_E23,
		@MatrixOneIn_E24,
		@MatrixOneIn_F11,
		@MatrixOneIn_F12,
		@MatrixOneIn_F13,
		@MatrixOneIn_F14,
		@MatrixOneIn_F21,
		@MatrixOneIn_F22,
		@MatrixOneIn_F23,
		@MatrixOneIn_F24,
		@MatrixOneIn_G11,
		@MatrixOneIn_G12,
		@MatrixOneIn_G13,
		@MatrixOneIn_G14,
		@MatrixOneIn_G21,
		@MatrixOneIn_G22,
		@MatrixOneIn_G23,
		@MatrixOneIn_G24,
		@MatrixOneIn_H11,
		@MatrixOneIn_H12,
		@MatrixOneIn_H13,
		@MatrixOneIn_H14,
		@MatrixOneIn_H21,
		@MatrixOneIn_H22,
		@MatrixOneIn_H23,
		@MatrixOneIn_H24,

		@MatrixTwoIn_SelectedOption,
		@MatrixTwoIn_D21,
		@MatrixTwoIn_D22,
		@MatrixTwoIn_D23,
		@MatrixTwoIn_D24,
		@MatrixTwoIn_E11,
		@MatrixTwoIn_E12,
		@MatrixTwoIn_E13,
		@MatrixTwoIn_E14,
		@MatrixTwoIn_E21,
		@MatrixTwoIn_E22,
		@MatrixTwoIn_E23,
		@MatrixTwoIn_E24,
		@MatrixTwoIn_F11,
		@MatrixTwoIn_F12,
		@MatrixTwoIn_F13,
		@MatrixTwoIn_F14,
		@MatrixTwoIn_F21,
		@MatrixTwoIn_F22,
		@MatrixTwoIn_F23,
		@MatrixTwoIn_F24,
		@MatrixTwoIn_G11,
		@MatrixTwoIn_G12,
		@MatrixTwoIn_G13,
		@MatrixTwoIn_G14,
		@MatrixTwoIn_G21,
		@MatrixTwoIn_G22,
		@MatrixTwoIn_G23,
		@MatrixTwoIn_G24,
		@MatrixTwoIn_H11,
		@MatrixTwoIn_H12,
		@MatrixTwoIn_H13,
		@MatrixTwoIn_H14,
		@MatrixTwoIn_H21,
		@MatrixTwoIn_H22,
		@MatrixTwoIn_H23,
		@MatrixTwoIn_H24,

	
		-- Output
		@SystemInputVolume_B19,
		@SystemInputVolumeErrorMargin_B21,		
		@AuthorizedConsumption_K12,		
		@AuthorizedConsumptionErrorMargin_K15,
		@WaterLosses_K29, 
		@WaterLossesErrorMargin_K31,		
		@BilledAuthorizedConsumption_T8, 
		@UnbilledAuthorizedConsumption_T16, 
		@UnbilledAuthorizedConsumptionErrorMargin_T20,
		@CommercialLosses_T26, 
		@CommercialLossesErrorMargin_T29, 
		@PhysicalLossesM3_T34, 
		@PhyscialLossesErrorMargin_AH35, 		
		@BilledMeteredConsumption_AC4,
		@BilledUnmeteredConsumption_AC9,
		@UnbilledMeteredConsumption_AC14,		
		@UnbilledUnmeteredConsumption_AC19,
		@UnbilledUnmeteredConsumptionErrorMargin_AO20,
		@UnauthorizedConsumption_AC24,
		@UnauthorizedConsumptionErrorMargin_AO25,
		@CustomerMeterInaccuraciesAndErrorsM3_AC29,
		@CustomerMeterInaccuraciesAndErrorsErrorMargin_AO30,		
		@RevenueWaterM3_AY8,
		@NonRevenueWaterM3_AY24,
		@NonRevenueWaterErrorMargin_AY26
	);

	SELECT @id = SCOPE_IDENTITY();

END ELSE BEGIN
	UPDATE [dbo].[tbWbEasyCalcData] SET
		ModifyLogin = @UserName,
		ModifyDate = GETDATE(), 
		
		ZoneId = @ZoneId, 
		YearNo = @YearNo, 
		MonthNo = @MonthNo,
		[Description] = @Description,
		IsArchive = @IsArchive,
		IsAccepted = @IsAccepted,

		-- input
		Start_PeriodDays_M21 = @Start_PeriodDays_M21,

		SysInput_Desc_B6 = @SysInput_Desc_B6,
		SysInput_Desc_B7 = @SysInput_Desc_B7,
		SysInput_Desc_B8 = @SysInput_Desc_B8,
		SysInput_Desc_B9 = @SysInput_Desc_B9,
        SysInput_SystemInputVolumeM3_D6  = @SysInput_SystemInputVolumeM3_D6,
        SysInput_SystemInputVolumeError_F6  = @SysInput_SystemInputVolumeError_F6,
        SysInput_SystemInputVolumeM3_D7  = @SysInput_SystemInputVolumeM3_D7,
        SysInput_SystemInputVolumeError_F7  = @SysInput_SystemInputVolumeError_F7,
        SysInput_SystemInputVolumeM3_D8  = @SysInput_SystemInputVolumeM3_D8,
        SysInput_SystemInputVolumeError_F8  = @SysInput_SystemInputVolumeError_F8,
        SysInput_SystemInputVolumeM3_D9  = @SysInput_SystemInputVolumeM3_D9,
        SysInput_SystemInputVolumeError_F9  = @SysInput_SystemInputVolumeError_F9,

        BilledCons_Desc_B8      = @BilledCons_Desc_B8     ,
        BilledCons_Desc_B9      = @BilledCons_Desc_B9     ,
        BilledCons_Desc_B10     = @BilledCons_Desc_B10    ,
        BilledCons_Desc_B11     = @BilledCons_Desc_B11    ,
        BilledCons_Desc_F8      = @BilledCons_Desc_F8     ,
        BilledCons_Desc_F9      = @BilledCons_Desc_F9     ,
        BilledCons_Desc_F10     = @BilledCons_Desc_F10    ,
        BilledCons_Desc_F11     = @BilledCons_Desc_F11    ,
        UnbilledCons_Desc_D8    = @UnbilledCons_Desc_D8   ,
        UnbilledCons_Desc_D9    = @UnbilledCons_Desc_D9   ,
        UnbilledCons_Desc_D10   = @UnbilledCons_Desc_D10  ,
        UnbilledCons_Desc_D11   = @UnbilledCons_Desc_D11  ,
        UnbilledCons_Desc_F6    = @UnbilledCons_Desc_F6   ,
        UnbilledCons_Desc_F7    = @UnbilledCons_Desc_F7   ,
        UnbilledCons_Desc_F8    = @UnbilledCons_Desc_F8   ,
        UnbilledCons_Desc_F9    = @UnbilledCons_Desc_F9   ,
        UnbilledCons_Desc_F10   = @UnbilledCons_Desc_F10  ,
        UnbilledCons_Desc_F11   = @UnbilledCons_Desc_F11  ,
        UnauthCons_Desc_B18     = @UnauthCons_Desc_B18    ,
        UnauthCons_Desc_B19     = @UnauthCons_Desc_B19    ,
        UnauthCons_Desc_B20     = @UnauthCons_Desc_B20    ,
        UnauthCons_Desc_B21     = @UnauthCons_Desc_B21    ,
        MetErrors_Desc_D12      = @MetErrors_Desc_D12     ,
        MetErrors_Desc_D13      = @MetErrors_Desc_D13     ,
        MetErrors_Desc_D14      = @MetErrors_Desc_D14     ,
        MetErrors_Desc_D15      = @MetErrors_Desc_D15     ,
        Network_Desc_B7         = @Network_Desc_B7        ,
        Network_Desc_B8         = @Network_Desc_B8        ,
        Network_Desc_B9         = @Network_Desc_B9        ,
        Network_Desc_B10        = @Network_Desc_B10       ,
        Interm_Area_B7          = @Interm_Area_B7         ,
        Interm_Area_B8          = @Interm_Area_B8         ,
        Interm_Area_B9          = @Interm_Area_B9         ,
        Interm_Area_B10         = @Interm_Area_B10        ,
										  
        BilledCons_BilledMetConsBulkWatSupExpM3_D6  = @BilledCons_BilledMetConsBulkWatSupExpM3_D6,
        BilledCons_BilledUnmetConsBulkWatSupExpM3_H6  = @BilledCons_BilledUnmetConsBulkWatSupExpM3_H6,

		BilledCons_UnbMetConsM3_D8 = @BilledCons_UnbMetConsM3_D8,
		BilledCons_UnbMetConsM3_D9 = @BilledCons_UnbMetConsM3_D9,
		BilledCons_UnbMetConsM3_D10 = @BilledCons_UnbMetConsM3_D10,
		BilledCons_UnbMetConsM3_D11 = @BilledCons_UnbMetConsM3_D11,
		BilledCons_UnbUnmetConsM3_H8 = @BilledCons_UnbUnmetConsM3_H8, 
		BilledCons_UnbUnmetConsM3_H9 = @BilledCons_UnbUnmetConsM3_H9, 
		BilledCons_UnbUnmetConsM3_H10 = @BilledCons_UnbUnmetConsM3_H10, 
		BilledCons_UnbUnmetConsM3_H11 = @BilledCons_UnbUnmetConsM3_H11, 

        UnbilledCons_MetConsBulkWatSupExpM3_D6  = @UnbilledCons_MetConsBulkWatSupExpM3_D6,

		UnbilledCons_UnbMetConsM3_D8 = @UnbilledCons_UnbMetConsM3_D8,
		UnbilledCons_UnbMetConsM3_D9 = @UnbilledCons_UnbMetConsM3_D9,
		UnbilledCons_UnbMetConsM3_D10 = @UnbilledCons_UnbMetConsM3_D10,
		UnbilledCons_UnbMetConsM3_D11 = @UnbilledCons_UnbMetConsM3_D11,
		UnbilledCons_UnbUnmetConsM3_H6 = @UnbilledCons_UnbUnmetConsM3_H6,
		UnbilledCons_UnbUnmetConsM3_H7 = @UnbilledCons_UnbUnmetConsM3_H7,
		UnbilledCons_UnbUnmetConsM3_H8 = @UnbilledCons_UnbUnmetConsM3_H8,
		UnbilledCons_UnbUnmetConsM3_H9 = @UnbilledCons_UnbUnmetConsM3_H9,
		UnbilledCons_UnbUnmetConsM3_H10 = @UnbilledCons_UnbUnmetConsM3_H10,
		UnbilledCons_UnbUnmetConsM3_H11 = @UnbilledCons_UnbUnmetConsM3_H11,
		UnbilledCons_UnbUnmetConsError_J6 = @UnbilledCons_UnbUnmetConsError_J6,
		UnbilledCons_UnbUnmetConsError_J7 = @UnbilledCons_UnbUnmetConsError_J7,
		UnbilledCons_UnbUnmetConsError_J8 = @UnbilledCons_UnbUnmetConsError_J8,
		UnbilledCons_UnbUnmetConsError_J9 = @UnbilledCons_UnbUnmetConsError_J9,
		UnbilledCons_UnbUnmetConsError_J10 = @UnbilledCons_UnbUnmetConsError_J10,
		UnbilledCons_UnbUnmetConsError_J11 = @UnbilledCons_UnbUnmetConsError_J11,

        UnauthCons_IllegalConnDomEstNo_D6  = @UnauthCons_IllegalConnDomEstNo_D6,
        UnauthCons_IllegalConnDomPersPerHouse_H6  = @UnauthCons_IllegalConnDomPersPerHouse_H6,
        UnauthCons_IllegalConnDomConsLitPerPersDay_J6  = @UnauthCons_IllegalConnDomConsLitPerPersDay_J6,
        UnauthCons_IllegalConnDomErrorMargin_F6  = @UnauthCons_IllegalConnDomErrorMargin_F6,
        UnauthCons_IllegalConnOthersErrorMargin_F10  = @UnauthCons_IllegalConnOthersErrorMargin_F10,

        IllegalConnectionsOthersEstimatedNumber_D10 = @IllegalConnectionsOthersEstimatedNumber_D10,
        IllegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10 = @IllegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10,

        UnauthCons_MeterTampBypEtcEstNo_D14  = @UnauthCons_MeterTampBypEtcEstNo_D14,
        UnauthCons_MeterTampBypEtcErrorMargin_F14  = @UnauthCons_MeterTampBypEtcErrorMargin_F14,
        UnauthCons_MeterTampBypEtcConsLitPerCustDay_J14  = @UnauthCons_MeterTampBypEtcConsLitPerCustDay_J14,

		UnauthCons_OthersErrorMargin_F18 = @UnauthCons_OthersErrorMargin_F18, 
		UnauthCons_OthersErrorMargin_F19 = @UnauthCons_OthersErrorMargin_F19, 
		UnauthCons_OthersErrorMargin_F20 = @UnauthCons_OthersErrorMargin_F20, 
		UnauthCons_OthersErrorMargin_F21 = @UnauthCons_OthersErrorMargin_F21, 
		UnauthCons_OthersM3PerDay_J18 = @UnauthCons_OthersM3PerDay_J18,
		UnauthCons_OthersM3PerDay_J19 = @UnauthCons_OthersM3PerDay_J19,
		UnauthCons_OthersM3PerDay_J20 = @UnauthCons_OthersM3PerDay_J20,
		UnauthCons_OthersM3PerDay_J21 = @UnauthCons_OthersM3PerDay_J21,

        MetErrors_DetailedManualSpec_J6  = @MetErrors_DetailedManualSpec_J6,
        MetErrors_BilledMetConsWoBulkSupMetUndrreg_H8  = @MetErrors_BilledMetConsWoBulkSupMetUndrreg_H8,
        MetErrors_BilledMetConsWoBulkSupErrorMargin_N8  = @MetErrors_BilledMetConsWoBulkSupErrorMargin_N8,

		MetErrors_Total_F12 = @MetErrors_Total_F12,
		MetErrors_Total_F13 = @MetErrors_Total_F13,
		MetErrors_Total_F14 = @MetErrors_Total_F14,
		MetErrors_Total_F15 = @MetErrors_Total_F15,
		MetErrors_Meter_H12 = @MetErrors_Meter_H12,
		MetErrors_Meter_H13 = @MetErrors_Meter_H13,
		MetErrors_Meter_H14 = @MetErrors_Meter_H14,
		MetErrors_Meter_H15 = @MetErrors_Meter_H15,
		MetErrors_Error_N12 = @MetErrors_Error_N12,
		MetErrors_Error_N13 = @MetErrors_Error_N13,
		MetErrors_Error_N14 = @MetErrors_Error_N14,
		MetErrors_Error_N15 = @MetErrors_Error_N15,

        MeteredBulkSupplyExportErrorMargin_N32 = @MeteredBulkSupplyExportErrorMargin_N32,
        UnbilledMeteredConsumptionWithoutBulkSupplyErrorMargin_N34 = @UnbilledMeteredConsumptionWithoutBulkSupplyErrorMargin_N34,
        CorruptMeterReadingPracticessErrorMargin_N38 = @CorruptMeterReadingPracticessErrorMargin_N38,
        DataHandlingErrorsOffice_L40 = @DataHandlingErrorsOffice_L40,
        DataHandlingErrorsOfficeErrorMargin_N40 = @DataHandlingErrorsOfficeErrorMargin_N40,

        MetErrors_MetBulkSupExpMetUnderreg_H32  = @MetErrors_MetBulkSupExpMetUnderreg_H32,
        MetErrors_UnbillMetConsWoBulkSupplMetUndrreg_H34  = @MetErrors_UnbillMetConsWoBulkSupplMetUndrreg_H34,
        MetErrors_CorruptMetReadPractMetUndrreg_H38  = @MetErrors_CorruptMetReadPractMetUndrreg_H38,
        Network_DistributionAndTransmissionMains_D7  = @Network_DistributionAndTransmissionMains_D7,
		Network_DistributionAndTransmissionMains_D8 = @Network_DistributionAndTransmissionMains_D8,
		Network_DistributionAndTransmissionMains_D9 = @Network_DistributionAndTransmissionMains_D9,
		Network_DistributionAndTransmissionMains_D10 = @Network_DistributionAndTransmissionMains_D10,
        Network_NoOfConnOfRegCustomers_H10  = @Network_NoOfConnOfRegCustomers_H10,
        Network_NoOfInactAccountsWSvcConns_H18  = @Network_NoOfInactAccountsWSvcConns_H18,
        Network_AvgLenOfSvcConnFromBoundaryToMeterM_H32  = @Network_AvgLenOfSvcConnFromBoundaryToMeterM_H32,

		Network_PossibleUnd_D30 = @Network_PossibleUnd_D30,
		Network_NoCustomers_H7 =  @Network_NoCustomers_H7,
		Network_ErrorMargin_J7 =  @Network_ErrorMargin_J7,
		Network_ErrorMargin_J10 = @Network_ErrorMargin_J10,
		Network_ErrorMargin_J18 = @Network_ErrorMargin_J18,
		Network_ErrorMargin_J32 = @Network_ErrorMargin_J32,
		Network_ErrorMargin_D35 = @Network_ErrorMargin_D35,

        Prs_Area_B7 = @Prs_Area_B7,
		Prs_Area_B8 = @Prs_Area_B8,
		Prs_Area_B9 = @Prs_Area_B9,
		Prs_Area_B10 = @Prs_Area_B10,

		Prs_ApproxNoOfConn_D7  = @Prs_ApproxNoOfConn_D7,
        Prs_DailyAvgPrsM_F7  = @Prs_DailyAvgPrsM_F7,
        Prs_ApproxNoOfConn_D8  = @Prs_ApproxNoOfConn_D8,
        Prs_DailyAvgPrsM_F8  = @Prs_DailyAvgPrsM_F8,
        Prs_ApproxNoOfConn_D9  = @Prs_ApproxNoOfConn_D9,
        Prs_DailyAvgPrsM_F9  = @Prs_DailyAvgPrsM_F9,
        Prs_ApproxNoOfConn_D10  = @Prs_ApproxNoOfConn_D10,
        Prs_DailyAvgPrsM_F10  = @Prs_DailyAvgPrsM_F10,

        Prs_ErrorMarg_F26  = @Prs_ErrorMarg_F26,

		Interm_Conn_D7 = @Interm_Conn_D7,
		Interm_Conn_D8 = @Interm_Conn_D8,
		Interm_Conn_D9 = @Interm_Conn_D9,
		Interm_Conn_D10 = @Interm_Conn_D10,
		Interm_Days_F7 = @Interm_Days_F7,
		Interm_Days_F8 = @Interm_Days_F8,
		Interm_Days_F9 = @Interm_Days_F9,
		Interm_Days_F10 = @Interm_Days_F10,
		Interm_Hour_H7 = @Interm_Hour_H7,
		Interm_Hour_H8 = @Interm_Hour_H8,
		Interm_Hour_H9 = @Interm_Hour_H9,
		Interm_Hour_H10 = @Interm_Hour_H10,
		Interm_ErrorMarg_H26 = @Interm_ErrorMarg_H26,

		FinancData_G6 = @FinancData_G6,
		FinancData_K6 = @FinancData_K6,
		FinancData_G8 = @FinancData_G8,
		FinancData_D26 = @FinancData_D26,
		FinancData_G35 = @FinancData_G35,

		MatrixOneIn_SelectedOption = @MatrixOneIn_SelectedOption,
		MatrixOneIn_C11 = @MatrixOneIn_C11,
        MatrixOneIn_C12 = @MatrixOneIn_C12,
        MatrixOneIn_C13 = @MatrixOneIn_C13,
        MatrixOneIn_C14 = @MatrixOneIn_C14,
        MatrixOneIn_C21 = @MatrixOneIn_C21,
        MatrixOneIn_C22 = @MatrixOneIn_C22,
        MatrixOneIn_C23 = @MatrixOneIn_C23,
        MatrixOneIn_C24 = @MatrixOneIn_C24,
        MatrixOneIn_D21 = @MatrixOneIn_D21,
        MatrixOneIn_D22 = @MatrixOneIn_D22,
        MatrixOneIn_D23 = @MatrixOneIn_D23,
        MatrixOneIn_D24 = @MatrixOneIn_D24,
        MatrixOneIn_E11 = @MatrixOneIn_E11,
        MatrixOneIn_E12 = @MatrixOneIn_E12,
        MatrixOneIn_E13 = @MatrixOneIn_E13,
        MatrixOneIn_E14 = @MatrixOneIn_E14,
        MatrixOneIn_E21 = @MatrixOneIn_E21,
        MatrixOneIn_E22 = @MatrixOneIn_E22,
        MatrixOneIn_E23 = @MatrixOneIn_E23,
        MatrixOneIn_E24 = @MatrixOneIn_E24,
        MatrixOneIn_F11 = @MatrixOneIn_F11,
        MatrixOneIn_F12 = @MatrixOneIn_F12,
        MatrixOneIn_F13 = @MatrixOneIn_F13,
        MatrixOneIn_F14 = @MatrixOneIn_F14,
        MatrixOneIn_F21 = @MatrixOneIn_F21,
        MatrixOneIn_F22 = @MatrixOneIn_F22,
        MatrixOneIn_F23 = @MatrixOneIn_F23,
        MatrixOneIn_F24 = @MatrixOneIn_F24,
        MatrixOneIn_G11 = @MatrixOneIn_G11,
        MatrixOneIn_G12 = @MatrixOneIn_G12,
        MatrixOneIn_G13 = @MatrixOneIn_G13,
        MatrixOneIn_G14 = @MatrixOneIn_G14,
        MatrixOneIn_G21 = @MatrixOneIn_G21,
        MatrixOneIn_G22 = @MatrixOneIn_G22,
        MatrixOneIn_G23 = @MatrixOneIn_G23,
        MatrixOneIn_G24 = @MatrixOneIn_G24,
        MatrixOneIn_H11 = @MatrixOneIn_H11,
        MatrixOneIn_H12 = @MatrixOneIn_H12,
        MatrixOneIn_H13 = @MatrixOneIn_H13,
        MatrixOneIn_H14 = @MatrixOneIn_H14,
        MatrixOneIn_H21 = @MatrixOneIn_H21,
        MatrixOneIn_H22 = @MatrixOneIn_H22,
        MatrixOneIn_H23 = @MatrixOneIn_H23,
        MatrixOneIn_H24 = @MatrixOneIn_H24,

   		MatrixTwoIn_SelectedOption = @MatrixTwoIn_SelectedOption,
        MatrixTwoIn_D21 = @MatrixTwoIn_D21,
        MatrixTwoIn_D22 = @MatrixTwoIn_D22,
        MatrixTwoIn_D23 = @MatrixTwoIn_D23,
        MatrixTwoIn_D24 = @MatrixTwoIn_D24,
        MatrixTwoIn_E11 = @MatrixTwoIn_E11,
        MatrixTwoIn_E12 = @MatrixTwoIn_E12,
        MatrixTwoIn_E13 = @MatrixTwoIn_E13,
        MatrixTwoIn_E14 = @MatrixTwoIn_E14,
        MatrixTwoIn_E21 = @MatrixTwoIn_E21,
        MatrixTwoIn_E22 = @MatrixTwoIn_E22,
        MatrixTwoIn_E23 = @MatrixTwoIn_E23,
        MatrixTwoIn_E24 = @MatrixTwoIn_E24,
        MatrixTwoIn_F11 = @MatrixTwoIn_F11,
        MatrixTwoIn_F12 = @MatrixTwoIn_F12,
        MatrixTwoIn_F13 = @MatrixTwoIn_F13,
        MatrixTwoIn_F14 = @MatrixTwoIn_F14,
        MatrixTwoIn_F21 = @MatrixTwoIn_F21,
        MatrixTwoIn_F22 = @MatrixTwoIn_F22,
        MatrixTwoIn_F23 = @MatrixTwoIn_F23,
        MatrixTwoIn_F24 = @MatrixTwoIn_F24,
        MatrixTwoIn_G11 = @MatrixTwoIn_G11,
        MatrixTwoIn_G12 = @MatrixTwoIn_G12,
        MatrixTwoIn_G13 = @MatrixTwoIn_G13,
        MatrixTwoIn_G14 = @MatrixTwoIn_G14,
        MatrixTwoIn_G21 = @MatrixTwoIn_G21,
        MatrixTwoIn_G22 = @MatrixTwoIn_G22,
        MatrixTwoIn_G23 = @MatrixTwoIn_G23,
        MatrixTwoIn_G24 = @MatrixTwoIn_G24,
        MatrixTwoIn_H11 = @MatrixTwoIn_H11,
        MatrixTwoIn_H12 = @MatrixTwoIn_H12,
        MatrixTwoIn_H13 = @MatrixTwoIn_H13,
        MatrixTwoIn_H14 = @MatrixTwoIn_H14,
        MatrixTwoIn_H21 = @MatrixTwoIn_H21,
        MatrixTwoIn_H22 = @MatrixTwoIn_H22,
        MatrixTwoIn_H23 = @MatrixTwoIn_H23,
        MatrixTwoIn_H24 = @MatrixTwoIn_H24,

     -- output
        SystemInputVolume_B19  = @SystemInputVolume_B19,
        SystemInputVolumeErrorMargin_B21  = @SystemInputVolumeErrorMargin_B21,
        AuthorizedConsumption_K12  = @AuthorizedConsumption_K12,
        AuthorizedConsumptionErrorMargin_K15  = @AuthorizedConsumptionErrorMargin_K15,
        WaterLosses_K29  = @WaterLosses_K29,
        WaterLossesErrorMargin_K31  = @WaterLossesErrorMargin_K31,
        BilledAuthorizedConsumption_T8  = @BilledAuthorizedConsumption_T8,
        UnbilledAuthorizedConsumption_T16  = @UnbilledAuthorizedConsumption_T16,
        UnbilledAuthorizedConsumptionErrorMargin_T20  = @UnbilledAuthorizedConsumptionErrorMargin_T20,
        CommercialLosses_T26  = @CommercialLosses_T26,
        CommercialLossesErrorMargin_T29  = @CommercialLossesErrorMargin_T29,
        PhysicalLossesM3_T34  = @PhysicalLossesM3_T34,
        PhyscialLossesErrorMargin_AH35  = @PhyscialLossesErrorMargin_AH35,
        BilledMeteredConsumption_AC4  = @BilledMeteredConsumption_AC4,
        BilledUnmeteredConsumption_AC9  = @BilledUnmeteredConsumption_AC9,
        UnbilledMeteredConsumption_AC14  = @UnbilledMeteredConsumption_AC14,
        UnbilledUnmeteredConsumption_AC19  = @UnbilledUnmeteredConsumption_AC19,
        UnbilledUnmeteredConsumptionErrorMargin_AO20  = @UnbilledUnmeteredConsumptionErrorMargin_AO20,
        UnauthorizedConsumption_AC24  = @UnauthorizedConsumption_AC24,
        UnauthorizedConsumptionErrorMargin_AO25  = @UnauthorizedConsumptionErrorMargin_AO25,
        CustomerMeterInaccuraciesAndErrorsM3_AC29  = @CustomerMeterInaccuraciesAndErrorsM3_AC29,
        CustomerMeterInaccuraciesAndErrorsErrorMargin_AO30  = @CustomerMeterInaccuraciesAndErrorsErrorMargin_AO30,
        RevenueWaterM3_AY8  = @RevenueWaterM3_AY8,
        NonRevenueWaterM3_AY24  = @NonRevenueWaterM3_AY24,
        NonRevenueWaterErrorMargin_AY26  = @NonRevenueWaterErrorMargin_AY26
	WHERE 
		WbEasyCalcDataId = @id;
END;

--EXEC dbo.spWbEasyCalcDataSaveArchive @id, @OldYearNo, @OldMonthNo, @OldZoneId, @OldIsArchive, @ChangedId;	

END;



GO
/****** Object:  StoredProcedure [dbo].[spWbEasyCalcDataSaveArchive]    Script Date: 26.05.2021 09:39:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spWbEasyCalcDataSaveArchive]
	@NewId [int],
	@OldYearNo [int] = NULL, 
	@OldMonthNo [int] = NULL,
	@OldZoneId [int] = NULL, 
	@OldIsArchive [bit] = NULL,
	@ChangedId [INT] = NULL
AS
BEGIN
SET NOCOUNT ON;

begin -- Logging 1 -----------------------------------------------------------------------------
DELETE FROM [dbo].[tbLog];
INSERT INTO [dbo].[tbLog](
     [ExecDate]
    ,[ExecTime]
    ,[CommandName]
    ,[TestDate]
) VALUES (
    GETDATE(),
    1,
    CONCAT(@NewId, '-', @OldYearNo, '-', @OldMonthNo, '-', @OldZoneId, '-', @OldIsArchive),
    GETDATE()
);
end; 

-- 7 local variables ------------------------
DECLARE	
	@OldId INT = NULL, 
	@OldArchiveDate DATETIME = NULL, 
	@NewYearNo [int],
	@NewMonthNo [int], 
	@NewArchiveDate DATETIME, 
	@NewZoneId [int], 
	@NewIsArchive [bit];

DECLARE
	@ZonePrefix NVARCHAR(50);

begin -- Fill 7 local variables ----------------------------------------------------------------

IF (@OldIsArchive IS NOT NULL) BEGIN
	SELECT
		@OldId = @NewId,
		@OldArchiveDate = dbo.fnGetArchivedDate(@OldYearNo, @OldMonthNo);	--CONCAT(@OldYearNo, '-', FORMAT(@OldMonthNo, '00'), '-01');
END;

SELECT 
	@NewYearNo = YearNo,
	@NewMonthNo = MonthNo, 
	@NewArchiveDate = dbo.fnGetArchivedDate(YearNo, MonthNo),	--CONCAT(YearNo, '-', FORMAT(MonthNo, '00'), '-01'), 
	@NewZoneId = ZoneId, 
	@NewIsArchive = IsArchive 
FROM 
	[dbo].[tbWbEasyCalcData] 
WHERE 
	WbEasyCalcDataId = @NewId;	

end;

begin -- Logging 2 -----------------------------------------------------------------------------
INSERT INTO [dbo].[tbLog](
     [ExecDate]
    ,[ExecTime]
    ,[CommandName]
    ,[TestDate]
) VALUES (
    GETDATE(),
    1,
    CONCAT(@OldId, '-', @NewYearNo, '-', @NewMonthNo, '-', @NewZoneId, '-', @NewIsArchive),
    GETDATE()
)
end;

-- Create @TabVar -------------------------
DECLARE @TabVar TABLE
(
	ZoneId INT, 
	VarName NVARCHAR(4000),
	ScadaId INT,
	ScadaTime DATETIME,
	VarValue FLOAT
);
/*
ZoneId	VarName					ScadaId	ScadaTime	VarValue
------------------------------------------------------------
1		PeriodDays_M21			225138	2019-01-15	1
1		SysInpVolumeM3_D6		225099	2019-01-15	1234567
1		SysInpVolumeError_F6	225120	2019-01-15	0,01
*/


begin -- Fill @TabVar -----------------------------------------------------------------------

DECLARE
	@IdValue INT = @NewId,
	@OrdinalPosition INT = 8,
	@TableName NVARCHAR(4000) = 'tbWbEasyCalcData',
	@IdColumnName NVARCHAR(4000) = 'WbEasyCalcDataId',
	@CastType NVARCHAR(4000) = 'FLOAT';

DECLARE @TempTable TABLE (
	[VarName] [nvarchar](4000) NOT NULL,
	[VarValue] [float] NOT NULL
);

INSERT INTO @TempTable 
EXEC [dbo].[spUnpivotTable]
	@IdValue,
	@OrdinalPosition,
	@TableName,
	@IdColumnName,
	@CastType;
/*
VarName								VarValue
--------------------------------------------
Start_PeriodDays_M21				1
SysInput_SystemInputVolumeM3_D6		1234567
SysInput_SystemInputVolumeError_F6	0,01
*/


SELECT
	@ZonePrefix = [DestinationName]
FROM
	[config].[WaterBalanceConfig]
WHERE
	[ZoneNo] = @NewZoneId;		-- 'SI'


INSERT INTO @TabVar (
	ZoneId,
	VarName,
	ScadaId,
	ScadaTime,
	VarValue
)
SELECT 
	@NewZoneId AS ZoneId, 
	tVar.VarScada,
	tTelMap.D_ID AS ScadaId,
	--CONCAT(@NewYearNo, '-', FORMAT(@NewMonthNo, '00'), '-01') ScadaTime,
	@NewArchiveDate ScadaTime,
	tTemp.VarValue
FROM 
	@TempTable tTemp
	INNER JOIN tbWbEasyCalcDataVar tVar ON tTemp.VarName = tVar.VarName
	INNER JOIN [TELWIN_MAP] tTelMap ON CONCAT(@ZonePrefix, '_', tVar.VarScada) = tTelMap.D_NAME;
/*
ZoneId	VarName					ScadaId	ScadaTime	VarValue
------------------------------------------------------------
1		PeriodDays_M21			225138	2019-01-15	1
1		SysInpVolumeM3_D6		225099	2019-01-15	1234567
1		SysInpVolumeError_F6	225120	2019-01-15	0,01
*/
end;


begin -- Delete in SCADA -----------------------------------------------------------------------
IF (
	@OldIsArchive=1 
	--AND (@OldYearNo<>@NewYearNo OR @OldMonthNo<>@NewMonthNo OR @OldZoneId<>@NewZoneId)
) 
BEGIN

	SELECT
		@ZonePrefix = [DestinationName]
	FROM
		[config].[WaterBalanceConfig]
	WHERE
		[ZoneNo] = @OldZoneId;		-- 'SI'

	DELETE 
		[dbo].[AR_0000_2020]
	FROM 
		tbWbEasyCalcDataVar tVar 
		INNER JOIN [TELWIN_MAP] tTelMap ON CONCAT(@ZonePrefix, '_', tVar.VarScada) = tTelMap.D_NAME
		INNER JOIN [dbo].[AR_0000_2020] tArch ON tTelMap.D_ID = tArch.D_VAR_ID
	WHERE
		tArch.D_TIME = @OldArchiveDate;

	begin -- Logging 3 -----------------------------------------------------------------------------
	INSERT INTO [dbo].[tbLog](		
		 [ExecDate]
		,[ExecTime]
		,[CommandName]
		,[TestDate]
	) VALUES (
		GETDATE(),
		3,
		'',
		GETDATE()
	);
	end;
END;
end;


begin -- Update and Insert in SCADA ------------------------------------------------------------

IF (@NewIsArchive=1)  
BEGIN
	
	-- UPDATE -------------------------------
	--SELECT 
	--	tWb.* 
	--FROM 
	--	@TabVar tWb
	--	INNER JOIN AR_0000_2020 tScada ON tWb.ScadaId = tScada.D_VAR_ID AND tWb.ScadaTime = tScada.D_TIME;  

	UPDATE [dbo].[AR_0000_2020] SET
		[D_VALUE_FLO] = tWb.VarValue
	FROM 
		@TabVar tWb
		INNER JOIN AR_0000_2020 tScada ON tWb.ScadaId = tScada.D_VAR_ID AND tWb.ScadaTime = tScada.D_TIME;

	-- INSERT ------------------------------
	--SELECT 
	--	tWb.* 
	--FROM 
	--	@TabVar tWb
	--	LEFT OUTER JOIN AR_0000_2020 tScada ON tWb.ScadaId = tScada.D_VAR_ID AND tWb.ScadaTime = tScada.D_TIME
	--WHERE
	--	tScada.D_VAR_ID IS NULL
	--	; 

	DECLARE @dateStart DATETIME = N'1970-01-01 00:00:00';
	INSERT INTO [dbo].[AR_0000_2020]
	(
		 [D_IDENT]
		,[D_VAR_ID]
		,[T_TYPE]
		,[D_TIME]
		,[D_STATUS]
		,[D_TYPE]
		,[D_VALUE_FLO]
		,[D_VALUE_INT]
	)
	SELECT
		 CONVERT(BIGINT, CONVERT(VARBINARY, (master.dbo.fn_varbintohexstr(DATEDIFF(SECOND, @dateStart, tWb.ScadaTime)) + '00000000'), 1))
		,tWb.ScadaId
		,1
		,tWb.ScadaTime
		,16
		,171
		,tWb.VarValue
		,NULL
	FROM 
		@TabVar tWb
		LEFT OUTER JOIN AR_0000_2020 tArch ON tWb.ScadaId = tArch.D_VAR_ID AND tWb.ScadaTime = tArch.D_TIME
	WHERE
		tArch.D_VAR_ID IS NULL;


	begin -- Logging 4 -----------------------------------------------------------------------------

	INSERT INTO [dbo].[tbLog](
		 [ExecDate]
		,[ExecTime]
		,[CommandName]
		,[TestDate]
	) VALUES (
		GETDATE(),
		4,
		'',
		GETDATE()
	);
	end;
		
END;
	
end;


END;

GO
/****** Object:  StoredProcedure [dbo].[spYearList]    Script Date: 26.05.2021 09:39:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spYearList] 
AS
BEGIN
	SET NOCOUNT ON;

SELECT 
    YearId AS Id,
	YearName AS [Name]
FROM
	tbYear;


END
GO
/****** Object:  StoredProcedure [dbo].[spZoneList]    Script Date: 26.05.2021 09:39:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spZoneList] 
AS
BEGIN
	SET NOCOUNT ON;

SELECT 
    --[ZoneNo] AS ZoneId,
    [ModelZoneId] AS ZoneId,
	LTRIM(STR([ZoneNo])) + ' - ' + [ZoneName] AS ZoneName,
    REPLACE([DestinationName], 'S', '') AS ZoneRomanNo
FROM 
	[config].[WaterBalanceConfig]
WHERE
	[ZoneNo] <= 7
	--[GisZoneId] IS NOT NULL
ORDER BY
	[ZoneNo];


END
GO
