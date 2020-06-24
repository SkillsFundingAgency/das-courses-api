using System.Collections.Generic;
using System.Linq;
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
    public class WhenGettingStandardsList
    {
        [Test, RecursiveMoqAutoData]
        public async Task Then_Gets_Standards_From_Repository(
            List<Standard> standardsFromRepo,
            [Frozen] Mock<IStandardRepository> mockStandardsRepository,
            StandardsService service)
        {
            mockStandardsRepository
                .Setup(repository => repository.GetAll())
                .ReturnsAsync(standardsFromRepo);

            var standards = (await service.GetStandardsList()).ToList();

            standards.Should().BeEquivalentTo(standardsFromRepo, options=> options
                .Excluding(c=>c.Sector)
                .Excluding(c=>c.RouteId)
            );

            foreach (var standard in standards)
            {
                standard.Route.Should().Be(standardsFromRepo.Single(c => c.Id.Equals(standard.Id)).Sector.Route);
            }
        }
    }
}
