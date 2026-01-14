CREATE TABLE [dbo].[Funding_Import]
(
    [LearnAimRef] VARCHAR(8) NOT NULL PRIMARY KEY,
    [FundingCategory] VARCHAR(30) NOT NULL,
    [EffectiveFrom] DATETIME NOT NULL,
    [EffectiveTo] DATETIME NULL,
    [RateWeighted] DECIMAL(5, 2) NOT NULL,
    [RateUnWeighted] DECIMAL(5, 2) NOT NULL,
    [RatingFactor] CHAR(1) NOT NULL,
    [AdultSkillsFundingBand] VARCHAR(20) NULL,
    [FundedGuidedLearningHours] INT NULL
)