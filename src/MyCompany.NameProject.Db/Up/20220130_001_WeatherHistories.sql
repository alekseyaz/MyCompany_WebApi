SET XACT_ABORT ON

BEGIN TRAN

GO
PRINT N'Creating [dbo].[WeatherHistories]...';

IF NOT EXISTS (SELECT * FROM dbo.sysobjects where id = object_id(N'dbo.[WeatherHistories]') and OBJECTPROPERTY(id, N'IsTable') = 1)
	BEGIN

		CREATE TABLE [dbo].WeatherHistories(
			[Id] [uniqueidentifier] NOT NULL,
			[Request] [nvarchar](MAX) NULL,
			[Data] [nvarchar](MAX) NULL,
			[ErrorDescription] [nvarchar](MAX) NULL,
			[LastUpdateDate] [datetime2](7) NOT NULL,
		 CONSTRAINT [PK_WeatherHistories] PRIMARY KEY CLUSTERED 
		(
			[Id] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
		) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

		ALTER TABLE [dbo].[WeatherHistories] ADD  DEFAULT (getdate()) FOR [LastUpdateDate]

	END
GO

COMMIT TRAN