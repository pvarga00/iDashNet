using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace iDashNet.Controllers
{
    public class ChartController : Controller
    {
        iDashNet.Models.ChartModel _model = new iDashNet.Models.ChartModel();

        public IActionResult ChartIndex()
        {
            _model.chartData = GetAppHistoricalData();
            //Get historical data for an app and return with model
            return View(_model);
        }

        [Route("api/Chart/GetAppHistoricalData")]
        [HttpGet]
        public int[] GetAppHistoricalData()
        {
            try
            {

                //create an instance of google analytics controller


                //get historical data for myql

                int sixMonthsAgo = Convert.ToInt32(GAnalyticsController.GetMetric("54149095", "MyQL", "pageLoadTime", "180daysAgo", "150daysAgo"));
                int fiveMonthsAgo = Convert.ToInt32(GAnalyticsController.GetMetric("54149095", "MyQL", "pageLoadTime", "150daysAgo", "120daysAgo"));
                int fourMonthsAgo = Convert.ToInt32(GAnalyticsController.GetMetric("54149095", "MyQL", "pageLoadTime", "120daysAgo", "30daysAgo"));
                int threeMonthsAgo = Convert.ToInt32(GAnalyticsController.GetMetric("54149095", "MyQL", "pageLoadTime", "90daysAgo", "60daysAgo"));
                int twoMonthsAgo = Convert.ToInt32(GAnalyticsController.GetMetric("54149095", "MyQL", "pageLoadTime", "60daysAgo", "30daysAgo"));
                int oneMonthsAgo = Convert.ToInt32(GAnalyticsController.GetMetric("54149095", "MyQL", "pageLoadTime", "30daysAgo", "yesterday"));



                //get data for MyQL from  google analytics and convert into chart.js format
                return new int[]{ sixMonthsAgo, fiveMonthsAgo, fourMonthsAgo, threeMonthsAgo, twoMonthsAgo, oneMonthsAgo};
            }
            catch (Exception e)
            {

                throw e;
            }
        }
    }
}