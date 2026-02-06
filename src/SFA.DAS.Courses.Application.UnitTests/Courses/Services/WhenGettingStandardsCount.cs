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
            mockRepository
                .Setup(repository => repository.Count(filter, CourseType.Apprenticeship))
                .ReturnsAsync(count);

            var actual = await service.Count(filter, CourseType.Apprenticeship);

            actual.Should().Be(count);
        }

        [Test, MoqAutoData]
        public async Task Then_Gets_Standard_Count_Unfiltered_From_Repo(
            int count,
            StandardFilter filter,
            [Frozen] Mock<IStandardRepository> mockRepository,
            StandardsService service)
        {
            mockRepository
                .Setup(repository => repository.Count(filter, null))
                .ReturnsAsync(count);

            var actual = await service.Count(filter, null);

            actual.Should().Be(count);
        }
    }
}
