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
        
        #region test
        //private readonly object _lock = new object();
        //private bool _shuttingDown;
        //public TaskScheduler()
        //{
            
        //    // Register this task with hosting evirontment 
        //    // allow for more gracefull stop the task, in case IIS shutting down
        //    HostingEnvironment.RegisterObject(this);
        //}
        //public void Execute()
        //{
        //    lock (_lock)
        //    {
        //        if (_shuttingDown)
        //            return;
        //        //code here
        //    }
        //}
        //public void Stop(bool immediate)
        //{
        //    lock (_lock)
        //    {
        //        _shuttingDown = true;
        //    }
        //    HostingEnvironment.UnregisterObject(this);
        //}
        #endregion
    }
}
