using PrismAutofac.Views;
using System.Windows;
using Prism.Modularity;
using Autofac;
using Prism.Autofac;
using PrismAutofac.Models;
using Prism.Regions;
using System;

namespace PrismAutofac
{
    class Bootstrapper : AutofacBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            // Bootstrapper.Run()内、CreateShell～InitializeShellで
            // コンストラクタが呼ばれるので、初期化するなら直前がよさそう
            /*
            using (var sr = new System.IO.StreamReader("config.json"))
            {
                var model = Container.Resolve<Models.ModelBase>();
                Newtonsoft.Json.JsonSerializer.CreateDefault().Populate(sr, model);
            }
            */
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
            
            //Modelの登録
            builder.RegisterType<Model>().As<ModelBase>().SingleInstance();
            //builder.RegisterType<Model>().As<ModelBase>()
            //    .WithParameter("ParamName", "ParamValue")
            //    .SingleInstance();
　　　　　　 
            //Viewの登録
            //builder.RegisterTypeForNavigation<PropertyGridUserControl>();
            builder.RegisterModule<MainWindowModulRegistry>();
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
            _regionManager.RegisterViewWithRegion("ContentRegion", typeof(ContentUserControl));
        }

    }

    public class MainWindowModulRegistry : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            //builder.RegisterType<MainWindowModule>();
            builder.RegisterTypeForNavigation<TreeUserControl>();
            builder.RegisterTypeForNavigation<PropertyGridUserControl>();
        }
    }
    
    // Prism-Autofac外でAutofacのコンテナ管理する場合
    /* 
    public static IContainer modelcontainer;
    public static void RegistContainer()
    {
        var modelbuilder = new ContainerBuilder();
        modelbuilder.RegisterType<Models.Model>().SingleInstance();
        modelcontainer = modelbuilder.Build();
        
        //同時に初期化する場合
        var model = modelcontainer.Resolve<ModelBase>();
        model.HogeHoge();
    }
    */
}
