
namespace WWWTCHttpClientFixed
{
    internal static class MapperWorkOrder
    {
        public static WorkOrderCreateRequest MapToWorkOrderCreateRequest(WorkOrderForCreate workOrderForCreate)
        {
            return new WorkOrderCreateRequest(
                description: workOrderForCreate.Title + ": " + workOrderForCreate.Description,
                creationDate: workOrderForCreate.DateTime,
                customerEmail: workOrderForCreate.Email
                );
        }
    }
}
