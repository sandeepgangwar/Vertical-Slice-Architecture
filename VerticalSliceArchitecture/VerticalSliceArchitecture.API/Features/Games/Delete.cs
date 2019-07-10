using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using VerticalSliceArchitecture.Infrastructure.Commands.Shared;
using VerticalSliceArchitecture.Infrastructure.Data;
using VerticalSliceArchitecture.Infrastructure.Exceptions;
using VerticalSliceArchitecture.Infrastructure.Exceptions.Codes;

namespace VerticalSliceArchitecture.API.Features.Games
{
   public static class Delete
   {
        public class Command : AuthCommand
        {
            public int Id { get; set; }
           
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

                _db.Remove(game);
                await _db.SaveChangesAsync(token);
                return Unit.Value;
            }


        }
    }
}
