using Prism.Mvvm;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System.Reactive.Linq;
using System.Windows.Input;

using System.Windows;
using System;
using Reactive.Bindings.Interactivity;
using System.Windows.Interactivity;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Collections.Generic;

namespace PrismUnityApp.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        public ReactiveProperty<string> Title { get; private set; }

        public ReactiveProperty<Point> MouseMove { get; private set; }
        public ReactiveProperty<MouseWheelEventArgs> MouseWheel { get; private set; }

        public ReactiveProperty<double> Scale { get; private set; }

        public ReactiveProperty<double> Width { get; private set; }
        public ReactiveProperty<double> Height { get; private set; }

        public ReactiveProperty<WriteableBitmap> img { get; private set; }
        
        public ReactiveProperty<(double H, double V)> ScrollBar { get; private set; }
        public ReactiveProperty<IEnumerable<string>> Shapes { get; private set; }
        
        public MainWindowViewModel()
        {
            MouseMove = new ReactiveProperty<Point>();



            MouseWheel = new ReactiveProperty<MouseWheelEventArgs>();
            MouseWheel.Subscribe(x => { if (x != null) x.Handled = true; } ); //ホイールでのスクロール抑制

            var m_srcBitmap = new BitmapImage(new Uri(@"C76wJkLVMAUbiDl.jpg", UriKind.Relative));

            Scale = new ReactiveProperty<double>(1.0);
            img = new ReactiveProperty<WriteableBitmap>(new WriteableBitmap(m_srcBitmap));

            Scale = MouseWheel.Select(x => Scale.Value * Math.Pow(2,(x?.Delta / 120 ?? 0))).ToReactiveProperty();


            // Scaleの後　順番注意

            Width = img.CombineLatest(Scale, (x,y)=> x.PixelWidth * y).ToReactiveProperty();
            Height = img.CombineLatest(Scale, (x, y) => x.PixelHeight * y).ToReactiveProperty();

            ScrollBar = new ReactiveProperty<(double H, double V)>();
            
            Title = ScrollBar.Select(x => $"{x.H},{x.V}").ToReactiveProperty();

            Shapes = MouseMove.Select(x =>
                (IEnumerable<string>)new List<string>()
                {
                    //$"Circle,{x.X},{x.Y},5",
                    $"Text,{x.X},{x.Y-20},20,{x.X / Scale.Value}-{x.Y/ Scale.Value}",
                    $"Rect,{x.X},{x.Y-10,0},{1* Scale.Value},{1* Scale.Value}"
                }).ToReactiveProperty();
        }
    }
}
//