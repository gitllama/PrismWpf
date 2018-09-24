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
using System.Windows;

namespace PrismAutofac.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {        
        public ReactiveCommand LoadedCommand { get; private set; }
        
        public ReactiveProperty<string> Title { get; private set; }
        public ReactiveProperty<string> StatusMessage { get; private set; }
        public ReactiveProperty<Visibility> isBusy { get; private set; }

        public ReactiveProperty<bool> FlyoutIsOpen { get; private set; }

        // Region

        public ReactiveCommand NavigateCommand { get; private set; }
        public ReactiveCommand ShortcutCommand { get; private set; }

        public MainWindowViewModel(ModelBase model, IRegionManager regionManager)
        {
            // TitleBar

            this.Title = model.ObserveProperty(x => x.Title).ToReactiveProperty();

            FlyoutIsOpen = new ReactiveProperty<bool>(false);

            // StatusBar

            this.StatusMessage = new ReactiveProperty<string>("StatusMessage");
            isBusy = model.ObserveProperty(x => x.isBusy)
                          .Select(x => x ? Visibility.Visible : Visibility.Collapsed)
                          .ToReactiveProperty();

            // Region

            this.NavigateCommand = new ReactiveCommand();
            this.NavigateCommand.Subscribe(x =>
            {
                regionManager.RequestNavigate("ContentRegion", $"{x}View");
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
                regionManager.RequestNavigate("ContentRegion", "ContentView");
            });
        }


    }



}


