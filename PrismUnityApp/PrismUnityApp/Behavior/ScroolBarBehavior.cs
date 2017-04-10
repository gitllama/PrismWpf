using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace PrismUnityApp.Behavior
{
    public class ScroolBarBehavior : BehaviorBase<ScrollViewer>
    {
        public static readonly DependencyProperty PositionProperty = DependencyProperty.Register(
            "Position",
            typeof((double H, double V)),
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

        protected override void OnSetup()
        {
            base.OnSetup();
            this.AssociatedObject.Loaded += AssociatedObject_Loaded;
            this.AssociatedObject.ScrollChanged += AssociatedObject_ScrollChanged;
        }
        protected override void OnCleanup()
        {
            this.AssociatedObject.Loaded -= AssociatedObject_Loaded;
            this.AssociatedObject.ScrollChanged -= AssociatedObject_ScrollChanged;
            sv = null;
            base.OnCleanup();
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
