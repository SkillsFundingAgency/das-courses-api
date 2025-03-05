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
using SFA.DAS.Courses.Domain.Search;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Api.UnitTests.Controllers.Import
{
    public class WhenCallingExport
    {
        [Test, MoqAutoData]
        public async Task Then_Sends_GetStandardsListQuery_To_Mediator(
            [Frozen] Mock<IMediator> mockMediator,
            GetStandardsListQueryResult queryResult,
            [Greedy] ExportController controller)
        {
            mockMediator.Setup(mediator => mediator.Send(
                It.Is<GetStandardsListQuery>(query => query.Filter.Equals(StandardFilter.None) && query.IncludeAllProperties),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(queryResult);

            var controllerResult = await controller.Index() as ObjectResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            mockMediator.Verify(x => x.Send(It.IsAny<GetStandardsListQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            var model = controllerResult.Value as GetStandardsExportResponse;
            model.Standards.Should().BeEquivalentTo(queryResult.Standards, o => o.Excluding(p => p.Options));
        }
    }
}
