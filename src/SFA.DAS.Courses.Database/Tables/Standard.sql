CREATE TABLE [dbo].[Standard]
(
	[StandardUId] VARCHAR(20) PRIMARY KEY,
    [IfateReferenceNumber] VARCHAR(10) NOT NULL,
    [LarsCode] INT NULL,
	[Title] VARCHAR(1000) NOT NULL,
	[Level] INT NOT NULL,
	[IntegratedDegree] VARCHAR(100) NULL,
	[OverviewOfRole] VARCHAR(MAX) NOT NULL,
	[RouteId] UNIQUEIDENTIFIER NOT NULL,
	[Keywords] VARCHAR(MAX) NULL,
	[TypicalJobTitles] VARCHAR(MAX) NULL,
	[StandardPageUrl] VARCHAR(500) NOT NULL,
	[Version] DECIMAL NULL,
    [RegulatedBody] VARCHAR(1000) NULL,
    [Skills] NVARCHAR(MAX) NULL, 
    [Knowledge] NVARCHAR(MAX) NULL, 
	[Behaviours] NVARCHAR(MAX) NULL, 
    [Duties] NVARCHAR(MAX) NULL, 
    [CoreAndOptions] BIT NOT NULL DEFAULT 0, 
    [IntegratedApprenticeship] BIT NOT NULL DEFAULT 0
)
GO

CREATE NONCLUSTERED INDEX [IDX_Standard_LarsCode] ON [dbo].[Standard] (LarsCode) 
INCLUDE ([StandardUId], [IfateReferenceNumber], [Title] , [Level], [IntegratedDegree], [OverviewOfRole], [RouteId], [Keywords], [TypicalJobTitles], [StandardPageUrl], [Version], [RegulatedBody], [Skills], [Knowledge], [Behaviours], [Duties], [CoreAndOptions], [IntegratedApprenticeship]) WITH (ONLINE = ON) 
GO 

CREATE NONCLUSTERED INDEX [IDX_Standard_IfateReferenceNumber] ON [dbo].[Standard] (IfateReferenceNumber) 
INCLUDE ([StandardUId], [LarsCode], [Title] , [Level], [IntegratedDegree], [OverviewOfRole], [RouteId], [Keywords], [TypicalJobTitles], [StandardPageUrl], [Version], [RegulatedBody], [Skills], [Knowledge], [Behaviours], [Duties], [CoreAndOptions], [IntegratedApprenticeship]) WITH (ONLINE = ON) 
GO 

CREATE NONCLUSTERED INDEX [IDX_Standard_RouteId] ON [dbo].[Standard] (RouteId) 
INCLUDE ([StandardUId], [IfateReferenceNumber], [LarsCode], [Title] , [Level], [IntegratedDegree], [OverviewOfRole], [Keywords], [TypicalJobTitles], [StandardPageUrl], [Version], [RegulatedBody], [Skills], [Knowledge], [Behaviours], [Duties], [CoreAndOptions], [IntegratedApprenticeship]) WITH (ONLINE = ON) 
GO 
