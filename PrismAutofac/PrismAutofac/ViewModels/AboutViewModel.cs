using Prism.Mvvm;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismAutofac.ViewModels
{


    public class AboutViewModel : BindableBase
    {
        System.Reflection.Assembly asm = System.Reflection.Assembly.GetExecutingAssembly();

        public string Html => "https://github.com/gitllama";

        public string Title => $"{asm.GetName().Name}";
        public string Version => $"Ver. {asm.GetName().Version} (x64)";
        public string Copyright => $"Copyright© 2018 Gitllama";

        public ReactiveCommand ClickCommand { get; private set; }

        public AboutViewModel()
        {
            ClickCommand = new ReactiveCommand();
            ClickCommand.Subscribe(_ => Process.Start(Html));
        }
    }
}


