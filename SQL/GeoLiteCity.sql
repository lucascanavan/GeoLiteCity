CREATE TABLE [dbo].[GeoLiteCity-Blocks](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[StartIpNum] [bigint] NULL,
	[EndIpNum] [bigint] NULL,
	[LocId] [bigint] NULL
 CONSTRAINT [PK_GeoLiteCity-Blocks] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)
)
GO

CREATE CLUSTERED INDEX [IX_GeoLiteCity-Blocks_StartIpNum_EndIpNum] ON [dbo].[GeoLiteCity-Blocks]
(
	[StartIpNum] ASC,
	[EndIpNum] ASC
)
GO

CREATE TABLE [dbo].[GeoLiteCity-Location](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[LocId] [bigint] NULL,
	[Country] [nchar](2) NULL,
	[Region] [nchar](2) NULL,
	[City] [nvarchar](255) NULL,
	[PostalCode] [nvarchar](8) NULL,
	[Latitude] decimal NULL,
	[Longitude] decimal NULL,
	[MetroCode] [bigint] NULL,
	[AreaCode] [nchar](3) NULL
 CONSTRAINT [PK_GeoLiteCity-Location] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)
)
GO

CREATE CLUSTERED INDEX [IX_GeoLiteCity-Location_LocId] ON [dbo].[GeoLiteCity-Location]
(
	[LocId] ASC
)
GO