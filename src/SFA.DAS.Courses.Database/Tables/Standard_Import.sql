CREATE TABLE [dbo].[Standard_Import]
(
	[Id] INT PRIMARY KEY,
	[Title] VARCHAR(1000) NOT NULL,
	[Level] INT NOT NULL,
	[IntegratedDegree] VARCHAR(100) NULL,
	[OverviewOfRole] VARCHAR(MAX) NOT NULL,
    [RouteId] UNIQUEIDENTIFIER NOT NULL,
	[Keywords] VARCHAR(MAX) NULL,
	[TypicalJobTitles] VARCHAR(MAX) NULL,
	[CoreSkillsCount] VARCHAR(MAX) NULL,
	[StandardPageUrl] VARCHAR(500) NOT NULL,
	[Version] DECIMAL NULL,
    [Behaviours] NVARCHAR(MAX) NULL, 
    CONSTRAINT [AK_StandardImport_Column] UNIQUE ([Id])
)
GO
