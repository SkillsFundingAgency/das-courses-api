using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Application.Courses.Queries.GetStandardOptionKsbs;
using SFA.DAS.Courses.Domain.Courses;
using SFA.DAS.Courses.Domain.Interfaces;

using CourseType = SFA.DAS.Courses.Domain.Entities.CourseType;

namespace SFA.DAS.Courses.Application.UnitTests.Courses.Queries
{
    public class WhenGettingKsbsForAStandard
    {
        [Test, AutoData]
        public async Task Then_Returns_Correct_Ksbs_Type(Standard standard)
        {
            // Arrange
            standard.Options = new List<StandardOption>
        {
            new StandardOption
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
            }
        };
            Mock<IStandardsService> standardServiceMock = new();
            standardServiceMock.Setup(x => x.GetStandardByAnyId(It.IsAny<string>(), CourseType.Apprenticeship)).ReturnsAsync(standard);
            var query = new GetStandardOptionKsbsQuery
            {
                Id = "123",
                Option = "Core"
            };
            var handler = new GetStandardOptionKsbsQueryHandler(standardServiceMock.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Ksbs.Should().BeEquivalentTo(standard.Options[0].Ksbs);
        }
    }
}
