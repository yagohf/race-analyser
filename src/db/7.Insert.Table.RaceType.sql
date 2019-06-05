USE [RaceAnalyser];
GO

IF 
	NOT EXISTS (SELECT 1 FROM [dbo].[RaceType] WHERE [Id] = 1)
AND 
	NOT EXISTS (SELECT 1 FROM [dbo].[RaceType] WHERE [Name] = 'Kart')
BEGIN
	INSERT INTO [dbo].[RaceType]([Id], [Name])
	VALUES(1, 'Kart');
END
GO

IF 
	NOT EXISTS (SELECT 1 FROM [dbo].[RaceType] WHERE [Id] = 2)
AND 
	NOT EXISTS (SELECT 1 FROM [dbo].[RaceType] WHERE [Name] = 'F-One')
BEGIN
	INSERT INTO [dbo].[RaceType]([Id], [Name])
	VALUES(2, 'F-One');
END
GO

IF 
	NOT EXISTS (SELECT 1 FROM [dbo].[RaceType] WHERE [Id] = 3)
AND 
	NOT EXISTS (SELECT 1 FROM [dbo].[RaceType] WHERE [Name] = 'Moto GP')
BEGIN
	INSERT INTO [dbo].[RaceType]([Id], [Name])
	VALUES(3, 'Moto GP');
END
GO