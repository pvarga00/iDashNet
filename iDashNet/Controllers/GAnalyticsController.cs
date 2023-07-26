using System;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Microsoft.Extensions.Options;

// Google APIs
using Google.Apis.Discovery;
using Google.Apis.Services;
using Google.Apis.Auth;
using Google.Apis.Analytics.v2_4;
using Google.Apis.Auth.OAuth2;

using Google.Apis.Vision.v1;


namespace iDashNet.Controllers
{

    public class GAnalyticsController : Controller
    {
        private iDashNet.Models.GAnalyticsModel model;

        public GAnalyticsController(IOptions<MySettings> settings, IOptions<List<GoogleAnalyticsApp>> googleAnalyticsApps)
        {
            model = new iDashNet.Models.GAnalyticsModel();

            model.gaApps = new List<GoogleAnalyticsApp>();

            Parallel.ForEach<GoogleAnalyticsApp>(googleAnalyticsApps.Value, (analyticsApp) =>
            {
                var app = populateGAApp(analyticsApp);
                model.gaApps.Add(app);
            });


        }


		public IActionResult GAnalyticsIndex()
        {
            return View(model);
        }

        private HttpClient HttpClientSetup()
        {
            var client = new HttpClient(new HttpClientHandler() { UseDefaultCredentials = true });

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }

        public static GoogleCredential GAAuth()
        {
            GoogleCredential credential;

            try
            {
                // https://developers.google.com/identity/protocols/OAuth2ServiceAccount

                using (var stream = new FileStream("wwwroot/json/client_secret_iDash.json", FileMode.Open, FileAccess.Read))
                {
                    credential = GoogleCredential.FromStream(stream)
                        .CreateScoped(AnalyticsService.Scope.Analytics);
                }
                return credential;
            }
            catch(Exception e)
            {
                throw e;
            }
        }


        public GoogleAnalyticsApp populateGAApp(GoogleAnalyticsApp analyticsApp)
        {
            // Use this to build queries: https://ga-dev-tools.appspot.com/query-explorer/ : Pick a desired application + metrics

            List<string> googleAnalyticsMetrics = GetGAnalyticsMetrics(analyticsApp.AppId, analyticsApp.Name, "avgPageDownloadTime", "avgPageLoadTime", "avgTimeOnPage", "pageLoadTime", "uniquePageViews");

            double avgPageDownloadTime = 0;
            double avgPageLoadTime = 0;
            double avgTimeOnPage = 0;
            double pageLoadTime = 0;
            double uniquePageViews = 0;

            Double.TryParse(googleAnalyticsMetrics[0], out avgPageDownloadTime);
            Double.TryParse(googleAnalyticsMetrics[1], out avgPageLoadTime);
            Double.TryParse(googleAnalyticsMetrics[2], out avgTimeOnPage);
            Double.TryParse(googleAnalyticsMetrics[3], out pageLoadTime);
            Double.TryParse(googleAnalyticsMetrics[4], out uniquePageViews);

            analyticsApp.avgPageDownloadTime = avgPageDownloadTime.ToString("#,##0.00") + " (ms)";
            analyticsApp.avgPageLoadTime = avgPageLoadTime.ToString("#,##0.00") + " (ms)";
            analyticsApp.avgTimeOnPage = avgTimeOnPage.ToString("#,##0.00") + " (ms)";
            analyticsApp.pageLoadTime = pageLoadTime.ToString("#,##0.00") + " (ms)";
            analyticsApp.uniquePageViews = uniquePageViews.ToString("#,##0.00");


            return analyticsApp;
        }



        /* Get and individual metric from the Google Analytics API */
        [HttpGet]
        [Route("api/GoogleAnalytics/GetMetric")]
        public static string GetMetric(string appId, string appName, string metric, string start_date = "30daysAgo", string end_date="yesterday")
        {
            var credential = GAAuth();

            var service = new AnalyticsService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = appName,
                //ApiKey = "AIzaSyAdLMP-_LVYbOUbEcDU3u1nc88pLW35dbM"
            });

            var Task = service.HttpClient.GetAsync("https://www.googleapis.com/analytics/v3/data/ga?ids=ga%3A" + appId + "&start-date=" + start_date + "&end-date=" + end_date + "&metrics=ga%3A" + metric);

            Task.Wait();

            var data = Task.Result;

            dynamic contentTask = data.Content.ReadAsStringAsync();
            contentTask.Wait();

            dynamic results = JsonConvert.DeserializeObject(contentTask.Result);

            dynamic totals = results["totalsForAllResults"];

            var response = "";
            try
            {
                response = totals["ga:" + metric];
            }
            catch (Exception e)
            {
                return "0";
            }

            return response;
        }



       /* Get a variable list of metrics from the Google Analytics API */
        [HttpGet]
        [Route("api/GoogleAnalytics/GetAnalyticsMetrics")]
        public static List<string> GetGAnalyticsMetrics(string appId, string appName, params string[] metrics)
        {
            try
            {
                var credential = GAAuth();


                var service = new AnalyticsService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = credential,
                    ApplicationName = appName,
                    //ApiKey = "AIzaSyAdLMP-_LVYbOUbEcDU3u1nc88pLW35dbM"
                });


                string metricStr = "";

                for (int i = 0; i < metrics.Length; i++)
                {

                    if (i > 0 && i < metrics.Length)
                    {
                        metricStr += "%2Cga%3A" + metrics[i];
                    }
                    else
                    {
                        metricStr += "ga%3A" + metrics[i];
                    }

                }


                var Task = service.HttpClient.GetAsync("https://www.googleapis.com/analytics/v3/data/ga?ids=ga%3A" + appId + "&start-date=30daysAgo&end-date=yesterday&metrics=" + metricStr);

                Task.Wait();

                var data = Task.Result;

                dynamic contentTask = data.Content.ReadAsStringAsync();
                contentTask.Wait();

                dynamic results = JsonConvert.DeserializeObject(contentTask.Result);

                List<string> returnList = new List<string>();

                foreach (string metric in results.rows[0])
                {
                    returnList.Add(metric);
                }

                return returnList;
            }
            catch (Exception e)
            {

                return new List<string>(new string[] { e.Message });
            }
        }



    }
}

