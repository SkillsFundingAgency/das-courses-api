﻿using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Courses.Domain.Courses;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Domain.UnitTests.Courses
{
    public class WhenCastingToStandardFromEntity
    {
        [Test, RecursiveMoqAutoData]
        public void Then_Maps_Fields_Appropriately(
            Domain.Entities.Standard source)
        {
            var response = (Standard)source;

            response.Should().BeEquivalentTo(source,options=>options
                .Excluding(c=>c.ApprenticeshipFunding)
                .Excluding(c=>c.LarsStandard)
                .Excluding(c=>c.RouteId)
                .Excluding(x=>x.Sector)
                .Excluding(x=>x.SearchScore)
            );

            response.Route.Should().Be(source.Sector.Route);
            response.ApprenticeshipFunding.Should().NotBeNull();
            response.StandardDates.Should().NotBeNull();
        }
    }
}
