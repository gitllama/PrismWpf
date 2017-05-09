using Autofac;
using AvalonDockUtil;
using Prism.Commands;
using Prism.Mvvm;
using PrismAutofacAvalonDock;
using PrismAutofacAvalonDock.Models;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

namespace PrismAutofacAvalonDock.ViewModels
{
    public class Document : DocumentContent
    {
        public ReactiveProperty<double> Width { get; private set; }
        public ReactiveProperty<double> Height { get; private set; }
        public ReactiveProperty<double> Scale { get; private set; }
        public ReactiveProperty<WriteableBitmap> img { get; private set; }


        public ReactiveProperty<double> ScrollBarV { get; private set; }
        public ReactiveProperty<double> ScrollBarH { get; private set; }

        public ReactiveProperty<bool> isLinkage { get; private set; }

        public ReactiveProperty<int> MouseWheel { get; private set; }
        public ReactiveProperty<Point> DMove { get; private set; }


        public Document()
        {
            var model = App.Container.Resolve<Model>();

            isLinkage = model.ObserveProperty(x => x.isLinkage).ToReactiveProperty();

            MouseWheel = model.ToReactivePropertyAsSynchronized(
                x => x.Scale,
                convert: x => (int)Math.Log(x,2),
                convertBack: x => Math.Pow(2, x));
 
            Scale = model.ObserveProperty(x => x.Scale).Where(_ => isLinkage.Value == true).ToReactiveProperty();



                        var buf = new WriteableBitmap(new BitmapImage(new Uri(
                @"D:\Aki\Documents\aragakiyui_i5004.jpg",
                UriKind.Relative)));
            img = new ReactiveProperty<WriteableBitmap>(buf);
            //img = imgmodel.ObserveProperty(x => x.Bitmap).ToReactiveProperty();

            Width = img.CombineLatest(Scale, (x, y) => (x?.PixelWidth ?? 0) * y).ToReactiveProperty();
            Height = img.CombineLatest(Scale, (x, y) => (x?.PixelHeight ?? 0) * y).ToReactiveProperty();


            DMove = new ReactiveProperty<Point>();
            DMove.Subscribe(x =>
            {
                model.ScrollBarH -= x.X;
                model.ScrollBarV -= x.Y;
            });

            ScrollBarV = model.ToReactivePropertyAsSynchronized(x => x.ScrollBarV).Where(_ => isLinkage.Value == true).ToReactiveProperty();
            ScrollBarH = model.ToReactivePropertyAsSynchronized(x => x.ScrollBarH).Where(_ => isLinkage.Value == true).ToReactiveProperty();


        }
    }
}
