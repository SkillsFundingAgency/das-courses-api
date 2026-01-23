-------------------------------------------------------------------------------
-- This script is used to show the differences between a set of saved Route, 
-- Route_Import, Standard and Standard_Import tables referred to as the 
-- _Master set and the current table contents, using the related scripts in 
-- the same folder generate the _Master tables (so called because the data
-- should originate form the master code branch) and leave the windows open to 
-- preserve the data, next run the algoritim changes in the API on the proposed
-- code branch to import data and finally run this script to show the differences
-- between the tables in the master branch and the code branch

-- This script produces 6 sets of data:
-- 1) The differences between the Route_Import_Master and Route_Import
-- 2) The differences between the Route_Master and Route
-- 3) The differences between the Standard_Import_Master and Standard_Import (added and removed rows)
-- 4) The differences between the Standard_Import_Master and Standard_Import (changed rows)
-- 5) The differences between the Standard_Master and Standard (added and removed rows)
-- 6) The differences between the Standard_Master and Standard (changed rows)
-------------------------------------------------------------------------------

-------------------------------------------------------------------------------
-- 1) ADDED (in [dbo].[Route_Import] but not in ##Route_Import_Master)
-------------------------------------------------------------------------------
SELECT
    'Added' AS DifferenceType,
    r.[Id],
    r.[Name],
    r.[Active],
    NULL AS Changes
FROM [dbo].[Route_Import] r
LEFT JOIN ##Route_Import_Master rm 
    ON r.Id = rm.Id
WHERE rm.Id IS NULL

UNION ALL

-------------------------------------------------------------------------------
-- 2) REMOVED (in ##Route_Import_Master but not in [dbo].[Route_Import])
-------------------------------------------------------------------------------
SELECT
    'Removed' AS DifferenceType,
    rm.[Id],
    rm.[Name],
    rm.[Active],
    NULL AS Changes
FROM ##Route_Import_Master rm
LEFT JOIN [dbo].[Route_Import] r 
    ON rm.Id = r.Id
WHERE r.Id IS NULL

UNION ALL

-------------------------------------------------------------------------------
-- 3) CHANGES (rows existing in both, but different columns)
-------------------------------------------------------------------------------
SELECT
    'Changes' AS DifferenceType,
    r.[Id],
    r.[Name],
    r.[Active],

    -- Build one row with all difference by STRING_AGG to unify them into a single column
    STRING_AGG(dif.ChangeDescription, '|') AS Changes
FROM [dbo].[Route_Import] r
JOIN ##Route_Import_Master rm 
    ON r.Id = rm.Id

CROSS APPLY
(
    VALUES
    -- Compare Name
    (
       CASE WHEN ISNULL(r.[Name], '<<NULL>>') 
                <> ISNULL(rm.[Name], '<<NULL>>')
         THEN 'Name|' + ISNULL(rm.[Name], 'NULL') 
              + '|' + ISNULL(r.[Name], 'NULL')
       END
    ),

    -- Compare Active
    (
       CASE WHEN ISNULL(CAST(r.[Active] AS VARCHAR), '<<NULL>>') 
                <> ISNULL(CAST(rm.[Active] AS VARCHAR), '<<NULL>>')
         THEN 'Active|' + ISNULL(CAST(rm.[Active] AS VARCHAR), 'NULL') 
              + '|' + ISNULL(CAST(r.[Active] AS VARCHAR), 'NULL')
       END
    )
) AS dif(ChangeDescription)

WHERE
    -- Only rows where at least one column is different
    (
        ISNULL(r.[Name], '<<NULL>>') <> ISNULL(rm.[Name], '<<NULL>>')
        OR ISNULL(r.[Active], 0)     <> ISNULL(rm.[Active], 0)
    )
GROUP BY
    r.[Id],
    r.[Name],
    r.[Active]

ORDER BY 
    DifferenceType,
    r.[Id];
GO

-------------------------------------------------------------------------------
-- 1) ADDED (in [dbo].[Route] but not in ##Route_Master)
-------------------------------------------------------------------------------
SELECT
    'Added' AS DifferenceType,
    r.[Id],
    r.[Name],
    r.[Active],
    NULL AS Changes
FROM [dbo].[Route] r
LEFT JOIN ##Route_Master rm ON r.Id = rm.Id
WHERE rm.Id IS NULL

UNION ALL

-------------------------------------------------------------------------------
-- 2) REMOVED (in ##Route_Master but not in [dbo].[Route])
-------------------------------------------------------------------------------
SELECT
    'Removed' AS DifferenceType,
    rm.[Id],
    rm.[Name],
    rm.[Active],
    NULL AS Changes
FROM ##Route_Master rm
LEFT JOIN [dbo].[Route] r ON rm.Id = r.Id
WHERE r.Id IS NULL

UNION ALL

-------------------------------------------------------------------------------
-- 3) CHANGES (rows existing in both, but different column values)
-------------------------------------------------------------------------------
SELECT
    'Changes' AS DifferenceType,
    r.[Id],
    r.[Name],
    r.[Active],

    -- Build one row with all difference by STRING_AGG to unify them into a single column
	STRING_AGG(dif.ChangeDescription, '|') AS Changes
FROM [dbo].[Route] r
JOIN ##Route_Master rm 
    ON r.Id = rm.Id

CROSS APPLY
(
    VALUES
    (
       CASE WHEN ISNULL(r.[Name], '<<NULL>>') 
                <> ISNULL(rm.[Name], '<<NULL>>')
         THEN 'Name|' + ISNULL(rm.[Name], 'NULL')
              + '|' + ISNULL(r.[Name], 'NULL')
       END
    ),
    (
       CASE WHEN ISNULL(CAST(r.[Active] AS VARCHAR), '<<NULL>>') 
                <> ISNULL(CAST(rm.[Active] AS VARCHAR), '<<NULL>>')
         THEN 'Active|' + ISNULL(CAST(rm.[Active] AS VARCHAR), 'NULL')
              + '|' + ISNULL(CAST(r.[Active] AS VARCHAR), 'NULL')
       END
    )
) AS dif(ChangeDescription)

WHERE
    (
        ISNULL(r.[Name], '<<NULL>>') 
            <> ISNULL(rm.[Name], '<<NULL>>')
        OR ISNULL(r.[Active], 0) 
            <> ISNULL(rm.[Active], 0)
    )
GROUP BY
    r.[Id],
    r.[Name],
    r.[Active]

ORDER BY 
    DifferenceType,
    r.[Id];
GO


-------------------------------------------------------------------------------
-- 1) ADDED (rows in Standard_Import but not in ##Standard_Import_Master)
-------------------------------------------------------------------------------
IF OBJECT_ID('tempdb..#Temp_Standard_Import_Changes') IS NOT NULL
    DROP TABLE #Temp_Standard_Import_Changes;

SELECT * INTO #Temp_Standard_Import_Changes
FROM
(
SELECT
    'Added' AS DifferenceType,
	NULL AS Changes,
    s.[StandardUId],
    s.[IfateReferenceNumber],
    s.[LarsCode],
    s.[Status],
    s.[VersionEarliestStartDate],
    s.[VersionLatestStartDate],
    s.[VersionLatestEndDate],
    s.[Title],
    s.[Level],
    s.[ProposedTypicalDuration],
    s.[ProposedMaxFunding],
    s.[IntegratedDegree],
    s.[OverviewOfRole],
    s.[RouteCode],
    s.[AssessmentPlanUrl],
    s.[ApprovedForDelivery],
    s.[TrailBlazerContact],
    s.[EqaProviderName],
    s.[EqaProviderContactName],
    s.[EqaProviderContactEmail],
    s.[EqaProviderWebLink],
    s.[Keywords],
    s.[TypicalJobTitles],
    s.[StandardPageUrl],
    s.[Version],
    s.[RegulatedBody],
    s.[Skills],
    s.[Knowledge],
    s.[Behaviours],
    s.[Duties],
    s.[CoreDuties],
    s.[CoreAndOptions],
    s.[IntegratedApprenticeship],
    s.[CreatedDate],
    s.[EPAChanged],
    s.[VersionMajor],
    s.[VersionMinor],
    s.[Options],
    s.[CoronationEmblem],
    s.[EpaoMustBeApprovedByRegulatorBody],
	s.[PublishDate],
	s.[ApprenticeshipType],
	s.[RelatedOccupations]
FROM [dbo].[Standard_Import] s
LEFT JOIN ##Standard_Import_Master sm 
    ON s.StandardUId = sm.StandardUId
WHERE sm.StandardUId IS NULL



UNION ALL

-------------------------------------------------------------------------------
-- 2) REMOVED (rows in ##Standard_Import_Master but not in Standard_Import)
-------------------------------------------------------------------------------
SELECT
    'Removed' AS DifferenceType,
	NULL AS Changes,
    sm.[StandardUId],
    sm.[IfateReferenceNumber],
    sm.[LarsCode],
    sm.[Status],
    sm.[VersionEarliestStartDate],
    sm.[VersionLatestStartDate],
    sm.[VersionLatestEndDate],
    sm.[Title],
    sm.[Level],
    sm.[ProposedTypicalDuration],
    sm.[ProposedMaxFunding],
    sm.[IntegratedDegree],
    sm.[OverviewOfRole],
    sm.[RouteCode],
    sm.[AssessmentPlanUrl],
    sm.[ApprovedForDelivery],
    sm.[TrailBlazerContact],
    sm.[EqaProviderName],
    sm.[EqaProviderContactName],
    sm.[EqaProviderContactEmail],
    sm.[EqaProviderWebLink],
    sm.[Keywords],
    sm.[TypicalJobTitles],
    sm.[StandardPageUrl],
    sm.[Version],
    sm.[RegulatedBody],
    sm.[Skills],
    sm.[Knowledge],
    sm.[Behaviours],
    sm.[Duties],
    sm.[CoreDuties],
    sm.[CoreAndOptions],
    sm.[IntegratedApprenticeship],
    sm.[CreatedDate],
    sm.[EPAChanged],
    sm.[VersionMajor],
    sm.[VersionMinor],
    sm.[Options],
    sm.[CoronationEmblem],
	sm.[EpaoMustBeApprovedByRegulatorBody],
	sm.[PublishDate],
	sm.[ApprenticeshipType],
	sm.[RelatedOccupations]
FROM ##Standard_Import_Master sm
LEFT JOIN [dbo].[Standard_Import] s 
    ON sm.StandardUId = s.StandardUId
WHERE s.StandardUId IS NULL

UNION ALL

-------------------------------------------------------------------------------
-- 3) CHANGES (rows present in both, but differing in >=1 column)
-------------------------------------------------------------------------------
SELECT
    'Changes' AS DifferenceType,
	-- Build one row with all difference by STRING_AGG to unify them into a single column
	STRING_AGG(dif.ChangeDescription, '|') AS Changes,
    s.[StandardUId],
    s.[IfateReferenceNumber],
    s.[LarsCode],
    s.[Status],
    s.[VersionEarliestStartDate],
    s.[VersionLatestStartDate],
    s.[VersionLatestEndDate],
    s.[Title],
    s.[Level],
    s.[ProposedTypicalDuration],
    s.[ProposedMaxFunding],
    s.[IntegratedDegree],
    s.[OverviewOfRole],
    s.[RouteCode],
    s.[AssessmentPlanUrl],
    s.[ApprovedForDelivery],
    s.[TrailBlazerContact],
    s.[EqaProviderName],
    s.[EqaProviderContactName],
    s.[EqaProviderContactEmail],
    s.[EqaProviderWebLink],
    s.[Keywords],
    s.[TypicalJobTitles],
    s.[StandardPageUrl],
    s.[Version],
    s.[RegulatedBody],
    s.[Skills],
    s.[Knowledge],
    s.[Behaviours],
    s.[Duties],
    s.[CoreDuties],
    s.[CoreAndOptions],
    s.[IntegratedApprenticeship],
    s.[CreatedDate],
    s.[EPAChanged],
    s.[VersionMajor],
    s.[VersionMinor],
    s.[Options],
    s.[CoronationEmblem],
    s.[EpaoMustBeApprovedByRegulatorBody],
	s.[PublishDate],
	s.[ApprenticeshipType],
	s.[RelatedOccupations]

FROM [dbo].[Standard_Import] s
JOIN ##Standard_Import_Master sm 
    ON s.StandardUId = sm.StandardUId

-- CROSS APPLY: one row per column-difference
CROSS APPLY
(
    VALUES
    (CASE WHEN ISNULL(s.[IfateReferenceNumber],'<<NULL>>') 
               <> ISNULL(sm.[IfateReferenceNumber],'<<NULL>>')
          THEN 'IfateReferenceNumber|' + ISNULL(sm.[IfateReferenceNumber], 'NULL')
               + '|' + ISNULL(s.[IfateReferenceNumber], 'NULL')
     END),
    (CASE WHEN ISNULL(CAST(s.[LarsCode] AS VARCHAR), '<<NULL>>') 
               <> ISNULL(CAST(sm.[LarsCode] AS VARCHAR), '<<NULL>>')
          THEN 'LarsCode|' + ISNULL(CAST(sm.[LarsCode] AS VARCHAR), 'NULL') 
               + '|' + ISNULL(CAST(s.[LarsCode] AS VARCHAR), 'NULL')
     END),
    (CASE WHEN ISNULL(s.[Status], '<<NULL>>') 
               <> ISNULL(sm.[Status], '<<NULL>>')
          THEN 'Status|' + ISNULL(sm.[Status], 'NULL') 
               + '|' + ISNULL(s.[Status], 'NULL')
     END),
    (CASE WHEN ISNULL(CAST(s.[VersionEarliestStartDate] AS VARCHAR), '<<NULL>>')
               <> ISNULL(CAST(sm.[VersionEarliestStartDate] AS VARCHAR), '<<NULL>>')
          THEN 'VersionEarliestStartDate|' 
               + ISNULL(CAST(sm.[VersionEarliestStartDate] AS VARCHAR), 'NULL')
               + '|'
               + ISNULL(CAST(s.[VersionEarliestStartDate] AS VARCHAR), 'NULL')
     END),
    (CASE WHEN ISNULL(CAST(s.[VersionLatestStartDate] AS VARCHAR), '<<NULL>>')
               <> ISNULL(CAST(sm.[VersionLatestStartDate] AS VARCHAR), '<<NULL>>')
          THEN 'VersionLatestStartDate|' 
               + ISNULL(CAST(sm.[VersionLatestStartDate] AS VARCHAR), 'NULL')
               + '|'
               + ISNULL(CAST(s.[VersionLatestStartDate] AS VARCHAR), 'NULL')
     END),
    (CASE WHEN ISNULL(CAST(s.[VersionLatestEndDate] AS VARCHAR), '<<NULL>>')
               <> ISNULL(CAST(sm.[VersionLatestEndDate] AS VARCHAR), '<<NULL>>')
          THEN 'VersionLatestEndDate|' 
               + ISNULL(CAST(sm.[VersionLatestEndDate] AS VARCHAR), 'NULL')
               + '|'
               + ISNULL(CAST(s.[VersionLatestEndDate] AS VARCHAR), 'NULL')
     END),
    (CASE WHEN ISNULL(s.[Title], '<<NULL>>') 
               <> ISNULL(sm.[Title], '<<NULL>>')
          THEN 'Title|' + ISNULL(sm.[Title], 'NULL')
               + '|' + ISNULL(s.[Title], 'NULL')
     END),
    (CASE WHEN ISNULL(CAST(s.[Level] AS VARCHAR), '<<NULL>>')
               <> ISNULL(CAST(sm.[Level] AS VARCHAR), '<<NULL>>')
          THEN 'Level|' + ISNULL(CAST(sm.[Level] AS VARCHAR), 'NULL')
               + '|' + ISNULL(CAST(s.[Level] AS VARCHAR), 'NULL')
     END),
    (CASE WHEN ISNULL(CAST(s.[ProposedTypicalDuration] AS VARCHAR), '<<NULL>>')
               <> ISNULL(CAST(sm.[ProposedTypicalDuration] AS VARCHAR), '<<NULL>>')
          THEN 'ProposedTypicalDuration|'
               + ISNULL(CAST(sm.[ProposedTypicalDuration] AS VARCHAR), 'NULL')
               + '|'
               + ISNULL(CAST(s.[ProposedTypicalDuration] AS VARCHAR), 'NULL')
     END),
    (CASE WHEN ISNULL(CAST(s.[ProposedMaxFunding] AS VARCHAR), '<<NULL>>')
               <> ISNULL(CAST(sm.[ProposedMaxFunding] AS VARCHAR), '<<NULL>>')
          THEN 'ProposedMaxFunding|'
               + ISNULL(CAST(sm.[ProposedMaxFunding] AS VARCHAR), 'NULL')
               + '|'
               + ISNULL(CAST(s.[ProposedMaxFunding] AS VARCHAR), 'NULL')
     END),
    (CASE WHEN ISNULL(s.[IntegratedDegree], '<<NULL>>')
               <> ISNULL(sm.[IntegratedDegree], '<<NULL>>')
          THEN 'IntegratedDegree|'
               + ISNULL(sm.[IntegratedDegree], 'NULL')
               + '|'
               + ISNULL(s.[IntegratedDegree], 'NULL')
     END),
    (CASE WHEN ISNULL(s.[OverviewOfRole], '<<NULL>>')
               <> ISNULL(sm.[OverviewOfRole], '<<NULL>>')
          THEN 'OverviewOfRole|'
               + ISNULL(sm.[OverviewOfRole], 'NULL')
               + '|'
               + ISNULL(s.[OverviewOfRole], 'NULL')
     END),
    (CASE WHEN ISNULL(CAST(s.[RouteCode] AS VARCHAR), '<<NULL>>')
               <> ISNULL(CAST(sm.[RouteCode] AS VARCHAR), '<<NULL>>')
          THEN 'RouteCode|'
               + ISNULL(CAST(sm.[RouteCode] AS VARCHAR), 'NULL')
               + '|'
               + ISNULL(CAST(s.[RouteCode] AS VARCHAR), 'NULL')
     END),
    (CASE WHEN ISNULL(s.[AssessmentPlanUrl], '<<NULL>>')
               <> ISNULL(sm.[AssessmentPlanUrl], '<<NULL>>')
          THEN 'AssessmentPlanUrl|'
               + ISNULL(sm.[AssessmentPlanUrl], 'NULL')
               + '|'
               + ISNULL(s.[AssessmentPlanUrl], 'NULL')
     END),
    (CASE WHEN ISNULL(CAST(s.[ApprovedForDelivery] AS VARCHAR), '<<NULL>>')
               <> ISNULL(CAST(sm.[ApprovedForDelivery] AS VARCHAR), '<<NULL>>')
          THEN 'ApprovedForDelivery|'
               + ISNULL(CAST(sm.[ApprovedForDelivery] AS VARCHAR), 'NULL')
               + '|'
               + ISNULL(CAST(s.[ApprovedForDelivery] AS VARCHAR), 'NULL')
     END),
    (CASE WHEN ISNULL(s.[TrailBlazerContact], '<<NULL>>')
               <> ISNULL(sm.[TrailBlazerContact], '<<NULL>>')
          THEN 'TrailBlazerContact|'
               + ISNULL(sm.[TrailBlazerContact], 'NULL')
               + '|'
               + ISNULL(s.[TrailBlazerContact], 'NULL')
     END),
    (CASE WHEN ISNULL(s.[EqaProviderName], '<<NULL>>')
               <> ISNULL(sm.[EqaProviderName], '<<NULL>>')
          THEN 'EqaProviderName|'
               + ISNULL(sm.[EqaProviderName], 'NULL')
               + '|'
               + ISNULL(s.[EqaProviderName], 'NULL')
     END),
    (CASE WHEN ISNULL(s.[EqaProviderContactName], '<<NULL>>')
               <> ISNULL(sm.[EqaProviderContactName], '<<NULL>>')
          THEN 'EqaProviderContactName|'
               + ISNULL(sm.[EqaProviderContactName], 'NULL')
               + '|'
               + ISNULL(s.[EqaProviderContactName], 'NULL')
     END),
    (CASE WHEN ISNULL(s.[EqaProviderContactEmail], '<<NULL>>')
               <> ISNULL(sm.[EqaProviderContactEmail], '<<NULL>>')
          THEN 'EqaProviderContactEmail|'
               + ISNULL(sm.[EqaProviderContactEmail], 'NULL')
               + '|'
               + ISNULL(s.[EqaProviderContactEmail], 'NULL')
     END),
    (CASE WHEN ISNULL(s.[EqaProviderWebLink], '<<NULL>>')
               <> ISNULL(sm.[EqaProviderWebLink], '<<NULL>>')
          THEN 'EqaProviderWebLink|'
               + ISNULL(sm.[EqaProviderWebLink], 'NULL')
               + '|'
               + ISNULL(s.[EqaProviderWebLink], 'NULL')
     END),
    (CASE WHEN ISNULL(s.[Keywords], '<<NULL>>')
               <> ISNULL(sm.[Keywords], '<<NULL>>')
          THEN 'Keywords|'
               + ISNULL(sm.[Keywords], 'NULL')
               + '|'
               + ISNULL(s.[Keywords], 'NULL')
     END),
    (CASE WHEN ISNULL(s.[TypicalJobTitles], '<<NULL>>')
               <> ISNULL(sm.[TypicalJobTitles], '<<NULL>>')
          THEN 'TypicalJobTitles|'
               + ISNULL(sm.[TypicalJobTitles], 'NULL')
               + '|'
               + ISNULL(s.[TypicalJobTitles], 'NULL')
     END),
    (CASE WHEN ISNULL(s.[StandardPageUrl], '<<NULL>>')
               <> ISNULL(sm.[StandardPageUrl], '<<NULL>>')
          THEN 'StandardPageUrl|'
               + ISNULL(sm.[StandardPageUrl], 'NULL')
               + '|'
               + ISNULL(s.[StandardPageUrl], 'NULL')
     END),
    (CASE WHEN ISNULL(s.[Version], '<<NULL>>')
               <> ISNULL(sm.[Version], '<<NULL>>')
          THEN 'Version|'
               + ISNULL(sm.[Version], 'NULL')
               + '|'
               + ISNULL(s.[Version], 'NULL')
     END),
    (CASE WHEN ISNULL(s.[RegulatedBody], '<<NULL>>')
               <> ISNULL(sm.[RegulatedBody], '<<NULL>>')
          THEN 'RegulatedBody|'
               + ISNULL(sm.[RegulatedBody], 'NULL')
               + '|'
               + ISNULL(s.[RegulatedBody], 'NULL')
     END),
    (CASE WHEN ISNULL(s.[Skills], '<<NULL>>')
               <> ISNULL(sm.[Skills], '<<NULL>>')
          THEN 'Skills|'
               + ISNULL(sm.[Skills], 'NULL')
               + '|'
               + ISNULL(s.[Skills], 'NULL')
     END),
    (CASE WHEN ISNULL(s.[Knowledge], '<<NULL>>')
               <> ISNULL(sm.[Knowledge], '<<NULL>>')
          THEN 'Knowledge|'
               + ISNULL(sm.[Knowledge], 'NULL')
               + '|'
               + ISNULL(s.[Knowledge], 'NULL')
     END),
    (CASE WHEN ISNULL(s.[Behaviours], '<<NULL>>')
               <> ISNULL(sm.[Behaviours], '<<NULL>>')
          THEN 'Behaviours|'
               + ISNULL(sm.[Behaviours], 'NULL')
               + '|'
               + ISNULL(s.[Behaviours], 'NULL')
     END),
    (CASE WHEN ISNULL(s.[Duties], '<<NULL>>')
               <> ISNULL(sm.[Duties], '<<NULL>>')
          THEN 'Duties|'
               + ISNULL(sm.[Duties], 'NULL')
               + '|'
               + ISNULL(s.[Duties], 'NULL')
     END),
    (CASE WHEN ISNULL(s.[CoreDuties], '<<NULL>>')
               <> ISNULL(sm.[CoreDuties], '<<NULL>>')
          THEN 'CoreDuties|'
               + ISNULL(sm.[CoreDuties], 'NULL')
               + '|'
               + ISNULL(s.[CoreDuties], 'NULL')
     END),
    (CASE WHEN ISNULL(CAST(s.[CoreAndOptions] AS VARCHAR), '<<NULL>>')
               <> ISNULL(CAST(sm.[CoreAndOptions] AS VARCHAR), '<<NULL>>')
          THEN 'CoreAndOptions|'
               + ISNULL(CAST(sm.[CoreAndOptions] AS VARCHAR), 'NULL')
               + '|'
               + ISNULL(CAST(s.[CoreAndOptions] AS VARCHAR), 'NULL')
     END),
    (CASE WHEN ISNULL(CAST(s.[IntegratedApprenticeship] AS VARCHAR), '<<NULL>>')
               <> ISNULL(CAST(sm.[IntegratedApprenticeship] AS VARCHAR), '<<NULL>>')
          THEN 'IntegratedApprenticeship|'
               + ISNULL(CAST(sm.[IntegratedApprenticeship] AS VARCHAR), 'NULL')
               + '|'
               + ISNULL(CAST(s.[IntegratedApprenticeship] AS VARCHAR), 'NULL')
     END),
    (CASE WHEN ISNULL(CAST(s.[CreatedDate] AS VARCHAR), '<<NULL>>')
               <> ISNULL(CAST(sm.[CreatedDate] AS VARCHAR), '<<NULL>>')
          THEN 'CreatedDate|'
               + ISNULL(CAST(sm.[CreatedDate] AS VARCHAR), 'NULL')
               + '|'
               + ISNULL(CAST(s.[CreatedDate] AS VARCHAR), 'NULL')
     END),
    (CASE WHEN ISNULL(CAST(s.[EPAChanged] AS VARCHAR), '<<NULL>>')
               <> ISNULL(CAST(sm.[EPAChanged] AS VARCHAR), '<<NULL>>')
          THEN 'EPAChanged|'
               + ISNULL(CAST(sm.[EPAChanged] AS VARCHAR), 'NULL')
               + '|'
               + ISNULL(CAST(s.[EPAChanged] AS VARCHAR), 'NULL')
     END),
    (CASE WHEN ISNULL(CAST(s.[VersionMajor] AS VARCHAR), '<<NULL>>')
               <> ISNULL(CAST(sm.[VersionMajor] AS VARCHAR), '<<NULL>>')
          THEN 'VersionMajor|'
               + ISNULL(CAST(sm.[VersionMajor] AS VARCHAR), 'NULL')
               + '|'
               + ISNULL(CAST(s.[VersionMajor] AS VARCHAR), 'NULL')
     END),
    (CASE WHEN ISNULL(CAST(s.[VersionMinor] AS VARCHAR), '<<NULL>>')
               <> ISNULL(CAST(sm.[VersionMinor] AS VARCHAR), '<<NULL>>')
          THEN 'VersionMinor|'
               + ISNULL(CAST(sm.[VersionMinor] AS VARCHAR), 'NULL')
               + '|'
               + ISNULL(CAST(s.[VersionMinor] AS VARCHAR), 'NULL')
     END),
    (CASE WHEN ISNULL(s.[Options], '<<NULL>>')
               <> ISNULL(sm.[Options], '<<NULL>>')
          THEN 'Options|'
               + ISNULL(sm.[Options], 'NULL')
               + '|'
               + ISNULL(s.[Options], 'NULL')
     END),
    (CASE WHEN ISNULL(CAST(s.[CoronationEmblem] AS VARCHAR), '<<NULL>>')
               <> ISNULL(CAST(sm.[CoronationEmblem] AS VARCHAR), '<<NULL>>')
          THEN 'CoronationEmblem|'
               + ISNULL(CAST(sm.[CoronationEmblem] AS VARCHAR), 'NULL')
               + '|'
               + ISNULL(CAST(s.[CoronationEmblem] AS VARCHAR), 'NULL')
     END),
	 (CASE WHEN ISNULL(CAST(s.[EpaoMustBeApprovedByRegulatorBody] AS VARCHAR), '<<NULL>>')
               <> ISNULL(CAST(sm.[EpaoMustBeApprovedByRegulatorBody] AS VARCHAR), '<<NULL>>')
          THEN 'EpaoMustBeApprovedByRegulatorBody|'
               + ISNULL(CAST(sm.[EpaoMustBeApprovedByRegulatorBody] AS VARCHAR), 'NULL')
               + '|'
               + ISNULL(CAST(s.[EpaoMustBeApprovedByRegulatorBody] AS VARCHAR), 'NULL')
     END),
	 (CASE WHEN ISNULL(CAST(s.[PublishDate] AS VARCHAR), '<<NULL>>')
               <> ISNULL(CAST(sm.[PublishDate] AS VARCHAR), '<<NULL>>')
          THEN 'PublishDate|'
               + ISNULL(CAST(sm.[PublishDate] AS VARCHAR), 'NULL')
               + '|'
               + ISNULL(CAST(s.[PublishDate] AS VARCHAR), 'NULL')
     END),
	 (CASE WHEN ISNULL(s.[ApprenticeshipType], '<<NULL>>')
               <> ISNULL(sm.[ApprenticeshipType], '<<NULL>>')
          THEN 'ApprenticeshipType|'
               + ISNULL(sm.[ApprenticeshipType], 'NULL')
               + '|'
               + ISNULL(s.[ApprenticeshipType], 'NULL')
     END),

	 (CASE WHEN ISNULL(s.[RelatedOccupations], '<<NULL>>')
               <> ISNULL(sm.[RelatedOccupations], '<<NULL>>')
          THEN 'RelatedOccupations|'
               + ISNULL(sm.[RelatedOccupations], 'NULL')
               + '|'
               + ISNULL(s.[RelatedOccupations], 'NULL')
     END)
) AS dif(ChangeDescription)

WHERE
    (
        ISNULL(s.[IfateReferenceNumber], '<<NULL>>') 
            <> ISNULL(sm.[IfateReferenceNumber], '<<NULL>>')
        OR ISNULL(s.[LarsCode], -999999) 
            <> ISNULL(sm.[LarsCode], -999999)
        OR ISNULL(s.[Status], '<<NULL>>') 
            <> ISNULL(sm.[Status], '<<NULL>>')
        OR ISNULL(s.[VersionEarliestStartDate], '19000101') 
            <> ISNULL(sm.[VersionEarliestStartDate], '19000101')
        OR ISNULL(s.[VersionLatestStartDate], '19000101') 
            <> ISNULL(sm.[VersionLatestStartDate], '19000101')
        OR ISNULL(s.[VersionLatestEndDate], '19000101') 
            <> ISNULL(sm.[VersionLatestEndDate], '19000101')
        OR ISNULL(s.[Title], '<<NULL>>') 
            <> ISNULL(sm.[Title], '<<NULL>>')
        OR ISNULL(s.[Level], -999999) 
            <> ISNULL(sm.[Level], -999999)
        OR ISNULL(s.[ProposedTypicalDuration], -999999) 
            <> ISNULL(sm.[ProposedTypicalDuration], -999999)
        OR ISNULL(s.[ProposedMaxFunding], -999999) 
            <> ISNULL(sm.[ProposedMaxFunding], -999999)
        OR ISNULL(s.[IntegratedDegree], '<<NULL>>') 
            <> ISNULL(sm.[IntegratedDegree], '<<NULL>>')
        OR ISNULL(s.[OverviewOfRole], '<<NULL>>') 
            <> ISNULL(sm.[OverviewOfRole], '<<NULL>>')
        OR ISNULL(s.[RouteCode], -999999) 
            <> ISNULL(sm.[RouteCode], -999999)
        OR ISNULL(s.[AssessmentPlanUrl], '<<NULL>>') 
            <> ISNULL(sm.[AssessmentPlanUrl], '<<NULL>>')
        OR ISNULL(s.[ApprovedForDelivery], '19000101') 
            <> ISNULL(sm.[ApprovedForDelivery], '19000101')
        OR ISNULL(s.[TrailBlazerContact], '<<NULL>>') 
            <> ISNULL(sm.[TrailBlazerContact], '<<NULL>>')
        OR ISNULL(s.[EqaProviderName], '<<NULL>>') 
            <> ISNULL(sm.[EqaProviderName], '<<NULL>>')
        OR ISNULL(s.[EqaProviderContactName], '<<NULL>>') 
            <> ISNULL(sm.[EqaProviderContactName], '<<NULL>>')
        OR ISNULL(s.[EqaProviderContactEmail], '<<NULL>>') 
            <> ISNULL(sm.[EqaProviderContactEmail], '<<NULL>>')
        OR ISNULL(s.[EqaProviderWebLink], '<<NULL>>') 
            <> ISNULL(sm.[EqaProviderWebLink], '<<NULL>>')
        OR ISNULL(s.[Keywords], '<<NULL>>') 
            <> ISNULL(sm.[Keywords], '<<NULL>>')
        OR ISNULL(s.[TypicalJobTitles], '<<NULL>>') 
            <> ISNULL(sm.[TypicalJobTitles], '<<NULL>>')
        OR ISNULL(s.[StandardPageUrl], '<<NULL>>') 
            <> ISNULL(sm.[StandardPageUrl], '<<NULL>>')
        OR ISNULL(s.[Version], '<<NULL>>') 
            <> ISNULL(sm.[Version], '<<NULL>>')
        OR ISNULL(s.[RegulatedBody], '<<NULL>>') 
            <> ISNULL(sm.[RegulatedBody], '<<NULL>>')
        OR ISNULL(s.[Skills], '<<NULL>>') 
            <> ISNULL(sm.[Skills], '<<NULL>>')
        OR ISNULL(s.[Knowledge], '<<NULL>>') 
            <> ISNULL(sm.[Knowledge], '<<NULL>>')
        OR ISNULL(s.[Behaviours], '<<NULL>>') 
            <> ISNULL(sm.[Behaviours], '<<NULL>>')
        OR ISNULL(s.[Duties], '<<NULL>>') 
            <> ISNULL(sm.[Duties], '<<NULL>>')
        OR ISNULL(s.[CoreDuties], '<<NULL>>') 
            <> ISNULL(sm.[CoreDuties], '<<NULL>>')
        OR ISNULL(s.[CoreAndOptions], 0) 
            <> ISNULL(sm.[CoreAndOptions], 0)
        OR ISNULL(s.[IntegratedApprenticeship], 0) 
            <> ISNULL(sm.[IntegratedApprenticeship], 0)
        OR ISNULL(s.[CreatedDate], '19000101') 
            <> ISNULL(sm.[CreatedDate], '19000101')
        OR ISNULL(s.[EPAChanged], 0) 
            <> ISNULL(sm.[EPAChanged], 0)
        OR ISNULL(s.[VersionMajor], -999999) 
            <> ISNULL(sm.[VersionMajor], -999999)
        OR ISNULL(s.[VersionMinor], -999999) 
            <> ISNULL(sm.[VersionMinor], -999999)
        OR ISNULL(s.[Options], '<<NULL>>') 
            <> ISNULL(sm.[Options], '<<NULL>>')
        OR ISNULL(s.[CoronationEmblem], 0) 
            <> ISNULL(sm.[CoronationEmblem], 0)
        OR ISNULL(s.EpaoMustBeApprovedByRegulatorBody, 0) 
            <> ISNULL(s.EpaoMustBeApprovedByRegulatorBody, 0)
		OR ISNULL(s.[PublishDate], '19000101') 
            <> ISNULL(sm.[PublishDate], '19000101')
		OR ISNULL(s.[ApprenticeshipType], '<<NULL>>') 
            <> ISNULL(sm.[ApprenticeshipType], '<<NULL>>')
		OR ISNULL(s.[RelatedOccupations], '<<NULL>>') 
            <> ISNULL(sm.[RelatedOccupations], '<<NULL>>')
    )
GROUP BY
    s.[StandardUId],
    s.[IfateReferenceNumber],
    s.[LarsCode],
    s.[Status],
    s.[VersionEarliestStartDate],
    s.[VersionLatestStartDate],
    s.[VersionLatestEndDate],
    s.[Title],
    s.[Level],
    s.[ProposedTypicalDuration],
    s.[ProposedMaxFunding],
    s.[IntegratedDegree],
    s.[OverviewOfRole],
    s.[RouteCode],
    s.[AssessmentPlanUrl],
    s.[ApprovedForDelivery],
    s.[TrailBlazerContact],
    s.[EqaProviderName],
    s.[EqaProviderContactName],
    s.[EqaProviderContactEmail],
    s.[EqaProviderWebLink],
    s.[Keywords],
    s.[TypicalJobTitles],
    s.[StandardPageUrl],
    s.[Version],
    s.[RegulatedBody],
    s.[Skills],
    s.[Knowledge],
    s.[Behaviours],
    s.[Duties],
    s.[CoreDuties],
    s.[CoreAndOptions],
    s.[IntegratedApprenticeship],
    s.[CreatedDate],
    s.[EPAChanged],
    s.[VersionMajor],
    s.[VersionMinor],
    s.[Options],
    s.[CoronationEmblem],
    s.[EpaoMustBeApprovedByRegulatorBody],
	s.[PublishDate],
	s.[ApprenticeshipType],
	s.[RelatedOccupations]
) [Standard_Import_Changes]

SELECT * 
FROM #Temp_Standard_Import_Changes
WHERE DifferenceType IN ('Removed', 'Added')
ORDER BY 
    DifferenceType,
    [StandardUId];


;WITH Split AS (
    SELECT 
		[StandardUId],
		DifferenceType,
        TRY_CAST([value] AS NVARCHAR(MAX)) AS Item,
        [ordinal]
    FROM #Temp_Standard_Import_Changes t
    CROSS APPLY STRING_SPLIT(t.[Changes], '|', 1)
),
Triples AS (
    SELECT
        [StandardUId],
		DifferenceType,
		Name = MAX(CASE WHEN (ordinal-1) % 3 = 0 THEN Item END),
        Was  = MAX(CASE WHEN (ordinal-1) % 3 = 1 THEN Item END),
        Now  = MAX(CASE WHEN (ordinal-1) % 3 = 2 THEN Item END),
        TripleGroup = (ordinal-1) / 3
    FROM Split
    GROUP BY [StandardUId], DifferenceType, (ordinal-1)/3
),
Lines AS (
    SELECT [StandardUId], DifferenceType, '  { "Name": "' + Name + '", "Was": "' + Was + '", "Now": "' + Now + '" },' [Change]
    FROM Triples
)
SELECT
	[Changes].DifferenceType,
	[Lines].[Change],
	[Changes].[StandardUId],
    [Changes].[IfateReferenceNumber],
    [Changes].[LarsCode],
    [Changes].[Status],
    [Changes].[VersionEarliestStartDate],
    [Changes].[VersionLatestStartDate],
    [Changes].[VersionLatestEndDate],
    [Changes].[Title],
    [Changes].[Level],
    [Changes].[ProposedTypicalDuration],
    [Changes].[ProposedMaxFunding],
    [Changes].[IntegratedDegree],
    [Changes].[OverviewOfRole],
    [Changes].[RouteCode],
    [Changes].[AssessmentPlanUrl],
    [Changes].[ApprovedForDelivery],
    [Changes].[TrailBlazerContact],
    [Changes].[EqaProviderName],
    [Changes].[EqaProviderContactName],
    [Changes].[EqaProviderContactEmail],
    [Changes].[EqaProviderWebLink],
    [Changes].[Keywords],
    [Changes].[TypicalJobTitles],
    [Changes].[StandardPageUrl],
    [Changes].[Version],
    [Changes].[RegulatedBody],
    [Changes].[Skills],
    [Changes].[Knowledge],
    [Changes].[Behaviours],
    [Changes].[Duties],
    [Changes].[CoreDuties],
    [Changes].[CoreAndOptions],
    [Changes].[IntegratedApprenticeship],
	[Changes].[CreatedDate],
    [Changes].[EPAChanged],
    [Changes].[VersionMajor],
    [Changes].[VersionMinor],
    [Changes].[Options],
    [Changes].[CoronationEmblem],
    [Changes].[EpaoMustBeApprovedByRegulatorBody],
	[Changes].[PublishDate],
	[Changes].[ApprenticeshipType],
	[Changes].[RelatedOccupations]
FROM 
	Lines INNER JOIN 
	#Temp_Standard_Import_Changes [Changes] ON Lines.StandardUId = [Changes].StandardUId
ORDER BY
	[Changes].StandardUId, [Changes].DifferenceType


---------------------

-------------------------------------------------------------------------------
-- 1) ADDED
-------------------------------------------------------------------------------
IF OBJECT_ID('tempdb..#Temp_Standard_Changes') IS NOT NULL
    DROP TABLE #Temp_Standard_Changes;

SELECT * INTO #Temp_Standard_Changes
FROM
(
SELECT
    'Added' AS DifferenceType,
	NULL AS Changes,
    s.[StandardUId],
    s.[IfateReferenceNumber],
    s.[LarsCode],
    s.[Status],
    s.[VersionEarliestStartDate],
    s.[VersionLatestStartDate],
    s.[VersionLatestEndDate],
    s.[Title],
    s.[Level],
    s.[ProposedTypicalDuration],
    s.[ProposedMaxFunding],
    s.[IntegratedDegree],
    s.[OverviewOfRole],
    s.[RouteCode],
    s.[AssessmentPlanUrl],
    s.[ApprovedForDelivery],
    s.[TrailBlazerContact],
    s.[EqaProviderName],
    s.[EqaProviderContactName],
    s.[EqaProviderContactEmail],
    s.[EqaProviderWebLink],
    s.[Keywords],
    s.[TypicalJobTitles],
    s.[StandardPageUrl],
    s.[Version],
    s.[RegulatedBody],
    s.[Skills],
    s.[Knowledge],
    s.[Behaviours],
    s.[Duties],
    s.[CoreDuties],
    s.[CoreAndOptions],
    s.[IntegratedApprenticeship],
	s.[CreatedDate],
    s.[EPAChanged],
    s.[VersionMajor],
    s.[VersionMinor],
    s.[Options],
    s.[CoronationEmblem],
    s.[EpaoMustBeApprovedByRegulatorBody],
	s.[PublishDate],
	s.[ApprenticeshipType],
	s.[RelatedOccupations]
FROM [dbo].[Standard] s
LEFT JOIN ##Standard_Master sm ON s.StandardUId = sm.StandardUId
WHERE sm.StandardUId IS NULL

UNION ALL

-------------------------------------------------------------------------------
-- 2) REMOVED
-------------------------------------------------------------------------------
SELECT
    'Removed' AS DifferenceType,
	NULL AS Changes,
    sm.[StandardUId],
    sm.[IfateReferenceNumber],
    sm.[LarsCode],
    sm.[Status],
    sm.[VersionEarliestStartDate],
    sm.[VersionLatestStartDate],
    sm.[VersionLatestEndDate],
    sm.[Title],
    sm.[Level],
    sm.[ProposedTypicalDuration],
    sm.[ProposedMaxFunding],
    sm.[IntegratedDegree],
    sm.[OverviewOfRole],
    sm.[RouteCode],
    sm.[AssessmentPlanUrl],
    sm.[ApprovedForDelivery],
    sm.[TrailBlazerContact],
    sm.[EqaProviderName],
    sm.[EqaProviderContactName],
    sm.[EqaProviderContactEmail],
    sm.[EqaProviderWebLink],
    sm.[Keywords],
    sm.[TypicalJobTitles],
    sm.[StandardPageUrl],
    sm.[Version],
    sm.[RegulatedBody],
    sm.[Skills],
    sm.[Knowledge],
    sm.[Behaviours],
    sm.[Duties],
    sm.[CoreDuties],
    sm.[CoreAndOptions],
    sm.[IntegratedApprenticeship],
	sm.[CreatedDate],
    sm.[EPAChanged],
    sm.[VersionMajor],
    sm.[VersionMinor],
    sm.[Options],
    sm.[CoronationEmblem],
    sm.[EpaoMustBeApprovedByRegulatorBody],
	sm.[PublishDate],
	sm.[ApprenticeshipType],
	sm.[RelatedOccupations]
FROM ##Standard_Master sm
LEFT JOIN [dbo].[Standard] s ON sm.StandardUId = s.StandardUId
WHERE s.StandardUId IS NULL

UNION ALL

-------------------------------------------------------------------------------
-- 3) CHANGES (all columns in a single semicolon-separated string)
-------------------------------------------------------------------------------
SELECT
    'Changes' AS DifferenceType,

	-- Build one row with all difference by STRING_AGG to unify them into a single column
    STRING_AGG(dif.ChangeDescription, '|') AS Changes,

    s.[StandardUId],
    s.[IfateReferenceNumber],
    s.[LarsCode],
    s.[Status],
    s.[VersionEarliestStartDate],
    s.[VersionLatestStartDate],
    s.[VersionLatestEndDate],
    s.[Title],
    s.[Level],
    s.[ProposedTypicalDuration],
    s.[ProposedMaxFunding],
    s.[IntegratedDegree],
    s.[OverviewOfRole],
    s.[RouteCode],
    s.[AssessmentPlanUrl],
    s.[ApprovedForDelivery],
    s.[TrailBlazerContact],
    s.[EqaProviderName],
    s.[EqaProviderContactName],
    s.[EqaProviderContactEmail],
    s.[EqaProviderWebLink],
    s.[Keywords],
    s.[TypicalJobTitles],
    s.[StandardPageUrl],
    s.[Version],
    s.[RegulatedBody],
    s.[Skills],
    s.[Knowledge],
    s.[Behaviours],
    s.[Duties],
    s.[CoreDuties],
    s.[CoreAndOptions],
    s.[IntegratedApprenticeship],
	s.[CreatedDate],
    s.[EPAChanged],
    s.[VersionMajor],
    s.[VersionMinor],
    s.[Options],
    s.[CoronationEmblem],
    s.[EpaoMustBeApprovedByRegulatorBody],
	s.[PublishDate],
	s.[ApprenticeshipType],
	s.[RelatedOccupations]
FROM [dbo].[Standard] s
JOIN ##Standard_Master sm ON s.StandardUId = sm.StandardUId

CROSS APPLY
(
    VALUES
    -- 1. IfateReferenceNumber
    (
       CASE WHEN ISNULL(s.[IfateReferenceNumber],'<<NULL>>') 
                 <> ISNULL(sm.[IfateReferenceNumber],'<<NULL>>')
         THEN 'IfateReferenceNumber|' + ISNULL(sm.[IfateReferenceNumber], 'NULL')
              + '|' + ISNULL(s.[IfateReferenceNumber], 'NULL')
       END
    ),

    -- 2. LarsCode
    (
       CASE WHEN ISNULL(CAST(s.[LarsCode] AS VARCHAR), '<<NULL>>')
                 <> ISNULL(CAST(sm.[LarsCode] AS VARCHAR), '<<NULL>>')
         THEN 'LarsCode|' + ISNULL(CAST(sm.[LarsCode] AS VARCHAR), 'NULL')
              + '|' + ISNULL(CAST(s.[LarsCode] AS VARCHAR), 'NULL')
       END
    ),

    -- 3. Status
    (
       CASE WHEN ISNULL(s.[Status], '<<NULL>>') 
                 <> ISNULL(sm.[Status], '<<NULL>>')
         THEN 'Status|' + ISNULL(sm.[Status], 'NULL')
              + '|' + ISNULL(s.[Status], 'NULL')
       END
    ),

    -- 4. VersionEarliestStartDate
    (
       CASE WHEN ISNULL(CAST(s.[VersionEarliestStartDate] AS VARCHAR), '<<NULL>>') 
                 <> ISNULL(CAST(sm.[VersionEarliestStartDate] AS VARCHAR), '<<NULL>>')
         THEN 'VersionEarliestStartDate|'
              + ISNULL(CAST(sm.[VersionEarliestStartDate] AS VARCHAR), 'NULL')
              + '|'
              + ISNULL(CAST(s.[VersionEarliestStartDate] AS VARCHAR), 'NULL')
       END
    ),

    -- 5. VersionLatestStartDate
    (
       CASE WHEN ISNULL(CAST(s.[VersionLatestStartDate] AS VARCHAR), '<<NULL>>') 
                 <> ISNULL(CAST(sm.[VersionLatestStartDate] AS VARCHAR), '<<NULL>>')
         THEN 'VersionLatestStartDate|'
              + ISNULL(CAST(sm.[VersionLatestStartDate] AS VARCHAR), 'NULL')
              + '|'
              + ISNULL(CAST(s.[VersionLatestStartDate] AS VARCHAR), 'NULL')
       END
    ),

    -- 6. VersionLatestEndDate
    (
       CASE WHEN ISNULL(CAST(s.[VersionLatestEndDate] AS VARCHAR), '<<NULL>>') 
                 <> ISNULL(CAST(sm.[VersionLatestEndDate] AS VARCHAR), '<<NULL>>')
         THEN 'VersionLatestEndDate|'
              + ISNULL(CAST(sm.[VersionLatestEndDate] AS VARCHAR), 'NULL')
              + '|'
              + ISNULL(CAST(s.[VersionLatestEndDate] AS VARCHAR), 'NULL')
       END
    ),

    -- 7. Title
    (
       CASE WHEN ISNULL(s.[Title], '<<NULL>>') 
                 <> ISNULL(sm.[Title], '<<NULL>>')
         THEN 'Title|' + ISNULL(sm.[Title], 'NULL')
              + '|' + ISNULL(s.[Title], 'NULL')
       END
    ),

    -- 8. Level
    (
       CASE WHEN ISNULL(CAST(s.[Level] AS VARCHAR), '<<NULL>>') 
                 <> ISNULL(CAST(sm.[Level] AS VARCHAR), '<<NULL>>')
         THEN 'Level|' + ISNULL(CAST(sm.[Level] AS VARCHAR), 'NULL')
              + '|' + ISNULL(CAST(s.[Level] AS VARCHAR), 'NULL')
       END
    ),

    -- 9. ProposedTypicalDuration
    (
       CASE WHEN ISNULL(CAST(s.[ProposedTypicalDuration] AS VARCHAR), '<<NULL>>') 
                 <> ISNULL(CAST(sm.[ProposedTypicalDuration] AS VARCHAR), '<<NULL>>')
         THEN 'ProposedTypicalDuration|'
              + ISNULL(CAST(sm.[ProposedTypicalDuration] AS VARCHAR), 'NULL')
              + '|'
              + ISNULL(CAST(s.[ProposedTypicalDuration] AS VARCHAR), 'NULL')
       END
    ),

    -- 10. ProposedMaxFunding
    (
       CASE WHEN ISNULL(CAST(s.[ProposedMaxFunding] AS VARCHAR), '<<NULL>>') 
                 <> ISNULL(CAST(sm.[ProposedMaxFunding] AS VARCHAR), '<<NULL>>')
         THEN 'ProposedMaxFunding|'
              + ISNULL(CAST(sm.[ProposedMaxFunding] AS VARCHAR), 'NULL')
              + '|'
              + ISNULL(CAST(s.[ProposedMaxFunding] AS VARCHAR), 'NULL')
       END
    ),

    -- 11. IntegratedDegree
    (
       CASE WHEN ISNULL(s.[IntegratedDegree], '<<NULL>>') 
                 <> ISNULL(sm.[IntegratedDegree], '<<NULL>>')
         THEN 'IntegratedDegree|'
              + ISNULL(sm.[IntegratedDegree], 'NULL')
              + '|'
              + ISNULL(s.[IntegratedDegree], 'NULL')
       END
    ),

    -- 12. OverviewOfRole
    (
       CASE WHEN ISNULL(s.[OverviewOfRole], '<<NULL>>') 
                 <> ISNULL(sm.[OverviewOfRole], '<<NULL>>')
         THEN 'OverviewOfRole|'
              + ISNULL(sm.[OverviewOfRole], 'NULL')
              + '|'
              + ISNULL(s.[OverviewOfRole], 'NULL')
       END
    ),

    -- 13. RouteCode
    (
       CASE WHEN ISNULL(CAST(s.[RouteCode] AS VARCHAR), '<<NULL>>')
                 <> ISNULL(CAST(sm.[RouteCode] AS VARCHAR), '<<NULL>>')
         THEN 'RouteCode|'
              + ISNULL(CAST(sm.[RouteCode] AS VARCHAR), 'NULL')
              + '|'
              + ISNULL(CAST(s.[RouteCode] AS VARCHAR), 'NULL')
       END
    ),

    -- 14. AssessmentPlanUrl
    (
       CASE WHEN ISNULL(s.[AssessmentPlanUrl], '<<NULL>>') 
                 <> ISNULL(sm.[AssessmentPlanUrl], '<<NULL>>')
         THEN 'AssessmentPlanUrl|'
              + ISNULL(sm.[AssessmentPlanUrl], 'NULL')
              + '|'
              + ISNULL(s.[AssessmentPlanUrl], 'NULL')
       END
    ),

    -- 15. ApprovedForDelivery
    (
       CASE WHEN ISNULL(CAST(s.[ApprovedForDelivery] AS VARCHAR), '<<NULL>>')
                 <> ISNULL(CAST(sm.[ApprovedForDelivery] AS VARCHAR), '<<NULL>>')
         THEN 'ApprovedForDelivery|'
              + ISNULL(CAST(sm.[ApprovedForDelivery] AS VARCHAR), 'NULL')
              + '|'
              + ISNULL(CAST(s.[ApprovedForDelivery] AS VARCHAR), 'NULL')
       END
    ),

    -- 16. TrailBlazerContact
    (
       CASE WHEN ISNULL(s.[TrailBlazerContact], '<<NULL>>')
                 <> ISNULL(sm.[TrailBlazerContact], '<<NULL>>')
         THEN 'TrailBlazerContact|'
              + ISNULL(sm.[TrailBlazerContact], 'NULL')
              + '|'
              + ISNULL(s.[TrailBlazerContact], 'NULL')
       END
    ),

    -- 17. EqaProviderName
    (
       CASE WHEN ISNULL(s.[EqaProviderName], '<<NULL>>')
                 <> ISNULL(sm.[EqaProviderName], '<<NULL>>')
         THEN 'EqaProviderName|'
              + ISNULL(sm.[EqaProviderName], 'NULL')
              + '|'
              + ISNULL(s.[EqaProviderName], 'NULL')
       END
    ),

    -- 18. EqaProviderContactName
    (
       CASE WHEN ISNULL(s.[EqaProviderContactName], '<<NULL>>')
                 <> ISNULL(sm.[EqaProviderContactName], '<<NULL>>')
         THEN 'EqaProviderContactName|'
              + ISNULL(sm.[EqaProviderContactName], 'NULL')
              + '|'
              + ISNULL(s.[EqaProviderContactName], 'NULL')
       END
    ),

    -- 19. EqaProviderContactEmail
    (
       CASE WHEN ISNULL(s.[EqaProviderContactEmail], '<<NULL>>')
                 <> ISNULL(sm.[EqaProviderContactEmail], '<<NULL>>')
         THEN 'EqaProviderContactEmail|'
              + ISNULL(sm.[EqaProviderContactEmail], 'NULL')
              + '|'
              + ISNULL(s.[EqaProviderContactEmail], 'NULL')
       END
    ),

    -- 20. EqaProviderWebLink
    (
       CASE WHEN ISNULL(s.[EqaProviderWebLink], '<<NULL>>')
                 <> ISNULL(sm.[EqaProviderWebLink], '<<NULL>>')
         THEN 'EqaProviderWebLink|'
              + ISNULL(sm.[EqaProviderWebLink], 'NULL')
              + '|'
              + ISNULL(s.[EqaProviderWebLink], 'NULL')
       END
    ),

    -- 21. Keywords
    (
       CASE WHEN ISNULL(s.[Keywords], '<<NULL>>')
                 <> ISNULL(sm.[Keywords], '<<NULL>>')
         THEN 'Keywords|'
              + ISNULL(sm.[Keywords], 'NULL')
              + '|'
              + ISNULL(s.[Keywords], 'NULL')
       END
    ),

    -- 22. TypicalJobTitles
    (
       CASE WHEN ISNULL(s.[TypicalJobTitles], '<<NULL>>')
                 <> ISNULL(sm.[TypicalJobTitles], '<<NULL>>')
         THEN 'TypicalJobTitles|'
              + ISNULL(sm.[TypicalJobTitles], 'NULL')
              + '|'
              + ISNULL(s.[TypicalJobTitles], 'NULL')
       END
    ),

    -- 23. StandardPageUrl
    (
       CASE WHEN ISNULL(s.[StandardPageUrl], '<<NULL>>')
                 <> ISNULL(sm.[StandardPageUrl], '<<NULL>>')
         THEN 'StandardPageUrl|'
              + ISNULL(sm.[StandardPageUrl], 'NULL')
              + '|'
              + ISNULL(s.[StandardPageUrl], 'NULL')
       END
    ),

    -- 24. Version
    (
       CASE WHEN ISNULL(s.[Version], '<<NULL>>')
                 <> ISNULL(sm.[Version], '<<NULL>>')
         THEN 'Version|'
              + ISNULL(sm.[Version], 'NULL')
              + '|'
              + ISNULL(s.[Version], 'NULL')
       END
    ),

    -- 25. RegulatedBody
    (
       CASE WHEN ISNULL(s.[RegulatedBody], '<<NULL>>')
                 <> ISNULL(sm.[RegulatedBody], '<<NULL>>')
         THEN 'RegulatedBody|'
              + ISNULL(sm.[RegulatedBody], 'NULL')
              + '|'
              + ISNULL(s.[RegulatedBody], 'NULL')
       END
    ),

    -- 26. Skills
    (
       CASE WHEN ISNULL(s.[Skills], '<<NULL>>')
                 <> ISNULL(sm.[Skills], '<<NULL>>')
         THEN 'Skills|'
              + ISNULL(sm.[Skills], 'NULL')
              + '|'
              + ISNULL(s.[Skills], 'NULL')
       END
    ),

    -- 27. Knowledge
    (
       CASE WHEN ISNULL(s.[Knowledge], '<<NULL>>')
                 <> ISNULL(sm.[Knowledge], '<<NULL>>')
         THEN 'Knowledge|'
              + ISNULL(sm.[Knowledge], 'NULL')
              + '|'
              + ISNULL(s.[Knowledge], 'NULL')
       END
    ),

    -- 28. Behaviours
    (
       CASE WHEN ISNULL(s.[Behaviours], '<<NULL>>')
                 <> ISNULL(sm.[Behaviours], '<<NULL>>')
         THEN 'Behaviours|'
              + ISNULL(sm.[Behaviours], 'NULL')
              + '|'
              + ISNULL(s.[Behaviours], 'NULL')
       END
    ),

    -- 29. Duties
    (
	   CASE WHEN ISNULL(REPLACE(s.[Duties], CHAR(160), ' '), '<<NULL>>')
                 <> ISNULL(REPLACE(sm.[Duties], CHAR(160), ' '), '<<NULL>>')
         THEN 'Duties|'
              + ISNULL(REPLACE(sm.[Duties], CHAR(160), ' '), 'NULL')
              + '|'
              + ISNULL(REPLACE(s.[Duties], CHAR(160), ' '), 'NULL')
       END
    ),

    -- 30. CoreDuties
    (
       CASE WHEN ISNULL(s.[CoreDuties], '<<NULL>>')
                 <> ISNULL(sm.[CoreDuties], '<<NULL>>')
         THEN 'CoreDuties|'
              + ISNULL(sm.[CoreDuties], 'NULL')
              + '|'
              + ISNULL(s.[CoreDuties], 'NULL')
       END
    ),

    -- 31. CoreAndOptions
    (
       CASE WHEN ISNULL(CAST(s.[CoreAndOptions] AS VARCHAR), '<<NULL>>')
                 <> ISNULL(CAST(sm.[CoreAndOptions] AS VARCHAR), '<<NULL>>')
         THEN 'CoreAndOptions|'
              + ISNULL(CAST(sm.[CoreAndOptions] AS VARCHAR), 'NULL')
              + '|'
              + ISNULL(CAST(s.[CoreAndOptions] AS VARCHAR), 'NULL')
       END
    ),

    -- 32. IntegratedApprenticeship
    (
       CASE WHEN ISNULL(CAST(s.[IntegratedApprenticeship] AS VARCHAR), '<<NULL>>')
                 <> ISNULL(CAST(sm.[IntegratedApprenticeship] AS VARCHAR), '<<NULL>>')
         THEN 'IntegratedApprenticeship|'
              + ISNULL(CAST(sm.[IntegratedApprenticeship] AS VARCHAR), 'NULL')
              + '|'
              + ISNULL(CAST(s.[IntegratedApprenticeship] AS VARCHAR), 'NULL')
       END
    ),

	-- 33. CreatedDate
    (
       CASE WHEN ISNULL(CAST(s.[CreatedDate] AS VARCHAR), '<<NULL>>')
                 <> ISNULL(CAST(sm.[CreatedDate] AS VARCHAR), '<<NULL>>')
         THEN 'CreatedDate|'
              + ISNULL(CAST(sm.[CreatedDate] AS VARCHAR), 'NULL')
              + '|'
              + ISNULL(CAST(s.[CreatedDate] AS VARCHAR), 'NULL')
       END
    ),

    -- 34. EPAChanged
    (
       CASE WHEN ISNULL(CAST(s.[EPAChanged] AS VARCHAR), '<<NULL>>')
                 <> ISNULL(CAST(sm.[EPAChanged] AS VARCHAR), '<<NULL>>')
         THEN 'EPAChanged|'
              + ISNULL(CAST(sm.[EPAChanged] AS VARCHAR), 'NULL')
              + '|'
              + ISNULL(CAST(s.[EPAChanged] AS VARCHAR), 'NULL')
       END
    ),

    -- 35. VersionMajor
    (
       CASE WHEN ISNULL(CAST(s.[VersionMajor] AS VARCHAR), '<<NULL>>')
                 <> ISNULL(CAST(sm.[VersionMajor] AS VARCHAR), '<<NULL>>')
         THEN 'VersionMajor|'
              + ISNULL(CAST(sm.[VersionMajor] AS VARCHAR), 'NULL')
              + '|'
              + ISNULL(CAST(s.[VersionMajor] AS VARCHAR), 'NULL')
       END
    ),

    -- 36. VersionMinor
    (
       CASE WHEN ISNULL(CAST(s.[VersionMinor] AS VARCHAR), '<<NULL>>')
                 <> ISNULL(CAST(sm.[VersionMinor] AS VARCHAR), '<<NULL>>')
         THEN 'VersionMinor|'
              + ISNULL(CAST(sm.[VersionMinor] AS VARCHAR), 'NULL')
              + '|'
              + ISNULL(CAST(s.[VersionMinor] AS VARCHAR), 'NULL')
       END
    ),

    -- 37. Options
    (
       -- Normalize whitespace in Duties, Skills, or Options fields
	   CASE WHEN ISNULL(s.[Options], '<<NULL>>')
                 <> ISNULL(sm.[Options], '<<NULL>>')
         THEN 'Options|'
              + ISNULL(sm.[Options], 'NULL')
              + '|'
              + ISNULL(s.[Options], 'NULL')
       END
    ),

    -- 38. CoronationEmblem
    (
       CASE WHEN ISNULL(CAST(s.[CoronationEmblem] AS VARCHAR), '<<NULL>>')
                 <> ISNULL(CAST(sm.[CoronationEmblem] AS VARCHAR), '<<NULL>>')
         THEN 'CoronationEmblem|'
              + ISNULL(CAST(sm.[CoronationEmblem] AS VARCHAR), 'NULL')
              + '|'
              + ISNULL(CAST(s.[CoronationEmblem] AS VARCHAR), 'NULL')
       END
    ),

    -- 39. EpaoMustBeApprovedByRegulatorBody
    (
       CASE WHEN ISNULL(CAST(s.[EpaoMustBeApprovedByRegulatorBody] AS VARCHAR), '<<NULL>>')
                 <> ISNULL(CAST(sm.[EpaoMustBeApprovedByRegulatorBody] AS VARCHAR), '<<NULL>>')
         THEN 'EpaoMustBeApprovedByRegulatorBody|'
              + ISNULL(CAST(sm.[EpaoMustBeApprovedByRegulatorBody] AS VARCHAR), 'NULL')
              + '|'
              + ISNULL(CAST(s.[EpaoMustBeApprovedByRegulatorBody] AS VARCHAR), 'NULL')
       END
	),

	-- 40. PublishDate
    (
       CASE WHEN ISNULL(CAST(s.[PublishDate] AS VARCHAR), '<<NULL>>')
                 <> ISNULL(CAST(sm.[PublishDate] AS VARCHAR), '<<NULL>>')
         THEN 'PublishDate|'
              + ISNULL(CAST(sm.[PublishDate] AS VARCHAR), 'NULL')
              + '|'
              + ISNULL(CAST(s.[PublishDate] AS VARCHAR), 'NULL')
       END
    ),

	-- 41. ApprenticeshipType
    (
       -- Normalize whitespace in Duties, Skills, or Options fields
	   CASE WHEN ISNULL(s.[ApprenticeshipType], '<<NULL>>')
                 <> ISNULL(sm.[ApprenticeshipType], '<<NULL>>')
         THEN 'ApprenticeshipType|'
              + ISNULL(sm.[ApprenticeshipType], 'NULL')
              + '|'
              + ISNULL(s.[ApprenticeshipType], 'NULL')
       END
    ),

	-- 42. RelatedOccupations
    (
       -- Normalize whitespace in Duties, Skills, or Options fields
	   CASE WHEN ISNULL(s.[RelatedOccupations], '<<NULL>>')
                 <> ISNULL(sm.[RelatedOccupations], '<<NULL>>')
         THEN 'RelatedOccupations|'
              + ISNULL(sm.[RelatedOccupations], 'NULL')
              + '|'
              + ISNULL(s.[RelatedOccupations], 'NULL')
       END
    )

) AS dif(ChangeDescription)

WHERE
    -- Return only if at least one column differs
    (
        ISNULL(s.[IfateReferenceNumber], '<<NULL>>')
            <> ISNULL(sm.[IfateReferenceNumber], '<<NULL>>')
        OR ISNULL(s.[LarsCode], -999999)
            <> ISNULL(sm.[LarsCode], -999999)
        OR ISNULL(s.[Status], '<<NULL>>')
            <> ISNULL(sm.[Status], '<<NULL>>')
        OR ISNULL(s.[VersionEarliestStartDate], '19000101')
            <> ISNULL(sm.[VersionEarliestStartDate], '19000101')
        OR ISNULL(s.[VersionLatestStartDate], '19000101')
            <> ISNULL(sm.[VersionLatestStartDate], '19000101')
        OR ISNULL(s.[VersionLatestEndDate], '19000101')
            <> ISNULL(sm.[VersionLatestEndDate], '19000101')
        OR ISNULL(s.[Title], '<<NULL>>')
            <> ISNULL(sm.[Title], '<<NULL>>')
        OR ISNULL(s.[Level], -999999)
            <> ISNULL(sm.[Level], -999999)
        OR ISNULL(s.[ProposedTypicalDuration], -999999)
            <> ISNULL(sm.[ProposedTypicalDuration], -999999)
        OR ISNULL(s.[ProposedMaxFunding], -999999)
            <> ISNULL(sm.[ProposedMaxFunding], -999999)
        OR ISNULL(s.[IntegratedDegree], '<<NULL>>')
            <> ISNULL(sm.[IntegratedDegree], '<<NULL>>')
        OR ISNULL(s.[OverviewOfRole], '<<NULL>>')
            <> ISNULL(sm.[OverviewOfRole], '<<NULL>>')
        OR ISNULL(s.[RouteCode], -999999)
            <> ISNULL(sm.[RouteCode], -999999)
        OR ISNULL(s.[AssessmentPlanUrl], '<<NULL>>')
            <> ISNULL(sm.[AssessmentPlanUrl], '<<NULL>>')
        OR ISNULL(s.[ApprovedForDelivery], '19000101')
            <> ISNULL(sm.[ApprovedForDelivery], '19000101')
        OR ISNULL(s.[TrailBlazerContact], '<<NULL>>')
            <> ISNULL(sm.[TrailBlazerContact], '<<NULL>>')
        OR ISNULL(s.[EqaProviderName], '<<NULL>>')
            <> ISNULL(sm.[EqaProviderName], '<<NULL>>')
        OR ISNULL(s.[EqaProviderContactName], '<<NULL>>')
            <> ISNULL(sm.[EqaProviderContactName], '<<NULL>>')
        OR ISNULL(s.[EqaProviderContactEmail], '<<NULL>>')
            <> ISNULL(sm.[EqaProviderContactEmail], '<<NULL>>')
        OR ISNULL(s.[EqaProviderWebLink], '<<NULL>>')
            <> ISNULL(sm.[EqaProviderWebLink], '<<NULL>>')
        OR ISNULL(s.[Keywords], '<<NULL>>')
            <> ISNULL(sm.[Keywords], '<<NULL>>')
        OR ISNULL(s.[TypicalJobTitles], '<<NULL>>')
            <> ISNULL(sm.[TypicalJobTitles], '<<NULL>>')
        OR ISNULL(s.[StandardPageUrl], '<<NULL>>')
            <> ISNULL(sm.[StandardPageUrl], '<<NULL>>')
        OR ISNULL(s.[Version], '<<NULL>>')
            <> ISNULL(sm.[Version], '<<NULL>>')
        OR ISNULL(s.[RegulatedBody], '<<NULL>>')
            <> ISNULL(sm.[RegulatedBody], '<<NULL>>')
        OR ISNULL(s.[Skills], '<<NULL>>')
            <> ISNULL(sm.[Skills], '<<NULL>>')
        OR ISNULL(s.[Knowledge], '<<NULL>>')
            <> ISNULL(sm.[Knowledge], '<<NULL>>')
        OR ISNULL(s.[Behaviours], '<<NULL>>')
            <> ISNULL(sm.[Behaviours], '<<NULL>>')
        OR ISNULL(s.[Duties], '<<NULL>>')
            <> ISNULL(sm.[Duties], '<<NULL>>')
        OR ISNULL(s.[CoreDuties], '<<NULL>>')
            <> ISNULL(sm.[CoreDuties], '<<NULL>>')
        OR ISNULL(s.[CoreAndOptions], 0)
            <> ISNULL(sm.[CoreAndOptions], 0)
        OR ISNULL(s.[IntegratedApprenticeship], 0)
            <> ISNULL(sm.[IntegratedApprenticeship], 0)
        OR ISNULL(s.[CreatedDate], '19000101')
            <> ISNULL(sm.[CreatedDate], '19000101')
        OR ISNULL(s.[EPAChanged], 0)
            <> ISNULL(sm.[EPAChanged], 0)
        OR ISNULL(s.[VersionMajor], -999999)
            <> ISNULL(sm.[VersionMajor], -999999)
        OR ISNULL(s.[VersionMinor], -999999)
            <> ISNULL(sm.[VersionMinor], -999999)
        OR ISNULL(s.[Options], '<<NULL>>')
            <> ISNULL(sm.[Options], '<<NULL>>')
        OR ISNULL(s.[CoronationEmblem], 0)
            <> ISNULL(sm.[CoronationEmblem], 0)
        OR ISNULL(s.[EpaoMustBeApprovedByRegulatorBody], 0)
            <> ISNULL(sm.[EpaoMustBeApprovedByRegulatorBody], 0)
		OR ISNULL(s.[PublishDate], 0)
            <> ISNULL(sm.[PublishDate], 0)
		OR ISNULL(s.[ApprenticeshipType], '<<NULL>>')
            <> ISNULL(sm.[ApprenticeshipType], '<<NULL>>')
		OR ISNULL(s.[RelatedOccupations], '<<NULL>>')
            <> ISNULL(sm.[RelatedOccupations], '<<NULL>>')
    )
GROUP BY
    s.[StandardUId],
    s.[IfateReferenceNumber],
    s.[LarsCode],
    s.[Status],
    s.[VersionEarliestStartDate],
    s.[VersionLatestStartDate],
    s.[VersionLatestEndDate],
    s.[Title],
    s.[Level],
    s.[ProposedTypicalDuration],
    s.[ProposedMaxFunding],
    s.[IntegratedDegree],
    s.[OverviewOfRole],
    s.[RouteCode],
    s.[AssessmentPlanUrl],
    s.[ApprovedForDelivery],
    s.[TrailBlazerContact],
    s.[EqaProviderName],
    s.[EqaProviderContactName],
    s.[EqaProviderContactEmail],
    s.[EqaProviderWebLink],
    s.[Keywords],
    s.[TypicalJobTitles],
    s.[StandardPageUrl],
    s.[Version],
    s.[RegulatedBody],
    s.[Skills],
    s.[Knowledge],
    s.[Behaviours],
    s.[Duties],
    s.[CoreDuties],
    s.[CoreAndOptions],
    s.[IntegratedApprenticeship],
	s.[CreatedDate],
    s.[EPAChanged],
    s.[VersionMajor],
    s.[VersionMinor],
    s.[Options],
    s.[CoronationEmblem],
    s.[EpaoMustBeApprovedByRegulatorBody],
	s.[PublishDate],
	s.[ApprenticeshipType],
	s.[RelatedOccupations]
) [Temp_Standard_Changes]


SELECT *
FROM #Temp_Standard_Changes
WHERE DifferenceType IN ('Removed', 'Added')
ORDER BY
    DifferenceType,
    [StandardUId];

;WITH Split AS (
    SELECT 
		[StandardUId],
		DifferenceType,
        TRY_CAST([value] AS NVARCHAR(MAX)) AS Item,
        [ordinal]
    FROM #Temp_Standard_Changes t
    CROSS APPLY STRING_SPLIT(t.[Changes], '|', 1)
),
Triples AS (
    SELECT
        [StandardUId],
		DifferenceType,
		Name = MAX(CASE WHEN (ordinal-1) % 3 = 0 THEN Item END),
        Was  = MAX(CASE WHEN (ordinal-1) % 3 = 1 THEN Item END),
        Now  = MAX(CASE WHEN (ordinal-1) % 3 = 2 THEN Item END),
        TripleGroup = (ordinal-1) / 3
    FROM Split
    GROUP BY [StandardUId], DifferenceType, (ordinal-1)/3
),
Lines AS (
    SELECT [StandardUId], DifferenceType, '  { "Name": "' + Name + '", "Was": "' + Was + '", "Now": "' + Now + '" },' [Change]
    FROM Triples
)
SELECT
	[Changes].DifferenceType,
	[Lines].[Change],
	[Changes].[StandardUId],
    [Changes].[IfateReferenceNumber],
    [Changes].[LarsCode],
    [Changes].[Status],
    [Changes].[VersionEarliestStartDate],
    [Changes].[VersionLatestStartDate],
    [Changes].[VersionLatestEndDate],
    [Changes].[Title],
    [Changes].[Level],
    [Changes].[ProposedTypicalDuration],
    [Changes].[ProposedMaxFunding],
    [Changes].[IntegratedDegree],
    [Changes].[OverviewOfRole],
    [Changes].[RouteCode],
    [Changes].[AssessmentPlanUrl],
    [Changes].[ApprovedForDelivery],
    [Changes].[TrailBlazerContact],
    [Changes].[EqaProviderName],
    [Changes].[EqaProviderContactName],
    [Changes].[EqaProviderContactEmail],
    [Changes].[EqaProviderWebLink],
    [Changes].[Keywords],
    [Changes].[TypicalJobTitles],
    [Changes].[StandardPageUrl],
    [Changes].[Version],
    [Changes].[RegulatedBody],
    [Changes].[Skills],
    [Changes].[Knowledge],
    [Changes].[Behaviours],
    [Changes].[Duties],
    [Changes].[CoreDuties],
    [Changes].[CoreAndOptions],
    [Changes].[IntegratedApprenticeship],
	[Changes].[CreatedDate],
    [Changes].[EPAChanged],
    [Changes].[VersionMajor],
    [Changes].[VersionMinor],
    [Changes].[Options],
    [Changes].[CoronationEmblem],
    [Changes].[EpaoMustBeApprovedByRegulatorBody],
	[Changes].[PublishDate],
	[Changes].[ApprenticeshipType],
	[Changes].[RelatedOccupations]
FROM 
	Lines INNER JOIN 
	#Temp_Standard_Changes [Changes] ON Lines.StandardUId = [Changes].StandardUId
ORDER BY
	[Changes].StandardUId, [Changes].DifferenceType

ROLLBACK