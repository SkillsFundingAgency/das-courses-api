CREATE TABLE [dbo].[ApprenticeshipFunding_Import]
(
    [Id] UNIQUEIDENTIFIER PRIMARY KEY,
    [LarsCode] INT NOT NULL,
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
