using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace Behavior
{
    public class WindowBehavior : BehaviorBase<Window>
    {

        Window cnv;
        ColumnDefinition splitter;

        #region Property

        //public static readonly DependencyProperty ShapesProperty = DependencyProperty.Register(
        //    "Shapes", typeof(string), typeof(WindowBehavior), new UIPropertyMetadata(null, ShapesPropertyChanged));
        //public string Shapes { get => (string)GetValue(ShapesProperty); set => SetValue(ShapesProperty, value); }

        //private static void ShapesPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    var crtl = d as CanvasBehavior;
        //    crtl?.ReDraw();
        //}

        #endregion


        bool flag = false;

        protected override void OnSetup()
        {
            base.OnSetup();
            this.AssociatedObject.Loaded += AssociatedObject_Loaded;
            this.AssociatedObject.MouseMove += AssociatedObject_MouseMove;
            this.AssociatedObject.MouseDown += AssociatedObject_MouseDown;
            this.AssociatedObject.QueryCursor += AssociatedObject_QueryCursor;
        }
        protected override void OnCleanup()
        {
            this.AssociatedObject.Loaded -= AssociatedObject_Loaded;
            this.AssociatedObject.MouseMove -= AssociatedObject_MouseMove;
            this.AssociatedObject.MouseDown -= AssociatedObject_MouseDown;
            this.AssociatedObject.QueryCursor -= AssociatedObject_QueryCursor;
            cnv = null;
            base.OnCleanup();
        }

        private void AssociatedObject_QueryCursor(object sender, QueryCursorEventArgs e)
        {
            if (flag)
            {
                e.Cursor = Cursors.Hand;
            }
            else
            {
                //e.Cursor = Cursors.Cross;
            }
            e.Handled = true;
        }



        private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            cnv = sender as Window;
            foreach (var child in LogicalTreeHelper.GetChildren(cnv))
            {
                var i = child as Grid;
                if(i != null)
                {
                    foreach (var child2 in LogicalTreeHelper.GetChildren(i))
                    {
                        var j = child2 as Grid;
                        if (j != null)
                        {
                            foreach (var child3 in LogicalTreeHelper.GetChildren(j))
                            {
                                Console.WriteLine(child3.GetType().ToString());
                                splitter = child3 as ColumnDefinition;
                                var name = splitter?.Name ?? "";
                                if (name == "A") break;
                            }
                            break;
                        }
                    }
                    break;
                }
            }
        }

        private void AssociatedObject_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void AssociatedObject_MouseMove(object sender, MouseEventArgs e)
        {
            var buf = e.GetPosition(cnv);
            var offset = splitter?.Offset ?? 0;


            if (buf.X > offset) flag = true;
            if (buf.X < offset - 30) flag = false;

        }

    }
}
