namespace SFA.DAS.Courses.Domain.Courses
{
    public class StandardVersionDetail : VersionDetail
    {
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
