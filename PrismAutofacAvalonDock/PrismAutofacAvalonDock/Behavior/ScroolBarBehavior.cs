using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace PrismAutofacAvalonDock.Behavior
{
    public class ScroolBarBehavior : BehaviorBase<ScrollViewer>
    {
        public static readonly DependencyProperty HPositionProperty = DependencyProperty.Register(
            "HPosition",
            typeof(double),
            typeof(ScroolBarBehavior),
            new UIPropertyMetadata(0.0, HPropertyChanged));
        public double HPosition
        {
            get => (double)GetValue(HPositionProperty);
            set
            {
                SetValue(HPositionProperty, value);
            }
        }
        public static readonly DependencyProperty VPositionProperty = DependencyProperty.Register(
            "VPosition",
            typeof(double),
            typeof(ScroolBarBehavior),
            new UIPropertyMetadata(0.0, VPropertyChanged));
        public double VPosition
        {
            get => (double)GetValue(VPositionProperty);
            set
            {
                SetValue(VPositionProperty, value);
            }
        }

        private static void VPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var crtl = d as ScroolBarBehavior;
            if (crtl?.sv?.VerticalOffset == (double)e.NewValue) return;
            crtl?.sv?.ScrollToVerticalOffset((double)e.NewValue);
        }
        private static void HPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var crtl = d as ScroolBarBehavior;
            if (crtl?.sv?.HorizontalOffset == (double)e.NewValue) return;

            crtl?.sv?.ScrollToHorizontalOffset((double)e.NewValue);
        }

        protected override void OnSetup()
        {
            base.OnSetup();
            this.AssociatedObject.Loaded += AssociatedObject_Loaded;
            this.AssociatedObject.ScrollChanged += AssociatedObject_ScrollChanged;
        }
        protected override void OnCleanup()
        {
            //this.AssociatedObject.Loaded -= AssociatedObject_Loaded;
            //this.AssociatedObject.ScrollChanged -= AssociatedObject_ScrollChanged;
            //sv = null;
            base.OnCleanup();
        }

        ScrollViewer sv;
        private void AssociatedObject_Loaded(object sender, EventArgs e)
        {
            sv = sender as ScrollViewer;
        }
        private void AssociatedObject_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (sv == null) return;

            if(VPosition != sv.VerticalOffset) VPosition = sv.VerticalOffset;
            if (HPosition != sv.HorizontalOffset) HPosition = sv.HorizontalOffset;
        }
    }
}
