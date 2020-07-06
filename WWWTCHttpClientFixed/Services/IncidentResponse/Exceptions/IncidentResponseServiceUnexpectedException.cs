using System;
using System.Diagnostics.CodeAnalysis;

namespace WWWTCHttpClientFixed
{

    [ExcludeFromCodeCoverage]
    public sealed class IncidentResponseServiceUnexpectedException : Exception
    {
        public IncidentResponseServiceUnexpectedException() { }
        public IncidentResponseServiceUnexpectedException(string message) : base(message) { }
        public IncidentResponseServiceUnexpectedException(string message, Exception inner) : base(message, inner) { }
    }
}
