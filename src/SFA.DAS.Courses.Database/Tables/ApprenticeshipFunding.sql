﻿CREATE TABLE [dbo].[ApprenticeshipFunding]
(
	[Id] UNIQUEIDENTIFIER PRIMARY KEY,
    [StandardUId] VARCHAR(20) NOT NULL,
	[LarsCode] INT NOT NULL,
	[EffectiveFrom] DATETIME NOT NULL,
	[EffectiveTo] DATETIME NULL,
	[MaxEmployerLevyCap] INT NOT NULL,
    [Duration] INT NOT NULL DEFAULT 0
)
GO

CREATE NONCLUSTERED INDEX [IDX_ApprenticeshipFunding_StandardUId] ON [dbo].[ApprenticeshipFunding] ([StandardUId]) 
INCLUDE ([Id], [LarsCode], [EffectiveFrom] , [EffectiveTo], [MaxEmployerLevyCap], [Duration]) WITH (ONLINE = ON) 
GO 