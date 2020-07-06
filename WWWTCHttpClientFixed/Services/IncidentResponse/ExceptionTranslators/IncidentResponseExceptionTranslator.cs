using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace WWWTCHttpClientFixed
{
    internal static class IncidentResponseExceptionTranslator
    {
        public static async Task EnsureSuccess(HttpResponseMessage httpResponseMessage)
        {
            var xmlResponseContent = await httpResponseMessage.Content.ReadAsStringAsync();

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var (xmlDocument, xmlNamespaceManager) = IncidentResponseServiceDocumentParser.InitializeXmlDocument(xmlResponseContent);
                bool isSuccess = IncidentResponseServiceDocumentParser.ExtractSuccessValue(xmlDocument, xmlNamespaceManager);

                if (!isSuccess)
                {
                    var (errorCode, errorMessage) = IncidentResponseServiceDocumentParser.ExtractErrorMessage(xmlDocument, xmlNamespaceManager);
                    throw new IncidentResponseServiceException($"Incident Response Service Returned the following Error - Code: {errorCode}, Message: {errorMessage}");
                }
            }
            else
            {
                throw new IncidentResponseServiceUnexpectedException(xmlResponseContent);
            }
        }
    }
}
