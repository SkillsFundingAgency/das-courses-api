CREATE TABLE [dbo].[FrameworkFundingPeriod]
(
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
	[FrameworkId] VARCHAR(15) NOT NULL,
	[EffectiveFrom] DATETIME NOT NULL,
	[EffectiveTo] DATETIME NULL,
	[FundingCap] INT NOT NULL
)
GO

CREATE NONCLUSTERED INDEX [IDX_FrameworkFundingPeriod_FrameworkId] ON [dbo].[FrameworkFundingPeriod] (FrameworkId) 
INCLUDE (Id,EffectiveFrom, EffectiveTo, FundingCap) WITH (ONLINE = ON) 
GO
