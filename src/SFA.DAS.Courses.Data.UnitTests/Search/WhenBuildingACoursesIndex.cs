using System.Collections.Generic;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Data.Search;
using SFA.DAS.Courses.Data.UnitTests.DatabaseMock;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Data.UnitTests.Search
{
    public class WhenBuildingACoursesIndex
    {
        [Test, MoqAutoData]
        public void Then_Adds_Doc_For_Each_Standard(
            List<Standard> standards,
            [Frozen] Mock<ICoursesDataContext> mockDataContext,
            DirectoryFactory directoryFactory)
        {
            mockDataContext
                .Setup(context => context.Standards)
                .ReturnsDbSet(standards);
            var builder = new CoursesIndexBuilder(mockDataContext.Object, directoryFactory);

            builder.Build();

            var directory = directoryFactory.GetDirectory();
            var files = directory.ListAll();
            files.Length.Should().NotBe(0);
        }
    }
}
