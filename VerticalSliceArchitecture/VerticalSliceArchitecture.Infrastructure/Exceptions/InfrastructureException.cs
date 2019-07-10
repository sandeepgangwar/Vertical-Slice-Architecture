using System;
using VerticalSliceArchitecture.Core.Exceptions;

namespace VerticalSliceArchitecture.Infrastructure.Exceptions
{
    public class InfrastructureException : AppException
    {
        public InfrastructureException()
        {
        }

        public InfrastructureException(string code) : base(code)
        {
        }

        public InfrastructureException(string message, params object[] args) : base(string.Empty, message, args)
        {
        }

        public InfrastructureException(string code, string message, params object[] args) : base(null, code, message,
            args)
        {
        }

        public InfrastructureException(Exception innerException, string message, params object[] args)
            : base(innerException, string.Empty, message, args)
        {
        }

        public InfrastructureException(Exception innerException, string code, string message, params object[] args)
            : base(string.Format(message, args), innerException)
        {
        }
    }
}