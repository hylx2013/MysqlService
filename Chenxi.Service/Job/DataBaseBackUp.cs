using Chenxi.Service.Services;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chenxi.Service.Job
{
    public class DataBaseBackUp : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            DataBaseBackUpService.BackUp();
        }
    }
}
