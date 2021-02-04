CREATE TABLE [dbo].[Standard]
(
	[StandardUId] VARCHAR(20) PRIMARY KEY,
    [IfateReferenceNumber] VARCHAR(10) NOT NULL,
    [LarsCode] INT NULL,
    [Status] VARCHAR(100) NOT NULL,
	[Title] VARCHAR(1000) NOT NULL,
	[Level] INT NOT NULL,
	[IntegratedDegree] VARCHAR(100) NULL,
	[OverviewOfRole] VARCHAR(MAX) NOT NULL,
	[RouteId] UNIQUEIDENTIFIER NOT NULL,
	[Keywords] VARCHAR(MAX) NULL,
	[TypicalJobTitles] VARCHAR(MAX) NULL,
	[StandardPageUrl] VARCHAR(500) NOT NULL,
	[Version] DECIMAL(18, 1) NULL,
    [RegulatedBody] VARCHAR(1000) NULL,
    [Skills] NVARCHAR(MAX) NULL, 
    [Knowledge] NVARCHAR(MAX) NULL, 
	[Behaviours] NVARCHAR(MAX) NULL, 
    [Duties] NVARCHAR(MAX) NULL, 
    [CoreAndOptions] BIT NOT NULL DEFAULT 0, 
    [IntegratedApprenticeship] BIT NOT NULL DEFAULT 0,
    [Options] NVARCHAR(MAX) NULL, 
    CONSTRAINT [AK_Standard_Column] UNIQUE ([StandardUId])
)
GO

CREATE NONCLUSTERED INDEX [IDX_Standard_RouteId] ON [dbo].[Standard] (RouteId) 
INCLUDE ([StandardUId], [IfateReferenceNumber], [LarsCode], [Status], [Title], [Level], [IntegratedDegree], [OverviewOfRole], [Keywords], [TypicalJobTitles], [StandardPageUrl], [Version], [RegulatedBody], [Skills], [Knowledge], [Behaviours], [Duties], [CoreAndOptions], [IntegratedApprenticeship]) WITH (ONLINE = ON) 
GO 

CREATE NONCLUSTERED INDEX [IDX_Standard_LarsCode] ON [dbo].[Standard] (LarsCode) 
INCLUDE ([StandardUId], [IfateReferenceNumber], [RouteId], [Status], [Title], [Level], [IntegratedDegree], [OverviewOfRole], [Keywords], [TypicalJobTitles], [StandardPageUrl], [Version], [RegulatedBody], [Skills], [Knowledge], [Behaviours], [Duties], [CoreAndOptions], [IntegratedApprenticeship]) WITH (ONLINE = ON) 
GO

CREATE NONCLUSTERED INDEX [IDX_Standard_Status] ON [dbo].[Standard] (Status) 
INCLUDE ([StandardUId], [IfateReferenceNumber], [RouteId], [LarsCode], [Title], [Level], [IntegratedDegree], [OverviewOfRole], [Keywords], [TypicalJobTitles], [StandardPageUrl], [Version], [RegulatedBody], [Skills], [Knowledge], [Behaviours], [Duties], [CoreAndOptions], [IntegratedApprenticeship]) WITH (ONLINE = ON) 
GO

CREATE NONCLUSTERED INDEX [IDX_Standard_IfateReferenceNumber] ON [dbo].[Standard] (IfateReferenceNumber) 
INCLUDE ([StandardUId], [Status], [RouteId], [LarsCode], [Title], [Level], [IntegratedDegree], [OverviewOfRole], [Keywords], [TypicalJobTitles], [StandardPageUrl], [Version], [RegulatedBody], [Skills], [Knowledge], [Behaviours], [Duties], [CoreAndOptions], [IntegratedApprenticeship]) WITH (ONLINE = ON) 
GO