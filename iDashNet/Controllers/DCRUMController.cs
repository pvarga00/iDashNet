﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.IO;
using Newtonsoft.Json.Linq;

//using System.Web;

namespace iDashNet.Controllers
{
    public class DCRUMController : Controller
    {
        // GET: /DCRUM/
        public string DCRUMIndex()
        {

            try
            {
                // DCRUM Docs: https://community.dynatrace.com/community/display/DCRUM123/REST+Developer+Tools+and+Tips
                // DCRUM Console: http://dcrumconsole/ (uses windows login + basic auth)
                // DCRUM: Using REST APIs: https://community.dynatrace.com/community/display/DCRUMDOC/Using+REST-based+web+services
                // Security Token (FYI: This is not used!): eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJzdWIiOiJSTWFyY2llIiwidWlkIjoiZjFiOTY5ZjllYTQ1YTdmYWRhZThkNWViNDg3OTllYWIiLCJ0eXAiOiJMT0MiLCJyb2wiOlsiU1UiXX0.eLl4iGQCgmrtayz8gCoeQwF5jawuAI4aNuwCrARl_No
                // string dcrumLogin = "DCRUM-API-access";
                // string dcrumPwd = "DCRUM2017*api";

                // These are the application calls we need to make:
                // TODO: FIGURE THIS OUT : What/How to make API calls to get metrics

                // DocViewer: Component Downtime
                // DocViewer: Document Request Errors
                // DocViewer: Page Load Speed

                // LOLA: Page Load Speed
                // 


                HttpClient client = new HttpClient();

                client.DefaultRequestHeaders.Accept.Clear();

                string dcrumLogin = "DCRUM-API-access";
                string dcrumPwd = "DCRUM2017*api";


                client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue(
                            "Basic",
                            Convert.ToBase64String(
                                System.Text.ASCIIEncoding.ASCII.GetBytes(
                                      string.Format("{0}:{1}", dcrumLogin, dcrumPwd)))); // 

                //String BodyJsonData = System.IO.File.ReadAllText("../../SupportFiles/TextFile1.txt"); // C:\CodeIT\IT\TextFile1.txt


                var BodyJsonData = new
                {
                    appId = "CVENT",
                    viewId = "ClientView",
                    dataSourceId = "ALL_AGGR",
                    dimensionIds = new string[2] { "appl", "sDNSName" },
                    metricIds = new string[2] { "c_sByte", "trans" },
                    dimFilters = new object[2] {


                               new object[3] { "appl", "WWW_HTTP", false },


                               new object[3] { "sDNSName","",false}
                },
                    metricFilters = new string[0],
                    sort = new string[0],
                    top = 1000,
                    resolution = "r",
                    timePeriod = "Today",
                    numberOfPeriods = 0,
                    timeBegin = 1136264400000,
                    timeEnd = 1136350800000
                };


                var JsonString = JsonConvert.SerializeObject(BodyJsonData);

                StringContent content = new StringContent(JsonString, System.Text.Encoding.UTF8, "application/json");


                var Task = client.PostAsync("http://dcrumconsole/rest/dmiquery/getApplication", content);



                Task.Wait();

                var data = Task.Result;

                var contentTask = data.Content.ReadAsStringAsync();
                contentTask.Wait();

                var jsonData = contentTask.Result;

                dynamic jsonObject
                    = JObject.Parse(jsonData);

                //var JsonResult = JsonConvert.DeserializeObject<String>(jsonData);

                return jsonObject[0].metricValues[0].value;
            }
            catch (Exception e)
            {

                return e.Message;
            }
            


            //return "";
        }


        
    }
}
