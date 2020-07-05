using System;
using System.Threading.Tasks;

namespace WWWTCHttpClientFixed
{
    internal static class Program
    {
        public static async Task Main(string[] args)
        {
            var workOrderForCreate = new WorkOrderForCreate("Title", "This is the Description", DateTime.Now, "shiv.kumar@whotube.com");
            var workOrderServiceSettings = new WorkOrderServiceSettings("skumar", "youtube", "https://localhost:44373/workorder/");
            
            using var workOrderGateway = new WorkOrderServiceGateway(workOrderServiceSettings);            
            var workOrderId = await workOrderGateway.CreateWorkOrder(workOrderForCreate).ConfigureAwait(false);

            Console.WriteLine("Fixed Version");
            Console.WriteLine($"Work Order Id: {workOrderId}");
            Console.ReadLine();
        }
    }
}
