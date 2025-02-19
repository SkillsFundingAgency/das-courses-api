using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Data.UnitTests.DatabaseMock;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Data.UnitTests.Repository.LarsStandardRepository
{
    public class WhenDeletingAllItems
    {
        private Mock<ICoursesDataContext> _coursesDataContext;
        private List<LarsStandard> _larsStandards;
        private Data.Repository.LarsStandardRepository _larsStandardRepository;

        [SetUp]
        public void Arrange()
        {
            _larsStandards = new List<LarsStandard>
            {
                new LarsStandard
                {
                    LarsCode = 1
                },
                new LarsStandard
                {
                    LarsCode = 2
                }
            };
            
            _coursesDataContext = new Mock<ICoursesDataContext>();
            _coursesDataContext.Setup(x => x.LarsStandards).ReturnsDbSet(_larsStandards);
            

            _larsStandardRepository = new Data.Repository.LarsStandardRepository(_coursesDataContext.Object);
        }

        [Test]
        public async Task Then_The_LarsStandard_Items_Are_Removed()
        {
            //Act
            await _larsStandardRepository.DeleteAll();
            
            //Assert
            _coursesDataContext.Verify(x=>x.LarsStandards.RemoveRange(_coursesDataContext.Object.LarsStandards), Times.Once);
            _coursesDataContext.Verify(x=>x.SaveChangesAsync(default), Times.Once);
        }
    }
}
