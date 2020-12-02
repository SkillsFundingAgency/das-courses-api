using MediatR;

namespace SFA.DAS.Courses.Application.Courses.Queries.GetOptions
{
    public struct GetStandardOptionsQuery : IRequest<GetStandardOptionsResult>
    {
        public string StandardUId { get; internal set; }
        public GetStandardOptionsQuery(string standardUId)
        {
            StandardUId = standardUId;
        }
    }
}
