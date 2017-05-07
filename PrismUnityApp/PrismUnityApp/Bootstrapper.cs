using Microsoft.Practices.Unity;
using Prism.Unity;
using PrismUnityApp.Views;
using System.Windows;

namespace PrismUnityApp
{
    class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void InitializeShell()
        {
            Application.Current.MainWindow.Show();
        }

//        protected override IUnityContainer CreateContainer()
//        {
//            var i = base.CreateContainer();

//return i;
//        }
    }
}
