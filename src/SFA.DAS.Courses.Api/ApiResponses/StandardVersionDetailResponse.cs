using System;
using SFA.DAS.Courses.Domain.Courses;

namespace SFA.DAS.Courses.Api.ApiResponses
{
    public class StandardVersionDetailResponse
    {
        public DateTime? EarliestStartDate { get; set; }
        public DateTime? LatestStartDate { get; set; }
        public DateTime? LatestEndDate { get; set; }
        public DateTime? ApprovedForDelivery { get; set; }
        public int ProposedTypicalDuration { get; set; }
        public int ProposedMaxFunding { get; set; }

        public static explicit operator StandardVersionDetailResponse(StandardVersionDetail source)
        {
            return new StandardVersionDetailResponse
            {
                EarliestStartDate = source.EarliestStartDate,
                LatestStartDate = source.LatestStartDate,
                LatestEndDate = source.LatestEndDate,
                ApprovedForDelivery = source.ApprovedForDelivery,
                ProposedTypicalDuration = source.TypicalDuration,
                ProposedMaxFunding = source.MaxFunding
            };
        }
    }
}

