CREATE TABLE [dbo].[SectorSubjectAreaTier1_Import]
(
    [SectorSubjectAreaTier1] INT NOT NULL PRIMARY KEY, 
    [SectorSubjectAreaTier1Desc] VARCHAR(500) NOT NULL, 
    [EffectiveFrom] DATETIME2 NULL, 
    [EffectiveTo] DATETIME2 NULL
)
