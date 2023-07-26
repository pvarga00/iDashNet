using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iDashNet.Models
{
	public class DCRUMModel
	{
		public List<DCRUMApp> dcrumApps;
	}
}

namespace iDashNet
{
    public class DCRUMApp
    {
        public string Name { get; set; }
    }
}
