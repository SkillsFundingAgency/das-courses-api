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
using SFA.DAS.Courses.Application.Courses.Queries.GetStandard;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Api.UnitTests.Controllers.Standards
{
    public class WhenCallingGetStandard
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Standards_List_From_Mediator(
            int standardId,
            GetStandardResult queryResult,
            [Frozen] Mock<IMediator> mockMediator,
            StandardsController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetStandardQuery>(), 
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(queryResult);

            var controllerResult = await controller.Get(standardId) as ObjectResult;

            var model = controllerResult.Value as GetStandardResponse;
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            model.Should().BeEquivalentTo(queryResult.Standard);
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Not_Found(
            int standardId,
            [Frozen] Mock<IMediator> mockMediator,
            StandardsController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetStandardQuery>(),
                    It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

            var controllerResult = await controller.Get(standardId) as StatusCodeResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }
        
    }
}