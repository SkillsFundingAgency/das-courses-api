using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Domain.UnitTests.Courses
{
    public class WhenCastingToApprenticeshipFundingFromEntity
    {
        [Test, RecursiveMoqAutoData]
        public void Then_The_Fields_Are_Mapped_Correctly(ApprenticeshipFunding apprenticeshipFunding)
        {
            var actual = (Domain.Courses.ApprenticeshipFunding)apprenticeshipFunding;

            actual.Should().BeEquivalentTo(apprenticeshipFunding, options => options
                .ExcludingMissingMembers()
            );
        }
    }
}
