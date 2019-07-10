using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VerticalSliceArchitecture.Core.Domain.Identity;
using VerticalSliceArchitecture.Infrastructure.Data;
using VerticalSliceArchitecture.Infrastructure.Framework.Helpers;

namespace VerticalSliceArchitecture.API.Features.Users
{
    public static class GetAll
    {
        public class Query : IRequest<Result>
        {           
        }
        public class Result
        {
            public IEnumerable<User> Users { get; set; }

            public Result()
            {
                Users = new List<User>();
            }

            public class User
            {
               
                public Guid Id { get; protected set; }
                public string Email { get; protected set; }
                public string Login { get; protected set; }
                public string Role { get; protected set; }
                public DateTime CreatedAt { get; protected set; }
               
                public User(Guid id,string email, string login, UserRole userRole, DateTime createdAt) 
                {
                    Id = id;
                    Email = email;
                    Login = login;
                    Role = EnumHelper.ToDescriptionString(userRole);
                    CreatedAt = createdAt;
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
                var users = await _db.Users
                    .AsNoTracking()
                    .ProjectTo<Result.User>(_mapper.ConfigurationProvider)
                    .ToListAsync(token);

                return new Result { Users = users };             
            }
        }
       
    }
}
