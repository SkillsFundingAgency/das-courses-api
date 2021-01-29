using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Data.UnitTests.DatabaseMock;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Data.UnitTests.Repository.LarsStandardImportRepository
{
    public class WhenGettingAllItems
    {
        private List<LarsStandardImport> _larsStandardImport;  
        private Mock<ICoursesDataContext> _coursesDataContext;
        private Data.Repository.LarsStandardImportRepository _larsStandardImportRepository;

        [SetUp]
        public void Arrange()
        {
            _larsStandardImport = new List<LarsStandardImport>
            {
                new LarsStandardImport
                {
                    LarsCode = 1
                },
                new LarsStandardImport
                {
                    LarsCode = 2
                }
            };
            
            _coursesDataContext = new Mock<ICoursesDataContext>();
            _coursesDataContext.Setup(x => x.LarsStandardsImport).ReturnsDbSet(_larsStandardImport);
            
            _larsStandardImportRepository = new Data.Repository.LarsStandardImportRepository(_coursesDataContext.Object);
        }

        [Test]
        public async Task Then_The_LarsStandardImport_Items_Are_Returned()
        {
            //Act
            var actual = await _larsStandardImportRepository.GetAll();
            
            //Assert
            Assert.IsNotNull(actual);
            actual.Should().BeEquivalentTo(_larsStandardImport);
        }
    }
}
