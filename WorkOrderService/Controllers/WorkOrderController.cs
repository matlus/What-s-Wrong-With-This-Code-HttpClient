using System;
using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using WorkOrderService.Exceptions;
using WorkOrderService.Models;

namespace WorkOrderService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkOrderController : ControllerBase
    {
        [HttpPost("login")]
        public string Login([FromForm] string username, [FromForm] string password)
        {
            var byteArray = Encoding.ASCII.GetBytes($"{username}:{password}");
            return Convert.ToBase64String(byteArray);
        }

        [HttpPost]
        public string CreateWorkOrder(WorkOrderCreateRequest workOrderCreateRequest)
        {
            if (Request.Headers.TryGetValue("Authorization", out var authorizationHeader) && authorizationHeader[0].StartsWith("AR-JWT ", StringComparison.InvariantCulture))
            {
                return $"{Guid.NewGuid().ToString("N", CultureInfo.CurrentCulture)}-{workOrderCreateRequest.CustomerEmail}";
            }
            
            throw new UserNotAuthorizedException("No Authorization Header.");
        }

        [HttpPost("v2")]
        public WorkOrderCreateResponse CreateWorkOrder2(WorkOrderCreateRequest workOrderCreateRequest)
        {
            if (Request.Headers.TryGetValue("Authorization", out var authorizationHeader) && authorizationHeader[0].StartsWith("AR-JWT ", StringComparison.InvariantCulture))
            {
                return new WorkOrderCreateResponse { WorkOrderId = $"{Guid.NewGuid().ToString("N", CultureInfo.CurrentCulture)}-{workOrderCreateRequest.CustomerEmail}" };
            }

            throw new UserNotAuthorizedException("No Authorization Header.");
        }

        [HttpPost("logout")]
        public void Logout()
        {
            if (!Request.Headers.TryGetValue("Authorization", out var authorizationHeader) && authorizationHeader[0].StartsWith("Bearer ", StringComparison.InvariantCulture))
            {
                throw new UserNotAuthorizedException("No Authorization Header.");
            }
        }
    }
}
