using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Application.Courses.Services;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.Interfaces;
using SFA.DAS.Courses.Domain.Search;
using SFA.DAS.Courses.Domain.TestHelper.AutoFixture;
using SFA.DAS.Testing.AutoFixture;

using CourseType = SFA.DAS.Courses.Domain.Entities.CourseType;

namespace SFA.DAS.Courses.Application.UnitTests.Courses.Services
{
    public class WhenGettingCoursesList
    {
        [Test, StandardInlineAutoData(ApprenticeshipType.Apprenticeship)]
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

        [Test, StandardInlineAutoData(ApprenticeshipType.Apprenticeship)]
        public async Task And_No_Keyword_And_Filtering_By_Active_Then_Gets_Courses_Filtered_From_Repository(
            OrderBy orderBy,
            StandardFilter filter,
            List<Standard> standardsFromRepo,
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

        [Test, StandardInlineAutoData(ApprenticeshipType.Apprenticeship)]
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
                standardsFromRepo.Select(s => (Domain.Courses.Course)s), config => config.Excluding(p => p.CourseDates));
        }

        [Test, StandardInlineAutoData(ApprenticeshipType.Apprenticeship)]
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
                .Setup(repository => repository.GetStandards(new List<int>(), levelCodes, filter, true, new List<ApprenticeshipType> { ApprenticeshipType.Apprenticeship }, null))
                .ReturnsAsync(standardsFromRepo);

            mockSortOrderService
                .Setup(orderService => orderService.OrderBy(standardsFromRepo, It.IsAny<OrderBy>(), It.IsAny<string>()))
                .Returns(standardsFromRepo.OrderBy(standard => standard.SearchScore));

            // Act
            var result = await service.GetCoursesList("", new List<int>(), levelCodes, orderBy, filter, true, new List<ApprenticeshipType> { ApprenticeshipType.Apprenticeship }, null);

            // Assert
            result.Should().BeEquivalentTo(
                standardsFromRepo.Select(s => (Domain.Courses.Course)s));
        }

        [Test, StandardInlineAutoData(ApprenticeshipType.Apprenticeship)]
        public async Task Then_Courses_Filtered_Are_Ordered_By_SortOrderService(
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
                .Setup(repository => repository.GetStandards(new List<int>(), levelCodes, filter, false, null, null))
                .ReturnsAsync(standardsFromRepo);

            mockSortOrderService
                .Setup(orderService => orderService.OrderBy(standardsFromRepo, orderBy, ""))
                .Returns(standardsFromRepo.OrderBy(standard => standard.SearchScore));

            // Act
            var result = await service.GetCoursesList("", new List<int>(), levelCodes, orderBy, filter, false, null, null);

            // Assert
            result.Should().BeEquivalentTo(
                standardsFromRepo.Select(s => (Domain.Courses.Course)s));
        }

        [Test, StandardInlineAutoData()]
        public async Task When_List_Contains_ShortCourse_Then_Populates_CourseDates(
            IFixture fixture,
            OrderBy orderBy,
            StandardFilter filter,
            [Frozen] Mock<IStandardRepository> standardsRepository,
            [Frozen] Mock<IStandardsSortOrderService> sortOrderService,
            StandardsService _sut)
        {
            // Arrange
            var shortCourseLars = "ZSC00123";
            var apprenticeshipLars = "123";

            var approvedForDelivery = new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var versionLatestStartDate = new DateTime(2020, 2, 2, 0, 0, 0, DateTimeKind.Utc);

            var firstActiveShortCourse = fixture.Build<Standard>()
                .With(x => x.StandardUId, "AU0001_1.0")
                .With(x => x.LarsCode, shortCourseLars)
                .With(x => x.CourseType, CourseType.ShortCourse)
                .With(x => x.LarsStandard, (LarsStandard)null)
                .With(x => x.Route, new Route { Name = "Route", Id = 1 })
                .With(x => x.ApprovedForDelivery, approvedForDelivery)
                .With(x => x.VersionLatestStartDate, versionLatestStartDate)
                .Create();

            var secondActiveShortCourse = fixture.Build<Standard>()
                .With(x => x.StandardUId, "AU0001_1.1")
                .With(x => x.LarsCode, shortCourseLars)
                .With(x => x.CourseType, CourseType.ShortCourse)
                .With(x => x.LarsStandard, (LarsStandard)null)
                .With(x => x.Route, new Route { Name = "Route", Id = 1 })
                .With(x => x.ApprovedForDelivery, approvedForDelivery.AddDays(10))
                .With(x => x.VersionLatestStartDate, versionLatestStartDate.AddDays(10))
                .Create();

            var thirdActiveShortCourse = fixture.Build<Standard>()
                .With(x => x.StandardUId, "AU0001_1.2")
                .With(x => x.LarsCode, shortCourseLars)
                .With(x => x.CourseType, CourseType.ShortCourse)
                .With(x => x.LarsStandard, (LarsStandard)null)
                .With(x => x.Route, new Route { Name = "Route", Id = 1 })
                .With(x => x.ApprovedForDelivery, approvedForDelivery.AddDays(20))
                .With(x => x.VersionLatestStartDate, versionLatestStartDate.AddDays(20))
                .Create();

            var activeApprenticeship = fixture.Build<Standard>()
                .With(x => x.StandardUId, "ST0001_1.0")
                .With(x => x.LarsCode, apprenticeshipLars)
                .With(x => x.CourseType, CourseType.Apprenticeship)
                .With(x => x.Route, new Route { Name = "Route", Id = 1 })
                .With(x => x.ApprovedForDelivery, new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc))
                .With(x => x.VersionLatestStartDate, new DateTime(2021, 2, 2, 0, 0, 0, DateTimeKind.Utc))
                .Create();

            var shortCoursesFromRepo = new List<Standard>
            {
                firstActiveShortCourse,
                secondActiveShortCourse,
                thirdActiveShortCourse
            };

            var apprenticeshipsFromRepo = new List<Standard> { activeApprenticeship };

            standardsRepository
                .Setup(r => r.GetStandards(new List<int>(), new List<int>(), filter, false, null, null))
                .ReturnsAsync(shortCoursesFromRepo.Union(apprenticeshipsFromRepo));

            standardsRepository
                .Setup(r => r.GetShortCourseDates(null))
                .ReturnsAsync(new List<ShortCourseDates>
                {
                    new ShortCourseDates
                    {
                        LarsCode = shortCourseLars,
                        EffectiveFrom = firstActiveShortCourse.ApprovedForDelivery.Value,
                        EffectiveTo = thirdActiveShortCourse.VersionLatestStartDate,
                        LastDateStarts = thirdActiveShortCourse.VersionLatestStartDate
                    }
                });

            sortOrderService
                .Setup(s => s.OrderBy(It.IsAny<IEnumerable<Standard>>(), It.IsAny<OrderBy>(), It.IsAny<string>()))
                .Returns((IEnumerable<Standard> items, OrderBy _, string __) => items.OrderBy(x => x.StandardUId));

            // Act
            var result = (await _sut.GetCoursesList("", new List<int>(), new List<int>(), orderBy, filter, false, null, null)).ToList();

            // Assert
            var shortCourses = result
                .Where(x => x.LarsCode == shortCourseLars)
                .ToList();

            shortCourses.Should().HaveCount(3);

            shortCourses.Select(x => x.CourseDates).Should().AllSatisfy(courseDates =>
            {
                courseDates.Should().NotBeNull();
                courseDates!.EffectiveFrom.Should().Be(firstActiveShortCourse.ApprovedForDelivery);
                courseDates.EffectiveTo.Should().Be(thirdActiveShortCourse.VersionLatestStartDate);
                courseDates.LastDateStarts.Should().Be(thirdActiveShortCourse.VersionLatestStartDate);
            });

            var apprenticeship = result.Single(x => x.LarsCode == apprenticeshipLars);
            apprenticeship.CourseDates.Should().BeEquivalentTo((Domain.Courses.CourseDates)activeApprenticeship.LarsStandard);
        }
    }
}
