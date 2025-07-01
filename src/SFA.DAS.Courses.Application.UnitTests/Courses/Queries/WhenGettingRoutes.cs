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
        public async Task Then_Gets_Active_Routes_From_Service(
            GetRoutesQuery query,
            [Frozen] Mock<IRouteService> routeService,
            GetRoutesQueryHandler handler)
        {
            string route1Active = "route 1";
            string route2Inactive = "route 2";
            string route3Active = "route 3";

            List<Route> routesFromService = new List<Route>
            {
                new() { Id = 1, Name = route1Active, Active = true },
                new() { Id = 2, Name = route2Inactive, Active = false },
                new() { Id = 3, Name = route3Active, Active = true }
            };

            List<Route> expectedRoutes = new List<Route>
            {
                new() { Id = 1, Name = route1Active, Active = true },
                new() { Id = 3, Name = route3Active, Active = true }
            };

            routeService
                .Setup(service => service.GetRoutes())
                .ReturnsAsync(routesFromService);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Routes.Should().BeEquivalentTo(expectedRoutes);
        }
    }
}
