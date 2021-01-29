CREATE TABLE [dbo].[Standard_Import]
(
	[Id] INT PRIMARY KEY,
    [IfateReferenceNumber] VARCHAR(10) NOT NULL,
    [LarsCode] INT NULL,
    [Status] VARCHAR(100) NOT NULL,
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
    [IntegratedApprenticeship] BIT NOT NULL DEFAULT 0,
    CONSTRAINT [AK_StandardImport_Column] UNIQUE ([Id])
)
GO
