using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PrismUnityApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static UnityContainer Container;
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Container = new UnityContainer();
            //Container.RegisterInstance<Models.Imodel>("Model", new Models.Model(), new ContainerControlledLifetimeManager());

            var lifetimeManager = new ContainerControlledLifetimeManager();
            Container.RegisterType<Models.Imodel, Models.Model>("Model", lifetimeManager);

            var bootstrapper = new Bootstrapper();
            bootstrapper.Run();
        }
    }
}
