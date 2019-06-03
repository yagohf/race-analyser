USE [RaceAnalyser];
GO

IF NOT EXISTS (
		SELECT 1
		FROM sys.tables t inner join sys.schemas s on t.schema_id = s.schema_id
		WHERE t.name = 'User' AND s.name = 'dbo'
		)
BEGIN
	PRINT 'CREATE TABLE [dbo].[User]';

	CREATE TABLE [dbo].[User]
	(
		[Id] INT NOT NULL IDENTITY(1,1),
		[Login] VARCHAR(20) NOT NULL,
		[Password] VARCHAR(32) NOT NULL,
		[Name] VARCHAR(100) NOT NULL,
		CONSTRAINT PK_User PRIMARY KEY CLUSTERED ([Id]),
		CONSTRAINT UQ_User UNIQUE ([Login])
	);
END
GO