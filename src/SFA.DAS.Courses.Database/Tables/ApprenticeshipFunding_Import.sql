﻿CREATE TABLE [dbo].[ApprenticeshipFunding_Import]
(
	[Id] UNIQUEIDENTIFIER PRIMARY KEY,
	[StandardId] INT NOT NULL,
	[EffectiveFrom] DATETIME NOT NULL,
	[EffectiveTo] DATETIME NULL,
	[MaxEmployerLevyCap] INT NOT NULL
)
GO