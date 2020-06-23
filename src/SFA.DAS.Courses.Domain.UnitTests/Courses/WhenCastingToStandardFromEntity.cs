using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Courses.Domain.Courses;

namespace SFA.DAS.Courses.Domain.UnitTests.Courses
{
    public class WhenCastingToStandardFromEntity
    {
        [Test, AutoData]
        public void Then_Maps_Fields_Appropriately(
            Domain.Entities.Standard source)
        {
            var response = (Standard)source;

            response.Should().BeEquivalentTo(source, config => config.Excluding(standard => standard.SearchScore));
        }
    }
}
