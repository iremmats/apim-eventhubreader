using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;
using System.Diagnostics;
using Nest;
using System.IO;
using Newtonsoft.Json;
using System.Globalization;


namespace Iremmats.Apim.EventhubReader
{
    public class SimpleEventProcessor : IEventProcessor
    {
        Stopwatch checkpointStopWatch;

        async Task IEventProcessor.CloseAsync(PartitionContext context, CloseReason reason)
        {
            //For Debug and Unit tests.
            //Console.WriteLine("Processor Shutting Down. Partition '{0}', Reason: '{1}'.", context.Lease.PartitionId, reason);
            if (reason == CloseReason.Shutdown)
            {
                await context.CheckpointAsync();
            }
        }

        Task IEventProcessor.OpenAsync(PartitionContext context)
        {
            //For Debug and Unit tests.
            //Console.WriteLine("SimpleEventProcessor initialized.  Partition: '{0}', Offset: '{1}'", context.Lease.PartitionId, context.Lease.Offset);
            this.checkpointStopWatch = new Stopwatch();
            this.checkpointStopWatch.Start();
            return Task.FromResult<object>(null);
        }

        async Task IEventProcessor.ProcessEventsAsync(PartitionContext context, IEnumerable<EventData> messages)
        {
            try
            {
                foreach (EventData eventData in messages)
                {
                    var data = Encoding.UTF8.GetString(eventData.GetBytes());
                    
                  
                    var items = data.Split(new string[]{"##"}, StringSplitOptions.None);

                    var apiManagementLogEvent = new ApiManagementLogEvent
                    {
                        Timestamp = Convert.ToDateTime(items[0]),
                        ServiceName = items[1],
                        UserId = items[2],
                        Region = items[3],
                        RequestId = items[4],
                        IpAddress = items[5],
                        Operation = items[6],
                        Status = items[7],
                        Body = items[8]

                    };
                    
                    
                    //Console.WriteLine(items[8]);
                    var esnode = new Uri(Constants.ElasticsearchUrl);
                    var settings = new ConnectionSettings(
                            esnode, "apim-" + DateTime.Now.ToString("yyyy.MM.dd")
                     );

                    var client = new ElasticClient(settings);
                    client.Index(apiManagementLogEvent);

                }
                
                //Call checkpoint every 5 minutes, so that worker can resume processing from the 5 minutes back if it restarts.
                if (this.checkpointStopWatch.Elapsed > TimeSpan.FromMinutes(5))
                {
                    await context.CheckpointAsync();
                    this.checkpointStopWatch.Restart();
                }
            }
            catch (Exception e)
            {
                Trace.Listeners.Add(new TextWriterTraceListener(Console.Out)); Trace.WriteLine(e.InnerException);

            }

        }
    }
}

