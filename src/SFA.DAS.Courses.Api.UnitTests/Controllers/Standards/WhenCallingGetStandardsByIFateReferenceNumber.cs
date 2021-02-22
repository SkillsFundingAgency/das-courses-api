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
using SFA.DAS.Courses.Application.Courses.Queries.GetStandardsList;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Api.UnitTests.Controllers.Standards
{
    public class WhenCallingGetStandardsByIFateReferenceNumber
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_StandardsVersions_List_From_Mediator(
            string iFateReferenceNumber,
            GetStandardsByIFateReferenceResult queryResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] StandardsController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetStandardsByIFateReferenceQuery>(query => 
                        query.IFateReferenceNumber == iFateReferenceNumber), 
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(queryResult);

            var controllerResult = await controller.GetStandardsByIFateReferenceNumber(iFateReferenceNumber) as ObjectResult;

            var model = controllerResult.Value as GetStandardVersionsListResponse;
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            model.Standards.Should().BeEquivalentTo(queryResult.Standards, StandardToGetStandardResponseOptions.Exclusions);
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Bad_Request(
            string iFateReferenceNumber,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] StandardsController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetStandardsByIFateReferenceQuery>(),
                    It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

            var controllerResult = await controller.GetStandardsByIFateReferenceNumber(iFateReferenceNumber) as StatusCodeResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
    }
}
