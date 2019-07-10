using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using VerticalSliceArchitecture.API.Features.Shared;
using VerticalSliceArchitecture.Infrastructure.Exceptions;
using VerticalSliceArchitecture.Infrastructure.Exceptions.Codes;


namespace VerticalSliceArchitecture.API.Features.Users
{
    [Route("api")]
    public class UsersController : BaseController
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }
        /// <summary>
        /// Create new user
        /// </summary>
        [SwaggerResponse(201, "resource created successfully", Type = typeof(Create.Result))]
        [SwaggerResponse(400)]
        [HttpPost("users")]
        public async Task<IActionResult> Create([FromBody]Create.Command command)
        {
            var result = await SendCommandWithResult(command);
            return Created(GetUrl(result.Id), result);
        }
        /// <summary>
        /// Get list of users (for user with admin role)
        /// </summary>
        [SwaggerResponse(200)]
        [SwaggerResponse(401)]
        [HttpGet("users")]
        [Authorize(Roles = "Admin")]
        public async Task<GetAll.Result> GetAll()
        {
            return await _mediator.Send(new GetAll.Query());
        }
        /// <summary>
        /// Get user by email
        /// </summary>
        [SwaggerResponse(200)]
        [SwaggerResponse(404, ResourceNotFoundExceptionCodes.UserNotFound, Type = typeof(ErrorResponse))]
        [HttpGet("users/{email}")]
        public async Task<GetByEmail.Result> GetByEmail([FromRoute] GetByEmail.Query query)
        {
            return await _mediator.Send(query);
        }




    }
}