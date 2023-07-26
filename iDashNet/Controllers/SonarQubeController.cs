﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Net;
//using InfluxDB.Collector;

namespace iDashNet.Controllers
{
    // SonarQube API Docs: http://sonar/web_api/api/measures
    // SonarQube Metrics Docs / Definitions: https://docs.sonarqube.org/display/SONAR/Metric+Definitions

    public class SonarQubeController : Controller
    {
        private MySettings _appSettings;
 
        public iDashNet.Models.SonarQubeModel model;

        public SonarQubeController(IOptions<MySettings> settings, IOptions<List<SonarQubeApp>> sonarApps)
        {
            this._appSettings = settings.Value;

            this.model = new Models.SonarQubeModel();

            this.model.sonarApps = new List<SonarQubeApp>();

            Parallel.ForEach<SonarQubeApp>(sonarApps.Value, (app) =>
            {

                this.model.sonarApps.Add(this.GetApp(app.Name, app.SonarKey, app.SonarUrl));
            });
        }


        [HttpGet]
        [Route("api/SonarQube/GetApp")]
        private SonarQubeApp GetApp(string name, string sonarKey, string sonarUrl)
        {
            Dictionary<string, string> metricValues = GetMetrics(sonarKey, "complexity", "coverage", "violations", "sqale_index", "tests", "test_errors", "test_success_density");


            return new SonarQubeApp
            {
                SonarKey = sonarKey,
                Name = name,
                SonarUrl = sonarUrl,
                Complexity = metricValues["complexity"],
                Coverage = metricValues["coverage"] + "%",
                Violations = metricValues["violations"],
                TechDebt = metricValues["sqale_index"] + " (hours)",
                TotNumberUnitTests = metricValues["tests"],
                UnitTestErrors = metricValues["test_errors"],
                UTPassPercent = metricValues["test_success_density"]
            };
        }


        public IActionResult SonarQubeIndex()
        {
            return View(model);
        }



        [HttpGet]
        [Route("api/SonarQube/GetMetrics")]
        // GET: /SonarQube/
        //Returns dictionary with metric name as key, metric value as value
        public static Dictionary<string, string> GetMetrics(string sonarKey, params string[] metrics)
        {
            try
            {
                // string sonarQubeToken = config.GetValue<string>("AppSettings:SonarQubeToken");
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(
                        "Basic",
                        Convert.ToBase64String(
                            System.Text.ASCIIEncoding.ASCII.GetBytes(
                                string.Format("{0}:{1}", "91b946edaab687290698c9525257b19d0eae969c", "")))); // Username, Pwd 

                string metricStr = String.Join(",", metrics);

                string UrlString = "http://sonar/api/measures/component?componentKey=" + WebUtility.UrlEncode(sonarKey) + String.Format("&metricKeys={0}", metricStr);
	            var Task = client.GetAsync(UrlString);

	            Task.Wait();
	            var data = Task.Result;
	            var contentTask = data.Content.ReadAsStringAsync();
	            contentTask.Wait();
	            var jsonData = contentTask.Result;

	            dynamic JsonResult = JsonConvert.DeserializeObject(jsonData);

                dynamic metricObjects = JsonResult["component"]["measures"];


                Dictionary<string, string> metricValues = new Dictionary<string, string>();
                //Dictionary<string, object> metricValuesDB = new Dictionary<string, object>();


                foreach(string m in metrics)
                {
                    metricValues.Add(m, null);
                    //metricValuesDB.Add(m, null);
                }


                foreach(dynamic measure in metricObjects)
                {
                    metricValues[measure["metric"].ToString()] =  measure["value"].ToString();
                    //metricValuesDB[measure["metric"].ToString()] = measure["value"].ToString();
                }

                /*
				// https://github.com/influxdata/influxdb-csharp
				// https://docs.influxdata.com/influxdb/v1.3/write_protocols/line_protocol_tutorial/

				Metrics.Collector = new CollectorConfiguration()
					.Tag.With("host", Environment.GetEnvironmentVariable("COMPUTERNAME"))
					.Batch.AtInterval(TimeSpan.FromSeconds(2))
					.WriteTo.InfluxDB("https://idash.squigglelines.com", "idash")
					.CreateCollector();
                

				Metrics.Collector.Write("SonarMetrics", metricValuesDB, null, DateTime.Now);

				/*
                 * Install Telegraf onto BT server > As a service: 
                 *          telegraf.exe -service install -config "c:\Program Files\Telegraf\telegraf.conf" -config-directory "c:\Program Files\Telegraf\telegraf.d"
                 * 
                 * Start writing data to InfluxDB
                 * 
                 * View it with Grafana instance
                 * 
                 * Set up a follow meeting with Tim to get more done
                 * 
                 *
                 * */


				return metricValues;
		    }
            catch (Exception e)
            {

                //return "Error finding coverage metrics for " + sonarKey + e.Message;

                throw e;
            }
        }


        [Route("api/SonarQube/GetMetric")]
        [HttpGet]
        public string GetMetric(string sonarKey, string metric)
        {
            try{
	            HttpClient client = new HttpClient();
	            client.DefaultRequestHeaders.Accept.Clear();

	            client.DefaultRequestHeaders.Authorization =
	                new AuthenticationHeaderValue(
	                    "Basic",
	                    Convert.ToBase64String(
	                        System.Text.ASCIIEncoding.ASCII.GetBytes(
	                            string.Format("{0}:{1}", _appSettings.SonarQubeToken, "")))); // Username, Pwd 

                string UrlString = "http://sonar/api/measures/component?componentKey=" + WebUtility.UrlEncode(sonarKey) + "&metricKeys=" + WebUtility.UrlEncode(metric);
	            var Task = client.GetAsync(UrlString);

	            Task.Wait();
	            var data = Task.Result;
	            var contentTask = data.Content.ReadAsStringAsync();
	            contentTask.Wait();
	            var jsonData = contentTask.Result;

	            dynamic jsonObject = JObject.Parse(jsonData);

                try{
                    return jsonObject.component.measures[0].value;
                }
                catch{
                    return "N/A";
                }
			}
			catch (Exception e)
			{
				return "Error finding coverage metrics for " + sonarKey + e.Message;
			}
        }
    }
}
