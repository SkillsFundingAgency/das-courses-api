using SFA.DAS.Courses.Domain.Courses;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Api.ApiResponses
{
    public class CourseVersionDetailResponse : VersionDetailResponse
    {
        public DurationUnits ProposedDurationUnits { get; set; }

        public static explicit operator CourseVersionDetailResponse(CourseVersionDetail source)
        {
            if (source == null)
                return null;

            return new CourseVersionDetailResponse
            {
                EarliestStartDate = source.EarliestStartDate,
                LatestStartDate = source.LatestStartDate,
                LatestEndDate = source.LatestEndDate,
                ApprovedForDelivery = source.ApprovedForDelivery,
                ProposedTypicalDuration = source.ProposedTypicalDuration,
                ProposedMaxFunding = source.ProposedMaxFunding,
                ProposedDurationUnits = source.ProposedDurationUnits
            };
        }
    }
}

