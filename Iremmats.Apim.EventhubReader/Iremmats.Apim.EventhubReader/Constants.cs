using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iremmats.Apim.EventhubReader
{
    public static class Constants
    {
        // Used to fetch from App.settings and get connectionstrings
        public static string EventHubName = Microsoft.Azure.CloudConfigurationManager.GetSetting("EventHubName");
        public static string ElasticsearchUrl = Microsoft.Azure.CloudConfigurationManager.GetSetting("ElasticsearchUrl");
        public static string EventHubStorage = System.Configuration.ConfigurationManager.ConnectionStrings["EventHubStorageAccount"].ConnectionString;
        public static string EventHubConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["EventHubServiceBus"].ConnectionString;
    }
}
