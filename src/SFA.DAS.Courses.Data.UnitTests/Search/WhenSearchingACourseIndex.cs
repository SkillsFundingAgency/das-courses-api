using System.Collections.Generic;
using System.Linq;
using AutoFixture.NUnit3;
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
        private const int OutdoorId = 4;
        private const int NetworkId = 5;
        private const int DeveloperId = 6;

        private readonly List<Standard> _standards= new List<Standard>
        {
            new Standard{Id = DentalTechId, Title = "Dental technician (integrated)", TypicalJobTitles = "denture maker", Keywords = "dentistry|removable prosthesis|impression trays|orthodontics|dentures|teeth"},
            new Standard{Id = DentalLabAsstId, Title = "Dental laboratory assistant", TypicalJobTitles = "denture maker", Keywords = "dentistry|dental devices|orthodontics|crowns|bridges|dentures|teeth"},
            new Standard{Id = DentalPracticeMgrId, Title = "Dental practice manager", TypicalJobTitles = "Denture Maker", Keywords = "Denture Maker"},
            new Standard{Id = OutdoorId, Title = "Outdoor activity instructor", TypicalJobTitles = "", Keywords = "Outdoor activity instructor|canoeing|sailing|climbing|surfing|cycling|hillwalking|archery|bushcraft|rock poolings|geology|plant identification|habitat|wildlife walk"},
            new Standard{Id = NetworkId, Title = "Network engineer", TypicalJobTitles = "Network Technician|Network Engineer", Keywords = "communication|networks"},
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

        [Test]
        public void Then_Searches_By_Title_Case_Insensitive()
        {
            var searchTerm = "software";
            
            var result = _searchManager.Query(searchTerm);

            result.Standards.Count().Should().Be(1);
            result.Standards.ToList().Should().Contain(searchResult => searchResult.Id == DeveloperId);
        }

        [Test]
        public void Then_Searches_By_Title_Soundex()
        {
            var searchTerm = "softawre";
            
            var result = _searchManager.Query(searchTerm);

            result.Standards.Count().Should().Be(1);
            result.Standards.ToList().Should().Contain(searchResult => searchResult.Id == DeveloperId);
        }

        [Test]
        public void Then_Searches_By_TypicalJobTitles_Case_Insensitive()
        {
            var searchTerm = "application";
            
            var result = _searchManager.Query(searchTerm);

            result.Standards.Count().Should().Be(1);
            result.Standards.ToList().Should().Contain(searchResult => searchResult.Id == DeveloperId);
        }

        [Test]
        public void Then_Searches_By_TypicalJobTitles_Soundex()
        {
            var searchTerm = "applacation";
            
            var result = _searchManager.Query(searchTerm);

            result.Standards.Count().Should().Be(1);
            result.Standards.ToList().Should().Contain(searchResult => searchResult.Id == DeveloperId);
        }

        [Test]
        public void Then_Searches_By_Keywords_Case_Insensitive()
        {
            var searchTerm = "coding";
            
            var result = _searchManager.Query(searchTerm);

            result.Standards.Count().Should().Be(1);
            result.Standards.ToList().Should().Contain(searchResult => searchResult.Id == DeveloperId);
        }

        [Test]
        public void Then_Searches_By_Keywords_Soundex()
        {
            var searchTerm = "kodng";
            
            var result = _searchManager.Query(searchTerm);

            result.Standards.Count().Should().Be(1);
            result.Standards.ToList().Should().Contain(searchResult => searchResult.Id == DeveloperId);
        }
    }
}
