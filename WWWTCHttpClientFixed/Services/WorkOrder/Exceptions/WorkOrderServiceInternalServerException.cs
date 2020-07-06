using System;
using System.Diagnostics.CodeAnalysis;

namespace WWWTCHttpClientFixed
{

    [ExcludeFromCodeCoverage]
    public sealed class WorkOrderServiceInternalServerException : Exception
    {
        public WorkOrderServiceInternalServerException() { }
        public WorkOrderServiceInternalServerException(string message) : base(message) { }
        public WorkOrderServiceInternalServerException(string message, Exception inner) : base(message, inner) { }
    }
}
