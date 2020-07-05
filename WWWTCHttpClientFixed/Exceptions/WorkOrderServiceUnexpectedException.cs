using System;
using System.Diagnostics.CodeAnalysis;

namespace WWWTCHttpClientFixed
{

    [ExcludeFromCodeCoverage]
    public sealed class WorkOrderServiceUnexpectedException : Exception
    {
        public WorkOrderServiceUnexpectedException() { }
        public WorkOrderServiceUnexpectedException(string message) : base(message) { }
        public WorkOrderServiceUnexpectedException(string message, Exception inner) : base(message, inner) { }
    }
}
