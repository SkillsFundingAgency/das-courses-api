using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Application.SectorSubjectArea.Queries.GetAllSectorSubjectAreaTier1;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Application.UnitTests.SectorSubjectArea.Queries;

public class WhenGettingSectorSubjectAreaTier1List
{
    [Test, RecursiveMoqAutoData]
    public async Task Then_Gets_All_Ssa1s_From_Repository(
        [Frozen] Mock<ISectorSubjectAreaTier1Repository> repositoryMock,
        List<SectorSubjectAreaTier1> sectorSubjectAreaTier1s,
        GetAllSectorSubjectAreaTier1Query query,
        GetAllSectorSubjectAreaTier1QueryHandler sut,
        CancellationToken cancellationToken)
    {
        repositoryMock.Setup(r => r.GetAll(cancellationToken)).ReturnsAsync(sectorSubjectAreaTier1s);

        var result = await sut.Handle(query, cancellationToken);

        result.Should().BeEquivalentTo(sectorSubjectAreaTier1s);
    }
}
