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

    public class GridSplitterBehavior : BehaviorBase<UserControl>
    {

        UserControl root;
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
            if (obj.root == null) return;
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
            if (obj.root == null) return;
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
            root = null;
            foldingMarkLeft = null;
            foldingMarkRight = null;
            base.OnCleanup();
        }

        #endregion


        #region Event

        private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            root = sender as UserControl;
            //mainDefinition = win.FindName("mainDefinition") as ColumnDefinition;
            foldingMarkLeft = root.FindName("foldingMarkLeft") as TextBlock;
            foldingMarkRight = root.FindName("foldingMarkRight") as TextBlock;
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
            if (root == null) return;

            var mousePosition = e.GetPosition(root);
            Point parent = root.PointToScreen(new Point(0.0d, 0.0d));

            var i1 = root.FindName("leftDefinition") as ColumnDefinition;
            var i2 = root.FindName("mainDefinition") as ColumnDefinition;
            var i3 = root.FindName("rightDefinition") as ColumnDefinition;
            var w = root.FindName("splitterLeft") as GridSplitter;
            //Point left = foldingMarkLeft.PointToScreen(new Point(0.0d, 0.0d));
            //Point right = foldingMarkRight.PointToScreen(new Point(0.0d, 0.0d));

            //mainDefinition?.Offset ?? 0, 
            //(mainDefinition?.Offset ?? 0) + (mainDefinition?.Width.Value ?? 0), 

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
            var child = obj.root.FindName("leftDefinition") as ColumnDefinition;
            var childGrid = obj.root.FindName("gridLeft") as Grid;
            var childSplitter = obj.root.FindName("splitterLeft") as GridSplitter;
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
            var child = obj.root.FindName("rightDefinition") as ColumnDefinition;
            var childGrid = obj.root.FindName("gridRight") as Grid;
            var childSplitter = obj.root.FindName("splitterRight") as GridSplitter;
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



}
