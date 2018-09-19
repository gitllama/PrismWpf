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
    public class PropertyViewModel : BindableBase, INavigationAware, IRegionMemberLifetime
    {
        private ModelBase model;

        public ReactiveProperty<object> obj { get; private set; }

        public PropertyViewModel(ModelBase model)
        {
            this.model = model;
            obj = new ReactiveProperty<object>((object)model);
        }

        public bool KeepAlive
        {
            get
            {
                return false;
            }
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            //throw new NotImplementedException();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            //throw new NotImplementedException();
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            //throw new NotImplementedException();
        }
    }
}
