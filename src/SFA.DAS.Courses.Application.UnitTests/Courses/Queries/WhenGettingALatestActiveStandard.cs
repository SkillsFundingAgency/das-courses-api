using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Application.Courses.Queries.GetStandard;
using SFA.DAS.Courses.Application.UnitTests.Customisations;
using SFA.DAS.Courses.Domain.Courses;
using SFA.DAS.Courses.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Application.UnitTests.Courses.Queries
{
    public class WhenGettingALatestActiveStandard
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Standard_From_Service_By_LarsCode(
            [GetLatestActiveStandardQueryByLarsCode] GetLatestActiveStandardQuery query,
            Standard standardFromService,
            [Frozen] Mock<IStandardsService> mockStandardsService,
            GetStandardQueryHandler handler)
        {
            query.IfateRefNumber = string.Empty;
            mockStandardsService
                .Setup(service => service.GetLatestActiveStandard(query.LarsCode))
                .ReturnsAsync(standardFromService);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Standard.Should().BeEquivalentTo(standardFromService);
        }

        [Test, MoqAutoData]
        public async Task Then_Gets_Standard_From_Service_By_IFateReferenceNumber(
            [GetLatestActiveStandardQueryByIFateReference] GetLatestActiveStandardQuery query,
            Standard standardFromService,
            [Frozen] Mock<IStandardsService> mockStandardsService,
            GetStandardQueryHandler handler)
        {
            query.LarsCode = 0;
            mockStandardsService
                .Setup(service => service.GetLatestActiveStandard(query.IfateRefNumber))
                .ReturnsAsync(standardFromService);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Standard.Should().BeEquivalentTo(standardFromService);
        }
    }
}
