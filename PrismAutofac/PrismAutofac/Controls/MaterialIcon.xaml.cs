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

namespace PrismAutofac.Controls
{
    /// <summary>
    /// MaterialIcon.xaml の相互作用ロジック
    /// </summary>
    public partial class MaterialIcon : UserControl
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
            typeof(MaterialIcon),
            new PropertyMetadata("Run", (d,e)=>(d as MaterialIcon)?.Update())
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
            typeof(MaterialIcon),
            new PropertyMetadata(30, (d, e) => (d as MaterialIcon)?.Update())
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
            typeof(MaterialIcon),
            new PropertyMetadata(null, (d, e) => (d as MaterialIcon)?.Update())
        );

        public double Radius
        {
            get => (double)GetValue(RadiusProperty);
            set => SetValue(RadiusProperty, value);
        }
        public static readonly DependencyProperty RadiusProperty = DependencyProperty.Register
        (
            "Radius",
            typeof(double),
            typeof(MaterialIcon),
            new PropertyMetadata(0.0, (d, e) => (d as MaterialIcon)?.Update())
        );

        private void Update()
        {
            this.UpdateLayout();
        }

        public MaterialIcon()
        {
            InitializeComponent();
        }
    }
}

