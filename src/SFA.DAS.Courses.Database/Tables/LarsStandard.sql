CREATE TABLE [dbo].[LarsStandard]
(
	[Id] UNIQUEIDENTIFIER PRIMARY KEY,
	[StandardId] INT NOT NULL,
	[Version] INT NOT NULL,
	[EffectiveFrom] DATETIME NOT NULL,
	[EffectiveTo] DATETIME NULL,
	[LastDateStarts] DATETIME NULL,
	[SectorSubjectAreaTier2] DECIMAL NULL,
)
GO

CREATE NONCLUSTERED INDEX [IDX_LarsStandard_StandardId] ON [dbo].[LarsStandard] (StandardId) 
INCLUDE (Id,Version,EffectiveFrom,EffectiveTo,  LastDateStarts, SectorSubjectAreaTier2) WITH (ONLINE = ON) 
GO 
