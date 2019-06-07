USE [RaceAnalyser];
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[User] WHERE [Login] = 'gympass')
BEGIN
	INSERT INTO [dbo].[User]([Login], Name, Password)
	VALUES ('gympass', 'Usuário de testes', '89794b621a313bb59eed0d9f0f4e8205'); --SENHA: 123mudar
END
GO