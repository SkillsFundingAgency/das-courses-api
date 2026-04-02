using System;
using FluentAssertions.Equivalency;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Data.UnitTests.Repository.StandardRepository
{
    public static class EquivalencyAssertionOptionsHelper
    {
        public static Func<EquivalencyAssertionOptions<Standard>, EquivalencyAssertionOptions<Standard>> DoNotIncludeAllPropertiesExcludes()
        {
            // these are the Standard properties which are not returned when the respository is
            // not querying all entity properties in a search i.e. includeAllProperties = false
            return options => options
                .Excluding(c => c.ApprenticeshipFunding)
                .Excluding(c => c.ApprenticeshipType)
                .Excluding(c => c.AssessmentPlanUrl)
                .Excluding(c => c.CoreAndOptions)
                .Excluding(c => c.CoreDuties)
                .Excluding(c => c.CourseType)
                .Excluding(c => c.CreatedDate)
                .Excluding(c => c.Duties)
                .Excluding(c => c.DurationUnits)
                .Excluding(c => c.EPAChanged)
                .Excluding(c => c.EqaProviderContactEmail)
                .Excluding(c => c.EqaProviderContactName)
                .Excluding(c => c.EqaProviderName)
                .Excluding(c => c.EqaProviderWebLink)
                .Excluding(c => c.IsRegulatedForEPAO)
                .Excluding(c => c.IsRegulatedForProvider)
                .Excluding(c => c.LastUpdated)
                .Excluding(c => c.Options)
                .Excluding(c => c.OverviewOfRole)
                .Excluding(c => c.ProposedMaxFunding)
                .Excluding(c => c.ProposedTypicalDuration)
                .Excluding(c => c.PublishDate)
                .Excluding(c => c.RelatedOccupations)
                .Excluding(c => c.SearchScore)
                .Excluding(c => c.TrailBlazerContact);
        }
    }
}
