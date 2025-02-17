CREATE TABLE [dbo].[Standard]
(
	[StandardUId] VARCHAR(20) PRIMARY KEY,
    [IfateReferenceNumber] VARCHAR(10) NOT NULL,
    [LarsCode] INT NULL,
    [Status] VARCHAR(100) NOT NULL,
    [VersionEarliestStartDate] DATETIME NULL,
    [VersionLatestStartDate] DATETIME NULL,
    [VersionLatestEndDate] DATETIME NULL,
	[Title] VARCHAR(1000) NOT NULL,
	[Level] INT NOT NULL,
    [ProposedTypicalDuration] INT NOT NULL,
    [ProposedMaxFunding] INT NOT NULL,
	[IntegratedDegree] VARCHAR(100) NULL,
	[OverviewOfRole] VARCHAR(MAX) NOT NULL,
	[RouteCode] INT NOT NULL DEFAULT 0,
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
	[Version] VARCHAR(20) NULL,
    [RegulatedBody] VARCHAR(1000) NULL,
    [Skills] NVARCHAR(MAX) NULL, -- TODO: Remove
    [Knowledge] NVARCHAR(MAX) NULL, -- TODO: Remove
	[Behaviours] NVARCHAR(MAX) NULL, -- TODO: Remove
    [Duties] NVARCHAR(MAX) NULL,
    [CoreDuties] NVARCHAR(MAX) NULL,
    [CoreAndOptions] BIT NOT NULL DEFAULT 0, 
    [IntegratedApprenticeship] BIT NOT NULL DEFAULT 0,
    [Old_Options] NVARCHAR(MAX) NULL, 
    [EPAChanged] BIT NOT NULL DEFAULT 0,
    [VersionMajor] INT NOT NULL DEFAULT 0,
    [VersionMinor] INT NOT NULL DEFAULT 0,
    [Options] NVARCHAR(MAX) NULL, 
    [CoronationEmblem] BIT NOT NULL DEFAULT 0,
    [EpaoMustBeApprovedByRegulatorBody] BIT NOT NULL Default 0,
    [IsRegulatedForProvider] BIT NOT NULL DEFAULT 0, 
    [IsRegulatedForEPAO] BIT NOT NULL DEFAULT 0, 
    CONSTRAINT [AK_Standard_Column] UNIQUE ([StandardUId])
)
GO

CREATE NONCLUSTERED INDEX [IDX_Standard_RouteCode] ON [dbo].[Standard] (RouteCode) 
GO 

CREATE NONCLUSTERED INDEX [IDX_Standard_LarsCode] ON [dbo].[Standard] (LarsCode) WHERE LarsCode IS NOT NULL
GO

CREATE NONCLUSTERED INDEX [IDX_Standard_Status] ON [dbo].[Standard] (Status)
INCLUDE([StandardUId],[IFateReferenceNumber],[LarsCode],[VersionEarliestStartDate],[VersionLatestStartDate],[VersionLatestEndDate],[Title],[Level],[ProposedTypicalDuration],[ProposedMaxFunding],[IntegratedDegree],[OverviewOfRole],[RouteCode],[AssessmentPlanUrl],[ApprovedForDelivery],[TrailBlazerContact],[EqaProviderName],[EqaProviderContactName],[EqaProviderContactEmail],[EqaProviderWebLink],[Keywords],[TypicalJobTitles],[StandardPageUrl],[Version],[RegulatedBody],[Duties],[CoreAndOptions],[IntegratedApprenticeship],[Old_Options],[CoreDuties],[CoronationEmblem])
GO

CREATE NONCLUSTERED INDEX [IDX_Standard_IfateReferenceNumber] ON [dbo].[Standard] (IfateReferenceNumber) 
GO