using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;

namespace Iremmats.Apim.EventhubReader
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = new JobHost();
            Console.WriteLine("EventHubReader has been started at " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:tt"));
            host.Call(typeof(Functions).GetMethod("ReadEventHub"));
            //host.RunAndBlock();
        }
    }
}
