﻿CREATE TABLE [dbo].[Standard]
(
	[Id] INT CONSTRAINT [PK_Id] PRIMARY KEY,
	[TypicalDuration] INT NULL,
	[Title] VARCHAR(1000) NOT NULL,
	[Level] INT NOT NULL,
	[IntegratedDegree] VARCHAR(100) NULL,
	[MaxFunding] BIGINT NOT NULL,
	[OverviewOfRole] VARCHAR(MAX) NOT NULL,
	[Route] VARCHAR(500) NULL,
	[Keywords] VARCHAR(MAX) NULL,
	[TypicalJobTitles] VARCHAR(MAX) NULL,
	[CoreSkillsCount] VARCHAR(MAX) NULL,
	[StandardPageUrl] VARCHAR(500) NOT NULL,
	[Version] DECIMAL NULL
)
GO
