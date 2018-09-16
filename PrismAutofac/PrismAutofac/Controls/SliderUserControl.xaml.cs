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


namespace PrismAutofac.Behavior
{
    /// <summary>
    /// SliderUserControl.xaml の相互作用ロジック
    /// </summary>
    public partial class SliderUserControl : UserControl
    {
        //public static readonly DependencyProperty TitleNameProperty = DependencyProperty.Register("TitleName",
        //    typeof(string),
        //    typeof(SliderUserControl),
        //    new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits)
        //);
        //public string TitleName { get => (string)GetValue(TitleNameProperty); set => SetValue(TitleNameProperty, value); }


        //public static readonly DependencyProperty dataProperty = DependencyProperty.Register(
        //    "data",
        //    typeof(int[]),
        //    typeof(HistogramUserControl),
        //    new PropertyMetadata(null, dataPropertyChanged));
        //public int[] data { get => (int[])GetValue(dataProperty); set => SetValue(dataProperty, value); }

        //private static void dataPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    var obj = d as HistogramUserControl;
        //    if (obj?.data == null) return;

        //    var w = obj.ActualWidth / obj.data.Length;
        //    var StackPanel = obj.FindName("target") as StackPanel;
        //    StackPanel.Children.Clear();
        //    foreach (var i in obj.data)
        //    {
        //        var child = new Rectangle()
        //        {
        //            Width = w,
        //            VerticalAlignment = VerticalAlignment.Bottom,
        //            Fill = Brushes.Aqua,
        //            Height = i
        //        };
        //        StackPanel.Children.Add(child);
        //    }
        //}

        public SliderUserControl()
        {
            InitializeComponent();
            (this.Content as FrameworkElement).DataContext = this;

            //Dispatcher.BeginInvoke(new Action(() =>
            //{
            //    w = ActualWidth / 32;
            //}),
            //DispatcherPriority.Loaded);

            //SizeChanged += (sender, args) =>
            //{
            //    //dataPropertyChanged(this, new DependencyPropertyChangedEventArgs());
            //};
        }
    }
}

    //<Grid>
    //    <StackPanel x:Name="target" Orientation="Horizontal" />
    //</Grid>


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