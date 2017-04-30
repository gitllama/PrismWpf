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

### （名前付きで）登録して取得するだけ

```C#
  var container = new UnityContainer();  
  container.RegisterInstance<IMasterManager>("Customer", new CustomerManager());  

  IMasterManager manager = container.Resolve<IMasterManager>("Customer");
  
  //全部呼び出し
  IEnumerable<IMasterManager> managers = container.ResolveAll<IMasterManager>();
  foreach (IMasterManager manager in managers)
  {
    Console.WriteLine(manager.Read().DataSetName);  
  }
```

### lifetime付きで登録

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

- ContainerControlledLifetimeManager : 1 containerに1 instance。IDisposable実装してりLifetimeManager破棄でインスタンス破棄
- ExternallyControlledLifetimeManager : 呼出場所のスコープ範囲内。GCに回収されない限り同一。WeakReference使用。
- PerThreadLifetimeManager : Thread毎のSingleton
- TransientLifetimeManager : 単なるFactoryMethod。DIコンテナに要求する度にinstance生成。newと同じ。

### 依存性注入

```C#
public interface IAnimal  
{
  string Cry();  
}  
public class Cat : IAnimal  
{  
  public string Cry() => "ニャ～";
} 
public class Dog : IAnimal  
{  
  public string Cry() => "バウ！";
} 

public class Person  
{  
    [Dependency("Dog")] //Dependency属性で注入する依存を決定
    public IAnimal Pet { get;set; } 
    public string CallPet() => Pet.Cry();
}

static void Main(string[] args)  
{
  UnityContainer container = new UnityContainer();  
  container.RegisterInstance<IAnimal>("Dog", new Dog());  
  container.RegisterInstance<IAnimal>("Cat", new Cat());  
  //Container.RegisterType<IAnimal, Dog>("Dog");  
  //Container.RegisterType<IAnimal, Cat>("Cat");
        
  Person person = new Person();  
  person = container.BuildUp<Person>(person); //依存性の注入
}  
```

### 依存性注入

```C#
public class Person  
{  
  [Dependency]  
  public IAnimal Pet { get; set; }  
}
static void Main(string[] args)  
{
  UnityContainer container = new UnityContainer();  
  UnityConfigurationSection section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");  
  
  // 構成ファイルの内容を適用する  
  section.Containers["Sample"].Configure(container);
  
  Person person = new Person();
  person = container.BuildUp<Person>(person);  
}
```
```xml
<?xml version="1.0" encoding="utf-8" ?>  
<configuration>  
    <configSections>  
        <section name="unity" type="Microsoft.Practices.Unity.Configuration.UnityConfigurationSection, Microsoft.Practices.Unity.Configuration, Version=1.1.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"/>  
    </configSections>  
    <unity>  
        <!--型の別名を定義-->  
        <typeAliases>  
            <typeAlias alias="IAnimal" type="BuildUpSample.IAnimal, BuildUpSample"/>  
            <typeAlias alias="Cat" type="BuildUpSample.Cat, BuildUpSample"/>  
            <typeAlias alias="Dog" type="BuildUpSample.Dog, BuildUpSample"/>  
        </typeAliases>  
          
        <containers>  
            <container name="Sample">  
                <!--DI コンテナに登録する型を記述-->                  
                <types>  
                    <type type="IAnimal" mapTo="Dog"/>  
                </types>  
            </container>  
        </containers>  
    </unity>  
</configuration>  
```

別名使用しないで```<type type="BuildUpSample.IAnimal, BuildUpSample" mapTo="BuildUpSample.Cat, BuildUpSample"/>  ```でもOK  
xmlめんどうなのでscriptで書いた方がはやそう


UnityContainerExtension 
