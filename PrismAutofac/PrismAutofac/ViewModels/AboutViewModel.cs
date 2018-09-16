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
        public string Html => "https://github.com/gitllama";
        public ReactiveCommand ClickCommand { get; private set; }

        public AboutViewModel()
        {
            ClickCommand = new ReactiveCommand();
            ClickCommand.Subscribe(_ => Process.Start(Html));
        }
    }
}
