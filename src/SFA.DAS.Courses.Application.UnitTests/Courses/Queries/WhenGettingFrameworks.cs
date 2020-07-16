using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Application.Courses.Queries.GetFrameworks;
using SFA.DAS.Courses.Domain.Courses;
using SFA.DAS.Courses.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Application.UnitTests.Courses.Queries
{
    public class WhenGettingFrameworks
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Frameworks_From_Service(
            GetFrameworksQuery query,
            List<Framework> frameworkFromService,
            [Frozen] Mock<IFrameworksService> mockFrameworksService,
            GetFrameworksQueryHandler handler)
        {
            mockFrameworksService
                .Setup(service => service.GetFrameworks())
                .ReturnsAsync(frameworkFromService);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Frameworks.Should().BeEquivalentTo(frameworkFromService);
        }
    }
}