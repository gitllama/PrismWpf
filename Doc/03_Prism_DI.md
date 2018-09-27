# DI

PrismでのDIコンテナは以下

- Autofac ( Nuget / v6.3.0 )
- DryIoc ( Nuget / v6.3.0 )
- Mef ( Nuget / v6.3.0 )
- Ninject ( Nuget / inaccessible )
- StructureMap ( Nuget / v6.3.0 )
- Unity ( Nuget / v6.3.0 )

UWPなら

- Autofac
- SimpleInjector
- Unity

Xamarin

- Autofac
- DryIoc
- Ninject
- Unity

Unityが標準らしいが、開発終了っぽい。  
どうせUWPやXamarin元考えるならAutofacでよろしいのではないでしょうか。

## Autofac

### 基本

#### 登録して取得

```C#
    var builder = new ContainerBuilder();

    // Register types that expose interfaces...
    builder.RegisterType<ConsoleLogger>().As<ILogger>();
    // Register instances of objects you create...
    var output = new StringWriter();
    builder.RegisterInstance(output).As<TextWriter>();

    // Register expressions that execute to create objects...
    builder.Register(c => new ConfigReader("mysection")).As<IConfigReader>();

    builder.Register(c => new TaskController(c.Resolve<ITaskRepository>()));
    builder.RegisterType<TaskController>();
    builder.RegisterInstance(new TaskController());
    builder.RegisterAssemblyTypes(controllerAssembly);

    // Build the container to finalize registrations
    // and prepare for object resolution.
    var container = builder.Build();
```
```C#
    class Program
    {
        static void Main(string[] args)
        {
            //コンテナの作成
            var builder = new ContainerBuilder();
            builder.RegisterType<Greeter>().As<IGreeter>().SingleInstance();
            builder.RegisterType<GreeterClient>().As<IGreeterClient>();
            var container = builder.Build();

            //コンテナの呼び出し
            var greeter = container.Resolve<IGreeterClient>();
            greeter.SayHello();

            using(var scope = container.BeginLifetimeScope())
            {
              var reader = container.Resolve<IConfigReader>();
            }
        }
    }

    interface IGreeter
    {
        string Greet();
    }

    class Greeter : IGreeter
    {
        public string Greet() { return "Hello world"; }
    }

    interface IGreeterClient
    {
        void SayHello();
    }

    class GreeterClient : IGreeterClient
    {
        private IGreeter greeter;
        public GreeterClient(IGreeter greeter)
        {
            this.greeter = greeter;
        }
        public void SayHello() => Console.WriteLine(this.greeter.Greet());
    }
```
#### あとから登録

破棄予定。Prismもそれに応じて変更するみたい。

```C#
    var builder = new ContainerBuilder();
    builder.RegisterType<TaskController>();
    builder.Update(container);
```

#### lifetime付きで登録

```C#
    builder.RegisterType<Greeter>().As<IGreeter>().SingleInstance();
```
### Prismでの使用

#### Shellの登録

```C#
class Bootstrapper : AutofacBootstrapper
{
    //ConfigureContainerBuilderをoverrideしてShellの登録
    protected override void ConfigureContainerBuilder(ContainerBuilder builder)
    {
        base.ConfigureContainerBuilder(builder);
        builder.RegisterType<Shell>();　         // Shell の登録
    }

    protected override DependencyObject CreateShell()
    {
        return Container.Resolve<MainWindow>();
    }

    protected override void InitializeShell()
    {
        Application.Current.MainWindow.Show();
    }
}
```

#### Modelの登録

ShellのContainerと共有する場合、class Bootstrapperをpublicにしてclass AppにBootstrapper bootstrapperをpublicにするとできそうだが、めんどうなので分ける。

```C#
//App.xaml.cs
public partial class App : Application
{
    public static IContainer modelcontainer;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var modelbuilder = new ContainerBuilder();
        modelbuilder.RegisterType<Models.Model>().SingleInstance();
        modelcontainer = modelbuilder.Build();

        var bootstrapper = new Bootstrapper();
        bootstrapper.Run();
    }
}

//ViewModel.cs
public class MainWindowViewModel : BindableBase
{
    public MainWindowViewModel()
    {
        var model = App.modelcontainer.Resolve<Models.Model>();
    }
}
```

## UnityContainer

プロジェクトがR.I.P.なされたっぽくてあまり。

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
- ExternallyControlledLifetimeManager : 呼出場所のスコープ範囲内。GCに回収されない限り同一。WeakReference使用  
- PerThreadLifetimeManager : Thread毎のSingleton  
- TransientLifetimeManager : 単なるFactoryMethod。DIコンテナに要求する度にinstance生成。newと同じ  

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
