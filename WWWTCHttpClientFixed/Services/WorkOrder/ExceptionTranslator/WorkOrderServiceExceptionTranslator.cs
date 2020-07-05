using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace WWWTCHttpClientFixed
{
    internal static class WorkOrderServiceExceptionTranslator
    {
        public static async Task EnsureSuccess(HttpResponseMessage httpResponseMessage)
        {
            var statusCode = httpResponseMessage.StatusCode;
            if (statusCode == HttpStatusCode.OK)
            {
                return;
            }

            var httpContent = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);

            switch (statusCode)
            {
                case HttpStatusCode.NotFound:
                    throw new WorkOrderServiceNotFoundException("The Remedy Service call resulted in a Not Found Status Code");
                case HttpStatusCode.InternalServerError:
                    {
                        if (httpContent.Contains("User is currently connected from another machine or incompatible session.", StringComparison.OrdinalIgnoreCase))
                        {
                            throw new WorkOrderServiceIncompatibleSessionException($"The Remedy Service call resulted in a status code of: {statusCode}, with body: {httpContent}");
                        }
                        throw new WorkOrderServiceInternalServerException($"The Remedy Service call resulted in a status code of: {statusCode}, with body: {httpContent}");
                    }
                default:
                    throw new WorkOrderServiceUnexpectedException($"The Remedy Service call resulted in a status code of: {statusCode}, with body: {httpContent}");
            }
        }
    }
}
