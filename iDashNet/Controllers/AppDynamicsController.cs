using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace iDashNet.Controllers
{

   
    public class AppDynamicsController : Controller
    {
        public iDashNet.Models.AppDynamicsModel model;

        private MySettings _appSettings;

        public AppDynamicsController(IOptions<MySettings> settings, IOptions<List<AppDynamicsApp>> appDApps)
        {
            this._appSettings = settings.Value;

            this.model = new Models.AppDynamicsModel();

            this.model.appdApps = new List<AppDynamicsApp>();

            foreach (AppDynamicsApp app in appDApps.Value)
            {

                this.model.appdApps.Add(this.GetApp(app.Name));
            }
        }


        
        [HttpGet]
        [Route("api/AppDynamics/GetApp")]
        public AppDynamicsApp GetApp(string name)
        {

            double responseTime = 0;

            Double.TryParse(GetMetric(name, "Average Response Time (ms)"), out responseTime);

   

            return new AppDynamicsApp
            {

                Name = name,
                AvgResponseTime = responseTime.ToString("#,##0.00") + "(ms)"

            };
        }

		// GET: /AppDynamics/
        [HttpGet]
		public IActionResult AppDynamicsIndex()
        {
            return View(model);
		}


        [HttpGet]
        [Route("api/AppDynamics/GetMetric")]
        public static string GetMetric(string appName, string metric)
        {
            // These are the application calls we need to make:
            // 
            // MyQL PROD (Avg Response Time): https://quicken.saas.appdynamics.com/controller/rest/applications/MyQL.com%20PROD/metric-data?metric-path=Overall%20Application%20Performance%7CAverage%20Response%20Time%20%28ms%29&time-range-type=BEFORE_NOW&duration-in-mins=15&output=json
            // Overall Application Performance|Average Response Time (ms)

            // Servicing PROD (Avg Response Time): https://quicken.saas.appdynamics.com/controller/rest/applications/Servicing%20PROD/metric-data?metric-path=Overall%20Application%20Performance%7Craven%7CAverage%20Response%20Time%20%28ms%29&time-range-type=BEFORE_NOW&duration-in-mins=15&output=json
            // Overall Application Performance|raven|Average Response Time (ms)

            // Servicing Running Rocket (Business Transaction) : https://quicken.saas.appdynamics.com/controller/rest/applications/Servicing%20PROD/metric-data?metric-path=Business%20Transaction%20Performance%7CBusiness%20Transactions%7Craven%7CRunning%20Rocket%20-%20Raven%7CAverage%20Response%20Time%20%28ms%29&time-range-type=BEFORE_NOW&duration-in-mins=15&output=json



            // AppD Auth Docs: 
            // https://docs.appdynamics.com/display/PRO42/Using+the+Controller+APIs - In order to access AppD data, you'll need to use the Controller's API
            // https://docs.appdynamics.com/display/PRO42/Managing+API+Keys


            try
            {
                HttpClient client = new HttpClient();

                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue(
                            "Basic",
                            Convert.ToBase64String(
                                System.Text.ASCIIEncoding.ASCII.GetBytes(
                                      string.Format("{0}@{1}:{2}", "idash_appd", "quicken", "3UsW5fekecut")))); // <your_username>@<your_accountname>:<your_password> Username=idash_appd @Account=quicken : Pwd=3UsW5fekecut


                appName = appName.Replace(" ", "%20");
                metric = metric.Replace(" ", "%20");
                appName = appName.Replace("(", "%28");

                appName = appName.Replace(")", "%29");

                metric = metric.Replace("(", "%28");
                metric = metric.Replace(")", "%29");





                var url = "https://quicken.saas.appdynamics.com/controller/rest/applications/" + appName + "/metric-data?metric-path=Overall%20Application%20Performance%7C" + metric + "&time-range-type=BEFORE_NOW&duration-in-mins=15&output=json";
                var Task = client.GetAsync(url);
                Task.Wait();
                var data = Task.Result;
                var contentTask = data.Content.ReadAsStringAsync();
                contentTask.Wait();
                var jsonData = contentTask.Result;

                dynamic jsonArray = JArray.Parse(jsonData);

                //var JsonResult = JsonConvert.DeserializeObject<String>(jsonData);

                return jsonArray[0].metricValues[0].value;                
            }
            catch (Exception e)
            {

                return "0";
            }
        }
    }
}
