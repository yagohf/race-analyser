USE [RaceAnalyser];
GO

IF NOT EXISTS (
		SELECT 1
		FROM sys.tables t
		INNER JOIN sys.schemas s ON t.schema_id = s.schema_id
		WHERE t.name = 'Log'
			AND s.name = 'dbo'
		)
BEGIN
	PRINT 'CREATE TABLE [dbo].[Log]';

	CREATE TABLE [Log] (
		[Id] INT IDENTITY(1, 1) NOT NULL
		,[Message] NVARCHAR(MAX) NULL
		,[MessageTemplate] NVARCHAR(MAX) NULL
		,[Level] NVARCHAR(128) NULL
		,[TimeStamp] DATETIMEOFFSET(7) NOT NULL
		,[Exception] NVARCHAR(MAX) NULL
		,[LogEvent] NVARCHAR(MAX) NULL
		,CONSTRAINT [PK_Log] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (
			PAD_INDEX = OFF
			,STATISTICS_NORECOMPUTE = OFF
			,IGNORE_DUP_KEY = OFF
			,ALLOW_ROW_LOCKS = ON
			,ALLOW_PAGE_LOCKS = ON
			) ON [PRIMARY]
		);
END
GO