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
using SFA.DAS.Courses.Application.Courses.Queries.GetRoutes;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Api.UnitTests.Controllers.Routes
{
    public class WhenGettingRoutes
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Routes_List_From_Mediator(
            GetRoutesQueryResult queryResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] RoutesController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetRoutesQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(queryResult);

            var controllerResult = await controller.GetList() as OkObjectResult;

            var model = controllerResult.Value as GetRoutesListResponse;
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            model.Routes.Should().BeEquivalentTo(queryResult.Routes.Where(r => r.Active));
        }
    }
}
