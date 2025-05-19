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
    public class WhenGettingRoutes
    {
        [Test, RecursiveMoqAutoData]
        public async Task Then_Gets_Sectors_From_Repository(
            List<Route> routeEntities,
            [Frozen] Mock<IRouteRepository> routeRepository,
            RouteService service)
        {
            routeRepository
                .Setup(repository => repository.GetAll())
                .ReturnsAsync(routeEntities);

            var sectors = (await service.GetRoutes()).ToList();

            sectors.Should().BeEquivalentTo(routeEntities, options=> options
                .Excluding(c => c.Standards)
            );

        }
    }
}
