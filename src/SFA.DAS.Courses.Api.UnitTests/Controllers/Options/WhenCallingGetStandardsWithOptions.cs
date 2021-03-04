using System;
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
using SFA.DAS.Courses.Api.UnitTests.Controllers.Standards;
using SFA.DAS.Courses.Application.Courses.Queries.GetStandardsList;
using SFA.DAS.Courses.Domain.Search;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Api.UnitTests.Controllers.Options
{
    public class WhenCallingGetStandardsWithOptions
    {
        [Test, MoqAutoData]
        public async Task Then_GetStandardOptionsList(GetStandardsListResult queryResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] OptionsController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(It.Is<GetStandardsListQuery>(query =>
                    query.Filter.Equals(StandardFilter.Active)), It.IsAny<CancellationToken>()))
                .ReturnsAsync(queryResult);

            var controllerResponse = await controller.GetStandardOptionsList() as ObjectResult;

            var model = controllerResponse.Value as GetStandardOptionsListResponse;

            controllerResponse.StatusCode.Should().Be((int)HttpStatusCode.OK);

            model.StandardOptions.Should().BeEquivalentTo(queryResult.Standards.Select(standard => new GetStandardOptionsResponse
            {
                StandardUId = standard.StandardUId,
                IfateReferenceNumber = standard.IfateReferenceNumber,
                LarsCode = standard.LarsCode,
                Options = standard.Options
            }).ToList());
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Bad_Request([Frozen] Mock<IMediator> mockMediator, [Greedy] OptionsController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetStandardsListQuery>(),
                    It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

            var controllerResult = await controller.GetStandardOptionsList() as StatusCodeResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
    }
}
