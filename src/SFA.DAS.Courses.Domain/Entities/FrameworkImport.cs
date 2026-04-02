namespace SFA.DAS.Courses.Domain.Entities
{
    public class FrameworkImport : FrameworkBase
    {
        public static implicit operator FrameworkImport(ImportTypes.Framework source)
        {
            if (source == null)
                return null;

            return new FrameworkImport
            {
                Id = source.Id,
                Duration = source.Duration,
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
                TypicalLengthFrom = source.TypicalLength.From,
                TypicalLengthTo = source.TypicalLength.To,
                TypicalLengthUnit = source.TypicalLength.Unit
            };
        }
    }
}
