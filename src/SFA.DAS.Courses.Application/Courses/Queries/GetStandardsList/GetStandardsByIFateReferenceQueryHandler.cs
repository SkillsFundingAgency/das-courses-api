﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Application.Courses.Queries.GetStandardsList
{
    public class GetStandardsByIFateReferenceQueryHandler : IRequestHandler<GetStandardsByIFateReferenceQuery, GetStandardsByIFateReferenceResult>
    {
        private readonly ILogger<GetStandardsByIFateReferenceQueryHandler> _logger;
        private readonly IStandardsService _standardsService;

        public GetStandardsByIFateReferenceQueryHandler(
            ILogger<GetStandardsByIFateReferenceQueryHandler> logger,
            IStandardsService standardsService)
        {
            _logger = logger;
            _standardsService = standardsService;
        }

        public async Task<GetStandardsByIFateReferenceResult> Handle(GetStandardsByIFateReferenceQuery request, CancellationToken cancellationToken)
        {
            var standards = (await _standardsService.GetAllVersionsOfAStandard(request.IFateReferenceNumber)).ToList();

            return new GetStandardsByIFateReferenceResult
            {
                Standards = standards,
            };
        }
    }
}
