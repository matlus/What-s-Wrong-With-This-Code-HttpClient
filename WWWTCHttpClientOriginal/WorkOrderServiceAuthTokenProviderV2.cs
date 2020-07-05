using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace WWWTCHttpClientOriginal
{
    /*
     ******* WorkOrderServiceAuthTokenProvider ********
     * 1. public methods should not be virtual     * 2. 
     * 2. Use "var" when you can
     * 3. Leave blanks lines after lines that end in "};"
     * 4. Don't Invent names - Rename client to httpClient
     * 5. Don't Invent names - Rename req to httpRequestMessage
     * 6. Don't Invent names - Rename response to httpResponseMessage
     * 7. Don't Invent names - Rename content to httpContent
     * 8. Don't Invent names - Rename createRequest to workOrderForCreateJson
     * 9. Don't Invent names - GetAccessTokenAsync method parameter settings
     * 10. Define local const "mediaType" for "application/json"
     * 11. HttpClient define BaseUri property, then use only the trailing part of Url
     * 12. No need to use Newtonsoft to serialize
     * ************* Bugs *******************
     * 1. HttpClient should NOT be created each time - Cache instance as class member
     * 2. Not checking HttpResponseMessage after SendAsync (in both methods)
     * 3. HttpClient is Disposable and MUST be Disposed
     * 4. HttpRequestMessage is Disposable. They MUST be disposed
     * 5. HttpResponseMessage is Disposable. They MUST be disposed
     * 6. Need ConfigureAwait(false) in all awaited methods (unless project is .NET Core)
     * ************ Design ******************
     * 1. Class should be internal
     * 2. Class should be sealed
     * 3. Only the CreateWorkOrder method needs to/should be public.
     * 4. Is the class name correct? This should be a Gateway, not a "provider"
     * 5. No need for virtual methods in this class
     * 6. Public methods should NOT be virtual
     * 7. GetAccessToken - Isn't this method essentially Log in?
     * 8. Logout method - Should not ask for url, this is intrinsic to the method itself
     * 9. Leaky iky - WorkOrderCreateRequest - Don't leak models from a service into the application
     * 10. Levels of Abstraction GetAccessTokenAsync and Logout are correct, the rest need to be abstracted to another method
     * 11. Extract the creation and initialization of HttpRequestMessage to a private static method
    */

    // TODO: PWI-Design: Class should be internal
    // TODO: PWI-Design: Class should be sealed
    // TODO: PWI-Design: Is the class name correct? This should be a Gateway, not a "provider"
    public class WorkOrderServiceAuthTokenProviderV2
    {
        // TODO: PWI: Public methods should not be virtual
        // TODO: PWI: Don't Invent names - settings parameter should be called workOrderSettings
        // TODO: PWI: Need to know Basis - No need to ask for the entire WorOrderSettings class, just username and password
        // TODO: PWI-Design: Only the CreateWorkOrder method needs to/should be public.
        // TODO: PWI-Design: No need for virtual methods in this class
        // TODO: PWI-Design: GetAccessToken - Isn't this method essentially Log in?
        public virtual async Task<string> GetAccessTokenAsync(WorkOrderSettings settings)
        {
            var credentials = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("username", settings.UserName),
                new KeyValuePair<string, string>("password", settings.Password)
            };
            // TODO: PWI: Leave blanks lines after lines that end in "};"
            // TODO: PWI: Don't Invent names - Rename client to httpClient
            // TODO: PWI-Bug: HttpClient should NOT be created each time - Cache instance as class member
            // TODO: PWI-Bug: HttpClient is Disposable and MUST be Disposed
            var client = new HttpClient();
            // TODO: PWI: HttpClient define BaseUri property, then use only the trailing part of Url
            // TODO: PWI: Don't Invent names - Rename req to httpRequestMessage
            // TODO: PWI: HttpRequestMessage is Disposable. They MUST be disposed
            var req = new HttpRequestMessage(HttpMethod.Post, settings.BaseUrl + "/login")
            {
                Content = new FormUrlEncodedContent(credentials)
            };
            // TODO: PWI: Leave blanks lines after lines that end in "};"
            // TODO: PWI: Don't Invent names - Rename response to httpResponseMessage
            // TODO: PWI-Bug: Need ConfigureAwait(false) in all awaited methods (unless project is .NET Core)
            var response = await client.SendAsync(req);
            // TODO: PWI-Bug: Not checking HttpResponseMessage after SendAsync (in both methods)
            // TODO: PWI-Bug: Need ConfigureAwait(false) in all awaited methods (unless project is .NET Core)
            var contents = await response.Content.ReadAsStringAsync();
            return contents;
        }

        // TODO: PWI: Public methods should not be virtual
        // TODO: PWI-Design: Only the CreateWorkOrder method needs to/should be public.
        // TODO: PWI-Design: No need for virtual methods in this class
        // TODO: PWI-Design: Logout method - Should not ask for url, this is intrinsic to the method itself
        public virtual async Task Logout(string token, string logoutUrl)
        {
            // TODO: PWI: Don't Invent names
            // TODO: PWI: HttpRequestMessage is Disposable. They MUST be disposed
            var req = new HttpRequestMessage();
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            req.RequestUri = new Uri(logoutUrl);
            req.Method = HttpMethod.Post;
            // TODO: PWI: Don't Invent names
            // TODO: PWI-Bug: HttpClient should NOT be created each time - Cache instance as class member
            // TODO: PWI-Bug: HttpClient is Disposable and MUST be Disposed
            var client = new HttpClient();
            // TODO: PWI-Bug: Need ConfigureAwait(false) in all awaited methods (unless project is .NET Core)
            await client.SendAsync(req);
            // TODO: PWI-Bug: Not checking HttpResponseMessage after SendAsync (in both methods)
        }

        // TODO: PWI-Design: Only the CreateWorkOrder method needs to/should be public.
        // TODO: PWI-Design: Leaky iky - WorkOrderCreateRequest - Don't leak models from a service into the application
        // TODO: PWI-Design: Levels of Abstraction GetAccessTokenAsync and Logout are correct, the rest need to be abstracted to another method
        public async Task<string> CreateWorkOrder(WorkOrderCreateRequest workOrderCreateRequest, WorkOrderSettings workOrderSettings)
        {
            // TODO: PWI: Don't Invent names
            // TODO: PWI: HttpRequestMessage is Disposable. They MUST be disposed
            var req = new HttpRequestMessage();
            // TODO: PWI: Don't Invent names - name this local variable accessToken
            // TODO: PWI-Bug: Need ConfigureAwait(false) in all awaited methods (unless project is .NET Core)
            string access = await GetAccessTokenAsync(workOrderSettings);
            // TODO: PWI-Design: Extract the creation and initialization of HttpRequestMessage to a private static method
            req.Headers.Authorization = new AuthenticationHeaderValue("AR-JWT", access);
            req.RequestUri = new Uri(workOrderSettings.BaseUrl);
            req.Method = HttpMethod.Post;

            // TODO: PWI: No need to use Newtonsoft to serialize
            // TODO: PWI: Define local const "mediaType" for "application/json"
            // TODO: PWI: Use var when you can
            string createRequest = JsonConvert.SerializeObject(workOrderCreateRequest);
            req.Content = new StringContent(createRequest, Encoding.UTF8, "application/json");
            req.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // TODO: PWI: Don't Invent names
            // TODO: PWI-Bug: HttpClient should NOT be created each time - Cache instance as class member
            // TODO: PWI-Bug: HttpClient is Disposable and MUST be Disposed
            var httpClient = new HttpClient();
            // TODO: PWI: Don't Invent names            
            var response = await httpClient.SendAsync(req);
            // TODO: PWI-Bug: Not checking HttpResponseMessage after SendAsync (in both methods)

            // TODO: PWI: HttpClient define BaseUri property, then use only the trailing part of Url
            await Logout(access, workOrderSettings.BaseUrl + "/logout");
            // TODO: PWI-Bug: EnsureSuccessStatusCode() should be called after SendAsync on line 117
            response.EnsureSuccessStatusCode();
            
            // TODO: PWI-Bug: Need ConfigureAwait(false) in all awaited methods (unless project is .NET Core)
            return await response.Content.ReadAsStringAsync();
        }
    }
}
