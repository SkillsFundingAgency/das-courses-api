using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Courses.Api.ApiResponses;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Api.Controllers
{
    [ApiController]
    public class StandardsController : ControllerBase
    {
        private readonly IStandardsService _standardsService;

        public StandardsController(IStandardsService standardsService)
        {
            _standardsService = standardsService;
        }

        public async Task<IActionResult> GetList()
        {
            var standards = await _standardsService.GetStandardsList();

            var response = new GetStandardsListResponse
            {
                Standards = standards.Select(standard => (GetStandardResponse)standard)
            };

            return Ok(response);
        }
    }
}
