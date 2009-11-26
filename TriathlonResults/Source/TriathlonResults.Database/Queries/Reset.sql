
USE TriathlonResults
GO

DELETE SectorTimes;

DELETE Races;

DELETE Sectors;

DELETE Athletes; 

INSERT INTO Races(RaceId, RaceName) VALUES(1, 'Eton SuperSprint');
INSERT INTO Races(RaceId, RaceName) VALUES(2, 'Nokia Royal Windsor');
INSERT INTO Races(RaceId, RaceName) VALUES(3, 'Men''s Fitness Rough Track');

INSERT INTO Sectors(SectorId, SectorName) VALUES(1, 'Swim');
INSERT INTO Sectors(SectorId, SectorName) VALUES(2, 'Bike');
INSERT INTO Sectors(SectorId, SectorName) VALUES(3, 'Run');

INSERT INTO Athletes(AthleteId, AthleteName) VALUES(1, 'Tim Don');