﻿using System;
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
        private const string DentalMatchInTitle = "ST001";
        private const string DentalMatchInKeywords = "ST002";
        private const string DentalMatchInOtherTitles = "ST003";
        private const string PortMatchInTitle = "ST011";
        private const string PortMatchInOtherTitles = "ST012";
        private const string PortMatchInKeywords = "ST013";
        private const string OutdoorId = "ST040";
        private const string NetworkId = "ST050";
        private const string DeveloperId = "STO60";

        private readonly List<Standard> _standards= new List<Standard>
        {
            // scoring and sorting group 1 - has noise in search fields
            new Standard{StandardUId = DentalMatchInTitle, Title = "Dental technician", TypicalJobTitles = "something else", Keywords = "something else"},
            new Standard{StandardUId = DentalMatchInKeywords, Title = "something else", TypicalJobTitles = "something else", Keywords = "something else|Dental technician|something else"},
            new Standard{StandardUId = DentalMatchInOtherTitles, Title = "something else", TypicalJobTitles = "something else|Dental technician", Keywords = "something else"},
            // scoring group 2 - no noise in search fields
            new Standard{StandardUId = PortMatchInTitle, Title = "Port operator", TypicalJobTitles = "something else", Keywords = "something else"},
            new Standard{StandardUId = PortMatchInOtherTitles, Title = "something else", TypicalJobTitles = "Port operator", Keywords = "something else"},
            new Standard{StandardUId = PortMatchInKeywords, Title = "something else", TypicalJobTitles = "something else", Keywords = "Port operator"},
            // control
            new Standard{StandardUId = OutdoorId, Title = "Outdoor activity instructor", TypicalJobTitles = "", Keywords = "Outdoor activity instructor|canoeing|sailing|climbing|surfing|cycling|hillwalking|archery|bushcraft|rock poolings|geology|plant identification|habitat|wildlife walk"},
            new Standard{StandardUId = NetworkId, Title = "Network engineer", TypicalJobTitles = "Network Something|Network Engineer", Keywords = "communication|networks"},
            // text matching
            new Standard{StandardUId = DeveloperId, Title = "Software developer", TypicalJobTitles = "Web Developer|Application Developer", Keywords = "coding things|technology"}
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
        public void Then_Searches_Test_Cases(string searchTerm, int expectedCount, string expectedStandardUId)
        {
            var result = _searchManager.Query(searchTerm);

            result.Standards.Count().Should().Be(expectedCount);
            result.Standards.ToList().Should().Contain(searchResult => searchResult.StandardUId == expectedStandardUId);
        }

        [TestCase("dental technician", 3, TestName = "Exact 2 word match")]
        [TestCase("technician dental", 3, TestName = "2 word match")]
        [TestCase("technician", 3, TestName = "1 word match")]
        [TestCase("technical", 1, TestName = "NGram match")]
        public void And_Noise_Then_Scores_By_Title_Then_TypicalJobTitles_Then_Keywords_Test_Cases(string searchTerm, int expectedCount)
        {
            var result = _searchManager.Query(searchTerm);

            result.Standards.Count().Should().Be(expectedCount);
            var matchTitle = result.Standards.SingleOrDefault(searchResult => searchResult.StandardUId == DentalMatchInTitle);
            var matchKeywords = result.Standards.SingleOrDefault(searchResult => searchResult.StandardUId == DentalMatchInKeywords);
            var matchOtherTitles = result.Standards.SingleOrDefault(searchResult => searchResult.StandardUId == DentalMatchInOtherTitles);

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
            var matchTitle = result.Standards.SingleOrDefault(searchResult => searchResult.StandardUId == PortMatchInTitle);
            var matchKeywords = result.Standards.SingleOrDefault(searchResult => searchResult.StandardUId == PortMatchInKeywords);
            var matchOtherTitles = result.Standards.SingleOrDefault(searchResult => searchResult.StandardUId == PortMatchInOtherTitles);

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
