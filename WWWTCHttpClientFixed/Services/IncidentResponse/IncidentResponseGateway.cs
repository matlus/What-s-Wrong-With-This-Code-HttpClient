using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace WWWTCHttpClientFixed
{
    internal sealed class IncidentResponseGateway : IDisposable
    {
        private bool _disposed;
        private ServiceAgentHttp _serviceAgentHttp;
        private readonly IncidentResponseServiceConnectionInfo _incidentResponseServiceConnectionInfo;

        public IncidentResponseGateway(IncidentResponseServiceConnectionInfo incidentResponseServiceConnectionInfo)
        {
            _incidentResponseServiceConnectionInfo = incidentResponseServiceConnectionInfo;
            _serviceAgentHttp = new ServiceAgentHttp(incidentResponseServiceConnectionInfo.BaseUrl);
        }

        public async Task<string> IntiateIncidentNotifications(IncidentForCreate incidentForCreate)
        {
            var mirSoapBody = MapperIncidentResponse.MapToIncidentCreateRequest(_incidentResponseServiceConnectionInfo.Username, _incidentResponseServiceConnectionInfo.Password, incidentForCreate);
            var intiateIncidentXmlResponse = await _serviceAgentHttp.PostAsync(string.Empty, mirSoapBody, "text/xml", IncidentResponseExceptionTranslator.EnsureSuccess);
            return MapperIncidentResponse.MapCreateIncidentResponseToIncidentNumber(intiateIncidentXmlResponse);
        }

        [ExcludeFromCodeCoverage]
        private void Dispose(bool disposing)
        {
            if (disposing && !_disposed && _serviceAgentHttp != null)
            {
                var localServiceAgentHttp = _serviceAgentHttp;
                localServiceAgentHttp.Dispose();
                _serviceAgentHttp = null;
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
