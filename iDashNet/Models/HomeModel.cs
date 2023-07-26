using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iDashNet.Models
{
    public class HomeModel
    {
        public List<HomePageApp> homePageApps;
    }
}

namespace iDashNet
{
    public class HomePageApp
    {
        public string Name { get; set; }
        public string sonar_key { get; set; }
        public string appD_name { get; set; }
        public string sonar_url { get; set; }

        public string sonar_violations { get; set; }
        public string sonar_coverage { get; set; }
        public string sonar_techDebt { get; set; }
        public string sonar_complexity { get; set; }
        public string sonar_totalTests { get; set; }
        public string sonar_failingUTs { get; set; }
        public string sonar_percPass { get; set; }

        public string ga_appId { get; set; }
        public string ga_pageLoadTime { get; set; }
        public string ga_avgPageLoadTime { get; set; }
        public string ga_avgPageDownloadTime { get; set; }
        public string ga_avgTimeOnPage { get; set; }
        public string ga_uniquePageViews { get; set; }
        public string ga_appName { get; set; }
        public string appD_avgResponseTime { get; set; }
        public string testStatusRelativeUrl { get; set; }
        public string bugs { get; set; }
        public string app_functional_coverage { get; set; }


        //create update function that uses
    }
}
