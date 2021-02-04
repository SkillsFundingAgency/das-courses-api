﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Application.Courses.Queries.GetStandardsList;
using SFA.DAS.Courses.Domain.Courses;
using SFA.DAS.Courses.Domain.Interfaces;
using SFA.DAS.Courses.Domain.Search;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Application.UnitTests.Courses.Queries
{
    public class WhenGettingAllVersionsOfAStandard
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Standards_From_Service(
            int count,
            GetStandardsByIFateReferenceQuery query,
            List<Standard> standards,
            [Frozen] Mock<IStandardsService> mockStandardsService,
            GetStandardsByIFateReferenceHandler handler)
        {
            mockStandardsService
                .Setup(service => service.GetAllVersionsOfAStandard(query.IFateReferenceNumber))
                .ReturnsAsync(standards);
            mockStandardsService
                .Setup(service => service.Count(StandardFilter.None))
                .ReturnsAsync(count);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Standards.Should().BeEquivalentTo(standards);
            result.Total.Should().Be(count);
            result.TotalFiltered.Should().Be(standards.Count);
        }
    }
}
