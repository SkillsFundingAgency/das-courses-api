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
                new Standard{Id = 1, SearchScore = 2, LarsStandard = new LarsStandard()},
                new Standard{Id = 2, SearchScore = 1, LarsStandard = new LarsStandard()},
                new Standard{Id = 3, SearchScore = 3, LarsStandard = new LarsStandard()}
            };                                      
            //Act                                   
            var actual = standardsSortOrderService.OrderBy(standards, orderBy, keyword);

            //Assert
            Assert.AreEqual(3, actual.ElementAt(0).Id);
            Assert.AreEqual(1, actual.ElementAt(1).Id);
            Assert.AreEqual(2, actual.ElementAt(2).Id);
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
                new Standard{Id = 1, SearchScore = 1, Title = "aardvark", LarsStandard = new LarsStandard()},
                new Standard{Id = 2, SearchScore = 3, Title = "aardvark", LarsStandard = new LarsStandard()},
                new Standard{Id = 3, SearchScore = 2, Title = "aardvark", LarsStandard = new LarsStandard()}
            };

            //Act
            var actual = standardsSortOrderService.OrderBy(standards, orderBy, keyword);

            //Assert
            Assert.AreEqual(2, actual .ElementAt(0).Id);
            Assert.AreEqual(3, actual .ElementAt(1).Id);
            Assert.AreEqual(1, actual .ElementAt(2).Id);
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
                new Standard{Id = 1, SearchScore = 1, Title = "aardvark", Level = 3, LarsStandard = new LarsStandard()},
                new Standard{Id = 2, SearchScore = 1, Title = "aardvark", Level = 1, LarsStandard = new LarsStandard()},
                new Standard{Id = 3, SearchScore = 1, Title = "aardvark", Level = 2, LarsStandard = new LarsStandard()}
            };

            //Act
            var actual = standardsSortOrderService.OrderBy(standards, orderBy, keyword);

            //Assert
            Assert.AreEqual(2, actual.ElementAt(0).Id);
            Assert.AreEqual(3, actual.ElementAt(1).Id);
            Assert.AreEqual(1, actual.ElementAt(2).Id);
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
                new Standard{Id = 1, SearchScore = 1, Title = "aardvark", Level = 3, LarsStandard = new LarsStandard{StandardId = 3}},
                new Standard{Id = 2, SearchScore = 1, Title = "aardvark", Level = 3, LarsStandard = new LarsStandard{StandardId = 1}},
                new Standard{Id = 3, SearchScore = 1, Title = "aardvark", Level = 3, LarsStandard = new LarsStandard{StandardId = 2}}
            };

            //Act
            var actual = standardsSortOrderService.OrderBy(standards, orderBy, keyword);

            //Assert
            Assert.AreEqual(2, actual.ElementAt(0).Id);
            Assert.AreEqual(3, actual.ElementAt(1).Id);
            Assert.AreEqual(1, actual.ElementAt(2).Id);
        }

        [Test, RecursiveMoqAutoData]
        public void And_OrderBy_Title_Then_Ordered_By_Title(
            StandardsSortOrderService standardsSortOrderService)
        {
            //Arrange
            var orderBy = OrderBy.Title;
            var standards = new List<Standard>
            {
                new Standard{Id = 1, Title = "bear", LarsStandard = new LarsStandard()},
                new Standard{Id = 2, Title = "chimp", LarsStandard = new LarsStandard()},
                new Standard{Id = 3, Title = "aardvark", LarsStandard = new LarsStandard()}
            };

            //Act
            var actual = standardsSortOrderService.OrderBy(standards, orderBy, null);

            //Assert
            Assert.AreEqual(3, actual.ElementAt(0).Id);
            Assert.AreEqual(1, actual.ElementAt(1).Id);
            Assert.AreEqual(2, actual.ElementAt(2).Id);
        }

        [Test, RecursiveMoqAutoData]
        public void And_OrderBy_Title_Then_Ordered_By_Title_Then_By_Level(
            StandardsSortOrderService standardsSortOrderService)
        {
            //Arrange
            var orderBy = OrderBy.Title;
            var standards = new List<Standard>
            {
                new Standard{Id = 1, Title = "aardvark", Level = 3, LarsStandard = new LarsStandard()},
                new Standard{Id = 2, Title = "aardvark", Level = 1, LarsStandard = new LarsStandard()},
                new Standard{Id = 3, Title = "aardvark", Level = 2, LarsStandard = new LarsStandard()}
            };

            //Act
            var actual = standardsSortOrderService.OrderBy(standards, orderBy, null);

            //Assert
            Assert.AreEqual(2, actual.ElementAt(0).Id);
            Assert.AreEqual(3, actual.ElementAt(1).Id);
            Assert.AreEqual(1, actual.ElementAt(2).Id);
        }

        [Test, RecursiveMoqAutoData]
        public void And_OrderBy_Title_Then_Ordered_By_Title_Then_By_Level_Then_By_LarsStandard(
            StandardsSortOrderService standardsSortOrderService)
        {
            //Arrange
            var orderBy = OrderBy.Title;
            var standards = new List<Standard>
            {
                new Standard{Id = 1, Title = "aardvark", Level = 3, LarsStandard = new LarsStandard{StandardId = 6}},
                new Standard{Id = 2, Title = "aardvark", Level = 3, LarsStandard = new LarsStandard{StandardId = 4}},
                new Standard{Id = 3, Title = "aardvark", Level = 3, LarsStandard = new LarsStandard{StandardId = 5}}
            };

            //Act
            var actual = standardsSortOrderService.OrderBy(standards, orderBy, null);

            //Assert
            Assert.AreEqual(2, actual.ElementAt(0).Id);
            Assert.AreEqual(3, actual.ElementAt(1).Id);
            Assert.AreEqual(1, actual.ElementAt(2).Id);
        }
    }
}
