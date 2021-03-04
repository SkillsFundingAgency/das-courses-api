CREATE TABLE [dbo].[SectorSubjectAreaTier2]
(
	[SectorSubjectAreaTier2] decimal(10,4) NOT NULL PRIMARY KEY,
	[SectorSubjectAreaTier2Desc] VARCHAR(500) NOT NULL,
	[Name] VARCHAR(500) NOT NULL,
	[EffectiveFrom] DATETIME NOT NULL,
	[EffectiveTo] DATETIME NULL
)
GO

CREATE NONCLUSTERED INDEX [IDX_LarsStandard_SectorSubjectAreaTier2] ON [dbo].[LarsStandard] (SectorSubjectAreaTier2) 
GO 
