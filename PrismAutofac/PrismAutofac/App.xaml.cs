using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PrismAutofac
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            // 取りこぼしの例外をすべてCatchする
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            
            base.OnStartup(e);

            var bootstrapper = new Bootstrapper();
            bootstrapper.Run();
            
            // StartupEventArgsがほしい場合は
            // bootstrapperのコンストラクタかRunをoverrideして
            // StartupEventArgs受けれるようにするのがよろしい
        }
        
        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = (Exception)e.ExceptionObject;
            MessageBox.Show(ex.ToString(), "UnhandledException",
                      MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
