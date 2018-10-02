# ReactiveProperty

## Property

「ReactivePropertyMode.DistinctUntilChanged」を指定すると、監視対象から変更通知はあったけれども
実際の値は変化していない場合に、自身は変更通知を発行しません。

_ => などは using Systemが必要なので注意

### 基本

```cs
    public ReactiveProperty<string> Input { get; private set; }
    public ReactiveProperty<string> Output { get; private set; }

    public MainViewModel()
    {
        this.Input = new ReactiveProperty<string>("初期値");
        this.Output = this.Input
            .Select(s => s != null ? s.ToUpper() : null)
            .ToReactiveProperty();
    }
```

```cs
    public ReactiveProperty<string> Output { get; private set; }

    public MainViewModel()
    {
        Model model = new Model();

		//M -> VM
		this.Output = this.model
			.ObserveProperty(x => x.output)
            .ToReactiveProperty();

		this.Age = model
			.ObserveProperty(x => x.Age) // Ageプロパティを監視するIObservableに変換
			.Select(x => x.ToString()) // LINQで加工して
			.ToReactiveProperty(); // ReactivePropertyに変換1

      model
        .ObserveProperty(x => x.Age) // Ageプロパティを監視するIObservableに変換
        .Subscribe(_=>);

		//M <-> VM
        this.IO = this.model
            .ToReactivePropertyAsSynchronized(x => x.IO);

            this.Age = model
                .ToReactivePropertyAsSynchronized(
                    x => x.Age, // Ageプロパティを
                    convert: x => x.ToString(), // M -> VMのときは文字列に変換
                    convertBack: x => int.Parse(x)); // VM -> Mの時にはintに変換

		//VM -> M only
            this.Name = ReactiveProperty.FromObject(
                model, // もとになるModelを指定して
                x => x.Name); // プロパティを指定する

            this.Age = ReactiveProperty.FromObject(
                model, // もとになるModelを指定して
                x => x.Age, // プロパティを指定する
                convert: x => x.ToString(), // M -> VMの変換処理
                convertBack: x => int.Parse(x)); // VM -> Mの変換処理
    }
```

### 合成

```cs
	public ReactiveProperty<int> Lhs { get; }
  	public ReactiveProperty<int> Rhs { get; }
  	public ReadOnlyReactiveProperty<int> Answer { get; }

  	this.Lhs = new ReactiveProperty<int>(0);
   	this.Rhs = new ReactiveProperty<int>(0);
  	this.Answer = this.Lhs.CombineLatest(this.Rhs, (x, y) => x + y)
                .ToReadOnlyReactiveProperty();
```

### 入力検査

```cs
           this.Name = model
                .ToReactivePropertyAsSynchronized(x => x.Name)
                .SetValidateNotifyError((IObservable<string> ox) => // 入力値のストリーム
                    Observable.Merge(
                        ox.Where(x => string.IsNullOrWhiteSpace(x)).Select(_ => "Error"), // 空文字のときはエラーメッセージを返す
                        ox.Where(x => !string.IsNullOrWhiteSpace(x)).Select(_ => default(string))) // 空文字以外のときはエラーがないのでnull
                );

this.NameError = Observable.Merge(
        this.Name.ObserveErrorChanged.Where(x => x == null).Select(_ => default(string)), // エラーのないときはnull
        this.Name.ObserveErrorChanged.Where(x => x != null).Select(x => x.OfType<string>().FirstOrDefault()) // エラーのあるときは最初のstring
    )
    .ToReactiveProperty();

this.Name = model
    .ToReactivePropertyAsSynchronized(x => x.Name)
    .SetValidateNotifyError((string x) =>
    {
        return string.IsNullOrWhiteSpace(x) ? "Error" : null;
    });

// 定義部分
[Required(ErrorMessage = "Error!!")]
public ReactiveProperty<string> Name { get; private set; }
// インスタンス化処理
this.Name = new ReactiveProperty<string>()
    .SetValidateAttribute(() => this.Name);

this.Name = model
    .ToReactivePropertyAsSynchronized(
        x => x.Name,
        ignoreValidationErrorValue: true) // 検証エラーのある値はModelに渡さない
    .SetValidateNotifyError((string x) => string.IsNullOrWhiteSpace(x) ? "Error" : null);
```
### 後始末

``` this.Name = model.ObserveProperty(x => x.Name).ToReactiveProperty(); ```

のようなケースではDisposeしないと予期しない動作する可能性あり。

```cs
	//Aパターン
	private CompositeDisposable Disposable { get; } = new CompositeDisposable();
    public Main()
	{
		this.PropA = hogeObservable.ToReactiveProperty();
	    this.PropB = fugaObservable.ToReactiveProperty();
	    // Disposeを集めておく
	    this.Disposable.Add(this.PropA);
	    this.Disposable.Add(this.PropA);
	}
	public void Close()
	{
		this.Disposable.Dispose();
	}

	//Bパターン
	private CompositeDisposable Disposable { get; } = new CompositeDisposable();
    public Main()
	{
		this.PropA = hogeObservable.ToReactiveProperty().AddTo(this.Disposable);;
	    this.PropB = fugaObservable.ToReactiveProperty().AddTo(this.Disposable);;
	}
	public void Close()
	{
		this.Disposable.Dispose();
	}
```

### その他

```cs
            this.Output = this.Input
                .Delay(TimeSpan.FromSeconds(1)) // 1秒間待機して
                .Select(x => x.ToUpper()) // 大文字に変換して
                .ToReactiveProperty(); // ReactiveProperty化する
```

#### 時間毎の実行

```cs
public ReactiveProperty<string> Name { get; } = Observable.Interval(TimeSpan.FromSeconds(1))
        .Select(x => $"tanaka {x}")
        .ToReactiveProperty();
```

## Command

###基本

```cs
	public ReactiveCommand ClearCommand { get; private set; }

	this.ClearCommand = this.Input
      	.Select(x => !string.IsNullOrWhiteSpace(x)) // Input.Valueが空じゃないとき
      	.ToReactiveCommand(); // 実行可能なCommandを作る
  	this.ClearCommand.Subscribe(_ => this.Input.Value = "")
```

### 合成

```cs
        IsCheckedA = new ReactiveProperty<bool>();
        IsCheckedB = new ReactiveProperty<bool>();
        IsCheckedC = new ReactiveProperty<bool>();

        ExecCommand = new[] { IsCheckedA, IsCheckedB, IsCheckedC }
            .CombineLatestValuesAreAllTrue()
            .ToReactiveCommand();

        ExecCommand.Subscribe(_ => MessageBox.Show("しんぷる！"));
```

###

http://blog.okazuki.jp/entry/2015/02/22/212827
http://sssslide.com/www.slideshare.net/okazuki0130/prism-reactiveproperty
