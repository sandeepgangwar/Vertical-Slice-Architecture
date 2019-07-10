using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VerticalSliceArchitecture.Infrastructure.Data;
using VerticalSliceArchitecture.Core.Services;
using VerticalSliceArchitecture.Infrastructure.Configurations.App;
using VerticalSliceArchitecture.Infrastructure.Exceptions;
using VerticalSliceArchitecture.Infrastructure.Exceptions.Codes;
using VerticalSliceArchitecture.Core.Helpers;
using FluentValidation;

namespace VerticalSliceArchitecture.API.Features.Tokens
{
    public static class RefreshToken
    {
        public class Command : IRequest<Result>
        {
            public string RefreshToken { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(c => c.RefreshToken)
                    .Cascade(CascadeMode.StopOnFirstFailure)
                    .NotEmpty()
                    .MaximumLength(64);
            }
        }

        public class Result
        {
            public string AccessToken { get; set; }
            public string RefreshToken { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result>
        {
            private readonly AppDb _db;
            private readonly IJwtHandler _jwtHandler;
            private readonly IHasher _hasher;
            private readonly RefreshTokenSettings _refreshTokenSettings;

            public Handler(
                AppDb db,
                IJwtHandler jwtHandler,
                IHasher hasher,
                RefreshTokenSettings refreshTokenSettings)
            {
                _db = db;
                _jwtHandler = jwtHandler;
                _hasher = hasher;
                _refreshTokenSettings = refreshTokenSettings;
            }

            public async Task<Result> Handle(Command request, CancellationToken token)
            {
                var session = await _db.Sessions
                    .SingleOrDefaultAsync(s => !s.User.IsDeactivated && s.RefreshToken == request.RefreshToken, token);

                if (session == null)
                    throw new InfrastructureException(
                        InfrastructureExceptionCodes.InvalidRefreshToken, "Invalid refresh token.");

                if (session.IsExpired())
                    throw new InfrastructureException(
                        InfrastructureExceptionCodes.InvalidRefreshToken, "Refresh token is expired.");

                var user = await _db.Users
                    .SingleAsync(u => u.Id == session.UserId, token);

                var refreshToken = _hasher.GetSalt();
                var refreshTokenExpires = DateTimeHelper.Now.AddMinutes(_refreshTokenSettings.ExpiryMinutes);

                session.SetUpdatedAt();
                session.LogAndSetRefreshToken(refreshToken, refreshTokenExpires);

                await _db.SaveChangesAsync(token);

                return new Result
                {
                    AccessToken = _jwtHandler.CreateToken(user.Id, user.Login, user.UserRole),
                    RefreshToken = refreshToken
                };
            }
            
        }
    }
}