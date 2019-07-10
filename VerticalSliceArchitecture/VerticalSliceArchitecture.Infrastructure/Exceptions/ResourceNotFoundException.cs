using System;
using VerticalSliceArchitecture.Core.Exceptions;

namespace VerticalSliceArchitecture.Infrastructure.Exceptions
{
    public class ResourceNotFoundException : AppException
    {
        public ResourceNotFoundException()
        {
        }

        public ResourceNotFoundException(string code) : base(code)
        {
        }

        public ResourceNotFoundException(string message, params object[] args) : base(string.Empty, message, args)
        {
        }

        public ResourceNotFoundException(string code, string message, params object[] args) : base(null, code, message,
            args)
        {
        }

        public ResourceNotFoundException(Exception innerException, string message, params object[] args)
            : base(innerException, string.Empty, message, args)
        {
        }

        public ResourceNotFoundException(Exception innerException, string code, string message, params object[] args)
            : base(string.Format(message, args), innerException)
        {
        }
    }
}