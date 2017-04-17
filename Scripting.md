# Roslyn Scripting

Verでけっこう違うので注意

## Packge

Microsoft.CodeAnalysis.CSharp.Scripting

## 基本

```C#
CSharpScript.RunAsync("System.Console.WriteLine(\"Hello, World!\");").Wait();

CSharpScript.RunAsync(File.ReadAllText("test.csx")).Wait();
```

AsyncしかないのでConsoleのMainなどの時はWaitするか戻り値を受け取るか

## Script内部

```C#
#r "System.Core"      //アンセンブリのロード
#r "Newtonsoft.Json.dll"
#load "test2.csx"   //スクリプトのロード

using System;       //ちゃんとせんげんしませう
using System.Linq;

Console.WriteLine("Hello, World!");
```
```C#
//test2.csx
using System;
public class Hoge
{
    public void SayHello() 
    {
        Console.WriteLine("Hello World!");
    }
}
```

## スクリプトとの変数の共有

```C#
Main()
{
  var globals = new Globals { X = 1, Y = 2 };
  
  var state = CSharpScript.RunAsync(
    "X+Y",
    ScriptOptions.Default.WithImports("System.Math"),
    globals: globals).Wait();
}
public class Globals
{
  public int X;
  public int Y;
}
```
## 結果の取得

```C#
//処理を実行して戻り値を取得する ;とかreturnいらない
var result = CSharpScript.EvaluateAsync<int>("var i=1; i+1+1").Wait();

//処理を実行してVariablesに放り込み
var state = CSharpScript.RunAsync("var i=1; var j = i+1+1;").Result; 
foreach (var variable in state.Variables)
  Console.WriteLine($"{variable.Name} = {variable.Value} of type {variable.Type}\r\n");
```


## スクリプトとの変数の共有

```C#
Main()
{
  var globals = new Globals { X = 1, Y = 2 };
  
  CSharpScript.RunAsync(
    "1+1;",
    ScriptOptions.Default.WithImports("System.Math"),
    globals: globals);
}
public class Globals
{
  public int X;
  public int Y;
}
```

## オプション

ScriptOptions

```C#
var ssr = ScriptSourceResolver.Default.WithBaseDirectory(Environment.CurrentDirectory);
var smr = ScriptMetadataResolver.Default.WithBaseDirectory(Environment.CurrentDirectory);

var o = ScriptOptions.Default
          .WithSourceResolver(ssr);                         //ファイル探索のカレント設定（しないとフルパスしか通らない
          .WithMetadataResolver(smr)                        //アセンブリ探索のカレント設定（#rで読みたいとき
          .WithFilePath(Path.GetFullPath("myscript1.csx")); //上二つの代用に使える
          .WithReferences(Assembly.GetEntryAssembly())      //アセンブリの登録（本体側のexeなので自作クラスよびだせる
          .WithImports("System.Math");                      //アンセンブリのインポート（using省略

CSharpScript.RunAsync("1+1;", o).Wait();
```
WithSearchPathなんてのもある

## コンパイル結果

で、DLLにして保存できる

```C#
    var script = CSharpScript.Create(File.ReadAllText(@"test.csx"));
    script.RunAsync().Wait();
 
    var comp = script.GetCompilation();
    using (var ms = new MemoryStream())
    {
        comp.Emit(ms);
        File.WriteAllBytes("test.dll", ms.ToArray());
    }
```
