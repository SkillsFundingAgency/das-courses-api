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

        public static implicit operator Framework(Entities.Framework source)
        {
            if (source == null)
                return null;

            return new Framework
            {
                Id = source.Id,
                Duration = source.Duration,
                Level = source.Level,
                Ssa1 = source.Ssa1,
                Ssa2 = source.Ssa2,
                Title = source.Title,
                EffectiveFrom = source.EffectiveFrom,
                EffectiveTo = source.EffectiveTo,
                TypicalLength = new TypicalLength
                {
                    From=source.TypicalLengthFrom,
                    To = source.TypicalLengthTo,
                    Unit = source.TypicalLengthUnit
                },
                ExtendedTitle = source.ExtendedTitle,
                FrameworkCode = source.FrameworkCode,
                FrameworkName = source.FrameworkName,
                MaxFunding = source.MaxFunding,
                PathwayCode = source.PathwayCode,
                PathwayName = source.PathwayName,
                ProgrammeType = source.ProgrammeType,
                ProgType = source.ProgType,
                CurrentFundingCap = source.CurrentFundingCap,
                HasSubGroups = source.HasSubGroups,
                IsActiveFramework = source.IsActiveFramework,
                FundingPeriods = source.FundingPeriods.Select(c=>(FrameworkFunding)c).ToList()
            };
        }
    }

    public class FrameworkFunding
    {
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public int FundingCap { get; set; }

        public static implicit operator FrameworkFunding(Entities.FrameworkFunding source)
        {
            if (source == null)
                return null;

            return new FrameworkFunding
            {
                EffectiveFrom = source.EffectiveFrom,
                EffectiveTo = source.EffectiveTo,
                FundingCap = source.FundingCap
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
