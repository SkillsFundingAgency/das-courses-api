using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Courses.Domain.Courses;

namespace SFA.DAS.Courses.Api.ApiResponses
{
    public class GetFrameworkResponse
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string FrameworkName { get; set; }
        public string PathwayName { get; set; }
        public int ProgType { get; set; }
        public int FrameworkCode { get; set; }
        public int PathwayCode { get; set; }
        public int Level { get; set; }
        public TypicalLengthResponse TypicalLength { get; set; }
        public int Duration { get; set; }
        public int CurrentFundingCap { get; set; }
        public int MaxFunding { get; set; }
        public double Ssa1 { get; set; }
        public double Ssa2 { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime EffectiveTo { get; set; }
        public bool IsActiveFramework { get; set; }
        public IEnumerable<FrameworkFundingResponse> FundingPeriods { get; set; }
        public int ProgrammeType { get; set; }
        public bool HasSubGroups { get; set; }
        public string ExtendedTitle { get; set; }
        
        public static implicit operator GetFrameworkResponse(Framework source)
        {
            if (source == null)
                return null;

            return new GetFrameworkResponse
            {
                Duration = source.Duration,
                Id = source.Id,
                Level = source.Level,
                Ssa1 = source.Ssa1,
                Ssa2 = source.Ssa2,
                Title = source.Title,
                EffectiveFrom = source.EffectiveFrom,
                EffectiveTo = source.EffectiveTo,
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
                TypicalLength = source.TypicalLength,
                FundingPeriods = source.FundingPeriods.Select(c=>(FrameworkFundingResponse)c).ToList()
            };
        }
    }

    public class TypicalLengthResponse
    {
        public string Unit { get ; set ; }

        public int From { get ; set ; }

        public int To { get ; set ; }

        public static implicit operator TypicalLengthResponse(TypicalLength source)
        {
            if (source == null)
                return null;

            return new TypicalLengthResponse
            {
                To = source.To,
                From = source.From,
                Unit = source.Unit
            };
        }
    }

    public class FrameworkFundingResponse
    {
        public int FundingCap { get ; set ; }

        public DateTime? EffectiveTo { get ; set ; }

        public DateTime EffectiveFrom { get ; set ; }

        public static implicit operator FrameworkFundingResponse(FrameworkFunding source)
        {
            if (source == null)
                return null;

            return new FrameworkFundingResponse
            {
                EffectiveFrom = source.EffectiveFrom,
                EffectiveTo = source.EffectiveTo,
                FundingCap = source.FundingCap
            };
        }
    }
}
