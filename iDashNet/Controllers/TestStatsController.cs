using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;

// For more information on Client Platforms: Tests Statuses: https://confluence/display/~MStollman/Software+Quality+Metrics+API

namespace iDashNet.Controllers
{
    public class TestStatsController : Controller
	{
		// GET: /TestStats/
		public string TestStatsIndex()
		{

			// CP Quality Metrics
			// Base Url: http://10.9.11.51:4000/api/v1.5/
            /*

			Endpoint Description:
            URL - GET
            MyQL / myql
            Rocket Mortgage Web / rocketweb
            Rocket Mortgage Mobile / rocketmobile
            Marketing Technology    / marketingtech
            Lander / lander
            ORM / orm
            QLMS / qlms
            QL.Com / ql
            */


			HttpClient client = new HttpClient();

			client.DefaultRequestHeaders.Accept.Clear();

            string baseURL = "http://10.9.11.51:4000/api/v2/";
            string relativeURL = "myql";



			var Task = client.GetAsync(baseURL + relativeURL);
			Task.Wait();
			var data = Task.Result;
			var contentTask = data.Content.ReadAsStringAsync();
			contentTask.Wait();
			var jsonData = contentTask.Result;

			//var JsonResult = JsonConvert.DeserializeObject<String>(jsonData);

			return jsonData;
		}
	}
}
