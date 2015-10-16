using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;
using System.Diagnostics;
using Nest;
using System.Web;
using System.Configuration;
using Microsoft.Azure.WebJobs;

namespace Iremmats.Apim.EventhubReader
{
    public class Functions
    {
        // This function will get all events on the hub
        
        [NoAutomaticTrigger]
        public static void ReadEventHub()
        {
                string eventProcessorHostName = Guid.NewGuid().ToString();
                //Console.WriteLine("Start..");
                
                EventProcessorHost eventProcessorHost = new EventProcessorHost(eventProcessorHostName, Constants.EventHubName, EventHubConsumerGroup.DefaultGroupName, Constants.EventHubConnectionString, Constants.EventHubStorage);

                Console.WriteLine("Registering EventProcessor...");
                eventProcessorHost.RegisterEventProcessorAsync<SimpleEventProcessor>().Wait();

                Console.WriteLine("Receiving. Press enter key to stop worker.");
                Console.ReadLine();
        }
    }
}
