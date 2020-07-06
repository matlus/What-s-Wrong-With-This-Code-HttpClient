using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace WWWTCHttpClientFixed
{
    internal static class IncidentResponseServiceDocumentParser
    {
        public static (XmlDocument xmlDocument, XmlNamespaceManager xmlNamespaceManager) InitializeXmlDocument(string xmlResponseContent)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xmlResponseContent);
            var xmlNamespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);
            xmlNamespaceManager.AddNamespace("soapenv", "http://schemas.xmlsoap.org/soap/envelope/");
            xmlNamespaceManager.AddNamespace("ns", "http://www.matlus.com/ws");
            return (xmlDocument, xmlNamespaceManager);
        }

        public static (string errorCode, string message) ExtractErrorMessage(XmlDocument xmlDocument, XmlNamespaceManager xmlNamespaceManager)
        {
            var errorNode = xmlDocument.SelectSingleNode("/soapenv:Envelope/soapenv:Body/ns:response/ns:error", xmlNamespaceManager);
            var errorCodeNode = errorNode.SelectSingleNode("./ns:errorCode", xmlNamespaceManager);
            var errorMessageNode = errorNode.SelectSingleNode("./ns:errorMessage", xmlNamespaceManager);
            return (errorCodeNode.InnerText, errorMessageNode.InnerText);
        }

        public static bool ExtractSuccessValue(XmlDocument xmlDocument, XmlNamespaceManager xmlNamespaceManager)
        {
            var successNode = xmlDocument.SelectSingleNode("/soapenv:Envelope/soapenv:Body/ns:response/ns:success", xmlNamespaceManager);
            return bool.Parse(successNode.InnerText);
        }

        public static string ExtractCreatedIncidentNumber(string xmlResponseContent)
        {
            var (xmlDocument, xmlNamespaceManager) = InitializeXmlDocument(xmlResponseContent);
            var notificationReportNode = xmlDocument.SelectSingleNode("/soapenv:Envelope/soapenv:Body/ns:response/ns:oneStepNotificationResponse/ns:notificationReportUUID", xmlNamespaceManager);
            return notificationReportNode.InnerText;
        }
    }
}
