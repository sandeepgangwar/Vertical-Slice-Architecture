using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VerticalSliceArchitecture.API.Extensions;
using VerticalSliceArchitecture.Infrastructure.Commands.Shared;
using VerticalSliceArchitecture.Infrastructure.Queries.Shared;

namespace VerticalSliceArchitecture.API.Features.Shared
{
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        private IMediator _mediator;

        protected IMediator Mediator => _mediator ?? (_mediator = (IMediator)HttpContext.RequestServices.GetService(typeof(IMediator)));

        protected async Task SendCommand(IRequest command)
            => await Mediator.Send(command);

        protected async Task<Result> SendCommandWithResult<Result>(IRequest<Result> command) where Result : new()
            => await Mediator.Send(command);

        protected async Task SendAuthCommand(AuthCommand command)
        {
            command.SetUserId(User.Identity.GetId());
            await Mediator.Send(command);
        }

        protected async Task<Result> SendAuthCommandWithResult<Result>(AuthCommandWithResult<Result> command) where Result : new()
        {
            command.SetUserId(User.Identity.GetId());
            return await Mediator.Send(command);
        }

        protected async Task<Result> SendQuery<Result>(IRequest<Result> query) where Result : new()
            => await Mediator.Send(query);

        protected async Task<Result> SendAuthQuery<Result>(AuthQuery<Result> query) where Result : new()
        {
            query.SetUserId(User.Identity.GetId());
            return await Mediator.Send(query);
        }
        protected string GetUrl<T>(T id)
        {
            return $"{Request.Scheme}://{Request.Host}{Request.PathBase}{Request.Path}/{id}";
        }
    }
}