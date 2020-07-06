
namespace WWWTCHttpClientFixed
{
    internal static class MapperIncidentResponse
    {
        public static string MapToIncidentCreateRequest(string username, string password, IncidentForCreate incidentForCreate)
        {
            var incidentCreateSoapRequest =
$@"<?xml version=""1.0"" encoding=""UTF-8""?>
<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"">
   <soapenv:Header/>
   <soapenv:Body>
<oneStepNotification xmlns=""http://www.matlus.com/ws"">
<apiVersion>4.8</apiVersion>
    <authorization>
        <username>{username}</username>
        <password>{password}</password>
    </authorization>
<notificationDetail>
               <contactCycleDelay>1800</contactCycleDelay>
               <contactAttemptCycles> 1 </contactAttemptCycles>
               <textDeviceDelay>420</textDeviceDelay>
               <priority>STANDARD</priority>
               <useAlternates>false</useAlternates>
               <playGreeting>true</playGreeting>
               <division>/</division>
               <title>{incidentForCreate.Title}</title>
               <emailImportance>HIGH</emailImportance>
               <verbiage>
                  <text locale=""en_US"" messageType=""Default"">{incidentForCreate.Verbiage}
WebEx Room or Call Back Information: 
{incidentForCreate.MeetingLink} {incidentForCreate.MeetingPhone} code {incidentForCreate.MeetingCode}
Ticket Number: {incidentForCreate.IncidentNumber}
Thank you,
GEICO NOC
                  </text>
               </verbiage>
               <requiresPin>false</requiresPin>
               <broadcastInfo>
                  <broadcastDuration>1800</broadcastDuration>
                  <recipients>
                     <recipientGroup>{incidentForCreate.RecipientGroup}</recipientGroup>
                  </recipients>
               </broadcastInfo>
               <leaveMessage>
                  <message>true</message>
                  <callbackInfo>false</callbackInfo>
               </leaveMessage>
               <validateRecipient>false</validateRecipient>
               <locationOverride>
                  <overrideDefaultStatusOnly>false</overrideDefaultStatusOnly>
                  <devices>
                     <override>
                        <deviceType>Work Phone</deviceType>
                        <priority>OFF</priority>
                     </override>
                     <override>
                        <deviceType>Mobile Phone</deviceType>
                        <priority>1</priority>
                     </override>
                     <override>
                        <deviceType>Home Phone</deviceType>
                        <priority>OFF</priority>
                     </override>
                     <override>
                        <deviceType>TTY Phone</deviceType>
                        <priority>OFF</priority>
                     </override>
                     <override>
                        <deviceType>Work Email</deviceType>
                        <priority>1</priority>
                     </override>
                     <override>
                        <deviceType>Home Email</deviceType>
                        <priority>OFF</priority>
                     </override>
                     <override>
                        <deviceType>1-Way Pager</deviceType>
                        <priority>OFF</priority>
                     </override>
                     <override>
                        <deviceType>2-Way Pager</deviceType>
                        <priority>OFF</priority>
                     </override>
                     <override>
                        <deviceType>Numeric Pager</deviceType>
                        <priority>OFF</priority>
                     </override>
                     <override>
                        <deviceType>SMS</deviceType>
                        <priority>1</priority>
                     </override>
                     <override>
                        <deviceType>Fax</deviceType>
                        <priority>OFF</priority>
                     </override>
                     <override>
                        <deviceType>Blackberry Pin-to-Pin</deviceType>
                        <priority>OFF</priority>
                     </override>
                     <override>
                        <deviceType>Mobile App</deviceType>
                        <priority>OFF</priority>
                     </override>
                     <override>
                        <deviceType>Desktop Alert</deviceType>
                        <priority>OFF</priority>
                     </override>
                  </devices>
               </locationOverride>
               <suppressWarningsForEmail>false</suppressWarningsForEmail>              
               <stopIfFullHuman>true</stopIfFullHuman>
               <stopIfPartialHuman>false</stopIfPartialHuman>
               <stopIfFullMachine>true</stopIfFullMachine>
               <stopIfPartialMachine>false</stopIfPartialMachine>
               <replayMessage>true</replayMessage>
               <callAnalysis>true</callAnalysis>
               <reportRecipientsFlag>false</reportRecipientsFlag>
               <selectLanguage>false</selectLanguage>
               <useTopics>false</useTopics>
               <sendToSubscribers>false</sendToSubscribers>
               <strictDeviceDelay>false</strictDeviceDelay>
               <textDevicesOnce>true</textDevicesOnce>
               <callRoutingRollover>false</callRoutingRollover>
               <confirmResponse>false</confirmResponse>
               <currentUserCanViewAllRecipients>true</currentUserCanViewAllRecipients>
               <currentUserCanViewAllTopics>true</currentUserCanViewAllTopics>
               <currentUserCanEditNotification>true</currentUserCanEditNotification>
               <currentUserCanSendNotification>true</currentUserCanSendNotification>
               <currentUserCanViewThisNotificationReport>true</currentUserCanViewThisNotificationReport>
               <identicalDeviceSuppression>true</identicalDeviceSuppression>
            </notificationDetail>
</oneStepNotification>
</soapenv:Body>
</soapenv:Envelope>";
            return incidentCreateSoapRequest;
        }

        public static string MapCreateIncidentResponseToIncidentNumber(string intiateIncidentXmlResponse)
        {
            return IncidentResponseServiceDocumentParser.ExtractCreatedIncidentNumber(intiateIncidentXmlResponse);
        }
    }
}
