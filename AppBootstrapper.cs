using Caliburn.Micro;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Organizer.ViewModels;
using Organizer.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Organizer
{
    public class AppBootstrapper : BootstrapperBase
    {
        private static readonly SimpleContainer container = new SimpleContainer();
        public AppBootstrapper()
        {
            Initialize();
        }

        protected override void Configure()
        {
            container.Singleton<IWindowManager, WindowManager>();
            container.Singleton<IEventAggregator, EventAggregator>();
        }
    }
}
