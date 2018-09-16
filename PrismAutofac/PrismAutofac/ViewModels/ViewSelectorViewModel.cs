using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PrismAutofac.ViewModels
{
    public class ViewSelectorViewModel : BindableBase
    {
        private IRegionManager regionManager;

        public ReactiveProperty<string[]> Items { get; private set; }
        public ReactiveProperty<string> Selected { get; private set; }
        
        public ViewSelectorViewModel(IRegionManager rm)
        {
            regionManager = rm;

            Items = new ReactiveProperty<string[]>(new string[] 
            {
                "Content",
                "Configuration",
                "About"
            });

            Selected = new ReactiveProperty<string>("Content");
            Selected.Subscribe(x => 
            {
                regionManager.RequestNavigate("ContentRegion", $"{x}View");
            });

        }
    }
}
