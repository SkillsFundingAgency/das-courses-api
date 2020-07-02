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
    public class WhenGettingSectors
    {
        [Test, RecursiveMoqAutoData]
        public async Task Then_Gets_Sectors_From_Repository(
            List<Sector> sectorEntities,
            [Frozen] Mock<ISectorRepository> sectorRepository,
            SectorService service)
        {
            sectorRepository
                .Setup(repository => repository.GetAll())
                .ReturnsAsync(sectorEntities);

            var sectors = (await service.GetSectors()).ToList();

            sectors.Should().BeEquivalentTo(sectorEntities, options=> options
                .Excluding(c=>c.Standards)
            );

        }
    }
}