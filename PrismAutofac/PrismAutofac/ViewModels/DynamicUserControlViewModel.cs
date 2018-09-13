using Prism.Commands;
using Prism.Mvvm;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PrismAutofac.ViewModels
{
    public class DynamicUserControlViewModel : BindableBase
    {
        public ReactiveProperty<string> A { get; private set; }

        public DynamicUserControlViewModel()
        {
            A = new ReactiveProperty<string>("Test");
        }
    }
}
