using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Courses.Application.CoursesImport.Handlers.ImportStandards;
using Microsoft.Extensions.Logging;

namespace SFA.DAS.Courses.Api.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("ops")]
    public class DataLoadController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<DataLoadController> _logger;

        public DataLoadController (
            IMediator mediator,
            ILogger<DataLoadController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
        
        [HttpPost]
        [Route("dataload")]
        public async Task<IActionResult> Index()
        {
            try
            {
                _logger.LogInformation("Data import request received");
                await _mediator.Send(new ImportDataCommand());
                _logger.LogInformation("Data import completed successfully");
                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Data import failed");
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("import/standards")]
        public async Task<IActionResult> ImportStandards()
        {
            try
            {
                _logger.LogInformation("Standards import request received");
                await _mediator.Send(new ImportStandardsCommand());
                _logger.LogInformation("Standards import request completed successfully");
                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Data import failed");
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("load/standards")]
        public async Task<IActionResult> LoadStandards()
        {
            try
            {
                _logger.LogInformation("Standards load request received");
                await _mediator.Send(new LoadStandardsCommand());
                _logger.LogInformation("Standards load request completed successfully");
                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Data load failed");
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("import/standards/documents")]
        public async Task<IActionResult> LoadStandardDocuments()
        {
            try
            {
                _logger.LogInformation("Standards import request received");
                await _mediator.Send(new ImportStandardDocumentsCommand());
                _logger.LogInformation("Standards import completed successfully");
                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Document import failed");
                throw;
            }
        }
    }
}
