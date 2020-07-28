using System;
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
        private const int PortMatchInTitle = 11;
        private const int PortMatchInOtherTitles = 12;
        private const int PortMatchInKeywords = 13;
        private const int OutdoorId = 40;
        private const int NetworkId = 50;
        private const int DeveloperId = 60;

        private readonly List<Standard> _standards= new List<Standard>
        {
            // scoring and sorting group 1 - has noise in search fields
            new Standard{Id = DentalTechId, Title = "Dental technician", TypicalJobTitles = "something else", Keywords = "something else"},
            new Standard{Id = DentalLabAsstId, Title = "something else", TypicalJobTitles = "something else", Keywords = "something else|Dental technician|something else"},
            new Standard{Id = DentalPracticeMgrId, Title = "something else", TypicalJobTitles = "something else|Dental technician", Keywords = "something else"},
            // scoring group 2 - no noise in search fields
            new Standard{Id = PortMatchInTitle, Title = "Port operator", TypicalJobTitles = "something else", Keywords = "something else"},
            new Standard{Id = PortMatchInOtherTitles, Title = "something else", TypicalJobTitles = "Port operator", Keywords = "something else"},
            new Standard{Id = PortMatchInKeywords, Title = "something else", TypicalJobTitles = "something else", Keywords = "Port operator"},
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
        public void And_Noise_In_Search_Fields_Then_Scores_By_Title_Then_TypicalJobTitles_Then_Keywords()
        {
            var exactMatch = "dental technician";
            var twoWordMatch = "technician dental";
            var singleWordMatch = "technician";
            var soundexMatch = "technical";
            
            var exactMatchResult = _searchManager.Query(exactMatch);
            var twoWordMatchResult = _searchManager.Query(twoWordMatch);
            var singleWordResult = _searchManager.Query(singleWordMatch);
            var soundexResult = _searchManager.Query(soundexMatch);

            exactMatchResult.Standards.Count().Should().Be(3);
            var exactMatchInTitle = exactMatchResult.Standards.Single(searchResult => searchResult.Id == DentalTechId);
            var exactMatchInKeywords = exactMatchResult.Standards.Single(searchResult => searchResult.Id == DentalLabAsstId);
            var exactMatchInOtherTitles = exactMatchResult.Standards.Single(searchResult => searchResult.Id == DentalPracticeMgrId);
            
            twoWordMatchResult.Standards.Count().Should().Be(3);
            var twoWordMatchInTitle = twoWordMatchResult.Standards.Single(searchResult => searchResult.Id == DentalTechId);
            var twoWordMatchInKeywords = twoWordMatchResult.Standards.Single(searchResult => searchResult.Id == DentalLabAsstId);
            var twoWordMatchInOtherTitles = twoWordMatchResult.Standards.Single(searchResult => searchResult.Id == DentalPracticeMgrId);

            singleWordResult.Standards.Count().Should().Be(3);
            var singleWordMatchInTitle = singleWordResult.Standards.Single(searchResult => searchResult.Id == DentalTechId);
            var singleWordMatchInKeywords = singleWordResult.Standards.Single(searchResult => searchResult.Id == DentalLabAsstId);
            var singleWordMatchInOtherTitles = singleWordResult.Standards.Single(searchResult => searchResult.Id == DentalPracticeMgrId);

            soundexResult.Standards.Count().Should().Be(3);
            var soundexMatchInTitle = soundexResult.Standards.Single(searchResult => searchResult.Id == DentalTechId);
            var soundexMatchInKeywords = soundexResult.Standards.Single(searchResult => searchResult.Id == DentalLabAsstId);
            var soundexMatchInOtherTitles = soundexResult.Standards.Single(searchResult => searchResult.Id == DentalPracticeMgrId);

            WriteScoresToConsole(new List<Tuple<string, float>>
            {
                new Tuple<string, float>(nameof(exactMatchInTitle), exactMatchInTitle.Score),
                new Tuple<string, float>(nameof(exactMatchInOtherTitles), exactMatchInOtherTitles.Score),
                new Tuple<string, float>(nameof(twoWordMatchInTitle), twoWordMatchInTitle.Score),
                new Tuple<string, float>(nameof(twoWordMatchInOtherTitles), twoWordMatchInOtherTitles.Score),
                new Tuple<string, float>(nameof(singleWordMatchInTitle), singleWordMatchInTitle.Score),
                new Tuple<string, float>(nameof(singleWordMatchInOtherTitles), singleWordMatchInOtherTitles.Score),
                new Tuple<string, float>(nameof(exactMatchInKeywords), exactMatchInKeywords.Score),
                new Tuple<string, float>(nameof(twoWordMatchInKeywords), twoWordMatchInKeywords.Score),
                new Tuple<string, float>(nameof(singleWordMatchInKeywords), singleWordMatchInKeywords.Score),
                new Tuple<string, float>(nameof(soundexMatchInTitle), soundexMatchInTitle.Score),
                new Tuple<string, float>(nameof(soundexMatchInOtherTitles), soundexMatchInOtherTitles.Score),
                new Tuple<string, float>(nameof(soundexMatchInKeywords), soundexMatchInKeywords.Score),
            });

            exactMatchInTitle.Score.Should().BeGreaterThan(exactMatchInOtherTitles.Score, "exact match on title");
            exactMatchInOtherTitles.Score.Should().BeGreaterThan(twoWordMatchInTitle.Score, "exact match on typical job title");
            twoWordMatchInTitle.Score.Should().BeGreaterThan(twoWordMatchInOtherTitles.Score, "2 word match on title");
            twoWordMatchInOtherTitles.Score.Should().BeGreaterThan(singleWordMatchInTitle.Score, "2 word match on typical job title");
            singleWordMatchInTitle.Score.Should().BeGreaterThan(singleWordMatchInOtherTitles.Score, "1 word match on title");
            singleWordMatchInOtherTitles.Score.Should().BeGreaterThan(exactMatchInKeywords.Score, "1 word match on typical job title");
            exactMatchInKeywords.Score.Should().BeGreaterThan(twoWordMatchInKeywords.Score, "exact match on keyword");
            twoWordMatchInKeywords.Score.Should().BeGreaterThan(singleWordMatchInKeywords.Score, "2 word match on keyword");
            singleWordMatchInKeywords.Score.Should().BeGreaterThan(soundexMatchInTitle.Score, "1 word match on keyword");
            soundexMatchInTitle.Score.Should().BeGreaterThan(soundexMatchInOtherTitles.Score, "soundex match on title");
            soundexMatchInOtherTitles.Score.Should().BeGreaterThan(soundexMatchInKeywords.Score, "soundex match on typical job title");
        }

        [Test]
        public void And_No_Noise_Then_Scores_By_Title_Then_TypicalJobTitles_Then_Keywords()
        {
            var exactMatch = "Port operator";
            var twoWordMatch = "operator port";
            var singleWordMatch = "operator";
            var soundexMatch = "poo";
            
            var exactMatchResult = _searchManager.Query(exactMatch);
            var twoWordMatchResult = _searchManager.Query(twoWordMatch);
            var singleWordResult = _searchManager.Query(singleWordMatch);
            var soundexResult = _searchManager.Query(soundexMatch);

            exactMatchResult.Standards.Count().Should().Be(3);
            var exactMatchInTitle = exactMatchResult.Standards.Single(searchResult => searchResult.Id == PortMatchInTitle);
            var exactMatchInKeywords = exactMatchResult.Standards.Single(searchResult => searchResult.Id == PortMatchInKeywords);
            var exactMatchInOtherTitles = exactMatchResult.Standards.Single(searchResult => searchResult.Id == PortMatchInOtherTitles);
            
            twoWordMatchResult.Standards.Count().Should().Be(3);
            var twoWordMatchInTitle = twoWordMatchResult.Standards.Single(searchResult => searchResult.Id == PortMatchInTitle);
            var twoWordMatchInKeywords = twoWordMatchResult.Standards.Single(searchResult => searchResult.Id == PortMatchInKeywords);
            var twoWordMatchInOtherTitles = twoWordMatchResult.Standards.Single(searchResult => searchResult.Id == PortMatchInOtherTitles);

            singleWordResult.Standards.Count().Should().Be(3);
            var singleWordMatchInTitle = singleWordResult.Standards.Single(searchResult => searchResult.Id == PortMatchInTitle);
            var singleWordMatchInKeywords = singleWordResult.Standards.Single(searchResult => searchResult.Id == PortMatchInKeywords);
            var singleWordMatchInOtherTitles = singleWordResult.Standards.Single(searchResult => searchResult.Id == PortMatchInOtherTitles);

            soundexResult.Standards.Count().Should().Be(3);
            var soundexMatchInTitle = soundexResult.Standards.Single(searchResult => searchResult.Id == PortMatchInTitle);
            var soundexMatchInKeywords = soundexResult.Standards.Single(searchResult => searchResult.Id == PortMatchInKeywords);
            var soundexMatchInOtherTitles = soundexResult.Standards.Single(searchResult => searchResult.Id == PortMatchInOtherTitles);

            WriteScoresToConsole(new List<Tuple<string, float>>
            {
                new Tuple<string, float>(nameof(exactMatchInTitle), exactMatchInTitle.Score),
                new Tuple<string, float>(nameof(exactMatchInOtherTitles), exactMatchInOtherTitles.Score),
                new Tuple<string, float>(nameof(twoWordMatchInTitle), twoWordMatchInTitle.Score),
                new Tuple<string, float>(nameof(twoWordMatchInOtherTitles), twoWordMatchInOtherTitles.Score),
                new Tuple<string, float>(nameof(singleWordMatchInTitle), singleWordMatchInTitle.Score),
                new Tuple<string, float>(nameof(singleWordMatchInOtherTitles), singleWordMatchInOtherTitles.Score),
                new Tuple<string, float>(nameof(exactMatchInKeywords), exactMatchInKeywords.Score),
                new Tuple<string, float>(nameof(twoWordMatchInKeywords), twoWordMatchInKeywords.Score),
                new Tuple<string, float>(nameof(singleWordMatchInKeywords), singleWordMatchInKeywords.Score),
                new Tuple<string, float>(nameof(soundexMatchInTitle), soundexMatchInTitle.Score),
                new Tuple<string, float>(nameof(soundexMatchInOtherTitles), soundexMatchInOtherTitles.Score),
                new Tuple<string, float>(nameof(soundexMatchInKeywords), soundexMatchInKeywords.Score),
            });

            exactMatchInTitle.Score.Should().BeGreaterThan(exactMatchInOtherTitles.Score, "exact match on title");
            exactMatchInOtherTitles.Score.Should().BeGreaterThan(twoWordMatchInTitle.Score, "exact match on typical job title");
            twoWordMatchInTitle.Score.Should().BeGreaterThan(twoWordMatchInOtherTitles.Score, "2 word match on title");
            twoWordMatchInOtherTitles.Score.Should().BeGreaterThan(singleWordMatchInTitle.Score, "2 word match on typical job title");
            singleWordMatchInTitle.Score.Should().BeGreaterThan(singleWordMatchInOtherTitles.Score, "1 word match on title");
            singleWordMatchInOtherTitles.Score.Should().BeGreaterThan(exactMatchInKeywords.Score, "1 word match on typical job title");
            exactMatchInKeywords.Score.Should().BeGreaterThan(twoWordMatchInKeywords.Score, "exact match on keyword");
            twoWordMatchInKeywords.Score.Should().BeGreaterThan(singleWordMatchInKeywords.Score, "2 word match on keyword");
            singleWordMatchInKeywords.Score.Should().BeGreaterThan(soundexMatchInTitle.Score, "1 word match on keyword");
            soundexMatchInTitle.Score.Should().BeGreaterThan(soundexMatchInOtherTitles.Score, "soundex match on title");
            soundexMatchInOtherTitles.Score.Should().BeGreaterThan(soundexMatchInKeywords.Score, "soundex match on typical job title");
        }

        [Test]
        public void Then_Scores_By_Title_Then_TypicalJobTitles_Then_Keywords_Soundex()
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

        private void WriteScoresToConsole(IEnumerable<Tuple<string, float>> scores)
        {
            foreach (var score in scores)
            {
                Console.WriteLine($"{score.Item1}:{score.Item2}");
            }
        }
    }
}
