using PrismAutofac.Views;
using System.Windows;
using Prism.Modularity;
using Autofac;
using Prism.Autofac;
using PrismAutofac.Models;
using Prism.Regions;

namespace PrismAutofac
{
    class Bootstrapper : AutofacBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void InitializeShell()
        {
            Application.Current.MainWindow.Show();
        }

        protected override void ConfigureModuleCatalog()
        {
            var moduleCatalog = (ModuleCatalog)ModuleCatalog;
            moduleCatalog.AddModule(typeof(MainWindowModule));
        }

        protected override void ConfigureContainerBuilder(ContainerBuilder builder)
        {
            base.ConfigureContainerBuilder(builder);
            builder.RegisterType<Model>().As<ModelBase>().SingleInstance();

            //builder.RegisterTypeForNavigation<PropertyGridUserControl>();
            //builder.RegisterModule<MainWindowModulRegistry>();
        }


    }

    public class MainWindowModule : IModule
    {
        private IRegionManager _regionManager;
        private ContainerBuilder _builder;

        public MainWindowModule(ContainerBuilder builder, IRegionManager regionManager)
        {
            _builder = builder;
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            //_builder.RegisterTypeForNavigation<PropertyGridUserControl>();
            //_regionManager.RegisterViewWithRegion("SubRegion", typeof(PropertyGridUserControl));
        }

    }

    //public class MainWindowModulRegistry : Module
    //{
    //    protected override void Load(ContainerBuilder builder)
    //    {
    //        base.Load(builder);

    //        builder.RegisterType<MainWindowModule>();
    //        builder.RegisterType<PropertyGridUserControl>();
    //    }
    //}

}


//public static IContainer modelcontainer;
//var modelbuilder = new ContainerBuilder();
//modelbuilder.RegisterType<Models.Model>().SingleInstance();
//modelcontainer = modelbuilder.Build();