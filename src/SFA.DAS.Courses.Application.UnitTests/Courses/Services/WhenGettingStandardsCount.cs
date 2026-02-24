using System.Threading.Tasks;
using AutoFixture.NUnit3;
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
    public class WhenGettingStandardsCount
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Standard_Count_Filtered_From_Repo(
            int count,
            StandardFilter filter,
            [Frozen] Mock<IStandardRepository> mockRepository,
            StandardsService service)
        {
            // Arrange
            mockRepository
                .Setup(repository => repository.Count(filter, CourseType.Apprenticeship))
                .ReturnsAsync(count);

            // Act
            var actual = await service.CountStandards(filter);

            // Assert
            actual.Should().Be(count);
        }
    }
}
