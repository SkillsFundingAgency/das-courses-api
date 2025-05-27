CREATE TABLE [dbo].[ApprenticeshipFunding]
(
    [Id] UNIQUEIDENTIFIER PRIMARY KEY,
    [StandardUId] VARCHAR(20) NOT NULL,
    [EffectiveFrom] DATETIME NOT NULL,
    [EffectiveTo] DATETIME NULL,
    [MaxEmployerLevyCap] INT NOT NULL,
    [Duration] INT NOT NULL DEFAULT 0,
    [Incentive1618] int,
    [ProviderAdditionalPayment1618] int,
    [EmployerAdditionalPayment1618] int,
    [CareLeaverAdditionalPayment] int,
    [FoundationAppFirstEmpPayment] int,
    [FoundationAppSecondEmpPayment] int,
    [FoundationAppThirdEmpPayment] int
)
GO

CREATE NONCLUSTERED INDEX [IDX_ApprenticeshipFunding_StandardUId] ON [dbo].[ApprenticeshipFunding] (StandardUId) 
GO 