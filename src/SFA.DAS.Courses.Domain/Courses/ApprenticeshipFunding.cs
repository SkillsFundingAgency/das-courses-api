using System;

namespace SFA.DAS.Courses.Domain.Courses
{
    public class ApprenticeshipFunding
    {
        public DateTime EffectiveFrom { get ; set ; }

        public DateTime? EffectiveTo { get ; set ; }

        public int MaxEmployerLevyCap { get ; set ; }
        public int Duration { get; set; }

        public static explicit operator ApprenticeshipFunding(Domain.Entities.ApprenticeshipFunding apprenticeshipFunding)
        {
            if(apprenticeshipFunding == null)
            {
                return null;
            }

            return new ApprenticeshipFunding
            {
                EffectiveFrom = apprenticeshipFunding.EffectiveFrom,
                EffectiveTo = apprenticeshipFunding.EffectiveTo,
                MaxEmployerLevyCap = apprenticeshipFunding.MaxEmployerLevyCap,
                Duration = apprenticeshipFunding.Duration
            };  
        }
    }
}
