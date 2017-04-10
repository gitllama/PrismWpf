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

namespace PrismUnityApp.Behavior
{
    public class CanvasBehavior : BehaviorBase<Canvas>
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
        public static readonly DependencyProperty ShapesProperty = DependencyProperty.Register(
            "Shapes",
            typeof(IEnumerable<string>),
            typeof(CanvasBehavior),
            new UIPropertyMetadata(null, ShapesPropertyChanged));
        public IEnumerable<string> Shapes
        {
            get => (IEnumerable<string>)GetValue(ShapesProperty);
            set => SetValue(ShapesProperty, value);
        }
        private static void ShapesPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var crtl = d as CanvasBehavior;
            crtl?.ReDraw();
        }


        protected override void OnSetup()
        {
            base.OnSetup();
            this.AssociatedObject.Loaded += AssociatedObject_Loaded;
            this.AssociatedObject.MouseMove += AssociatedObject_MouseMove;
            this.AssociatedObject.MouseDown += AssociatedObject_MouseDown;
            this.AssociatedObject.SizeChanged += AssociatedObject_LayoutUpdated;
        }
        protected override void OnCleanup()
        {
            this.AssociatedObject.Loaded -= AssociatedObject_Loaded;
            this.AssociatedObject.MouseMove -= AssociatedObject_MouseMove;
            this.AssociatedObject.MouseDown -= AssociatedObject_MouseDown;
            this.AssociatedObject.SizeChanged -= AssociatedObject_LayoutUpdated;
            cnv = null;
            base.OnCleanup();
        }


        private void AssociatedObject_LayoutUpdated(object sender, EventArgs e)
        {
            //Scaleが変わった時の再描画に使える
            ReDraw();
        }

        private void AssociatedObject_MouseDown(object sender, MouseButtonEventArgs e)
        {
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
            if (Shapes == null) return;

            foreach(var i in Shapes)
            {
                var arry = i.Split(',');
                switch(arry[0].Trim())
                {
                    case "Circle":
                        DrawCircle(arry);
                        break;
                    case "Text":
                        DrawText(arry);
                        break;
                    case "Rectangle":
                    case "Rect":
                        DrawRectangle(arry);
                        break;
                    //case "Grid":
                    //    DrawGrid(arry);
                    //    break;
                    default:
                        break;
                }

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

        /**/

        //Circle, x ,y, size
        private void DrawCircle(string[] c)
        {
            double x = uScale * c[1].TryParse(0) * uScale;
            double y = uScale * c[2].TryParse(0) * uScale;
            double size = uScale * c[3].TryParse(0) * uScale;

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
        //Rect, x ,y, w, h
        private void DrawRectangle(string[] c)
        {
            double x = uScale * c[1].TryParse(0) * uScale;
            double y = uScale * c[2].TryParse(0) * uScale;
            double w = uScale * c[3].TryParse(0) * uScale;
            double h = uScale * c[4].TryParse(0) * uScale;

            if (w <= 0.0 || h <= 0.0 ) return;

            var element = new Rectangle
            {
                Width = w,
                Height = h,
                StrokeThickness = 1.0,
                Stroke = Brushes.Red
            };
            Canvas.SetLeft(element, x);
            Canvas.SetTop(element, y);
            cnv.Children.Add(element);
        }
        //Text, x ,y, s, text
        private void DrawText(string[] c)
        {
            double x = uScale * c[1].TryParse(0) * uScale;
            double y = uScale * c[2].TryParse(0) * uScale;
            double s = uScale * c[3].TryParse(0) * uScale;

            string text = c[4] ?? "";
            if (s < 0.0) return;

            var element = new TextBlock
            {
                FontSize = s,
                Foreground = Brushes.Red,
                Text = text
            };
            Canvas.SetLeft(element, x);
            Canvas.SetTop(element, y);
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
}
