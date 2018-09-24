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
    /// ScalingCanvas.xaml の相互作用ロジック
    /// </summary>
    public partial class ScalingCanvas : UserControl
    {

        public WriteableBitmap Image
        {
            get => (WriteableBitmap)GetValue(ImageProperty);
            set => SetValue(ImageProperty, value);
        }
        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register
        (
            nameof(Image),
            typeof(WriteableBitmap),
            typeof(ScalingCanvas),
            new UIPropertyMetadata(null, (d, e) => (d as ScalingCanvas)?.CanvasPropertyChanged())
        );

        public double Scale
        {
            get => (double)GetValue(ScaleProperty);
            set => SetValue(ScaleProperty, value);
        }
        public static readonly DependencyProperty ScaleProperty = DependencyProperty.Register
        (
            nameof(Scale),
            typeof(double),
            typeof(ScalingCanvas),
            new UIPropertyMetadata(1.0, (d,e)=> (d as ScalingCanvas)?.CanvasPropertyChanged())
        );

        public bool AutoScale
        {
            get => (bool)GetValue(AutoScaleProperty);
            set => SetValue(AutoScaleProperty, value);
        }
        public static readonly DependencyProperty AutoScaleProperty = DependencyProperty.Register
        (
            nameof(AutoScale),
            typeof(bool),
            typeof(ScalingCanvas),
            new UIPropertyMetadata(false, (d, e) => (d as ScalingCanvas)?.CanvasPropertyChanged())
        );

        public void CanvasPropertyChanged()
        {
            if (AutoScale)
            {
                scrollviewer.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
                scrollviewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
                viewbox.Stretch = Stretch.Uniform;
                canvas.Width = 1 * Image?.Width ?? 1;
                canvas.Height = 1 * Image?.Height ?? 1;
                image.ImageSource = Image;
            }
            else
            {
                scrollviewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                scrollviewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
                viewbox.Stretch = Stretch.None;
                canvas.Width = Scale * Image?.Width ?? 1;
                canvas.Height = Scale * Image?.Height ?? 1;
                image.ImageSource = Image;
            }


        }

        public ScalingCanvas()
        {
            InitializeComponent();
        }
    }
}
