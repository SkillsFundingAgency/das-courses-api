using FluentAssertions.Equivalency;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Application.UnitTests.Courses.Services
{
    public static class StandardEquivalencyAssertionOptions
    {
        public static EquivalencyAssertionOptions<Standard> ExcludingFieldsWithStrictOrdering(EquivalencyAssertionOptions<Standard> config) => ExcludingFields(config).WithStrictOrdering();

        public static EquivalencyAssertionOptions<Standard> ExcludingFields (EquivalencyAssertionOptions<Standard> config) =>
            config
                .Excluding(c => c.LarsStandard)
                .Excluding(c => c.ApprenticeshipFunding)
                .Excluding(c => c.SearchScore)
                .Excluding(c => c.Sector)
                .Excluding(c => c.RouteId)
                .Excluding(c => c.RegulatedBody)
                .Excluding(c => c.CoreDuties)
                .Excluding(c => c.EarliestStartDate)
                .Excluding(c => c.LatestStartDate)
                .Excluding(c => c.LatestEndDate)
                .Excluding(c => c.ApprovedForDelivery)
                .Excluding(c => c.TypicalDuration)
                .Excluding(c => c.MaxFunding)
                .Excluding(c => c.EqaProviderContactEmail)
                .Excluding(c => c.EqaProviderContactName)
                .Excluding(c => c.EqaProviderName)
                .Excluding(c => c.EqaProviderWebLink);
    }
}
