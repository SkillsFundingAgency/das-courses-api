using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Application.Courses.Queries.GetStandardsByIFateReference;
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
            var allowableStandards = standards
                .Where(p => p.ApprenticeshipType == Domain.Entities.ApprenticeshipType.Apprenticeship || p.ApprenticeshipType == Domain.Entities.ApprenticeshipType.FoundationApprenticeship);
            
            mockStandardsService
                .Setup(service => service.GetAllVersionsOfAStandard(query.IFateReferenceNumber))
                .ReturnsAsync(allowableStandards);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Standards.Should().BeEquivalentTo(allowableStandards);
        }
    }
}
