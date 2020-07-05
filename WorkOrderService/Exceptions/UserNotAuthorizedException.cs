using System;
using System.Diagnostics.CodeAnalysis;

namespace WorkOrderService.Exceptions
{

    [ExcludeFromCodeCoverage]
    public sealed class UserNotAuthorizedException : Exception
    {
        public UserNotAuthorizedException() { }
        public UserNotAuthorizedException(string message) : base(message) { }
        public UserNotAuthorizedException(string message, Exception inner) : base(message, inner) { }
    }
}
