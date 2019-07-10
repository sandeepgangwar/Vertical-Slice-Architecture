using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using VerticalSliceArchitecture.Core.Domain.Games;
using VerticalSliceArchitecture.Infrastructure.Commands.Shared;
using VerticalSliceArchitecture.Infrastructure.Data;

namespace VerticalSliceArchitecture.API.Features.Games
{
    public static class Create
    {
        public class Command : AuthCommandWithResult<Result>
        {
            public string Title { get; set; }
            public DateTime? ReleaseDate { get; set; }
        }
        public class Result
        {
            public int Id { get; set; }
            public string Title { get; set; }
        }
        public class Handler : IRequestHandler<Command, Result>
        {
            private readonly AppDb _db;

            public Handler(AppDb db)
            {
                _db = db;
            }

            public async Task<Result> Handle(Command request, CancellationToken token)
            {
                var newGame = new Game(request.UserId, request.Title, request.ReleaseDate);

                await _db.Games.AddAsync(newGame, token);
                await _db.SaveChangesAsync(token);

                return new Result { Id = newGame.Id, Title = newGame.Title };
            }


        }
        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(c => c.Title)
                    .Cascade(CascadeMode.StopOnFirstFailure)
                    .NotEmpty()
                    .Length(2, 50);
            }
        }
    }
}
