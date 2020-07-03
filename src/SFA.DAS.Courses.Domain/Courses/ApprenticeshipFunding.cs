using System;

namespace SFA.DAS.Courses.Domain.Courses
{
    public class ApprenticeshipFunding
    {
        public DateTime EffectiveFrom { get ; set ; }

        public DateTime? EffectiveTo { get ; set ; }

        public int MaxEmployerLevyCap { get ; set ; }

        public static implicit operator ApprenticeshipFunding(
            Domain.Entities.ApprenticeshipFunding apprenticeshipFunding)
        {
            return new ApprenticeshipFunding
            {
                EffectiveFrom = apprenticeshipFunding.EffectiveFrom,
                EffectiveTo = apprenticeshipFunding.EffectiveTo,
                MaxEmployerLevyCap = apprenticeshipFunding.MaxEmployerLevyCap
            };  
        }
    }
}