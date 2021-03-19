using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Courses.Data;
using SFA.DAS.Courses.Domain.Entities;
using Sector = SFA.DAS.Courses.Domain.Entities.Sector;
using Standard = SFA.DAS.Courses.Domain.Entities.Standard;
using LarsStandard = SFA.DAS.Courses.Domain.Entities.LarsStandard;
using Framework = SFA.DAS.Courses.Domain.Entities.Framework;

namespace SFA.DAS.Courses.Api.AcceptanceTests.Infrastructure
{
    public static class DbUtilities
    {
        public static void LoadTestData(CoursesDataContext context)
        {
            var testSectors = GetTestSectors().ToList();
            context.Sectors.AddRange(testSectors);
            context.Routes.AddRange(GetTestRoutes().ToList());
            context.SectorSubjectAreaTier2.AddRange(GetSectorSubjectAreaTier2Items());
            context.Standards.AddRange(GetValidTestStandards());
            context.Standards.AddRange(GetInValidTestStandards());
            context.Standards.AddRange(GetNotYetApprovedTestStandards());
            context.Standards.AddRange(GetOlderVersionsOfStandards());
            context.Standards.AddRange(GetWithdrawnStandards());
            context.Frameworks.AddRange(GetFrameworks());
            context.SaveChanges();
        }

        public static void ClearTestData(CoursesDataContext context)
        {
            context.Standards.RemoveRange(context.Standards.ToList());
            context.Sectors.RemoveRange(context.Sectors.ToList());
            context.SectorSubjectAreaTier2.RemoveRange(context.SectorSubjectAreaTier2.ToList());
            context.Frameworks.RemoveRange(context.Frameworks.ToList());
            context.SaveChanges();
        }

        public static IEnumerable<SectorSubjectAreaTier2> GetSectorSubjectAreaTier2Items()
        {
            return new List<SectorSubjectAreaTier2>
            {
                new SectorSubjectAreaTier2
                {
                    EffectiveFrom = DateTime.Today,
                    SectorSubjectAreaTier2Desc = "Test Sector",
                    SectorSubjectAreaTier2 = 1m,
                    Name = "Test Sector"
                },
                new SectorSubjectAreaTier2
                {
                    EffectiveFrom = DateTime.Today,
                    SectorSubjectAreaTier2Desc = "Test Sector 2",
                    SectorSubjectAreaTier2 = 1.1m,
                    Name = "Test Sector 2"
                }
            };
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
                },
                new Sector
                {
                    Route = "Creative and design",
                    Id = Guid.Parse("7ECB9D82-47A0-45EB-94FE-2F9E2161A55F")
                }
            };
        }
        public static IEnumerable<Route> GetTestRoutes()
        {
            return new List<Route>
            {
                new Route
                {
                    Name = "Engineering and manufacturing",
                    Id = 1
                },
                new Route
                {
                    Name = "Construction",
                    Id = 2
                },
                new Route
                {
                    Name = "Creative and design",
                    Id = 3
                }
            };
        }

        public static IEnumerable<Standard> GetValidTestStandards()
        {
            var sectors = GetTestSectors().ToList();
            var routes = GetTestRoutes().ToList();
            var subjectSectorArea = GetSectorSubjectAreaTier2Items().ToList();
            return new List<Standard>
            {
                new Standard
                {
                    LarsCode = 1,
                    StandardUId = "ST0001_1.3",
                    IfateReferenceNumber = "ST0001",
                    Title = "Head Brewer",
                    Keywords = "Head, Brewer, Beer",
                    TypicalJobTitles = "Overseer of brewery operations",
                    Level = 2,
                    RouteId = sectors[0].Id,
                    RouteCode = routes[0].Id,
                    LarsStandard = new LarsStandard
                    {
                        EffectiveFrom = DateTime.UtcNow.AddDays(-1),
                        LastDateStarts = null,
                        LarsCode = 1,
                        SectorSubjectAreaTier2 = 1m
                    },
                    Status = "Approved for delivery",
                    Version = 1.3m,
                    Options = new List<string>
                    {
                        "Beer",
                        "Cider"
                    }
                },
                new Standard
                {
                    LarsCode = 2,
                    StandardUId = "ST0002_1.0",
                    IfateReferenceNumber = "ST0002",
                    Title = "Brewer",
                    Keywords = "Brewer, Beer",
                    TypicalJobTitles = "Brewery operations",
                    Level = 1,
                    RouteId = sectors[0].Id,
                    RouteCode = routes[0].Id,
                    LarsStandard = new LarsStandard
                    {
                        EffectiveFrom = DateTime.UtcNow.AddDays(-1),
                        LastDateStarts = null,
                        LarsCode = 2,
                        SectorSubjectAreaTier2 = 1m
                    },
                    Status = "Approved for delivery",
                    Version = 1.0m
                },
                new Standard
                {
                    LarsCode = 3,
                    StandardUId = "ST0003_1.0",
                    IfateReferenceNumber = "ST0003",
                    Title = "Senior / head of facilities management (degree)",
                    Keywords = "Head",
                    TypicalJobTitles = "Overseer of brewery operations",
                    Level = 6,
                    RouteId = sectors[1].Id,
                    RouteCode = routes[1].Id,
                    LarsStandard = new LarsStandard
                    {
                        EffectiveFrom = DateTime.UtcNow.AddDays(-1),
                        LastDateStarts = null,
                        LarsCode = 3,
                        SectorSubjectAreaTier2 = 1.1m
                    },
                    Status = "Approved for delivery",
                    Version = 1.0m
                },
                new Standard
                {
                    LarsCode = 4,
                    StandardUId = "ST0004_1.0",
                    IfateReferenceNumber = "ST0004",
                    Title = "Dentist",
                    Keywords = "Dentist|Dentistry",
                    TypicalJobTitles = "Dentist",
                    Level = 7,
                    RouteId = sectors[1].Id,
                    RouteCode = routes[1].Id,
                    LarsStandard = new LarsStandard
                    {
                        EffectiveFrom = DateTime.UtcNow.AddDays(-1),
                        LastDateStarts = null,
                        LarsCode = 4,
                        SectorSubjectAreaTier2 = 1.1m
                    },
                    Status = "Approved for delivery",
                    Version = 1.0m
                },
                new Standard
                {
                    LarsCode = 5,
                    StandardUId = "ST0005_1.1",
                    IfateReferenceNumber = "ST0005",
                    Title = "Photographic assistant SortOrder",
                    Keywords = null,
                    TypicalJobTitles = "Assistant Photographer|Photographic Technician",
                    Level = 3,
                    RouteId = sectors[2].Id,
                    RouteCode = routes[2].Id,
                    LarsStandard = new LarsStandard
                    {
                        EffectiveFrom = DateTime.UtcNow.AddDays(-1),
                        LastDateStarts = null,
                        LarsCode = 5,
                        SectorSubjectAreaTier2 = 1m
                    },
                    Status = "Approved for delivery",
                    Version = 1.1m
                },
                new Standard
                {
                    LarsCode = 6,
                    StandardUId = "ST0006_1.0",
                    IfateReferenceNumber = "ST0006",
                    Title = "Camera prep technician",
                    Keywords = "SortOrder",
                    TypicalJobTitles = "Camera prep technician|Camera equipment technician|",
                    Level = 3,
                    RouteId = sectors[2].Id,
                    RouteCode = routes[2].Id,
                    LarsStandard = new LarsStandard
                    {
                        EffectiveFrom = DateTime.UtcNow.AddDays(-1),
                        LastDateStarts = null,
                        LarsCode = 6,
                        SectorSubjectAreaTier2 = 1.1m
                    },
                    Status = "Approved for delivery",
                    Version = 1.0m
                },
                new Standard
                {
                    LarsCode = 7,
                    StandardUId = "ST0007_1.0",
                    IfateReferenceNumber = "ST0007",
                    Title = "Junior animator SortOrder",
                    Keywords = "SortOrder",
                    TypicalJobTitles = "Junior animator|SortOrder",
                    Level = 4,
                    RouteId = sectors[2].Id,
                    RouteCode = routes[2].Id,
                    LarsStandard = new LarsStandard
                    {
                        EffectiveFrom = DateTime.UtcNow.AddDays(-1),
                        LastDateStarts = null,
                        LarsCode = 7,
                        SectorSubjectAreaTier2 = 1m
                    },
                    Status = "Approved for delivery",
                    Version = 1.0m
                }
            };
        }

        public static IEnumerable<Standard> GetInValidTestStandards()
        {
            var sectors = GetTestSectors().ToList();
            var routes = GetTestRoutes().ToList();
            return new List<Standard>
            {
                new Standard
                {
                    LarsCode = 11,
                    StandardUId = "ST0011_1.0",
                    IfateReferenceNumber = "ST0011",
                    Title = "Structural Engineer - invalid",
                    Keywords = "building, structural, engineer",
                    TypicalJobTitles = "Structural Engineer",
                    Level = 2,
                    RouteId = sectors[0].Id,
                    RouteCode = routes[0].Id,
                    LarsStandard = new LarsStandard
                    {
                        EffectiveFrom = DateTime.UtcNow.AddDays(1),
                        LarsCode = 11,
                        SectorSubjectAreaTier2 = 1m
                    },
                    Status = "Approved for delivery",
                    Version = 1.0m
                },
                new Standard
                {
                    LarsCode = 14,
                    StandardUId = "ST0014_1.0",
                    IfateReferenceNumber = "ST0014",
                    Title = "Dentist - invalid",
                    Keywords = "Dentist|Dentistry",
                    TypicalJobTitles = "Dentist",
                    Level = 7,
                    RouteId = sectors[1].Id,
                    RouteCode = routes[1].Id,
                    LarsStandard = new LarsStandard
                    {
                        EffectiveFrom = DateTime.UtcNow.AddDays(1),
                        LarsCode = 14,
                        SectorSubjectAreaTier2 = 1m
                    },
                    Status = "Approved for delivery",
                    Version = 1.0m,
                    Options = new List<string>
                    {
                        "Cosmetic",
                        "Orthodontist"
                    }
                }
            };
        }

        public static IEnumerable<Standard> GetNotYetApprovedTestStandards()
        {
            var sectors = GetTestSectors().ToList();
            var routes = GetTestRoutes().ToList();
            return new List<Standard>
            {
                new Standard
                {
                    LarsCode = 0,
                    StandardUId = "ST0015_1.0",
                    IfateReferenceNumber = "ST0015",
                    Title = "Assistant Brewer - Proposal in development",
                    Keywords = "Head, Brewer, Beer",
                    TypicalJobTitles = "Assistant of brewery operations",
                    Level = 1,
                    RouteId = sectors[0].Id,
                    RouteCode = routes[0].Id,
                    LarsStandard = null,
                    Status = "Proposal in development",
                    Version = 1.1m
                },
                 new Standard
                {
                    LarsCode = 0,
                    StandardUId = "ST0016_1.0",
                    IfateReferenceNumber = "ST0016",
                    Title = "Metallurgy Engineer - In development",
                    Keywords = "Metallurgy, Engineer, Metal",
                    TypicalJobTitles = "Metallurgy Engineer",
                    Level = 4,
                    RouteId = sectors[0].Id,
                    RouteCode = routes[0].Id,
                    LarsStandard = null,
                    Status = "In development",
                    Version = 1.0m,
                    Options = new List<string>
                    {
                        "Ferrous",
                        "Non-ferroes"
                    }
                }
            };
        }

        public static IEnumerable<Standard> GetWithdrawnStandards()
        {
            var sectors = GetTestSectors().ToList();
            var routes = GetTestRoutes().ToList();
            return new List<Standard>
            {
                new Standard
                {
                    LarsCode = 0,
                    StandardUId = "ST0030_1.0",
                    IfateReferenceNumber = "ST0030",
                    Title = "Assistant Brewer - Withdrawn",
                    Keywords = "Head, Brewer, Beer",
                    TypicalJobTitles = "Assistant of brewery operations",
                    Level = 1,
                    RouteId = sectors[0].Id,
                    RouteCode = routes[0].Id,
                    LarsStandard = null,
                    Status = "Withdrawn",
                    Version = 1.0m,
                    Options = new List<string>
                    {
                        "Wine",
                        "Spirits"
                    }
                }
            };
        }
        public static IEnumerable<Standard> GetOlderVersionsOfStandards()
        {
            var sectors = GetTestSectors().ToList();
            var routes = GetTestRoutes().ToList();
            return new List<Standard>
            {
                new Standard
                {
                    LarsCode = 1,
                    StandardUId = "ST0001_1.2",
                    IfateReferenceNumber = "ST0001",
                    Title = "Head Brewer",
                    Keywords = "Head, Brewer, Beer",
                    TypicalJobTitles = "Overseer of brewery operations",
                    Level = 2,
                    RouteId = sectors[0].Id,
                    RouteCode = routes[0].Id,
                    Status = "Retired",
                    Version = 1.2m,
                    Options = new List<string>
                    {
                        "Beer",
                        "Cider"
                    }
                },
                new Standard
                {
                    LarsCode = 1,
                    StandardUId = "ST0001_1.1",
                    IfateReferenceNumber = "ST0001",
                    Title = "Head Brewer",
                    Keywords = "Head, Brewer, Beer",
                    TypicalJobTitles = "Overseer of brewery operations",
                    Level = 2,
                    RouteId = sectors[0].Id,
                    RouteCode = routes[0].Id,
                    Status = "Retired",
                    Version = 1.1m
                },
                new Standard
                {
                    LarsCode = 5,
                    StandardUId = "ST0005_1.0",
                    IfateReferenceNumber = "ST0005",
                    Title = "Photographic assistant SortOrder",
                    Keywords = null,
                    TypicalJobTitles = "Assistant Photographer|Photographic Technician",
                    Level = 3,
                    RouteId = sectors[2].Id,
                    RouteCode = routes[2].Id,
                    Status = "Retired",
                    Version = 1.0m,
                    Options = new List<string>
                    {
                        "Studio",
                        "Landscape"
                    }
                },
                new Standard
                {
                    LarsCode = 99,
                    StandardUId = "ST0099_1.0",
                    IfateReferenceNumber = "ST0099",
                    Title = "Photographic assistant",
                    Keywords = null,
                    TypicalJobTitles = "Assistant Photographer|Photographic Technician",
                    Level = 3,
                    RouteId = sectors[2].Id,
                    Status = "Retired",
                    Version = 1.0m,
                    Options = new List<string>
                    {
                        "Studio",
                        "Landscape"
                    }
                }
            };
        }

        public static IEnumerable<Standard> GetAllTestStandards()
        {
            var combinedStandards = new List<Standard>();

            combinedStandards.AddRange(GetValidTestStandards());
            combinedStandards.AddRange(GetInValidTestStandards());
            combinedStandards.AddRange(GetNotYetApprovedTestStandards());
            combinedStandards.AddRange(GetOlderVersionsOfStandards());
            combinedStandards.AddRange(GetWithdrawnStandards());

            return combinedStandards;
        }

        public static IEnumerable<Framework> GetFrameworks()
        {
            return new List<Framework>
            {
                new Framework
                {
                    Duration = 2,
                    Id = "1",
                    Level = 2,
                    Title = "Test framework 1",
                    EffectiveFrom = new DateTime(2020, 01, 01),
                    EffectiveTo = new DateTime(2025, 01, 01),
                    ExtendedTitle = "This is the test framework 1",
                    FrameworkCode = 20,
                    FrameworkName = "Framework test 1",
                    MaxFunding = 10000,
                    PathwayCode = 2,
                    PathwayName = "Test pathway 1",
                    ProgrammeType = 2,
                    ProgType = 2,
                    CurrentFundingCap = 20000,
                    HasSubGroups = false,
                    IsActiveFramework = true,
                    TypicalLengthFrom = 2,
                    TypicalLengthTo = 3,
                    TypicalLengthUnit = "Years",
                },
                new Framework
                {
                    Id = "2",
                    Level = 3,
                    Title = "Test framework 2",
                    EffectiveFrom = new DateTime(2021, 01, 01),
                    EffectiveTo = new DateTime(2022, 01, 01),
                    ExtendedTitle = "This is the test framework 2",
                    FrameworkCode = 3,
                    FrameworkName = "Framework test 2",
                    MaxFunding = 15000,
                    PathwayCode = 1,
                    PathwayName = "Test pathway 2",
                    ProgrammeType = 4,
                    ProgType = 4,
                    CurrentFundingCap = 18000,
                    HasSubGroups = true,
                    IsActiveFramework = true,
                    TypicalLengthFrom = 3,
                    TypicalLengthTo = 4,
                    TypicalLengthUnit = "Years",
                },
                new Framework
                {
                    Id = "3",
                    Level = 4,
                    Title = "Test framework 3",
                    EffectiveFrom = new DateTime(2015, 01, 01),
                    EffectiveTo = new DateTime(2019, 01, 01),
                    ExtendedTitle = "This is the test framework 3",
                    FrameworkCode = 5,
                    FrameworkName = "Framework test 3",
                    MaxFunding = 8000,
                    PathwayCode = 7,
                    PathwayName = "Test pathway 3",
                    ProgrammeType = 5,
                    ProgType = 5,
                    CurrentFundingCap = 2000,
                    HasSubGroups = false,
                    IsActiveFramework = false,
                    TypicalLengthFrom = 1,
                    TypicalLengthTo = 2,
                    TypicalLengthUnit = "Years",
                },
            };
        }

        public static Framework GetFramework(string id)
        {
            return GetFrameworks().FirstOrDefault(c => c.Id.Equals(id, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}
