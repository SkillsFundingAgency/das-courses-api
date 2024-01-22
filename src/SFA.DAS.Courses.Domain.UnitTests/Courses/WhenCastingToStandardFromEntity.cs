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
            var response = (Standard)source;

            response.Should().BeEquivalentTo(source, options => options
                .Excluding(c => c.ApprenticeshipFunding)
                .Excluding(c => c.LarsStandard)
                .Excluding(c => c.Route)
                .Excluding(c => c.RouteCode)
                .Excluding(c => c.SearchScore)
                .Excluding(c => c.RegulatedBody)
                .Excluding(c => c.CoreDuties)
                .Excluding(c => c.VersionEarliestStartDate)
                .Excluding(c => c.VersionLatestStartDate)
                .Excluding(c => c.VersionLatestEndDate)
                .Excluding(c => c.ApprovedForDelivery)
                .Excluding(c => c.ProposedTypicalDuration)
                .Excluding(c => c.ProposedMaxFunding)
                .Excluding(c => c.EqaProviderContactEmail)
                .Excluding(c => c.EqaProviderContactName)
                .Excluding(c => c.EqaProviderName)
                .Excluding(c => c.EqaProviderWebLink)
            );

            response.Route.Should().Be(source.Route.Name);
            response.ApprenticeshipFunding.Should().NotBeNull();
            response.StandardDates.Should().NotBeNull();
            response.VersionDetail.Should().NotBeNull();
            response.EqaProvider.Should().NotBeNull();
            response.SectorSubjectAreaTier2.Should().Be(source.LarsStandard.SectorSubjectArea2.SectorSubjectAreaTier2);
            response.SectorSubjectAreaTier2Description.Should().Be(source.LarsStandard.SectorSubjectArea2.Name);
            response.SectorSubjectAreaTier1.Should().Be(source.LarsStandard.SectorSubjectArea1.SectorSubjectAreaTier1);
            response.SectorSubjectAreaTier1Description.Should().Be(source.LarsStandard.SectorSubjectArea1.SectorSubjectAreaTier1Desc);
            response.OtherBodyApprovalRequired.Should().Be(source.LarsStandard.OtherBodyApprovalRequired);
            response.SectorCode.Should().Be(source.LarsStandard.SectorCode);

            response.VersionDetail.EarliestStartDate.Should().Be(source.VersionEarliestStartDate);
            response.VersionDetail.LatestStartDate.Should().Be(source.VersionLatestStartDate);
            response.VersionDetail.LatestEndDate.Should().Be(source.VersionLatestEndDate);
            response.VersionDetail.ApprovedForDelivery.Should().Be(source.ApprovedForDelivery);
            response.VersionDetail.ProposedTypicalDuration.Should().Be(source.ProposedTypicalDuration);
            response.VersionDetail.ProposedMaxFunding.Should().Be(source.ProposedMaxFunding);

            response.EqaProvider.Name.Should().Be(source.EqaProviderName);
            response.EqaProvider.ContactName.Should().Be(source.EqaProviderContactName);
            response.EqaProvider.ContactEmail.Should().Be(source.EqaProviderContactEmail);
            response.EqaProvider.WebLink.Should().Be(source.EqaProviderWebLink);
        }
    }
}
