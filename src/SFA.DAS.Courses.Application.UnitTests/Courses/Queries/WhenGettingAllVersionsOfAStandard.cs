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
using SFA.DAS.Courses.Domain.Search;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Application.UnitTests.Courses.Queries
{
    public class WhenGettingAllVersionsOfAStandard
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Standards_From_Service(
            GetStandardsByIFateReferenceQuery query,
            List<Standard> standards,
            [Frozen] Mock<IStandardsService> mockStandardsService,
            GetStandardsByIFateReferenceQueryHandler handler)
        {
            mockStandardsService
                .Setup(service => service.GetAllVersionsOfAStandard(query.IFateReferenceNumber))
                .ReturnsAsync(standards);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Standards.Should().BeEquivalentTo(standards);
        }
    }
}
