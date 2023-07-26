using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using RestSharp;

namespace iDashNet.Controllers
{
    public class CherwellController : Controller
    {
        // GET: /Cherwell/
        //public async Task<IActionResult> CherwellIndex()
        public string CherwellIndex()
        {

            // API Key: 616302a7-e09b-42e1-9fae-941e39e77e46
            // API token lifespan is set to 60 minutes
            // Refresh token lifespan is set to 1440 minutes

            /* 
            // Good Links <Please do not remove>
            // 
            // Cherwell BETA Swagger: http://ql1cwbeta1/CherwellAPI/Swagger/ui/index#
            // Cherwell PROD Swagger: http://ql1cw1/CherwellAPI/Swagger/ui/index#/
            // 
            // Cherwell Examples: https://cherwellsupport.com/CherwellAPI/Documentation/en-US/csm_rest_ad_hoc_search_with_filter.html
            // 
            // Cherwell Community Forums: https://www.cherwell.com/community/builders-network/f/rest_web_api
            //
            // Cherwell API call to get CRIs: http://servicedesk/CherwellAPI/api/V1/getsearchresults/association/934ec7a1701c451ce57f2c43bfbbe2e46fe4843f81/scope/User/scopeowner/942f82fdfe35eb4334cd5c4bd8bbb14e95b5d5c087/searchname/Client%20Platforms%20CRIs 
            //
            // Good Links <Please do not remove>
            */

            HttpClient client = new HttpClient();

            var token = GetCherwellToken();

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("authorization", "bearer " + token.Result);

            string baseURL = "http://ql1cw1/CherwellAPI/api/V1/";
            string relativeURL = "getsearchresults/association/934ec7a1701c451ce57f2c43bfbbe2e46fe4843f81/scope/User/scopeowner/942f82fdfe35eb4334cd5c4bd8bbb14e95b5d5c087/searchname/Client%20Platforms%20CRIs";

            var Task = client.GetAsync(baseURL + relativeURL);
            Task.Wait();
            var data = Task.Result;
            var contentTask = data.Content.ReadAsStringAsync();
            contentTask.Wait();
            var jsonData = contentTask.Result;

            //var JsonResult = JsonConvert.DeserializeObject<String>(jsonData);

            return jsonData;

        }


        [Route("api/Cherwell/GetCherwellToken")]
        [HttpGet]

        public async Task<string> GetCherwellToken()
        {

            try
            {
                string username = "idashboard";

                string password = "Ida$hboard";

                string clientId = "a4c4030c-bf2c-4f7d-a95f-524ea53bf653";

                string token = "";


                var client = new RestClient("http://ql1cw1/CherwellAPI/token");
                var request = new RestRequest(Method.POST);
                //request.AddHeader("cache-control", "no-cache");
                request.AddHeader("accept", "application/json");
                request.AddHeader("content-type", "application/x-www-form-urlencoded");
                request.AddParameter("grant_type", "password");
                request.AddParameter("client_id", clientId);
                request.AddParameter("username", username);
                request.AddParameter("password", password);

                RestResponse response = await RestClientExtensions.ExecuteAsync(client, request);

                dynamic deserializedResponse = JsonConvert.DeserializeObject(response.Content);

                token = deserializedResponse["access_token"];

                return token;


            }
            catch(Exception e)
            {
                throw e;
            }
        }



    }
}
