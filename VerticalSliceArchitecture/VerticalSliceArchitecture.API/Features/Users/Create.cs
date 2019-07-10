using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using VerticalSliceArchitecture.Core.Domain.Identity;
using VerticalSliceArchitecture.Core.Services;
using VerticalSliceArchitecture.Infrastructure.Data;


namespace VerticalSliceArchitecture.API.Features.Users
{
    public static class Create
    {
        public class Command : IRequest<Result>
        {
            public string Email { get; set; }
            public string Login { get; set; }
            public string Password { get; set; }
            public UserRole UserRole { get; set; }
            public bool HasAcceptedTermsOfUse { get; set; }
        }
        public class CommandValidator : AbstractValidator<Command>
        {
            private readonly AppDb _db;

            public CommandValidator(AppDb db)
            {
                _db = db;

                RuleFor(c => c.Email)
                    .Cascade(CascadeMode.StopOnFirstFailure)
                    .EmailAddress()
                    .NotEmpty()
                    .MaximumLength(255)
                    .Custom((email, context) =>
                    {
                        if (_db.Users.Any(u => u.Email == email))
                        {
                            context.AddFailure("Email", $"Email adress {email} is already taken.");
                        };
                    });


                RuleFor(c => c.Password)
                    .Cascade(CascadeMode.StopOnFirstFailure)
                    .NotEmpty()
                    .Length(8, 50);

                RuleFor(c => c.Login)
                    .Cascade(CascadeMode.StopOnFirstFailure)
                    .NotEmpty()
                    .Length(4, 50)
                    .Custom((login, context) =>
                    {
                        if (_db.Users.Any(u => u.Login == login))
                        {
                            context.AddFailure("Login", $"User name {login} is already taken.");
                        };
                    });

                RuleFor(c => c.UserRole)
                    .Cascade(CascadeMode.StopOnFirstFailure)
                    .NotEmpty()
                    .IsInEnum();

                RuleFor(c => c.HasAcceptedTermsOfUse)
                    .Must(c => c.Equals(true));
            }
        }
        public class Result
        {
            public Guid Id { get; set; }
            public string Login { get; set; }           
        }
        public class Handler : IRequestHandler<Command, Result>
        {
            private readonly AppDb _db;
            private readonly IHasher _passwordHasher;

            public Handler(AppDb db, IHasher passwordHasher)
            {
                _db = db;
                _passwordHasher = passwordHasher;
            }

            public async Task<Result> Handle(Command request, CancellationToken token)
            {
                var salt = _passwordHasher.GetSalt();
                var passwordHash = _passwordHasher.GetHash(request.Password, salt);

                var newUser = new User(request.Email, request.Login, request.UserRole, passwordHash, salt);

                await _db.Users.AddAsync(newUser, token);
                await _db.SaveChangesAsync(token);
                return new Result { Id = newUser.Id,Login = newUser.Login };
            }
           
        }
    }
}