﻿using MediatR;
using System;

namespace VerticalSliceArchitecture.Infrastructure.Commands.Shared
{
    public class AuthCommandWithResult<Result> : IRequest<Result>
    {
        internal Guid UserId { get; private set; }

        public void SetUserId(Guid userId)
        {
            UserId = userId;
        }
    }
}