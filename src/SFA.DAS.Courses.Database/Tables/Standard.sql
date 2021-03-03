CREATE TABLE [dbo].[Standard]
(
	[StandardUId] VARCHAR(20) PRIMARY KEY,
    [IfateReferenceNumber] VARCHAR(10) NOT NULL,
    [LarsCode] INT NULL,
    [Status] VARCHAR(100) NOT NULL,
    [EarliestStartDate] DATETIME NULL,
    [LatestStartDate] DATETIME NULL,
    [LatestEndDate] DATETIME NULL,
	[Title] VARCHAR(1000) NOT NULL,
	[Level] INT NOT NULL,
    [TypicalDuration] INT NOT NULL,
    [MaxFunding] INT NOT NULL,
	[IntegratedDegree] VARCHAR(100) NULL,
	[OverviewOfRole] VARCHAR(MAX) NOT NULL,
	[RouteCode] INT NOT NULL DEFAULT 0,
	[RouteId] UNIQUEIDENTIFIER NOT NULL,
    [AssessmentPlanUrl] VARCHAR(500) NULL,
    [ApprovedForDelivery] DATETIME NULL,
    [TrailBlazerContact] VARCHAR(200) NULL,
    [EqaProviderName] VARCHAR(200) NULL,
    [EqaProviderContactName] VARCHAR(200) NULL,
    [EqaProviderContactEmail] VARCHAR(200) NULL,
    [EqaProviderWebLink] VARCHAR(500) NULL,
	[Keywords] VARCHAR(MAX) NULL,
	[TypicalJobTitles] VARCHAR(MAX) NULL,
	[StandardPageUrl] VARCHAR(500) NOT NULL,
	[Version] DECIMAL(18, 1) NULL,
    [RegulatedBody] VARCHAR(1000) NULL,
    [Skills] NVARCHAR(MAX) NULL, 
    [Knowledge] NVARCHAR(MAX) NULL, 
	[Behaviours] NVARCHAR(MAX) NULL, 
    [Duties] NVARCHAR(MAX) NULL, 
    [CoreAndOptions] BIT NOT NULL DEFAULT 0, 
    [IntegratedApprenticeship] BIT NOT NULL DEFAULT 0,
    [Options] NVARCHAR(MAX) NULL, 
    CONSTRAINT [AK_Standard_Column] UNIQUE ([StandardUId])
)
GO

CREATE NONCLUSTERED INDEX [IDX_Standard_RouteId] ON [dbo].[Standard] (RouteId) 
GO 

CREATE NONCLUSTERED INDEX [IDX_Standard_LarsCode] ON [dbo].[Standard] (LarsCode) 
GO

CREATE NONCLUSTERED INDEX [IDX_Standard_Status] ON [dbo].[Standard] (Status) 
GO

CREATE NONCLUSTERED INDEX [IDX_Standard_IfateReferenceNumber] ON [dbo].[Standard] (IfateReferenceNumber) 
GO