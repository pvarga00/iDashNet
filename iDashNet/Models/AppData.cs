using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace iDashNet.Models
{
    public class AppData
    {
        [Key]
        public int ID { get; set; }
        public string ApplicationID { get; set; }
        public string KPIName { get; set; }
        public DateTime Timestamp { get; set; }
        public string Value { get; set; }

    }
}
