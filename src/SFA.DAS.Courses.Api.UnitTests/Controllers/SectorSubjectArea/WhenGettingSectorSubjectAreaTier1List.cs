using System.Collections.Generic;
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
using SFA.DAS.Courses.Application.SectorSubjectArea.Queries.GetAllSectorSubjectAreaTier1;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Api.UnitTests.Controllers.SectorSubjectArea1;

public class WhenGettingSectorSubjectAreaTier1List
{
    [Test, RecursiveMoqAutoData]
    public async Task Then_Gets_Ssa1_List_From_Mediator(
        List<SectorSubjectAreaTier1> sectorSubjectAreaTier1s,
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] SectorSubjectAreaController sut,
        CancellationToken cancellationToken)
    {
        mediatorMock.Setup(m => m.Send(It.IsAny<GetAllSectorSubjectAreaTier1Query>(), cancellationToken)).ReturnsAsync(sectorSubjectAreaTier1s);

        var response = await sut.GetSectorSubjectAreaTier1List(cancellationToken);

        response.As<OkObjectResult>().Value.As<GetSectorSubjectAreaTier1ListResponse>().SectorSubjectAreaTier1s.Should().BeEquivalentTo(sectorSubjectAreaTier1s, options => options.ExcludingMissingMembers());
    }
}
