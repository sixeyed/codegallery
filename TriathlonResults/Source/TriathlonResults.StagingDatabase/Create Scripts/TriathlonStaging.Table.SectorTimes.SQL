/****** Object:  Table [dbo].[SectorTimes]    Script Date: 10/22/2008 07:43:09 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SectorTimes]') AND type in (N'U'))
DROP TABLE [dbo].[SectorTimes]
GO
/****** Object:  Table [dbo].[SectorTimes]    Script Date: 10/22/2008 07:43:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SectorTimes]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SectorTimes](
	[RaceId] [int] NOT NULL,
	[AthleteId] [int] NOT NULL,
	[SectorId] [int] NOT NULL,
	[StartTime] [datetime] NULL,
	[EndTime] [datetime] NULL,
	[Duration] [int] NOT NULL,
	[Processed] [bit] NOT NULL CONSTRAINT [DF_SectorTimes_Processed]  DEFAULT ((0))
) ON [PRIMARY]
END
GO

/****** Object:  Trigger [ProcessSectorTime]    Script Date: 10/22/2008 07:43:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE TRIGGER dbo.ProcessSectorTime
   ON  dbo.SectorTimes
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
