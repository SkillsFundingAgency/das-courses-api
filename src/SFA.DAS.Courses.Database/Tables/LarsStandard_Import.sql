CREATE TABLE [dbo].[LarsStandard_Import]
(
	[LarsCode] INT PRIMARY KEY,
	[Version] INT NOT NULL,
	[EffectiveFrom] DATETIME NOT NULL,
	[EffectiveTo] DATETIME NULL,
	[LastDateStarts] DATETIME NULL,
	[SectorSubjectAreaTier2] decimal(10,4) NOT NULL DEFAULT (0.0),
    [OtherBodyApprovalRequired] bit NOT NULL DEFAULT 0,
    [SectorCode] INT NOT NULL DEFAULT 0,
)
GO
