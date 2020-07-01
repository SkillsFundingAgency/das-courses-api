using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Data.Search;
using SFA.DAS.Courses.Data.UnitTests.DatabaseMock;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Data.UnitTests.Search
{
    public class WhenSearchingACourseIndex
    {
        private const int DentalTechId = 1;
        private const int DentalLabAsstId = 2;
        private const int DentalPracticeMgrId = 3;
        private const int Baker1Id = 10;
        private const int Baker2Id = 11;
        private const int Baker3Id = 12;
        private const int OutdoorId = 40;
        private const int NetworkId = 50;
        private const int DeveloperId = 60;

        private readonly List<Standard> _standards= new List<Standard>
        {
            // scoring and sorting
            new Standard{Id = DentalTechId, Title = "Dental technician (integrated)", TypicalJobTitles = "something else", Keywords = "something else"},
            new Standard{Id = DentalLabAsstId, Title = "Laboratory assistant", TypicalJobTitles = "something else", Keywords = "dentistry|dental devices|something else"},
            new Standard{Id = DentalPracticeMgrId, Title = "Practice manager", TypicalJobTitles = "something else|Dental practice", Keywords = "something else"},
            // default sort
            new Standard{Id = Baker1Id, Title = "AAA Baker"},
            new Standard{Id = Baker2Id, Title = "ZZZ Baker"},
            new Standard{Id = Baker3Id, Title = "CCC Baker"},
            // control
            new Standard{Id = OutdoorId, Title = "Outdoor activity instructor", TypicalJobTitles = "", Keywords = "Outdoor activity instructor|canoeing|sailing|climbing|surfing|cycling|hillwalking|archery|bushcraft|rock poolings|geology|plant identification|habitat|wildlife walk"},
            new Standard{Id = NetworkId, Title = "Network engineer", TypicalJobTitles = "Network Technician|Network Engineer", Keywords = "communication|networks"},
            // text matching
            new Standard{Id = DeveloperId, Title = "Software developer", TypicalJobTitles = "Web Developer|Application Developer", Keywords = "coding|technology"}
        };

        private CoursesSearchManager _searchManager;

        [SetUp]
        public void Setup()
        {
            var mockDataContext = new Mock<ICoursesDataContext>();
            mockDataContext
                .Setup(context => context.Standards)
                .ReturnsDbSet(_standards);
            var directoryFactory = new DirectoryFactory();
            var builder = new CoursesIndexBuilder(mockDataContext.Object, directoryFactory);
            builder.Build();
            _searchManager = new CoursesSearchManager(directoryFactory);
        }

        [TestCase("SOFTWARE", 1, DeveloperId, TestName = "Title case insensitive")]
        [TestCase("softawre", 1, DeveloperId, TestName = "Title soundex")]
        [TestCase("APPLICATION", 1, DeveloperId, TestName = "TypicalJobTitles case insensitive")]
        [TestCase("applacation", 1, DeveloperId, TestName = "TypicalJobTitles soundex")]
        [TestCase("CODING", 1, DeveloperId, TestName = "Keywords case insensitive")]
        [TestCase("kodng", 1, DeveloperId, TestName = "Keywords soundex")]
        public void Then_Searches_Test_Cases(string searchTerm, int expectedCount, int expectedId)
        {
            var result = _searchManager.Query(searchTerm);

            result.Standards.Count().Should().Be(expectedCount);
            result.Standards.ToList().Should().Contain(searchResult => searchResult.Id == expectedId);
        }
        
        [Test]
        public void Then_Scores_By_Title_Then_TypicalJobTitles_Then_Keywords()
        {
            var searchTerm = "dental";
            
            var result = _searchManager.Query(searchTerm);

            result.Standards.Count().Should().Be(3);
            var dentalTech = result.Standards.Single(searchResult => searchResult.Id == DentalTechId);
            var dentalLabAsst = result.Standards.Single(searchResult => searchResult.Id == DentalLabAsstId);
            var dentalPracticeMgr = result.Standards.Single(searchResult => searchResult.Id == DentalPracticeMgrId);

            dentalTech.Score.Should().BeGreaterThan(dentalPracticeMgr.Score);
            dentalPracticeMgr.Score.Should().BeGreaterThan(dentalLabAsst.Score);
        }

        [Test]
        public void Then_Orders_By_Score()
        {
            var searchTerm = "dental";
            
            var result = _searchManager.Query(searchTerm);

            result.Standards.Count().Should().Be(3);
            result.Standards.Select((searchResult, position) => new {searchResult.Id, position}).ToList()
                .Should().BeEquivalentTo(new List<dynamic>
                {
                    new {Id = DentalTechId, position = 0},
                    new {Id = DentalPracticeMgrId, position = 1},
                    new {Id = DentalLabAsstId, position = 2}
                });
        }

        [Test]
        public void Then_Default_Order_By_Title()
        {
            var searchTerm = "baker";
            
            var result = _searchManager.Query(searchTerm);

            result.Standards.Count().Should().Be(3);
            result.Standards.Select((searchResult, position) => new {searchResult.Id, position}).ToList()
                .Should().BeEquivalentTo(new List<dynamic>
                {
                    new {Id = Baker1Id, position = 0},
                    new {Id = Baker3Id, position = 1},
                    new {Id = Baker2Id, position = 2}
                });
        }
    }
}
