using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Domain.UnitTests.Entities
{
    public class WhenCastingFromStandardImportToStandard
    {
        [Test, AutoData]
        public void Then_Maps_The_Fields(StandardImport standardImport)
        {
            var actual = (Standard) standardImport;
            
            actual.Should().BeEquivalentTo(standardImport);
        }
    }
}