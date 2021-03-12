using System;
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
using SFA.DAS.Courses.Application.Courses.Queries.GetFramework;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Api.UnitTests.Controllers.Frameworks
{
    public class WhenCallingGetFramework
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Framework_From_Mediator(
            string frameworkId,
            GetFrameworkResult queryResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] FrameworksController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetFrameworkQuery>(c=>c.FrameworkId.Equals(frameworkId)), 
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(queryResult);

            var controllerResult = await controller.Get(frameworkId) as ObjectResult;

            var model = controllerResult.Value as GetFrameworkResponse;
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            model.Should().BeEquivalentTo(queryResult.Framework);
        }

        [Test, MoqAutoData]
        public async Task And_No_Result_Then_Returns_Not_Found(
            string frameworkId,
            GetFrameworkResult queryResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] FrameworksController controller)
        {
            queryResult.Framework = null;
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetFrameworkQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(queryResult);

            var controllerResult = await controller.Get(frameworkId) as StatusCodeResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }
    }
}
