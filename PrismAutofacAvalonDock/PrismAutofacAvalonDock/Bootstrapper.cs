using Autofac;
using Prism.Autofac;
using PrismAutofacAvalonDock.Views;
using System.Windows;

namespace PrismAutofacAvalonDock
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
    }
}
