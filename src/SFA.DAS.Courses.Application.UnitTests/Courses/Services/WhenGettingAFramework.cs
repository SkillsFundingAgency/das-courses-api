using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Application.Courses.Services;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Application.UnitTests.Courses.Services
{
    public class WhenGettingAFramework
    {
        [Test, RecursiveMoqAutoData]
        public async Task Then_Gets_A_Framework_From_The_Repository(
            string frameworkId,
            Framework frameworkFromRepo,
            [Frozen] Mock<IFrameworkRepository> mockStandardsRepository,
            FrameworksService service)
        {
            mockStandardsRepository
                .Setup(repository => repository.Get(frameworkId))
                .ReturnsAsync(frameworkFromRepo);

            var framework = await service.GetFramework(frameworkId);

            framework.Should().BeEquivalentTo(frameworkFromRepo, options=> options
                .Excluding(c=>c.FundingPeriods)
                .Excluding(c=>c.TypicalLengthFrom)
                .Excluding(c=>c.TypicalLengthTo)
                .Excluding(c=>c.TypicalLengthUnit)
            );

            

        }
    }
}