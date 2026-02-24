using FluentAssertions.Equivalency;

namespace SFA.DAS.Courses.Api.UnitTests.Controllers.Courses
{
    public static class CoursesEquivalencyAssertionOptions
    {
        public static EquivalencyAssertionOptions<Domain.Courses.Course> GetCourseResponseExclusions(EquivalencyAssertionOptions<Domain.Courses.Course> config) => config
            .Excluding(s => s.AssessmentPlanUrl)
            .Excluding(s => s.EqaProvider)
            .Excluding(s => s.EPAChanged)
            .Excluding(s => s.Options)
            .Excluding(c => c.RelatedOccupations)
            .Excluding(s => s.TrailBlazerContact)
            .Excluding(s => s.VersionDetail)
            .Excluding(s => s.VersionMajor)
            .Excluding(s => s.VersionMinor);
    }
}
