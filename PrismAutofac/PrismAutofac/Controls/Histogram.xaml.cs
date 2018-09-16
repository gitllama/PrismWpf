using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace PrismAutofac.Controls
{
    /// <summary>
    /// HistogramUserControl.xaml の相互作用ロジック
    /// </summary>
    public partial class Histogram : UserControl
    {

        public static readonly DependencyProperty DataProperty = DependencyProperty.Register(
            nameof(Data),
            typeof(int[]),
            typeof(Histogram),
            new PropertyMetadata(null, dataPropertyChanged));
        public int[] Data { get => (int[])GetValue(DataProperty); set => SetValue(DataProperty, value); }

        public static readonly DependencyProperty BarColorProperty = DependencyProperty.Register(
            nameof(BarColor),
            typeof(Brush),
            typeof(Histogram),
            new PropertyMetadata(Brushes.DeepSkyBlue, dataPropertyChanged));
        public Brush BarColor { get => (Brush)GetValue(BarColorProperty); set => SetValue(BarColorProperty, value); }

        public static readonly DependencyProperty SelectedColorProperty = DependencyProperty.Register(
            nameof(SelectedColor),
            typeof(Brush),
            typeof(Histogram),
            new PropertyMetadata(Brushes.Red, dataPropertyChanged));
        public Brush SelectedColor { get => (Brush)GetValue(SelectedColorProperty); set => SetValue(SelectedColorProperty, value); }

        public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register(
            nameof(MaxValue),
            typeof(int),
            typeof(Histogram),
            new PropertyMetadata(-1, dataPropertyChanged));
        public int MaxValue { get => (int)GetValue(MaxValueProperty); set => SetValue(MaxValueProperty, value); }

        public static readonly DependencyProperty MinValueProperty = DependencyProperty.Register(
            nameof(MinValue),
            typeof(int),
            typeof(Histogram),
            new PropertyMetadata(-1, dataPropertyChanged));
        public int MinValue { get => (int)GetValue(MinValueProperty); set => SetValue(MinValueProperty, value); }


        private static void dataPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = d as Histogram;
            obj.UpdateState();
        }

        public Histogram()
        {
            InitializeComponent();
            (this.Content as FrameworkElement).DataContext = this;

            Dispatcher.BeginInvoke(new Action(() =>
            {
                UpdateState(ActualWidth / Data.Length, ActualHeight);
            }), DispatcherPriority.Loaded);

            SizeChanged += (sender, args) =>
            {
                UpdateState();
            };
        }

        private void UpdateState(double w = 0, double h = 0)
        {
            w = w <= 0 ? ActualWidth / Data.Length : w;
            h = h <= 0 ? ActualHeight : h;

            var StackPanel = FindName("target") as StackPanel;
            StackPanel.Children.Clear();
            foreach (var (item, index) in Data.Select((item, index) => (item, index)))
            {
                var child = new Rectangle()
                {
                    Width = w,
                    VerticalAlignment = VerticalAlignment.Bottom,
                    Fill = (MaxValue > index && index >= MinValue) ? SelectedColor : BarColor,
                    Height = item > h ? h : item < 0 ? 0 : item
                };
                StackPanel.Children.Add(child);
            }
        }

        //private void UpdateState(bool useTransition)
        //{
        //    if (this.Value >= 0)
        //    {
        //        VisualStateManager.GoToState(this, "Positive", useTransition);
        //    }
        //    else
        //    {
        //        VisualStateManager.GoToState(this, "Negative", useTransition);
        //    }
        //}
    }
}
