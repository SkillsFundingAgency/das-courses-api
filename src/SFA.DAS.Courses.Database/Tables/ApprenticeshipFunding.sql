﻿CREATE TABLE [dbo].[ApprenticeshipFunding]
(
    [Id] UNIQUEIDENTIFIER PRIMARY KEY,
	[StandardUId] VARCHAR(20) NULL,
	[EffectiveFrom] DATETIME NOT NULL,
	[EffectiveTo] DATETIME NULL,
	[MaxEmployerLevyCap] INT NOT NULL,
    [Duration] INT NOT NULL DEFAULT 0
)
GO