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

namespace BTCV.Controls
{
    /// <summary>
    /// ButtonControl.xaml の相互作用ロジック
    /// </summary>
    public partial class ButtonControl : Button
    {

        public string Kind
        {
            get => (string)GetValue(KindProperty);
            set => SetValue(KindProperty, value);
        }
        public static readonly DependencyProperty KindProperty = DependencyProperty.Register
        (
            "Kind",
            typeof(string),
            typeof(ButtonControl),
            new PropertyMetadata(null)
        );

        public int IconSize
        {
            get => (int)GetValue(IconSizeProperty);
            set => SetValue(IconSizeProperty, value);
        }
        public static readonly DependencyProperty IconSizeProperty = DependencyProperty.Register
        (
            "IconSize",
            typeof(int),
            typeof(ButtonControl),
            new PropertyMetadata(30)
        );

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register
        (
            "Text",
            typeof(string),
            typeof(ButtonControl),
            new PropertyMetadata(null)
        );

        public ButtonControl()
        {
            InitializeComponent();
        }
    }
}
