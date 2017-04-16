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
using Prism.Interactivity.InteractionRequest;
using Prism.Commands;

namespace PrismUnityApp.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {

        Model model = new Model();
        ImageModel imgmodel = new ImageModel();



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




        public ReactiveCommand PropertyCommand { get; private set; }
        public InteractionRequest<INotification> PropertyRequest { get; private set; }

        public ReactiveCommand FileDropCommand { get; private set; }

        public InteractionRequest<INotification> NotificationRequest { get; set; }
        public DelegateCommand NotificationCommand { get; set; }

        public MainWindowViewModel()
        {



            Scale = model.ObserveProperty(x => x.Scale).ToReactiveProperty();
            ScalingMode = model.ObserveProperty(x => x.ScalingMode).ToReactiveProperty();



            MouseMove = new ReactiveProperty<Point>();

            MouseWheel = new ReactiveProperty<MouseWheelEventArgs>();
            MouseWheel.Subscribe(x => { if (x != null) x.Handled = true; } ); //ホイールでのスクロール抑制
            
            img = new ReactiveProperty<WriteableBitmap>();
            img = imgmodel.ObserveProperty(x => x.Bitmap).ToReactiveProperty();

            Scale = MouseWheel.Select(x => Scale.Value * Math.Pow(2,(x?.Delta / 120 ?? 0))).ToReactiveProperty();


            // Scaleの後　順番注意

            Width = img.CombineLatest(Scale, (x,y)=> (x?.PixelWidth ?? 0) * y).ToReactiveProperty();
            Height = img.CombineLatest(Scale, (x, y) => (x?.PixelHeight ??0) * y).ToReactiveProperty();

            ScrollBar = new ReactiveProperty<(double H, double V)>();
            
            //Title = ScrollBar.Select(x => $"{x.H},{x.V}").ToReactiveProperty();
            Title = imgmodel.ObserveProperty(x => x.FileName).ToReactiveProperty();

            Shapes = MouseMove.Select(x =>
                (IEnumerable<string>)new List<string>()
                {
                    //$"Circle,{x.X},{x.Y},5",
                    $"Text,{x.X},{x.Y-20},20,{(int)(x.X / Scale.Value)}-{(int)(x.Y/ Scale.Value)}",
                    $"Rect,{(int)(x.X / Scale.Value) *Scale.Value },{(int)(x.Y / Scale.Value) *Scale.Value},{1* Scale.Value},{1* Scale.Value}"
                }).ToReactiveProperty();


            PropertyCommand = new ReactiveCommand();
            PropertyRequest = new InteractionRequest<INotification>();
            PropertyCommand.Subscribe(_ => PropertyRequest.Raise(new Notification { Title = "編集" }));

            FileDropCommand = new ReactiveCommand();
            NotificationRequest = new InteractionRequest<INotification>();
            FileDropCommand.Subscribe(x =>
            {
                var i = x as string[];
                var ii = RegexFile.Match("config.yaml", i[0]);

                if(ii == null)
                {
                    NotificationRequest.Raise(new Notification
                    {
                        Content = "Notification Message",
                        Title = "Notification"
                    });
                }
                else
                {
                    imgmodel.FileName = i[0];
                }


            });


        }
    }
}
