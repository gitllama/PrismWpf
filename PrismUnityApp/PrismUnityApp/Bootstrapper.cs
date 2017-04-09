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
    }
}
