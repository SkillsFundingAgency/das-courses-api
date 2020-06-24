﻿using MediatR;

namespace SFA.DAS.Courses.Application.Courses.Queries.GetStandardsList
{
    public class GetStandardsListQuery : IRequest<GetStandardsListResult>
    {
        public string Keyword { get; set; }
    }
}
