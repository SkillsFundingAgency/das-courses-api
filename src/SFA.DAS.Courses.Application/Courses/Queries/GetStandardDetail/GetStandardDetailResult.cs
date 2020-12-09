using SFA.DAS.Courses.Domain.Entities.Versioning;

namespace SFA.DAS.Courses.Application.Courses.Queries.GetStandardDetail
{
    public struct GetStandardDetailResult
    {
        public Standard Standard { get; set; }
        public StandardAdditionalInformation StandardAdditionalInformation { get; set; }
        public GetStandardDetailResult(Standard standard, StandardAdditionalInformation standardAdditionalInformation)
        {
            Standard = standard;
            StandardAdditionalInformation = standardAdditionalInformation;
        }
    }
}
