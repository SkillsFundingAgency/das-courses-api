using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Application.Courses.Services;
using SFA.DAS.Courses.Domain.Interfaces;
using SFA.DAS.Courses.Domain.Search;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Application.UnitTests.Courses.Services
{
    public class WhenGettingStandardsCount
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Count_From_Repo(
            int count,
            StandardFilter filter,
            [Frozen] Mock<IStandardRepository> mockRepository,
            StandardsService service)
        {
            mockRepository
                .Setup(repository => repository.Count(filter))
                .ReturnsAsync(count);

            var actual = await service.Count(filter);

            actual.Should().Be(count);
        }
    }
}
