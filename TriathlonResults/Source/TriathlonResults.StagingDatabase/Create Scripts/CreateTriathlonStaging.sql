USE [master]
GO
/****** Object:  Database [TriathlonStaging]    Script Date: 10/24/2008 07:33:57 ******/
CREATE DATABASE [TriathlonStaging] ON  PRIMARY 
( NAME = N'TriathlonStaging', FILENAME = N'C:\TriathlonStaging.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'TriathlonStaging_log', FILENAME = N'C:\TriathlonStaging_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
EXEC dbo.sp_dbcmptlevel @dbname=N'TriathlonStaging', @new_cmptlevel=90
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [TriathlonStaging].[dbo].[sp_fulltext_database] @action = 'disable'
end
GO
ALTER DATABASE [TriathlonStaging] SET ANSI_NULL_DEFAULT OFF
GO
ALTER DATABASE [TriathlonStaging] SET ANSI_NULLS OFF
GO
ALTER DATABASE [TriathlonStaging] SET ANSI_PADDING OFF
GO
ALTER DATABASE [TriathlonStaging] SET ANSI_WARNINGS OFF
GO
ALTER DATABASE [TriathlonStaging] SET ARITHABORT OFF
GO
ALTER DATABASE [TriathlonStaging] SET AUTO_CLOSE OFF
GO
ALTER DATABASE [TriathlonStaging] SET AUTO_CREATE_STATISTICS ON
GO
ALTER DATABASE [TriathlonStaging] SET AUTO_SHRINK OFF
GO
ALTER DATABASE [TriathlonStaging] SET AUTO_UPDATE_STATISTICS ON
GO
ALTER DATABASE [TriathlonStaging] SET CURSOR_CLOSE_ON_COMMIT OFF
GO
ALTER DATABASE [TriathlonStaging] SET CURSOR_DEFAULT  GLOBAL
GO
ALTER DATABASE [TriathlonStaging] SET CONCAT_NULL_YIELDS_NULL OFF
GO
ALTER DATABASE [TriathlonStaging] SET NUMERIC_ROUNDABORT OFF
GO
ALTER DATABASE [TriathlonStaging] SET QUOTED_IDENTIFIER OFF
GO
ALTER DATABASE [TriathlonStaging] SET RECURSIVE_TRIGGERS OFF
GO
ALTER DATABASE [TriathlonStaging] SET  ENABLE_BROKER
GO
ALTER DATABASE [TriathlonStaging] SET AUTO_UPDATE_STATISTICS_ASYNC OFF
GO
ALTER DATABASE [TriathlonStaging] SET DATE_CORRELATION_OPTIMIZATION OFF
GO
ALTER DATABASE [TriathlonStaging] SET TRUSTWORTHY OFF
GO
ALTER DATABASE [TriathlonStaging] SET ALLOW_SNAPSHOT_ISOLATION OFF
GO
ALTER DATABASE [TriathlonStaging] SET PARAMETERIZATION SIMPLE
GO
ALTER DATABASE [TriathlonStaging] SET  READ_WRITE
GO
ALTER DATABASE [TriathlonStaging] SET RECOVERY FULL
GO
ALTER DATABASE [TriathlonStaging] SET  MULTI_USER
GO
ALTER DATABASE [TriathlonStaging] SET PAGE_VERIFY CHECKSUM
GO
ALTER DATABASE [TriathlonStaging] SET DB_CHAINING OFF
GO
USE [TriathlonStaging]
GO
/****** Object:  User [TriathlonResults]    Script Date: 10/24/2008 07:33:59 ******/
CREATE USER [TriathlonResults] FOR LOGIN [TriathlonResults] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  Table [dbo].[SectorTimes]    Script Date: 10/24/2008 07:34:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SectorTimes](
	[RaceId] [int] NOT NULL,
	[AthleteId] [int] NOT NULL,
	[SectorId] [int] NOT NULL,
	[StartTime] [datetime] NULL,
	[EndTime] [datetime] NULL,
	[Duration] [int] NOT NULL,
	[Processed] [bit] NOT NULL CONSTRAINT [DF_SectorTimes_Processed]  DEFAULT ((0))
) ON [PRIMARY]
GO
/****** Object:  Trigger [ProcessSectorTime]    Script Date: 10/24/2008 07:34:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE TRIGGER [dbo].[ProcessSectorTime]
   ON  [dbo].[SectorTimes]
   AFTER INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

DECLARE @raceId int
DECLARE @sectorId int
DECLARE @athleteId int
declare @startTime datetime
declare @endTime datetime
declare @duration int
 
SET @raceId  = (SELECT RaceId FROM inserted)
SET @sectorId  = (SELECT sectorId FROM inserted)
SET @athleteId  = (SELECT athleteId FROM inserted)
SET @startTime  = (SELECT startTime FROM inserted)
SET @endTime  = (SELECT endTime FROM inserted)
SET @duration  = (SELECT duration FROM inserted)

    EXEC TriathlonResults.dbo.SetSectorTime 
		@RaceId=@raceId, @SectorId=@sectorId, @AthleteId=@athleteId, 
		@StartTime=@startTime, @EndTime=@endTime, @Duration=@duration;

update SectorTimes set Processed = 1 where RaceId=@raceId and SectorId=@sectorId and AthleteId=@athleteId;



END
GO
/****** Object:  StoredProcedure [dbo].[GetUnprocessedSectorTimes]    Script Date: 10/24/2008 07:34:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetUnprocessedSectorTimes]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT 
		RaceId, 
		AthleteId, 
		SectorId , 
		Duration , 
		Processed 
	FROM SectorTimes
	WHERE processed = 0
	FOR XML PATH (''), ELEMENTS;

END
GO
/****** Object:  StoredProcedure [dbo].[UpdateProcessedSectorTime]    Script Date: 10/24/2008 07:34:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[UpdateProcessedSectorTime]
	@RaceId int,
	@AthleteId int,
	@SectorId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE SectorTimes
	SET Processed = 1
	WHERE RaceId = @RaceId
		AND AthleteId = @AthleteId
		AND SectorId = @SectorId
		AND Processed = 0;

END
GO
GRANT DELETE ON [dbo].[SectorTimes] TO [TriathlonResults]
GO
GRANT INSERT ON [dbo].[SectorTimes] TO [TriathlonResults]
GO
GRANT SELECT ON [dbo].[SectorTimes] TO [TriathlonResults]
GO
GRANT UPDATE ON [dbo].[SectorTimes] TO [TriathlonResults]
GO
GRANT ALTER ON [dbo].[SectorTimes] TO [TriathlonResults]
GO
GRANT EXECUTE ON [dbo].[GetUnprocessedSectorTimes] TO [TriathlonResults]
GO
GRANT EXECUTE ON [dbo].[UpdateProcessedSectorTime] TO [TriathlonResults]
GO