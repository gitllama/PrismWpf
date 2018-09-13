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
        public static readonly DependencyProperty TitleNameProperty = DependencyProperty.Register("TitleName",
            typeof(string),
            typeof(SliderUserControl),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits)
        );
        public string TitleName { get => (string)GetValue(TitleNameProperty); set => SetValue(TitleNameProperty, value); }



        public SliderUserControl()
        {
            InitializeComponent();
        }
    }
}
