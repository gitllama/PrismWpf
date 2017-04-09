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

        public MainWindowViewModel()
        {
            MouseMove = new ReactiveProperty<Point>(mode: ReactivePropertyMode.RaiseLatestValueOnSubscribe);



            MouseWheel = new ReactiveProperty<MouseWheelEventArgs>();
            MouseWheel.Subscribe(x => { if (x != null) x.Handled = true; } ); //ホイールでのスクロール抑制

            var m_srcBitmap = new BitmapImage(new Uri(@"C76wJkLVMAUbiDl.jpg", UriKind.Relative));

            Scale = new ReactiveProperty<double>(1.0);
            img = new ReactiveProperty<WriteableBitmap>(new WriteableBitmap(m_srcBitmap));

            Scale = MouseWheel.Select(x => Scale.Value * Math.Pow(2,(x?.Delta / 120 ?? 0))).ToReactiveProperty();


            // Scaleの後　順番注意

            Width = img.CombineLatest(Scale, (x,y)=> x.PixelWidth * y).ToReactiveProperty();
            Height = img.CombineLatest(Scale, (x, y) => x.PixelHeight * y).ToReactiveProperty();

            Title = MouseMove.Select(x => $"{x.X / Scale.Value},{x.Y / Scale.Value}").ToReactiveProperty();

        }

    }


    public class MouseMoveBehavior : Behavior<Canvas>
    {
        public static readonly DependencyProperty PositionProperty = DependencyProperty.Register(
            "Position", 
            typeof(Point), 
            typeof(MouseMoveBehavior), 
            new UIPropertyMetadata(null));
        public Point Position 
        {
            get => (Point)GetValue(PositionProperty);
            set => SetValue(PositionProperty, value);
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.MouseMove += AssociatedObject_MouseMove;
        }
        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.MouseMove -= AssociatedObject_MouseMove;
        }

        private void AssociatedObject_MouseMove(object sender, MouseEventArgs e)
        {
            Position = e.GetPosition((Canvas)sender);
        }

    }

    public class ScroolBarBehavior : Behavior<ScrollViewer>
    {
        public static readonly DependencyProperty VPositonProperty = DependencyProperty.Register(
            "VPositon",
            typeof(double),
            typeof(ScroolBarBehavior),
            new UIPropertyMetadata(0));
        public double VPositon
        {
            get => (double)GetValue(VPositonProperty);
            set => SetValue(VPositonProperty, value);
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.ScrollChanged += AssociatedObject_ScrollChanged;
        }
        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.ScrollChanged -= AssociatedObject_ScrollChanged;
        }
        private void AssociatedObject_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var obj = sender as ScrollViewer;

            //obj.ScrollToVerticalOffset
            //obj.ScrollToHorizontalOffset
        }
    }

    public class CanvasShapeBehavior : Behavior<Canvas>
    {
        public static readonly DependencyProperty ShapeProperty = DependencyProperty.Register(
            "Shape",
            typeof(string),
            typeof(CanvasShapeBehavior),
            new UIPropertyMetadata(0));
        public string Shape
        {
            get => (string)GetValue(ShapeProperty);
            set
            {
                SetValue(ShapeProperty, value);
            }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            //this.AssociatedObject.ScrollChanged += AssociatedObject_ScrollChanged;
        }
        protected override void OnDetaching()
        {
            base.OnDetaching();
            //this.AssociatedObject.ScrollChanged -= AssociatedObject_ScrollChanged;
        }
    }
}
