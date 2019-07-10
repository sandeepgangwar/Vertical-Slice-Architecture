using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VerticalSliceArchitecture.Infrastructure.Data;
using VerticalSliceArchitecture.Infrastructure.Exceptions;
using VerticalSliceArchitecture.Infrastructure.Exceptions.Codes;

namespace VerticalSliceArchitecture.API.Features.Games
{
    public static class GetById
    {
        public class Query : IRequest<Result>
        {
            public int Id { get; set; }
        }
        public class Result
        {
            public int Id { get; protected set; }
            public Guid UserId { get; protected set; }
            public string Title { get; protected set; }
            public DateTime? ReleaseDate { get; protected set; }

            public Result(int id,Guid userId, string title, DateTime? releaseDate)
            {
                Id = id;
                UserId = userId;
                Title = title;
                ReleaseDate = releaseDate;              
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
                var result = await _db.Games
                   .AsNoTracking()
                   .Where(u => u.Id == query.Id)
                   .ProjectTo<Result>(_mapper.ConfigurationProvider)
                   .FirstOrDefaultAsync(token);

                if (result == null)
                    throw new ResourceNotFoundException(
                        ResourceNotFoundExceptionCodes.GameNotFound,
                        $"game with id: {query.Id} not found.");

                return result;
            }
        }        
    }
}
