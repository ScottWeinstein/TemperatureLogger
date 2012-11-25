using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
// using System.Threading.Tasks;

namespace TLogger
{
    public partial class TempLoggerService : ServiceBase
    {
        public TempLoggerService()
        {
            this.ServiceName = "TempLoggerService";
        }

        protected override void OnStart(string[] args)
        {
        }

        protected override void OnStop()
        {
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing )
            {
                // components.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
