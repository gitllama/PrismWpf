# 

##

## コードビハインド

### Bindingを強制的に評価

```C#
  private void Button_Click(object sender, RoutedEventArgs e)
  {
    var expression = this.e.GetBindingExpression(TextBox.TextProperty);
    expression.UpdateTarget();
  }
```

## UnityContainer

###

```C#
var container = new UnityContainer();

// 依存関係の登録
container.RegisterType<ITextSpeechService, TextSpeechService>(
    null,                                                       // 同一型で区別される登録名称（省略可能）
    new ContainerControlledLifetimeManager(),                   // インスタンスの生存管理方法（省略可能）
    new InjectionConstructor(typeof(ILogger)),                  // 注入する依存関係設定（省略可能）
);

// インスタンスの手動解決
var service = container.ResolveType<ITextSpeechService>();

//ContainerControlledLifetimeManagerはシングルトン管理なのでTrue
var i = (p == c.Resolve<ITextSpeechService>());
```

- ContainerControlledLifetimeManager : シングルトン。LifetimeManagerが破棄されるときにインスタンスも破棄
- ExternallyControlledLifetimeManager : インスタンスの管理に WeakReference を使用しています。
- PerThreadLifetimeManager : スレッド内で１つのインスタンスを使い回します。
- TransientLifetimeManager : 毎回新しいインスタンスを生成します。
