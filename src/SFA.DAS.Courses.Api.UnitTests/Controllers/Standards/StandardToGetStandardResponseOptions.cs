using FluentAssertions.Equivalency;

namespace SFA.DAS.Courses.Api.UnitTests.Controllers.Standards
{
    public static class StandardToGetStandardResponseOptions
    {
        public static EquivalencyAssertionOptions<Domain.Courses.Standard> Exclusions(EquivalencyAssertionOptions<Domain.Courses.Standard> config) => config
            .Excluding(s => s.AssessmentPlanUrl)
            .Excluding(s => s.VersionDetail)
            .Excluding(s => s.EqaProvider)
            .Excluding(s => s.Options)
            .Excluding(s => s.TrailBlazerContact)
            .Excluding(s => s.EPAChanged)
            .Excluding(s => s.VersionMajor)
            .Excluding(s => s.VersionMinor);

    }
}
