using System;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace WWWTCHttpClientFixed
{
    internal sealed class WorkOrderServiceGatewayV2 : IDisposable
    {
        private bool _disposed;
        private ServiceAgentHttp _serviceAgentHttp;
        private readonly WorkOrderServiceSettings _workOrderServiceSettings;

        public WorkOrderServiceGatewayV2(WorkOrderServiceSettings workOrderServiceSettings)
        {
            _workOrderServiceSettings = workOrderServiceSettings;
            _serviceAgentHttp = new ServiceAgentHttp(_workOrderServiceSettings.BaseUrl);
        }

        public async Task<string> CreateWorkOrder(WorkOrderForCreate workOrderForCreate)
        {
            var accessToken = await Login(_workOrderServiceSettings.UserName, _workOrderServiceSettings.Password).ConfigureAwait(false);
            var workOrderCreateResponse = await CreateWorkOrderInternal(workOrderForCreate, accessToken).ConfigureAwait(false);
            await Logout(accessToken).ConfigureAwait(false);
            return workOrderCreateResponse.WorkOrderId;
        }

        private Task<string> Login(string userName, string password)
        {
            var nameValueCollection = new NameValueCollection { { "username", userName }, { "passowrd", password } };
            return _serviceAgentHttp.PostFormUrlEncodedAsync("login", nameValueCollection, WorkOrderServiceExceptionTranslator.EnsureSuccess);
        }

        private Task<WorkOrderCreateResponse> CreateWorkOrderInternal(WorkOrderForCreate workOrderForCreate, string accessToken)
        {
            var workOrderCreateRequest = MapperWorkOrder.MapToWorkOrderCreateRequest(workOrderForCreate);
            return _serviceAgentHttp.PostAsJsonAsync<WorkOrderCreateRequest, WorkOrderCreateResponse>(
                "v2",
                workOrderCreateRequest,
                "AR-JWT", accessToken,
                WorkOrderServiceExceptionTranslator.EnsureSuccess);
        }

        private Task Logout(string token)
        {
            return _serviceAgentHttp.PostAsJsonAsync<string, string>("logout", null, "Bearer", token, WorkOrderServiceExceptionTranslator.EnsureSuccess);
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
