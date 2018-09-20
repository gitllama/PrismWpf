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
using MahApps.Metro.SimpleChildWindow;
using Prism.Interactivity;
using MahApps.Metro.Controls;

namespace BTCV.Behavior
{
    public class MetroPopupWindowAction : PopupWindowAction
    {
        protected override Window CreateWindow()
        {
            return new MetroWindow
            {
                Style = new Style(),
                SizeToContent = SizeToContent.WidthAndHeight
            };
        }
    }

    public class WindowBehavior : BehaviorBase<Window>
    {
        Window root;

        #region Property

        public ICommand ShortcutCommand
        {
            get => (ICommand)GetValue(ShortcutCommandProperty);
            set => SetValue(ShortcutCommandProperty, value);
        }
        public static readonly DependencyProperty ShortcutCommandProperty = DependencyProperty.Register
        (
            nameof(ShortcutCommand), 
            typeof(ICommand), 
            typeof(WindowBehavior), 
            new PropertyMetadata(null)
        );

        public (bool, bool, bool) KeyState
        {
            get => ((bool, bool, bool))GetValue(KeyStateProperty);
            set => SetValue(KeyStateProperty, value);
        }
        public static readonly DependencyProperty KeyStateProperty = DependencyProperty.Register
        (
            nameof(KeyState), 
            typeof((bool, bool, bool)), 
            typeof(WindowBehavior), 
            new UIPropertyMetadata((false, false, false))
        );




        public static readonly DependencyProperty LeftProperty = DependencyProperty.Register(
            "Left", typeof(double), typeof(WindowBehavior), new UIPropertyMetadata(1.0, SizePropertyChanged));
        public double Left { get => (double)GetValue(LeftProperty); set => SetValue(LeftProperty, value); }

        public static readonly DependencyProperty TopProperty = DependencyProperty.Register(
            "Top", typeof(double), typeof(WindowBehavior), new UIPropertyMetadata(1.0, SizePropertyChanged));
        public double Top { get => (double)GetValue(TopProperty); set => SetValue(TopProperty, value); }

        public static readonly DependencyProperty WidthProperty = DependencyProperty.Register(
            "Width", typeof(double), typeof(WindowBehavior), new UIPropertyMetadata(1.0, SizePropertyChanged));
        public double Width { get => (double)GetValue(WidthProperty); set => SetValue(WidthProperty, value); }

        public static readonly DependencyProperty HeightProperty = DependencyProperty.Register(
            "Height", typeof(double), typeof(WindowBehavior), new UIPropertyMetadata(1.0, SizePropertyChanged));
        public double Height { get => (double)GetValue(HeightProperty); set => SetValue(HeightProperty, value); }

        private static void SizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var canvas = d as WindowBehavior;
            if (canvas.root == null) return; //Load前に呼び出される
            canvas.root.Width = canvas.Width;
            canvas.root.Height = canvas.Height;
            canvas.root.Left = canvas.Left;
            canvas.root.Top = canvas.Top;
        }

        public ICommand ChildWindowCommand { get => (ICommand)GetValue(ChildWindowCommandProperty); set => SetValue(ChildWindowCommandProperty, value); }
        public static readonly DependencyProperty ChildWindowCommandProperty = DependencyProperty.Register(
            nameof(ChildWindowCommand), 
            typeof(ICommand), 
            typeof(WindowBehavior), 
            new PropertyMetadata(null));

        #endregion


        #region OnSetup/OnCleanup

        protected override void OnSetup()
        {
            base.OnSetup();
            this.AssociatedObject.Initialized += AssociatedObject_Initialized;
            this.AssociatedObject.Loaded += AssociatedObject_Loaded;

            this.AssociatedObject.PreviewMouseWheel += AssociatedObject_MouseWheel;
            this.AssociatedObject.MouseDown += AssociatedObject_MouseDown;
            this.AssociatedObject.PreviewKeyDown += AssociatedObject_KeyDown;
            this.AssociatedObject.PreviewKeyUp += AssociatedObject_PreviewKeyUp; ;

        }
        protected override void OnCleanup()
        {
            //propertyWindow?.Close();
            //this.AssociatedObject.Initialized -= AssociatedObject_Initialized;
            //this.AssociatedObject.Loaded -= AssociatedObject_Loaded;
            root = null;
            base.OnCleanup();
        }



        private void AssociatedObject_Initialized(object sender, EventArgs e)
        {
            root = sender as Window;
            root.Width = Width;
            root.Height = Height;
            root.Left = Left;
            root.Top = Top;
        }

        Views.ChildWindow propertyWindow;
        Views.ChildWindow controlWindow;
        private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            ChildWindowCommand = new Prism.Commands.DelegateCommand<string>(async (x) =>
            {
                switch (x)
                {
                    case "PropertyWindow":
                        {
                            if (propertyWindow == null || !propertyWindow.IsVisible)
                            {
                                propertyWindow = null;
                                propertyWindow = new Views.ChildWindow
                                {
                                    Name = "childWindow",
                                    Width = 400,
                                    Height = 600,
                                    RegionName = "PropertyRegion"
                                };
                                propertyWindow.Show();
                            }
                            else
                            {
                                propertyWindow.Focus();
                            }
                        }
                        break;
                    case "ControlWindow":
                        {
                            if (controlWindow == null || !controlWindow.IsVisible)
                            {
                                controlWindow = null;
                                controlWindow = new Views.ChildWindow
                                {
                                    Name = "childWindow",
                                    Width = 400,
                                    Height = 600,
                                    RegionName = "ControlRegion"

                                };
                                controlWindow.Show();
                            }
                            else
                            {
                                controlWindow.Focus();
                            }
                        }
                        break;
                }
                //await ((MahApps.Metro.Controls.MetroWindow)win).ShowChildWindowAsync(
                //    new Views.PropertyWindow() {   },
                //    ChildWindowManager.OverlayFillBehavior.FullWindow);


            });
        }

        #endregion

        string GetStringKeyState((bool, bool, bool) v) => $"{(v.Item1 ? "Shift" : "")}{(v.Item2 ? "Ctrl" : "")}{(v.Item3 ? "Alt" : "")}";

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

            // 子要素に伝播させたくない時true
            // e.Handled = true;

            //if (
            //    (Keyboard.GetKeyStates(Key.LeftCtrl) & KeyStates.Down) == KeyStates.Down ||
            //    (Keyboard.GetKeyStates(Key.RightCtrl) & KeyStates.Down) == KeyStates.Down
            //    )
            //{
            //}
        }

    }
}





