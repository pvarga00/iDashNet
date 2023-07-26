using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace iDashNet.Controllers
{
	public class ZabbixController : Controller
	{
		// GET: /Zabbix/
		public string ZabbixIndex()
		{

			// Zabbix Info:
			// Zabbix API Main URL: http://zabbix/api_jsonrpc.php
			// Zabix API Docs: https://www.zabbix.com/documentation/3.2/manual/api/reference
            // 
			// string zabbixLogin = "appdash";
			// string zabbixPwd = "5NYDm1L&(4!1";

			// These are the application calls we need to make:
			// TODO: FIGURE THIS OUT : What/How to make API calls to get metrics

			HttpClient client = new HttpClient();

			client.DefaultRequestHeaders.Accept.Clear();

			string zabbixLogin = "appdash";
			string zabbixPwd = "5NYDm1L&(4!1";


			client.DefaultRequestHeaders.Authorization =
					new AuthenticationHeaderValue(
						"Basic",
						Convert.ToBase64String(
							System.Text.ASCIIEncoding.ASCII.GetBytes(
								  string.Format("{0}:{1}", zabbixLogin, zabbixPwd)))); // 

			var Task = client.GetAsync("http://zabbix/api_jsonrpc.php");
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

// Additional Notes: 
// http://ql1aow1:3000 : admin/admin

// http://zabbixstats/ : admin/admin

