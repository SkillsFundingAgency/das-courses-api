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
        private const int DentalMatchInTitle = 1;
        private const int DentalMatchInKeywords = 2;
        private const int DentalMatchInOtherTitles = 3;
        private const int PortMatchInTitle = 11;
        private const int PortMatchInOtherTitles = 12;
        private const int PortMatchInKeywords = 13;
        private const int OutdoorId = 40;
        private const int NetworkId = 50;
        private const int DeveloperId = 60;

        private readonly List<Standard> _standards= new List<Standard>
        {
            // scoring and sorting group 1 - has noise in search fields
            new Standard{Id = DentalMatchInTitle, Title = "Dental technician", TypicalJobTitles = "something else", Keywords = "something else"},
            new Standard{Id = DentalMatchInKeywords, Title = "something else", TypicalJobTitles = "something else", Keywords = "something else|Dental technician|something else"},
            new Standard{Id = DentalMatchInOtherTitles, Title = "something else", TypicalJobTitles = "something else|Dental technician", Keywords = "something else"},
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
        [TestCase("develper softawre", 1, DeveloperId, TestName = "multiple words out of order ngram")]
        [TestCase("softawre", 1, DeveloperId, TestName = "Title ngram")]
        [TestCase(" softawre", 1, DeveloperId, TestName = "Ngram leading whitespace")]
        [TestCase("softawre ", 1, DeveloperId, TestName = "Ngram trailing whitespace")]
        [TestCase("APPLICATION DEVELOPER", 1, DeveloperId, TestName = "TypicalJobTitles phrase case insensitive")]
        [TestCase("CODING THINGS", 1, DeveloperId, TestName = "Keywords phrase case insensitive")]
        public void Then_Searches_Test_Cases(string searchTerm, int expectedCount, int expectedId)
        {
            var result = _searchManager.Query(searchTerm);

            result.Standards.Count().Should().Be(expectedCount);
            result.Standards.ToList().Should().Contain(searchResult => searchResult.Id == expectedId);
        }

        [TestCase("dental technician", 3, TestName = "Exact 2 word match")]
        [TestCase("technician dental", 3, TestName = "2 word match")]
        [TestCase("technician", 3, TestName = "1 word match")]
        [TestCase("technical", 1, TestName = "NGram match")]
        public void And_Noise_Then_Scores_By_Title_Then_TypicalJobTitles_Then_Keywords_Test_Cases(string searchTerm, int expectedCount)
        {
            var result = _searchManager.Query(searchTerm);

            result.Standards.Count().Should().Be(expectedCount);
            var matchTitle = result.Standards.SingleOrDefault(searchResult => searchResult.Id == DentalMatchInTitle);
            var matchKeywords = result.Standards.SingleOrDefault(searchResult => searchResult.Id == DentalMatchInKeywords);
            var matchOtherTitles = result.Standards.SingleOrDefault(searchResult => searchResult.Id == DentalMatchInOtherTitles);

            WriteScoresToConsole(new List<Tuple<string, float>>
            {
                new Tuple<string, float>(nameof(matchTitle), matchTitle?.Score ?? 0),
                new Tuple<string, float>(nameof(matchOtherTitles), matchOtherTitles?.Score ?? 0),
                new Tuple<string, float>(nameof(matchKeywords), matchKeywords?.Score ?? 0)
            });

            matchTitle?.Score.Should().BeGreaterThan(matchOtherTitles?.Score ?? 0);
            matchOtherTitles?.Score.Should().BeGreaterThan(matchKeywords?.Score ?? 0);
        }

        [TestCase("Port operator", 3, TestName = "Exact 2 word match")]
        [TestCase("operator port", 3, TestName = "2 word match")]
        [TestCase("operator", 3, TestName = "1 word match")]
        [TestCase("operations", 1, TestName = "NGram match")]
        public void And_No_Noise_Then_Scores_By_Title_Then_TypicalJobTitles_Then_Keywords_Test_Cases(string searchTerm, int expectedCount)
        {
            var result = _searchManager.Query(searchTerm);

            result.Standards.Count().Should().Be(expectedCount);
            var matchTitle = result.Standards.SingleOrDefault(searchResult => searchResult.Id == PortMatchInTitle);
            var matchKeywords = result.Standards.SingleOrDefault(searchResult => searchResult.Id == PortMatchInKeywords);
            var matchOtherTitles = result.Standards.SingleOrDefault(searchResult => searchResult.Id == PortMatchInOtherTitles);

            WriteScoresToConsole(new List<Tuple<string, float>>
            {
                new Tuple<string, float>(nameof(matchTitle), matchTitle?.Score ?? 0),
                new Tuple<string, float>(nameof(matchOtherTitles), matchOtherTitles?.Score ?? 0),
                new Tuple<string, float>(nameof(matchKeywords), matchKeywords?.Score ?? 0)
            });

            matchTitle?.Score.Should().BeGreaterThan(matchOtherTitles?.Score ?? 0);
            matchOtherTitles?.Score.Should().BeGreaterThan(matchKeywords?.Score ?? 0);
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
