using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using PrismAutofac.Models;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PrismAutofac.ViewModels
{
    public class ContentUserControlViewModel : BindableBase
    {
        private ModelBase model;

        public ContentUserControlViewModel(ModelBase model)
        {
            this.model = model;

        }
    }
}
