USE [RaceAnalyser];
GO

IF NOT EXISTS (
		SELECT 1
		FROM sys.tables t inner join sys.schemas s on t.schema_id = s.schema_id
		WHERE t.name = 'Lap' AND s.name = 'dbo'
		)
BEGIN
	PRINT 'CREATE TABLE [dbo].[Lap]';

	CREATE TABLE [dbo].[Lap]
	(
		[Id] INT NOT NULL IDENTITY(1,1),
		[RaceId] INT NOT NULL,
		[Date] DATE NOT NULL,
		[DriverNumber] INT NOT NULL,
		[DriverName] VARCHAR(100) NOT NULL,
		[Number] INT NOT NULL,
		[TimeTicks] BIGINT NOT NULL,
		[AverageSpeed] DECIMAL(5, 2) NOT NULL,
		CONSTRAINT PK_Lap PRIMARY KEY CLUSTERED ([Id]),
		CONSTRAINT UQ_Lap UNIQUE ([RaceId], [DriverNumber], [Number]),
		CONSTRAINT FK_Lap_Race FOREIGN KEY ([RaceId]) REFERENCES [dbo].[Race]([Id])
	);
END
GO