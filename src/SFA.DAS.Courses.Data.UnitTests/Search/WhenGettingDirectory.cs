using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Courses.Data.Search;

namespace SFA.DAS.Courses.Data.UnitTests.Search
{
    public class WhenGettingDirectory
    {
        [Test, AutoData]
        public void Then_Gets_Same_Directory(DirectoryFactory directoryFactory)
        {
            var directory1 = directoryFactory.GetDirectory();
            var directory2 = directoryFactory.GetDirectory();

            directory1.Should().BeSameAs(directory2);
        }
    }
}
