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
    public class MainWindowViewModel : BindableBase
    {
        private ModelBase model;
        private IRegionManager regionManager;

        // Loaded

        public ReactiveCommand LoadedCommand { get; private set; }

        // TitleBar

        public ReactiveProperty<string> Title { get; private set; }

        // StatusBar

        public ReactiveProperty<string> StatusMessage { get; private set; }

        // Region

        public ReactiveProperty<string> LeftRegion { get; private set; }
        public ReactiveProperty<string> RightRegion { get; private set; }

        public ReactiveCommand NavigateCommand { get; private set; }

        // Visible SubRegion

        public ReactiveProperty<bool> LeftRegionVisible { get; private set; }
        public ReactiveProperty<bool> RightRegionVisible { get; private set; }

        public ReactiveCommand ClickFoldingMarkCommand { get; private set; }

        // ShortCut 

        public ReactiveCommand ShortcutCommand { get; private set; }

        public MainWindowViewModel(ModelBase model, IRegionManager rm)
        {
            this.model = model;
            this.regionManager = rm;

            // TitleBar

            this.Title = model.ObserveProperty(x => x.Title).ToReactiveProperty();


            // StatusBar

            this.StatusMessage = new ReactiveProperty<string>("StatusMessage");

            // Region

            this.LeftRegion = new ReactiveProperty<string>("");
            this.RightRegion = new ReactiveProperty<string>("");
            this.LeftRegion.Subscribe(x => regionManager.RequestNavigate("LeftRegion", x.ToString()) );
            this.RightRegion.Subscribe(x =>
            {
                Console.WriteLine(x.ToString());
                regionManager.RequestNavigate("RightRegion", x.ToString());
            });

            this.NavigateCommand = new ReactiveCommand();
            this.NavigateCommand.Subscribe(x =>
            {
                regionManager.RequestNavigate("RightRegion", x.ToString());
                //this.RegionManager.RequestNavigate("MainRegion", nameof(AView), new NavigationParameters($"id={x}"));
                //_regionManager.RegisterViewWithRegion("SubRegion", typeof(PropertyGridUserControl));
            });


            // Visible SubRegion

            this.LeftRegionVisible = new ReactiveProperty<bool>();
            this.RightRegionVisible = new ReactiveProperty<bool>();

            this.ClickFoldingMarkCommand = new ReactiveCommand();
            this.ClickFoldingMarkCommand.Subscribe(x =>
            {
                switch (x.ToString().Trim().ToLower())
                {
                    case "left":
                        LeftRegionVisible.Value = !LeftRegionVisible.Value;
                        break;
                    case "right":
                        RightRegionVisible.Value = !RightRegionVisible.Value;
                        break;
                    default:
                        break;
                }
                
            });

            // ShortcutCommand

            this.ShortcutCommand = new ReactiveCommand();
            this.ShortcutCommand.Subscribe(x =>
            {
                StatusMessage.Value = x.ToString();
            });


            LoadedCommand = new ReactiveCommand();
            this.LoadedCommand.Subscribe(x =>
            {
                //初期化
                LeftRegionVisible.Value = true;
                RightRegionVisible.Value = false;

                //LeftRegion.Value = "TreeUserControl";
                RightRegion.Value = "PropertyGridUserControl";
            });
        }


    }



}

/*
            //mm.LoadModule("MainWindowModule");
            //this.mm.LoadModuleCompleted += this.ModuleManager_LoadModuleCompleted;
            //_regionManager.Regions.CollectionChanged += Regions_CollectionChanged;
     
        private void Regions_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //if (e.Action == NotifyCollectionChangedAction.Add)
            //{
            //    var region = (IRegion)e.NewItems[0];
            //    region.Views.CollectionChanged += Views_CollectionChanged;
            //}
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
     
     
     
     */
