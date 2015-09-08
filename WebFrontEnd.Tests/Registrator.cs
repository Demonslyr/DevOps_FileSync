using BusinessMgmt.File.Engines.Implementations;
using BusinessMgmt.File.Engines.Interfaces;
using BusinessMgmt.File.Manager.Implementations;
using BusinessMgmt.File.Manager.Interfaces;
using BusinessMgmt.Log.Engines.Implementations;
using BusinessMgmt.Log.Engines.Interfaces;
using BusinessMgmt.Log.Manager.Implementations;
using BusinessMgmt.Log.Manager.Interfaces;
using BusinessMgmt.Timer.Manager.Implementations;
using BusinessMgmt.Timer.Manager.Interfaces;
using BusinessMgmt.Timer.Engines.Implementations;
using BusinessMgmt.Timer.Engines.Interfaces;
using DAL.DataAccess.Log.Implementations;
using DAL.DataAccess.Log.Interfaces;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject.Modules;


namespace WebFrontEnd.Tests
{
    public class Registrator : NinjectModule

    {
        public StandardKernel kernel { get; set; }


        public Registrator()
        {

            init();
            register();
        }

        private void init()
        {
            kernel = new StandardKernel();
        }

        public void register()
        {
            kernel.Bind<IFileManager>().To<FileManager>();
            kernel.Bind<ILogManager>().To<LogManager>();
            kernel.Bind<ITimerManager>().To<TimerManager>();
            kernel.Bind<IFileEngine>().To<FileEngine>();
            kernel.Bind<ILogEngine>().To<LogEngine>();
            kernel.Bind<ITimerEngine>().To<TimerEngine>();
            kernel.Bind<ILogDataAccess>().To<LogDataAccess>();
        }

        
        public override void Load()
        {
            register();
            //throw new NotImplementedException();
        }
    }
}
