using System;
using System.Diagnostics.CodeAnalysis;

namespace WWWTCHttpClientFixed
{

    [ExcludeFromCodeCoverage]
    public sealed class IncidentResponseServiceException : Exception
    {
        public IncidentResponseServiceException() { }
        public IncidentResponseServiceException(string message) : base(message) { }
        public IncidentResponseServiceException(string message, Exception inner) : base(message, inner) { }
    }
}
