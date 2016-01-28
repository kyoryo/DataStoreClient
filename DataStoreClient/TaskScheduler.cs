using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;
using FluentScheduler;
using System.Threading;

namespace DataStoreClient
{
    public class TaskScheduler: Registry //ITask, IRegisteredObject
    {
        
        private readonly object _lock = new object();
        private bool _shuttingDown;
        public TaskScheduler()
        {   
            Schedule<MyTask>().ToRunNow().AndEvery(20).Seconds();
            // Register this task with hosting evirontment 
            // allow for more gracefull stop the task, in case IIS shutting down
            //HostingEnvironment.RegisterObject(this);
        }
        public class MyTask : ITask
        {
            public void Execute()
            {
                Console.WriteLine("ITask Task: " + DateTime.Now);
                Console.WriteLine("Test jalan");
            }
        }
        public class InlineTask : ITask
        {
            public void Execute()
            {
                Console.WriteLine("Inline Task: " + DateTime.Now);
                
            }
        }
    }
}
