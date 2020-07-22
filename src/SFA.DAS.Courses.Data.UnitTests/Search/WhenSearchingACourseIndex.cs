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
        private const int DentalTechId2 = 11;
        private const int DentalLabAsstId = 2;
        private const int DentalLabAsstId2 = 12;
        private const int DentalPracticeMgrId = 3;
        private const int DentalPracticeMgrId2 = 13;
        private const int OutdoorId = 40;
        private const int NetworkId = 50;
        private const int DeveloperId = 60;

        private readonly List<Standard> _standards= new List<Standard>
        {
            // scoring and sorting
            new Standard{Id = DentalTechId, Title = "Dental technician", TypicalJobTitles = "something else", Keywords = "something else"},
            new Standard{Id = DentalTechId2, Title = "Technician dental", TypicalJobTitles = "something else", Keywords = "something else"},
            new Standard{Id = DentalLabAsstId, Title = "Laboratory assistant", TypicalJobTitles = "something else", Keywords = "dentistry|Dental technician|something else"},
            new Standard{Id = DentalLabAsstId2, Title = "Laboratory assistant", TypicalJobTitles = "something else", Keywords = "dentistry|Technician dental|something else"},
            new Standard{Id = DentalPracticeMgrId, Title = "Practice manager", TypicalJobTitles = "something else|Dental technician", Keywords = "something else"},
            new Standard{Id = DentalPracticeMgrId2, Title = "Practice manager", TypicalJobTitles = "something else|Technician dental", Keywords = "something else"},
            // control
            new Standard{Id = OutdoorId, Title = "Outdoor activity instructor", TypicalJobTitles = "", Keywords = "Outdoor activity instructor|canoeing|sailing|climbing|surfing|cycling|hillwalking|archery|bushcraft|rock poolings|geology|plant identification|habitat|wildlife walk"},
            new Standard{Id = NetworkId, Title = "Network engineer", TypicalJobTitles = "Network Something|Network Engineer", Keywords = "communication|networks"},
            // text matching
            new Standard{Id = DeveloperId, Title = "Software developer", TypicalJobTitles = "Web Developer|Application Developer", Keywords = "coding things|technology"}
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

        [TestCase("SOFTWARE DEVELOPER", 1, DeveloperId, TestName = "Title phrase case insensitive")]
        [TestCase(" SOFTWARE DEVELOPER", 1, DeveloperId, TestName = "Phrase leading whitespace")]
        [TestCase("SOFTWARE DEVELOPER ", 1, DeveloperId, TestName = "Phrase trailing whitespace")]
        [TestCase("developer software", 1, DeveloperId, TestName = "multiple words out of order")]
        [TestCase("develper softawre", 1, DeveloperId, TestName = "multiple words out of order soundex")]
        [TestCase("softawre", 1, DeveloperId, TestName = "Title soundex")]
        [TestCase(" softawre", 1, DeveloperId, TestName = "Soundex leading whitespace")]
        [TestCase("softawre ", 1, DeveloperId, TestName = "Soundex trailing whitespace")]
        [TestCase("APPLICATION DEVELOPER", 1, DeveloperId, TestName = "TypicalJobTitles phrase case insensitive")]
        [TestCase("applacation", 1, DeveloperId, TestName = "TypicalJobTitles soundex")]
        [TestCase("CODING THINGS", 1, DeveloperId, TestName = "Keywords phrase case insensitive")]
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
            var searchTerm = "dental technician";
            
            var result = _searchManager.Query(searchTerm);

            result.Standards.Count().Should().Be(6);
            var dentalTech = result.Standards.Single(searchResult => searchResult.Id == DentalTechId);
            var dentalTech2 = result.Standards.Single(searchResult => searchResult.Id == DentalTechId2);
            var dentalLabAsst = result.Standards.Single(searchResult => searchResult.Id == DentalLabAsstId);
            var dentalLabAsst2 = result.Standards.Single(searchResult => searchResult.Id == DentalLabAsstId2);
            var dentalPracticeMgr = result.Standards.Single(searchResult => searchResult.Id == DentalPracticeMgrId);
            var dentalPracticeMgr2 = result.Standards.Single(searchResult => searchResult.Id == DentalPracticeMgrId2);

            dentalTech.Score.Should().BeGreaterThan(dentalPracticeMgr.Score, "exact match on title");
            dentalPracticeMgr.Score.Should().BeGreaterThan(dentalTech2.Score, "exact match on typical job title");
            dentalTech2.Score.Should().BeGreaterThan(dentalPracticeMgr2.Score, "2 word match on title");
            dentalPracticeMgr2.Score.Should().BeGreaterThan(dentalLabAsst.Score, "2 word match on typical job title");
            dentalLabAsst.Score.Should().BeGreaterThan(dentalLabAsst2.Score, "exact match on keyword");
        }

        [Test]
        public void Then_Scores_By_Title_Then_TypicalJobTitles_Then_Keywords_Soundex()
        {
            var searchTerm = "dental";
            
            var result = _searchManager.Query(searchTerm);

            result.Standards.Count().Should().Be(6);
            var dentalTech = result.Standards.Single(searchResult => searchResult.Id == DentalTechId);
            var dentalLabAsst = result.Standards.Single(searchResult => searchResult.Id == DentalLabAsstId);
            var dentalPracticeMgr = result.Standards.Single(searchResult => searchResult.Id == DentalPracticeMgrId);

            dentalTech.Score.Should().BeGreaterThan(dentalPracticeMgr.Score);
            dentalPracticeMgr.Score.Should().BeGreaterThan(dentalLabAsst.Score);
        }
    }
}
