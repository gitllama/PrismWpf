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
        
        public ReactiveProperty<(double H, double V)> ScrollBar { get; private set; }
        public ReactiveProperty<string> Shape { get; private set; }
        
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

            ScrollBar = new ReactiveProperty<(double H, double V)>();
            
            Title = MouseMove.Select(x => $"{x.X / Scale.Value},{x.Y / Scale.Value}").ToReactiveProperty();
            //Title = ScrollBar.Select(x => $"{x.H},{x.V}").ToReactiveProperty();

            Shape = MouseMove.Select(x=>$"Text,{x.X},{x.Y},20,TEST").ToReactiveProperty();
        }

    }

    public class CanvasBehavior : Behavior<Canvas>
    {
        public static readonly DependencyProperty MouseMoveProperty = DependencyProperty.Register(
            "MouseMove",
            typeof(Point),
            typeof(CanvasBehavior),
            new UIPropertyMetadata(null));
        public Point MouseMove
        {
            get => (Point)GetValue(MouseMoveProperty);
            set => SetValue(MouseMoveProperty, value);
        }

        //プロパティ変更時にイベント発火させたいときはPropertyChangedを実装
        public static readonly DependencyProperty ShapeProperty = DependencyProperty.Register(
            "Shape",
            typeof(string),
            typeof(CanvasBehavior),
            new UIPropertyMetadata(null, ShapePropertyChanged));
        private static void ShapePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var crtl = d as CanvasBehavior;
            crtl.ReDraw();
        }

        public string Shape
        {
            get => (string)GetValue(ShapeProperty);
            set => SetValue(ShapeProperty, value);
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.Loaded += AssociatedObject_Loaded;
            this.AssociatedObject.MouseMove += AssociatedObject_MouseMove;
            this.AssociatedObject.MouseDown += AssociatedObject_MouseDown;
            this.AssociatedObject.SizeChanged += AssociatedObject_LayoutUpdated;
        }

        private void AssociatedObject_LayoutUpdated(object sender, EventArgs e)
        {
            //Scaleが変わった時の再描画に使える
            ReDraw();
        }

        private void AssociatedObject_MouseDown(object sender, MouseButtonEventArgs e)
        {
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.Loaded -= AssociatedObject_Loaded;
            this.AssociatedObject.MouseMove -= AssociatedObject_MouseMove;
            this.AssociatedObject.MouseDown -= AssociatedObject_MouseDown;
            this.AssociatedObject.SizeChanged -= AssociatedObject_LayoutUpdated;
            cnv = null;
        }

        Canvas cnv;
        double uScale = 1; 
        private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            cnv = sender as Canvas;
        }
        private void AssociatedObject_MouseMove(object sender, MouseEventArgs e)
        {
            MouseMove = e.GetPosition((Canvas)sender);
        }


        public void ReDraw()
        {
            if (cnv == null) return;
            cnv.Children.Clear();
            if (Shape == null)
            {
                return;
            }
            DrawText(Shape.Split(','));
            //using (IEnumerator<string> enumerator = Shape.GetEnumerator())
            //{
            //    while (enumerator.MoveNext())
            //    {
            //        string[] array = enumerator.Current.Split(new char[]
            //        {
            //            ','
            //        });
            //        string a = array[0].Trim();
            //        if (!(a == "Grid"))
            //        {
            //            if (!(a == "Circle"))
            //            {
            //                if (!(a == "Rectangle") && !(a == "Rect"))
            //                {
            //                    if (a == "Text")
            //                    {
            //                        DrawText(array);
            //                    }
            //                }
            //                else
            //                {
            //                    DrawRectangle(array);
            //                }
            //            }
            //            else
            //            {
            //                DrawCircle(array);
            //            }
            //        }
            //        else
            //        {
            //            //DrawGrid(ctrl, array);
            //        }
            //    }
            //}
        }

        //private static void DrawGrid(Canvas ctrl, string[] c)
        //{
        //    int num2;
        //    int num = int.TryParse(c[1], out num2) ? num2 : 0;
        //    if (num < 1)
        //    {
        //        return;
        //    }
        //    int num3 = 0;
        //    while ((double)num3 < this.uWidth)
        //    {
        //        Line element = new Line
        //        {
        //            X1 = (double)num3 * this.uScale,
        //            Y1 = 0.0,
        //            X2 = (double)num3 * this.uScale,
        //            Y2 = this.uHeight * this.uScale,
        //            StrokeThickness = 1.0,
        //            Stroke = Brushes.Red
        //        };
        //        cnv.Children.Add(element);
        //        num3 += num;
        //    }
        //    int num4 = 0;
        //    while ((double)num4 < this.uHeight)
        //    {
        //        Line element2 = new Line
        //        {
        //            X1 = 0.0,
        //            Y1 = (double)num4 * this.uScale,
        //            X2 = this.uWidth * this.uScale,
        //            Y2 = (double)num4 * this.uScale,
        //            StrokeThickness = 1.0,
        //            Stroke = Brushes.Red
        //        };
        //        cnv.Children.Add(element2);
        //        num4 += num;
        //    }
        //}

        private void DrawCircle(string[] c)
        {
            double num2;
            double num = double.TryParse(c[1], out num2) ? (num2 * uScale) : -1.0;
            double num4;
            double num3 = double.TryParse(c[2], out num4) ? (num4 * uScale) : -1.0;
            double num6;
            double num5 = double.TryParse(c[3], out num6) ? (num6 * uScale) : -1.0;
            if (num5 < 0.0)
            {
                return;
            }
            Ellipse element = new Ellipse
            {
                Width = num5 * 2.0,
                Height = num5 * 2.0,
                StrokeThickness = 1.0,
                Stroke = Brushes.Red
            };
            Canvas.SetLeft(element, num - num5);
            Canvas.SetTop(element, num3 - num5);
            cnv.Children.Add(element);
        }

        private void DrawRectangle(string[] c)
        {
            double num;
            double length = double.TryParse(c[1], out num) ? (num * uScale) : -1.0;
            double num2;
            double length2 = double.TryParse(c[2], out num2) ? (num2 * uScale) : -1.0;
            double num4;
            double num3 = double.TryParse(c[3], out num4) ? (num4 * uScale) : -1.0;
            double num6;
            double num5 = double.TryParse(c[4], out num6) ? (num6 * uScale) : -1.0;
            if (num3 < 0.0)
            {
                return;
            }
            if (num5 < 0.0)
            {
                return;
            }
            Rectangle element = new Rectangle
            {
                Width = num3,
                Height = num5,
                StrokeThickness = 1.0,
                Stroke = Brushes.Red
            };
            Canvas.SetLeft(element, length);
            Canvas.SetTop(element, length2);
            cnv.Children.Add(element);
        }

        private void DrawText(string[] c)
        {
            double num;
            double length = double.TryParse(c[1], out num) ? (num * uScale) : -1.0;
            double num2;
            double length2 = double.TryParse(c[2], out num2) ? (num2 * uScale) : -1.0;
            double num4;
            double num3 = double.TryParse(c[3], out num4) ? (num4 * uScale) : -1.0;
            string text = c[4] ?? "";
            if (num3 < 0.0)
            {
                return;
            }
            TextBlock element = new TextBlock
            {
                FontSize = num3,
                Foreground = Brushes.Red,
                Text = text
            };
            Canvas.SetLeft(element, length);
            Canvas.SetTop(element, length2);
            cnv.Children.Add(element);
        }
    }

    public class ScroolBarBehavior : Behavior<ScrollViewer>
    {
        public static readonly DependencyProperty PositionProperty = DependencyProperty.Register(
            "Position",
            typeof((double H,double V)),
            typeof(ScroolBarBehavior),
            new UIPropertyMetadata((0.0, 0.0)));
        public (double H, double V) Position
        {
            get => ((double H, double V))GetValue(PositionProperty);
            set
            {
                SetValue(PositionProperty, value);

                //obj.ScrollToVerticalOffset
                //obj.ScrollToHorizontalOffset
            }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.Loaded += AssociatedObject_Loaded;
            this.AssociatedObject.ScrollChanged += AssociatedObject_ScrollChanged;
        }
        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.Loaded -= AssociatedObject_Loaded;
            this.AssociatedObject.ScrollChanged -= AssociatedObject_ScrollChanged;
            sv = null;
        }

        ScrollViewer sv;
        private void AssociatedObject_Loaded(object sender, EventArgs e)
        {
            sv = sender as ScrollViewer;
        }
        private void AssociatedObject_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            //var obj = sender as ScrollViewer;
            //Position = (obj.HorizontalOffset, obj.VerticalOffset);
            if (sv == null) return;
            Position = (sv.HorizontalOffset, sv.VerticalOffset);
        }
    }
}
