using System;
using System.Net;
using System.Threading.Tasks;

namespace WWWTCHttpClientFixed
{
    internal static class Program
    {
        public static async Task Main(string[] args)
        {
            ServicePointManager.DefaultConnectionLimit = 10;
            ServicePointManager.UseNagleAlgorithm = false;
            await CreateWorkOrder();
            await CreateIncidentResponse();
            Console.ReadLine();
        }

        private static async Task CreateWorkOrder()
        {
            var workOrderForCreate = new WorkOrderForCreate("Title", "This is the Description", DateTime.Now, "shiv.kumar@whotube.com");
            var workOrderServiceSettings = new WorkOrderServiceConnectionInfo("skumar", "youtube", "https://localhost:44373/api/workorder/");

            using var workOrderGateway = new WorkOrderServiceGatewayV2(workOrderServiceSettings);
            var workOrderId = await workOrderGateway.CreateWorkOrder(workOrderForCreate).ConfigureAwait(false);
            Console.WriteLine("Fixed Version");
            Console.WriteLine($"Work Order Id: {workOrderId}");
        }

        private static async Task CreateIncidentResponse()
        {
            var incidentResponseServiceConnectionInfo = new IncidentResponseServiceConnectionInfo("skumar", "youtube", "https://localhost:44373/api/incidentresponse/");
            var incidentForCreate = new IncidentForCreate(
                "This is the subject of the Email",
                "This is the description of the emails that will be sent",
                "https://matlus.webex.com/meet/test",
                "800-888-0000",
                "808 123 456",
                "Internal Ticket#: 123456789",
                "TechSupportGroup");
            using var incidentResponseGateway = new IncidentResponseGateway(incidentResponseServiceConnectionInfo);
            var incidentNumber = await incidentResponseGateway.IntiateIncidentNotifications(incidentForCreate);
            Console.WriteLine("Incident Response Created");
            Console.WriteLine($"Incident #: {incidentNumber}");
        }
    }
}
