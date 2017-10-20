using Autofac;
using Prism.Autofac;
using PrismAutofac.Views;
using System.Windows;

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
    }
}
