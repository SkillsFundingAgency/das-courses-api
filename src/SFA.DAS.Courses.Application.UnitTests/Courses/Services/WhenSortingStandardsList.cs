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
                new Standard{Id = 1, SearchScore = 5, Title = "zz top", Level = 3, Sector = new Sector(), ApprenticeshipFunding = new List<ApprenticeshipFunding>(), LarsStandard = new LarsStandard()},
                new Standard{Id = 2, SearchScore = 6, Title = "aardvark", Level = 3, Sector = new Sector(), ApprenticeshipFunding = new List<ApprenticeshipFunding>(), LarsStandard = new LarsStandard()},
                new Standard{Id = 3, SearchScore = 3, Title = "aardvark", Level = 2, Sector = new Sector(), ApprenticeshipFunding = new List<ApprenticeshipFunding>(), LarsStandard = new LarsStandard()},
                new Standard{Id = 4, SearchScore = 4, Title = "in between", Level = 3, Sector = new Sector(), ApprenticeshipFunding = new List<ApprenticeshipFunding>(), LarsStandard = new LarsStandard()},
                new Standard{Id = 5, SearchScore = 1, Title = "zz top", Level = 1, Sector = new Sector(), ApprenticeshipFunding = new List<ApprenticeshipFunding>(), LarsStandard = new LarsStandard()},
                new Standard{Id = 6, SearchScore = 2, Title = "in between", Level = 5, Sector = new Sector(), ApprenticeshipFunding = new List<ApprenticeshipFunding>(), LarsStandard = new LarsStandard()}
            };

            //Act
            var actual = standardsSortOrderService.OrderBy(standards, orderBy, keyword);

            //Assert
            Assert.AreEqual(2, actual.ElementAt(0).Id);
            Assert.AreEqual(1, actual.ElementAt(1).Id);
            Assert.AreEqual(4, actual.ElementAt(2).Id);
            Assert.AreEqual(3, actual.ElementAt(3).Id);
            Assert.AreEqual(6, actual.ElementAt(4).Id);
            Assert.AreEqual(5, actual.ElementAt(5).Id);
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
                new Standard{Id = 1, SearchScore = 5, Title = "zz top", Level = 3, Sector = new Sector(), ApprenticeshipFunding = new List<ApprenticeshipFunding>(), LarsStandard = new LarsStandard()},
                new Standard{Id = 2, SearchScore = 5, Title = "aardvark", Level = 3, Sector = new Sector(), ApprenticeshipFunding = new List<ApprenticeshipFunding>(), LarsStandard = new LarsStandard()},
                new Standard{Id = 3, SearchScore = 3, Title = "zz top", Level = 2, Sector = new Sector(), ApprenticeshipFunding = new List<ApprenticeshipFunding>(), LarsStandard = new LarsStandard()},
                new Standard{Id = 4, SearchScore = 3, Title = "aardvark", Level = 3, Sector = new Sector(), ApprenticeshipFunding = new List<ApprenticeshipFunding>(), LarsStandard = new LarsStandard()},
                new Standard{Id = 5, SearchScore = 1, Title = "zz top", Level = 1, Sector = new Sector(), ApprenticeshipFunding = new List<ApprenticeshipFunding>(), LarsStandard = new LarsStandard()},
                new Standard{Id = 6, SearchScore = 1, Title = "aardvark", Level = 5, Sector = new Sector(), ApprenticeshipFunding = new List<ApprenticeshipFunding>(), LarsStandard = new LarsStandard()}
            };

            //Act
            var actual = standardsSortOrderService.OrderBy(standards, orderBy, keyword);

            //Assert
            Assert.AreEqual(2, actual .ElementAt(0).Id);
            Assert.AreEqual(1, actual .ElementAt(1).Id);
            Assert.AreEqual(4, actual .ElementAt(2).Id);
            Assert.AreEqual(3, actual .ElementAt(3).Id);
            Assert.AreEqual(6, actual .ElementAt(4).Id);
            Assert.AreEqual(5, actual .ElementAt(5).Id);
        }

        [Test, RecursiveMoqAutoData]
        public void And_OrderBy_Score_Then_Ordered_By_Score_Then_By_Title_Then_By_LarsStandard(
            string keyword,
            StandardsSortOrderService standardsSortOrderService)
        {
            //Arrange
            var orderBy = OrderBy.Score;
            var standards = new List<Standard>
            {
                new Standard{Id = 1, SearchScore = 1, Title = "aardvark", Level = 3, Sector = new Sector(), ApprenticeshipFunding = new List<ApprenticeshipFunding>(), LarsStandard = new LarsStandard{StandardId = 5}},
                new Standard{Id = 2, SearchScore = 1, Title = "aardvark", Level = 3, Sector = new Sector(), ApprenticeshipFunding = new List<ApprenticeshipFunding>(), LarsStandard = new LarsStandard{StandardId = 4}},
                new Standard{Id = 3, SearchScore = 1, Title = "aardvark", Level = 2, Sector = new Sector(), ApprenticeshipFunding = new List<ApprenticeshipFunding>(), LarsStandard = new LarsStandard{StandardId = 6}},
                new Standard{Id = 4, SearchScore = 1, Title = "aardvark", Level = 3, Sector = new Sector(), ApprenticeshipFunding = new List<ApprenticeshipFunding>(), LarsStandard = new LarsStandard{StandardId = 2}},
                new Standard{Id = 5, SearchScore = 1, Title = "aardvark", Level = 1, Sector = new Sector(), ApprenticeshipFunding = new List<ApprenticeshipFunding>(), LarsStandard = new LarsStandard{StandardId = 1}},
                new Standard{Id = 6, SearchScore = 1, Title = "aardvark", Level = 5, Sector = new Sector(), ApprenticeshipFunding = new List<ApprenticeshipFunding>(), LarsStandard = new LarsStandard{StandardId = 3}}
            };

            //Act
            var actual = standardsSortOrderService.OrderBy(standards, orderBy, keyword);

            //Assert
            Assert.AreEqual(5, actual.ElementAt(0).Id);
            Assert.AreEqual(4, actual.ElementAt(1).Id);
            Assert.AreEqual(6, actual.ElementAt(2).Id);
            Assert.AreEqual(2, actual.ElementAt(3).Id);
            Assert.AreEqual(1, actual.ElementAt(4).Id);
            Assert.AreEqual(3, actual.ElementAt(5).Id);
        }

        [Test, RecursiveMoqAutoData]
        public void And_OrderBy_Title_Then_Ordered_By_Title(
            StandardsSortOrderService standardsSortOrderService)
        {
            //Arrange
            var orderBy = OrderBy.Title;
            var standards = new List<Standard>
            {
                new Standard{Id = 1, SearchScore = 5, Title = "bear", Level = 3, Sector = new Sector(), ApprenticeshipFunding = new List<ApprenticeshipFunding>(), LarsStandard = new LarsStandard()},
                new Standard{Id = 2, SearchScore = 6, Title = "aardvark", Level = 3, Sector = new Sector(), ApprenticeshipFunding = new List<ApprenticeshipFunding>(), LarsStandard = new LarsStandard()},
                new Standard{Id = 3, SearchScore = 3, Title = "dog", Level = 2, Sector = new Sector(), ApprenticeshipFunding = new List<ApprenticeshipFunding>(), LarsStandard = new LarsStandard()},
                new Standard{Id = 4, SearchScore = 4, Title = "chimp", Level = 3, Sector = new Sector(), ApprenticeshipFunding = new List<ApprenticeshipFunding>(), LarsStandard = new LarsStandard()},
                new Standard{Id = 5, SearchScore = 1, Title = "fish", Level = 1, Sector = new Sector(), ApprenticeshipFunding = new List<ApprenticeshipFunding>(), LarsStandard = new LarsStandard()},
                new Standard{Id = 6, SearchScore = 2, Title = "elephant", Level = 5, Sector = new Sector(), ApprenticeshipFunding = new List<ApprenticeshipFunding>(), LarsStandard = new LarsStandard()}
            };

            //Act
            var actual = standardsSortOrderService.OrderBy(standards, orderBy, null);

            //Assert
            Assert.AreEqual(2, actual.ElementAt(0).Id);
            Assert.AreEqual(1, actual.ElementAt(1).Id);
            Assert.AreEqual(4, actual.ElementAt(2).Id);
            Assert.AreEqual(3, actual.ElementAt(3).Id);
            Assert.AreEqual(6, actual.ElementAt(4).Id);
            Assert.AreEqual(5, actual.ElementAt(5).Id);
        }

        [Test, RecursiveMoqAutoData]
        public void And_OrderBy_Score_Then_Ordered_By_Title_Then_By_Score(
            StandardsSortOrderService standardsSortOrderService)
        {
            //Arrange
            var orderBy = OrderBy.Title;
            var standards = new List<Standard>
            {
                new Standard{Id = 1, SearchScore = 5, Title = "zz top", Level = 3, Sector = new Sector(), ApprenticeshipFunding = new List<ApprenticeshipFunding>(), LarsStandard = new LarsStandard()},
                new Standard{Id = 2, SearchScore = 5, Title = "aardvark", Level = 3, Sector = new Sector(), ApprenticeshipFunding = new List<ApprenticeshipFunding>(), LarsStandard = new LarsStandard()},
                new Standard{Id = 3, SearchScore = 3, Title = "zz top", Level = 2, Sector = new Sector(), ApprenticeshipFunding = new List<ApprenticeshipFunding>(), LarsStandard = new LarsStandard()},
                new Standard{Id = 4, SearchScore = 3, Title = "aardvark", Level = 3, Sector = new Sector(), ApprenticeshipFunding = new List<ApprenticeshipFunding>(), LarsStandard = new LarsStandard()},
                new Standard{Id = 5, SearchScore = 1, Title = "zz top", Level = 1, Sector = new Sector(), ApprenticeshipFunding = new List<ApprenticeshipFunding>(), LarsStandard = new LarsStandard()},
                new Standard{Id = 6, SearchScore = 1, Title = "aardvark", Level = 5, Sector = new Sector(), ApprenticeshipFunding = new List<ApprenticeshipFunding>(), LarsStandard = new LarsStandard()}
            };

            //Act
            var actual = standardsSortOrderService.OrderBy(standards, orderBy, null);

            //Assert
            Assert.AreEqual(2, actual.ElementAt(0).Id);
            Assert.AreEqual(4, actual.ElementAt(1).Id);
            Assert.AreEqual(6, actual.ElementAt(2).Id);
            Assert.AreEqual(1, actual.ElementAt(3).Id);
            Assert.AreEqual(3, actual.ElementAt(4).Id);
            Assert.AreEqual(5, actual.ElementAt(5).Id);
        }

        [Test, RecursiveMoqAutoData]
        public void And_OrderBy_Score_Then_Ordered_By_Title_Then_By_Score_Then_By_LarsStandard(
            StandardsSortOrderService standardsSortOrderService)
        {
            //Arrange
            var orderBy = OrderBy.Title;
            var standards = new List<Standard>
            {
                new Standard{Id = 1, SearchScore = 1, Title = "aardvark", Level = 3, Sector = new Sector(), ApprenticeshipFunding = new List<ApprenticeshipFunding>(), LarsStandard = new LarsStandard{StandardId = 5}},
                new Standard{Id = 2, SearchScore = 1, Title = "aardvark", Level = 3, Sector = new Sector(), ApprenticeshipFunding = new List<ApprenticeshipFunding>(), LarsStandard = new LarsStandard{StandardId = 4}},
                new Standard{Id = 3, SearchScore = 1, Title = "aardvark", Level = 2, Sector = new Sector(), ApprenticeshipFunding = new List<ApprenticeshipFunding>(), LarsStandard = new LarsStandard{StandardId = 6}},
                new Standard{Id = 4, SearchScore = 1, Title = "aardvark", Level = 3, Sector = new Sector(), ApprenticeshipFunding = new List<ApprenticeshipFunding>(), LarsStandard = new LarsStandard{StandardId = 2}},
                new Standard{Id = 5, SearchScore = 1, Title = "aardvark", Level = 1, Sector = new Sector(), ApprenticeshipFunding = new List<ApprenticeshipFunding>(), LarsStandard = new LarsStandard{StandardId = 1}},
                new Standard{Id = 6, SearchScore = 1, Title = "aardvark", Level = 5, Sector = new Sector(), ApprenticeshipFunding = new List<ApprenticeshipFunding>(), LarsStandard = new LarsStandard{StandardId = 3}}
            };

            //Act
            var actual = standardsSortOrderService.OrderBy(standards, orderBy, null);

            //Assert
            Assert.AreEqual(5, actual.ElementAt(0).Id);
            Assert.AreEqual(4, actual.ElementAt(1).Id);
            Assert.AreEqual(6, actual.ElementAt(2).Id);
            Assert.AreEqual(2, actual.ElementAt(3).Id);
            Assert.AreEqual(1, actual.ElementAt(4).Id);
            Assert.AreEqual(3, actual.ElementAt(5).Id);
        }
    }
}
