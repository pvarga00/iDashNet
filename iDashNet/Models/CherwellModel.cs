using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iDashNet.Models
{
    public class CherwellModel
    {
        public List<CherwellApp> cherwellApps;
    }
}

namespace iDashNet
{
    public class CherwellApp
    {
        public string Name { get; set; }
    }
}