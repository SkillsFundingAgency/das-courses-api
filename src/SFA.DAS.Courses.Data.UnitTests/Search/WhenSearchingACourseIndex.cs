﻿using System.Collections.Generic;
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
        private const int OutdoorId = 40;
        private const int NetworkId = 50;
        private const int DeveloperId = 60;

        private readonly List<Standard> _standards= new List<Standard>
        {
            // scoring and sorting
            new Standard{Id = DentalTechId, Title = "Dental technician (integrated)", TypicalJobTitles = "something else", Keywords = "something else"},
            new Standard{Id = DentalLabAsstId, Title = "Laboratory assistant", TypicalJobTitles = "something else", Keywords = "dentistry|dental devices|something else"},
            new Standard{Id = DentalPracticeMgrId, Title = "Practice manager", TypicalJobTitles = "something else|Dental practice", Keywords = "something else"},
            // control
            new Standard{Id = OutdoorId, Title = "Outdoor activity instructor", TypicalJobTitles = "", Keywords = "Outdoor activity instructor|canoeing|sailing|climbing|surfing|cycling|hillwalking|archery|bushcraft|rock poolings|geology|plant identification|habitat|wildlife walk"},
            new Standard{Id = NetworkId, Title = "Network engineer", TypicalJobTitles = "Network Technician|Network Engineer", Keywords = "communication|networks"},
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
        [TestCase("softawre", 1, DeveloperId, TestName = "Title soundex")]
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
            var searchTerm = "dental";
            
            var result = _searchManager.Query(searchTerm);

            result.Standards.Count().Should().Be(3);
            var dentalTech = result.Standards.Single(searchResult => searchResult.Id == DentalTechId);
            var dentalLabAsst = result.Standards.Single(searchResult => searchResult.Id == DentalLabAsstId);
            var dentalPracticeMgr = result.Standards.Single(searchResult => searchResult.Id == DentalPracticeMgrId);

            dentalTech.Score.Should().BeGreaterThan(dentalPracticeMgr.Score);
            dentalPracticeMgr.Score.Should().BeGreaterThan(dentalLabAsst.Score);
        }
    }
}
