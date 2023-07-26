using System.Text.Encodings.Web;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
/*
namespace iDashNet.Controllers
{
	public class TFSController : Controller
	{
		// GET: /TFS/
		public string TFSIndex()
		{

			 https://confluence/display/~MStollman/How+to+get+metrics+from+TFS
			 * 
             * Create a query in TFS. Saving under shared/wildwildwest helps us with permissions.
            Preform a GET on your query URL.  Example URL: "http://mstollman:dqipweychw3xzbdbnds4r4ggvws2dll6ua24fcuwxgl6x5lvbr4q@tfs/tfs/ql/it/_apis/wit/queries/Shared%20Queries%2FWild%20Wild%20West%2FMyQL%20Combined%20-%20Bugs?api-version=2.2"
            Grab the Query ID from the response. Examp;e: "id": "a1109bfd-8550-4de8-a5ba-64b2b6434a1c"
            Create GET with new URL to actually run query. Example: "http://mstollman:dqipweychw3xzbdbnds4r4ggvws2dll6ua24fcuwxgl6x5lvbr4q@tfs/tfs/ql/it/_apis/wit/wiql/8f2a0526-404f-4e55-a959-bcdd2e2c00d5?api-version=2.2"
            Have fun parsing response with your framework of choice to get the data that you need.


			// These are the application calls we need to make:
			// TODO: FIGURE THIS OUT : What/How to make API calls to get metrics

			HttpClient client = new HttpClient();

			client.DefaultRequestHeaders.Accept.Clear();

			string TFSLogin = "XXX";
			string TFSPwd = "XXXX";


			client.DefaultRequestHeaders.Authorization =
					new AuthenticationHeaderValue(
						"Basic",
						Convert.ToBase64String(
							System.Text.ASCIIEncoding.ASCII.GetBytes(
								  string.Format("{0}:{1}", TFSLogin, TFSPwd)))); // 

			var Task = client.GetAsync("http://XXXX");
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
*/