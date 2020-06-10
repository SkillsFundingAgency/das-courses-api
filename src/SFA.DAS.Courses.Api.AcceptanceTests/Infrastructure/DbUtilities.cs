using System.Collections.Generic;
using SFA.DAS.Courses.Data;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Api.AcceptanceTests.Infrastructure
{
    public static class DbUtilities
    {
        public static void LoadTestData(CoursesDataContext context)
        {
            context.Standards.AddRange(GetTestStandards());
            context.SaveChanges();
        }

        public static void ClearTestData(CoursesDataContext context)
        {
            context.Standards.RemoveRange(GetTestStandards());
            context.SaveChanges();
        }

        public static IEnumerable<Standard> GetTestStandards()
        {
            return new List<Standard>
            {
                new Standard{Id = 1, Title = "Head Brewer", Keywords = "Head, Brewer, Beer", OverviewOfRole = "Overseer of brewery operations", Level = 6, Route = "Engineering and manufacturing"},
                new Standard{Id = 2, Title = "Brewer", Keywords = "Brewer, Beer", OverviewOfRole = "Brewery operations", Level = 4, Route = "Engineering and manufacturing"},
                new Standard{Id = 3, Title = "Senior / head of facilities management (degree)", Keywords = "Head", OverviewOfRole = "Overseer of brewery operations", Level = 6, Route = "Construction"},
            };
        }
    }
}
