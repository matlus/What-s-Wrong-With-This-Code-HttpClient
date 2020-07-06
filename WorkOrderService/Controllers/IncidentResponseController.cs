using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WorkOrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncidentResponseController : ControllerBase
    {
        [HttpPost]
        public async Task<string> CreateIncident()
        {
            EnsureContentTypeIsXml(Request.Headers);
            var (xmlDocument, xmlNamespaceManager) = await InitializeXmlDocument(Request.Body);
            var (username, password) = ExtractUserCredentials(xmlDocument, xmlNamespaceManager);

            if (UserIsAuthorized(username, password))
            {
                return
@$"<?xml version=""1.0"" encoding=""UTF-8""?>
<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"">
  <soapenv:Body>
    <response xmlns=""http://www.matlus.com/ws"">
      <success>true</success>
      <oneStepNotificationResponse>
        <notificationReportUUID>{Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture)}</notificationReportUUID>
        <timeInitiated>{DateTime.UtcNow.ToString("o", CultureInfo.InvariantCulture)}</timeInitiated>
      </oneStepNotificationResponse>
    </response>
  </soapenv:Body>
</soapenv:Envelope>";
            }

            return
@"<?xml version=""1.0"" encoding=""UTF-8""?>
  <soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"">
    <soapenv:Body>    
      <response xmlns=""http://www.matlus.com/ws"">
        <success>false</success>
        <error>     
          <errorCode>1004</errorCode>
          <errorMessage>Authorization failed.Please check the authorization username/password.</errorMessage>
        </error>
      </response>
    </soapenv:Body>
  </soapenv:Envelope>";
        }

        private void EnsureContentTypeIsXml(IHeaderDictionary headers)
        {
            if (headers.TryGetValue("Content-Type", out var value))
            {
                if (value[0].StartsWith("text/xml", StringComparison.InvariantCulture))
                {
                    return;
                }
            }
            
            throw new Exception("No Content-Type Header provided. Expecting a Content-Type header of text/xml");
        }

        private bool UserIsAuthorized(string username, string password)
        {
            return string.Compare(username, "skumar", StringComparison.OrdinalIgnoreCase) == 0
                && string.Compare(password, "youtube", StringComparison.OrdinalIgnoreCase) == 0;
        }

        private static async Task<(XmlDocument xmlDocument, XmlNamespaceManager xmlNamespaceManager)> InitializeXmlDocument(Stream stream)
        {
            var content = await GetStreamContent(stream);
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(content);
            var xmlNamespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);
            xmlNamespaceManager.AddNamespace("soapenv", "http://schemas.xmlsoap.org/soap/envelope/");
            xmlNamespaceManager.AddNamespace("ns", "http://www.matlus.com/ws");
            return (xmlDocument, xmlNamespaceManager);
        }

        private static async Task<string> GetStreamContent(Stream stream)
        {
            using var streamReader = new StreamReader(stream);
            return await streamReader.ReadToEndAsync();
        }

        private static (string username, string password) ExtractUserCredentials(XmlDocument xmlDocument, XmlNamespaceManager xmlNamespaceManager)
        {
            var authorizationNode = xmlDocument.SelectSingleNode("/soapenv:Envelope/soapenv:Body/ns:oneStepNotification/ns:authorization", xmlNamespaceManager);
            var usernameNode = authorizationNode.SelectSingleNode("./ns:username", xmlNamespaceManager);
            var passwordNode = authorizationNode.SelectSingleNode("./ns:password", xmlNamespaceManager);
            return (usernameNode.InnerText, passwordNode.InnerText);
        }

    }
}
