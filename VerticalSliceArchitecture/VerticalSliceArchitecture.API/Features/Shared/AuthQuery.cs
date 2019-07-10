using System;
using MediatR;

namespace VerticalSliceArchitecture.Infrastructure.Queries.Shared
{
    public abstract class AuthQuery<Result> : IRequest<Result>
    {
        internal Guid UserId { get; private set; }

        public void SetUserId(Guid userId)
        {
            UserId = userId;
        }
    }
}