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

    -- Build one row per changed column, then STRING_AGG them
    STRING_AGG(dif.ChangeDescription, '; ') AS Changes
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
         THEN 'Name was: ' + ISNULL(rm.[Name], 'NULL') 
              + ' now: ' + ISNULL(r.[Name], 'NULL')
       END
    ),

    -- Compare Active
    (
       CASE WHEN ISNULL(CAST(r.[Active] AS VARCHAR), '<<NULL>>') 
                <> ISNULL(CAST(rm.[Active] AS VARCHAR), '<<NULL>>')
         THEN 'Active was: ' + ISNULL(CAST(rm.[Active] AS VARCHAR), 'NULL') 
              + ' now: ' + ISNULL(CAST(r.[Active] AS VARCHAR), 'NULL')
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

    STRING_AGG(dif.ChangeDescription, '; ') AS Changes
FROM [dbo].[Route] r
JOIN ##Route_Master rm 
    ON r.Id = rm.Id

CROSS APPLY
(
    VALUES
    (
       CASE WHEN ISNULL(r.[Name], '<<NULL>>') 
                <> ISNULL(rm.[Name], '<<NULL>>')
         THEN 'Name was: ' + ISNULL(rm.[Name], 'NULL')
              + ' now: ' + ISNULL(r.[Name], 'NULL')
       END
    ),
    (
       CASE WHEN ISNULL(CAST(r.[Active] AS VARCHAR), '<<NULL>>') 
                <> ISNULL(CAST(rm.[Active] AS VARCHAR), '<<NULL>>')
         THEN 'Active was: ' + ISNULL(CAST(rm.[Active] AS VARCHAR), 'NULL')
              + ' now: ' + ISNULL(CAST(r.[Active] AS VARCHAR), 'NULL')
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
	s.[PublishDate]
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
	sm.[PublishDate]
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
	STRING_AGG(dif.ChangeDescription, '; ') AS Changes,
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
	s.[PublishDate]

FROM [dbo].[Standard_Import] s
JOIN ##Standard_Import_Master sm 
    ON s.StandardUId = sm.StandardUId

-- CROSS APPLY: one row per column-difference
CROSS APPLY
(
    VALUES
    (CASE WHEN ISNULL(s.[IfateReferenceNumber],'<<NULL>>') 
               <> ISNULL(sm.[IfateReferenceNumber],'<<NULL>>')
          THEN 'IfateReferenceNumber was: ' + ISNULL(sm.[IfateReferenceNumber], 'NULL')
               + ' now: ' + ISNULL(s.[IfateReferenceNumber], 'NULL')
     END),
    (CASE WHEN ISNULL(CAST(s.[LarsCode] AS VARCHAR), '<<NULL>>') 
               <> ISNULL(CAST(sm.[LarsCode] AS VARCHAR), '<<NULL>>')
          THEN 'LarsCode was: ' + ISNULL(CAST(sm.[LarsCode] AS VARCHAR), 'NULL') 
               + ' now: ' + ISNULL(CAST(s.[LarsCode] AS VARCHAR), 'NULL')
     END),
    (CASE WHEN ISNULL(s.[Status], '<<NULL>>') 
               <> ISNULL(sm.[Status], '<<NULL>>')
          THEN 'Status was: ' + ISNULL(sm.[Status], 'NULL') 
               + ' now: ' + ISNULL(s.[Status], 'NULL')
     END),
    (CASE WHEN ISNULL(CAST(s.[VersionEarliestStartDate] AS VARCHAR), '<<NULL>>')
               <> ISNULL(CAST(sm.[VersionEarliestStartDate] AS VARCHAR), '<<NULL>>')
          THEN 'VersionEarliestStartDate was: ' 
               + ISNULL(CAST(sm.[VersionEarliestStartDate] AS VARCHAR), 'NULL')
               + ' now: '
               + ISNULL(CAST(s.[VersionEarliestStartDate] AS VARCHAR), 'NULL')
     END),
    (CASE WHEN ISNULL(CAST(s.[VersionLatestStartDate] AS VARCHAR), '<<NULL>>')
               <> ISNULL(CAST(sm.[VersionLatestStartDate] AS VARCHAR), '<<NULL>>')
          THEN 'VersionLatestStartDate was: ' 
               + ISNULL(CAST(sm.[VersionLatestStartDate] AS VARCHAR), 'NULL')
               + ' now: '
               + ISNULL(CAST(s.[VersionLatestStartDate] AS VARCHAR), 'NULL')
     END),
    (CASE WHEN ISNULL(CAST(s.[VersionLatestEndDate] AS VARCHAR), '<<NULL>>')
               <> ISNULL(CAST(sm.[VersionLatestEndDate] AS VARCHAR), '<<NULL>>')
          THEN 'VersionLatestEndDate was: ' 
               + ISNULL(CAST(sm.[VersionLatestEndDate] AS VARCHAR), 'NULL')
               + ' now: '
               + ISNULL(CAST(s.[VersionLatestEndDate] AS VARCHAR), 'NULL')
     END),
    (CASE WHEN ISNULL(s.[Title], '<<NULL>>') 
               <> ISNULL(sm.[Title], '<<NULL>>')
          THEN 'Title was: ' + ISNULL(sm.[Title], 'NULL')
               + ' now: ' + ISNULL(s.[Title], 'NULL')
     END),
    (CASE WHEN ISNULL(CAST(s.[Level] AS VARCHAR), '<<NULL>>')
               <> ISNULL(CAST(sm.[Level] AS VARCHAR), '<<NULL>>')
          THEN 'Level was: ' + ISNULL(CAST(sm.[Level] AS VARCHAR), 'NULL')
               + ' now: ' + ISNULL(CAST(s.[Level] AS VARCHAR), 'NULL')
     END),
    (CASE WHEN ISNULL(CAST(s.[ProposedTypicalDuration] AS VARCHAR), '<<NULL>>')
               <> ISNULL(CAST(sm.[ProposedTypicalDuration] AS VARCHAR), '<<NULL>>')
          THEN 'ProposedTypicalDuration was: '
               + ISNULL(CAST(sm.[ProposedTypicalDuration] AS VARCHAR), 'NULL')
               + ' now: '
               + ISNULL(CAST(s.[ProposedTypicalDuration] AS VARCHAR), 'NULL')
     END),
    (CASE WHEN ISNULL(CAST(s.[ProposedMaxFunding] AS VARCHAR), '<<NULL>>')
               <> ISNULL(CAST(sm.[ProposedMaxFunding] AS VARCHAR), '<<NULL>>')
          THEN 'ProposedMaxFunding was: '
               + ISNULL(CAST(sm.[ProposedMaxFunding] AS VARCHAR), 'NULL')
               + ' now: '
               + ISNULL(CAST(s.[ProposedMaxFunding] AS VARCHAR), 'NULL')
     END),
    (CASE WHEN ISNULL(s.[IntegratedDegree], '<<NULL>>')
               <> ISNULL(sm.[IntegratedDegree], '<<NULL>>')
          THEN 'IntegratedDegree was: '
               + ISNULL(sm.[IntegratedDegree], 'NULL')
               + ' now: '
               + ISNULL(s.[IntegratedDegree], 'NULL')
     END),
    (CASE WHEN ISNULL(s.[OverviewOfRole], '<<NULL>>')
               <> ISNULL(sm.[OverviewOfRole], '<<NULL>>')
          THEN 'OverviewOfRole was: '
               + ISNULL(sm.[OverviewOfRole], 'NULL')
               + ' now: '
               + ISNULL(s.[OverviewOfRole], 'NULL')
     END),
    (CASE WHEN ISNULL(CAST(s.[RouteCode] AS VARCHAR), '<<NULL>>')
               <> ISNULL(CAST(sm.[RouteCode] AS VARCHAR), '<<NULL>>')
          THEN 'RouteCode was: '
               + ISNULL(CAST(sm.[RouteCode] AS VARCHAR), 'NULL')
               + ' now: '
               + ISNULL(CAST(s.[RouteCode] AS VARCHAR), 'NULL')
     END),
    (CASE WHEN ISNULL(s.[AssessmentPlanUrl], '<<NULL>>')
               <> ISNULL(sm.[AssessmentPlanUrl], '<<NULL>>')
          THEN 'AssessmentPlanUrl was: '
               + ISNULL(sm.[AssessmentPlanUrl], 'NULL')
               + ' now: '
               + ISNULL(s.[AssessmentPlanUrl], 'NULL')
     END),
    (CASE WHEN ISNULL(CAST(s.[ApprovedForDelivery] AS VARCHAR), '<<NULL>>')
               <> ISNULL(CAST(sm.[ApprovedForDelivery] AS VARCHAR), '<<NULL>>')
          THEN 'ApprovedForDelivery was: '
               + ISNULL(CAST(sm.[ApprovedForDelivery] AS VARCHAR), 'NULL')
               + ' now: '
               + ISNULL(CAST(s.[ApprovedForDelivery] AS VARCHAR), 'NULL')
     END),
    (CASE WHEN ISNULL(s.[TrailBlazerContact], '<<NULL>>')
               <> ISNULL(sm.[TrailBlazerContact], '<<NULL>>')
          THEN 'TrailBlazerContact was: '
               + ISNULL(sm.[TrailBlazerContact], 'NULL')
               + ' now: '
               + ISNULL(s.[TrailBlazerContact], 'NULL')
     END),
    (CASE WHEN ISNULL(s.[EqaProviderName], '<<NULL>>')
               <> ISNULL(sm.[EqaProviderName], '<<NULL>>')
          THEN 'EqaProviderName was: '
               + ISNULL(sm.[EqaProviderName], 'NULL')
               + ' now: '
               + ISNULL(s.[EqaProviderName], 'NULL')
     END),
    (CASE WHEN ISNULL(s.[EqaProviderContactName], '<<NULL>>')
               <> ISNULL(sm.[EqaProviderContactName], '<<NULL>>')
          THEN 'EqaProviderContactName was: '
               + ISNULL(sm.[EqaProviderContactName], 'NULL')
               + ' now: '
               + ISNULL(s.[EqaProviderContactName], 'NULL')
     END),
    (CASE WHEN ISNULL(s.[EqaProviderContactEmail], '<<NULL>>')
               <> ISNULL(sm.[EqaProviderContactEmail], '<<NULL>>')
          THEN 'EqaProviderContactEmail was: '
               + ISNULL(sm.[EqaProviderContactEmail], 'NULL')
               + ' now: '
               + ISNULL(s.[EqaProviderContactEmail], 'NULL')
     END),
    (CASE WHEN ISNULL(s.[EqaProviderWebLink], '<<NULL>>')
               <> ISNULL(sm.[EqaProviderWebLink], '<<NULL>>')
          THEN 'EqaProviderWebLink was: '
               + ISNULL(sm.[EqaProviderWebLink], 'NULL')
               + ' now: '
               + ISNULL(s.[EqaProviderWebLink], 'NULL')
     END),
    (CASE WHEN ISNULL(s.[Keywords], '<<NULL>>')
               <> ISNULL(sm.[Keywords], '<<NULL>>')
          THEN 'Keywords was: '
               + ISNULL(sm.[Keywords], 'NULL')
               + ' now: '
               + ISNULL(s.[Keywords], 'NULL')
     END),
    (CASE WHEN ISNULL(s.[TypicalJobTitles], '<<NULL>>')
               <> ISNULL(sm.[TypicalJobTitles], '<<NULL>>')
          THEN 'TypicalJobTitles was: '
               + ISNULL(sm.[TypicalJobTitles], 'NULL')
               + ' now: '
               + ISNULL(s.[TypicalJobTitles], 'NULL')
     END),
    (CASE WHEN ISNULL(s.[StandardPageUrl], '<<NULL>>')
               <> ISNULL(sm.[StandardPageUrl], '<<NULL>>')
          THEN 'StandardPageUrl was: '
               + ISNULL(sm.[StandardPageUrl], 'NULL')
               + ' now: '
               + ISNULL(s.[StandardPageUrl], 'NULL')
     END),
    (CASE WHEN ISNULL(s.[Version], '<<NULL>>')
               <> ISNULL(sm.[Version], '<<NULL>>')
          THEN 'Version was: '
               + ISNULL(sm.[Version], 'NULL')
               + ' now: '
               + ISNULL(s.[Version], 'NULL')
     END),
    (CASE WHEN ISNULL(s.[RegulatedBody], '<<NULL>>')
               <> ISNULL(sm.[RegulatedBody], '<<NULL>>')
          THEN 'RegulatedBody was: '
               + ISNULL(sm.[RegulatedBody], 'NULL')
               + ' now: '
               + ISNULL(s.[RegulatedBody], 'NULL')
     END),
    (CASE WHEN ISNULL(s.[Skills], '<<NULL>>')
               <> ISNULL(sm.[Skills], '<<NULL>>')
          THEN 'Skills was: '
               + ISNULL(sm.[Skills], 'NULL')
               + ' now: '
               + ISNULL(s.[Skills], 'NULL')
     END),
    (CASE WHEN ISNULL(s.[Knowledge], '<<NULL>>')
               <> ISNULL(sm.[Knowledge], '<<NULL>>')
          THEN 'Knowledge was: '
               + ISNULL(sm.[Knowledge], 'NULL')
               + ' now: '
               + ISNULL(s.[Knowledge], 'NULL')
     END),
    (CASE WHEN ISNULL(s.[Behaviours], '<<NULL>>')
               <> ISNULL(sm.[Behaviours], '<<NULL>>')
          THEN 'Behaviours was: '
               + ISNULL(sm.[Behaviours], 'NULL')
               + ' now: '
               + ISNULL(s.[Behaviours], 'NULL')
     END),
    (CASE WHEN ISNULL(s.[Duties], '<<NULL>>')
               <> ISNULL(sm.[Duties], '<<NULL>>')
          THEN 'Duties was: '
               + ISNULL(sm.[Duties], 'NULL')
               + ' now: '
               + ISNULL(s.[Duties], 'NULL')
     END),
    (CASE WHEN ISNULL(s.[CoreDuties], '<<NULL>>')
               <> ISNULL(sm.[CoreDuties], '<<NULL>>')
          THEN 'CoreDuties was: '
               + ISNULL(sm.[CoreDuties], 'NULL')
               + ' now: '
               + ISNULL(s.[CoreDuties], 'NULL')
     END),
    (CASE WHEN ISNULL(CAST(s.[CoreAndOptions] AS VARCHAR), '<<NULL>>')
               <> ISNULL(CAST(sm.[CoreAndOptions] AS VARCHAR), '<<NULL>>')
          THEN 'CoreAndOptions was: '
               + ISNULL(CAST(sm.[CoreAndOptions] AS VARCHAR), 'NULL')
               + ' now: '
               + ISNULL(CAST(s.[CoreAndOptions] AS VARCHAR), 'NULL')
     END),
    (CASE WHEN ISNULL(CAST(s.[IntegratedApprenticeship] AS VARCHAR), '<<NULL>>')
               <> ISNULL(CAST(sm.[IntegratedApprenticeship] AS VARCHAR), '<<NULL>>')
          THEN 'IntegratedApprenticeship was: '
               + ISNULL(CAST(sm.[IntegratedApprenticeship] AS VARCHAR), 'NULL')
               + ' now: '
               + ISNULL(CAST(s.[IntegratedApprenticeship] AS VARCHAR), 'NULL')
     END),
    (CASE WHEN ISNULL(CAST(s.[CreatedDate] AS VARCHAR), '<<NULL>>')
               <> ISNULL(CAST(sm.[CreatedDate] AS VARCHAR), '<<NULL>>')
          THEN 'CreatedDate was: '
               + ISNULL(CAST(sm.[CreatedDate] AS VARCHAR), 'NULL')
               + ' now: '
               + ISNULL(CAST(s.[CreatedDate] AS VARCHAR), 'NULL')
     END),
    (CASE WHEN ISNULL(CAST(s.[EPAChanged] AS VARCHAR), '<<NULL>>')
               <> ISNULL(CAST(sm.[EPAChanged] AS VARCHAR), '<<NULL>>')
          THEN 'EPAChanged was: '
               + ISNULL(CAST(sm.[EPAChanged] AS VARCHAR), 'NULL')
               + ' now: '
               + ISNULL(CAST(s.[EPAChanged] AS VARCHAR), 'NULL')
     END),
    (CASE WHEN ISNULL(CAST(s.[VersionMajor] AS VARCHAR), '<<NULL>>')
               <> ISNULL(CAST(sm.[VersionMajor] AS VARCHAR), '<<NULL>>')
          THEN 'VersionMajor was: '
               + ISNULL(CAST(sm.[VersionMajor] AS VARCHAR), 'NULL')
               + ' now: '
               + ISNULL(CAST(s.[VersionMajor] AS VARCHAR), 'NULL')
     END),
    (CASE WHEN ISNULL(CAST(s.[VersionMinor] AS VARCHAR), '<<NULL>>')
               <> ISNULL(CAST(sm.[VersionMinor] AS VARCHAR), '<<NULL>>')
          THEN 'VersionMinor was: '
               + ISNULL(CAST(sm.[VersionMinor] AS VARCHAR), 'NULL')
               + ' now: '
               + ISNULL(CAST(s.[VersionMinor] AS VARCHAR), 'NULL')
     END),
    (CASE WHEN ISNULL(s.[Options], '<<NULL>>')
               <> ISNULL(sm.[Options], '<<NULL>>')
          THEN 'Options was: '
               + ISNULL(sm.[Options], 'NULL')
               + ' now: '
               + ISNULL(s.[Options], 'NULL')
     END),
    (CASE WHEN ISNULL(CAST(s.[CoronationEmblem] AS VARCHAR), '<<NULL>>')
               <> ISNULL(CAST(sm.[CoronationEmblem] AS VARCHAR), '<<NULL>>')
          THEN 'CoronationEmblem was: '
               + ISNULL(CAST(sm.[CoronationEmblem] AS VARCHAR), 'NULL')
               + ' now: '
               + ISNULL(CAST(s.[CoronationEmblem] AS VARCHAR), 'NULL')
     END),

	 (CASE WHEN ISNULL(CAST(s.[EpaoMustBeApprovedByRegulatorBody] AS VARCHAR), '<<NULL>>')
               <> ISNULL(CAST(sm.[EpaoMustBeApprovedByRegulatorBody] AS VARCHAR), '<<NULL>>')
          THEN 'EpaoMustBeApprovedByRegulatorBody was: '
               + ISNULL(CAST(sm.[EpaoMustBeApprovedByRegulatorBody] AS VARCHAR), 'NULL')
               + ' now: '
               + ISNULL(CAST(s.[EpaoMustBeApprovedByRegulatorBody] AS VARCHAR), 'NULL')
     END),

	 (CASE WHEN ISNULL(CAST(s.[PublishDate] AS VARCHAR), '<<NULL>>')
               <> ISNULL(CAST(sm.[PublishDate] AS VARCHAR), '<<NULL>>')
          THEN 'PublishDate was: '
               + ISNULL(CAST(sm.[PublishDate] AS VARCHAR), 'NULL')
               + ' now: '
               + ISNULL(CAST(s.[PublishDate] AS VARCHAR), 'NULL')
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
	s.[PublishDate]
) [Standard_Import_Changes]

SELECT * FROM #Temp_Standard_Import_Changes
ORDER BY 
    DifferenceType,
    [StandardUId];

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
	s.[PublishDate]
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
	sm.[PublishDate]
FROM ##Standard_Master sm
LEFT JOIN [dbo].[Standard] s ON sm.StandardUId = s.StandardUId
WHERE s.StandardUId IS NULL

UNION ALL

-------------------------------------------------------------------------------
-- 3) CHANGES (all columns in a single semicolon-separated string)
-------------------------------------------------------------------------------
SELECT
    'Changes' AS DifferenceType,

	-- Build one row per difference; STRING_AGG to unify them into a single column
    STRING_AGG(dif.ChangeDescription, '; ') AS Changes,

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
	s.[PublishDate]
FROM [dbo].[Standard] s
JOIN ##Standard_Master sm ON s.StandardUId = sm.StandardUId

CROSS APPLY
(
    VALUES
    -- 1. IfateReferenceNumber
    (
       CASE WHEN ISNULL(s.[IfateReferenceNumber],'<<NULL>>') 
                 <> ISNULL(sm.[IfateReferenceNumber],'<<NULL>>')
         THEN 'IfateReferenceNumber was: ' + ISNULL(sm.[IfateReferenceNumber], 'NULL')
              + ' now: ' + ISNULL(s.[IfateReferenceNumber], 'NULL')
       END
    ),

    -- 2. LarsCode
    (
       CASE WHEN ISNULL(CAST(s.[LarsCode] AS VARCHAR), '<<NULL>>')
                 <> ISNULL(CAST(sm.[LarsCode] AS VARCHAR), '<<NULL>>')
         THEN 'LarsCode was: ' + ISNULL(CAST(sm.[LarsCode] AS VARCHAR), 'NULL')
              + ' now: ' + ISNULL(CAST(s.[LarsCode] AS VARCHAR), 'NULL')
       END
    ),

    -- 3. Status
    (
       CASE WHEN ISNULL(s.[Status], '<<NULL>>') 
                 <> ISNULL(sm.[Status], '<<NULL>>')
         THEN 'Status was: ' + ISNULL(sm.[Status], 'NULL')
              + ' now: ' + ISNULL(s.[Status], 'NULL')
       END
    ),

    -- 4. VersionEarliestStartDate
    (
       CASE WHEN ISNULL(CAST(s.[VersionEarliestStartDate] AS VARCHAR), '<<NULL>>') 
                 <> ISNULL(CAST(sm.[VersionEarliestStartDate] AS VARCHAR), '<<NULL>>')
         THEN 'VersionEarliestStartDate was: '
              + ISNULL(CAST(sm.[VersionEarliestStartDate] AS VARCHAR), 'NULL')
              + ' now: '
              + ISNULL(CAST(s.[VersionEarliestStartDate] AS VARCHAR), 'NULL')
       END
    ),

    -- 5. VersionLatestStartDate
    (
       CASE WHEN ISNULL(CAST(s.[VersionLatestStartDate] AS VARCHAR), '<<NULL>>') 
                 <> ISNULL(CAST(sm.[VersionLatestStartDate] AS VARCHAR), '<<NULL>>')
         THEN 'VersionLatestStartDate was: '
              + ISNULL(CAST(sm.[VersionLatestStartDate] AS VARCHAR), 'NULL')
              + ' now: '
              + ISNULL(CAST(s.[VersionLatestStartDate] AS VARCHAR), 'NULL')
       END
    ),

    -- 6. VersionLatestEndDate
    (
       CASE WHEN ISNULL(CAST(s.[VersionLatestEndDate] AS VARCHAR), '<<NULL>>') 
                 <> ISNULL(CAST(sm.[VersionLatestEndDate] AS VARCHAR), '<<NULL>>')
         THEN 'VersionLatestEndDate was: '
              + ISNULL(CAST(sm.[VersionLatestEndDate] AS VARCHAR), 'NULL')
              + ' now: '
              + ISNULL(CAST(s.[VersionLatestEndDate] AS VARCHAR), 'NULL')
       END
    ),

    -- 7. Title
    (
       CASE WHEN ISNULL(s.[Title], '<<NULL>>') 
                 <> ISNULL(sm.[Title], '<<NULL>>')
         THEN 'Title was: ' + ISNULL(sm.[Title], 'NULL')
              + ' now: ' + ISNULL(s.[Title], 'NULL')
       END
    ),

    -- 8. Level
    (
       CASE WHEN ISNULL(CAST(s.[Level] AS VARCHAR), '<<NULL>>') 
                 <> ISNULL(CAST(sm.[Level] AS VARCHAR), '<<NULL>>')
         THEN 'Level was: ' + ISNULL(CAST(sm.[Level] AS VARCHAR), 'NULL')
              + ' now: ' + ISNULL(CAST(s.[Level] AS VARCHAR), 'NULL')
       END
    ),

    -- 9. ProposedTypicalDuration
    (
       CASE WHEN ISNULL(CAST(s.[ProposedTypicalDuration] AS VARCHAR), '<<NULL>>') 
                 <> ISNULL(CAST(sm.[ProposedTypicalDuration] AS VARCHAR), '<<NULL>>')
         THEN 'ProposedTypicalDuration was: '
              + ISNULL(CAST(sm.[ProposedTypicalDuration] AS VARCHAR), 'NULL')
              + ' now: '
              + ISNULL(CAST(s.[ProposedTypicalDuration] AS VARCHAR), 'NULL')
       END
    ),

    -- 10. ProposedMaxFunding
    (
       CASE WHEN ISNULL(CAST(s.[ProposedMaxFunding] AS VARCHAR), '<<NULL>>') 
                 <> ISNULL(CAST(sm.[ProposedMaxFunding] AS VARCHAR), '<<NULL>>')
         THEN 'ProposedMaxFunding was: '
              + ISNULL(CAST(sm.[ProposedMaxFunding] AS VARCHAR), 'NULL')
              + ' now: '
              + ISNULL(CAST(s.[ProposedMaxFunding] AS VARCHAR), 'NULL')
       END
    ),

    -- 11. IntegratedDegree
    (
       CASE WHEN ISNULL(s.[IntegratedDegree], '<<NULL>>') 
                 <> ISNULL(sm.[IntegratedDegree], '<<NULL>>')
         THEN 'IntegratedDegree was: '
              + ISNULL(sm.[IntegratedDegree], 'NULL')
              + ' now: '
              + ISNULL(s.[IntegratedDegree], 'NULL')
       END
    ),

    -- 12. OverviewOfRole
    (
       CASE WHEN ISNULL(s.[OverviewOfRole], '<<NULL>>') 
                 <> ISNULL(sm.[OverviewOfRole], '<<NULL>>')
         THEN 'OverviewOfRole was: '
              + ISNULL(sm.[OverviewOfRole], 'NULL')
              + ' now: '
              + ISNULL(s.[OverviewOfRole], 'NULL')
       END
    ),

    -- 13. RouteCode
    (
       CASE WHEN ISNULL(CAST(s.[RouteCode] AS VARCHAR), '<<NULL>>')
                 <> ISNULL(CAST(sm.[RouteCode] AS VARCHAR), '<<NULL>>')
         THEN 'RouteCode was: '
              + ISNULL(CAST(sm.[RouteCode] AS VARCHAR), 'NULL')
              + ' now: '
              + ISNULL(CAST(s.[RouteCode] AS VARCHAR), 'NULL')
       END
    ),

    -- 14. AssessmentPlanUrl
    (
       CASE WHEN ISNULL(s.[AssessmentPlanUrl], '<<NULL>>') 
                 <> ISNULL(sm.[AssessmentPlanUrl], '<<NULL>>')
         THEN 'AssessmentPlanUrl was: '
              + ISNULL(sm.[AssessmentPlanUrl], 'NULL')
              + ' now: '
              + ISNULL(s.[AssessmentPlanUrl], 'NULL')
       END
    ),

    -- 15. ApprovedForDelivery
    (
       CASE WHEN ISNULL(CAST(s.[ApprovedForDelivery] AS VARCHAR), '<<NULL>>')
                 <> ISNULL(CAST(sm.[ApprovedForDelivery] AS VARCHAR), '<<NULL>>')
         THEN 'ApprovedForDelivery was: '
              + ISNULL(CAST(sm.[ApprovedForDelivery] AS VARCHAR), 'NULL')
              + ' now: '
              + ISNULL(CAST(s.[ApprovedForDelivery] AS VARCHAR), 'NULL')
       END
    ),

    -- 16. TrailBlazerContact
    (
       CASE WHEN ISNULL(s.[TrailBlazerContact], '<<NULL>>')
                 <> ISNULL(sm.[TrailBlazerContact], '<<NULL>>')
         THEN 'TrailBlazerContact was: '
              + ISNULL(sm.[TrailBlazerContact], 'NULL')
              + ' now: '
              + ISNULL(s.[TrailBlazerContact], 'NULL')
       END
    ),

    -- 17. EqaProviderName
    (
       CASE WHEN ISNULL(s.[EqaProviderName], '<<NULL>>')
                 <> ISNULL(sm.[EqaProviderName], '<<NULL>>')
         THEN 'EqaProviderName was: '
              + ISNULL(sm.[EqaProviderName], 'NULL')
              + ' now: '
              + ISNULL(s.[EqaProviderName], 'NULL')
       END
    ),

    -- 18. EqaProviderContactName
    (
       CASE WHEN ISNULL(s.[EqaProviderContactName], '<<NULL>>')
                 <> ISNULL(sm.[EqaProviderContactName], '<<NULL>>')
         THEN 'EqaProviderContactName was: '
              + ISNULL(sm.[EqaProviderContactName], 'NULL')
              + ' now: '
              + ISNULL(s.[EqaProviderContactName], 'NULL')
       END
    ),

    -- 19. EqaProviderContactEmail
    (
       CASE WHEN ISNULL(s.[EqaProviderContactEmail], '<<NULL>>')
                 <> ISNULL(sm.[EqaProviderContactEmail], '<<NULL>>')
         THEN 'EqaProviderContactEmail was: '
              + ISNULL(sm.[EqaProviderContactEmail], 'NULL')
              + ' now: '
              + ISNULL(s.[EqaProviderContactEmail], 'NULL')
       END
    ),

    -- 20. EqaProviderWebLink
    (
       CASE WHEN ISNULL(s.[EqaProviderWebLink], '<<NULL>>')
                 <> ISNULL(sm.[EqaProviderWebLink], '<<NULL>>')
         THEN 'EqaProviderWebLink was: '
              + ISNULL(sm.[EqaProviderWebLink], 'NULL')
              + ' now: '
              + ISNULL(s.[EqaProviderWebLink], 'NULL')
       END
    ),

    -- 21. Keywords
    (
       CASE WHEN ISNULL(s.[Keywords], '<<NULL>>')
                 <> ISNULL(sm.[Keywords], '<<NULL>>')
         THEN 'Keywords was: '
              + ISNULL(sm.[Keywords], 'NULL')
              + ' now: '
              + ISNULL(s.[Keywords], 'NULL')
       END
    ),

    -- 22. TypicalJobTitles
    (
       CASE WHEN ISNULL(s.[TypicalJobTitles], '<<NULL>>')
                 <> ISNULL(sm.[TypicalJobTitles], '<<NULL>>')
         THEN 'TypicalJobTitles was: '
              + ISNULL(sm.[TypicalJobTitles], 'NULL')
              + ' now: '
              + ISNULL(s.[TypicalJobTitles], 'NULL')
       END
    ),

    -- 23. StandardPageUrl
    (
       CASE WHEN ISNULL(s.[StandardPageUrl], '<<NULL>>')
                 <> ISNULL(sm.[StandardPageUrl], '<<NULL>>')
         THEN 'StandardPageUrl was: '
              + ISNULL(sm.[StandardPageUrl], 'NULL')
              + ' now: '
              + ISNULL(s.[StandardPageUrl], 'NULL')
       END
    ),

    -- 24. Version
    (
       CASE WHEN ISNULL(s.[Version], '<<NULL>>')
                 <> ISNULL(sm.[Version], '<<NULL>>')
         THEN 'Version was: '
              + ISNULL(sm.[Version], 'NULL')
              + ' now: '
              + ISNULL(s.[Version], 'NULL')
       END
    ),

    -- 25. RegulatedBody
    (
       CASE WHEN ISNULL(s.[RegulatedBody], '<<NULL>>')
                 <> ISNULL(sm.[RegulatedBody], '<<NULL>>')
         THEN 'RegulatedBody was: '
              + ISNULL(sm.[RegulatedBody], 'NULL')
              + ' now: '
              + ISNULL(s.[RegulatedBody], 'NULL')
       END
    ),

    -- 26. Skills
    (
       CASE WHEN ISNULL(s.[Skills], '<<NULL>>')
                 <> ISNULL(sm.[Skills], '<<NULL>>')
         THEN 'Skills was: '
              + ISNULL(sm.[Skills], 'NULL')
              + ' now: '
              + ISNULL(s.[Skills], 'NULL')
       END
    ),

    -- 27. Knowledge
    (
       CASE WHEN ISNULL(s.[Knowledge], '<<NULL>>')
                 <> ISNULL(sm.[Knowledge], '<<NULL>>')
         THEN 'Knowledge was: '
              + ISNULL(sm.[Knowledge], 'NULL')
              + ' now: '
              + ISNULL(s.[Knowledge], 'NULL')
       END
    ),

    -- 28. Behaviours
    (
       CASE WHEN ISNULL(s.[Behaviours], '<<NULL>>')
                 <> ISNULL(sm.[Behaviours], '<<NULL>>')
         THEN 'Behaviours was: '
              + ISNULL(sm.[Behaviours], 'NULL')
              + ' now: '
              + ISNULL(s.[Behaviours], 'NULL')
       END
    ),

    -- 29. Duties
    (
	   CASE WHEN ISNULL(REPLACE(s.[Duties], CHAR(160), ' '), '<<NULL>>')
                 <> ISNULL(REPLACE(sm.[Duties], CHAR(160), ' '), '<<NULL>>')
         THEN 'Duties was: '
              + ISNULL(REPLACE(sm.[Duties], CHAR(160), ' '), 'NULL')
              + ' now: '
              + ISNULL(REPLACE(s.[Duties], CHAR(160), ' '), 'NULL')
       END
    ),

    -- 30. CoreDuties
    (
       CASE WHEN ISNULL(s.[CoreDuties], '<<NULL>>')
                 <> ISNULL(sm.[CoreDuties], '<<NULL>>')
         THEN 'CoreDuties was: '
              + ISNULL(sm.[CoreDuties], 'NULL')
              + ' now: '
              + ISNULL(s.[CoreDuties], 'NULL')
       END
    ),

    -- 31. CoreAndOptions
    (
       CASE WHEN ISNULL(CAST(s.[CoreAndOptions] AS VARCHAR), '<<NULL>>')
                 <> ISNULL(CAST(sm.[CoreAndOptions] AS VARCHAR), '<<NULL>>')
         THEN 'CoreAndOptions was: '
              + ISNULL(CAST(sm.[CoreAndOptions] AS VARCHAR), 'NULL')
              + ' now: '
              + ISNULL(CAST(s.[CoreAndOptions] AS VARCHAR), 'NULL')
       END
    ),

    -- 32. IntegratedApprenticeship
    (
       CASE WHEN ISNULL(CAST(s.[IntegratedApprenticeship] AS VARCHAR), '<<NULL>>')
                 <> ISNULL(CAST(sm.[IntegratedApprenticeship] AS VARCHAR), '<<NULL>>')
         THEN 'IntegratedApprenticeship was: '
              + ISNULL(CAST(sm.[IntegratedApprenticeship] AS VARCHAR), 'NULL')
              + ' now: '
              + ISNULL(CAST(s.[IntegratedApprenticeship] AS VARCHAR), 'NULL')
       END
    ),

	-- 33. CreatedDate
    (
       CASE WHEN ISNULL(CAST(s.[CreatedDate] AS VARCHAR), '<<NULL>>')
                 <> ISNULL(CAST(sm.[CreatedDate] AS VARCHAR), '<<NULL>>')
         THEN 'CreatedDate was: '
              + ISNULL(CAST(sm.[CreatedDate] AS VARCHAR), 'NULL')
              + ' now: '
              + ISNULL(CAST(s.[CreatedDate] AS VARCHAR), 'NULL')
       END
    ),

    -- 34. EPAChanged
    (
       CASE WHEN ISNULL(CAST(s.[EPAChanged] AS VARCHAR), '<<NULL>>')
                 <> ISNULL(CAST(sm.[EPAChanged] AS VARCHAR), '<<NULL>>')
         THEN 'EPAChanged was: '
              + ISNULL(CAST(sm.[EPAChanged] AS VARCHAR), 'NULL')
              + ' now: '
              + ISNULL(CAST(s.[EPAChanged] AS VARCHAR), 'NULL')
       END
    ),

    -- 35. VersionMajor
    (
       CASE WHEN ISNULL(CAST(s.[VersionMajor] AS VARCHAR), '<<NULL>>')
                 <> ISNULL(CAST(sm.[VersionMajor] AS VARCHAR), '<<NULL>>')
         THEN 'VersionMajor was: '
              + ISNULL(CAST(sm.[VersionMajor] AS VARCHAR), 'NULL')
              + ' now: '
              + ISNULL(CAST(s.[VersionMajor] AS VARCHAR), 'NULL')
       END
    ),

    -- 36. VersionMinor
    (
       CASE WHEN ISNULL(CAST(s.[VersionMinor] AS VARCHAR), '<<NULL>>')
                 <> ISNULL(CAST(sm.[VersionMinor] AS VARCHAR), '<<NULL>>')
         THEN 'VersionMinor was: '
              + ISNULL(CAST(sm.[VersionMinor] AS VARCHAR), 'NULL')
              + ' now: '
              + ISNULL(CAST(s.[VersionMinor] AS VARCHAR), 'NULL')
       END
    ),

    -- 37. Options
    (
       -- Normalize whitespace in Duties, Skills, or Options fields
	   CASE WHEN ISNULL(s.[Options], '<<NULL>>')
                 <> ISNULL(sm.[Options], '<<NULL>>')
         THEN 'Options was: '
              + ISNULL(sm.[Options], 'NULL')
              + ' now: '
              + ISNULL(s.[Options], 'NULL')
       END
    ),

    -- 38. CoronationEmblem
    (
       CASE WHEN ISNULL(CAST(s.[CoronationEmblem] AS VARCHAR), '<<NULL>>')
                 <> ISNULL(CAST(sm.[CoronationEmblem] AS VARCHAR), '<<NULL>>')
         THEN 'CoronationEmblem was: '
              + ISNULL(CAST(sm.[CoronationEmblem] AS VARCHAR), 'NULL')
              + ' now: '
              + ISNULL(CAST(s.[CoronationEmblem] AS VARCHAR), 'NULL')
       END
    ),

    -- 39. EpaoMustBeApprovedByRegulatorBody
    (
       CASE WHEN ISNULL(CAST(s.[EpaoMustBeApprovedByRegulatorBody] AS VARCHAR), '<<NULL>>')
                 <> ISNULL(CAST(sm.[EpaoMustBeApprovedByRegulatorBody] AS VARCHAR), '<<NULL>>')
         THEN 'EpaoMustBeApprovedByRegulatorBody was: '
              + ISNULL(CAST(sm.[EpaoMustBeApprovedByRegulatorBody] AS VARCHAR), 'NULL')
              + ' now: '
              + ISNULL(CAST(s.[EpaoMustBeApprovedByRegulatorBody] AS VARCHAR), 'NULL')
       END
	),

	-- 40. PublishDate
    (
       CASE WHEN ISNULL(CAST(s.[PublishDate] AS VARCHAR), '<<NULL>>')
                 <> ISNULL(CAST(sm.[PublishDate] AS VARCHAR), '<<NULL>>')
         THEN 'PublishDate was: '
              + ISNULL(CAST(sm.[PublishDate] AS VARCHAR), 'NULL')
              + ' now: '
              + ISNULL(CAST(s.[PublishDate] AS VARCHAR), 'NULL')
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
	s.[PublishDate]
) [Temp_Standard_Changes]

SELECT * FROM #Temp_Standard_Changes
ORDER BY
    DifferenceType,
    [StandardUId];
GO
