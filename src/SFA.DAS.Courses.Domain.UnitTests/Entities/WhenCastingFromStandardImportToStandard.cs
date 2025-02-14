using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Domain.UnitTests.Entities
{
    public class WhenCastingFromStandardImportToStandard
    {
        [Test, RecursiveMoqAutoData]
        public void Then_Maps_The_Fields(StandardImport standardImport)
        {
            var actual = (Standard) standardImport;
            
            actual.Should().BeEquivalentTo(standardImport, options=> options
                .Excluding(c => c.ApprenticeshipFunding)
                .Excluding(c => c.LarsStandard)
                .Excluding(c => c.Route)
            );
        }

        [Test, RecursiveMoqAutoData]
        public void Then_If_Options_And_OptionsUnstructuredTemplate_Exists_Then_Options_Are_Mapped_To_Options(StandardImport standardImport)
        {
            var actual = (Standard)standardImport;

            actual.Options.Should().BeEquivalentTo(standardImport.Options);
        }
    }
}
