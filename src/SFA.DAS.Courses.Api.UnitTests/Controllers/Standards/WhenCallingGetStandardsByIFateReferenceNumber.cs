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
            // Arrange – distinct LarsCode
            var expectedLarsCodes = queryResult.Standards
                .Select((standard, index) =>
                {
                    standard.LarsCode = (1000 + index).ToString();
                    return 1000 + index;
                })
                .ToList();

            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetStandardsByIFateReferenceQuery>(q =>
                        q.IFateReferenceNumber == iFateReferenceNumber),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(queryResult);

            // Act
            var controllerResult =
                await controller.GetStandardsByIFateReferenceNumber(iFateReferenceNumber) as ObjectResult;

            // Assert
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var model = controllerResult.Value as GetStandardVersionsListResponse;

            // Assert all properties except LarsCode
            model.Standards.Should().BeEquivalentTo(
                queryResult.Standards,
                options => StandardToGetStandardResponseOptions
                    .ExclusionsForGetStandardDetailResponse(options)
                    .Excluding(s => s.LarsCode));

            // Assert LarsCode conversion with distinct values
            model.Standards
                .Select(s => s.LarsCode)
                .Should()
                .BeEquivalentTo(expectedLarsCodes, o => o.WithStrictOrdering());
        }
    }
}
