# WPF / xmal

## WPF子要素の検索

```cs

//x:Nameの検索

var w = root.FindName("hoge").Width
var w = hoge.Width

//Templete下のx:Nameの検索
root.Template.FindName("content1", root) as ContentControl;
root.GetTemplateChild("svg1") as Rectangle;

//
var canvas = VisualTreeHelper.GetChild(ctrl.Template.FindName("root", ctrl) as DependencyObject, 0);
```

## リソースの登録

ビルドアクション : Resource

```cs
  var bitmap = new BitmapImage(new Uri("pack://application:,,,/logo.png", UriKind.Absolute));
  //var bitmap = new BitmapImage(new Uri("logo.png", UriKind.Relative));
  img = new WriteableBitmap(bitmap);
```

ビルドアクション : 埋め込みリソース

```cs
<Image Source="PrismAutofac;component/img/Lock.svg">
```
```cs
//sharpvectorsを使用したsvg->bitmapimgの例

var myAssembly = System.Reflection.Assembly.GetExecutingAssembly();

using (var memStream = new MemoryStream())
using (var sr = myAssembly.GetManifestResourceStream($"PrismAutofac.img.{kind}.svg"))
{
  var settings = new WpfDrawingSettings()
  {
    IncludeRuntime = true,
    TextAsGeometry = false
  };
  var converter = new StreamSvgConverter(settings);
  if (converter.Convert(sr, memStream))
  {
    BitmapImage bitmap = new BitmapImage();
    bitmap.BeginInit();
    bitmap.CacheOption = BitmapCacheOption.OnLoad;
    bitmap.StreamSource = memStream;
    bitmap.EndInit();

    target.ImageSource = bitmap;
   }
}
```

App.xmal
```cs
<Application x:Class="hogehoge">
    <Application.Resources>
        <ResourceDictionary>
            <Grid x:Key="hoge" />
        </ResourceDictionary>
    </Application.Resources>
</Application>
```
```cs
<ContentControl Content="{StaticResource logo}" />
```
```cs
contentControl.Content = this.FindResource("hoge");
contentControl.Content = this.Resources["hoge"];

//※staticなのでContentControlにそのまま入れるとSingletonな動きする
//別インスタンスにしたいなら下記のような感じでも使える

grid.Width = this.Resources["hoge"].Width;

//もしくはApp.xml内で

<hogehoge x:Shared="False"/>
```
