using Prism.Modularity;
using Prism.Mvvm;
using Prism.Regions;
using PrismAutofac.Models;
using PrismAutofac.Views;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Specialized;
using System.Reactive.Linq;

namespace PrismAutofac.ViewModels
{
    public class Main2WindowViewModel : BindableBase
    {
        private ModelBase model;
        private IRegionManager regionManager;

        // Loaded

        public ReactiveCommand LoadedCommand { get; private set; }

        // TitleBar

        public ReactiveProperty<string> Title { get; private set; }

        public ReactiveCommand NavigateCommand { get; private set; }


        // ShortCut 

        public ReactiveCommand ShortcutCommand { get; private set; }

        public Main2WindowViewModel(ModelBase model, IRegionManager rm)
        {
            this.model = model;
            this.regionManager = rm;

            // TitleBar

            this.Title = model.ObserveProperty(x => x.Title).ToReactiveProperty();

            this.NavigateCommand = new ReactiveCommand();
            this.NavigateCommand.Subscribe(x =>
            {
                regionManager.RequestNavigate("RightRegion", x.ToString());
                //this.RegionManager.RequestNavigate("MainRegion", nameof(AView), new NavigationParameters($"id={x}"));
                //_regionManager.RegisterViewWithRegion("SubRegion", typeof(PropertyGridUserControl));
            });


            // ShortcutCommand

            this.ShortcutCommand = new ReactiveCommand();
            this.ShortcutCommand.Subscribe(x =>
            {
                //StatusMessage.Value = x.ToString();
            });


            LoadedCommand = new ReactiveCommand();
            this.LoadedCommand.Subscribe(x =>
            {
                this.regionManager.RequestNavigate("ContentRegion", nameof(ContentView));
            });
        }


    }



}
