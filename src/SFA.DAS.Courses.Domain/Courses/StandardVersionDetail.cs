using System;

namespace SFA.DAS.Courses.Domain.Courses
{
    public class StandardVersionDetail
    {
        public DateTime? EarliestStartDate { get; set; }
        public DateTime? LatestStartDate { get; set; }
        public DateTime? LatestEndDate { get; set; }
        public DateTime? ApprovedForDelivery { get; set; }
        public int ProposedTypicalDuration { get; set; }
        public int ProposedMaxFunding { get; set; }

        public static explicit operator StandardVersionDetail(Entities.Standard source)
        {
            return new StandardVersionDetail
            {
                EarliestStartDate = source.VersionEarliestStartDate,
                LatestStartDate = source.VersionLatestStartDate,
                LatestEndDate = source.VersionLatestEndDate,
                ApprovedForDelivery = source.ApprovedForDelivery,
                ProposedTypicalDuration = source.ProposedTypicalDuration,
                ProposedMaxFunding = source.ProposedMaxFunding
            };
        }
    }
}
