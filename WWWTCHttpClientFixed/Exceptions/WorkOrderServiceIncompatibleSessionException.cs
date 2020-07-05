using System;
using System.Diagnostics.CodeAnalysis;

namespace WWWTCHttpClientFixed
{

    [ExcludeFromCodeCoverage]
    public sealed class WorkOrderServiceIncompatibleSessionException : Exception
    {
        public WorkOrderServiceIncompatibleSessionException() { }
        public WorkOrderServiceIncompatibleSessionException(string message) : base(message) { }
        public WorkOrderServiceIncompatibleSessionException(string message, Exception inner) : base(message, inner) { }
    }
}
