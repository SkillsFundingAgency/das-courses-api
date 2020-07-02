using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using SFA.DAS.Courses.Data;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Api.AcceptanceTests.Infrastructure
{
    public static class DbUtilities
    {
        public static void LoadTestData(CoursesDataContext context)
        {
            var testSectors = GetTestSectors().ToList();
            context.Sectors.AddRange(testSectors);
            context.Standards.AddRange(GetTestStandards(testSectors));
            context.SaveChanges();
        }

        public static void ClearTestData(CoursesDataContext context)
        {
            context.Standards.RemoveRange(context.Standards.ToList());
            context.Sectors.RemoveRange(context.Sectors.ToList());
            context.SaveChanges();
        }

        public static IEnumerable<Sector> GetTestSectors()
        {
            return new List<Sector>
            {
                new Sector { Route = "Engineering and manufacturing" , Id = Guid.NewGuid()},
                new Sector{ Route="Construction" , Id = Guid.NewGuid()}
            };
        }
        
        public static IEnumerable<Standard> GetTestStandards(List<Sector> sectors)
        {
            
            return new List<Standard>
            {
                new Standard{Id = 1, Title = "Head Brewer", Keywords = "Head, Brewer, Beer", OverviewOfRole = "Overseer of brewery operations", Level = 6, RouteId = sectors[0].Id},
                new Standard{Id = 2, Title = "Brewer", Keywords = "Brewer, Beer", OverviewOfRole = "Brewery operations", Level = 4, RouteId = sectors[0].Id},
                new Standard{Id = 3, Title = "Senior / head of facilities management (degree)", Keywords = "Head", OverviewOfRole = "Overseer of brewery operations", Level = 6, RouteId = sectors[1].Id }
            };
        }
    }
}
