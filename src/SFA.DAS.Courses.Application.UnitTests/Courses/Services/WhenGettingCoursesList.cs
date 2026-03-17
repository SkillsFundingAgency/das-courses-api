using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Application.Courses.Services;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.Interfaces;
using SFA.DAS.Courses.Domain.Search;
using SFA.DAS.Testing.AutoFixture;

using CourseType = SFA.DAS.Courses.Domain.Entities.CourseType;

namespace SFA.DAS.Courses.Application.UnitTests.Courses.Services
{
    public class WhenGettingCoursesList
    {
        [Test, RecursiveMoqAutoData]
        public async Task And_No_Keyword_Then_Gets_Courses_Filtered_From_Repository(
            List<Standard> standardsFromRepo,
            OrderBy orderBy,
            StandardFilter filter,
            [Frozen] Mock<IStandardRepository> mockStandardsRepository,
            [Frozen] Mock<IStandardsSortOrderService> mockSortOrderService,
            StandardsService service)
        {
            // Arrange
            mockStandardsRepository
                .Setup(repository => repository.GetStandards(new List<int>(), new List<int>(), filter, false, null, null))
                .ReturnsAsync(standardsFromRepo);
            mockSortOrderService
                .Setup(orderService => orderService.OrderBy(standardsFromRepo, It.IsAny<OrderBy>(), It.IsAny<string>()))
                .Returns(standardsFromRepo.OrderBy(standard => standard.SearchScore));

            // Act
            var result = (await service.GetCoursesList("", new List<int>(), new List<int>(), orderBy, filter, false, null, null)).ToList();

            // Assert
            result.Should().BeEquivalentTo(
                standardsFromRepo.Select(s => (Domain.Courses.Course)s));
        }

        [Test, RecursiveMoqAutoData]
        public async Task And_No_Keyword_And_Filtering_By_Active_Then_Gets_Courses_Filtered_From_Repository(
            List<Standard> standardsFromRepo,
            OrderBy orderBy,
            StandardFilter filter,
            [Frozen] Mock<IStandardRepository> mockStandardsRepository,
            [Frozen] Mock<IStandardsSortOrderService> mockSortOrderService,
            StandardsService service)
        {
            // Arrange
            filter = StandardFilter.Active;
            mockStandardsRepository
                .Setup(repository => repository.GetStandards(new List<int>(), new List<int>(), filter, false, null, null))
                .ReturnsAsync(standardsFromRepo);
            mockSortOrderService
                .Setup(orderService => orderService.OrderBy(standardsFromRepo, It.IsAny<OrderBy>(), It.IsAny<string>()))
                .Returns(standardsFromRepo.OrderBy(standard => standard.SearchScore));

            // Act
            var result = (await service.GetCoursesList("", new List<int>(), new List<int>(), orderBy, filter, false, null, null)).ToList();

            // Assert
            result.Should().BeEquivalentTo(
                standardsFromRepo.Select(s => (Domain.Courses.Course)s));
        }

        [Test, RecursiveMoqAutoData]
        public async Task And_Has_Keyword_Then_Gets_Courses_Filtered_From_SearchManager(
            string keyword,
            OrderBy orderBy,
            StandardFilter filter,
            List<Standard> standardsFromRepo,
            StandardSearchResultsList searchResult,
            [Frozen] Mock<IStandardRepository> mockStandardsRepository,
            [Frozen] Mock<ISearchManager> mockSearchManager,
            [Frozen] Mock<IStandardsSortOrderService> mockSortOrderService,
            StandardsService service)
        {
            // Arrange
            searchResult.Standards = new List<StandardSearchResult>
            {
                new StandardSearchResult{StandardUId = standardsFromRepo[0].StandardUId}
            };
            var standardsFoundInSearch = standardsFromRepo
                .Where(standard => searchResult.Standards.Select(result => result.StandardUId).Contains(standard.StandardUId))
                .ToList();
            mockStandardsRepository
                .Setup(repository => repository.GetStandards(new List<int>(), new List<int>(), filter, false, null, null))
                .ReturnsAsync(standardsFromRepo);
            mockSearchManager
                .Setup(manager => manager.Query(keyword))
                .Returns(searchResult);
            mockSortOrderService
                .Setup(orderService => orderService.OrderBy(standardsFoundInSearch, It.IsAny<OrderBy>(), It.IsAny<string>()))
                .Returns(standardsFoundInSearch.OrderBy(standard => standard.SearchScore));

            // Act
            var result = await service.GetCoursesList(keyword, new List<int>(), new List<int>(), orderBy, filter, false, null, null);

            // Assert
            result.Should().BeEquivalentTo(
                standardsFoundInSearch.Select(s => (Domain.Courses.Course)s));
        }

        [Test, RecursiveMoqAutoData]
        public async Task And_Has_Keyword_And_Sectors_Then_Gets_Courses_Filtered_From_SearchManager_And_Filters(
            string keyword,
            OrderBy orderBy,
            StandardFilter filter,
            List<int> routeIds,
            List<Standard> standardsFromRepo,
            StandardSearchResultsList searchResult,
            [Frozen] Mock<IStandardRepository> mockStandardsRepository,
            [Frozen] Mock<ISearchManager> mockSearchManager,
            [Frozen] Mock<IStandardsSortOrderService> mockSortOrderService,
            StandardsService service)
        {
            // Arrange
            searchResult.Standards = new List<StandardSearchResult>
            {
                new StandardSearchResult{StandardUId = standardsFromRepo[0].StandardUId}
            };
            var standardsFoundInSearch = standardsFromRepo
                .Where(standard => searchResult.Standards.Select(result => result.StandardUId).Contains(standard.StandardUId))
                .ToList();
            mockStandardsRepository
                .Setup(repository => repository.GetStandards(routeIds, new List<int>(), filter, true, null, null))
                .ReturnsAsync(standardsFromRepo);
            mockSearchManager
                .Setup(manager => manager.Query(keyword))
                .Returns(searchResult);
            mockSortOrderService
                .Setup(orderService => orderService.OrderBy(standardsFoundInSearch, It.IsAny<OrderBy>(), It.IsAny<string>()))
                .Returns(standardsFoundInSearch.OrderBy(standard => standard.SearchScore));

            // Act
            var result = await service.GetCoursesList(keyword, routeIds, new List<int>(), orderBy, filter, true, null, null);

            // Assert
            result.Should().BeEquivalentTo(
                standardsFoundInSearch.Select(s => (Domain.Courses.Course)s));
        }

        [Test, RecursiveMoqAutoData]
        public async Task And_Has_Keyword_And_Sectors_And_Filter_Active_Then_Gets_Courses_Filtered_From_SearchManager_And_Filters(
            string keyword,
            OrderBy orderBy,
            StandardFilter filter,
            List<int> routeIds,
            List<Standard> standardsFromRepo,
            StandardSearchResultsList searchResult,
            [Frozen] Mock<IStandardRepository> mockStandardsRepository,
            [Frozen] Mock<ISearchManager> mockSearchManager,
            [Frozen] Mock<IStandardsSortOrderService> mockSortOrderService,
            StandardsService service)
        {
            // Arrange
            searchResult.Standards = new List<StandardSearchResult>
            {
                new StandardSearchResult{StandardUId = standardsFromRepo[0].StandardUId}
            };
            filter = StandardFilter.Active;
            var standardsFoundInSearch = standardsFromRepo
                .Where(standard => searchResult.Standards.Select(result => result.StandardUId).Contains(standard.StandardUId))
                .ToList();
            mockStandardsRepository
                .Setup(repository => repository.GetStandards(routeIds, new List<int>(), filter, false, null, null))
                .ReturnsAsync(standardsFromRepo);
            mockSearchManager
                .Setup(manager => manager.Query(keyword))
                .Returns(searchResult);
            mockSortOrderService
                .Setup(orderService => orderService.OrderBy(standardsFoundInSearch, It.IsAny<OrderBy>(), It.IsAny<string>()))
                .Returns(standardsFoundInSearch.OrderBy(standard => standard.SearchScore));

            // Act
            var result = await service.GetCoursesList(keyword, routeIds, new List<int>(), orderBy, filter, false, null, null);

            // Assert
            result.Should().BeEquivalentTo(
                standardsFoundInSearch.Select(s => (Domain.Courses.Course)s));
        }

        [Test, RecursiveMoqAutoData]
        public async Task And_Has_Levels_Then_Gets_Standards_Filtered_From_Filters(
            List<int> levelCodes,
            OrderBy orderBy,
            StandardFilter filter,
            List<Standard> standardsFromRepo,
            [Frozen] Mock<IStandardRepository> mockStandardsRepository,
            [Frozen] Mock<IStandardsSortOrderService> mockSortOrderService,
            StandardsService service)
        {
            // Arrange
            mockStandardsRepository
                .Setup(repository => repository.GetStandards(new List<int>(), levelCodes, filter, true, null, null))
                .ReturnsAsync(standardsFromRepo);
            mockSortOrderService
                .Setup(orderService => orderService.OrderBy(standardsFromRepo, It.IsAny<OrderBy>(), It.IsAny<string>()))
                .Returns(standardsFromRepo.OrderBy(standard => standard.SearchScore));

            // Act
            var result = await service.GetCoursesList("", new List<int>(), levelCodes, orderBy, filter, true, null, null);

            // Assert
            result.Should().BeEquivalentTo(
                standardsFromRepo.Select(s => (Domain.Courses.Course)s));
        }

        [Test, RecursiveMoqAutoData]
        public async Task And_Has_ApprenticeshipType_Then_Gets_Courses_Filtered_From_Filters(
            List<int> levelCodes,
            OrderBy orderBy,
            StandardFilter filter,
            List<Standard> standardsFromRepo,
            [Frozen] Mock<IStandardRepository> mockStandardsRepository,
            [Frozen] Mock<IStandardsSortOrderService> mockSortOrderService,
            StandardsService service)
        {
            // Arrange
            mockStandardsRepository
                .Setup(repository => repository.GetStandards(new List<int>(), levelCodes, filter, true, ApprenticeshipType.Apprenticeship, null))
                .ReturnsAsync(standardsFromRepo);
            mockSortOrderService
                .Setup(orderService => orderService.OrderBy(standardsFromRepo, It.IsAny<OrderBy>(), It.IsAny<string>()))
                .Returns(standardsFromRepo.OrderBy(standard => standard.SearchScore));

            // Act
            var result = await service.GetCoursesList("", new List<int>(), levelCodes, orderBy, filter, true, ApprenticeshipType.Apprenticeship, null);

            // Assert
            result.Should().BeEquivalentTo(
                standardsFromRepo.Select(s => (Domain.Courses.Course)s));
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_Courses_Filtered_Are_Ordered_By_SortOrderService(
            List<int> levelCodes,
            OrderBy orderBy,
            StandardFilter filter,
            List<Standard> standardsFromRepo,
            List<Standard> standardsFromSortService,
            [Frozen] Mock<IStandardRepository> mockStandardsRepository,
            [Frozen] Mock<IStandardsSortOrderService> mockSortOrderService,
            StandardsService service)
        {
            // Arrange
            mockStandardsRepository
                .Setup(repository => repository.GetStandards(new List<int>(), levelCodes, filter, false, null, null))
                .ReturnsAsync(standardsFromRepo);
            mockSortOrderService
                .Setup(orderService => orderService.OrderBy(standardsFromRepo, orderBy, ""))
                .Returns(standardsFromSortService.OrderBy(standard => standard.SearchScore));

            // Act
            var result = await service.GetCoursesList("", new List<int>(), levelCodes, orderBy, filter, false, null, null);

            // Assert
            result.Should().BeEquivalentTo(
                standardsFromSortService.Select(s => (Domain.Courses.Course)s));
        }

        [Test, MoqAutoData]
        public async Task When_List_Contains_ShortCourse_With_Null_CourseDates_Then_Populates_Only_For_ShortCourse(
            OrderBy orderBy,
            StandardFilter filter,
            [Frozen] Mock<IStandardRepository> standardsRepository,
            [Frozen] Mock<IStandardsSortOrderService> sortOrderService,
            StandardsService _sut)
        {
            // Arrange
            var shortCourseLars = "ZSC00123";
            var apprenticeshipLars = "123";

            var earliestApproved = new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var latestStart = new DateTime(2030, 6, 30, 0, 0, 0, DateTimeKind.Utc);

            var earliestActiveShortCourse = new Standard
            {
                StandardUId = "AU0001_1.0",
                LarsCode = shortCourseLars,
                CourseType = CourseType.ShortCourse,
                LarsStandard = null,
                Route = new Route { Name = "Route", Id = 1 },
                ApprovedForDelivery = earliestApproved
            };

            var latestActiveShortCourseFromRepo = new Standard
            {
                StandardUId = "AU0001_1.1",
                LarsCode = shortCourseLars,
                CourseType = CourseType.ShortCourse,
                LarsStandard = null,
                Route = new Route { Name = "Route", Id = 1 },
                VersionLatestStartDate = latestStart
            };

            var activeApprenticeshipFromRepo = new Standard
            {
                StandardUId = "ST0001_1.0",
                LarsCode = apprenticeshipLars,
                CourseType = CourseType.Apprenticeship,
                LarsStandard = null,
                Route = new Route { Name = "Route", Id = 1 }
            };

            var listFromRepo = new List<Standard> { latestActiveShortCourseFromRepo, activeApprenticeshipFromRepo };

            standardsRepository
                .Setup(r => r.GetStandards(new List<int>(), new List<int>(), filter, false, null, null))
                .ReturnsAsync(listFromRepo);

            standardsRepository
                .Setup(r => r.GetLatestActiveStandard(shortCourseLars, null))
                .ReturnsAsync(latestActiveShortCourseFromRepo);

            standardsRepository
                .Setup(r => r.GetEarliestActiveStandard(shortCourseLars, null))
                .ReturnsAsync(earliestActiveShortCourse);

            sortOrderService
            .Setup(s => s.OrderBy(It.IsAny<IEnumerable<Standard>>(), It.IsAny<OrderBy>(), It.IsAny<string>()))
            .Returns((IEnumerable<Standard> items, OrderBy _, string __) => items.OrderBy(x => x.StandardUId));

            // Act
            var result = (await _sut.GetCoursesList("", new List<int>(), new List<int>(), orderBy, filter, false, null, null)).ToList();

            // Assert
            var shortCourse = result.Single(x => x.LarsCode == shortCourseLars);
            shortCourse.CourseDates.Should().NotBeNull();
            shortCourse.CourseDates.EffectiveFrom.Should().Be(earliestApproved);
            shortCourse.CourseDates.EffectiveTo.Should().Be(latestStart);
            shortCourse.CourseDates.LastDateStarts.Should().Be(latestStart);

            var apprenticeship = result.Single(x => x.LarsCode == apprenticeshipLars);
            apprenticeship.CourseDates.Should().BeNull();

            // the short courses course data was calculated from versions
            standardsRepository.Verify(r => r.GetLatestActiveStandard(shortCourseLars, null), Times.Once);
            standardsRepository.Verify(r => r.GetEarliestActiveStandard(shortCourseLars, null), Times.Once);

            // the apprenticeship courses data was known from lars standard
            standardsRepository.Verify(r => r.GetLatestActiveStandard(apprenticeshipLars, null), Times.Never);
            standardsRepository.Verify(r => r.GetEarliestActiveStandard(apprenticeshipLars, null), Times.Never);
        }
    }
}
