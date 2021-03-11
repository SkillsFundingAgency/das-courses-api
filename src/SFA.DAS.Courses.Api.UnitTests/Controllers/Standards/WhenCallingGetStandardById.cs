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
        public async Task Then_Gets_Standard_From_Mediator_With_LarsCode(
            int larsCode,
            GetLatestActiveStandardResult queryResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] StandardsController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetLatestActiveStandardQuery>(x => x.LarsCode == larsCode),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(queryResult);

            var controllerResult = await controller.Get(larsCode.ToString()) as ObjectResult;

            var model = controllerResult.Value as GetStandardDetailResponse;
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            model.Should().BeEquivalentTo(queryResult.Standard, StandardToGetStandardResponseOptions.Exclusions);
        }

        [Test, MoqAutoData]
        public async Task And_No_Standard_FOund_Then_Returns_Not_Found_When_Using_LarsCode(
            int larsCode,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] StandardsController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetLatestActiveStandardQuery>(x => x.LarsCode == larsCode),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetLatestActiveStandardResult { Standard = null });

            var controllerResult = await controller.Get(larsCode.ToString()) as StatusCodeResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_InternalServerError_When_Using_LarsCode(
            int larsCode,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] StandardsController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetLatestActiveStandardQuery>(x => x.LarsCode == larsCode),
                    It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

            var controllerResult = await controller.Get(larsCode.ToString()) as StatusCodeResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }

        [Test, MoqAutoData]
        public async Task Then_Gets_Standard_From_Mediator_With_IFateReferenceNumber(
            string iFateReferenceNumber,
            GetLatestActiveStandardResult queryResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] StandardsController controller)
        {
            iFateReferenceNumber = iFateReferenceNumber.Substring(0, 6);
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetLatestActiveStandardQuery>(x => x.IfateRefNumber == iFateReferenceNumber),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(queryResult);

            var controllerResult = await controller.Get(iFateReferenceNumber) as ObjectResult;

            var model = controllerResult.Value as GetStandardDetailResponse;
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            model.Should().BeEquivalentTo(queryResult.Standard, StandardToGetStandardResponseOptions.Exclusions);
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Not_Found_When_Using_IFateReferenceNumber(
            string iFateReferenceNumber,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] StandardsController controller)
        {
            iFateReferenceNumber = iFateReferenceNumber.Substring(0, 6);
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetLatestActiveStandardQuery>(x => x.IfateRefNumber == iFateReferenceNumber),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetLatestActiveStandardResult { Standard = null });

            var controllerResult = await controller.Get(iFateReferenceNumber) as StatusCodeResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_InternalServerError_When_Using_IFateReferenceNumber(
            string iFateReferenceNumber,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] StandardsController controller)
        {
            iFateReferenceNumber = iFateReferenceNumber.Substring(0, 6);
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetLatestActiveStandardQuery>(x => x.IfateRefNumber == iFateReferenceNumber),
                    It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

            var controllerResult = await controller.Get(iFateReferenceNumber) as StatusCodeResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }

        [Test, MoqAutoData]
        public async Task Then_Gets_Standard_From_Mediator(
            string standardUId,
            GetStandardByStandardUIdResult queryResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] StandardsController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetStandardByStandardUIdQuery>(x => x.StandardUId == standardUId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(queryResult);

            var controllerResult = await controller.Get(standardUId) as ObjectResult;

            var model = controllerResult.Value as GetStandardDetailResponse;
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            model.Should().BeEquivalentTo(queryResult.Standard, StandardToGetStandardResponseOptions.Exclusions);
        }

        [Test, MoqAutoData]
        public async Task And_No_Standard_Found_Then_Returns_Not_Found(
            string standardUId,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] StandardsController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetStandardByStandardUIdQuery>(x => x.StandardUId == standardUId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetStandardByStandardUIdResult { Standard = null });

            var controllerResult = await controller.Get(standardUId) as StatusCodeResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_InternalServerError(
            string standardUId,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] StandardsController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetStandardByStandardUIdQuery>(x => x.StandardUId == standardUId),
                    It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

            var controllerResult = await controller.Get(standardUId) as StatusCodeResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}
