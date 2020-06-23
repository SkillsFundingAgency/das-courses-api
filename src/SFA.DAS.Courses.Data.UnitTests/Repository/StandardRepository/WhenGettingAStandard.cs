using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Data.UnitTests.DatabaseMock;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Data.UnitTests.Repository.StandardRepository
{
    public class WhenGettingAStandard
    {
        private Mock<ICoursesDataContext> _coursesDataContext;
        private List<Standard> _standards;
        private Data.Repository.StandardRepository _standardRepository;
        private const int ExpectedStandardId = 2;

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
                    Id = ExpectedStandardId
                }
            };
            
            _coursesDataContext = new Mock<ICoursesDataContext>();
            _coursesDataContext.Setup(x => x.Standards).ReturnsDbSet(_standards);

            _standardRepository = new Data.Repository.StandardRepository(_coursesDataContext.Object);
        }

        [Test]
        public async Task Then_The_Standard_Is_Returned_By_Id()
        {
            //Act
            var standards = await _standardRepository.Get(ExpectedStandardId);
            
            //Assert
            Assert.IsNotNull(standards);
            standards.Should().BeEquivalentTo(_standards.SingleOrDefault(c=>c.Id.Equals(ExpectedStandardId)));
        }
        
        [Test]
        public void Then_An_Entity_Not_Found_Exception_Is_Thrown()
        {
            //Arrange
            _coursesDataContext.Setup(x => x.Standards).ReturnsDbSet(new List<Standard>());
            
            //Act Assert
            Assert.ThrowsAsync<InvalidOperationException>(() => _standardRepository.Get(ExpectedStandardId));
        }
    }
}