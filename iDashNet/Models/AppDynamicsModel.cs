using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iDashNet.Models
{
	public class AppDynamicsModel
	{
		public List<AppDynamicsApp> appdApps;
	}
}


namespace iDashNet
{
    public class AppDynamicsApp
    {
        public string Name { get; set; }
        public string AvgResponseTime { get; set; }
    }
}
