using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Data.UnitTests.DatabaseMock;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Data.UnitTests.Repository.StandardRepository
{
    public class WhenDeletingAllStandards
    {
        private Mock<ICoursesDataContext> _coursesDataContext;
        private List<Standard> _standards;
        private Data.Repository.StandardRepository _standardImportRepository;

        [SetUp]
        public void Arrange()
        {
            _standards = new List<Standard>
            {
                new Standard()
                {
                    Id = 1
                },
                new Standard
                {
                    Id = 2
                }
            };
            
            _coursesDataContext = new Mock<ICoursesDataContext>();
            _coursesDataContext.Setup(x => x.Standards).ReturnsDbSet(_standards);
            

            _standardImportRepository = new Data.Repository.StandardRepository(_coursesDataContext.Object);
        }

        [Test]
        public void Then_The_Standards_Are_Removed()
        {
            //Act
            _standardImportRepository.DeleteAll();
            
            //Assert
            _coursesDataContext.Verify(x=>x.Standards.RemoveRange(_coursesDataContext.Object.Standards), Times.Once);
            _coursesDataContext.Verify(x=>x.SaveChanges(), Times.Once);
        }
    }
}