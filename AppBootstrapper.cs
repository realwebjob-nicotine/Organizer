using Organizer.ViewModels;
using Organizer.Views;
using Organizer.Models;
using Caliburn.Micro;
using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Threading;
using System.Windows;
using System.ComponentModel;
using System.Linq;

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
            container.Instance(container);
            container.Singleton<IWindowManager, WindowManager>();

            container.Singleton<MainModel>();

            foreach (var assembly in SelectAssemblies())
            {
                assembly.GetTypes()
               .Where(type => type.IsClass)
               .Where(type => type.Name.EndsWith("ViewModel"))
               .ToList()
               .ForEach(viewModelType => container.RegisterPerRequest(
                   viewModelType, viewModelType.ToString(), viewModelType));
            }
        }

        protected override async void OnStartup(object sender, StartupEventArgs e)
        {
            var c = IoC.Get<SimpleContainer>();
            await DisplayRootViewForAsync(typeof(MainViewModel));
        }
    }
}
