USE [RaceAnalyser];
GO

IF NOT EXISTS (
		SELECT 1
		FROM sys.tables t inner join sys.schemas s on t.schema_id = s.schema_id
		WHERE t.name = 'DriverResult' AND s.name = 'dbo'
		)
BEGIN
	PRINT 'CREATE TABLE [dbo].[DriverResult]';

	CREATE TABLE [dbo].[DriverResult]
	(
		[Id] INT NOT NULL IDENTITY(1,1),
		[RaceId] INT NOT NULL,
		[Position] INT NOT NULL,
		[DriverNumber] INT NOT NULL,
		[DriverName] VARCHAR(100) NOT NULL,
		[Laps] INT NOT NULL,
		[TotalRaceTimeTicks] BIGINT NOT NULL,
		[BestLapTicks] BIGINT NOT NULL,
		[AverageSpeed] DECIMAL(5, 2) NOT NULL,
		[GapTicks] BIGINT NULL,
		CONSTRAINT PK_DriverResult PRIMARY KEY CLUSTERED ([Id]),
		CONSTRAINT UQ_DriverResult UNIQUE ([RaceId], [Position]),
		CONSTRAINT FK_DriverResult_Race FOREIGN KEY ([RaceId]) REFERENCES [dbo].[Race]([Id])
	);
END
GO