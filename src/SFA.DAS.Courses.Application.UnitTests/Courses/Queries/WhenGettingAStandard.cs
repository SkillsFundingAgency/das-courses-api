using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Application.Courses.Queries.GetStandard;
using SFA.DAS.Courses.Domain.Courses;
using SFA.DAS.Courses.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Application.UnitTests.Courses.Queries
{
    public class WhenGettingAStandard
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Standards_From_Service(
            GetStandardQuery query,
            Standard standardsFromService,
            [Frozen] Mock<IStandardsService> mockStandardsService,
            GetStandardQueryHandler handler)
        {
            mockStandardsService
                .Setup(service => service.GetStandard(query.LarsCode))
                .ReturnsAsync(standardsFromService);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Standard.Should().BeEquivalentTo(standardsFromService);
        }
    }
}