using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using VerticalSliceArchitecture.API.Features.Shared;
using VerticalSliceArchitecture.Infrastructure.Exceptions;
using VerticalSliceArchitecture.Infrastructure.Exceptions.Codes;

namespace VerticalSliceArchitecture.API.Features.Games
{
    [Route("api")]
    public class GamesController : BaseController
    {
        private readonly IMediator _mediator;

        public GamesController(IMediator mediator)
        {
            _mediator = mediator;
        }
        /// <summary>
        /// Get list of all games
        /// </summary>
        [SwaggerResponse(200, Type = typeof(GetAll.Result))]
        [SwaggerResponse(400)]
        [HttpGet("games")]
        public async Task<GetAll.Result> GetAll()
        {
            return await _mediator.Send(new GetAll.Query());
        }
        /// <summary>
        /// Get game by id
        /// </summary>
        [SwaggerResponse(200, Type = typeof(GetById.Result))]
        [SwaggerResponse(400)]
        [SwaggerResponse(404, ResourceNotFoundExceptionCodes.GameNotFound, Type = typeof(ErrorResponse))]
        [HttpGet("games/{id}")]
        public async Task<GetById.Result> GetById(int id)
        {
            return await _mediator.Send(new GetById.Query { Id = id });
        }
        /// <summary>
        /// Create new game
        /// </summary>
        [SwaggerResponse(201, "resource created successfully", Type = typeof(Create.Result))]
        [SwaggerResponse(400)]
        [HttpPost("games")]
        [Authorize(Roles = "StandardUser")]
        public async Task<IActionResult> Create([FromBody]Create.Command command)
        {
            var result = await SendAuthCommandWithResult(command);
            return Created(GetUrl(result.Id), result);

        }
        /// <summary>
        /// Edit game
        /// </summary>
        [SwaggerResponse(200, "resource updated successfully")]
        [SwaggerResponse(400)]
        [SwaggerResponse(404, ResourceNotFoundExceptionCodes.GameNotFound, Type = typeof(ErrorResponse))]
        [SwaggerResponse(401)]
        [HttpPut("games/{id}")]
        [Authorize(Roles = "StandardUser")]
        public async Task<IActionResult> Edit(int id, [FromBody]Edit.Command command)
        {
            command.Id = id;
            await SendAuthCommand(command);
            return Ok();
        }
        /// <summary>
        /// Delete game
        /// </summary>
        [SwaggerResponse(204, "resource deleted successfully")]
        [SwaggerResponse(400)]
        [SwaggerResponse(404, ResourceNotFoundExceptionCodes.GameNotFound, Type = typeof(ErrorResponse))]
        [SwaggerResponse(401)]
        [HttpDelete("games/{id}")]
        [Authorize(Roles = "StandardUser")]
        public async Task<IActionResult> Delete(int id)
        {
            await SendAuthCommand(new Delete.Command { Id = id });
            return NoContent();
        }

    }
}