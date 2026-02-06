using System.Collections.Generic;
using System.Linq;
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
using ApprenticeshipType = SFA.DAS.Courses.Domain.Entities.ApprenticeshipType;

namespace SFA.DAS.Courses.Api.UnitTests.Controllers.Standards
{
    public class WhenCallingGetStandardsList
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Standards_List_From_Mediator(
            List<int> routeIds,
            List<int> levels,
            string keyword,
            OrderBy orderBy,
            StandardFilter filter,
            ApprenticeshipType apprenticeshipType,
            GetStandardsListQueryResult queryResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] StandardsController controller)
        {
            // Arrange – distinct LarsCode
            var expectedLarsCodes = queryResult.Standards
                .Select((standard, index) =>
                {
                    standard.LarsCode = (1000 + index).ToString();
                    return 1000 + index;
                })
                .ToList();

            filter = StandardFilter.None;
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetStandardsListQuery>(query =>
                        query.Keyword == keyword &&
                        query.RouteIds.Equals(routeIds) &&
                        query.Levels.Equals(levels) &&
                        query.OrderBy.Equals(orderBy) &&
                        query.Filter.Equals(filter) &&
                        !query.IncludeAllProperties &&
                        query.ApprenticeshipType.Equals(apprenticeshipType)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(queryResult);

            // Act
            var controllerResult = await controller.GetList(keyword, routeIds, levels, apprenticeshipType, orderBy, filter) as ObjectResult;

            // Assert
            var model = controllerResult.Value as GetStandardsListResponse;
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            model.Total.Should().Be(queryResult.Total);
            model.TotalFiltered.Should().Be(queryResult.TotalFiltered);

            // Assert all properties except LarsCode
            model.Standards.Should().BeEquivalentTo(
                queryResult.Standards,
                options => StandardToGetStandardResponseOptions
                    .ExclusionsGetStandardResponse(options)
                    .Excluding(s => s.LarsCode));

            // Assert LarsCode conversion with distinct values
            model.Standards
                .Select(s => s.LarsCode)
                .Should()
                .BeEquivalentTo(expectedLarsCodes, o => o.WithStrictOrdering());
        }
    }
}
