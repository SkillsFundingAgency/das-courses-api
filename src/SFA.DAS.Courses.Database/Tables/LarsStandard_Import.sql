CREATE TABLE [dbo].[LarsStandard_Import]
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
