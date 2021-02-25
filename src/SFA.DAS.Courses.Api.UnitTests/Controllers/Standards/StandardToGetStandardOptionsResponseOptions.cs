using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions.Equivalency;

namespace SFA.DAS.Courses.Api.UnitTests.Controllers.Standards
{
    public static class StandardToGetStandardOptionsResponseOptions
    {
        public static EquivalencyAssertionOptions<Domain.Courses.Standard> Exclusions(EquivalencyAssertionOptions<Domain.Courses.Standard> config) => 
            config.Excluding(c => c.ApprenticeshipFunding)
                .Excluding(c => c.Route)
                .Excluding(c => c.SearchScore)
                .Excluding(c => c.CoreDuties)
                .Excluding(c => c.AssessmentPlanUrl)
                .Excluding(c => c.TrailBlazerContact)
                .Excluding(c => c.Status)
                .Excluding(c => c.Title)
                .Excluding(c => c.Level)
                .Excluding(c => c.Version)
                .Excluding(c => c.OverviewOfRole)
                .Excluding(c => c.Keywords)
                .Excluding(c => c.TypicalJobTitles)
                .Excluding(c => c.StandardPageUrl)
                .Excluding(c => c.IntegratedDegree)
                .Excluding(c => c.Skills)
                .Excluding(c => c.Knowledge)
                .Excluding(c => c.Behaviours)
                .Excluding(c => c.Duties)
                .Excluding(c => c.CoreAndOptions)
                .Excluding(c => c.IntegratedApprenticeship)
                .Excluding(c => c.CoreSkillsCount)
                .Excluding(c => c.VersionDetail)
                .Excluding(c => c.EqaProvider)
                .Excluding(c => c.StandardDates)
                .Excluding(c => c.SectorSubjectAreaTier2)
                .Excluding(c => c.SectorSubjectAreaTier2Description)
                .Excluding(c => c.OtherBodyApprovalRequired)
                .Excluding(c => c.ApprovalBody);
    }
}
