using System;
using System.Threading.Tasks;

namespace WWWTCHttpClientOriginal
{
    internal static class Program
    {
        public static async Task Main(string[] args)
        {
            var workOrderForCreate = new WorkOrderCreateRequest { Description = "This is the Description", CreationDate = DateTime.Now, CustomerEmail = "shiv.kumar@whotube.com" };
            var workOrderSettings = new WorkOrderSettings { UserName = "skumar",  Password = "youtube", BaseUrl = "https://localhost:44373/workorder" };
            
            var workOrderServiceAuthTokenProvider = new WorkOrderServiceAuthTokenProvider();
            var workOrderId = await workOrderServiceAuthTokenProvider.CreateWorkOrder(workOrderForCreate, workOrderSettings).ConfigureAwait(false);

            Console.WriteLine("Original Version");
            Console.WriteLine($"Work Order Id: {workOrderId}");
            Console.ReadLine();
        }
    }
}
