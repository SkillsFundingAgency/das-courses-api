CREATE TABLE [dbo].[Funding_Import]
(
    [Id] UNIQUEIDENTIFIER PRIMARY KEY,
    [LearnAimRef] VARCHAR(8) NOT NULL,
    [FundingCategory] VARCHAR(30) NOT NULL,
    [EffectiveFrom] DATETIME NOT NULL,
    [EffectiveTo] DATETIME NULL,
    [RateWeighted] DECIMAL(7,2) NOT NULL,
    [RateUnWeighted] DECIMAL(7,2) NOT NULL,
    [WeightingFactor] CHAR(1) NOT NULL,
    [AdultSkillsFundingBand] VARCHAR(20) NULL,
    [FundedGuidedLearningHours] INT NULL
)