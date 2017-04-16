﻿using Prism.Commands;
using Prism.Mvvm;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PrismUnityApp.ViewModels
{
    public class PropertyViewModel : BindableBase
    {
        public ReactiveProperty<string> Title { get; private set; }
        public ReactiveProperty<object> obj { get; private set; }

        public PropertyViewModel()
        {
            obj = new ReactiveProperty<object>();
        }
    }
}
