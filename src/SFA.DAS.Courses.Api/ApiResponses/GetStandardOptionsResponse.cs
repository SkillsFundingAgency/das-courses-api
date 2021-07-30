using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.Courses.Domain.Courses;

namespace SFA.DAS.Courses.Api.ApiResponses
{
    public class GetStandardOptionsResponse
    {
        public string StandardUId { get; set; }
        public int LarsCode { get; set; }
        public string IfateReferenceNumber { get; set; }
        public string Version { get; set; }
        public List<string> Options { get; set; }

        public static implicit operator GetStandardOptionsResponse(Standard source)
        {
            return new GetStandardOptionsResponse
            {
                StandardUId = source.StandardUId,
                LarsCode = source.LarsCode,
                IfateReferenceNumber = source.IfateReferenceNumber,
                Version = source.Version,
                Options = source.Options
            };
        }
    }
}
