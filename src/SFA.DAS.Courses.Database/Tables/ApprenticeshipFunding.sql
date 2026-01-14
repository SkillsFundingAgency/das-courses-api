CREATE TABLE [dbo].[ApprenticeshipFunding]
(
    [Id] UNIQUEIDENTIFIER PRIMARY KEY,
    [LarsCode] VARCHAR(8) NOT NULL,
    [EffectiveFrom] DATETIME NOT NULL,
    [EffectiveTo] DATETIME NULL,
    [MaxEmployerLevyCap] DECIMAL(5, 2) NOT NULL,
    [Duration] INT NOT NULL DEFAULT 0,
    [DurationUnits] VARCHAR(6) NOT NULL,
    [FundingStream] VARCHAR(30) NOT NULL,
    [Incentive1618] INT,
    [ProviderAdditionalPayment1618] INT,
    [EmployerAdditionalPayment1618] INT,
    [CareLeaverAdditionalPayment] INT,
    [FoundationAppFirstEmpPayment] INT,
    [FoundationAppSecondEmpPayment] INT,
    [FoundationAppThirdEmpPayment] INT
)
GO

CREATE NONCLUSTERED INDEX [IDX_ApprenticeshipFunding_LarsCode] ON [dbo].[ApprenticeshipFunding] (LarsCode) 
GO 