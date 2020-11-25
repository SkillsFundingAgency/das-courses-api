CREATE TABLE [Versioning].[Standard]
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
    [SSA1] VARCHAR(500) NULL,
    [SSA2] VARCHAR(500) NULL,
    [IntegratedApprenticeship] BIT NULL,
    [IntegratedDegree] VARCHAR(100) NULL,
    [CoreAndOptions] BIT NULL,
    [TypicalJobTitles] VARCHAR(MAX) NULL,
    [StandardPageUrl] VARCHAR(500) NULL, 
    CONSTRAINT [AK_Standard_StandardUId] UNIQUE ([StandardUId])
)
GO

CREATE INDEX [IX_Standard_Status] ON [Versioning].[Standard] ([Status])
GO
