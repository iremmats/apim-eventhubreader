using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iremmats.Apim.EventhubReader
{
    class ApiManagementLogEvent
    {
        public DateTime Timestamp { get; set; }
        public string UserId { get; set; }
        public string Region { get; set; }
        public string ServiceName { get; set; }
        public string RequestId { get; set; }
        public string IpAddress { get; set; }
        public string Operation { get; set; }
        public string Body { get; set; }
        public string Status { get; set; }


    }
}
