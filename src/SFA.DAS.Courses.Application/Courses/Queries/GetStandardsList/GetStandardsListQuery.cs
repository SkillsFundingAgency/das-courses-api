using System;
using System.Collections.Generic;
using MediatR;
using SFA.DAS.Courses.Domain.Search;

namespace SFA.DAS.Courses.Application.Courses.Queries.GetStandardsList
{
    public class GetStandardsListQuery : IRequest<GetStandardsListResult>
    {
        public string Keyword { get; set; }
        public IList<Guid> RouteIds { get ; set ; }
        public IList<int> Levels { get; set; }
        public OrderBy OrderBy { get; set; }
        public StandardFilter Filter { get ; set ; }
    }
}
