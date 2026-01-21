using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AngleSharp.Common;
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
            // Arrange – force distinct LarsCodes per standard
            for (var i = 0; i < queryResult.Standards.Count(); i++)
            {
                queryResult.Standards.GetItemByIndex(i).LarsCode = (12345 + i).ToString();
            }

            mockMediator
                .Setup(m => m.Send(
                    It.Is<GetStandardsListQuery>(q =>
                        q.Filter == StandardFilter.None &&
                        q.IncludeAllProperties),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(queryResult);

            // Act
            var result = await controller.Index() as ObjectResult;

            // Assert
            result!.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var model = (GetStandardsExportResponse)result.Value!;

            model.Standards.Should().BeEquivalentTo(
                queryResult.Standards,
                o => o
                    .ExcludingMissingMembers()
                    .Excluding(s => s.Options)
                    .Excluding(s => s.LarsCode)
            );

            // Assert LarsCode mapping explicitly
            for (var i = 0; i < model.Standards.Count(); i++)
            {
                model.Standards.GetItemByIndex(i).LarsCode
                    .Should()
                    .Be(12345 + i);
            }
        }

    }
}
