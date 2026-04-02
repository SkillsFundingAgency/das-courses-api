CREATE TABLE [dbo].[ShortCourseDates]
(
	[LarsCode] VARCHAR(8) NOT NULL PRIMARY KEY,
    [EffectiveFrom] DATETIME NOT NULL,
    [EffectiveTo] DATETIME NULL,
    [LastDateStarts] DATETIME NULL
)
