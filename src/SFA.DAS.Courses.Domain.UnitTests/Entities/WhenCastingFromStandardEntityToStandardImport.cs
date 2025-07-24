using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Domain.UnitTests.Entities;
public class WhenCastingFromStandardEntityToStandardImport
{
    [Test, RecursiveMoqAutoData]
    public void Then_StandardEntity_Can_Be_Cast_To_StandardImport(
        Standard standardEntity)
    {
        // Act
        StandardImport standardImport = standardEntity;
        // Assert
        standardImport.Should().BeEquivalentTo(standardEntity, options => options
            .Excluding(s => s.StandardUId)
            .Excluding(s => s.LarsStandard)
            .Excluding(s => s.ApprenticeshipFunding)
            .Excluding(s => s.SearchScore));
    }
}
