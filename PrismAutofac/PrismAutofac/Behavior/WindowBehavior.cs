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


    /*

    View:
    VisibleFoldingMark="{Binding VisibleFoldingMarkFromBehavior.Value, Mode=OneWayToSource}"

    Behavior:
    public static readonly DependencyProperty VisibleFoldingMarkProperty = DependencyProperty.Register(
        nameof(VisibleFoldingMark), 
        typeof(bool), 
        typeof(WindowBehavior), 
        new UIPropertyMetadata(false)
    );
    public bool VisibleFoldingMark { get => (bool)GetValue(VisibleFoldingMarkProperty); set => SetValue(VisibleFoldingMarkProperty, value); }

    VM:
    public ReactiveProperty<bool> VisibleFoldingMarkFromBehavior { get; private set; }
    public ReactiveProperty<string> VisibleFoldingMarkToView { get; private set; }
    this.VisibleFoldingMarkFromBehavior = new ReactiveProperty<bool>();
    this.VisibleFoldingMarkToView = VisibleFoldingMarkFromBehavior.Select(x => x ? "Visible" : "Collapsed").ToReactiveProperty();

        
    */

    public class WindowBehavior : BehaviorBase<Window>
    {

        Window win;

        #region Property

        public static readonly DependencyProperty LeftProperty = DependencyProperty.Register(
            nameof(Left), 
            typeof(double), 
            typeof(WindowBehavior), 
            new UIPropertyMetadata(1.0, SizePropertyChanged)
        );
        public double Left { get => (double)GetValue(LeftProperty); set => SetValue(LeftProperty, value); }

        public static readonly DependencyProperty TopProperty = DependencyProperty.Register(
            nameof(Top),
            typeof(double), 
            typeof(WindowBehavior), 
            new UIPropertyMetadata(1.0, SizePropertyChanged)
        );
        public double Top { get => (double)GetValue(TopProperty); set => SetValue(TopProperty, value); }

        public static readonly DependencyProperty WidthProperty = DependencyProperty.Register(
            nameof(Width),
            typeof(double), 
            typeof(WindowBehavior), 
            new UIPropertyMetadata(640.0, SizePropertyChanged));
        public double Width { get => (double)GetValue(WidthProperty); set => SetValue(WidthProperty, value); }

        public static readonly DependencyProperty HeightProperty = DependencyProperty.Register(
            nameof(Height),
            typeof(double), 
            typeof(WindowBehavior), 
            new UIPropertyMetadata(480.0, SizePropertyChanged));
        public double Height { get => (double)GetValue(HeightProperty); set => SetValue(HeightProperty, value); }

        private static void SizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var canvas = d as WindowBehavior;
            if (canvas.win == null) return; //Load前に呼び出される
            canvas.win.Width = canvas.Width;
            canvas.win.Height = canvas.Height;
            canvas.win.Left = canvas.Left;
            canvas.win.Top = canvas.Top;
        }

        #endregion


        #region OnSetup/Cleanup

        protected override void OnSetup()
        {
            base.OnSetup();
            this.AssociatedObject.Initialized += AssociatedObject_Initialized;
        }

        protected override void OnCleanup()
        {
            this.AssociatedObject.Initialized -= AssociatedObject_Initialized;
            win = null;
            base.OnCleanup();
        }

        #endregion


        #region Event

        private void AssociatedObject_Initialized(object sender, EventArgs e)
        {
            win = sender as Window;
            //win.Width = Width;
            //win.Height = Height;
            //win.Left = Left;
            //win.Top = Top;
        }

        #endregion

    }

}
