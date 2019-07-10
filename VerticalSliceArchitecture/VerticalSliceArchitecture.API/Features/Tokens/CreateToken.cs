using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VerticalSliceArchitecture.Infrastructure.Data;
using VerticalSliceArchitecture.Core.Services;
using VerticalSliceArchitecture.Infrastructure.Configurations.App;
using VerticalSliceArchitecture.Core.Domain.Identity;
using VerticalSliceArchitecture.Infrastructure.Exceptions;
using VerticalSliceArchitecture.Infrastructure.Exceptions.Codes;
using VerticalSliceArchitecture.Core.Helpers;
using FluentValidation;


namespace VerticalSliceArchitecture.API.Features.Tokens
{
    public static class CreateToken
    {
        public class Command : IRequest<Result>
        {
            public string Email { get; set; }
            public string Password { get; set; }
            public string DeviceId { get; set; }
            public string DeviceName { get; set; }
            public Platform Platform { get; set; }
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
                var user = await _db.Users
                    .SingleOrDefaultAsync(u => !u.IsDeactivated && u.Email == request.Email, token);

                if (user == null)
                    throw new InfrastructureException(InfrastructureExceptionCodes.InvalidCredentials);


                var passwordHash = _hasher.GetHash(request.Password, user.PasswordSalt);

                if (passwordHash != user.PasswordHash)
                    throw new InfrastructureException(InfrastructureExceptionCodes.InvalidCredentials);

                var session = await _db.Sessions
                    .SingleOrDefaultAsync(
                        s => s.UserId == user.Id && s.DeviceId == request.DeviceId,
                        token);

                var refreshToken = _hasher.GetSalt();

                var refreshTokenExpires = DateTimeHelper.Now.AddMinutes(_refreshTokenSettings.ExpiryMinutes);

                if (session == null)
                {
                    session = new Session
                    (
                        user.Id,
                        request.DeviceId,
                        request.DeviceName,
                        request.Platform,
                        refreshToken,
                        refreshTokenExpires
                    );

                    _db.Sessions.Add(session);
                }
                else
                {
                    session.SetDeviceName(request.DeviceName);
                    session.LogAndSetRefreshToken(refreshToken, refreshTokenExpires);
                }

                await _db.SaveChangesAsync(token);

                return new Result
                {
                    AccessToken = _jwtHandler.CreateToken(user.Id, user.Login, user.UserRole),
                    RefreshToken = refreshToken
                };
            }

            public class CommandValidator : AbstractValidator<Command>
            {               
                public CommandValidator(AppDb db, IHasher hasher)
                {

                    RuleFor(c => c.Email)
                        .Cascade(CascadeMode.StopOnFirstFailure)
                        .NotEmpty()
                        .EmailAddress()
                        .Length(3, 255);

                    RuleFor(c => c.DeviceId)
                        .Cascade(CascadeMode.StopOnFirstFailure)
                        .NotEmpty()
                        .MaximumLength(255);

                    RuleFor(c => c.DeviceName)
                      .NotEmpty()
                      .MaximumLength(255);

                    RuleFor(c => c.Platform)
                        .Cascade(CascadeMode.StopOnFirstFailure)
                        .NotEmpty()
                        .IsInEnum();
                }
            }
        }
    }
}