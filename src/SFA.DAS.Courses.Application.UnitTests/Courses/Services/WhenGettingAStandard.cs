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
    public class WhenGettingAStandard
    {
        [Test, RecursiveMoqAutoData]
        public async Task Then_Gets_A_Standard_From_The_Repository(
            int standardId,
            Standard standardFromRepo,
            [Frozen] Mock<IStandardRepository> mockStandardsRepository,
            StandardsService service)
        {
            mockStandardsRepository
                .Setup(repository => repository.Get(standardId))
                .ReturnsAsync(standardFromRepo);

            var standard = await service.GetStandard(standardId);

            standard.Should().BeEquivalentTo(standardFromRepo, options=> options
                .Excluding(c=>c.ApprenticeshipFunding)
                .Excluding(c=>c.LarsStandard)
                .Excluding(c=>c.Sector)
                .Excluding(c=>c.RouteId)
                .Excluding(c=>c.SearchScore)
                .Excluding(c => c.RegulatedBody)
                .Excluding(c => c.CoreDuties)
            );

            standard.Route.Should().Be(standardFromRepo.Sector.Route);
                
        }
    }
}
