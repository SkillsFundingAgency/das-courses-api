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

            response.Should().BeEquivalentTo(source,options=>options
                .Excluding(c => c.ApprenticeshipFunding)
                .Excluding(c => c.LarsStandard)
                .Excluding(c => c.RouteId)
                .Excluding(c => c.Sector)
                .Excluding(c => c.SearchScore)
                .Excluding(c => c.RegulatedBody)
                .Excluding(c => c.CoreDuties)
                .Excluding(c => c.EarliestStartDate)
                .Excluding(c => c.LatestStartDate)
                .Excluding(c => c.LatestEndDate)
                .Excluding(c => c.ApprovedForDelivery)
                .Excluding(c => c.TypicalDuration)
                .Excluding(c => c.MaxFunding)
                .Excluding(c => c.EqaProviderContactEmail)
                .Excluding(c => c.EqaProviderContactName)
                .Excluding(c => c.EqaProviderName)
                .Excluding(c => c.EqaProviderWebLink)
            );

            response.Route.Should().Be(source.Sector.Route);
            response.ApprenticeshipFunding.Should().NotBeNull();
            response.StandardDates.Should().NotBeNull();
            response.VersionDetail.Should().NotBeNull();
            response.EqaProvider.Should().NotBeNull();
            response.SectorSubjectAreaTier2.Should().Be(source.LarsStandard.SectorSubjectArea.SectorSubjectAreaTier2);
            response.SectorSubjectAreaTier2Description.Should().Be(source.LarsStandard.SectorSubjectArea.Name);
            response.OtherBodyApprovalRequired.Should().Be(source.LarsStandard.OtherBodyApprovalRequired);

            response.VersionDetail.EarliestStartDate.Should().Be(source.EarliestStartDate);
            response.VersionDetail.LatestStartDate.Should().Be(source.LatestStartDate);
            response.VersionDetail.LatestEndDate.Should().Be(source.LatestEndDate);
            response.VersionDetail.ApprovedForDelivery.Should().Be(source.ApprovedForDelivery);
            response.VersionDetail.TypicalDuration.Should().Be(source.TypicalDuration);
            response.VersionDetail.MaxFunding.Should().Be(source.MaxFunding);

            response.EqaProvider.Name.Should().Be(source.EqaProviderName);
            response.EqaProvider.ContactName.Should().Be(source.EqaProviderContactName);
            response.EqaProvider.ContactEmail.Should().Be(source.EqaProviderContactEmail);
            response.EqaProvider.WebLink.Should().Be(source.EqaProviderWebLink);
        }
    }
}
