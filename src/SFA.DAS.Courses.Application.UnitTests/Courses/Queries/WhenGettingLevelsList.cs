using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Application.Courses.Queries.GetLevels;
using SFA.DAS.Courses.Domain.Courses;
using SFA.DAS.Courses.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Application.UnitTests.Courses.Queries
{
    public class WhenGettingLevelsList
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Sectors_From_Service(
            GetLevelsListQuery query,
            List<Level> levelsFromService,
            [Frozen] Mock<ILevelsService> levelsService,
            GetLevelsListQueryHandler handler)
        {
            levelsService
                .Setup(service => service.GetAll())
                .Returns(levelsFromService);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Levels.Should().BeEquivalentTo(levelsFromService);
        }
    }
}
