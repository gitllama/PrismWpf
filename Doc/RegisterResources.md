# WPF / xmal

## リソースの登録

リソースを実行ファイルに埋め込む際のあれこれ

### ビルドアクション : 埋め込みリソース

WPF以前からの登録方法

XMALで```Source="PrismAutofac;component/img/hoge.png" />```で読めてたような気もしないでもないけど読めない。

Code
```cs
var myAssembly = System.Reflection.Assembly.GetExecutingAssembly();
/* リソースの一覧が欲しい場合
string[] resnames = myAssembly.GetManifestResourceNames();
foreach (string res in resnames)
{
    Console.WriteLine("resource {0}", res);
}
*/

using (var memStream = new MemoryStream())
using (var sr = myAssembly.GetManifestResourceStream($"PrismAutofac.img.hoge.svg"))
{
  //PrismAutofac : アンセンブリ名

  //SharpVectorsを使用したsvg2BitmapImage
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

codebehindでコンバータを作成して以下のように参照する手もあり

```cs
<Image Source="{local:ImageResource PrismAutofac.img.hoge.hoge}" />
```

```cs
[ContentProperty("Source")]
public class ImageResourceExtension : IMarkupExtension
{
  public string Source { get; set; }
  public object ProvideValue(IServiceProvider serviceProvider)
  {
    if (Source == null) return null;
    return ImageSource.FromResource(Source);

    //FromResourceはXamarin.Formなので、WPFだと上記のように
    //GetManifestResourceStreamで呼び出す
  }
}
```

どちらにせよ、あまりメリットなさそうなので使う理由がない

### ビルドアクション : Resource

WPFからの登録方法。

XMAL
```cs
<Image x:Name="image" Source="/img/hoge.png" />
```

Code
```cs
var bitmap = new System.Windows.Media.Imaging.BitmapImage(new System.Uri("pack://application:,,,/img/hoge.png", System.UriKind.Absolute));
//var bitmap = new Uri("/img/hoge.png",　UriKind.Relative);  
image.Source = new System.Windows.Media.Imaging.WriteableBitmap(bitmap);
```

### App.xmlへの登録

svgなどは、XAMLでCanvasに一度書いた方がメリットが大きい（Fill書き換えなど画像操作を考えると）

その際、App.xmlでリソース登録する方法があげられる

App.xmal
```cs
<Application x:Class="hogehoge">
    <Application.Resources>
        <ResourceDictionary>

            <Grid x:Key="hoge" x:Shared="False"
                  BackGround={Binding Path=Foreground,  RelativeSource={RelativeSource AncestorType={x:Type ContentControl}} }/>

        </ResourceDictionary>
    </Application.Resources>
</Application>
```

XMAL
```cs
<ContentControl x:Name="hogecontent" Foreground="Black" Content="{StaticResource hoge}" />
```

Code
```cs
hogecontent.Content = this.FindResource("hoge");
//hogecontent.Content = this.Resources["hoge"];
```

staticなのでContentControlにそのまま入れるとSingletonな動きをして複数個所同時表示などできなかったりする。  
明確に別インスタンスとするなら```x:Shared```を忘れないように

もしくは

```cs
grid.Width = this.FindResource("hoge").Width;
```

のように、パラメータだけぶっこぶいて要素に代入してしまうか
