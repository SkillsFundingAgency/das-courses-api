CREATE TABLE [dbo].[FrameworkFundingPeriod_Import]
(
    [Id] INT PRIMARY KEY,
	[FrameworkId] VARCHAR(15) NOT NULL,
	[EffectiveFrom] DATETIME NOT NULL,
	[EffectiveTo] DATETIME NULL,
	[FundingCap] INT NOT NULL
)
GO
