USE [RaceAnalyser];
GO

IF NOT EXISTS (
		SELECT 1
		FROM sys.tables t inner join sys.schemas s on t.schema_id = s.schema_id
		WHERE t.name = 'RaceType' AND s.name = 'dbo'
		)
BEGIN
	PRINT 'CREATE TABLE [dbo].[RaceType]';

	CREATE TABLE [dbo].[RaceType]
	(
		[Id] INT NOT NULL,
		[Name] VARCHAR(100) NOT NULL,
		CONSTRAINT PK_RaceType PRIMARY KEY CLUSTERED ([Id]),
		CONSTRAINT UQ_RaceType UNIQUE ([Name])
	);
END
GO