using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iDashNet.Models
{
	public class ZabbixModel
	{
		public List<ZabbixApp> zabbixApps;
	}
}

namespace iDashNet
{

    public class ZabbixApp
    {
        public string Name { get; set; }
    }
}