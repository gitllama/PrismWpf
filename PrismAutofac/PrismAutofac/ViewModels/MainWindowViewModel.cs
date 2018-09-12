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
        private IRegionManager _regionManager;

        // TitleBar

        public ReactiveProperty<string> Title { get; private set; }

        // StatusBar

        public ReactiveProperty<string> StatusMessage { get; private set; }

        // FoldingMark

        //public ReactiveProperty<bool> VisibleFoldingMarkFromBehavior { get; private set; }
        //public ReactiveProperty<string> VisibleFoldingMarkToView { get; private set; }

        // Visible SubRegion

        public ReactiveProperty<string> VisibleLeftRegion { get; private set; }
        public ReactiveProperty<string> VisibleRightRegion { get; private set; }
        private string _columnWidthLeft;
        private string _columnWidthRight;
        public ReactiveProperty<string> ColumnWidthLeft { get; private set; }
        public ReactiveProperty<string> ColumnWidthRight { get; private set; }
        public ReactiveCommand ClickFoldingMarkLeftCommand { get; private set; }
        public ReactiveCommand ClickFoldingMarkRightCommand { get; private set; }


        public MainWindowViewModel(ModelBase model, IRegionManager rm)
        {
            this.model = model;
            this._regionManager = rm;

            // TitleBar

            this.Title = model.ObserveProperty(x => x.Title).ToReactiveProperty();


            // StatusBar

            this.StatusMessage = new ReactiveProperty<string>("StatusMessage");

            // FoldingMark

            //this.VisibleFoldingMarkFromBehavior = new ReactiveProperty<bool>();
            //this.VisibleFoldingMarkToView = VisibleFoldingMarkFromBehavior.Select(x => x ? "Visible" : "Collapsed").ToReactiveProperty();

            // Visible SubRegion

            this.VisibleLeftRegion = new ReactiveProperty<string>("Visible");
            this.VisibleRightRegion = new ReactiveProperty<string>("Visible");
            this.ColumnWidthLeft = new ReactiveProperty<string>("10*");
            this.ColumnWidthRight = new ReactiveProperty<string>("10*");
            this._columnWidthLeft = null;
            this._columnWidthRight = null;
            this.ClickFoldingMarkLeftCommand = new ReactiveCommand();
            this.ClickFoldingMarkLeftCommand.Subscribe(_ =>
            {
                if (_columnWidthLeft == null)
                {
                    _columnWidthLeft = ColumnWidthLeft.Value;
                    ColumnWidthLeft.Value = "0";
                    VisibleLeftRegion.Value = "Hidden";
                }
                else
                {
                    ColumnWidthLeft.Value = _columnWidthLeft;
                    _columnWidthLeft = null;
                    VisibleLeftRegion.Value = "Visible";
                }
            });
            this.ClickFoldingMarkRightCommand = new ReactiveCommand();
            this.ClickFoldingMarkRightCommand.Subscribe(_ =>
            {
                if (_columnWidthRight == null)
                {
                    _columnWidthRight = ColumnWidthRight.Value;
                    ColumnWidthRight.Value = "0";
                    VisibleRightRegion.Value = "Hidden";
                }
                else
                {
                    ColumnWidthRight.Value = _columnWidthRight;
                    _columnWidthRight = null;
                    VisibleRightRegion.Value = "Visible";
                }
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
