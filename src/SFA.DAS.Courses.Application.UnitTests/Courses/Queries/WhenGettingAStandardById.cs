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
    public class WhenGettingAStandardById
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Standard_From_Service_By_LarsCode(
            GetStandardByIdQuery query,
            Standard standardFromService,
            [Frozen] Mock<IStandardsService> mockStandardsService,
            GetStandardByIdQueryHandler handler)
        {
            query.Id = standardFromService.LarsCode.ToString();
            mockStandardsService
                .Setup(service => service.GetLatestActiveStandard(standardFromService.LarsCode))
                .ReturnsAsync(standardFromService);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Standard.Should().BeEquivalentTo(standardFromService);
        }

        [Test, MoqAutoData]
        public async Task Then_Gets_Standard_From_Service_By_IFateReferenceNumber(
            GetStandardByIdQuery query,
            Standard standardFromService,
            [Frozen] Mock<IStandardsService> mockStandardsService,
            GetStandardByIdQueryHandler handler)
        {
            query.Id = "ST0012";
            mockStandardsService
                .Setup(service => service.GetLatestActiveStandard(query.Id))
                .ReturnsAsync(standardFromService);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Standard.Should().BeEquivalentTo(standardFromService);
        }

        [Test, MoqAutoData]
        public async Task Then_Gets_Standard_From_Service_By_StandardUId(
            GetStandardByIdQuery query,
            Standard standardFromService,
            [Frozen] Mock<IStandardsService> mockStandardsService,
            GetStandardByIdQueryHandler handler)
        {
            query.Id = "ST0012_1.2";
            mockStandardsService
                .Setup(service => service.GetStandard(query.Id))
                .ReturnsAsync(standardFromService);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Standard.Should().BeEquivalentTo(standardFromService);
        }
    }
}
