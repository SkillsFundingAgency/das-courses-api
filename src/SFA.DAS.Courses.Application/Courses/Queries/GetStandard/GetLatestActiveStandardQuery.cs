using System;
using MediatR;

namespace SFA.DAS.Courses.Application.Courses.Queries.GetStandard
{
    public class GetLatestActiveStandardQuery : IRequest<GetLatestActiveStandardResult>
    {
        public int LarsCode { get; set; }
        public string IfateRefNumber { get; set; }

        public GetLatestActiveStandardQuery(string id)
        {
            if (int.TryParse(id, out var larsCode))
            {
                LarsCode = larsCode;
            }
            else if (id.Length == 6)
            {
                IfateRefNumber = id;
            }
            else
            {
                throw new ArgumentException("Id must be a LarsCode or IFateReferenceNumber", nameof(id));
            }
        }

        public static bool IsLarsCodeOrIFateRefNumber(string id) => int.TryParse(id, out var _) || id.Length == 6;

    }
}
