using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Application.Courses.Services;
using SFA.DAS.Courses.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Application.UnitTests.Courses.Services
{
    public class WhenGettingFrameworksList
    {
        [Test, RecursiveMoqAutoData]
        public async Task Then_Gets_Frameworks_From_Repository(
            List<Domain.Entities.Framework> frameworkEntities,
            [Frozen] Mock<IFrameworkRepository> sectorRepository,
            FrameworksService service)
        {
            sectorRepository
                .Setup(repository => repository.GetAll())
                .ReturnsAsync(frameworkEntities);

            var frameworks = (await service.GetFrameworks()).ToList();

            frameworks.Should().BeEquivalentTo(frameworkEntities, options=> options
                .Excluding(c=>c.FundingPeriods)
                .Excluding(c=>c.TypicalLengthFrom)
                .Excluding(c=>c.TypicalLengthTo)
                .Excluding(c=>c.TypicalLengthUnit)
            );

        }
    }
}