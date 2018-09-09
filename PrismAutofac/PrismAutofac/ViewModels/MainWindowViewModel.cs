using Prism.Modularity;
using Prism.Mvvm;
using Prism.Regions;
using PrismAutofac.Models;
using PrismAutofac.Views;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Specialized;

namespace PrismAutofac.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private ModelBase model;
        private IRegionManager _regionManager;


        private string _title = "Prism Application";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public ReactiveProperty<string> text { get; private set; }

        public ReactiveCommand Button1Command { get; private set; }
        public int ModuleManager_LoadModuleCompleted { get; }

        public MainWindowViewModel(ModelBase model, IRegionManager rm)
        {
            this.model = model;
            this._regionManager = rm;

            //this.mm.LoadModuleCompleted += this.ModuleManager_LoadModuleCompleted;
            //_regionManager.Regions.CollectionChanged += Regions_CollectionChanged;

            this.text = model.ObserveProperty(x => x.Text).ToReactiveProperty();

            //mm.LoadModule("MainWindowModule");

            this.Button1Command = new ReactiveCommand();
            this.Button1Command.Subscribe(x =>
            {
                Console.WriteLine(x);
                _regionManager.RequestNavigate("SubRegion", "PropertyGridUserControl");
                
                //rm.RegisterViewWithRegion("SubRegion", typeof(PropertyGridUserControl));

            });

        }

        private void Regions_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                var region = (IRegion)e.NewItems[0];
                region.Views.CollectionChanged += Views_CollectionChanged;
            }
        }

        private void Views_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //if (e.Action == NotifyCollectionChangedAction.Add)
            //{
            //    Views.Add(e.NewItems[0].GetType().Name);
            //}
            //else if (e.Action == NotifyCollectionChangedAction.Remove)
            //{
            //    Views.Remove(e.OldItems[0].GetType().Name);
            //}
        }
    }
}

//this.RegionManager.RequestNavigate("MainRegion", nameof(AView), new NavigationParameters($"id={x}"));