using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VerticalSliceArchitecture.Core.Domain.Games;
using VerticalSliceArchitecture.Infrastructure.Data;

namespace VerticalSliceArchitecture.API.Features.Games
{
    public static class GetAll
    {
        public class Query : IRequest<Result>
        {

        }
        public class Result
        {
            public IEnumerable<Game> Games { get; set; }

            public Result()
            {
                Games = new List<Game>();
            }

            public class Game
            {               
                public int Id { get; protected set; }
                public Guid UserId { get; protected set; }
                public string Title { get; protected set; }
                public DateTime? ReleaseDate { get; protected set; }

                public Game(int id, Guid userId, string title, DateTime? releaseDate) 
                {
                    Id = id;
                    UserId = userId;
                    Title = title;
                    ReleaseDate = releaseDate;
                }
                
            }
        }
        public class Handler : IRequestHandler<Query, Result>
        {
            private readonly AppDb _db;
            private readonly IMapper _mapper;

            public Handler(AppDb db, IMapper mapper)
            {
                _db = db;
                _mapper = mapper;
            }

            public async Task<Result> Handle(Query query, CancellationToken token)
            {
                var games = await _db.Games
                    .AsNoTracking()
                    .ProjectTo<Result.Game>(_mapper.ConfigurationProvider)
                    .ToListAsync(token);

                return new Result { Games = games };
            }
        }       
    }
}
