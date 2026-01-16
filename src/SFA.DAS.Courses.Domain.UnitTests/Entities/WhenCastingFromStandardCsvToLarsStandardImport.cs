using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.ImportTypes;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Domain.UnitTests.Entities
{
    public class WhenCastingFromStandardCsvToLarsStandardImport
    {
        [Test]
        [MoqInlineAutoData("y", true)]
        [MoqInlineAutoData("Y", true)]
        [MoqInlineAutoData("n", false)]
        [MoqInlineAutoData(null, false)]
        [MoqInlineAutoData("false", false)]
        public void Then_The_Fields_Are_Mapped_Correctly(
            string approvalRequired,
            bool expected,
            StandardCsv standardCsv)
        {
            standardCsv.OtherBodyApprovalRequired = approvalRequired;
            var actual = (LarsStandardImport)standardCsv;

            actual.Should().BeEquivalentTo(standardCsv, options => options
                .Excluding(c => c.StandardCode)
                .Excluding(c => c.OtherBodyApprovalRequired)
                .Excluding(c => c.StandardSectorCode)
                .Excluding(c => c.SectorSubjectAreaTier1)
            );
            actual.LarsCode.Should().Be(standardCsv.StandardCode);
            actual.SectorCode.Should().Be(standardCsv.StandardSectorCode);
            actual.OtherBodyApprovalRequired.Should()
                .Be(expected);
        }

        [Test, AutoData]
        public void Then_The_SSA1_Is_Transformed_Correctly(StandardCsv standardCsv)
        {
            standardCsv.SectorSubjectAreaTier1 = "6.00";

            LarsStandardImport actual = standardCsv;

            actual.SectorSubjectAreaTier1.Should().Be(6);
        }
    }
}
