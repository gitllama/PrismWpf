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
        private IRegionManager _regionManager;

        public ReactiveCommand NavigateCommand { get; private set; }

        public ContentUserControlViewModel(ModelBase model, IRegionManager rm)
        {
            this.model = model;
            this._regionManager = rm;

            this.NavigateCommand = new ReactiveCommand();
            this.NavigateCommand.Subscribe(x =>
            {
                _regionManager.RequestNavigate("RightRegion", x.ToString());
                //this.RegionManager.RequestNavigate("MainRegion", nameof(AView), new NavigationParameters($"id={x}"));
                //_regionManager.RegisterViewWithRegion("SubRegion", typeof(PropertyGridUserControl));
            });
        }
    }
}
