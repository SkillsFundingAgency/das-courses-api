using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Courses.Data;
using Sector = SFA.DAS.Courses.Domain.Entities.Sector;
using Standard = SFA.DAS.Courses.Domain.Entities.Standard;
using LarsStandard = SFA.DAS.Courses.Domain.Entities.LarsStandard;

namespace SFA.DAS.Courses.Api.AcceptanceTests.Infrastructure
{
    public static class DbUtilities
    {
        public static void LoadTestData(CoursesDataContext context)
        {
            var testSectors = GetTestSectors().ToList();
            context.Sectors.AddRange(testSectors);
            context.Standards.AddRange(GetValidTestStandards());
            context.Standards.AddRange(GetInValidTestStandards());
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
                new Sector
                {
                    Route = "Engineering and manufacturing",
                    Id = Guid.Parse("54A073E7-F2BA-4C04-9868-1EC3AB3A89D4")
                },
                new Sector
                {
                    Route = "Construction",
                    Id = Guid.Parse("B30D7750-9ADF-41BA-94BD-E4584128EC76")
                }
            };
        }

        public static IEnumerable<Standard> GetValidTestStandards()
        {
            var sectors = GetTestSectors().ToList();
            return new List<Standard>
            {
                new Standard
                {
                    Id = 1,
                    Title = "Head Brewer",
                    Keywords = "Head, Brewer, Beer",
                    TypicalJobTitles = "Overseer of brewery operations",
                    Level = 2,
                    RouteId = sectors[0].Id,
                    Sector = sectors[0], 
                    LarsStandard = new LarsStandard
                    {
                        EffectiveFrom = DateTime.UtcNow.AddDays(-1),
                        LastDateStarts = null,
                        StandardId = 1
                    }
                },
                new Standard
                {
                    Id = 2,
                    Title = "Brewer",
                    Keywords = "Brewer, Beer",
                    TypicalJobTitles = "Brewery operations",
                    Level = 1,
                    RouteId = sectors[0].Id,
                    Sector = sectors[0], 
                    LarsStandard = new LarsStandard
                    {
                        EffectiveFrom = DateTime.UtcNow.AddDays(-1),
                        LastDateStarts = null,
                        StandardId = 2
                    }
                },
                new Standard
                {
                    Id = 3,
                    Title = "Senior / head of facilities management (degree)",
                    Keywords = "Head",
                    TypicalJobTitles = "Overseer of brewery operations",
                    Level = 6,
                    RouteId = sectors[1].Id,
                    Sector = sectors[1], 
                    LarsStandard = new LarsStandard
                    {
                        EffectiveFrom = DateTime.UtcNow.AddDays(-1),
                        LastDateStarts = null,
                        StandardId = 3
                    }
                },
                new Standard
                {
                    Id = 4,
                    Title = "Dentist",
                    Keywords = "Dentist|Dentistry",
                    TypicalJobTitles = "Dentist",
                    Level = 7,
                    RouteId = sectors[1].Id,
                    Sector = sectors[1], 
                    LarsStandard = new LarsStandard
                    {
                        EffectiveFrom = DateTime.UtcNow.AddDays(-1),
                        LastDateStarts = null,
                        StandardId = 4
                    }
                }
            };
        }

        public static IEnumerable<Standard> GetInValidTestStandards()
        {
            var sectors = GetTestSectors().ToList();
            return new List<Standard>
            {
                new Standard
                {
                    Id = 11,
                    Title = "Head Brewer - invalid",
                    Keywords = "Head, Brewer, Beer",
                    TypicalJobTitles = "Overseer of brewery operations",
                    Level = 2,
                    RouteId = sectors[0].Id,
                    Sector = sectors[0], 
                    LarsStandard = new LarsStandard
                    {
                        EffectiveFrom = DateTime.UtcNow.AddDays(1),
                        StandardId = 11
                    }
                },
                new Standard
                {
                    Id = 14,
                    Title = "Dentist - invalid",
                    Keywords = "Dentist|Dentistry",
                    TypicalJobTitles = "Dentist",
                    Level = 7,
                    RouteId = sectors[1].Id,
                    Sector = sectors[1], 
                    LarsStandard = new LarsStandard
                    {
                        EffectiveFrom = DateTime.UtcNow.AddDays(1),
                        StandardId = 14
                    }
                }
            };
        }
    }
}
