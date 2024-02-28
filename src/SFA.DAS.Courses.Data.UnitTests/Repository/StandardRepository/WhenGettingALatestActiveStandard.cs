using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Data.UnitTests.DatabaseMock;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Data.UnitTests.Repository.StandardRepository
{
    public class WhenGettingALatestActiveStandard
    {
        private Mock<ICoursesDataContext> _coursesDataContext;
        private List<Standard> _standards;
        private Data.Repository.StandardRepository _standardRepository;
        private const string ExpectedStandardUId = "ST002_1.2";
        private const int ExpectedLarsCode = 2;
        private const string ExpectedIFateReferenceNumber = "ST002";

        [SetUp]
        public void Arrange()
        {
            // same standard with lars code, on retired and two active
            _standards = new List<Standard>
            {
                new Standard()
                {
                    IfateReferenceNumber = "ST001",
                    StandardUId = "ST001_1.0",
                    LarsCode = 1,
                    Status = "Approved for delivery",
                    Version = "1.0",
                    VersionMajor = 1,
                    VersionMinor = 0,
                    LarsStandard = new LarsStandard
                    {
                        LarsCode = 1
                    }
                },
                new Standard
                {
                    IfateReferenceNumber = "ST002",
                    StandardUId = "ST002_1.1",
                    LarsCode = 2,
                    Status = "Approved for delivery",
                    Version = "1.1",
                    VersionMajor = 1,
                    VersionMinor = 1,
                    LarsStandard = new LarsStandard
                    {
                        LarsCode = 2
                    }
                },
                new Standard
                {
                    IfateReferenceNumber = "ST002",
                    StandardUId = ExpectedStandardUId,
                    LarsCode = 2,
                    Status = "Approved for delivery",
                    Version = "1.2",
                    VersionMajor = 1,
                    VersionMinor = 2,
                    LarsStandard = new LarsStandard
                    {
                        LarsCode = 2
                    }
                },
                new Standard
                {
                    IfateReferenceNumber = "ST002",
                    StandardUId = "ST002_1.0",
                    LarsCode = 2,
                    Status = "Retired",
                    Version = "1.0",
                    VersionMajor = 1,
                    VersionMinor = 0,
                    LarsStandard = new LarsStandard
                    {
                        LarsCode = 2
                    }
                }
            };
            
            _coursesDataContext = new Mock<ICoursesDataContext>();
            _coursesDataContext.Setup(x => x.Standards).ReturnsDbSet(_standards);

            _standardRepository = new Data.Repository.StandardRepository(_coursesDataContext.Object);
        }

        [Test]
        public async Task Then_The_Standard_Is_Returned_By_LarsCode()
        {
            //Act
            var standards = await _standardRepository.GetLatestActiveStandard(ExpectedLarsCode);
            
            //Assert
            Assert.That(standards, Is.Not.Null);
            standards.Should().BeEquivalentTo(_standards.SingleOrDefault(c=>c.StandardUId.Equals(ExpectedStandardUId)));
        }

        [Test]
        public async Task Then_The_Standard_Is_Returned_By_IFateReferenceNumber()
        {
            //Act
            var standards = await _standardRepository.GetLatestActiveStandard(ExpectedIFateReferenceNumber);

            //Assert
            Assert.That(standards, Is.Not.Null);
            standards.Should().BeEquivalentTo(_standards.SingleOrDefault(c => c.StandardUId.Equals(ExpectedStandardUId)));
        }

        [Test]
        public async Task Then_Standard_Should_Be_Null()
        {
            //Arrange
            _coursesDataContext.Setup(x => x.Standards).ReturnsDbSet(new List<Standard>());

            //Act Assert
            var result = await _standardRepository.GetLatestActiveStandard(ExpectedLarsCode);
            result.Should().BeNull();
        }
    }
}
