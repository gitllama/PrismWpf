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
