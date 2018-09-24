using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;


namespace PrismAutofac.Controls
{
    /// <summary>
    /// OutLineText.xaml の相互作用ロジック
    /// </summary>
    public partial class OutLineText : UserControl
    {
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            nameof(Text),
            typeof(string),
            typeof(OutLineText),
            new PropertyMetadata(null, (d,e)=> (d as OutLineText)?.TextChangedCallback())
        );


        public double StrokeThickness
        {
            get => (double)GetValue(StrokeThicknessProperty);
            set => SetValue(StrokeThicknessProperty, value);
        }
        public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register(
            nameof(StrokeThickness),
            typeof(double),
            typeof(OutLineText),
            new PropertyMetadata(2.0, (d, e) => (d as OutLineText)?.TextChangedCallback())
        );

        public Brush StrokeColor
        {
            get => (Brush)GetValue(StrokeColorProperty);
            set => SetValue(StrokeColorProperty, value);
        }
        public static readonly DependencyProperty StrokeColorProperty = DependencyProperty.Register(
            nameof(StrokeColor),
            typeof(Brush),
            typeof(OutLineText),
            new PropertyMetadata(Brushes.Black, (d, e) => (d as OutLineText)?.TextChangedCallback())
        );

        public Brush Color
        {
            get => (Brush)GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(
            nameof(Color),
            typeof(Brush),
            typeof(OutLineText),
            new PropertyMetadata(Brushes.White, (d, e) => (d as OutLineText)?.TextChangedCallback())
        );

        public double BlurRadius
        {
            get => (double)GetValue(BlurRadiusProperty);
            set => SetValue(BlurRadiusProperty, value);
        }
        public static readonly DependencyProperty BlurRadiusProperty = DependencyProperty.Register(
            nameof(BlurRadius),
            typeof(double),
            typeof(OutLineText),
            new PropertyMetadata(0.0, (d, e) => (d as OutLineText)?.TextChangedCallback())
        );

        public void TextChangedCallback()
        {
           // if (Text == null) return;

            var typeface = new Typeface(
                    FontFamily,
                    FontStyle,
                    FontWeight,
                    FontStretch
                );

            FormattedText formattedText = new FormattedText(
                Text,
                CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                typeface,
                FontSize,
                Brushes.Black,
                96
                );

            Geometry textGeometry = formattedText.BuildGeometry(new System.Windows.Point(0, 0));

            //RadialGradientBrush brush = new RadialGradientBrush(
            //    Colors.Black,
            //    Colors.Transparent
            //);

            path2.Data = textGeometry;
            path2.Stroke = StrokeColor;
            path2.StrokeThickness = StrokeThickness;

            if(BlurRadius > 0)
            {
                path2.Effect = new BlurEffect() { Radius = BlurRadius };
            }
            else
            {
                path2.Effect = null;
            }

            path1.Data = textGeometry;
            path1.Fill = Color;
            path1.StrokeThickness = 0;

        }

        public OutLineText()
        {
            InitializeComponent();
        }

        static OutLineText()
        {
            //FontSizeプロパティ
            OutLineText.FontFamilyProperty.OverrideMetadata(typeof(OutLineText), new FrameworkPropertyMetadata(OnFontSizeChanged));
            OutLineText.FontStyleProperty.OverrideMetadata(typeof(OutLineText), new FrameworkPropertyMetadata(OnFontSizeChanged));
            OutLineText.FontWeightProperty.OverrideMetadata(typeof(OutLineText), new FrameworkPropertyMetadata(OnFontSizeChanged));
            OutLineText.FontStretchProperty.OverrideMetadata(typeof(OutLineText), new FrameworkPropertyMetadata(OnFontSizeChanged));
            OutLineText.FontSizeProperty.OverrideMetadata(typeof(OutLineText), new FrameworkPropertyMetadata(OnFontSizeChanged));
        }
        static void OnFontSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as OutLineText;
            if (ctrl != null)
            {
                //ctrl.FontSize = (double)e.NewValue;
                //Console.WriteLine($"{(double)e.NewValue} ");
                ctrl.TextChangedCallback();
            }
        }
    }

}
