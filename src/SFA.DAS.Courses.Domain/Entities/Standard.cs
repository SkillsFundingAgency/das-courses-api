﻿using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.Courses.Domain.Entities
{
    public class Standard : StandardBase
    {
        public float? SearchScore { get; set; }

        public static implicit operator Standard(StandardImport import)
        {
            return new Standard
            {
                StandardUId = import.StandardUId,
                LarsCode = import.LarsCode,
                IfateReferenceNumber = import.IfateReferenceNumber,
                Status = import.Status,
                VersionEarliestStartDate = import.VersionEarliestStartDate,
                VersionLatestStartDate = import.VersionLatestStartDate,
                VersionLatestEndDate = import.VersionLatestEndDate,
                IntegratedDegree = import.IntegratedDegree,
                Level = import.Level,
                CoronationEmblem = import.CoronationEmblem,
                ProposedTypicalDuration = import.ProposedTypicalDuration,
                ProposedMaxFunding = import.ProposedMaxFunding,
                OverviewOfRole = import.OverviewOfRole,
                StandardPageUrl = import.StandardPageUrl,
                RouteCode = import.RouteCode,
                AssessmentPlanUrl = import.AssessmentPlanUrl,
                ApprovedForDelivery = import.ApprovedForDelivery,
                TrailBlazerContact = import.TrailBlazerContact,
                EqaProviderName = import.EqaProviderName,
                EqaProviderContactName = import.EqaProviderContactName,
                EqaProviderContactEmail = import.EqaProviderContactEmail,
                EqaProviderWebLink = import.EqaProviderWebLink,
                Title = import.Title,
                Route = import.Route,
                TypicalJobTitles = import.TypicalJobTitles,
                Version = import.Version,
                Keywords = import.Keywords,
                RegulatedBody = import.RegulatedBody,
                Duties = import.Duties,
                CoreAndOptions = import.CoreAndOptions,
                CoreDuties = import.CoreDuties,
                IntegratedApprenticeship = import.IntegratedApprenticeship,
                Options = import.Options,
                EPAChanged = import.EPAChanged,
                VersionMajor = import.VersionMajor,
                VersionMinor = import.VersionMinor,
                CreatedDate = import.CreatedDate,
                EpaoMustBeApprovedByRegulatorBody = import.EpaoMustBeApprovedByRegulatorBody,
                PublishDate = import.PublishDate
            };
        }
    }
}
