using Chenxi.Service.Services;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Chenxi.Service
{
    public partial class Service1 : ServiceBase
    {
        private readonly IScheduler _scheduler;
        public Service1()
        {
            InitializeComponent();
            StdSchedulerFactory factory = new StdSchedulerFactory();
            _scheduler = factory.GetScheduler();
            log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(AppDomain.CurrentDomain.BaseDirectory + "/Config/log4net.config"));
        }

        protected override void OnStart(string[] args)
        {
            this._scheduler.Start();

        }

        protected override void OnStop()
        {
        }
        public void Start()
        {
           // DataBaseBackUpService.BackUp();
           this.OnStart(null);
        }
    }
}
