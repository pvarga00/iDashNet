using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iDashNet.Models
{
    public class AvailabilityModel
    {
        public List<AvailabilityApps> AvailApps;
    }
}

namespace iDashNet
{
    public class AvailabilityApps
    {
        public string Name { get; set; }
        public System.Net.HttpStatusCode AppStatus { get; set; }
    }
}
