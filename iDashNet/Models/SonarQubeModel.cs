using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iDashNet.Models
{
    public class SonarQubeModel
    {
        public List<SonarQubeApp> sonarApps;
    }

}

namespace iDashNet
{
    public class SonarQubeApp
    {
        public string Name { get; set; }
        public string SonarUrl { get; set; }
        public string SonarKey { get; set; }
        public string Coverage { get; set; }
        public string Violations { get; set; }
        public string Complexity { get; set; }
        public string TechDebt { get; set; }
        public string TotNumberUnitTests { get; set; }
        public string UnitTestErrors { get; set; }
        public string UTPassPercent { get; set; }
    }
}
