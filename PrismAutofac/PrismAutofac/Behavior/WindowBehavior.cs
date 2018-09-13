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

    public class GridSplitterBehavior : BehaviorBase<Window>
    {

        Window win;
        TextBlock foldingMarkLeft;
        TextBlock foldingMarkRight;

        int margin = 3;

        #region Property

        public static readonly DependencyProperty VisibleLeftRegionProperty = DependencyProperty.Register(
            nameof(VisibleLeftRegion),
            typeof(bool),
            typeof(GridSplitterBehavior),
            new UIPropertyMetadata(true, LeftRegionPropertyChanged)
        );
        public bool VisibleLeftRegion { get => (bool)GetValue(VisibleLeftRegionProperty); set => SetValue(VisibleLeftRegionProperty, value); }

        private static void LeftRegionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = d as GridSplitterBehavior;
            if (obj.win == null) return;
            LeftRegionShow(obj);
        }

        public static readonly DependencyProperty VisibleRightRegionProperty = DependencyProperty.Register(
            nameof(VisibleRightRegion),
            typeof(bool),
            typeof(GridSplitterBehavior),
            new UIPropertyMetadata(true, RightRegionPropertyChanged)
        );
        public bool VisibleRightRegion { get => (bool)GetValue(VisibleRightRegionProperty); set => SetValue(VisibleRightRegionProperty, value); }

        private static void RightRegionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = d as GridSplitterBehavior;
            if (obj.win == null) return;
            RightRegionShow(obj);
        }

        public static readonly DependencyProperty OffsetProperty = DependencyProperty.Register(
            nameof(Offset),
            typeof(int),
            typeof(GridSplitterBehavior),
            new UIPropertyMetadata(30)
        );
        public int Offset { get => (int)GetValue(OffsetProperty); set => SetValue(OffsetProperty, value); }


        public static readonly DependencyProperty FoldingMarkWidthProperty = DependencyProperty.Register(
            nameof(FoldingMarkWidth),
            typeof(int),
            typeof(GridSplitterBehavior),
            new UIPropertyMetadata(25)
        );
        public int FoldingMarkWidth { get => (int)GetValue(FoldingMarkWidthProperty); set => SetValue(FoldingMarkWidthProperty, value); }

        public static readonly DependencyProperty AnimationTimeProperty = DependencyProperty.Register(
            nameof(AnimationTime),
            typeof(int),
            typeof(GridSplitterBehavior),
            new UIPropertyMetadata(100)
        );
        public int AnimationTime { get => (int)GetValue(AnimationTimeProperty); set => SetValue(AnimationTimeProperty, value); }

    
        #endregion


        #region OnSetup/Cleanup

        protected override void OnSetup()
        {
            base.OnSetup();
            this.AssociatedObject.Loaded += AssociatedObject_Loaded;
            this.AssociatedObject.MouseMove += AssociatedObject_MouseMove;
            //this.AssociatedObject.MouseDown += AssociatedObject_MouseDown;
            //this.AssociatedObject.QueryCursor += AssociatedObject_QueryCursor;
        }

        protected override void OnCleanup()
        {
            this.AssociatedObject.Loaded -= AssociatedObject_Loaded;
            this.AssociatedObject.MouseMove -= AssociatedObject_MouseMove;
            //this.AssociatedObject.MouseDown -= AssociatedObject_MouseDown;
            //this.AssociatedObject.QueryCursor -= AssociatedObject_QueryCursor;
            win = null;
            foldingMarkLeft = null;
            foldingMarkRight = null;
            base.OnCleanup();
        }

        #endregion


        #region Event

        private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            win = sender as Window;
            //mainDefinition = win.FindName("mainDefinition") as ColumnDefinition;
            foldingMarkLeft = win.FindName("foldingMarkLeft") as TextBlock;
            foldingMarkRight = win.FindName("foldingMarkRight") as TextBlock;
            foldingMarkLeft.Width = minMark;
            foldingMarkRight.Width = minMark;

            //以下のような探し方も可能
            /*
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
            */
        }


        private void AssociatedObject_MouseMove(object sender, MouseEventArgs e)
        {
            if (win == null) return;

            var mousePosition = e.GetPosition(win);
            Point parent = win.PointToScreen(new Point(0.0d, 0.0d));

            var i1 = win.FindName("leftDefinition") as ColumnDefinition;
            var i2 = win.FindName("mainDefinition") as ColumnDefinition;
            var i3 = win.FindName("rightDefinition") as ColumnDefinition;
            var w = win.FindName("splitterLeft") as GridSplitter;
            //Point left = foldingMarkLeft.PointToScreen(new Point(0.0d, 0.0d));
            //Point right = foldingMarkRight.PointToScreen(new Point(0.0d, 0.0d));

            //mainDefinition?.Offset ?? 0, 
            //(mainDefinition?.Offset ?? 0) + (mainDefinition?.Width.Value ?? 0), 

            Console.WriteLine($"{mousePosition.X} {i1.ActualWidth}");
            FoldingMarkLeftAnimation(foldingMarkLeft,
                mousePosition.X,
                i1.ActualWidth + w.ActualWidth + margin,
                Offset, FoldingMarkWidth, AnimationTime);

            FoldingMarkRightAnimation(foldingMarkRight,
                mousePosition.X,
                i1.ActualWidth + w.ActualWidth + i2.ActualWidth - margin,
                Offset, FoldingMarkWidth, AnimationTime);
        }

        private void AssociatedObject_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void AssociatedObject_QueryCursor(object sender, QueryCursorEventArgs e)
        {
            //if (VisibleFoldingMark)
            //{
            //    //e.Cursor = Cursors.Hand;
            //}
            //else
            //{
            //    //e.Cursor = Cursors.Cross;
            //}
            e.Handled = true;
        }

        #endregion





        #region Animation

        static int minMark = 0;

        private static void FoldingMarkLeftAnimation(TextBlock target, double mouse, double splitterLeft, double offset, int width, int time)
        {
            if (mouse < splitterLeft && target.Width == minMark)
            {
                MarkVisible(target, width, time);
            }
            else if (mouse > splitterLeft + offset && target.Width == width)
            {
                MarkHidden(target, width, time);
            }
        }

        private static void FoldingMarkRightAnimation(TextBlock target, double mouse, double splitterLeft, double offset, int width, int time)
        {
            if (mouse > splitterLeft && target.Width == minMark)
            {
                MarkVisible(target, width, time);

            }
            else if (mouse < splitterLeft - offset && target.Width == width)
            {
                MarkHidden(target, width, time);
            }
        }

        private static void MarkVisible(TextBlock target, int width, int time)
        {
            var storyboard = new Storyboard();
            var animation = new DoubleAnimation
            {
                From = minMark,
                To = width,
                Duration = TimeSpan.FromMilliseconds(time)
            };
            Storyboard.SetTargetProperty(animation, new PropertyPath("Width"));
            storyboard.Children.Add(animation);

            target.BeginStoryboard(storyboard);

            //foldingMark.Visibility = Visibility.Visible;
            //VisibleFoldingMark = true;
        }

        private static void MarkHidden(TextBlock target, int width, int time)
        {
            var storyboard = new Storyboard();
            var animation = new DoubleAnimation
            {
                From = width,
                To = minMark,
                Duration = TimeSpan.FromMilliseconds(time) //0.1sec
            };
            Storyboard.SetTargetProperty(animation, new PropertyPath("Width"));
            storyboard.Children.Add(animation);

            target.BeginStoryboard(storyboard);

            //var animation2 = new StringAnimationUsingKeyFrames()
            //{
            //    Duration = TimeSpan.FromMilliseconds(100) //0.1sec
            //};
            //animation2.KeyFrames.Add(new DiscreteStringKeyFrame()
            //{
            //    Value = "Visible",
            //    KeyTime = TimeSpan.FromMilliseconds(0)
            //});
            //animation2.KeyFrames.Add(new DiscreteStringKeyFrame()
            //{
            //    Value = "Collapsed",
            //    KeyTime = TimeSpan.FromMilliseconds(100)
            //});
            //Storyboard.SetTargetProperty(animation2, new PropertyPath("Visibility"));
            //storyboard.Children.Add(animation2);


            //VisibleFoldingMark = false;
        }

        #endregion


        #region HiddenAction

        GridLength _columnWidthLeft;
        GridLength _columnWidthRight;

        private static void LeftRegionShow(GridSplitterBehavior obj)
        {
            var child = obj.win.FindName("leftDefinition") as ColumnDefinition;
            var childGrid = obj.win.FindName("gridLeft") as Grid;
            var childSplitter = obj.win.FindName("splitterLeft") as GridSplitter;
            if (childGrid.Visibility == Visibility.Visible)
            {
                obj._columnWidthLeft = child.Width;
                child.Width = new GridLength(0);
                childGrid.Visibility = Visibility.Hidden;
                childSplitter.Visibility = Visibility.Hidden;
            }
            else
            {
                childGrid.Visibility = Visibility.Visible;
                childSplitter.Visibility = Visibility.Visible;
                child.Width = obj._columnWidthLeft;
            }
        }

        private static void RightRegionShow(GridSplitterBehavior obj)
        {
            var child = obj.win.FindName("rightDefinition") as ColumnDefinition;
            var childGrid = obj.win.FindName("gridRight") as Grid;
            var childSplitter = obj.win.FindName("splitterRight") as GridSplitter;
            if (childGrid.Visibility == Visibility.Visible)
            {
                obj._columnWidthRight = child.Width;
                child.Width = new GridLength(0);
                childGrid.Visibility = Visibility.Hidden;
                childSplitter.Visibility = Visibility.Hidden;
            }
            else
            {
                childGrid.Visibility = Visibility.Visible;
                childSplitter.Visibility = Visibility.Visible;
                child.Width = obj._columnWidthRight;
            }
        }

        #endregion
    }

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
        TextBlock foldingMarkLeft;
        TextBlock foldingMarkRight;

        #region Property

        //public static readonly DependencyProperty VisibleFoldingMarkProperty = DependencyProperty.Register(
        //    nameof(VisibleFoldingMark), 
        //    typeof(bool), 
        //    typeof(WindowBehavior), 
        //    new UIPropertyMetadata(false)
        //);
        //public bool VisibleFoldingMark { get => (bool)GetValue(VisibleFoldingMarkProperty); set => SetValue(VisibleFoldingMarkProperty, value); }


        public static readonly DependencyProperty VisibleLeftRegionProperty = DependencyProperty.Register(
            nameof(VisibleLeftRegion),
            typeof(bool),
            typeof(WindowBehavior),
            new UIPropertyMetadata(true, LeftRegionPropertyChanged)
        );
        public bool VisibleLeftRegion { get => (bool)GetValue(VisibleLeftRegionProperty); set => SetValue(VisibleLeftRegionProperty, value); }

        private static void LeftRegionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = d as WindowBehavior;
            if (obj.win == null) return;
            LeftRegionShow(obj);
        }

        public static readonly DependencyProperty VisibleRightRegionProperty = DependencyProperty.Register(
            nameof(VisibleRightRegion),
            typeof(bool),
            typeof(WindowBehavior),
            new UIPropertyMetadata(true, RightRegionPropertyChanged)
        );
        public bool VisibleRightRegion { get => (bool)GetValue(VisibleRightRegionProperty); set => SetValue(VisibleRightRegionProperty, value); }

        private static void RightRegionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = d as WindowBehavior;
            if (obj.win == null) return;
            RightRegionShow(obj);
        }

        public static readonly DependencyProperty OffsetProperty = DependencyProperty.Register(
            nameof(Offset),
            typeof(int),
            typeof(WindowBehavior),
            new UIPropertyMetadata(30)
        );
        public int Offset { get => (int)GetValue(OffsetProperty); set => SetValue(OffsetProperty, value); }


        public static readonly DependencyProperty FoldingMarkWidthProperty = DependencyProperty.Register(
            nameof(FoldingMarkWidth),
            typeof(int),
            typeof(WindowBehavior),
            new UIPropertyMetadata(25)
        );
        public int FoldingMarkWidth { get => (int)GetValue(FoldingMarkWidthProperty); set => SetValue(FoldingMarkWidthProperty, value); }

        public static readonly DependencyProperty AnimationTimeProperty = DependencyProperty.Register(
            nameof(AnimationTime),
            typeof(int),
            typeof(WindowBehavior),
            new UIPropertyMetadata(100)
        );
        public int AnimationTime { get => (int)GetValue(AnimationTimeProperty); set => SetValue(AnimationTimeProperty, value); }






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

        public static readonly DependencyProperty ShortcutCommandProperty = DependencyProperty.Register(
            nameof(ShortcutCommand), 
            typeof(ICommand), 
            typeof(WindowBehavior), 
            new PropertyMetadata(null));
        public ICommand ShortcutCommand { get => (ICommand)GetValue(ShortcutCommandProperty); set => SetValue(ShortcutCommandProperty, value); }


        public static readonly DependencyProperty KeyStateProperty = DependencyProperty.Register(
            nameof(KeyState), 
            typeof((bool, bool, bool)), 
            typeof(WindowBehavior), 
            new UIPropertyMetadata((false, false, false)));
        public (bool, bool, bool) KeyState { get => ((bool, bool, bool))GetValue(KeyStateProperty); set => SetValue(KeyStateProperty, value); }

        #endregion


        #region OnSetup/Cleanup

        protected override void OnSetup()
        {
            base.OnSetup();
            this.AssociatedObject.Initialized += AssociatedObject_Initialized;
            this.AssociatedObject.Loaded += AssociatedObject_Loaded;
            this.AssociatedObject.MouseMove += AssociatedObject_MouseMove;
            this.AssociatedObject.PreviewKeyDown += AssociatedObject_KeyDown;
            this.AssociatedObject.PreviewKeyUp += AssociatedObject_PreviewKeyUp;
            //this.AssociatedObject.MouseDown += AssociatedObject_MouseDown;
            //this.AssociatedObject.QueryCursor += AssociatedObject_QueryCursor;
        }

        protected override void OnCleanup()
        {
            this.AssociatedObject.Initialized -= AssociatedObject_Initialized;
            this.AssociatedObject.Loaded -= AssociatedObject_Loaded;
            this.AssociatedObject.MouseMove -= AssociatedObject_MouseMove;
            this.AssociatedObject.PreviewKeyDown -= AssociatedObject_KeyDown;
            this.AssociatedObject.PreviewKeyUp -= AssociatedObject_PreviewKeyUp;
            //this.AssociatedObject.MouseDown -= AssociatedObject_MouseDown;
            //this.AssociatedObject.QueryCursor -= AssociatedObject_QueryCursor;
            win = null;
            foldingMarkLeft = null;
            foldingMarkRight = null;
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

        private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            win = sender as Window;
            //mainDefinition = win.FindName("mainDefinition") as ColumnDefinition;
            foldingMarkLeft = win.FindName("foldingMarkLeft") as TextBlock;
            foldingMarkRight = win.FindName("foldingMarkRight") as TextBlock;
            foldingMarkLeft.Width = 0;
            foldingMarkRight.Width = 0;

            //以下のような探し方も可能
            /*
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
            */
        }

        private void AssociatedObject_MouseMove(object sender, MouseEventArgs e)
        {
            if (win == null) return;
            
            var mousePosition = e.GetPosition(win);
            Point parent = win.PointToScreen(new Point(0.0d, 0.0d));
            Point left = foldingMarkLeft.PointToScreen(new Point(0.0d, 0.0d)); 
            Point right = foldingMarkRight.PointToScreen(new Point(0.0d, 0.0d));

            //mainDefinition?.Offset ?? 0, 
            //(mainDefinition?.Offset ?? 0) + (mainDefinition?.Width.Value ?? 0), 

            FoldingMarkLeftAnimation(foldingMarkLeft, 
                mousePosition.X,
                left.X - parent.X, 
                Offset, FoldingMarkWidth, AnimationTime);
            FoldingMarkRightAnimation(foldingMarkRight, 
                mousePosition.X,
                right.X - parent.X, 
                Offset, FoldingMarkWidth, AnimationTime);
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
                    Console.WriteLine(GetStringKeyState(KeyState) + e.Key.ToString());
                    //ShortcutCommand.Execute(GetStringKeyState(KeyState) + e.Key.ToString());
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

        private void AssociatedObject_QueryCursor(object sender, QueryCursorEventArgs e)
        {
            //if (VisibleFoldingMark)
            //{
            //    //e.Cursor = Cursors.Hand;
            //}
            //else
            //{
            //    //e.Cursor = Cursors.Cross;
            //}
            e.Handled = true;
        }


        #endregion

        GridLength _columnWidthLeft;
        GridLength _columnWidthRight;

        private static void LeftRegionShow(WindowBehavior obj)
        {
            var child = obj.win.FindName("leftDefinition") as ColumnDefinition;
            if (obj.VisibleLeftRegion)
            {
                obj._columnWidthLeft = child.Width;
                child.Width = new GridLength(0);
            }
            else
            {
                child.Width = obj._columnWidthLeft;
            }
        }

        private static void RightRegionShow(WindowBehavior obj)
        {
            var child = obj.win.FindName("rightDefinition") as ColumnDefinition;
            if (obj.VisibleRightRegion)
            {
                obj._columnWidthRight = child.Width;
                child.Width = new GridLength(0);
            }
            else
            {
                child.Width = obj._columnWidthRight;
            }
        }


        #region Animation

        private static void FoldingMarkLeftAnimation(TextBlock target, double mouse, double splitterLeft, double offset, int width, int time)
        {
            if (mouse < splitterLeft && target.Width == 0)
            {
                MarkVisible(target, width, time);
            }
            else if (mouse > splitterLeft + offset && target.Width == width)
            {
                MarkHidden(target, width, time);
            }
        }

        private static void FoldingMarkRightAnimation(TextBlock target, double mouse, double splitterLeft, double offset, int width, int time)
        {
            if (mouse > splitterLeft && target.Width == 0)
            {
                MarkVisible(target, width, time);
            }
            else if (mouse < splitterLeft - offset && target.Width == width)
            {
                MarkHidden(target, width, time);
            }
        }

        private static void MarkVisible(TextBlock target, int width, int time)
        {
            var storyboard = new Storyboard();
            var animation = new DoubleAnimation
            {
                From = 0,
                To = width,
                Duration = TimeSpan.FromMilliseconds(time)
            };
            Storyboard.SetTargetProperty(animation, new PropertyPath("Width"));
            storyboard.Children.Add(animation);

            target.BeginStoryboard(storyboard);

            //foldingMark.Visibility = Visibility.Visible;
            //VisibleFoldingMark = true;
        }

        private static void MarkHidden(TextBlock target, int width, int time)
        {
            var storyboard = new Storyboard();
            var animation = new DoubleAnimation
            {
                From = width,
                To = 0,
                Duration = TimeSpan.FromMilliseconds(time) //0.1sec
            };
            Storyboard.SetTargetProperty(animation, new PropertyPath("Width"));
            storyboard.Children.Add(animation);

            target.BeginStoryboard(storyboard);

            //var animation2 = new StringAnimationUsingKeyFrames()
            //{
            //    Duration = TimeSpan.FromMilliseconds(100) //0.1sec
            //};
            //animation2.KeyFrames.Add(new DiscreteStringKeyFrame()
            //{
            //    Value = "Visible",
            //    KeyTime = TimeSpan.FromMilliseconds(0)
            //});
            //animation2.KeyFrames.Add(new DiscreteStringKeyFrame()
            //{
            //    Value = "Collapsed",
            //    KeyTime = TimeSpan.FromMilliseconds(100)
            //});
            //Storyboard.SetTargetProperty(animation2, new PropertyPath("Visibility"));
            //storyboard.Children.Add(animation2);


            //VisibleFoldingMark = false;
        }

        #endregion

    }

    public class ShortcutBehavior : BehaviorBase<Window>
    {

        #region Property

        public static readonly DependencyProperty ShortcutCommandProperty = DependencyProperty.Register(
            nameof(ShortcutCommand),
            typeof(ICommand),
            typeof(ShortcutBehavior),
            new PropertyMetadata(null));
        public ICommand ShortcutCommand { get => (ICommand)GetValue(ShortcutCommandProperty); set => SetValue(ShortcutCommandProperty, value); }

        public static readonly DependencyProperty KeyStateProperty = DependencyProperty.Register(
            nameof(KeyState),
            typeof((bool, bool, bool)),
            typeof(ShortcutBehavior),
            new UIPropertyMetadata((false, false, false)));
        public (bool, bool, bool) KeyState { get => ((bool, bool, bool))GetValue(KeyStateProperty); set => SetValue(KeyStateProperty, value); }

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
