using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace BTCV.Behavior
{
    //親のwindowに権利移譲したけど、マウスはフォーカスで動作変化するのでチョイ考える
    public class ScroolBarBehavior : BehaviorBase<ScrollViewer>
    {

        public ICommand ShortcutCommand { get => (ICommand)GetValue(ShortcutCommandProperty); set => SetValue(ShortcutCommandProperty, value); }
        public static readonly DependencyProperty ShortcutCommandProperty = DependencyProperty.Register(
            "ShortcutCommand", typeof(ICommand), typeof(ScroolBarBehavior), new PropertyMetadata(null));

        public static readonly DependencyProperty KeyStateProperty = DependencyProperty.Register(
            "KeyState", typeof((bool, bool, bool)), typeof(ScroolBarBehavior), new UIPropertyMetadata((false, false, false)));
        public (bool, bool, bool) KeyState { get => ((bool, bool, bool))GetValue(KeyStateProperty); set => SetValue(KeyStateProperty, value); }


        //public static readonly DependencyProperty ScaleProperty = DependencyProperty.Register(
        //    "Scale", typeof(double), typeof(ScroolBarBehavior), new UIPropertyMetadata(1.0));
        //public double Scale { get => (double)GetValue(ScaleProperty); set => SetValue(ScaleProperty, value); }



        //public static readonly DependencyProperty ScrollBarPositionProperty = DependencyProperty.Register(
        //    "ScrollBarPosition", typeof(Point), typeof(ScroolBarBehavior), new UIPropertyMetadata(new Point(0, 0), ScrollBarPositionPropertyChanged));
        //public Point ScrollBarPosition { get => (Point)GetValue(ScrollBarPositionProperty); set => SetValue(ScrollBarPositionProperty, value); }

        //private static void ScrollBarPositionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    var crtl = d as ScroolBarBehavior;
        //    Point v = (Point)e.NewValue;
        //    if (crtl?.sv?.VerticalOffset != v.Y) crtl?.sv?.ScrollToVerticalOffset(v.Y);
        //    if (crtl?.sv?.HorizontalOffset != v.X) crtl?.sv?.ScrollToHorizontalOffset(v.X);
        //}

        ScrollViewer sv;

        protected override void OnSetup()
        {
            base.OnSetup();

            this.AssociatedObject.Loaded += AssociatedObject_Loaded;
            //this.AssociatedObject.ScrollChanged += AssociatedObject_ScrollChanged;
            //this.AssociatedObject.MouseMove += AssociatedObject_MouseMove;
            this.AssociatedObject.PreviewMouseWheel += AssociatedObject_MouseWheel;
            this.AssociatedObject.MouseDown += AssociatedObject_MouseDown;
            this.AssociatedObject.PreviewKeyDown += AssociatedObject_KeyDown;
            this.AssociatedObject.PreviewKeyUp += AssociatedObject_PreviewKeyUp; ;
        }

        protected override void OnCleanup()
        {
            //this.AssociatedObject.ScrollChanged -= AssociatedObject_ScrollChanged;
            //this.AssociatedObject.MouseMove -= AssociatedObject_MouseMove;
            this.AssociatedObject.PreviewMouseWheel -= AssociatedObject_MouseWheel;
            this.AssociatedObject.MouseDown -= AssociatedObject_MouseDown;
            this.AssociatedObject.PreviewKeyDown -= AssociatedObject_KeyDown;
            this.AssociatedObject.PreviewKeyUp -= AssociatedObject_PreviewKeyUp;
            //sv = null;
            base.OnCleanup();
        }

        private void AssociatedObject_Loaded(object sender, EventArgs e)
        {
            sv = sender as ScrollViewer;
        }

        //private Point PointL_old;
        //private bool flagLeft = false;
        //private void AssociatedObject_MouseMove(object sender, MouseEventArgs e)
        //{
        //    //左
        //    if (e.LeftButton == MouseButtonState.Pressed)
        //    {
        //        var i = e.GetPosition(null);
        //        if (!flagLeft)
        //        {
        //            PointL_old = e.GetPosition(null);
        //        }
        //        else
        //        {
        //            double x = (i.X - PointL_old.X);
        //            double y = (i.Y - PointL_old.Y);

        //            sv.ScrollToVerticalOffset(sv.VerticalOffset - y);
        //            sv.ScrollToHorizontalOffset(sv.HorizontalOffset - x);

        //            PointL_old = i;
        //        }
        //        flagLeft = true;
        //    }
        //    else if (e.LeftButton == MouseButtonState.Released)
        //    {
        //        flagLeft = false;
        //    }
        //}

        //private void AssociatedObject_ScrollChanged(object sender, ScrollChangedEventArgs e)
        //{
        //    if (sv == null) return;
        //    Point v = new Point(sv.HorizontalOffset, sv.VerticalOffset);
        //    if (ScrollBarPosition != v) ScrollBarPosition = v;
        //}



        string GetStringKeyState((bool, bool,bool) v) => $"{(v.Item1 ? "Shift" : "")}{(v.Item2 ? "Ctrl" : "")}{(v.Item3 ? "Alt" : "")}";
      
        private void AssociatedObject_KeyDown(object sender, KeyEventArgs e)
        {
            KeyState = (
                (Keyboard.Modifiers & ModifierKeys.Shift) > 0,
                (Keyboard.Modifiers & ModifierKeys.Control) > 0,
                (Keyboard.Modifiers & ModifierKeys.Alt) > 0);

            //Alt系はSystem命令とかぶるのでマスクする必要あり
            switch (e.Key)
            {
                case Key.LeftAlt:
                case Key.LeftCtrl:
                case Key.LeftShift:
                case Key.RightAlt:
                case Key.RightCtrl:
                case Key.RightShift:
                case Key.System:
                    break;
                default:
                    ShortcutCommand.Execute(GetStringKeyState(KeyState) + e.Key.ToString());
                    break;
            }

            //if (e.Key == (Key.P & Key.LeftShift))
            //    ShortcutCommand.Execute("XButton1");


        }

        private void AssociatedObject_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            KeyState = (
                (Keyboard.Modifiers & ModifierKeys.Shift) > 0,
                (Keyboard.Modifiers & ModifierKeys.Control) > 0,
                (Keyboard.Modifiers & ModifierKeys.Alt) > 0);
        }

        private void AssociatedObject_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.XButton1 == MouseButtonState.Pressed) ShortcutCommand.Execute("XButton1");
            if (e.XButton2 == MouseButtonState.Pressed) ShortcutCommand.Execute("XButton2");
            if (e.MiddleButton == MouseButtonState.Pressed) ShortcutCommand.Execute("MiddleButton");
        }

        private void AssociatedObject_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            KeyState = (
                    (Keyboard.Modifiers & ModifierKeys.Shift) > 0,
                    (Keyboard.Modifiers & ModifierKeys.Control) > 0,
                    (Keyboard.Modifiers & ModifierKeys.Alt) > 0);

            if (e.Delta / 120 > 0) ShortcutCommand.Execute(GetStringKeyState(KeyState) + "MouseWheel+");
            if (e.Delta / 120 < 0) ShortcutCommand.Execute(GetStringKeyState(KeyState) + "MouseWheel-");

            e.Handled = true;

            //if (
            //    (Keyboard.GetKeyStates(Key.LeftCtrl) & KeyStates.Down) == KeyStates.Down ||
            //    (Keyboard.GetKeyStates(Key.RightCtrl) & KeyStates.Down) == KeyStates.Down
            //    )
            //{
            //}
        }

    }
}
