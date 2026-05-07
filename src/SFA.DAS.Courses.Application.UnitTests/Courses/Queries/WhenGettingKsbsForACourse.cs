using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit4;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Application.Courses.Queries.GetCourseOptionKsbs;
using SFA.DAS.Courses.Domain.Courses;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Application.UnitTests.Courses.Queries
{
    public class WhenGettingKsbsForACourse
    {
        [Test, AutoData]
        public async Task Then_Returns_Ksbs_For_Requested_Option(Course course)
        {
            // Arrange
            course.Options = new List<CourseOption>
            {
                new CourseOption
                {
                    Title = "Core",
                    Ksbs = new List<Ksb>
                    {
                        new Ksb { Id = Guid.NewGuid(), Key = "KSB1", Detail = "Detail 1", Type = KsbType.Knowledge },
                        new Ksb { Id = Guid.NewGuid(), Key = "KSB2", Detail = "Detail 2", Type = KsbType.Skill },
                        new Ksb { Id = Guid.NewGuid(), Key = "KSB3", Detail = "Detail 3", Type = KsbType.Behaviour },
                        new Ksb { Id = Guid.NewGuid(), Key = "KSB4", Detail = "Detail 4", Type = KsbType.TechnicalKnowledge },
                        new Ksb { Id = Guid.NewGuid(), Key = "KSB5", Detail = "Detail 5", Type = KsbType.TechnicalSkill },
                        new Ksb { Id = Guid.NewGuid(), Key = "KSB6", Detail = "Detail 6", Type = KsbType.EmployabilitySkillsAndBehaviour }
                    }
                },
                new CourseOption
                {
                    Title = "Option 1",
                    Ksbs = new List<Ksb>
                    {
                        new Ksb { Id = Guid.NewGuid(), Key = "KSB7", Detail = "Detail 7", Type = KsbType.Knowledge }
                    }
                }
            };

            var standardsServiceMock = new Mock<IStandardsService>();

            standardsServiceMock
                .Setup(service => service.GetCourseByAnyId("123"))
                .ReturnsAsync(course);

            var query = new GetCourseOptionKsbsQuery
            {
                Id = "123",
                Option = "Core"
            };

            var _sut = new GetCourseOptionKsbsQueryHandler(standardsServiceMock.Object);

            // Act
            var result = await _sut.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Ksbs.Should().BeEquivalentTo(course.Options[0].Ksbs);

            standardsServiceMock.Verify(
                service => service.GetCourseByAnyId("123"),
                Times.Once);
        }

        [Test, AutoData]
        public async Task Then_Returns_All_Distinct_Ksbs_When_Option_Is_All(Course course)
        {
            // Arrange
            var sharedKsbId = Guid.NewGuid();

            var sharedKsbOne = new Ksb
            {
                Id = sharedKsbId,
                Key = "KSB1",
                Detail = "Shared detail",
                Type = KsbType.Knowledge
            };

            var sharedKsbTwo = new Ksb
            {
                Id = sharedKsbId,
                Key = "KSB1",
                Detail = "Shared detail",
                Type = KsbType.Knowledge
            };

            var optionOneKsb = new Ksb
            {
                Id = Guid.NewGuid(),
                Key = "KSB2",
                Detail = "Option one detail",
                Type = KsbType.Skill
            };

            var optionTwoKsb = new Ksb
            {
                Id = Guid.NewGuid(),
                Key = "KSB3",
                Detail = "Option two detail",
                Type = KsbType.Behaviour
            };

            course.Options = new List<CourseOption>
            {
                new CourseOption
                {
                    Title = "Core",
                    Ksbs = new List<Ksb>
                    {
                        sharedKsbOne,
                        optionOneKsb
                    }
                },
                new CourseOption
                {
                    Title = "Option 1",
                    Ksbs = new List<Ksb>
                    {
                        sharedKsbTwo,
                        optionTwoKsb
                    }
                }
            };

            var standardsServiceMock = new Mock<IStandardsService>();

            standardsServiceMock
                .Setup(service => service.GetCourseByAnyId("123"))
                .ReturnsAsync(course);

            var query = new GetCourseOptionKsbsQuery
            {
                Id = "123",
                Option = "all"
            };

            var _sut = new GetCourseOptionKsbsQueryHandler(standardsServiceMock.Object);

            // Act
            var result = await _sut.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();

            result.Ksbs.Should().BeEquivalentTo(new[]
            {
                sharedKsbOne,
                optionOneKsb,
                optionTwoKsb
            });

            result.Ksbs
                .Where(ksb => ksb.Id == sharedKsbId)
                .Should()
                .ContainSingle();

            standardsServiceMock.Verify(
                service => service.GetCourseByAnyId("123"),
                Times.Once);
        }

        [Test, AutoData]
        public async Task Then_Returns_Empty_Array_When_Course_Does_Not_Exist(
            string id,
            string option)
        {
            // Arrange
            var standardsServiceMock = new Mock<IStandardsService>();

            standardsServiceMock
                .Setup(service => service.GetCourseByAnyId(id))
                .ReturnsAsync((Course)null);

            var query = new GetCourseOptionKsbsQuery
            {
                Id = id,
                Option = option
            };

            var _sut = new GetCourseOptionKsbsQueryHandler(standardsServiceMock.Object);

            // Act
            var result = await _sut.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Ksbs.Should().BeEmpty();

            standardsServiceMock.Verify(
                service => service.GetCourseByAnyId(id),
                Times.Once);
        }

        [Test, AutoData]
        public async Task Then_Returns_Empty_Array_When_Option_Does_Not_Exist(Course course)
        {
            // Arrange
            course.Options = new List<CourseOption>
            {
                new CourseOption
                {
                    Title = "Core",
                    Ksbs = new List<Ksb>
                    {
                        new Ksb
                        {
                            Id = Guid.NewGuid(),
                            Key = "KSB1",
                            Detail = "Detail 1",
                            Type = KsbType.Knowledge
                        }
                    }
                }
            };

            var standardsServiceMock = new Mock<IStandardsService>();

            standardsServiceMock
                .Setup(service => service.GetCourseByAnyId("123"))
                .ReturnsAsync(course);

            var query = new GetCourseOptionKsbsQuery
            {
                Id = "123",
                Option = "Not an option"
            };

            var _sut = new GetCourseOptionKsbsQueryHandler(standardsServiceMock.Object);

            // Act
            var result = await _sut.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Ksbs.Should().BeEmpty();

            standardsServiceMock.Verify(
                service => service.GetCourseByAnyId("123"),
                Times.Once);
        }
    }
}
