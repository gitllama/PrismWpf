using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace Behavior
{
    public class ShortcutBehavior : BehaviorBase<Window>
    {

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
            typeof(ShortcutBehavior),
            new PropertyMetadata(null)
        );

        public (bool, bool, bool) KeyState
        {
            get => ((bool, bool, bool))GetValue(KeyStateProperty);
            set => SetValue(KeyStateProperty, value);
        }
        public static readonly DependencyProperty KeyStateProperty = DependencyProperty.Register(
            nameof(KeyState),
            typeof((bool, bool, bool)),
            typeof(ShortcutBehavior),
            new UIPropertyMetadata((false, false, false)));

        #endregion


        #region OnSetup/Cleanup

        protected override void OnSetup()
        {
            base.OnSetup();
            this.AssociatedObject.Loaded += AssociatedObject_Loaded;
            this.AssociatedObject.PreviewKeyDown += AssociatedObject_KeyDown;
            this.AssociatedObject.PreviewKeyUp += AssociatedObject_PreviewKeyUp;
        }

        protected override void OnCleanup()
        {
            this.AssociatedObject.Loaded -= AssociatedObject_Loaded;
            this.AssociatedObject.PreviewKeyDown -= AssociatedObject_KeyDown;
            this.AssociatedObject.PreviewKeyUp -= AssociatedObject_PreviewKeyUp;
            base.OnCleanup();
        }

        #endregion


        #region Event


        private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void AssociatedObject_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

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
                    //Console.WriteLine(GetStringKeyState(KeyState) + e.Key.ToString());
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


        #endregion

    }
}
