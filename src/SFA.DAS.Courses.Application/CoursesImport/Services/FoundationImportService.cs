using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Application.CoursesImport.Services;

public class FoundationImportService(
    IJsonFileHelper jsonFileHelper,
    IStandardImportRepository standardImportRepository, 
    ILarsStandardImportRepository larsStandardRepository,
    IConfiguration configuration, IApprenticeshipFundingImportRepository apprenticeshipFundingImportRepository) : IFoundationImportService
{
    public async Task ImportDataIntoStaging()
    {
        try
        {
                
            var latestFile = jsonFileHelper.GetLatestFoundationFileFromDirectory();

            if (configuration["ResourceEnvironmentName"].Equals("PROD", StringComparison.CurrentCultureIgnoreCase))
            {
                return;
            }

            var foundationsFromFile = jsonFileHelper.ParseJsonFile<StandardFoundationImport>(latestFile)
                .ToList();

            await larsStandardRepository.InsertMany(foundationsFromFile.Select(c=>new LarsStandardImport
            {
                Version = 1,
                LarsCode = c.LarsCode,
                EffectiveFrom = new DateTime(2025,03,01),
                EffectiveTo = null,
                SectorSubjectAreaTier1 = c.SectorSubjectAreaTier1,
                SectorSubjectAreaTier2 = c.SectorSubjectAreaTier2,
                LastDateStarts = null,
                SectorCode = c.SectorCode,
                OtherBodyApprovalRequired = false,
            }));
            
            await apprenticeshipFundingImportRepository.InsertMany(foundationsFromFile.Select(c=>new ApprenticeshipFundingImport
            {
                LarsCode = c.LarsCode,
                EffectiveFrom = new DateTime(2025,03,01),
                EffectiveTo = null,
                Duration = c.ApprenticeshipFunding.Duration,
                MaxEmployerLevyCap = c.ApprenticeshipFunding.MaxEmployerLevyCap
            }));

            await standardImportRepository.InsertMany(foundationsFromFile.Select(c=>new StandardImport
            {
                Version = c.Version,
                Keywords = c.Keywords,
                RegulatedBody = string.Empty,
                Duties = c.Duties,
                CoreAndOptions = c.CoreAndOptions,
                CoreDuties = c.CoreDuties,
                IntegratedApprenticeship = c.IntegratedApprenticeship,
                Options = [],
                RouteCode = c.RouteCode,
                CreatedDate = DateTime.UtcNow,
                CoronationEmblem = c.CoronationEmblem,
                ProposedTypicalDuration = 9,
                ProposedMaxFunding = 12000,
                OverviewOfRole = c.OverviewOfRole,
                StandardPageUrl = c.StandardPageUrl,
                IntegratedDegree = c.IntegratedDegree,
                Level = c.Level,
                VersionMajor = c.VersionMajor,
                VersionMinor = c.VersionMinor,
                Title = c.Title,
                TypicalJobTitles = c.TypicalJobTitles,
                AssessmentPlanUrl = c.AssessmentPlanUrl,
                ApprovedForDelivery = DateTime.UtcNow.AddDays(-1),
                TrailBlazerContact = c.TrailBlazerContact,
                EqaProviderName = "Ofqual",
                EqaProviderContactName ="",
                EqaProviderContactEmail = "",
                EqaProviderWebLink = "",
                LarsCode = c.LarsCode,
                IfateReferenceNumber = c.IfateReferenceNumber,
                Status = c.Status,
                VersionEarliestStartDate = null,
                VersionLatestStartDate = null,
                VersionLatestEndDate = null,
                IsRegulatedForProvider = c.IsRegulatedForProvider,
                IsRegulatedForEPAO = true,
                EPAChanged = false,
                StandardUId = c.StandardUId,
                ApprenticeshipType = "Foundation"
            }));

        }
        catch (Exception e)
        {
            throw;
        }
    }

}
