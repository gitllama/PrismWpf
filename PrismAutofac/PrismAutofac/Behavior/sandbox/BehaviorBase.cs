using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows.Shapes;

namespace BTCV.Behavior
{
    public class BehaviorBase<T> : Behavior<T> where T : FrameworkElement
    {

        #region フィールド
        /// <summary>
        /// セットアップ状態
        /// </summary>
        private bool isSetup = false;

        /// <summary>
        /// Hook状態
        /// </summary>
        private bool isHookedUp = false;

        /// <summary>
        /// 対象オブジェクト
        /// </summary>
        private WeakReference weakTarget;
        #endregion  // フィールド

        #region メソッド
        /// <summary>
        /// Changedハンドラ
        /// </summary>
        protected override void OnChanged()
        {
            base.OnChanged();


            //==== 関連オブジェクト有無判定 ====//
            var target = AssociatedObject;
            if (target != null)
            {
                //-==- 有り -==-//

                //==== Hook開始 ====//
                HookupBehavior(target);
            }
            else
            {
                //-==- 無し -==-//

                //==== Hook解除 ====//
                UnHookupBehavior();
            }
        }


        /// <summary>
        /// ビヘイビアをHookする。
        /// </summary>
        /// <param name="target"></param>
        private void HookupBehavior(T target)
        {
            //==== Hook状態判定 ====//
            if (isHookedUp)
            {
                //-==- Hook中 -==-//
                return;
            }


            //==== Hook開始 ====//
            weakTarget = new WeakReference(target);
            isHookedUp = true;
            target.Unloaded += OnTargetUnloaded;
            target.Loaded += OnTargetLoaded;


            //==== ビヘイビアのセットアップ ====//
            SetupBehavior();
        }


        /// <summary>
        /// ビヘイビアをUnhookする。
        /// </summary>
        private void UnHookupBehavior()
        {
            //==== Hook状態判定 ====//
            if (!isHookedUp)
            {
                //-==- 未Hook -==-//
                return;
            }


            //==== Hook解除 ====//
            isHookedUp = false;
            var target = AssociatedObject ?? (T)weakTarget.Target;
            if (target != null)
            {
                target.Unloaded -= OnTargetUnloaded;
                target.Loaded -= OnTargetLoaded;
            }


            //==== ビヘイビアのクリーンアップ ====//
            CleanupBehavior();
        }


        /// <summary>
        /// [関連オブジェクト] Loadedハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTargetLoaded(object sender, RoutedEventArgs e)
        {
            //==== ビヘイビアのセットアップ ====//
            SetupBehavior();
        }


        /// <summary>
        /// [関連オブジェクト] Unloadedハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTargetUnloaded(object sender, RoutedEventArgs e)
        {
            //==== ビヘイビアのクリーンアップ ====//
            CleanupBehavior();
        }


        /// <summary>
        /// セットアップ時の処理を行う。
        /// </summary>
        protected virtual void OnSetup() { }


        /// <summary>
        /// クリーンアップ時の処理を行う。
        /// </summary>
        protected virtual void OnCleanup() { }


        /// <summary>
        /// ビヘイビアのセットアップを行う。
        /// </summary>
        private void SetupBehavior()
        {
            if (isSetup)
            {
                return;
            }

            isSetup = true;
            OnSetup();
        }


        /// <summary>
        /// ビヘイビアのクリーンアップを行う。
        /// </summary>
        private void CleanupBehavior()
        {
            if (!isSetup)
            {
                return;
            }

            isSetup = false;
            OnCleanup();
        }
        #endregion  // メソッド

    }


    #region Shape


    // UIスレッドが必要な為、生成はBehavior
    // Scaleに関してはBehaviorで再計算されるため、
    // 等倍座標の入力
    public enum ShapeType
    {
        Circle,
        Rectangle,
        Text
    }
    public struct ShapeParam
    {
        public ShapeType type;
        public int left;
        public int top;
        public int width;
        public int height;
        public int size;
        public string text;
        public Brush brush;
    }

    public static class ShapesExtentitons
    {
        public static UIElement Create(this ShapeParam param, double scale)
        {
            UIElement element = null;
            switch (param.type)
            {
                case ShapeType.Text:
                    var c1 = ((SolidColorBrush)(param.brush ?? Brushes.Red)).Color.ToHSV();
                    var c2 = ((SolidColorBrush)(param.brush ?? Brushes.Red)).Color.ToHSV();
                    c2.V /= 2;
                    element = new OutlineText()
                    {
                        FontSize = param.size * scale,
                        StrokeThickness = (param.size * scale) / 32,
                        Fill = new SolidColorBrush(c1.ToRGB()),
                        Stroke = new SolidColorBrush(c2.ToRGB()),
                        Text = param.text
                    };
                    Canvas.SetLeft(element, param.left * scale);
                    Canvas.SetTop(element, param.top * scale);
                    return element;
                case ShapeType.Rectangle:
                    element = new Rectangle
                    {
                        Width = param.width * scale,
                        Height = param.height * scale,
                        StrokeThickness = 1.0,
                        Stroke = param.brush ?? Brushes.Red
                    };
                    Canvas.SetLeft(element, param.left * scale);
                    Canvas.SetTop(element, param.top * scale);
                    return element;
                case ShapeType.Circle:
                    element = new Ellipse
                    {
                        Width = param.size * 2.0 * scale,
                        Height = param.size * 2.0 * scale,
                        StrokeThickness = 1.0,
                        Stroke = param.brush ?? Brushes.Red
                    };
                    Canvas.SetLeft(element, (param.left - param.size) * scale);
                    Canvas.SetTop(element, (param.top - param.size) * scale);
                    return element;
            }
            return null;
            //var element = new TextBlock
            //{
            //    FontSize = num3,
            //    Foreground = Brushes.Red,
            //    Text = text
            //};
        }
    }


    //private static void DrawGrid(Canvas ctrl, string[] c)
    //{
    //    int num2;
    //    int num = int.TryParse(c[1], out num2) ? num2 : 0;
    //    if (num < 1)
    //    {
    //        return;
    //    }
    //    int num3 = 0;
    //    while ((double)num3 < this.uWidth)
    //    {
    //        Line element = new Line
    //        {
    //            X1 = (double)num3 * this.uScale,
    //            Y1 = 0.0,
    //            X2 = (double)num3 * this.uScale,
    //            Y2 = this.uHeight * this.uScale,
    //            StrokeThickness = 1.0,
    //            Stroke = Brushes.Red
    //        };
    //        cnv.Children.Add(element);
    //        num3 += num;
    //    }
    //    int num4 = 0;
    //    while ((double)num4 < this.uHeight)
    //    {
    //        Line element2 = new Line
    //        {
    //            X1 = 0.0,
    //            Y1 = (double)num4 * this.uScale,
    //            X2 = this.uWidth * this.uScale,
    //            Y2 = (double)num4 * this.uScale,
    //            StrokeThickness = 1.0,
    //            Stroke = Brushes.Red
    //        };
    //        cnv.Children.Add(element2);
    //        num4 += num;
    //    }
    //}

    [System.Windows.Markup.ContentProperty("Text")]
    internal class OutlineText : FrameworkElement
    {
        private FormattedText FormattedText;
        private Geometry TextGeometry;

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof(string), typeof(OutlineText),
            new FrameworkPropertyMetadata(OnFormattedTextInvalidated));

        public static readonly DependencyProperty TextAlignmentProperty = DependencyProperty.Register(
            "TextAlignment", typeof(TextAlignment), typeof(OutlineText),
            new FrameworkPropertyMetadata(OnFormattedTextUpdated));

        public static readonly DependencyProperty TextDecorationsProperty = DependencyProperty.Register(
            "TextDecorations", typeof(TextDecorationCollection), typeof(OutlineText),
            new FrameworkPropertyMetadata(OnFormattedTextUpdated));

        public static readonly DependencyProperty TextTrimmingProperty = DependencyProperty.Register(
            "TextTrimming", typeof(TextTrimming), typeof(OutlineText),
            new FrameworkPropertyMetadata(OnFormattedTextUpdated));

        public static readonly DependencyProperty TextWrappingProperty = DependencyProperty.Register(
            "TextWrapping", typeof(TextWrapping), typeof(OutlineText),
            new FrameworkPropertyMetadata(TextWrapping.NoWrap, OnFormattedTextUpdated));

        public static readonly DependencyProperty FillProperty = DependencyProperty.Register(
            "Fill", typeof(Brush), typeof(OutlineText),
            new FrameworkPropertyMetadata(Brushes.Red, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register(
            "Stroke", typeof(Brush), typeof(OutlineText),
            new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register(
            "StrokeThickness", typeof(double), typeof(OutlineText),
            new FrameworkPropertyMetadata(1d, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty FontFamilyProperty = System.Windows.Documents.TextElement.FontFamilyProperty.AddOwner(
            typeof(OutlineText), new FrameworkPropertyMetadata(OnFormattedTextUpdated));

        public static readonly DependencyProperty FontSizeProperty = System.Windows.Documents.TextElement.FontSizeProperty.AddOwner(
            typeof(OutlineText), new FrameworkPropertyMetadata(OnFormattedTextUpdated));

        public static readonly DependencyProperty FontStretchProperty = System.Windows.Documents.TextElement.FontStretchProperty.AddOwner(
            typeof(OutlineText), new FrameworkPropertyMetadata(OnFormattedTextUpdated));

        public static readonly DependencyProperty FontStyleProperty = System.Windows.Documents.TextElement.FontStyleProperty.AddOwner(
            typeof(OutlineText), new FrameworkPropertyMetadata(OnFormattedTextUpdated));

        public static readonly DependencyProperty FontWeightProperty = System.Windows.Documents.TextElement.FontWeightProperty.AddOwner(
            typeof(OutlineText), new FrameworkPropertyMetadata(OnFormattedTextUpdated));

        public OutlineText()
        {
            this.TextDecorations = new TextDecorationCollection();
        }

        public Brush Fill
        {
            get { return (Brush)GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }

        public FontFamily FontFamily
        {
            get { return (FontFamily)GetValue(FontFamilyProperty); }
            set { SetValue(FontFamilyProperty, value); }
        }

        [System.ComponentModel.TypeConverter(typeof(FontSizeConverter))]
        public double FontSize
        {
            get { return (double)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }

        public FontStretch FontStretch
        {
            get { return (FontStretch)GetValue(FontStretchProperty); }
            set { SetValue(FontStretchProperty, value); }
        }

        public FontStyle FontStyle
        {
            get { return (FontStyle)GetValue(FontStyleProperty); }
            set { SetValue(FontStyleProperty, value); }
        }

        public FontWeight FontWeight
        {
            get { return (FontWeight)GetValue(FontWeightProperty); }
            set { SetValue(FontWeightProperty, value); }
        }

        public Brush Stroke
        {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }

        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public TextAlignment TextAlignment
        {
            get { return (TextAlignment)GetValue(TextAlignmentProperty); }
            set { SetValue(TextAlignmentProperty, value); }
        }

        public TextDecorationCollection TextDecorations
        {
            get { return (TextDecorationCollection)this.GetValue(TextDecorationsProperty); }
            set { this.SetValue(TextDecorationsProperty, value); }
        }

        public TextTrimming TextTrimming
        {
            get { return (TextTrimming)GetValue(TextTrimmingProperty); }
            set { SetValue(TextTrimmingProperty, value); }
        }

        public TextWrapping TextWrapping
        {
            get { return (TextWrapping)GetValue(TextWrappingProperty); }
            set { SetValue(TextWrappingProperty, value); }
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            this.EnsureGeometry();

            drawingContext.DrawGeometry(this.Fill, new Pen(this.Stroke, this.StrokeThickness), this.TextGeometry);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            this.EnsureFormattedText();

            this.FormattedText.MaxTextWidth = Math.Min(3579139, availableSize.Width);
            this.FormattedText.MaxTextHeight = availableSize.Height;

            return new Size(this.FormattedText.Width, this.FormattedText.Height);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            this.EnsureFormattedText();

            this.FormattedText.MaxTextWidth = finalSize.Width;
            this.FormattedText.MaxTextHeight = finalSize.Height;

            this.TextGeometry = null;

            return finalSize;
        }

        private static void OnFormattedTextInvalidated(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var outlinedTextBlock = (OutlineText)dependencyObject;
            outlinedTextBlock.FormattedText = null;
            outlinedTextBlock.TextGeometry = null;

            outlinedTextBlock.InvalidateMeasure();
            outlinedTextBlock.InvalidateVisual();
        }

        private static void OnFormattedTextUpdated(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var outlinedTextBlock = (OutlineText)dependencyObject;
            outlinedTextBlock.UpdateFormattedText();
            outlinedTextBlock.TextGeometry = null;

            outlinedTextBlock.InvalidateMeasure();
            outlinedTextBlock.InvalidateVisual();
        }

        private void EnsureFormattedText()
        {
            if (this.FormattedText != null || this.Text == null)
                return;

            this.FormattedText = new FormattedText(
                this.Text,
                System.Globalization.CultureInfo.CurrentUICulture,
                this.FlowDirection,
                new Typeface(this.FontFamily, this.FontStyle, this.FontWeight, FontStretches.Normal),
                this.FontSize,
                Brushes.Black);

            this.UpdateFormattedText();
        }

        private void UpdateFormattedText()
        {
            if (this.FormattedText == null)
                return;

            this.FormattedText.MaxLineCount = this.TextWrapping == TextWrapping.NoWrap ? 1 : int.MaxValue;
            this.FormattedText.TextAlignment = this.TextAlignment;
            this.FormattedText.Trimming = this.TextTrimming;

            this.FormattedText.SetFontSize(this.FontSize);
            this.FormattedText.SetFontStyle(this.FontStyle);
            this.FormattedText.SetFontWeight(this.FontWeight);
            this.FormattedText.SetFontFamily(this.FontFamily);
            this.FormattedText.SetFontStretch(this.FontStretch);
            this.FormattedText.SetTextDecorations(this.TextDecorations);
        }

        private void EnsureGeometry()
        {
            if (this.TextGeometry != null)
                return;

            this.EnsureFormattedText();
            this.TextGeometry = this.FormattedText.BuildGeometry(new Point());
        }
    }


    #endregion


    #region MyRegion


    public struct HSVColor
    {
        public float H { get; set; }
        public float S { get; set; }
        public float V { get; set; }

        public static HSVColor FromHSV(float h, float s, float v)
        {
            return new HSVColor() { H = h, S = s, V = v };
        }

        public override string ToString()
        {
            return string.Format("H:{0}, S:{1}, V:{2}", H, S, V);
        }

        public Color ToRGB()
        {
            int Hi = ((int)(H / 60.0)) % 6;
            float f = H / 60.0f - (int)(H / 60.0);
            float p = V * (1 - S);
            float q = V * (1 - f * S);
            float t = V * (1 - (1 - f) * S);

            switch (Hi)
            {
                case 0:
                    return FromRGB(V, t, p);
                case 1:
                    return FromRGB(q, V, p);
                case 2:
                    return FromRGB(p, V, t);
                case 3:
                    return FromRGB(p, q, V);
                case 4:
                    return FromRGB(t, p, V);
                case 5:
                    return FromRGB(V, p, q);
            }

            // ここには来ない
            throw new InvalidOperationException();
        }

        private Color FromRGB(float fr, float fg, float fb)
        {
            fr *= 255;
            fg *= 255;
            fb *= 255;
            byte r = (byte)((fr < 0) ? 0 : (fr > 255) ? 255 : fr);
            byte g = (byte)((fg < 0) ? 0 : (fg > 255) ? 255 : fg);
            byte b = (byte)((fb < 0) ? 0 : (fb > 255) ? 255 : fb);
            return Color.FromRgb(r, g, b);
        }
    }

    public static class ColorExtension
    {
        public static HSVColor ToHSV(this Color c)
        {
            float r = c.R / 255.0f;
            float g = c.G / 255.0f;
            float b = c.B / 255.0f;

            var list = new float[] { r, g, b };
            var max = list.Max();
            var min = list.Min();

            float h, s, v;
            if (max == min)
                h = 0;
            else if (max == r)
                h = (60 * (g - b) / (max - min) + 360) % 360;
            else if (max == g)
                h = 60 * (b - r) / (max - min) + 120;
            else
                h = 60 * (r - g) / (max - min) + 240;

            if (max == 0)
                s = 0;
            else
                s = (max - min) / max;

            v = max;

            return new HSVColor() { H = h, S = s, V = v };
        }
    }


    #endregion
}
