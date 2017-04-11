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
using PrismUnityApp.Models;
using System.Windows.Media;

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

        public ReactiveProperty<BitmapScalingMode> ScalingMode { get; private set; }

        Model model = new Model();
        public ReactiveProperty<Object> obj { get; private set; }

        public MainWindowViewModel()
        {
            obj = new ReactiveProperty<object>(model);


            Scale = model.ObserveProperty(x => x.Scale).ToReactiveProperty();
            ScalingMode = model.ObserveProperty(x => x.ScalingMode).ToReactiveProperty();



            MouseMove = new ReactiveProperty<Point>();

            MouseWheel = new ReactiveProperty<MouseWheelEventArgs>();
            MouseWheel.Subscribe(x => { if (x != null) x.Handled = true; } ); //ホイールでのスクロール抑制

            var m_srcBitmap = new BitmapImage(new Uri(@"C76wJkLVMAUbiDl.jpg", UriKind.Relative));

            
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
                    $"Text,{x.X},{x.Y-20},20,{(int)(x.X / Scale.Value)}-{(int)(x.Y/ Scale.Value)}",
                    $"Rect,{(int)(x.X / Scale.Value) *Scale.Value },{(int)(x.Y / Scale.Value) *Scale.Value},{1* Scale.Value},{1* Scale.Value}"
                }).ToReactiveProperty();



        }
    }
}
//