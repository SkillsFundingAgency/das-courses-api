﻿CREATE TABLE [Versioning].[Standard_Staging]
(
    [StandardUId] VARCHAR(40) PRIMARY KEY,
    [LarsCode] INT NULL,
    [ReferenceNumber] VARCHAR(20) NOT NULL,
    [Title] VARCHAR(1000) NOT NULL,
    [Status] VARCHAR(200) NOT NULL,
    [Version] VARCHAR(20) NULL,
    [EarlierStartDate] DATETIME NULL,
    [LatestStartDate] DATETIME NULL,
    [LatestEndDate] DATETIME NULL,
    [OverviewOfRole] VARCHAR(MAX) NULL,
    [Level] INT NULL,
    [TypicalDuration] INT NULL,
    [MaxFunding] SMALLMONEY NULL,
    [Route] VARCHAR(500) NULL,
    [Keywords] NVARCHAR(MAX) NULL,
    [AssessmentPlanUrl] VARCHAR(500) NULL,
    [SSA1] VARCHAR(500) NULL,
    [SSA2] VARCHAR(500) NULL,
    [StandardInformation] VARCHAR(MAX) NULL,
    [Knowledges] NVARCHAR(MAX) NULL, 
    [Behaviours] NVARCHAR(MAX) NULL,
    [Skills] NVARCHAR(MAX) NULL, 
    [Options] NVARCHAR(MAX) NULL,
    [OptionsUnstructuredTemplate] NVARCHAR(MAX) NULL,
    [IntegratedApprenticeship] BIT NULL,
    [IntegratedDegree] VARCHAR(100) NULL,
    [CoreAndOptions] BIT NULL,
    [TypicalJobTitles] VARCHAR(MAX) NULL,
    [StandardPageUrl] VARCHAR(500) NULL
)
GO
