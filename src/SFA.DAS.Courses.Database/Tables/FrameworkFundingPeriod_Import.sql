CREATE TABLE [dbo].[FrameworkFundingPeriod_Import]
(
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
	[FrameworkId] VARCHAR(15) NOT NULL,
	[EffectiveFrom] DATETIME NOT NULL,
	[EffectiveTo] DATETIME NULL,
	[FundingCap] INT NOT NULL
)
GO
