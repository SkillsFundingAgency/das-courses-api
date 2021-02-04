using MediatR;

namespace SFA.DAS.Courses.Application.Courses.Queries.GetStandardsList
{
    public class GetStandardsByIFateReferenceQuery : IRequest<GetStandardsByIFateReferenceResult>
    {
        public string IFateReferenceNumber { get; set; }
    }
}
