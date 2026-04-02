using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Courses.Api.ApiResponses;
using SFA.DAS.Courses.Domain.Courses;

namespace SFA.DAS.Courses.Api.UnitTests.ApiResponses.GetCourseResponseTests;

public class WhenConvertingFromCourse
{
    [Test, AutoData]
    public void ThenTransformsFromStandard(Course course)
    {
        // Act
        GetCourseResponse sut = course;

        // Assert
        sut.Should().BeEquivalentTo(
            course,
            options => options
                .ExcludingMissingMembers()
        );
    }
}
