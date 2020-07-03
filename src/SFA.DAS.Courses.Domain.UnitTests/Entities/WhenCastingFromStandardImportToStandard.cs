using System;
using AutoFixture.NUnit3;
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
                .Excluding(c=>c.ApprenticeshipFunding)
                .Excluding(c=>c.LarsStandard)
            );
        }
    }
}