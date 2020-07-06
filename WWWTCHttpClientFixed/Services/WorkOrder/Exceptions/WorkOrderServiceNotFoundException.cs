using System;
using System.Diagnostics.CodeAnalysis;

namespace WWWTCHttpClientFixed
{

    [ExcludeFromCodeCoverage]
    public sealed class WorkOrderServiceNotFoundException : Exception
    {
        public WorkOrderServiceNotFoundException() { }
        public WorkOrderServiceNotFoundException(string message) : base(message) { }
        public WorkOrderServiceNotFoundException(string message, Exception inner) : base(message, inner) { }
    }
}
