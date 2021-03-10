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
    public class WhenGettingALatestActiveStandard
    {
        [Test, RecursiveMoqAutoData]
        public async Task Then_Gets_A_Standard_From_The_Repository_By_LarsCode(
            int larsCode,
            Standard standardFromRepo,
            [Frozen] Mock<IStandardRepository> mockStandardsRepository,
            StandardsService service)
        {
            mockStandardsRepository
                .Setup(repository => repository.GetLatestActiveStandard(larsCode))
                .ReturnsAsync(standardFromRepo);

            var standard = await service.GetLatestActiveStandard(larsCode);

            standard.Should().BeEquivalentTo(standardFromRepo, StandardEquivalencyAssertionOptions.ExcludingFields);

            standard.Route.Should().Be(standardFromRepo.Sector.Route);
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_Gets_A_Standard_From_The_Repository_By_IFateReferenceNumber(
            string iFateReferenceNumber,
            Standard standardFromRepo,
            [Frozen] Mock<IStandardRepository> mockStandardsRepository,
            StandardsService service)
        {
            mockStandardsRepository
                .Setup(repository => repository.GetLatestActiveStandard(iFateReferenceNumber))
                .ReturnsAsync(standardFromRepo);

            var standard = await service.GetLatestActiveStandard(iFateReferenceNumber);

            standard.Should().BeEquivalentTo(standardFromRepo, StandardEquivalencyAssertionOptions.ExcludingFields);

            standard.Route.Should().Be(standardFromRepo.Sector.Route);
        }
    }
}
