using System.Collections.Generic;
using MediatR;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.Search;

namespace SFA.DAS.Courses.Application.Courses.Queries.GetStandardsList
{
    public class GetStandardsListQuery : IRequest<GetStandardsListQueryResult>
    {
        public string Keyword { get; set; }
        public IList<int> RouteIds { get; set; } = new List<int>();
        public IList<int> Levels { get; set; } = new List<int>();
        public OrderBy OrderBy { get; set; }
        public StandardFilter Filter { get; set; }
        public ApprenticeshipType? ApprenticeshipType { get; set; }
        public bool IncludeAllProperties { get; set; }
    }
}
