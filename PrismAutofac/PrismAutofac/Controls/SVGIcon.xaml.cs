using System;
using System.Collections.Generic;
using System.IO;
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
using System.Xaml;
using System.Windows.Markup;
using System.Xml;
using System.Windows.Controls.Primitives;


namespace PrismAutofac.Controls
{
    /// <summary>
    /// UserControl1.xaml の相互作用ロジック
    /// </summary>
    public partial class SVGIcon : ToggleButton
    {

        #region MyRegion

        #endregion

        //public bool Flag
        //{
        //    get => (bool)GetValue(FlagProperty);
        //    set => SetValue(FlagProperty, value);
        //}
        //public static readonly DependencyProperty FlagProperty = DependencyProperty.Register(
        //    nameof(Flag),
        //    typeof(bool),
        //    typeof(SVGIcon),
        //    new PropertyMetadata(null, (d, e) => (d as SVGIcon)?.Update())
        //);


        public string Kind1
        {
            get => (string)GetValue(Source1Property);
            set => SetValue(Source1Property, value);
        }
        public static readonly DependencyProperty Source1Property = DependencyProperty.Register(
            nameof(Kind1),
            typeof(string),
            typeof(SVGIcon),
            new PropertyMetadata(null, (d,e)=>(d as SVGIcon)?.Update())
        );

        public string Kind2
        {
            get => (string)GetValue(Source2Property);
            set => SetValue(Source2Property, value);
        }
        public static readonly DependencyProperty Source2Property = DependencyProperty.Register(
            nameof(Kind2),
            typeof(string),
            typeof(SVGIcon),
            new PropertyMetadata(null, (d, e) => (d as SVGIcon)?.Update())
        );

        public string Kind3
        {
            get => (string)GetValue(Source3Property);
            set => SetValue(Source3Property, value);
        }
        public static readonly DependencyProperty Source3Property = DependencyProperty.Register(
            nameof(Kind3),
            typeof(string),
            typeof(SVGIcon),
            new PropertyMetadata(null, (d, e) => (d as SVGIcon)?.Update())
        );

        public Brush Color1
        {
            get => (Brush)GetValue(Color1Property);
            set => SetValue(Color1Property, value);
        }
        public static readonly DependencyProperty Color1Property = DependencyProperty.Register(
            nameof(Color1),
            typeof(Brush),
            typeof(SVGIcon),
            new PropertyMetadata(Brushes.White, (d, e) => (d as SVGIcon)?.Update())
        );

        public Brush Color2
        {
            get => (Brush)GetValue(Color2Property);
            set => SetValue(Color2Property, value);
        }
        public static readonly DependencyProperty Color2Property = DependencyProperty.Register(
            nameof(Color2),
            typeof(Brush),
            typeof(SVGIcon),
            new PropertyMetadata(Brushes.Blue, (d, e) => (d as SVGIcon)?.Update())
        );

        public Brush Color3
        {
            get => (Brush)GetValue(Color3Property);
            set => SetValue(Color3Property, value);
        }
        public static readonly DependencyProperty Color3Property = DependencyProperty.Register(
            nameof(Color3),
            typeof(Brush),
            typeof(SVGIcon),
            new PropertyMetadata(Brushes.Red, (d, e) => (d as SVGIcon)?.Update())
        );


        public double BlurRadius
        {
            get => (double)GetValue(BlurRadiusProperty);
            set => SetValue(BlurRadiusProperty, value);
        }
        public static readonly DependencyProperty BlurRadiusProperty = DependencyProperty.Register(
            nameof(BlurRadius),
            typeof(double),
            typeof(SVGIcon),
            new PropertyMetadata(0.0, (d, e) => (d as SVGIcon)?.Update())
        );

        public SVGIcon()
        {
            InitializeComponent();
            this.MouseMove += SVGIcon_MouseMove;
            this.MouseEnter += SVGIcon_MouseMove;
            this.MouseLeave += SVGIcon_MouseMove;
            this.MouseDown += SVGIcon_MouseMove;
        }

        static SVGIcon()
        {
            SVGIcon.IsCheckedProperty.OverrideMetadata(
                typeof(SVGIcon), 
                new FrameworkPropertyMetadata((d,s)=>(d as SVGIcon)?.Update()));

        }

        private void Update()
        {
            var svg1 = this.GetTemplateChild("svg1") as Rectangle;
            if (IsChecked ?? false)
            {

            }
            else
            {
                if (IsMouseOver)
                {

                }
                else
                {

                }
            }

            //var i = this.Resources["a"];


            //if (svg1 != null && svg2 != null)
            //{
            //    KindToImageSource(Kind1, svg1);
            //    KindToImageSource(Kind2, svg2);
            //}


        }




        private void SVGIcon_MouseMove(object sender, MouseEventArgs e)
        {
            Update();
        }

        //public Rectangle svg1;
        //public Rectangle svg2;
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            // var canvas = VisualTreeHelper.GetChild(ctrl.Template.FindName("root", ctrl) as DependencyObject, 0);
            //svg1 = this.GetTemplateChild("svg1") as Rectangle;
            //svg2 = this.GetTemplateChild("svg2") as Rectangle;
            Update();

        }


    }
}


/*
    <UserControl.Template>
        <ControlTemplate TargetType="UserControl">
            <Grid x:Name="rootGrid" DataContext="{Binding ElementName=root}"
                  VerticalAlignment="Center" HorizontalAlignment="Center" >
                <Rectangle Fill="WhiteSmoke" 
                           VerticalAlignment="Center" HorizontalAlignment="Center" 
                           x:Name="svg1" >
                    <Rectangle.OpacityMask>
                        <ImageBrush/>
                    </Rectangle.OpacityMask>
                </Rectangle>
                <Rectangle Fill="Blue" 
                           VerticalAlignment="Center" HorizontalAlignment="Center" 
                           x:Name="svg2">
                    <Rectangle.OpacityMask>
                        <ImageBrush/>
                    </Rectangle.OpacityMask>
                </Rectangle>
            </Grid>
            
            <ControlTemplate.Triggers>
                <Trigger Property="IsMouseOver" Value="False">
                    <Setter TargetName="svg1" Property="Opacity" Value="0.6"/>
                    <Setter TargetName="svg2" Property="Opacity" Value="0.0"/>
                </Trigger>
                <Trigger Property="IsMouseOver" Value="True">
                    <Setter TargetName="svg1" Property="Opacity" Value="0.0"/>
                    <Setter TargetName="svg2" Property="Opacity" Value="0.6"/>
                </Trigger>

            </ControlTemplate.Triggers>
        
        </ControlTemplate>
    </UserControl.Template> 
*/

//if (kind == null) return;

//var myAssembly = System.Reflection.Assembly.GetExecutingAssembly();
//using (var memStream = new MemoryStream())
//using (var sr = myAssembly.GetManifestResourceStream($"PrismAutofac.img.{kind}.svg"))
//{
//    var settings = new WpfDrawingSettings()
//    {
//        IncludeRuntime = false,
//        TextAsGeometry = false
//    };
//    var converter = new StreamSvgConverter(settings);
//    if (converter.Convert(sr, memStream))
//    {
//        BitmapImage bitmap = new BitmapImage();
//        bitmap.BeginInit();
//        bitmap.CacheOption = BitmapCacheOption.OnLoad;
//        bitmap.StreamSource = memStream;
//        bitmap.EndInit();



//        var img = this.GetTemplateChild("img") as ImageBrush;

//        img.ImageSource = bitmap;
//        //target.OpacityMask = new ImageBrush()
//        //{
//        //    ImageSource = bitmap,
//        //    Stretch = Stretch.Fill,
//        //    TileMode = TileMode.Tile
//        //};
//    }
//}

//if (kind == null) return;

//var myAssembly = System.Reflection.Assembly.GetExecutingAssembly();
//using (var memStream = new MemoryStream())
//using (var sr = myAssembly.GetManifestResourceStream($"PrismAutofac.img.{kind}.svg"))
//{
//    var settings = new WpfDrawingSettings()
//    {
//        IncludeRuntime = true,
//        TextAsGeometry = false
//    };
//    //SvgViewbox svg = new SvgViewbox();
//    //svg.Source = new Uri("test.svg", UriKind.Relative);
//    //svg.
//}