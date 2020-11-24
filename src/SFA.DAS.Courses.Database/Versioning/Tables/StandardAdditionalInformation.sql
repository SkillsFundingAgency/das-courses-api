CREATE TABLE [Versioning].[StandardAdditionalInformation]
(
    [StandardUId] VARCHAR(40) PRIMARY KEY,
    [StandardInformation] VARCHAR(MAX) NULL,
    [Keywords] NVARCHAR(MAX) NULL,
    [Knowledges] NVARCHAR(MAX) NULL, 
    [Behaviours] NVARCHAR(MAX) NULL,
    [Skills] NVARCHAR(MAX) NULL, 
    [Options] NVARCHAR(MAX) NULL,
    [OptionsUnstructuredTemplate] NVARCHAR(MAX) NULL
)
GO 
