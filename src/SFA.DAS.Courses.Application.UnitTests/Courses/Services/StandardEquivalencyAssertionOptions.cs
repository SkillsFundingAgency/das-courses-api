using FluentAssertions.Equivalency;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Application.UnitTests.Courses.Services
{
    public static class StandardEquivalencyAssertionOptions
    {
        public static EquivalencyAssertionOptions<Standard> ExcludingFieldsWithStrictOrdering(EquivalencyAssertionOptions<Standard> config) => ExcludingFields(config).WithStrictOrdering();

        public static EquivalencyAssertionOptions<Standard> ExcludingFields(EquivalencyAssertionOptions<Standard> config) =>
            config
                .Excluding(c => c.LarsStandard)
                .Excluding(c => c.ApprenticeshipFunding)
                .Excluding(c => c.SearchScore)
                .Excluding(c => c.Route)
                .Excluding(c => c.RouteCode)
                .Excluding(c => c.RegulatedBody)
                .Excluding(c => c.CoreDuties)
                .Excluding(c => c.Options)
                .Excluding(c => c.VersionEarliestStartDate)
                .Excluding(c => c.VersionLatestStartDate)
                .Excluding(c => c.VersionLatestEndDate)
                .Excluding(c => c.ApprovedForDelivery)
                .Excluding(c => c.ProposedTypicalDuration)
                .Excluding(c => c.ProposedMaxFunding)
                .Excluding(c => c.EqaProviderContactEmail)
                .Excluding(c => c.EqaProviderContactName)
                .Excluding(c => c.EqaProviderName)
                .Excluding(c => c.EqaProviderWebLink)
                .Excluding(c => c.VersionMajor)
                .Excluding(c => c.VersionMinor);

    }
}
