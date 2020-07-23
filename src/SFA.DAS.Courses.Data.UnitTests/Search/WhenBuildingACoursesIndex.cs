using System.Collections.Generic;
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
        [Test, RecursiveMoqAutoData]
        public void Then_Adds_Doc_For_Each_Standard(
            List<Standard> standards,
            Mock<ICoursesDataContext> mockDataContext,
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

        [Test, RecursiveMoqAutoData]
        public void And_Called_Multiple_Times_Then_No_Duplicates(
            List<Standard> standards,
            Mock<ICoursesDataContext> mockDataContext,
            DirectoryFactory directoryFactory)
        {
            var directory = directoryFactory.GetDirectory();
            mockDataContext
                .Setup(context => context.Standards)
                .ReturnsDbSet(standards);
            var builder = new CoursesIndexBuilder(mockDataContext.Object, directoryFactory);

            // act
            builder.Build();
            var firstFiles = directory.ListAll();
            builder.Build();
            var secondFiles = directory.ListAll();

            firstFiles.Length.Should().NotBe(0);
            secondFiles.Length.Should().Be(firstFiles.Length);
        }
    }
}
