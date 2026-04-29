using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Application.Courses.Queries.GetCoursesByIFateReference
{
    public class GetCoursesByIFateReferenceQueryHandler : IRequestHandler<GetCoursesByIFateReferenceQuery, GetCoursesByIFateReferenceResult>
    {
        private readonly IStandardsService _standardsService;

        public GetCoursesByIFateReferenceQueryHandler(IStandardsService standardsService)
        {
            ArgumentNullException.ThrowIfNull(standardsService);

            _standardsService = standardsService;
        }

        public async Task<GetCoursesByIFateReferenceResult> Handle(GetCoursesByIFateReferenceQuery request, CancellationToken cancellationToken)
        {
            var courses = await _standardsService.GetAllVersionsOfACourse(request.IFateReferenceNumber);
            return new GetCoursesByIFateReferenceResult
            {
                Courses = courses
            };
        }
    }
}
