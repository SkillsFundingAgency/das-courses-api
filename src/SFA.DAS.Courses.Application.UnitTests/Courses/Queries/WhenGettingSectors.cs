using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Application.Courses.Queries.GetSectors;
using SFA.DAS.Courses.Domain.Courses;
using SFA.DAS.Courses.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Application.UnitTests.Courses.Queries
{
    public class WhenGettingSectors
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Sectors_From_Service(
            GetSectorsListQuery query,
            List<Sector> sectorsFromService,
            [Frozen] Mock<ISectorService> sectorService,
            GetSectorsListQueryHandler handler)
        {
            sectorService
                .Setup(service => service.GetSectors())
                .ReturnsAsync(sectorsFromService);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Sectors.Should().BeEquivalentTo(sectorsFromService);
        }
    }
}