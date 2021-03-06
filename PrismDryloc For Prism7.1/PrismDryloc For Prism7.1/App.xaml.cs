﻿using PrismDryloc_For_Prism7._1.Views;
using Prism.Ioc;
using Prism.Modularity;
using System.Windows;

namespace PrismDryloc_For_Prism7._1
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {

        #region Add

        private string dataFilePath = string.Empty;

        //JObject json = JObject.Parse(File.ReadAllText("config.json"));

        #endregion

        protected override Window CreateShell()
        {
            #region Add

            //if (this.dataFilePath == string.Empty) { this.Shutdown(); }

            #endregion

            return Container.Resolve<MainWindow>();

        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            #region Add

            containerRegistry.RegisterSingleton<Models.ModelBase, Models.Model_A>();

            Models.ModelBase model = Container.Resolve<Models.ModelBase>();
            model.a = "in register";



            //containerRegistry.RegisterInstance(new Models.Model("abc"));
            //containerRegistry.RegisterTypeForNavigation<NavigationPage>();

            //containerRegistry.RegisterInstance<WpfTestAppData>(DataLoader.Load(this.dataFilePath));

            //    var modelbuilder = new ContainerBuilder();
            //    modelbuilder.RegisterInstance(Models.ModelBilder.Build()).SingleInstance();
            //    modelcontainer = modelbuilder.Build();


            #endregion
        }

        #region Add Function

        protected override void OnStartup(StartupEventArgs e)
        {
            if (e.Args.Length == 1) { this.dataFilePath = e.Args[0]; }

            base.OnStartup(e);
        }


        //protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        //{
        //    moduleCatalog.AddModule<NavigationTreeModule>(InitializationMode.WhenAvailable);
        //}

        #endregion

    }
}


//protected override void OnStartup(StartupEventArgs e)
//{
//    base.OnStartup(e);

//    #region Add

//    var modelbuilder = new ContainerBuilder();
//    modelbuilder.RegisterInstance(Models.ModelBilder.Build()).SingleInstance();
//    modelcontainer = modelbuilder.Build();

//    #endregion

//    var bootstrapper = new Bootstrapper();
//    bootstrapper.Run();
//}


//class Bootstrapper : AutofacBootstrapper
//{
//    protected override DependencyObject CreateShell()
//    {
//        return (DependencyObject)Container.Resolve<MainWindow>();
//    }

//    protected override void InitializeShell()
//    {
//        Application.Current.MainWindow.Show();
//    }

//    protected override void ConfigureModuleCatalog()
//    {
//        var moduleCatalog = (ModuleCatalog)ModuleCatalog;
//        //moduleCatalog.AddModule(typeof(YOUR_MODULE));
//    }
//}

//class Bootstrapper : AutofacBootstrapper
//{
//    JObject json = JObject.Parse(File.ReadAllText("config.json"));

//    protected override DependencyObject CreateShell()
//    {
//        return Container.CreateWindow(json);
//        //return Container.Resolve<MainWindow>();
//    }

//    protected override void InitializeShell()
//    {
//        Application.Current.MainWindow.Show();
//    }

//    protected override void ConfigureModuleCatalog()
//    {
//        var moduleCatalog = (ModuleCatalog)ModuleCatalog;
//        moduleCatalog.AddModule(typeof(MainWindowModule));
//    }

//    protected override void ConfigureContainerBuilder(ContainerBuilder builder)
//    {
//        base.ConfigureContainerBuilder(builder);

//        //Modelの登録
//        builder.CreateModel(json);

//        //Viewの登録
//        builder.RegisterModule<MainWindowModulRegistry>();
//    }
//}


//public class MainWindowModule : IModule
//{
//    private IRegionManager _regionManager;
//    private ContainerBuilder _builder;

//    public MainWindowModule(ContainerBuilder builder, IRegionManager regionManager)
//    {
//        _builder = builder;
//        _regionManager = regionManager;
//    }

//    public void Initialize()
//    {
//        //MainWindowViewModelのLoadedに権利移譲
//        //_regionManager.RegisterViewWithRegion("ContentRegion", typeof(ContentUserControl));
//        //_regionManager.RegisterViewWithRegion("MenuRegion", typeof(MenuView));
//        //_regionManager.RegisterViewWithRegion("VSplitGridRegion", typeof(VSplitGridView));

//        _regionManager.RegisterViewWithRegion("RightRegion", typeof(PropertyView));
//        _regionManager.RegisterViewWithRegion("FlyoutLeftRegion", typeof(MenuView));

//    }
//}

//public class MainWindowModulRegistry : Module
//{
//    protected override void Load(ContainerBuilder builder)
//    {
//        base.Load(builder);

//        // Moduleの登録
//        //builder.RegisterType<MainWindowModule>();

//        // Viewの登録
//        // Reflectionで代用
//        var asm = System.Reflection.Assembly.GetExecutingAssembly();
//        Type[] ts = asm.GetTypes();
//        var method = typeof(AutofacExtensions).GetMethod("RegisterTypeForNavigation");
//        foreach (Type t in ts)
//        {
//            if (t.Namespace == $"{nameof(PrismAutofac)}.{nameof(Views)}")
//            {
//                if (t.FullName.EndsWith("View"))
//                {
//                    var constructed = method.MakeGenericMethod(t);
//                    constructed.Invoke(null, new object[] { builder, null });
//                }
//            }
//        }
//        //builder.RegisterTypeForNavigation<TreeUserControl>();

//        /* Reflectionを使用しない場合ex.
//        builder.RegisterTypeForNavigation<ConfigurationView>();
//        builder.RegisterTypeForNavigation<ViewSelectorView>();
//        builder.RegisterTypeForNavigation<PropertyView>();
//        builder.RegisterTypeForNavigation<ContentView>();
//        builder.RegisterTypeForNavigation<AboutView>();
//        */
//    }
//}

//// Bootstrapper.Run()内、CreateShell～InitializeShellで
//// コンストラクタが呼ばれるので、初期化するなら直前がよさそう
//static class ModelBilder
//{
//    public static void CreateModel(this ContainerBuilder builder, JObject json)
//    {
//        switch (json["Model"].Value<string>())
//        {
//            case "A":
//                builder.RegisterType<ModelA>().As<ModelBase>().SingleInstance();
//                break;
//            case "B":
//                builder.RegisterType<ModelB>().As<ModelBase>().SingleInstance();
//                break;
//        }
//        //builder.RegisterType<Model>().As<ModelBase>()
//        //    .WithParameter("ParamName", "ParamValue")
//        //    .SingleInstance();

//        //using (var sr = new System.IO.StreamReader("config.json"))
//        //{
//        //  Newtonsoft.Json.JsonSerializer.CreateDefault().Populate(sr, model);
//        //}
//    }

//    public static DependencyObject CreateWindow(this IContainer container, JObject json)
//    {
//        //Modelの初期化
//        var model = container.Resolve<Models.ModelBase>();
//        using (var sr = json["Config"].CreateReader())
//        {
//            Newtonsoft.Json.JsonSerializer.CreateDefault().Populate(sr, model);
//        }

//        switch (json["MainWindow"].Value<string>())
//        {
//            case "0":
//                return container.Resolve<MainWindow>();
//            case "1":
//                return container.Resolve<Main2Window>();
//        }
//        return null;
//    }
//}

//// Prism-Autofac外でAutofacのコンテナ管理する場合
///* 
//public static IContainer modelcontainer;
//public static void RegistContainer()
//{
//    var modelbuilder = new ContainerBuilder();
//    modelbuilder.RegisterType<Models.Model>().SingleInstance();
//    modelcontainer = modelbuilder.Build();

//    //同時に初期化する場合
//    var model = modelcontainer.Resolve<ModelBase>();
//    model.HogeHoge();
//}
//*/