using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VerticalSliceArchitecture.Core.Domain.Games;
using VerticalSliceArchitecture.Infrastructure.Commands.Shared;
using VerticalSliceArchitecture.Infrastructure.Data;
using VerticalSliceArchitecture.Infrastructure.Exceptions;
using VerticalSliceArchitecture.Infrastructure.Exceptions.Codes;

namespace VerticalSliceArchitecture.API.Features.Games
{
   public static class Edit
   {
        public class Command : AuthCommand
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public DateTime? ReleaseDate { get; set; }

        }
        public class Handler : IRequestHandler<Command>
        {
            private readonly AppDb _db;

            public Handler(AppDb db)
            {
                _db = db;
            }

            public async Task<Unit> Handle(Command command, CancellationToken token)
            {
                var game = await _db.Games.FirstOrDefaultAsync(g => g.Id == command.Id, token);

                if (game == null)
                    throw new ResourceNotFoundException(
                        ResourceNotFoundExceptionCodes.GameNotFound,
                        $"game with id: {command.Id} not found.");
            
                game.SetTitle(command.Title);
                game.SetReleaseDate(command.ReleaseDate);
                game.SetUpdatedAt();

                await _db.SaveChangesAsync(token);
                return Unit.Value;
            }
            public class CommandValidator : AbstractValidator<Command>
            {
                public CommandValidator()
                {
                    RuleFor(c => c.Title)
                        .NotEmpty()
                        .Length(2, 50);
                }
            }


        }
    }
}
