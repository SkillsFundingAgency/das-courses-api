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
                .Excluding(c => c.SearchScore)
                .Excluding(c => c.ProposedTypicalDuration)
                .Excluding(c => c.ProposedMaxFunding)
                .Excluding(c => c.OverviewOfRole)
                .Excluding(c => c.AssessmentPlanUrl)
                .Excluding(c => c.TrailBlazerContact)
                .Excluding(c => c.EqaProviderName)
                .Excluding(c => c.EqaProviderContactEmail)
                .Excluding(c => c.EqaProviderContactName)
                .Excluding(c => c.EqaProviderWebLink)
                .Excluding(c => c.Duties)
                .Excluding(c => c.CoreDuties)
                .Excluding(c => c.Options)
                .Excluding(c => c.CoreAndOptions)
                .Excluding(c => c.EPAChanged)
                .Excluding(c => c.CreatedDate)
                .Excluding(c => c.PublishDate)
                .Excluding(c => c.IsRegulatedForProvider)
                .Excluding(c => c.IsRegulatedForEPAO)
                .Excluding(c => c.ApprenticeshipType)
                .Excluding(c => c.RelatedOccupations)
                .Excluding(c => c.CourseType)
                .Excluding(c => c.DurationUnits)
                .Excluding(c => c.LastUpdated);
        }
    }
}
