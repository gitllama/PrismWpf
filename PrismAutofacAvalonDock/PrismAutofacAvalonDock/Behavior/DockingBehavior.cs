using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using Xceed.Wpf.AvalonDock;

namespace PrismAutofacAvalonDock.Behavior
{
    public class DockingBehavior : BehaviorBase<Xceed.Wpf.AvalonDock.DockingManager>
    {
        public ICommand TargetCommand
        {
            get { return (ICommand)GetValue(TargetCommandProperty); }
            set { SetValue(TargetCommandProperty, value); }
        }
        public static readonly DependencyProperty TargetCommandProperty = DependencyProperty.Register(
                "TargetCommand",
                typeof(ICommand),
                typeof(DockingBehavior),
                new PropertyMetadata(null));

        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.Loaded += AssociatedObject_Loaded;
            this.AssociatedObject.DocumentClosed += AssociatedObject_DocumentClosed;
        }
        protected override void OnDetaching()
        {
            this.AssociatedObject.Loaded -= AssociatedObject_Loaded;
            this.AssociatedObject.DocumentClosed -= AssociatedObject_DocumentClosed;
            base.OnDetaching();
        }

        DockingManager m;
        private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            m = sender as DockingManager;
        }
        private void AssociatedObject_DocumentClosed(object sender, DocumentClosedEventArgs e)
        {
            if (TargetCommand == null) return;
            var buf = e.Document.ContentId.ToString();
            if (buf != null) TargetCommand.Execute(buf);
        }
    }
}
