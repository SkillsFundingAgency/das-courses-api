using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Courses.Data;
using SFA.DAS.Courses.Domain.Entities;
using Framework = SFA.DAS.Courses.Domain.Entities.Framework;
using LarsStandard = SFA.DAS.Courses.Domain.Entities.LarsStandard;
using Standard = SFA.DAS.Courses.Domain.Entities.Standard;

namespace SFA.DAS.Courses.Api.AcceptanceTests.Infrastructure
{
    public static class DbUtilities
    {
        public static Dictionary<string, List<Standard>> Standards = [];
        public static List<Route> Routes = [];
        public static List<SectorSubjectAreaTier2> SectorSubjectAreaTier2s = [];

        public static void LoadTestData(CoursesDataContext context)
        {
            LoadTestRoutes();
            LoadSectorSubjectAreaTier2Items();

            context.Routes.AddRange(Routes);
            context.SectorSubjectAreaTier2.AddRange(SectorSubjectAreaTier2s);

            context.Standards.AddRange(LoadValidTestStandards(Routes));
            context.Standards.AddRange(LoadInValidTestStandards(Routes));
            context.Standards.AddRange(LoadNotYetApprovedTestStandards(Routes));
            context.Standards.AddRange(LoadOlderVersionsOfStandards(Routes));
            context.Standards.AddRange(LoadWithdrawnStandards(Routes));

            context.Frameworks.AddRange(GetFrameworks());
            context.SaveChanges();
        }

        private static void LoadSectorSubjectAreaTier2Items()
        {
            if (SectorSubjectAreaTier2s.Count == 0)
            {
                SectorSubjectAreaTier2s.AddRange(
                    new List<SectorSubjectAreaTier2>
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
                    });
            }
        }

        public static IEnumerable<Route> GetTestRoutes()
        {
            return Routes;
        }

        public static void LoadTestRoutes()
        {
            if(Routes.Count == 0)
            {
                Routes.AddRange(
                    new List<Route>
                    {
                        new Route
                        {
                            Name = "Engineering and manufacturing",
                            Id = 1,
                            Active = true
                        },
                        new Route
                        {
                            Name = "Construction",
                            Id = 2,
                            Active = true
                        },
                        new Route
                        {
                            Name = "Creative and design",
                            Id = 3,
                            Active = true
                        }
                    });
            }
        }

        public static IEnumerable<Standard> GetValidTestStandards()
        {
            return Standards[nameof(GetValidTestStandards)];
        }


        private static IEnumerable<Standard> LoadValidTestStandards(IReadOnlyList<Route> routes)
        {
            if (!Standards.ContainsKey(nameof(GetValidTestStandards)))
            {
                Standards.Add(nameof(GetValidTestStandards),
                    new List<Standard>
                    {
                        new Standard
                        {
                            LarsCode = "1",
                            StandardUId = "ST0001_1.3",
                            IfateReferenceNumber = "ST0001",
                            Title = "Head Brewer",
                            Keywords = "Head, Brewer, Beer",
                            TypicalJobTitles = "Overseer of brewery operations",
                            Level = 2,
                            RouteCode = routes[0].Id,
                            Route = routes[0],
                            LarsStandard = new LarsStandard
                            {
                                EffectiveFrom = DateTime.UtcNow.AddDays(-1),
                                LastDateStarts = null,
                                LarsCode = "1",
                                SectorSubjectAreaTier2 = 1m,
                                SectorSubjectAreaTier1 = 1
                            },
                            Status = "Approved for delivery",
                            Version = "1.3",
                            Options = new List<StandardOption>
                            {
                                StandardOption.Create(Guid.NewGuid(), "Beer", [Ksb.Skill(Guid.NewGuid(), 1, "BeerDetail")]),
                                StandardOption.Create(Guid.NewGuid(), "Cider", [Ksb.Skill(Guid.NewGuid(), 1, "CiderDetail")]),
                            },
                            OverviewOfRole = "test",
                            StandardPageUrl = "https://tempuri.org"
                        },
                        new Standard
                        {
                            LarsCode = "2",
                            StandardUId = "ST0002_1.0",
                            IfateReferenceNumber = "ST0002",
                            Title = "Brewer",
                            Keywords = "Brewer, Beer",
                            TypicalJobTitles = "Brewery operations",
                            Level = 1,
                            RouteCode = routes[0].Id,
                            Route = routes[0],
                            LarsStandard = new LarsStandard
                            {
                                EffectiveFrom = DateTime.UtcNow.AddDays(-1),
                                LastDateStarts = null,
                                LarsCode = "2",
                                SectorSubjectAreaTier2 = 1m,
                                SectorSubjectAreaTier1 = 1
                            },
                            Options = new List<StandardOption> { StandardOption.Create("core") },
                            Status = "Approved for delivery",
                            Version = "1.0",
                            OverviewOfRole = "test",
                            StandardPageUrl = "https://tempuri.org"
                        },
                        new Standard
                        {
                            LarsCode = "3",
                            StandardUId = "ST0003_1.0",
                            IfateReferenceNumber = "ST0003",
                            Title = "Senior / head of facilities management (degree)",
                            Keywords = "Head",
                            TypicalJobTitles = "Overseer of brewery operations",
                            Level = 6,
                            RouteCode = routes[1].Id,
                            Route = routes[1],
                            LarsStandard = new LarsStandard
                            {
                                EffectiveFrom = DateTime.UtcNow.AddDays(-1),
                                LastDateStarts = null,
                                LarsCode = "3",
                                SectorSubjectAreaTier2 = 1.1m,
                                SectorSubjectAreaTier1 = 1
                            },
                            Status = "Approved for delivery",
                            Version = "1.0",
                            OverviewOfRole = "test",
                            StandardPageUrl = "https://tempuri.org"
                        },
                        new Standard
                        {
                            LarsCode = "4",
                            StandardUId = "ST0004_1.0",
                            IfateReferenceNumber = "ST0004",
                            Title = "Dentist",
                            Keywords = "Dentist|Dentistry",
                            TypicalJobTitles = "Dentist",
                            Level = 7,
                            RouteCode = routes[1].Id,
                            Route = routes[1],
                            LarsStandard = new LarsStandard
                            {
                                EffectiveFrom = DateTime.UtcNow.AddDays(-1),
                                LastDateStarts = null,
                                LarsCode = "4",
                                SectorSubjectAreaTier2 = 1.1m,
                                SectorSubjectAreaTier1 = 1
                            },
                            Status = "Approved for delivery",
                            Version = "1.0",
                            OverviewOfRole = "test",
                            StandardPageUrl = "https://tempuri.org"
                        },
                        new Standard
                        {
                            LarsCode = "5",
                            StandardUId = "ST0005_1.1",
                            IfateReferenceNumber = "ST0005",
                            Title = "Photographic assistant SortOrder",
                            Keywords = null,
                            TypicalJobTitles = "Assistant Photographer|Photographic Technician",
                            Level = 3,
                            RouteCode = routes[2].Id,
                            Route = routes[2],
                            LarsStandard = new LarsStandard
                            {
                                EffectiveFrom = DateTime.UtcNow.AddDays(-1),
                                LastDateStarts = null,
                                LarsCode = "5",
                                SectorSubjectAreaTier2 = 1m,
                                SectorSubjectAreaTier1 = 1
                            },
                            Status = "Approved for delivery",
                            Version = "1.1",
                            OverviewOfRole = "test",
                            StandardPageUrl = "https://tempuri.org"
                        },
                        new Standard
                        {
                            LarsCode = "6",
                            StandardUId = "ST0006_1.0",
                            IfateReferenceNumber = "ST0006",
                            Title = "Camera prep technician",
                            Keywords = "SortOrder",
                            TypicalJobTitles = "Camera prep technician|Camera equipment technician|",
                            Level = 3,
                            RouteCode = routes[2].Id,
                            Route = routes[2],
                            LarsStandard = new LarsStandard
                            {
                                EffectiveFrom = DateTime.UtcNow.AddDays(-1),
                                LastDateStarts = null,
                                LarsCode = "6",
                                SectorSubjectAreaTier2 = 1.1m,
                                SectorSubjectAreaTier1 = 1
                            },
                            Status = "Approved for delivery",
                            Version = "1.0",
                            OverviewOfRole = "test",
                            StandardPageUrl = "https://tempuri.org"
                        },
                        new Standard
                        {
                            LarsCode = "7",
                            StandardUId = "ST0007_1.0",
                            IfateReferenceNumber = "ST0007",
                            Title = "Junior animator SortOrder",
                            Keywords = "SortOrder",
                            TypicalJobTitles = "Junior animator|SortOrder",
                            Level = 4,
                            RouteCode = routes[2].Id,
                            Route = routes[2],
                            LarsStandard = new LarsStandard
                            {
                                EffectiveFrom = DateTime.UtcNow.AddDays(-1),
                                LastDateStarts = null,
                                LarsCode = "7",
                                SectorSubjectAreaTier2 = 1m,
                                SectorSubjectAreaTier1 = 1
                            },
                            Status = "Approved for delivery",
                            Version = "1.0",
                            OverviewOfRole = "test",
                            StandardPageUrl = "https://tempuri.org"
                        },
                        new Standard
                        {
                            LarsCode = "8",
                            StandardUId = "ST0008_1.0",
                            IfateReferenceNumber = "ST0008",
                            Title = "Standard with option mapped KSBs",
                            Level = 3,
                            RouteCode = routes[2].Id,
                            Route = routes[2],
                            LarsStandard = new LarsStandard
                            {
                                EffectiveFrom = DateTime.UtcNow.AddDays(-1),
                                LarsCode = "8",
                                SectorSubjectAreaTier2 = 1m,
                                SectorSubjectAreaTier1 = 1
                            },
                            Status = "Approved for delivery",
                            Version = "1.0",
                            Options = new List<StandardOption>
                            {
                                StandardOption.Create(
                                    Guid.NewGuid(),
                                    "Option 1",
                                    [
                                        Ksb.Knowledge(Guid.NewGuid(), 1, "core_knowledge_1"),
                                        Ksb.Knowledge(Guid.NewGuid(), 2, "core_knowledge_2"),
                                        Ksb.Knowledge(Guid.NewGuid(), 3, "opt1_knowledge_3"),
                                        Ksb.Skill(Guid.NewGuid(), 1, "core_skill_1"),
                                        Ksb.Behaviour(Guid.NewGuid(), 1, "opt1_behaviour_1"),
                                    ]),
                                StandardOption.Create(
                                    "Option 2")
                            },
                            OverviewOfRole = "test",
                            StandardPageUrl = "https://tempuri.org"
                        }
                    });
            }

            return GetValidTestStandards();
        }

        public static IEnumerable<Standard> GetInValidTestStandards()
        {
            return Standards[nameof(GetInValidTestStandards)];
        }

        private static IEnumerable<Standard> LoadInValidTestStandards(IReadOnlyList<Route> routes)
        {
            if (!Standards.ContainsKey(nameof(GetInValidTestStandards)))
            {
                Standards.Add(nameof(GetInValidTestStandards),
                    new List<Standard>
                    {
                        new Standard
                        {
                            LarsCode = "11",
                            StandardUId = "ST0011_1.0",
                            IfateReferenceNumber = "ST0011",
                            Title = "Structural Engineer - invalid",
                            Keywords = "building, structural, engineer",
                            TypicalJobTitles = "Structural Engineer",
                            Level = 2,
                            RouteCode = routes[0].Id,
                            Route = routes[0],
                            LarsStandard = new LarsStandard
                            {
                                EffectiveFrom = DateTime.UtcNow.AddDays(1),
                                LarsCode = "11",
                                SectorSubjectAreaTier2 = 1m,
                                SectorSubjectAreaTier1 = 1
                            },
                            Status = "Approved for delivery",
                            Version = "1.0",
                            OverviewOfRole = "test",
                            StandardPageUrl = "https://tempuri.org"
                        },
                        new Standard
                        {
                            LarsCode = "14",
                            StandardUId = "ST0014_1.0",
                            IfateReferenceNumber = "ST0014",
                            Title = "Dentist - invalid",
                            Keywords = "Dentist|Dentistry",
                            TypicalJobTitles = "Dentist",
                            Level = 7,
                            RouteCode = routes[1].Id,
                            Route = routes[1],
                            LarsStandard = new LarsStandard
                            {
                                EffectiveFrom = DateTime.UtcNow.AddDays(1),
                                LarsCode = "14",
                                SectorSubjectAreaTier2 = 1m,
                                SectorSubjectAreaTier1 = 1
                            },
                            Status = "Approved for delivery",
                            Version = "1.0",
                            Options = new List<StandardOption>
                            {
                                StandardOption.Create("Cosmetic"),
                                StandardOption.Create("Orthodontist"),
                            },
                            OverviewOfRole = "test",
                            StandardPageUrl = "https://tempuri.org"
                        }
                    });
            }

            return GetInValidTestStandards();
        }

        public static IEnumerable<Standard> GetNotYetApprovedTestStandards()
        {
            return Standards[nameof(GetNotYetApprovedTestStandards)];
        }

        private static IEnumerable<Standard> LoadNotYetApprovedTestStandards(IReadOnlyList<Route> routes)
        {
            if (!Standards.ContainsKey(nameof(GetNotYetApprovedTestStandards)))
            {
                Standards.Add(nameof(GetNotYetApprovedTestStandards),
                    new List<Standard>
                    {
                        new Standard
                        {
                            LarsCode = "0",
                            StandardUId = "ST0015_1.0",
                            IfateReferenceNumber = "ST0015",
                            Title = "Assistant Brewer - Proposal in development",
                            Keywords = "Head, Brewer, Beer",
                            TypicalJobTitles = "Assistant of brewery operations",
                            Level = 1,
                            RouteCode = routes[0].Id,
                            Route = routes[0],
                            LarsStandard = null,
                            Status = "Proposal in development",
                            Version = "1.1",
                            OverviewOfRole = "test",
                            StandardPageUrl = "https://tempuri.org"
                        },
                         new Standard
                        {
                            LarsCode = "0",
                            StandardUId = "ST0016_1.0",
                            IfateReferenceNumber = "ST0016",
                            Title = "Metallurgy Engineer - In development",
                            Keywords = "Metallurgy, Engineer, Metal",
                            TypicalJobTitles = "Metallurgy Engineer",
                            Level = 4,
                            RouteCode = routes[0].Id,
                            Route = routes[0],
                            LarsStandard = null,
                            Status = "In development",
                            Version = "1.0",
                            Options = new List<StandardOption>
                            {
                                StandardOption.Create("Ferrous"),
                                StandardOption.Create("Non-ferroes"),
                            },
                            OverviewOfRole = "test",
                            StandardPageUrl = "https://tempuri.org"
                        }
                    });
            }

            return GetNotYetApprovedTestStandards();
        }

        public static IEnumerable<Standard> GetWithdrawnStandards()
        {
            return Standards[nameof(GetWithdrawnStandards)];
        }

        private static IEnumerable<Standard> LoadWithdrawnStandards(IReadOnlyList<Route> routes)
        {
            if (!Standards.ContainsKey(nameof(GetWithdrawnStandards)))
            {
                Standards.Add(nameof(GetWithdrawnStandards),
                    new List<Standard>
                    {
                        new Standard
                        {
                            LarsCode = "0",
                            StandardUId = "ST0030_1.0",
                            IfateReferenceNumber = "ST0030",
                            Title = "Assistant Brewer - Withdrawn",
                            Keywords = "Head, Brewer, Beer",
                            TypicalJobTitles = "Assistant of brewery operations",
                            Level = 1,
                            RouteCode = routes[0].Id,
                            Route = routes[0],
                            LarsStandard = null,
                            Status = "Withdrawn",
                            Version = "1.0",
                            Options = new List<StandardOption>
                            {
                                StandardOption.Create("Wine"),
                                StandardOption.Create("Spirits"),
                            },
                            OverviewOfRole = "test",
                            StandardPageUrl = "https://tempuri.org"
                        }
                    });
            }

            return GetWithdrawnStandards();
        }

        public static IEnumerable<Standard> GetOlderVersionsOfStandards()
        {
            return Standards[nameof(GetOlderVersionsOfStandards)];
        }

        private static IEnumerable<Standard> LoadOlderVersionsOfStandards(IReadOnlyList<Route> routes)
        {
            if (!Standards.ContainsKey(nameof(GetOlderVersionsOfStandards)))
            {
                Standards.Add(nameof(GetOlderVersionsOfStandards),
                    new List<Standard>
                    {
                        new Standard
                        {
                            LarsCode = "1",
                            StandardUId = "ST0001_1.2",
                            IfateReferenceNumber = "ST0001",
                            Title = "Head Brewer",
                            Keywords = "Head, Brewer, Beer",
                            TypicalJobTitles = "Overseer of brewery operations",
                            Level = 2,
                            RouteCode = routes[0].Id,
                            Route = routes[0],
                            Status = "Retired",
                            Version = "1.2",
                            Options = new List<StandardOption>
                            {
                                StandardOption.Create("Beer"),
                                StandardOption.Create("Cider"),
                            },
                            OverviewOfRole = "test",
                            StandardPageUrl = "https://tempuri.org"
                        },
                        new Standard
                        {
                            LarsCode = "1",
                            StandardUId = "ST0001_1.1",
                            IfateReferenceNumber = "ST0001",
                            Title = "Head Brewer",
                            Keywords = "Head, Brewer, Beer",
                            TypicalJobTitles = "Overseer of brewery operations",
                            Level = 2,
                            RouteCode = routes[0].Id,
                            Route = routes[0],
                            Status = "Retired",
                            Version = "1.1",
                            OverviewOfRole = "test",
                            StandardPageUrl = "https://tempuri.org"
                        },
                        new Standard
                        {
                            LarsCode = "5",
                            StandardUId = "ST0005_1.0",
                            IfateReferenceNumber = "ST0005",
                            Title = "Photographic assistant SortOrder",
                            Keywords = null,
                            TypicalJobTitles = "Assistant Photographer|Photographic Technician",
                            Level = 3,
                            RouteCode = routes[2].Id,
                            Route = routes[2],
                            Status = "Retired",
                            Version = "1.0",
                            Options = new List<StandardOption>
                            {
                                StandardOption.Create("Studio"),
                                StandardOption.Create("Landscape"),
                            },
                            OverviewOfRole = "test",
                            StandardPageUrl = "https://tempuri.org"
                        },
                        new Standard
                        {
                            LarsCode = "99",
                            StandardUId = "ST0099_1.0",
                            IfateReferenceNumber = "ST0099",
                            Title = "Photographic assistant",
                            Keywords = null,
                            TypicalJobTitles = "Assistant Photographer|Photographic Technician",
                            Level = 3,
                            RouteCode = routes[2].Id,
                            Route = routes[2],
                            Status = "Retired",
                            Version = "1.0",
                            Options = new List<StandardOption>
                            {
                                StandardOption.Create("Studio"),
                                StandardOption.Create("Landscape"),
                            },
                            OverviewOfRole = "test",
                            StandardPageUrl = "https://tempuri.org"
                        }
                    });
            }

            return GetOlderVersionsOfStandards();
        }

        public static IEnumerable<Standard> GetAllTestStandards()
        {
            return Standards.Values.SelectMany(p => p);
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
