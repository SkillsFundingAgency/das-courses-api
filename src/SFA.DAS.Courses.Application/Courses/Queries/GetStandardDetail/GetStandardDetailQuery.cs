using MediatR;

namespace SFA.DAS.Courses.Application.Courses.Queries.GetStandardDetail
{
    public struct GetStandardDetailQuery : IRequest<GetStandardDetailResult>
    {
        public string StandardUId { get; internal set; }
        public GetStandardDetailQuery(string standardUId)
        {
            StandardUId = standardUId;
        }
    }
}
