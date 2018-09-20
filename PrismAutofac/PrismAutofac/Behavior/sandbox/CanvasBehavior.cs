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
using System.Drawing;

namespace BTCV.Behavior
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

        bool flag = false;
        public void ReDraw()
        {
            if (cnv == null) return;
            var vb = cnv.FindName("viewbox") as Viewbox;
            var grid = cnv.FindName("canvas") as Canvas;
            var image = cnv.FindName("image") as Image;
            var a = cnv.FindName("scale") as ScaleTransform;


            if (AutoScale)
            {
                image.Width = image.Source.Width * 0.1;
                image.Height = image.Source.Height * 0.1;
                vb.Stretch = Stretch.Uniform;
                cnv.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                cnv.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;


                if (!flag)
                {
                    //grid.Width = 1;
                    //grid.Height = 1;
                    //a.ScaleX = 1;
                    //a.ScaleY = 1;
                    //image.Width = 1;
                    //image.Height = 1;
                    //vb.Stretch = Stretch.Uniform;
                }

                flag = true;
            }
            else
            {
                vb.Stretch = Stretch.None;
                cnv.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                cnv.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
                //grid.Width = image.Source.Width * Scale;
                //grid.Height = image.Source.Height * Scale;
                //a.ScaleX = Scale;
                //a.ScaleY = Scale;
                //a.CenterX = image.Source.Width / 2;
                //a.CenterY = image.Source.Height / 2;

                image.Width = image.Source.Width * Scale;
                image.Height = image.Source.Height * Scale;
                flag = false;
            }
        }
    }


    public class CanvasBehaviorOld : BehaviorBase<Canvas>
    {
        Canvas cnv;

        #region Property
        //プロパティ変更時にイベント発火させたいときはPropertyChangedを実装

        public static readonly DependencyProperty WidthProperty = DependencyProperty.Register(
            "Width", typeof(double), typeof(CanvasBehaviorOld), new UIPropertyMetadata(1.0, ScalePropertyChanged));
        public double Width { get => (double)GetValue(WidthProperty); set => SetValue(WidthProperty, value); }

        public static readonly DependencyProperty HeightProperty = DependencyProperty.Register(
            "Height", typeof(double), typeof(CanvasBehaviorOld), new UIPropertyMetadata(1.0, ScalePropertyChanged));
        public double Height { get => (double)GetValue(HeightProperty); set => SetValue(HeightProperty, value); }

        public static readonly DependencyProperty ScaleProperty = DependencyProperty.Register(
            "Scale", typeof(double), typeof(CanvasBehaviorOld), new UIPropertyMetadata(1.0, ScalePropertyChanged));
        public double Scale { get => (double)GetValue(ScaleProperty); set => SetValue(ScaleProperty, value); }

        private static void ScalePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var canvas = d as CanvasBehaviorOld;
            if (canvas.cnv == null) return; //Load前に呼び出される
            canvas.cnv.Width = canvas.Width * canvas.Scale;
            canvas.cnv.Height = canvas.Height * canvas.Scale;
            canvas?.ReDraw();
        }



        public static readonly DependencyProperty ShapesProperty = DependencyProperty.Register(
            "Shapes", typeof(IEnumerable<ShapeParam>), typeof(CanvasBehaviorOld), new UIPropertyMetadata(null, ShapesPropertyChanged));
        public IEnumerable<ShapeParam> Shapes { get => (IEnumerable<ShapeParam>)GetValue(ShapesProperty); set => SetValue(ShapesProperty, value); }
        private static void ShapesPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            var canvas = d as CanvasBehaviorOld;
            if (canvas.cnv == null) return; //Load前に呼び出される
            canvas?.ReDraw();
        }

        public static readonly DependencyProperty ScalingModeProperty = DependencyProperty.Register(
            "ScalingMode", typeof(BitmapScalingMode), typeof(CanvasBehaviorOld), new UIPropertyMetadata(BitmapScalingMode.Unspecified, ScalingModePropertyChanged));
        public BitmapScalingMode ScalingMode { get => (BitmapScalingMode)GetValue(ScalingModeProperty); set => SetValue(ScalingModeProperty, value); }
        private static void ScalingModePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var canvas = d as CanvasBehaviorOld;
            if (canvas.cnv == null) return; //Load前に呼び出される
            RenderOptions.SetBitmapScalingMode(canvas.cnv, (BitmapScalingMode)e.NewValue);
        }

        #endregion


        #region OnSetup/OnCleanup

        protected override void OnSetup()
        {
            base.OnSetup();
            this.AssociatedObject.Loaded += AssociatedObject_Loaded;
            //this.AssociatedObject.MouseMove += AssociatedObject_MouseMove;
            //this.AssociatedObject.MouseDown += AssociatedObject_MouseDown;
            //this.AssociatedObject.KeyDown += AssociatedObject_KeyDown;
            //this.AssociatedObject.SizeChanged += AssociatedObject_LayoutUpdated;
        }
        protected override void OnCleanup()
        {
            this.AssociatedObject.Loaded -= AssociatedObject_Loaded;
            //this.AssociatedObject.MouseMove -= AssociatedObject_MouseMove;
            //this.AssociatedObject.MouseDown -= AssociatedObject_MouseDown;
            //this.AssociatedObject.KeyDown -= AssociatedObject_KeyDown;
            //this.AssociatedObject.SizeChanged -= AssociatedObject_LayoutUpdated;
            //cnv = null;
            base.OnCleanup();
        }

        private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            cnv = sender as Canvas;
            cnv.Width = Width * Scale;
            cnv.Height = Height * Scale;
            RenderOptions.SetBitmapScalingMode(cnv, ScalingMode);
        }

        #endregion


        #region Draw Shapes

        public void ReDraw()
        {
            if (cnv == null) return;
            cnv.Children.Clear();
            if (Shapes == null) return;
            foreach (var element in Shapes)
            {
                cnv.Children.Add(element.Create(Scale));
            }
            //try
            //{
            //    //var json = JObject.Parse(Shapes);
            //    //DrawShape.Draw(cnv, json, Scale);
            //}
            //catch
            //{
            //}
        }

        #endregion

    }


}











