using MediatR;
using System;

namespace VerticalSliceArchitecture.Infrastructure.Commands.Shared
{
    public abstract class AuthCommand : IRequest
    {
        internal Guid UserId { get; private set; }

        public void SetUserId(Guid userId)
        {
            UserId = userId;
        }
    }
}