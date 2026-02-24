using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Domain.UnitTests.Courses
{
    public class WhenCastingToCourseApprenticeshipFundingFromEntity
    {
        [Test, RecursiveMoqAutoData]
        public void Then_The_Fields_Are_Mapped_Correctly(ApprenticeshipFunding apprenticeshipFunding)
        {
            var actual = (Domain.Courses.CourseApprenticeshipFunding)apprenticeshipFunding;

            actual.Should().BeEquivalentTo(apprenticeshipFunding, options => options
                .ExcludingMissingMembers());
        }
    }
}
