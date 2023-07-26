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
using Google.Apis.Discovery;
using Google.Apis.Services;
using Google.Apis.Auth;
using Google.Apis.Analytics.v2_4;
using Google.Apis.Auth.OAuth2;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Net;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
// Good CSS Info: http://materializecss.com/


namespace iDashNet.Controllers
{
    public class HomeController : Controller
    {

        Models.HomeModel model;
        private MySettings _appSettings;

        public HomeController(IOptions<List<HomePageApp>> homePageApps, IOptions<List<AppDynamicsApp>> appDynamicsApps, IOptions<List<GoogleAnalyticsApp>> googleAnalyticsApps, IOptions<List<SonarQubeApp>> sonarApps, IOptions<MySettings> settings)
        {
            this._appSettings = settings.Value;
            var populatedHomePageApps = new List<HomePageApp>();

            model = new Models.HomeModel();
            model.homePageApps = new List<HomePageApp>();

            Parallel.ForEach<HomePageApp>(homePageApps.Value, (homeApp) =>
            {
                model.homePageApps.Add(populateHomePageApp(homeApp));
            });
        }

        public HomePageApp populateHomePageApp(HomePageApp app)
        {

            /*
            List<string> gaMetrics = GAnalyticsController.GetGAnalyticsMetrics(app.ga_appId, app.ga_appName, "avgPageDownloadTime", "avgPageLoadTime", "avgTimeOnPage", "pageLoadTime", "uniquePageViews");

            double ga_avgPageDownloadTime = 0;
            double ga_avgPageLoadTime = 0;
            double ga_avgTimeOnPage = 0;
            double ga_pageLoadTime = 0;
            double ga_uniquePageViews = 0;

            Double.TryParse(gaMetrics[0], out ga_avgPageDownloadTime);
            Double.TryParse(gaMetrics[1], out ga_avgPageLoadTime);
            Double.TryParse(gaMetrics[2], out ga_avgTimeOnPage);
            Double.TryParse(gaMetrics[3], out ga_pageLoadTime);
            Double.TryParse(gaMetrics[4], out ga_uniquePageViews);


            //Google Analytics
            app.ga_avgPageDownloadTime = ga_avgPageDownloadTime.ToString("#,##0.00") + " (ms)";
            app.ga_avgPageLoadTime = ga_avgPageLoadTime.ToString("#,##0.00") + " (ms)";
            app.ga_avgTimeOnPage = ga_avgTimeOnPage.ToString("#,##0.00") + " (ms)";
            app.ga_pageLoadTime = ga_pageLoadTime.ToString("#,##0.00") + " (ms)";
            app.ga_uniquePageViews = ga_uniquePageViews.ToString("#,##0.00");

            //App Dynamics
            double appD_avgResponseTime = 0;
            Double.TryParse(AppDynamicsController.GetMetric(app.appD_name, "Average Response Time (ms)"), out appD_avgResponseTime);
            app.appD_avgResponseTime = appD_avgResponseTime.ToString("#,##0.00") + " (ms)";

            */


            Dictionary<string, string> sonarMetrics = SonarQubeController.GetMetrics(app.sonar_key, "complexity", "coverage", "violations", "sqale_index", "tests", "test_errors", "test_success_density");
            //Sonar Qube

            int complexity = 0;
            double coverage = 0;
            int violations = 0;
            double techdebt = 0;
            int tests = 0;
            int test_errors = 0;
            double test_success_density = 0;


            Int32.TryParse(sonarMetrics["complexity"], out complexity);
            Double.TryParse(sonarMetrics["coverage"], out coverage);
            Int32.TryParse(sonarMetrics["violations"], out violations);
            Double.TryParse(sonarMetrics["sqale_index"], out techdebt);

            Int32.TryParse(sonarMetrics["tests"], out tests);
            Int32.TryParse(sonarMetrics["test_errors"], out test_errors);
            Double.TryParse(sonarMetrics["test_success_density"], out test_success_density);


            app.sonar_complexity = complexity.ToString("#,##0.00");
            app.sonar_coverage = coverage.ToString("#,##0.00") + "%";
            app.sonar_violations = violations.ToString("#,##0");
            app.sonar_techDebt = techdebt.ToString("#,##0");

            app.sonar_totalTests = tests.ToString("#,##0");
            app.sonar_failingUTs = test_errors.ToString("#,##0");
            app.sonar_percPass = test_success_density.ToString("#,##0.00");


            Dictionary<string, string> CPTestMetrics = GetClientPlatformMetrics(app.testStatusRelativeUrl);
            app.bugs = CPTestMetrics["bugs"];
            app.app_functional_coverage = CPTestMetrics["app_functional_coverage"];

            return app;
        }



        [Route("api/Home/GetAppDynamicsMetric")]
        [HttpGet]
        public string GetAppDynamicsMetric(string appName, string metric)
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

                return jsonArray[0].metricValues[0].value;
            }
            catch (Exception e)
            {

                return e.Message;
            }
        }

        public Dictionary<string, string> GetClientPlatformMetrics (string AppName)
        {
            Dictionary<string, string> CPDict = new Dictionary<string, string>();


			HttpClient client = new HttpClient();

			client.DefaultRequestHeaders.Accept.Clear();

			string baseURL = "http://10.9.11.51:4000/api/v2/";
			string relativeURL = AppName;

			var appCoverageValue = "";
			var bugsValue = "";

			try
			{
    			var Task = client.GetAsync(baseURL + relativeURL);
    			Task.Wait();
    			var data = Task.Result;
    			var contentTask = data.Content.ReadAsStringAsync();
    			contentTask.Wait();
    			var jsonData = contentTask.Result;

                dynamic JsonResult = JObject.Parse(jsonData);

				//appCoverageValue = JsonResult["app_functional_coverage"].value;
				//bugsValue = JsonResult["bugs"].value;
                appCoverageValue = JsonResult.app_functional_coverage.Value;
                bugsValue = JsonResult.bugs.Value;

				CPDict.Add("bugs", bugsValue);
				CPDict.Add("app_functional_coverage", appCoverageValue);

				return CPDict;
			}
			catch
			{
				CPDict.Add("bugs", "N/A");
				CPDict.Add("app_functional_coverage", "N/A");
                return CPDict;
			}

        }



        //Views
        public IActionResult HomeIndex()
        {
            return View(model);
        }


        public IActionResult About()
        {
            ViewData["Message"] = "iDash helps provide visibility into your application's metrics.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Contact Information for owners of the iDash dashboard application.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
