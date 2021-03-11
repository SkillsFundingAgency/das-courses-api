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
    public class WhenCallingGetStandardById
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Standard_From_Mediator_With_Id(
            string Id,
            GetStandardByIdResult queryResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] StandardsController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetStandardByIdQuery>(x => x.Id == Id),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(queryResult);

            var controllerResult = await controller.Get(Id) as ObjectResult;

            var model = controllerResult.Value as GetStandardDetailResponse;
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            model.Should().BeEquivalentTo(queryResult.Standard, StandardToGetStandardResponseOptions.Exclusions);
        }

        [Test, MoqAutoData]
        public async Task And_No_Standard_Found_Then_Returns_Not_Found(
            string Id,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] StandardsController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetStandardByIdQuery>(x => x.Id == Id),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetStandardByIdResult { Standard = null });

            var controllerResult = await controller.Get(Id) as StatusCodeResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_InternalServerError(
            string Id,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] StandardsController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetStandardByIdQuery>(x => x.Id == Id),
                    It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

            var controllerResult = await controller.Get(Id) as StatusCodeResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}
