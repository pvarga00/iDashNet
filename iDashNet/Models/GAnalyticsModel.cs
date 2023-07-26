using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iDashNet.Models
{
	public class GAnalyticsModel
	{
		public List<GoogleAnalyticsApp> gaApps;
	}
}

namespace iDashNet
{
    public class GoogleAnalyticsApp
    {
        public string Name { get; set; }

        public string pageLoadTime { get; set; }
        public string avgPageLoadTime { get; set; }
        public string avgPageDownloadTime { get; set; }
        public string avgTimeOnPage { get; set; }

        public string uniquePageViews { get; set; }
        public string AppId { get; set; }
    }
}
