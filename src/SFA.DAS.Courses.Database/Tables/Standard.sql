CREATE TABLE [dbo].[Standard]
(
	[Id] INT PRIMARY KEY,
	[TypicalDuration] INT NOT NULL,
	[Title] VARCHAR(1000) NOT NULL,
	[Level] INT NOT NULL,
	[IntegratedDegree] VARCHAR(100) NULL,
	[MaxFunding] BIGINT NOT NULL,
	[OverviewOfRole] VARCHAR(MAX) NOT NULL,
	[RouteId] UNIQUEIDENTIFIER NOT NULL,
	[Keywords] VARCHAR(MAX) NULL,
	[TypicalJobTitles] VARCHAR(MAX) NULL,
	[CoreSkillsCount] VARCHAR(MAX) NULL,
	[StandardPageUrl] VARCHAR(500) NOT NULL,
	[Version] DECIMAL NULL,
	CONSTRAINT [AK_Standard_Column] UNIQUE ([Id])
)
GO

CREATE NONCLUSTERED INDEX [IDX_Standard_RouteId] ON [dbo].[Standard] (RouteId) 
INCLUDE (Id,TypicalDuration,Title,[Level],  IntegratedDegree, [MaxFunding], OverviewOfRole,[Keywords],[TypicalJobTitles], CoreSkillsCount,[StandardPageUrl], [Version]) WITH (ONLINE = ON) 
GO 