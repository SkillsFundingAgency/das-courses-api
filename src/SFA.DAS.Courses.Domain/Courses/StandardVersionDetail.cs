using System;

namespace SFA.DAS.Courses.Domain.Courses
{
    public class StandardVersionDetail
    {
        public DateTime? EarliestStartDate { get; set; }
        public DateTime? LatestStartDate { get; set; }
        public DateTime? LatestEndDate { get; set; }
        public DateTime? ApprovedForDelivery { get; set; }
        public int TypicalDuration { get; set; }
        public int MaxFunding { get; set; }

        public static explicit operator StandardVersionDetail(Entities.Standard source)
        {
            return new StandardVersionDetail
            {
                EarliestStartDate = source.EarliestStartDate,
                LatestStartDate = source.LatestStartDate,
                LatestEndDate = source.LatestEndDate,
                ApprovedForDelivery = source.ApprovedForDelivery,
                TypicalDuration = source.TypicalDuration,
                MaxFunding = source.MaxFunding
            };
        }
    }
}
