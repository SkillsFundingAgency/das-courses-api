CREATE TABLE [dbo].[Standard]
(
	[Id] INT PRIMARY KEY,
	[Title] VARCHAR(1000) NOT NULL,
	[Level] INT NOT NULL,
	[IntegratedDegree] VARCHAR(100) NULL,
	[OverviewOfRole] VARCHAR(MAX) NOT NULL,
	[RouteId] UNIQUEIDENTIFIER NOT NULL,
	[Keywords] VARCHAR(MAX) NULL,
	[TypicalJobTitles] VARCHAR(MAX) NULL,
	[StandardPageUrl] VARCHAR(500) NOT NULL,
	[Version] DECIMAL NULL,
  [RegulatedBody] VARCHAR(1000) NULL,
  [Skills] NVARCHAR(MAX) NULL, 
  [Knowledge] NVARCHAR(MAX) NULL, 
	[Behaviours] NVARCHAR(MAX) NULL, 
  [Duties] NVARCHAR(MAX) NULL, 
    [CoreAndOptions] BIT NOT NULL DEFAULT 0, 
    CONSTRAINT [AK_Standard_Column] UNIQUE ([Id])
)
GO

CREATE NONCLUSTERED INDEX [IDX_Standard_RouteId] ON [dbo].[Standard] (RouteId) 
INCLUDE (Id,Title,[Level],  IntegratedDegree, OverviewOfRole,[Keywords],[TypicalJobTitles], [StandardPageUrl], [Version]) WITH (ONLINE = ON) 
GO 