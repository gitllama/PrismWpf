using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace PrismAutofac.Views
{
    /// <summary>
    /// Interaction logic for ContentUserControl
    /// </summary>
    public partial class ContentUserControl : UserControl
    {
        public ContentUserControl()
        {
            InitializeComponent();
        }
    }

    [ValueConversion(typeof(double), typeof(string))]
    public class SliderAngleConverter : IValueConverter
    {
        const double FACTOR = 360.0 / 10.0;

        public object Convert(object value, System.Type targetType,
          object parameter, System.Globalization.CultureInfo culture)
        {
            double v = (double)value;
            return v * FACTOR;
        }

        public object ConvertBack(object value, System.Type targetType,
          object parameter, System.Globalization.CultureInfo culture)
        {
            string s = (string)value;
            double v;
            if (!double.TryParse(s, out v))
                return 0;
            return v / FACTOR;
        }
    }

}
