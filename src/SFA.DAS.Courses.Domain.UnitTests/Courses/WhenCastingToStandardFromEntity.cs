using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Courses.Domain.Courses;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Domain.UnitTests.Courses
{
    public class WhenCastingToStandardFromEntity
    {
        [Test, RecursiveMoqAutoData]
        public void Then_Maps_Fields_Appropriately(Domain.Entities.Standard source)
        {
            // Act
            var response = (Standard)source;

            response.Should().BeEquivalentTo(source, options => options
                .ExcludingMissingMembers()

                // name matches but shape differs / mapped elsewhere
                .Excluding(s => s.ApprenticeshipFunding)
                .Excluding(s => s.Route)
                .Excluding(s => s.RouteCode)
                .Excluding(s => s.RegulatedBody)

                // mapped to StandardDates, SectorSubjectAreaTier, OtherBodyApprovalRequired and SectorCode
                .Excluding(s => s.LarsStandard)

                // mapped to VersionDetail
                .Excluding(s => s.VersionEarliestStartDate)
                .Excluding(s => s.VersionLatestStartDate)
                .Excluding(s => s.VersionLatestEndDate)
                .Excluding(s => s.ApprovedForDelivery)
                .Excluding(s => s.ProposedTypicalDuration)
                .Excluding(s => s.ProposedMaxFunding)

                // mapped to EqaProvider
                .Excluding(s => s.EqaProviderContactEmail)
                .Excluding(s => s.EqaProviderContactName)
                .Excluding(s => s.EqaProviderName)
                .Excluding(s => s.EqaProviderWebLink)

                // explicitly asserted
                .Excluding(s => s.RelatedOccupations)
            );

            response.Route.Should().Be(source.Route.Name);
            response.RouteCode.Should().Be(source.Route.Id);
            response.ApprovalBody.Should().Be(source.RegulatedBody);

            response.StandardDates.Should().NotBeNull();
            response.StandardDates.EffectiveFrom.Should().Be(source.LarsStandard.EffectiveFrom);
            response.StandardDates.EffectiveTo.Should().Be(source.LarsStandard.EffectiveTo);
            response.StandardDates.LastDateStarts.Should().Be(source.LarsStandard.LastDateStarts);

            response.VersionDetail.Should().NotBeNull();
            response.VersionDetail.EarliestStartDate.Should().Be(source.VersionEarliestStartDate);
            response.VersionDetail.LatestStartDate.Should().Be(source.VersionLatestStartDate);
            response.VersionDetail.LatestEndDate.Should().Be(source.VersionLatestEndDate);
            response.VersionDetail.ApprovedForDelivery.Should().Be(source.ApprovedForDelivery);
            response.VersionDetail.ProposedTypicalDuration.Should().Be(source.ProposedTypicalDuration);
            response.VersionDetail.ProposedMaxFunding.Should().Be(source.ProposedMaxFunding);

            response.EqaProvider.Should().NotBeNull();
            response.EqaProvider.Name.Should().Be(source.EqaProviderName);
            response.EqaProvider.ContactName.Should().Be(source.EqaProviderContactName);
            response.EqaProvider.ContactEmail.Should().Be(source.EqaProviderContactEmail);
            response.EqaProvider.WebLink.Should().Be(source.EqaProviderWebLink);

            response.ApprenticeshipStandardTypeCode.Should().Be(source.LarsStandard.ApprenticeshipStandardTypeCode);

            response.SectorSubjectAreaTier2.Should().Be(source.LarsStandard.SectorSubjectArea2.SectorSubjectAreaTier2);
            response.SectorSubjectAreaTier2Description.Should().Be(source.LarsStandard.SectorSubjectArea2.Name);
            response.SectorSubjectAreaTier1.Should().Be(source.LarsStandard.SectorSubjectArea1.SectorSubjectAreaTier1);
            response.SectorSubjectAreaTier1Description.Should().Be(source.LarsStandard.SectorSubjectArea1.SectorSubjectAreaTier1Desc);
            response.OtherBodyApprovalRequired.Should().Be(source.LarsStandard.OtherBodyApprovalRequired);
            response.SectorCode.Should().Be(source.LarsStandard.SectorCode);

            // funding mapped
            response.ApprenticeshipFunding.Should().NotBeNull();
            var expectedFunding = source.ApprenticeshipFunding?.Select(f => (ApprenticeshipFunding)f).ToList() ?? [];
            response.ApprenticeshipFunding.Should().BeEquivalentTo(expectedFunding);

            // not mapped in the operator
            response.RelatedOccupations.Should().NotBeNull();
            response.RelatedOccupations.Should().BeEmpty();
        }
    }
}
