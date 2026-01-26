using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Courses.Data.Extensions;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.Search;

namespace SFA.DAS.Courses.Data.UnitTests.Extensions
{
    public class WhenFilteringIsLatestVersion
    {
        private List<Standard> _standards;

        [SetUp]
        public void Arrange()
        {
            _standards = GetStandards();
        }

        [Test]
        public void Then_ReturnOneVersionOfEachStandard()
        {
            var filteredStandards = _standards.InMemoryFilterIsLatestVersion(StandardFilter.Active);

            filteredStandards.Count().Should().Be(2);
        }

        [TestCase(false, false, "1.1")]
        [TestCase(true, false, "1.11")]
        [TestCase(false, true, "2.0")]
        [TestCase(true, true, "2.0")]
        public void And_NewerVersionAreAdded_Then_ReturnLatestStandardVersion(bool includeVersion1_11, bool includeVersion2_0, string expectedResultVersion)
        {
            AddVersion1_11(includeVersion1_11);
            AddVersion2_0(includeVersion2_0);

            var filteredStandards = _standards.InMemoryFilterIsLatestVersion(StandardFilter.Active);

            var resultStandard = filteredStandards.FirstOrDefault(standard => standard.LarsCode == "1");

            Assert.That(resultStandard.Version, Is.EqualTo(expectedResultVersion));
        }

        private List<Standard> GetStandards()
        {
            return new List<Standard>
            {
                new Standard
                {
                    Title = "Apprenticeship One v1.0",
                    LarsCode = "1",
                    Status = "Retired",
                    Version = "1.0",
                    VersionMajor = 1,
                    VersionMinor = 0,
                },
                new Standard
                {
                    Title = "Apprenticeship One v1.1",
                    LarsCode = "1",
                    Status = "Active",
                    Version = "1.1",
                    VersionMajor = 1,
                    VersionMinor = 1,
                },
                new Standard
                {
                    Title = "Apprenticeship Two v1.0",
                    LarsCode = "2",
                    Status = "Active",
                    Version = "1.0",
                    VersionMajor = 1,
                    VersionMinor = 0,
                }
            };
        }

        private void AddVersion1_11(bool addVersion)
        {
            if (addVersion)
            {
                _standards.Add(new Standard
                {
                    Title = "Apprenticeship v1.11",
                    LarsCode = "1",
                    Status = "Active",
                    Version = "1.11",
                    VersionMajor = 1,
                    VersionMinor = 11,
                });
            }
        }

        private void AddVersion2_0(bool addVersion)
        {
            if (addVersion)
            {
                _standards.Add(new Standard
                {
                    Title = "Apprenticeship v2.0",
                    LarsCode = "1",
                    Status = "Active",
                    Version = "2.0",
                    VersionMajor = 2,
                    VersionMinor = 0,
                });
            }
        }
    }
}
