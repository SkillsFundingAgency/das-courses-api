using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Application.Courses.Queries.GetRoutes;
using SFA.DAS.Courses.Domain.Courses;
using SFA.DAS.Courses.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Application.UnitTests.Courses.Queries
{
    public class WhenGettingRoutes
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Routes_From_Service(
            GetRoutesQuery query,
            List<Route> routesFromService,
            [Frozen] Mock<IRouteService> routeService,
            GetRoutesQueryHandler handler)
        {
            routeService
                .Setup(service => service.GetRoutes())
                .ReturnsAsync(routesFromService);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Routes.Should().BeEquivalentTo(routesFromService);
        }
    }
}