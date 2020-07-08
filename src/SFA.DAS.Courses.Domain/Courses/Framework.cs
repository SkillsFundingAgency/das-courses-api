using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.Courses.Domain.Courses
{
    public class Framework
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string FrameworkName { get; set; }
        public string PathwayName { get; set; }
        public int ProgType { get; set; }
        public int FrameworkCode { get; set; }
        public int PathwayCode { get; set; }
        public int Level { get; set; }
        public TypicalLength TypicalLength { get; set; }
        public int Duration { get; set; }
        public int CurrentFundingCap { get; set; }
        public int MaxFunding { get; set; }
        public double Ssa1 { get; set; }
        public double Ssa2 { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime EffectiveTo { get; set; }
        public bool IsActiveFramework { get; set; }
        public IEnumerable<FrameworkFunding> FundingPeriods { get; set; }
        public int ProgrammeType { get; set; }
        public bool HasSubGroups { get; set; }
        public string ExtendedTitle { get; set; }

        public static implicit operator Framework(Domain.Entities.Framework framework)
        {
            return new Framework
            {
                Id = framework.Id,
                Duration = framework.Duration,
                Level = framework.Level,
                Ssa1 = framework.Ssa1,
                Ssa2 = framework.Ssa2,
                Title = framework.Title,
                EffectiveFrom = framework.EffectiveFrom,
                EffectiveTo = framework.EffectiveTo,
                TypicalLength = new TypicalLength
                {
                    From=framework.TypicalLengthFrom,
                    To = framework.TypicalLengthTo,
                    Unit = framework.TypicalLengthUnit
                },
                ExtendedTitle = framework.ExtendedTitle,
                FrameworkCode = framework.FrameworkCode,
                FrameworkName = framework.FrameworkName,
                MaxFunding = framework.MaxFunding,
                PathwayCode = framework.PathwayCode,
                PathwayName = framework.PathwayName,
                ProgrammeType = framework.ProgrammeType,
                ProgType = framework.ProgType,
                CurrentFundingCap = framework.CurrentFundingCap,
                HasSubGroups = framework.HasSubGroups,
                IsActiveFramework = framework.IsActiveFramework,
                FundingPeriods = framework.FundingPeriods.Select(c=>(FrameworkFunding)c).ToList()
            };
        }
    }

    public class FrameworkFunding
    {
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public int FundingCap { get; set; }

        public static implicit operator FrameworkFunding(Domain.Entities.FrameworkFunding frameworkFunding)
        {
            return new FrameworkFunding
            {
                EffectiveFrom = frameworkFunding.EffectiveFrom,
                EffectiveTo = frameworkFunding.EffectiveTo,
                FundingCap = frameworkFunding.FundingCap
            }; 
                
        }
    }

    public class TypicalLength
    {
        public int From { get; set; }
        public int To { get; set; }
        public string Unit { get; set; }
    }
}