CREATE TABLE ##Standard_Import_Master
(
    StandardUId VARCHAR(20) PRIMARY KEY,
    IfateReferenceNumber VARCHAR(10) NOT NULL,
    LarsCode INT NULL,
    Status VARCHAR(100) NOT NULL,
    VersionEarliestStartDate DATETIME NULL,
    VersionLatestStartDate DATETIME NULL,
    VersionLatestEndDate DATETIME NULL,
    Title VARCHAR(1000) NOT NULL,
    Level INT NOT NULL,
    ProposedTypicalDuration INT NOT NULL,
    ProposedMaxFunding INT NOT NULL,
    IntegratedDegree VARCHAR(100) NULL,
    OverviewOfRole VARCHAR(MAX) NOT NULL,
    RouteCode INT NOT NULL DEFAULT 0,
    AssessmentPlanUrl VARCHAR(500) NULL,
    ApprovedForDelivery DATETIME NULL,
    TrailBlazerContact VARCHAR(200) NULL,
    EqaProviderName VARCHAR(200) NULL,
    EqaProviderContactName VARCHAR(200) NULL,
    EqaProviderContactEmail VARCHAR(200) NULL,
    EqaProviderWebLink VARCHAR(500) NULL,
    Keywords VARCHAR(MAX) NULL,
    TypicalJobTitles VARCHAR(MAX) NULL,
    StandardPageUrl VARCHAR(500) NOT NULL,
    Version VARCHAR(20) NULL,
    RegulatedBody VARCHAR(1000) NULL,
    Skills NVARCHAR(MAX) NULL,
    Knowledge NVARCHAR(MAX) NULL,
    Behaviours NVARCHAR(MAX) NULL,
    Duties NVARCHAR(MAX) NULL,
    CoreDuties NVARCHAR(MAX) NULL,
    CoreAndOptions BIT NOT NULL DEFAULT 0,
    IntegratedApprenticeship BIT NOT NULL DEFAULT 0,
    CreatedDate DATETIME NULL,
    EPAChanged BIT NOT NULL DEFAULT 0,
    VersionMajor INT NOT NULL DEFAULT 0,
    VersionMinor INT NOT NULL DEFAULT 0,
    Options NVARCHAR(MAX) NULL,
    CoronationEmblem BIT NOT NULL DEFAULT 0,
    IsRegulatedForProvider BIT NOT NULL DEFAULT 0,
    IsRegulatedForEPAO BIT NOT NULL DEFAULT 0,
    EpaoMustBeApprovedByRegulatorBody BIT NOT NULL DEFAULT 0,
    PublishDate DATETIME NULL
);

--DROP TABLE ##Standard_Import_Master

SET NOCOUNT ON;

SELECT 
    'INSERT INTO ##Standard_Import_Master (' +
    'StandardUId, IfateReferenceNumber, LarsCode, Status, VersionEarliestStartDate, VersionLatestStartDate, VersionLatestEndDate, Title, Level, ProposedTypicalDuration, ProposedMaxFunding, IntegratedDegree, OverviewOfRole, RouteCode, AssessmentPlanUrl, ApprovedForDelivery, TrailBlazerContact, EqaProviderName, EqaProviderContactName, EqaProviderContactEmail, EqaProviderWebLink, Keywords, TypicalJobTitles, StandardPageUrl, Version, RegulatedBody, Skills, Knowledge, Behaviours, Duties, CoreDuties, CoreAndOptions, IntegratedApprenticeship, CreatedDate, EPAChanged, VersionMajor, VersionMinor, Options, CoronationEmblem, IsRegulatedForProvider, IsRegulatedForEPAO, EpaoMustBeApprovedByRegulatorBody, PublishDate' +
    ') VALUES (' +
    '''' + REPLACE(REPLACE(ISNULL(StandardUId, ''), '''', ''''''), '’', '’’’’') + ''',' +
    '''' + REPLACE(REPLACE(ISNULL(IfateReferenceNumber, ''), '''', ''''''), '’', '’’’’') + ''',' +
    ISNULL(CAST(LarsCode AS VARCHAR), 'NULL') + ',' +
    '''' + REPLACE(REPLACE(Status, '''', ''''''), '’', '’’’’') + ''',' +
    ISNULL('''' + CONVERT(VARCHAR, VersionEarliestStartDate, 120) + '''', 'NULL') + ',' +
    ISNULL('''' + CONVERT(VARCHAR, VersionLatestStartDate, 120) + '''', 'NULL') + ',' +
    ISNULL('''' + CONVERT(VARCHAR, VersionLatestEndDate, 120) + '''', 'NULL') + ',' +
    '''' + REPLACE(REPLACE(Title, '''', ''''''), '’', '’’’’') + ''',' +
    CAST(Level AS VARCHAR) + ',' +
    CAST(ProposedTypicalDuration AS VARCHAR) + ',' +
    CAST(ProposedMaxFunding AS VARCHAR) + ',' +
    ISNULL('''' + REPLACE(REPLACE(IntegratedDegree, '''', ''''''), '’', '’’’’') + '''', 'NULL') + ',' +
    '''' + REPLACE(REPLACE(OverviewOfRole, '''', ''''''), '’', '’’’’') + ''',' +
    CAST(RouteCode AS VARCHAR) + ',' +
    ISNULL('''' + REPLACE(REPLACE(AssessmentPlanUrl, '''', ''''''), '’', '’’’’') + '''', 'NULL') + ',' +
    ISNULL('''' + CONVERT(VARCHAR, ApprovedForDelivery, 120) + '''', 'NULL') + ',' +
    ISNULL('''' + REPLACE(REPLACE(TrailBlazerContact, '''', ''''''), '’', '’’’’') + '''', 'NULL') + ',' +
    ISNULL('''' + REPLACE(REPLACE(EqaProviderName, '''', ''''''), '’', '’’’’') + '''', 'NULL') + ',' +
    ISNULL('''' + REPLACE(REPLACE(EqaProviderContactName, '''', ''''''), '’', '’’’’') + '''', 'NULL') + ',' +
    ISNULL('''' + REPLACE(REPLACE(EqaProviderContactEmail, '''', ''''''), '’', '’’’’') + '''', 'NULL') + ',' +
    ISNULL('''' + REPLACE(REPLACE(EqaProviderWebLink, '''', ''''''), '’', '’’’’') + '''', 'NULL') + ',' +
    ISNULL('''' + REPLACE(REPLACE(Keywords, '''', ''''''), '’', '’’’’') + '''', 'NULL') + ',' +
    ISNULL('''' + REPLACE(REPLACE(TypicalJobTitles, '''', ''''''), '’', '’’’’') + '''', 'NULL') + ',' +
    '''' + REPLACE(REPLACE(StandardPageUrl, '''', ''''''), '’', '’’’’') + ''',' +
    ISNULL('''' + REPLACE(REPLACE(Version, '''', ''''''), '’', '’’’’') + '''', 'NULL') + ',' +
    ISNULL('''' + REPLACE(REPLACE(RegulatedBody, '''', ''''''), '’', '’’’’') + '''', 'NULL') + ',' +
    ISNULL('''' + REPLACE(REPLACE(Skills, '''', ''''''), '’', '’’’’') + '''', 'NULL') + ',' +
    ISNULL('''' + REPLACE(REPLACE(Knowledge, '''', ''''''), '’', '’’’’') + '''', 'NULL') + ',' +
    ISNULL('''' + REPLACE(REPLACE(Behaviours, '''', ''''''), '’', '’’’’') + '''', 'NULL') + ',' +
    ISNULL('''' + REPLACE(REPLACE(Duties, '''', ''''''), '’', '’’’’') + '''', 'NULL') + ',' +
    ISNULL('''' + REPLACE(REPLACE(CoreDuties, '''', ''''''), '’', '’’’’') + '''', 'NULL') + ',' +
    CAST(CoreAndOptions AS VARCHAR) + ',' +
    CAST(IntegratedApprenticeship AS VARCHAR) + ',' +
    ISNULL('''' + CONVERT(VARCHAR, CreatedDate, 120) + '''', 'NULL') + ',' +
    CAST(EPAChanged AS VARCHAR) + ',' +
    CAST(VersionMajor AS VARCHAR) + ',' +
    CAST(VersionMinor AS VARCHAR) + ',' +
    ISNULL('''' + REPLACE(REPLACE(Options, '''', ''''''), '’', '’’’’') + '''', 'NULL') + ',' +
    CAST(CoronationEmblem AS VARCHAR) + ',' +
    CAST(IsRegulatedForProvider AS VARCHAR) + ',' +
    CAST(IsRegulatedForEPAO AS VARCHAR) + ',' +
    CAST(EpaoMustBeApprovedByRegulatorBody AS VARCHAR) + ',' +
    ISNULL('''' + CONVERT(VARCHAR, PublishDate, 120) + '''', 'NULL') +
    ');'
FROM dbo.Standard_Import;
