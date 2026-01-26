using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Application.Courses.Services;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.Search;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Application.UnitTests.Courses.Services
{
    public class WhenSortingStandardsList
    {
        [Test, RecursiveMoqAutoData]
        public void And_OrderBy_Score_Then_Ordered_By_Score(
            string keyword,
            StandardsSortOrderService standardsSortOrderService)
        {
            //Arrange
            var orderBy = OrderBy.Score;
            var standards = new List<Standard>
            {
                new Standard{LarsCode = "1", SearchScore = 2, LarsStandard = new LarsStandard()},
                new Standard{LarsCode = "2", SearchScore = 1, LarsStandard = new LarsStandard()},
                new Standard{LarsCode = "3", SearchScore = 3, LarsStandard = new LarsStandard()}
            };                                      
            //Act                                   
            var actual = standardsSortOrderService.OrderBy(standards, orderBy, keyword);

            //Assert
            Assert.That(actual.ElementAt(0).LarsCode, Is.EqualTo("3"));
            Assert.That(actual.ElementAt(1).LarsCode, Is.EqualTo("1"));
            Assert.That(actual.ElementAt(2).LarsCode, Is.EqualTo("2"));
        }

        [Test, RecursiveMoqAutoData]
        public void And_OrderBy_Score_Then_Ordered_By_Score_Then_By_Title(
            string keyword,
            StandardsSortOrderService standardsSortOrderService)
        {
            //Arrange
            var orderBy = OrderBy.Score;
            var standards = new List<Standard>
            {
                new Standard{LarsCode = "1", SearchScore = 1, Title = "aardvark", LarsStandard = new LarsStandard()},
                new Standard{LarsCode = "2", SearchScore = 3, Title = "aardvark", LarsStandard = new LarsStandard()},
                new Standard{LarsCode = "3", SearchScore = 2, Title = "aardvark", LarsStandard = new LarsStandard()}
            };

            //Act
            var actual = standardsSortOrderService.OrderBy(standards, orderBy, keyword);

            //Assert
            Assert.That(actual.ElementAt(0).LarsCode, Is.EqualTo("2"));
            Assert.That(actual.ElementAt(1).LarsCode, Is.EqualTo("3"));
            Assert.That(actual.ElementAt(2).LarsCode, Is.EqualTo("1"));
        }
        
        [Test, RecursiveMoqAutoData]
        public void And_OrderBy_Score_Then_Ordered_By_Score_Then_By_Title_Then_By_Level(
            string keyword,
            StandardsSortOrderService standardsSortOrderService)
        {
            //Arrange
            var orderBy = OrderBy.Score;
            var standards = new List<Standard>
            {
                new Standard{LarsCode = "1", SearchScore = 1, Title = "aardvark", Level = 3, LarsStandard = new LarsStandard()},
                new Standard{LarsCode = "2", SearchScore = 1, Title = "aardvark", Level = 1, LarsStandard = new LarsStandard()},
                new Standard{LarsCode = "3", SearchScore = 1, Title = "aardvark", Level = 2, LarsStandard = new LarsStandard()}
            };

            //Act
            var actual = standardsSortOrderService.OrderBy(standards, orderBy, keyword);

            //Assert
            Assert.That(actual.ElementAt(0).LarsCode, Is.EqualTo("2"));
            Assert.That(actual.ElementAt(1).LarsCode, Is.EqualTo("3"));
            Assert.That(actual.ElementAt(2).LarsCode, Is.EqualTo("1"));
        }

        [Test, RecursiveMoqAutoData]
        public void And_OrderBy_Score_Then_Ordered_By_Score_Then_By_Title_Then_By_Level_Then_By_LarsStandard(
            string keyword,
            StandardsSortOrderService standardsSortOrderService)
        {
            //Arrange
            var orderBy = OrderBy.Score;
            var standards = new List<Standard>
            {
                new Standard{LarsCode = "1", SearchScore = 1, Title = "aardvark", Level = 3, LarsStandard = new LarsStandard{LarsCode = "3"}},
                new Standard{LarsCode = "2", SearchScore = 1, Title = "aardvark", Level = 3, LarsStandard = new LarsStandard{LarsCode = "1"}},
                new Standard{LarsCode = "3", SearchScore = 1, Title = "aardvark", Level = 3, LarsStandard = new LarsStandard{LarsCode = "2"}}
            };

            //Act
            var actual = standardsSortOrderService.OrderBy(standards, orderBy, keyword);

            //Assert
            Assert.That(actual.ElementAt(0).LarsCode, Is.EqualTo("2"));
            Assert.That(actual.ElementAt(1).LarsCode, Is.EqualTo("3"));
            Assert.That(actual.ElementAt(2).LarsCode, Is.EqualTo("1"));
        }

        [Test, RecursiveMoqAutoData]
        public void And_OrderBy_Title_Then_Ordered_By_Title(
            StandardsSortOrderService standardsSortOrderService)
        {
            //Arrange
            var orderBy = OrderBy.Title;
            var standards = new List<Standard>
            {
                new Standard{LarsCode = "1", Title = "bear", LarsStandard = new LarsStandard()},
                new Standard{LarsCode = "2", Title = "chimp", LarsStandard = new LarsStandard()},
                new Standard{LarsCode = "3", Title = "aardvark", LarsStandard = new LarsStandard()}
            };

            //Act
            var actual = standardsSortOrderService.OrderBy(standards, orderBy, null);

            //Assert
            Assert.That(actual.ElementAt(0).LarsCode, Is.EqualTo("3"));
            Assert.That(actual.ElementAt(1).LarsCode, Is.EqualTo("1"));
            Assert.That(actual.ElementAt(2).LarsCode, Is.EqualTo("2"));
        }

        [Test, RecursiveMoqAutoData]
        public void And_OrderBy_Title_Then_Ordered_By_Title_Then_By_Level(
            StandardsSortOrderService standardsSortOrderService)
        {
            //Arrange
            var orderBy = OrderBy.Title;
            var standards = new List<Standard>
            {
                new Standard{LarsCode = "1", Title = "aardvark", Level = 3, LarsStandard = new LarsStandard()},
                new Standard{LarsCode = "2", Title = "aardvark", Level = 1, LarsStandard = new LarsStandard()},
                new Standard{LarsCode = "3", Title = "aardvark", Level = 2, LarsStandard = new LarsStandard()}
            };

            //Act
            var actual = standardsSortOrderService.OrderBy(standards, orderBy, null);

            //Assert
            Assert.That(actual.ElementAt(0).LarsCode, Is.EqualTo("2"));
            Assert.That(actual.ElementAt(1).LarsCode, Is.EqualTo("3"));
            Assert.That(actual.ElementAt(2).LarsCode, Is.EqualTo("1"));
        }

        [Test, RecursiveMoqAutoData]
        public void And_OrderBy_Title_Then_Ordered_By_Title_Then_By_Level_Then_By_LarsStandard(
            StandardsSortOrderService standardsSortOrderService)
        {
            //Arrange
            var orderBy = OrderBy.Title;
            var standards = new List<Standard>
            {
                new Standard{LarsCode = "1", Title = "aardvark", Level = 3, LarsStandard = new LarsStandard{LarsCode = "6"}},
                new Standard{LarsCode = "2", Title = "aardvark", Level = 3, LarsStandard = new LarsStandard{LarsCode = "4"}},
                new Standard{LarsCode = "3", Title = "aardvark", Level = 3, LarsStandard = new LarsStandard{LarsCode = "5"}}
            };

            //Act
            var actual = standardsSortOrderService.OrderBy(standards, orderBy, null);

            //Assert
            Assert.That(actual.ElementAt(0).LarsCode, Is.EqualTo("2"));
            Assert.That(actual.ElementAt(1).LarsCode, Is.EqualTo("3"));
            Assert.That(actual.ElementAt(2).LarsCode, Is.EqualTo("1"));
        }
    }
}
