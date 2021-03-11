using System;
using MediatR;

namespace SFA.DAS.Courses.Application.Courses.Queries.GetStandard
{
    public class GetStandardByIdQuery : IRequest<GetStandardByIdResult>
    {
        public string Id { get; set; }
    }
}
