using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Courses.Api.ApiResponses;
using SFA.DAS.Courses.Application.SectorSubjectArea.Queries.GetAllSectorSubjectAreaTier1;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Api.Controllers;

[ApiVersion("1.0")]
[Route("api/[controller]")]
[ApiController]
public class SectorSubjectAreaController : ControllerBase
{
    private readonly IMediator _mediator;

    public SectorSubjectAreaController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(GetSectorSubjectAreaTier1ListResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSectorSubjectAreaTier1List(CancellationToken cancellationToken)
    {
        List<SectorSubjectAreaTier1> response = await _mediator.Send(new GetAllSectorSubjectAreaTier1Query(), cancellationToken);
        GetSectorSubjectAreaTier1ListResponse result = new(response.Select(s => (GetSectorSubjectAreaTier1Response)s).ToList());
        return Ok(result);
    }
}
