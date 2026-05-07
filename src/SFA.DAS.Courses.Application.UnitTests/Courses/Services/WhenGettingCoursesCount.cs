using System.Threading.Tasks;
using AutoFixture.NUnit4;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Application.Courses.Services;
using SFA.DAS.Courses.Domain.Interfaces;
using SFA.DAS.Courses.Domain.Search;
using SFA.DAS.Testing.AutoFixture;

using CourseType = SFA.DAS.Courses.Domain.Entities.CourseType;

namespace SFA.DAS.Courses.Application.UnitTests.Courses.Services
{
    public class WhenGettingCoursesCount
    {
        [Test]
        [MoqInlineAutoData(CourseType.Apprenticeship)]
        [MoqInlineAutoData(CourseType.ShortCourse)]
        [MoqInlineAutoData(null)]
        public async Task Then_Gets_Course_Count_Filtered_From_Repo(
            CourseType? courseType,
            int count,
            StandardFilter filter,
            [Frozen] Mock<IStandardRepository> mockRepository,
            StandardsService service)
        {
            // Arrange
            mockRepository
                .Setup(repository => repository.Count(filter, courseType))
                .ReturnsAsync(count);

            // Act
            var actual = await service.CountCourses(filter, courseType);

            // Assert
            actual.Should().Be(count);
        }
    }
}
