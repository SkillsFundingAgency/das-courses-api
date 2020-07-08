using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Application.Courses.Queries.GetFramework;
using SFA.DAS.Courses.Domain.Courses;
using SFA.DAS.Courses.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Application.UnitTests.Courses.Queries
{
    public class WhenGettingAFramework
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Framework_From_Service(
            GetFrameworkQuery query,
            Framework frameworkFromService,
            [Frozen] Mock<IFrameworksService> mockFrameworksService,
            GetFrameworkQueryHandler handler)
        {
            mockFrameworksService
                .Setup(service => service.GetFramework(query.FrameworkId))
                .ReturnsAsync(frameworkFromService);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Framework.Should().BeEquivalentTo(frameworkFromService);
        }
    }
}