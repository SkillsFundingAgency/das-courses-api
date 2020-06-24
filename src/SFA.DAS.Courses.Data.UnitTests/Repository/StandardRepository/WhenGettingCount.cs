using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Data.UnitTests.DatabaseMock;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Data.UnitTests.Repository.StandardRepository
{
    public class WhenGettingCount
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Count_From_Context(
            List<Standard> standards,
            [Frozen] Mock<ICoursesDataContext> mockDataContext,
            Data.Repository.StandardRepository repository)
        {
            mockDataContext
                .Setup(context => context.Standards)
                .ReturnsDbSet(standards);

            var count = await repository.Count();

            count.Should().Be(standards.Count);
        }
    }
}
