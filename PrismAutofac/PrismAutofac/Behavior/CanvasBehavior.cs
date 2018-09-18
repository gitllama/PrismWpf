using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Behavior
{

    public class CanvasBehavior : BehaviorBase<ScrollViewer>
    {
        ScrollViewer cnv;
        bool flagLeftShift = false;
        Point PointR_old;


        public static readonly DependencyProperty MouseMoveProperty = DependencyProperty.Register(
            "MouseMove", 
            typeof(Point), 
            typeof(CanvasBehavior), 
            new UIPropertyMetadata(null));
        public Point MouseMove { get => (Point)GetValue(MouseMoveProperty); set => SetValue(MouseMoveProperty, value); }

        public static readonly DependencyProperty ScaleProperty = DependencyProperty.Register(
            "Scale", 
            typeof(double), 
            typeof(CanvasBehavior), 
            new UIPropertyMetadata(1.0, CanvasPropertyChanged));
        public double Scale { get => (double)GetValue(ScaleProperty); set => SetValue(ScaleProperty, value); }

        public static readonly DependencyProperty AutoScaleProperty = DependencyProperty.Register(
            "AutoScale", 
            typeof(bool), 
            typeof(CanvasBehavior), 
            new UIPropertyMetadata(false, CanvasPropertyChanged));
        public bool AutoScale { get => (bool)GetValue(AutoScaleProperty); set => SetValue(AutoScaleProperty, value); }


        private static void CanvasPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as CanvasBehavior;
            if (ctrl?.cnv == null) return;

            ctrl.ReDraw();

        }

        protected override void OnSetup()
        {
            base.OnSetup();
            this.AssociatedObject.Loaded += AssociatedObject_Loaded;
            this.AssociatedObject.MouseMove += AssociatedObject_MouseMove;
            this.AssociatedObject.MouseDown += AssociatedObject_MouseDown;
            //this.AssociatedObject.KeyDown += AssociatedObject_KeyDown;

            this.AssociatedObject.SizeChanged += AssociatedObject_LayoutUpdated;
            this.AssociatedObject.LayoutUpdated += AssociatedObject_LayoutUpdated;
            this.AssociatedObject.ScrollChanged += AssociatedObject_LayoutUpdated;
        }
        protected override void OnCleanup()
        {
            this.AssociatedObject.Loaded -= AssociatedObject_Loaded;
            this.AssociatedObject.MouseMove -= AssociatedObject_MouseMove;
            this.AssociatedObject.MouseDown -= AssociatedObject_MouseDown;
            //this.AssociatedObject.KeyDown -= AssociatedObject_KeyDown;
            //this.AssociatedObject.SizeChanged -= AssociatedObject_LayoutUpdated;
            this.AssociatedObject.LayoutUpdated -= AssociatedObject_LayoutUpdated;
            //this.AssociatedObject.ScrollChanged -= AssociatedObject_LayoutUpdated;
            //cnv = null;
            base.OnCleanup();
        }

        private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            cnv = sender as ScrollViewer;
            //RenderOptions.SetBitmapScalingMode(cnv, ScalingMode);
            ReDraw();
        }

        private void AssociatedObject_KeyDown(object sender, KeyEventArgs e)
        {

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
                    //Rect = RectClip(new Rect(PointR_old, MouseMove));
                }
                else
                {
                    //Rect = RectClip(new Rect(PointR_old, MouseMove));
                }
                flagLeftShift = true;
            }
            else if ((Keyboard.Modifiers & ModifierKeys.Shift) == 0)
            {
                flagLeftShift = false;
            }

            //Rect RectClip(Rect src)
            //{
            //    Point sta = src.TopLeft;
            //    Point end = src.BottomRight;

            //    sta = new Point((int)sta.X, (int)sta.Y);
            //    end = new Point((int)end.X, (int)end.Y);

            //    return new Rect(sta, end);
            //}
        }

        private void AssociatedObject_LayoutUpdated(object sender, EventArgs e)
        {
            //Scaleが変わった時の再描画に使える
            ReDraw();
        }

        public void ReDraw()
        {
            if (cnv == null) return;
            var vb = cnv.FindName("viewbox") as Viewbox;
            var grid = cnv.FindName("canvas") as Canvas;
            var image = cnv.FindName("image") as Image;
            var a = cnv.FindName("scale") as ScaleTransform;

            grid.Width = image.Source.Width;
            grid.Height = image.Source.Height;

            if (AutoScale)
            {
                a.ScaleX = 1;
                a.ScaleY = 1;

                image.Width = image.Source.Width * 1;
                image.Height = image.Source.Height * 1;
                vb.Stretch = Stretch.Uniform;
                cnv.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
                cnv.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
            }
            else
            {
                vb.Stretch = Stretch.None;
                cnv.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                cnv.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
                a.ScaleX = Scale;
                a.ScaleY = Scale;
                a.CenterX = image.Source.Width / 2;
                a.CenterY = image.Source.Height / 2;

                image.Width = image.Source.Width * Scale;
                image.Height = image.Source.Height * Scale;
            }
        }
    }
}
