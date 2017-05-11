using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace PrismAutofacAvalonDock.Behavior
{
    public class ScroolBarBehavior : BehaviorBase<ScrollViewer>
    {


        public static readonly DependencyProperty ScrollBarPositionProperty = DependencyProperty.Register(
            "ScrollBarPosition", typeof(Point), typeof(ScroolBarBehavior), new UIPropertyMetadata(new Point(0,0), ScrollBarPositionPropertyChanged));
        public Point ScrollBarPosition { get => (Point)GetValue(ScrollBarPositionProperty); set=> SetValue(ScrollBarPositionProperty, value);}

        private static void ScrollBarPositionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var crtl = d as ScroolBarBehavior;
            Point v = (Point)e.NewValue;
            if (crtl?.sv?.VerticalOffset != v.Y) crtl?.sv?.ScrollToVerticalOffset(v.Y);
            if (crtl?.sv?.HorizontalOffset != v.X) crtl?.sv?.ScrollToHorizontalOffset(v.X);
        }

        ScrollViewer sv;

        protected override void OnSetup()
        {
            base.OnSetup();

            this.AssociatedObject.Loaded += AssociatedObject_Loaded;
            this.AssociatedObject.ScrollChanged += AssociatedObject_ScrollChanged;
            this.AssociatedObject.MouseMove += AssociatedObject_MouseMove;
            //this.AssociatedObject.MouseWheel += AssociatedObject_MouseWheel;
        }
        protected override void OnCleanup()
        {
            this.AssociatedObject.ScrollChanged -= AssociatedObject_ScrollChanged;
            this.AssociatedObject.MouseMove -= AssociatedObject_MouseMove;
            //this.AssociatedObject.MouseWheel -= AssociatedObject_MouseWheel;
            //sv = null;
            base.OnCleanup();
        }

        private void AssociatedObject_Loaded(object sender, EventArgs e)
        {
            sv = sender as ScrollViewer;
        }

        private Point PointL_old;
        private bool flagLeft = false;
        private void AssociatedObject_MouseMove(object sender, MouseEventArgs e)
        {
            //左
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var i = e.GetPosition(null);
                if (!flagLeft)
                {
                    PointL_old = e.GetPosition(null);
                }
                else
                {
                    double x = (i.X - PointL_old.X);
                    double y = (i.Y - PointL_old.Y);

                    sv.ScrollToVerticalOffset(sv.VerticalOffset - y);
                    sv.ScrollToHorizontalOffset(sv.HorizontalOffset - x);

                    PointL_old = i;
                }
                flagLeft = true;
            }
            else if (e.LeftButton == MouseButtonState.Released)
            {
                flagLeft = false;
            }
        }

        private void AssociatedObject_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (sv == null) return;
            Point v = new Point(sv.HorizontalOffset, sv.VerticalOffset);
            if (ScrollBarPosition != v) ScrollBarPosition = v;
        }


        //private void AssociatedObject_MouseWheel(object sender, MouseWheelEventArgs e)
        //{
        //    //なぜかひろわない
        //    MouseWheel += e.Delta / 120;
        //    e.Handled = true;
        //}
    }

}
