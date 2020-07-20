CREATE TABLE [dbo].[Framework_Import]
(
	[Id] VARCHAR(15) NOT NULL PRIMARY KEY,
	[Title] VARCHAR(1000) NOT NULL,
	[FrameworkName] VARCHAR(500) NOT NULL,
	[PathwayName] VARCHAR(500) NOT NULL,
	[ProgType] INT NOT NULL,
	[FrameworkCode] INT NOT NULL,
	[PathwayCode] INT NOT NULL,
	[Level] INT NOT NULL,
	[TypicalLengthFrom] INT NOT NULL,
	[TypicalLengthTo] INT NOT NULL,
	[TypicalLengthUnit] VARCHAR(10) NOT NULL,
	[Duration] INT NOT NULL,
	[CurrentFundingCap] INT NOT NULL,
	[MaxFunding] INT NOT NULL,
	[Ssa1] decimal NOT NULL,
	[Ssa2] decimal NOT NULL,
	[EffectiveFrom] DATETIME NOT NULL,
	[EffectiveTo] DATETIME NOT NULL,
	[IsActiveFramework] BIT NOT NULL,
	[ProgrammeType] INT NOT NULL,
	[HasSubGroups] BIT NOT NULL,
	[ExtendedTitle] VARCHAR(2000) NOT NULL,
	CONSTRAINT [AK_FrameworkImport_Column] UNIQUE ([Id])
)
GO
