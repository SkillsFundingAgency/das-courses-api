using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Courses.Api.Infrastructure;
using SFA.DAS.Courses.Application.StandardsImport.Handlers.ImportStandards;

namespace SFA.DAS.Courses.Api.Controllers
{
    [Authorize(Policy = nameof(PolicyNames.HasDataLoadPolicy))]
    [ApiController]
    [Route("/ops/dataload/")]
    public class ImportController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ImportController (IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Index()
        {

            await _mediator.Send(new ImportStandardsCommand());
        
            return NoContent();
            
        }
    }
}