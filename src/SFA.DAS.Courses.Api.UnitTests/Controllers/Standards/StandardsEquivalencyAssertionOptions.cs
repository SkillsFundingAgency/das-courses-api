using FluentAssertions.Equivalency;

namespace SFA.DAS.Courses.Api.UnitTests.Controllers.Standards
{
    public static class StandardsEquivalencyAssertionOptions
    {
        public static EquivalencyOptions<Domain.Courses.Standard> GetStandardResponseExclusions(EquivalencyOptions<Domain.Courses.Standard> config) => config
            .Excluding(s => s.AssessmentPlanUrl)
            .Excluding(s => s.EPAChanged)
            .Excluding(s => s.EqaProvider)
            .Excluding(s => s.Options)
            .Excluding(c => c.RelatedOccupations)
            .Excluding(s => s.TrailBlazerContact)
            .Excluding(s => s.VersionDetail)
            .Excluding(s => s.VersionMajor)
            .Excluding(s => s.VersionMinor);

        public static EquivalencyOptions<Domain.Courses.Standard> GetStandardDetailResponseExclusions(EquivalencyOptions<Domain.Courses.Standard> config) => config
            .Excluding(t => t.Options);
    }
}
