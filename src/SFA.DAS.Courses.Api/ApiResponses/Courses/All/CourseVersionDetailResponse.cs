using SFA.DAS.Courses.Domain.Courses;

namespace SFA.DAS.Courses.Api.ApiResponses
{
    public class CourseVersionDetailResponse : VersionDetailResponse
    {
        public string ProposedDurationUnits { get; set; }

        public static explicit operator CourseVersionDetailResponse(CourseVersionDetail source)
        {
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

