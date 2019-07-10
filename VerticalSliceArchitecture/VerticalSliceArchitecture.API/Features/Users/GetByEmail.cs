using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VerticalSliceArchitecture.Core.Domain.Identity;
using VerticalSliceArchitecture.Infrastructure.Data;
using VerticalSliceArchitecture.Infrastructure.Exceptions;
using VerticalSliceArchitecture.Infrastructure.Exceptions.Codes;
using VerticalSliceArchitecture.Infrastructure.Framework.Helpers;

namespace VerticalSliceArchitecture.API.Features.Users
{
    public static class GetByEmail
    {
        public class Query : IRequest<Result>
        {
            public string Email { get; set; }
        }
       
        public class Result
        {
            public Guid Id { get; protected set; }
            public string Email { get; protected set; }
            public string Login { get; protected set; }
            public string Role { get; protected set; }
            public DateTime CreatedAt { get; protected set; }
           
            public Result(Guid id, string email, string login, UserRole userRole, DateTime createdAt) 
            {
                Id = id;
                Email = email;
                Login = login;
                Role = EnumHelper.ToDescriptionString(userRole);
                CreatedAt = createdAt;
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
                var result = await _db.Users
                    .AsNoTracking()
                    .Where(u => u.Email == query.Email)
                    .ProjectTo<Result>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(token);

                if (result == null)
                    throw new ResourceNotFoundException(
                        ResourceNotFoundExceptionCodes.UserNotFound,
                        $"user with email: {query.Email} not found.");

                return result;
            }
        }
              
    }
}
