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
    public class WhenGettingAStandardByStandardUId
    {
        [Test, RecursiveMoqAutoData]
        public async Task Then_Gets_A_Standard_From_The_Repository(
            string standardUId,
            Standard standardFromRepo,
            [Frozen] Mock<IStandardRepository> mockStandardsRepository,
            StandardsService service)
        {
            mockStandardsRepository
                .Setup(repository => repository.Get(standardUId))
                .ReturnsAsync(standardFromRepo);

            var standard = await service.GetStandard(standardUId);

            standard.Should().BeEquivalentTo(standardFromRepo, options => options
                .Excluding(c => c.ApprenticeshipFunding)
                .Excluding(c => c.LarsStandard)
                .Excluding(c => c.Sector)
                .Excluding(c => c.RouteId)
                .Excluding(c => c.SearchScore)
                .Excluding(c => c.RegulatedBody)
                .Excluding(c => c.CoreDuties)
            );

            standard.Route.Should().Be(standardFromRepo.Sector.Route);
        }
    }
}
