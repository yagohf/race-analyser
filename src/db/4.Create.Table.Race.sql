USE [RaceAnalyser];
GO

IF NOT EXISTS (
		SELECT 1
		FROM sys.tables t inner join sys.schemas s on t.schema_id = s.schema_id
		WHERE t.name = 'Race' AND s.name = 'dbo'
		)
BEGIN
	PRINT 'CREATE TABLE [dbo].[Race]';

	CREATE TABLE [dbo].[Race]
	(
		[Id] INT NOT NULL IDENTITY(1,1),
		[Description] VARCHAR(100) NOT NULL,
		[Date] DATE NOT NULL,
		[RaceTypeId] INT NOT NULL,
		[TotalLaps] INT NOT NULL,
		[UploaderId] INT NOT NULL,
		[UploadDate] DATETIME NOT NULL,
		CONSTRAINT PK_Race PRIMARY KEY CLUSTERED ([Id]),
		CONSTRAINT FK_Race_Uploader FOREIGN KEY ([UploaderId]) REFERENCES [dbo].[User]([Id]),
		CONSTRAINT FK_Race_RaceType FOREIGN KEY ([RaceTypeId]) REFERENCES [dbo].[RaceType]([Id])
	);
END
GO