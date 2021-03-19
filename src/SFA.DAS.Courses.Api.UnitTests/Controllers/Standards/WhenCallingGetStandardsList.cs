using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Api.ApiResponses;
using SFA.DAS.Courses.Api.Controllers;
using SFA.DAS.Courses.Application.Courses.Queries.GetStandardsList;
using SFA.DAS.Courses.Domain.Search;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Api.UnitTests.Controllers.Standards
{
    public class WhenCallingGetStandardsList
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Standards_List_From_Mediator(
            List<Guid> routeIds,
            List<int> levels,
            string keyword,
            OrderBy orderBy,
            StandardFilter filter,
            GetStandardsListResult queryResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] StandardsController controller)
        {
            filter = StandardFilter.None;
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetStandardsListQuery>(query => 
                        query.Keyword == keyword && 
                        query.RouteIds.Equals(routeIds) &&
                        query.Levels.Equals(levels) &&
                        query.OrderBy.Equals(orderBy) &&
                        query.Filter.Equals(filter)), 
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(queryResult);

            var controllerResult = await controller.GetList(keyword, routeIds, levels, orderBy, filter) as ObjectResult;

            var model = controllerResult.Value as GetStandardsListResponse;
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            model.Standards.Should().BeEquivalentTo(queryResult.Standards, StandardToGetStandardResponseOptions.Exclusions);
            model.Total.Should().Be(queryResult.Total);
            model.TotalFiltered.Should().Be(queryResult.TotalFiltered);
        }
    }
}
