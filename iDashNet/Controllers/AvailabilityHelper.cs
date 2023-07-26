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


namespace iDashNet.Controllers
{
    public static class AvailabilityHelper
    {

		static System.Uri MyQLBaseRESTURL = new Uri("https://myqlmwapi.rockfin.com/");
		static System.Uri MyQLBaseSOAPURL = new Uri("http://qlesigntest.rockfin.com/frontendservice.asmx");


		public static HttpClient HttpClientSetup()
		{
			var client = new HttpClient(new HttpClientHandler() { UseDefaultCredentials = true });
			//client.BaseAddress = MyQLBaseRESTURL;
			client.DefaultRequestHeaders.Accept.Clear();
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


			string ClientID = "cf4b9d9184f1471eb69646bcc9d998e4";
			string ClientSecret = "1b163932350b4b1f9C6EA27B9435D834";


			client.DefaultRequestHeaders.Add("client_id", ClientID);
			client.DefaultRequestHeaders.Add("client_secret", ClientSecret);


			return client;
		}

		public static HttpResponseMessage LoansListCheck()
		{
			var client = HttpClientSetup();

			var relativeUri = "https://myqlmwapi.rockfin.com/Loans/GetLoanListByAuthId/64671691";

			var Task = client.GetAsync(relativeUri);
			Task.Wait();
			var data = Task.Result;
			var contentTask = data.Content.ReadAsStringAsync();
			contentTask.Wait();
			var jsonData = contentTask.Result;

			return data;
		}

		public static HttpResponseMessage AuthCheck()
		{
			var client = HttpClientSetup();

			var relativeUri = "https://apibeta.quickenloans.com/auth-proxy/";

			var Task = client.GetAsync(relativeUri);
			Task.Wait();
			var data = Task.Result;
			var contentTask = data.Content.ReadAsStringAsync();
			contentTask.Wait();
			var jsonData = contentTask.Result;

			return data;
		}

		public static HttpResponseMessage AVSCheck()
		{
			var client = HttpClientSetup();

			var relativeUri = "https://apibeta.quickenloans.com/avs/zip_codes?city=Detroit&state=MI";

			var Task = client.GetAsync(relativeUri);
			Task.Wait();
			var data = Task.Result;
			var contentTask = data.Content.ReadAsStringAsync();
			contentTask.Wait();
			var jsonData = contentTask.Result;

			return data;
		}



        public static HttpResponseMessage TaxAssessmentCheck()
        {
			var client = HttpClientSetup();

			var BodyJsonData = new
			{
				Property = new
				{
					Type = "noncondo",
					Value = 100000,
					Address = new
					{
						AddressLine1 = "109 MAIN",
						AddressLine2 = "",
						City = "PLEVNA",
						State = "KS",
						Zip = "67568",
						ZipPlus4 = "0000",
						County = "Reno"
					}
				}
			};

			var JsonString = JsonConvert.SerializeObject(BodyJsonData);

			StringContent content = new StringContent(JsonString, System.Text.Encoding.UTF8, "application/json");


			var Task = client.PostAsync("https://apibeta.rockfin.com/tax-assessment/assessments", content);



			Task.Wait();
			var data = Task.Result;
            //var contentTask = data.Content.ReadAsStringAsync();
            //contentTask.Wait();
            //var jsonData = contentTask.Result;

            return data;
		}

        public static HttpResponseMessage TaxEstimateCheck()
        {
            var client = HttpClientSetup();

            var relativeUri = "https://apibeta.quickenloans.com/tax-estimate/estimates?property_type=noncondo&property_value=100000&city=WATERFORD&county=OAKLAND&state=MI&zip=48328";

            var Task = client.GetAsync(relativeUri);
            Task.Wait();
            var data = Task.Result;
            var contentTask = data.Content.ReadAsStringAsync();
            contentTask.Wait();
            var jsonData = contentTask.Result;

            return data;
        }

		public static HttpResponseMessage PendingActionCheck()
		{
			var client = HttpClientSetup();

			var relativeUri = "https://myqlmwapi.rockfin.com/Loan/3309310540/PendingActions/BorrowerUpdate";

			var Task = client.GetAsync(relativeUri);
			Task.Wait();
			var data = Task.Result;
			var contentTask = data.Content.ReadAsStringAsync();
			contentTask.Wait();
			var jsonData = contentTask.Result;

			return data;
		}


		public static HttpResponseMessage SvcActCheck()
		{
			var client = HttpClientSetup();

			var relativeUri = "https://myqlmwapi.rockfin.com/Loan/3309310540/ServicingActivity";

			var Task = client.GetAsync(relativeUri);
			Task.Wait();
			var data = Task.Result;
			var contentTask = data.Content.ReadAsStringAsync();
			contentTask.Wait();
			var jsonData = contentTask.Result;

			return data;
		}

        public static HttpResponseMessage RocketDyneCheck()
        {
            var client = HttpClientSetup();

            var relativeUri = "https://apibeta.quickenloans.com/rocketdyne/asset_status?maximumAgeInDays=0&loanNumber=1235678&gcid=123457";

            var Task = client.GetAsync(relativeUri);
            Task.Wait();
            var data = Task.Result;
            var contentTask = data.Content.ReadAsStringAsync();
            contentTask.Wait();
            var jsonData = contentTask.Result;

            return data;
        }

        public static HttpResponseMessage PiRCheck()
        {
            var client = HttpClientSetup();

            var relativeUri = "https://apibeta.quickenloans.com/price-is-right-admin/health";

            var Task = client.GetAsync(relativeUri);
            Task.Wait();
            var data = Task.Result;
            var contentTask = data.Content.ReadAsStringAsync();
            contentTask.Wait();
            var jsonData = contentTask.Result;

            return data;
        }

        public static HttpResponseMessage TGenCheck()
        {
            var client = HttpClientSetup();

            var relativeUri = "https://apibeta.quickenloans.com/token-generator/token";

            var Task = client.GetAsync(relativeUri);
            Task.Wait();
            var data = Task.Result;
            var contentTask = data.Content.ReadAsStringAsync();
            contentTask.Wait();
            var jsonData = contentTask.Result;

            return data;
        }

        public static HttpResponseMessage EHubCheck()
        {
            var client = HttpClientSetup();

            var relativeUri = "https://apibeta.quickenloans.com/solution-engine/mmc";

            var Task = client.GetAsync(relativeUri);
            Task.Wait();
            var data = Task.Result;
            var contentTask = data.Content.ReadAsStringAsync();
            contentTask.Wait();
            var jsonData = contentTask.Result;

            return data;
        }

        public static HttpResponseMessage MyQLQTweetConsumerCheck()
        {
            try
            {
                var client = HttpClientSetup();

                var relativeUri = "https://myqlmwapibeta1w1.mi.corp.rockfin.com/Health/MyQLQtweetConsumer/Basic";

                var Task = client.GetAsync(relativeUri);
                Task.Wait();
                var data = Task.Result;
                var contentTask = data.Content.ReadAsStringAsync();
                contentTask.Wait();
                var jsonData = contentTask.Result;
                return data;
            }
            catch
            {
                HttpResponseMessage oops_data = new HttpResponseMessage();
                oops_data.StatusCode = System.Net.HttpStatusCode.NotImplemented;
                return oops_data;
            }
        }

        public static HttpResponseMessage MyQLFrontendInterfaceCheck()
        {
            try
            {
                var client = HttpClientSetup();

                var relativeUri = "https://myqlmwapibeta1w1.mi.corp.rockfin.com/Health/MyQLFrontendInterface/Basic";

                var Task = client.GetAsync(relativeUri);
                Task.Wait();
                var data = Task.Result;
                var contentTask = data.Content.ReadAsStringAsync();
                contentTask.Wait();
                var jsonData = contentTask.Result;

                return data;
            }
            catch(Exception ex)
            {
                ex = ex;

                HttpResponseMessage oops_data = new HttpResponseMessage();
                oops_data.StatusCode = System.Net.HttpStatusCode.NotImplemented;
                return oops_data;
            }
        }
    }
}
