﻿CREATE TABLE [dbo].[Standard_Import]
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
    [CreatedDate] DATETIME NULL,
    [EPAChanged] BIT NOT NULL DEFAULT 0,
    [VersionMajor] INT NOT NULL DEFAULT 0,
    [VersionMinor] INT NOT NULL DEFAULT 0,
    [Options] NVARCHAR(MAX) NULL, 
    [CoronationEmblem] BIT NOT NULL DEFAULT 0,
    [IsRegulatedForProvider] BIT NOT NULL DEFAULT 0, 
    [IsRegulatedForEPAO] BIT NOT NULL DEFAULT 0, 
    [EpaoMustBeApprovedByRegulatorBody] BIT NOT NULL Default 0,
    [PublishDate] DATETIME NULL
    CONSTRAINT [AK_StandardImport_Column] UNIQUE ([StandardUId])
)
GO
