using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Drawing;

namespace BTCV.Behavior
{

    public class StatusBehavior : BehaviorBase<StackPanel>
    {
        StackPanel root;

        public string Document
        {
            get => (string)GetValue(WidthProperty);
            set => SetValue(WidthProperty, value);
        }
        public static readonly DependencyProperty WidthProperty = DependencyProperty.Register
        (
            nameof(Document),
            typeof(string),
            typeof(StatusBehavior),
            new UIPropertyMetadata("Test", DocumentPropertyChanged)
        );

        private static void DocumentPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as StatusBehavior;
            ctrl?.ReDraw();
        }

        protected override void OnSetup()
        {
            base.OnSetup();
            this.AssociatedObject.Loaded += AssociatedObject_Loaded;
            //this.AssociatedObject.SizeChanged += AssociatedObject_LayoutUpdated;
            //this.AssociatedObject.LayoutUpdated += AssociatedObject_LayoutUpdated;
        }
        protected override void OnCleanup()
        {
            this.AssociatedObject.Loaded -= AssociatedObject_Loaded;
            //this.AssociatedObject.SizeChanged -= AssociatedObject_LayoutUpdated;
            //this.AssociatedObject.LayoutUpdated -= AssociatedObject_LayoutUpdated;
            base.OnCleanup();
        }

        private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            root = sender as StackPanel;
            ReDraw();
        }

        public void ReDraw()
        {
            if (root == null) return;
            root.Children.Clear();

            foreach (var element in Document.Split(new[] { Environment.NewLine }, StringSplitOptions.None))
            {
                root.Children.Add(Create(element.Trim()));
            }
        }

        


        public static UIElement Create(string param)
        {
            //60 54 48 44 40 36 32 28 24 20 18 16
            int Head1 = 48;
            int Head2 = 36;
            int Head3 = 28;
            int Head4 = 20;
            UIElement element = null;

            //var c1 = ((SolidColorBrush)(param.brush ?? Brushes.Red)).Color.ToHSV();
            //var c2 = ((SolidColorBrush)(param.brush ?? Brushes.Red)).Color.ToHSV();
            var c1 = ((SolidColorBrush)(Brushes.Red)).Color.ToHSV();
            var c2 = ((SolidColorBrush)(Brushes.Red)).Color.ToHSV();
            c2.V /= 2;

            int size = 16;
            if (param.StartsWith("# ")) size = Head1;
            if (param.StartsWith("## ")) size = Head2;
            if (param.StartsWith("### ")) size = Head3;
            if (param.StartsWith("#### ")) size = Head4;

            element = new OutlineText()
            {
                FontSize = size,
                StrokeThickness = size / 28,
                Fill = new SolidColorBrush(c1.ToRGB()),
                Stroke = new SolidColorBrush(c2.ToRGB()),
                Text = param,
                Height = PtToPixel(size) + 8
            };

            //GirdやCanvasで一つづつ描画する場合
            //lineIndex += PtToPixel(size);
            return element;
        }

        static int PtToPixel(int pt) => (int)((pt) * 96 / 72);
    }







}





