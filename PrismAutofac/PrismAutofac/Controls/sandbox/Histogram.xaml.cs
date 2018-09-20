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

namespace BTCV.Views
{
    /// <summary>
    /// HistogramUserControl.xaml の相互作用ロジック
    /// </summary>
    public partial class Histogram : UserControl
    {
        public int[] Data
        {
            get => (int[])GetValue(dataProperty);
            set => SetValue(dataProperty, value);
        }
        public static readonly DependencyProperty dataProperty = DependencyProperty.Register
        (
            nameof(Data),
            typeof(int[]),
            typeof(Histogram),
            new PropertyMetadata(null, dataPropertyChanged)
        );

        public int Upper
        {
            get => (int)GetValue(UpperProperty);
            set => SetValue(UpperProperty, value);
        }
        public static readonly DependencyProperty UpperProperty = DependencyProperty.Register
        (
            nameof(Upper),
            typeof(int),
            typeof(Histogram),
            new PropertyMetadata(-1, dataPropertyChanged)
        );

        public int Lower
        {
            get => (int)GetValue(LowerProperty);
            set => SetValue(LowerProperty, value);
        }
        public static readonly DependencyProperty LowerProperty = DependencyProperty.Register
        (
            nameof(Lower),
            typeof(int),
            typeof(Histogram),
            new PropertyMetadata(-1, dataPropertyChanged)
        );

        private static void dataPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = d as Histogram;
            if (obj?.Data == null) return;


            //最大値と二番目の数値を取得
            List<(int idx, int val)> a = new List<(int i, int v)>();
            for(int i = 1;i < obj.Data.Length -1 ; i++)
            {
                if(obj.Data[i-1] < obj.Data[i] && obj.Data[i] > obj.Data[i+1])
                    a.Add((i, obj.Data[i]));
            }

            var first = -1;
            var second = -1;
            if (a.Count > 1)
            {
                var buf2 = a.OrderByDescending(x => x.val).Select(x=>x.idx).ToArray();
                first = buf2[0];
                second = buf2[1];
            }
           
            //Barの表示
            var w = obj.ActualWidth / obj.Data.Length;
            var StackPanel = obj.FindName("target") as StackPanel;
            StackPanel.Children.Clear();

            int index = 0;
            foreach (var i in obj.Data)
            {
                var hoge = obj.Upper >= index && index >= obj.Lower ? Brushes.Blue : Brushes.Aqua;
                var child = new Rectangle()
                {
                    Width = w,
                    VerticalAlignment = VerticalAlignment.Bottom,
                    
                    Fill = index == first ? Brushes.Red
                         : index == second ? Brushes.Pink : hoge,
                    Height = i <= 0 ? 1 : i,
                };
                StackPanel.Children.Add(child);
                index++;
            }
        }

        public Histogram()
        {
            InitializeComponent();
            (this.Content as FrameworkElement).DataContext = this;

            //Dispatcher.BeginInvoke(new Action(() =>
            //{
            //    w = ActualWidth / 32;
            //}),
            //DispatcherPriority.Loaded);

            SizeChanged += (sender, args) =>
            {
                dataPropertyChanged(this, new DependencyPropertyChangedEventArgs());
            };
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
