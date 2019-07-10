using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using VerticalSliceArchitecture.API.Features.Shared;
using VerticalSliceArchitecture.Infrastructure.Exceptions;
using VerticalSliceArchitecture.Infrastructure.Exceptions.Codes;

namespace VerticalSliceArchitecture.API.Features.Tokens
{
    [AllowAnonymous]
    [Route("api/tokens")]
    public class TokensController : BaseController
    {
        /// <summary>
        /// Get access and refresh token(Sign in method)
        /// </summary>      
        [SwaggerResponse(400, InfrastructureExceptionCodes.InvalidCredentials, Type = typeof(ErrorResponse))]
        [SwaggerResponse(401)]
        [SwaggerResponse(200, "token created successfully", Type = typeof(CreateToken.Result))]
        [HttpPost, Route("")]
        public async Task<CreateToken.Result> CreateToken([FromBody] CreateToken.Command command)
            => await SendCommandWithResult(command);

        /// <summary>
        /// Get new access and refresh token in exchange for current refresh token
        /// </summary>      
        [SwaggerResponse(400, InfrastructureExceptionCodes.InvalidRefreshToken, Type = typeof(ErrorResponse))]
        [SwaggerResponse(401)]
        [SwaggerResponse(200, "token refreshed successfully", Type = typeof(RefreshToken.Result))]
        [HttpPost, Route("refresh")]
        public async Task<RefreshToken.Result> RefreshToken([FromBody] RefreshToken.Command command)
            => await SendCommandWithResult(command);
    }
}