﻿using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Courses.Api.ApiResponses;
using SFA.DAS.Courses.Application.Courses.Queries.GetLevels;

namespace SFA.DAS.Courses.Api.Controllers
{
    [ApiController]
    [Route("api/courses/[controller]")]
    public class LevelsController : ControllerBase
    {
        private readonly ILogger<LevelsController> _logger;
        private readonly IMediator _mediator;

        public LevelsController(
            ILogger<LevelsController> logger,
            IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetList()
        {
            try
            {
                var queryResult = await _mediator.Send(new GetLevelsListQuery());

                var response = new GetLevelsListResponse
                {
                    Levels = queryResult.Levels.Select(level => (GetLevelResponse)level)
                };

                return Ok(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error attempting to get list of levels");
                return BadRequest();
            }
        }

    }
}