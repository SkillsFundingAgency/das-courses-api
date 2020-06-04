using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Application.Courses.Queries.GetStandardsList;
using SFA.DAS.Courses.Domain.Courses;
using SFA.DAS.Courses.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Application.UnitTests.Courses.Queries
{
    public class WhenGettingStandardsList
    {
        //And_Fails_Validation - maybe in the future

        [Test, MoqAutoData]
        public async Task Then_Gets_Standards_From_Service(
            GetStandardsListQuery query,
            List<Standard> standardsFromService,
            [Frozen] Mock<IStandardsService> mockStandardsService,
            GetStandardsListQueryHandler handler)
        {
            mockStandardsService
                .Setup(service => service.GetStandardsList())
                .ReturnsAsync(standardsFromService);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Standards.Should().BeEquivalentTo(standardsFromService);
        }
    }
}
