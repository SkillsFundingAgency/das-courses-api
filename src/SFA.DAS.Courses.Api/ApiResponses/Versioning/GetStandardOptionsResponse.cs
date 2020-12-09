using System;
using System.Collections.Generic;
using SFA.DAS.Courses.Domain.Entities.Versioning;

namespace SFA.DAS.Courses.Api.ApiResponses.Versioning
{
    public class GetStandardOptionsResponse
    {
        public string StandardUId { get; set; }
        public IEnumerable<string> Options { get; set; }
        public static implicit operator GetStandardOptionsResponse(StandardAdditionalInformation standardAdditionalInformation)
            => new GetStandardOptionsResponse
            {
                StandardUId = standardAdditionalInformation.StandardUId,
                Options = standardAdditionalInformation.Options?.ToArray() ?? Array.Empty<string>()
            };
    }
}
