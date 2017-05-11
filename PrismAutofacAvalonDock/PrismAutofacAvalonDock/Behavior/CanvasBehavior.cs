using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows.Shapes;
using YamlDotNet.Serialization;

namespace PrismAutofacAvalonDock.Behavior
{
    public class GridBehavior : BehaviorBase<Grid>
    {
        public static readonly DependencyProperty MouseWheelProperty = DependencyProperty.Register(
            "MouseWheel", typeof(int), typeof(GridBehavior), new UIPropertyMetadata(0));
        public int MouseWheel { get => (int)GetValue(MouseWheelProperty); set => SetValue(MouseWheelProperty, value); }

        protected override void OnSetup()
        {
            base.OnSetup();
            this.AssociatedObject.Loaded += AssociatedObject_Loaded;
            this.AssociatedObject.MouseWheel += AssociatedObject_MouseWheel;
        }
        protected override void OnCleanup()
        {
            this.AssociatedObject.Loaded -= AssociatedObject_Loaded;
            this.AssociatedObject.MouseWheel -= AssociatedObject_MouseWheel;
            base.OnCleanup();
        }

        private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {

        }
        private void AssociatedObject_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            MouseWheel += e.Delta / 120;
            e.Handled = true;
        }
    }

    public class CanvasBehavior : BehaviorBase<Canvas>
    {
        public static readonly DependencyProperty MouseMoveProperty = DependencyProperty.Register(
            "MouseMove", typeof(Point), typeof(CanvasBehavior), new UIPropertyMetadata(null));
        public Point MouseMove{ get => (Point)GetValue(MouseMoveProperty); set => SetValue(MouseMoveProperty, value); }

        public static readonly DependencyProperty ScaleProperty = DependencyProperty.Register(
            "Scale", typeof(double), typeof(CanvasBehavior), new UIPropertyMetadata(1.0));
        public double Scale { get => (double)GetValue(ScaleProperty); set => SetValue(ScaleProperty, value); }

        public static readonly DependencyProperty RectProperty = DependencyProperty.Register(
            "Rect", typeof(Rect), typeof(CanvasBehavior), new UIPropertyMetadata(null));
        public Rect Rect { get => (Rect)GetValue(RectProperty); set => SetValue(RectProperty, value); }


        //プロパティ変更時にイベント発火させたいときはPropertyChangedを実装
        public static readonly DependencyProperty ShapesProperty = DependencyProperty.Register(
            "Shapes", typeof(List<Shape>), typeof(CanvasBehavior), new UIPropertyMetadata(null, ShapesPropertyChanged));
        public List<Shape> Shapes { get => (List<Shape>)GetValue(ShapesProperty); set => SetValue(ShapesProperty, value);}
        private static void ShapesPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var crtl = d as CanvasBehavior;
            crtl?.ReDraw();
        }

        public static readonly DependencyProperty ScalingModeProperty = DependencyProperty.Register(
            "ScalingMode", typeof(BitmapScalingMode), typeof(CanvasBehavior), new UIPropertyMetadata(BitmapScalingMode.Unspecified, ScalingModePropertyChanged));
        public BitmapScalingMode ScalingMode { get => (BitmapScalingMode)GetValue(ScalingModeProperty); set => SetValue(ScalingModeProperty, value);}
        private static void ScalingModePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //Load前に呼び出される
            var crtl = d as CanvasBehavior;
            if (crtl.cnv == null) return;
            RenderOptions.SetBitmapScalingMode(crtl.cnv, (BitmapScalingMode)e.NewValue);
        }

        Canvas cnv;
        bool flagLeftShift = false;
        Point PointR_old;

        protected override void OnSetup()
        {
            base.OnSetup();
            this.AssociatedObject.Loaded += AssociatedObject_Loaded;
            this.AssociatedObject.MouseMove += AssociatedObject_MouseMove;
            this.AssociatedObject.MouseDown += AssociatedObject_MouseDown;
            this.AssociatedObject.KeyDown += AssociatedObject_KeyDown;
            this.AssociatedObject.SizeChanged += AssociatedObject_LayoutUpdated;
        }
        protected override void OnCleanup()
        {
            this.AssociatedObject.Loaded -= AssociatedObject_Loaded;
            this.AssociatedObject.MouseMove -= AssociatedObject_MouseMove;
            this.AssociatedObject.MouseDown -= AssociatedObject_MouseDown;
            this.AssociatedObject.SizeChanged -= AssociatedObject_LayoutUpdated;
            //cnv = null;
            base.OnCleanup();
        }

        private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            cnv = sender as Canvas;
            RenderOptions.SetBitmapScalingMode(cnv, ScalingMode);
        }

        private void AssociatedObject_KeyDown(object sender, KeyEventArgs e)
        {

        }




        private void AssociatedObject_LayoutUpdated(object sender, EventArgs e)
        {
            //Scaleが変わった時の再描画に使える
            ReDraw();
        }

        private void AssociatedObject_MouseDown(object sender, MouseButtonEventArgs e)
        {
        }
        private void AssociatedObject_MouseMove(object sender, MouseEventArgs e)
        {
            var buf = e.GetPosition(cnv);//(Canvas)sender
            MouseMove = new Point(buf.X / Scale, buf.Y / Scale); 

            //Shift
            if ((Keyboard.Modifiers & ModifierKeys.Shift) > 0)
            {
                if (!flagLeftShift)
                {
                    PointR_old = MouseMove;
                    Rect = RectClip(new Rect(PointR_old, MouseMove));
                }
                else 
                {
                    Rect = RectClip(new Rect(PointR_old, MouseMove));
                }
                flagLeftShift = true;
            }
            else if ((Keyboard.Modifiers & ModifierKeys.Shift) == 0)
            {
                flagLeftShift = false;
            }

            Rect RectClip(Rect src)
            {
                Point sta = src.TopLeft;
                Point end = src.BottomRight;

                sta = new Point((int)sta.X, (int)sta.Y);
                end = new Point((int)end.X, (int)end.Y);

                return new Rect(sta, end);
            }
        }



        public void ReDraw()
        {
            if (cnv == null) return;
            cnv.Children.Clear();
            if (Shapes == null) return;
            //List<Shape> shp = new List<Shape>();
            ////シリアライズ
            //try
            //{
            //    shp = (new Deserializer()).Deserialize<List<Shape>>(Shapes);
            //}
            //catch
            //{
            //    return;
            //}

            foreach (var i in Shapes)
            {
                UIElement element = null;


                switch (i.Key)
                {
                    case "Circle":
                        //DrawCircle(arry);
                        break;
                    case "Caption":
                        if (i.Size < 0.0) return;
                        element = new TextBlock
                        {
                            FontSize = i.Size,
                            Foreground = new SolidColorBrush(i.Brush),
                            Text = i.Text
                        };
                        Canvas.SetLeft(element, i.Left);
                        Canvas.SetTop(element, i.Top);
                        break;
                    case "Text":
                        if (i.Size < 0.0) return;
                        element = new TextBlock
                        {
                            FontSize = i.Size,
                            Foreground = new SolidColorBrush(i.Brush),
                            Text = i.Text
                        };
                        Canvas.SetLeft(element, i.Left * Scale);
                        Canvas.SetTop(element, i.Top * Scale);
                        break;
                    case "Rectangle":
                    case "Rect":
                        if (i.Width <= 0.0 || i.Height <= 0.0) return;
                        
                        element = new Rectangle
                        {
                            Width = i.Width * Scale,
                            Height = i.Height * Scale,
                            StrokeThickness = 1.0,
                            Stroke = new SolidColorBrush(i.Brush),
                            Fill = new SolidColorBrush(i.Fill)
                        };
                        Canvas.SetLeft(element, i.Left * Scale);
                        Canvas.SetTop(element, i.Top * Scale);
                        break;
                    //case "Grid":
                    //    DrawGrid(arry);
                    //    break;
                    default:
                        break;
                }
                if(element != null) cnv.Children.Add(element);
            }
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
            double x = Scale * c[1].TryParse(0) * Scale;
            double y = Scale * c[2].TryParse(0) * Scale;
            double size = Scale * c[3].TryParse(0) * Scale;

            if (size <= 0.0) return;

            var element = new Ellipse
            {
                Width = 2.0 * size,
                Height = 2.0 * size,
                StrokeThickness = 1.0,
                Stroke = Brushes.Red
            };
            Canvas.SetLeft(element, x - size);
            Canvas.SetTop(element, y - size);
            cnv.Children.Add(element);
        }


    }

    public static class CanvasBehaviorExtentions
    {
        public static double TryParse(this string value, double defaultValue)
        {
            return double.TryParse(value, out double i) ? i : defaultValue;
        }
    }

    public class Shape
    {
        //実座標いれて
        public string Key { get; set; }
        public string Text { get; set; } = "";
        public double Left { get; set; }
        public double Top { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double Size { get; set; }
        public Color Brush { get; set; } = Colors.Red;
        public Color Fill { get; set; } = new Color() { A = 0 };
    }
}