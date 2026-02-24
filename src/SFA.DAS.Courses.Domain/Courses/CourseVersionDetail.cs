using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Domain.Courses
{
    public class CourseVersionDetail : VersionDetail
    {
        public DurationUnits ProposedDurationUnits { get; set; }

        public static explicit operator CourseVersionDetail(Entities.Standard source)
        {
            return new CourseVersionDetail
            {
                EarliestStartDate = source.VersionEarliestStartDate,
                LatestStartDate = source.VersionLatestStartDate,
                LatestEndDate = source.VersionLatestEndDate,
                ApprovedForDelivery = source.ApprovedForDelivery,
                ProposedTypicalDuration = source.ProposedTypicalDuration,
                ProposedMaxFunding = source.ProposedMaxFunding,
                ProposedDurationUnits = source.CourseType == CourseType.Apprenticeship ? DurationUnits.Months : DurationUnits.Hours
            };
        }
    }
}
