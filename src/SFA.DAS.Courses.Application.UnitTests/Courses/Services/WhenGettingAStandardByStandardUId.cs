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

            var expectedEqaProvider = (Domain.Courses.EqaProvider)standardFromRepo;
            var expectedVersionDetail = (Domain.Courses.StandardVersionDetail)standardFromRepo;

            var standard = await service.GetStandard(standardUId);

            standard.Should().BeEquivalentTo(standardFromRepo, StandardEquivalencyAssertionOptions.ExcludingFields);

            standard.Route.Should().Be(standardFromRepo.Route.Name);
            standard.EqaProvider.Should().BeEquivalentTo(expectedEqaProvider);
            standard.VersionDetail.Should().BeEquivalentTo(expectedVersionDetail);
            standard.ApprenticeshipType.Should().Be(standardFromRepo.ApprenticeshipType);
        }
    }
}
