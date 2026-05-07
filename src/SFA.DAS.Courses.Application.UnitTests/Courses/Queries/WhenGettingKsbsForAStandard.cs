using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit4;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Application.Courses.Queries.GetStandardOptionKsbs;
using SFA.DAS.Courses.Domain.Courses;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Application.UnitTests.Courses.Queries
{
    public class WhenGettingKsbsForAStandard
    {
        [Test, AutoData]
        public async Task Then_Returns_Ksbs_For_Requested_Option(Standard standard)
        {
            // Arrange
            standard.Options = new List<CourseOption>
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

            var standardServiceMock = new Mock<IStandardsService>();

            standardServiceMock
                .Setup(service => service.GetStandardByAnyId("123"))
                .ReturnsAsync(standard);

            var query = new GetStandardOptionKsbsQuery
            {
                Id = "123",
                Option = "Core"
            };

            var _sut = new GetStandardOptionKsbsQueryHandler(standardServiceMock.Object);

            // Act
            var result = await _sut.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Ksbs.Should().BeEquivalentTo(standard.Options[0].Ksbs);

            standardServiceMock.Verify(
                service => service.GetStandardByAnyId("123"),
                Times.Once);
        }

        [Test, AutoData]
        public async Task Then_Returns_All_Distinct_Ksbs_When_Option_Is_All(Standard standard)
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

            standard.Options = new List<CourseOption>
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

            var standardServiceMock = new Mock<IStandardsService>();

            standardServiceMock
                .Setup(service => service.GetStandardByAnyId("123"))
                .ReturnsAsync(standard);

            var query = new GetStandardOptionKsbsQuery
            {
                Id = "123",
                Option = "all"
            };

            var _sut = new GetStandardOptionKsbsQueryHandler(standardServiceMock.Object);

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

            standardServiceMock.Verify(
                service => service.GetStandardByAnyId("123"),
                Times.Once);
        }

        [Test, AutoData]
        public async Task Then_Returns_Empty_Array_When_Standard_Does_Not_Exist(
            string id,
            string option)
        {
            // Arrange
            var standardServiceMock = new Mock<IStandardsService>();

            standardServiceMock
                .Setup(service => service.GetStandardByAnyId(id))
                .ReturnsAsync((Standard)null);

            var query = new GetStandardOptionKsbsQuery
            {
                Id = id,
                Option = option
            };

            var _sut = new GetStandardOptionKsbsQueryHandler(standardServiceMock.Object);

            // Act
            var result = await _sut.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Ksbs.Should().BeEmpty();

            standardServiceMock.Verify(
                service => service.GetStandardByAnyId(id),
                Times.Once);
        }

        [Test, AutoData]
        public async Task Then_Returns_Empty_Array_When_Option_Does_Not_Exist(Standard standard)
        {
            // Arrange
            standard.Options = new List<CourseOption>
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

            var standardServiceMock = new Mock<IStandardsService>();

            standardServiceMock
                .Setup(service => service.GetStandardByAnyId("123"))
                .ReturnsAsync(standard);

            var query = new GetStandardOptionKsbsQuery
            {
                Id = "123",
                Option = "Not an option"
            };

            var _sut = new GetStandardOptionKsbsQueryHandler(standardServiceMock.Object);

            // Act
            var result = await _sut.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Ksbs.Should().BeEmpty();

            standardServiceMock.Verify(
                service => service.GetStandardByAnyId("123"),
                Times.Once);
        }
    }
}
