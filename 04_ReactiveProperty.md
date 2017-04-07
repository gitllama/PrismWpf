# ReactiveProperty

## Property

�Ԗڂ̈����́A�Ď��Ώۂ̒l���ύX���ꂽ�ۂ�ReactiveProperty�̋������w�肵�Ă��܂��B
�uReactivePropertyMode.DistinctUntilChanged�v���w�肷��ƁA�Ď��Ώۂ���ύX�ʒm�͂���������ǂ����ۂ̒l�͕ω����Ă��Ȃ��ꍇ�ɁA���g�͕ύX�ʒm�𔭍s���܂���B

_ => �� using System.Windows?

### ��{

```C#
    public ReactiveProperty<string> Input { get; private set; }
    public ReactiveProperty<string> Output { get; private set; }

    public MainViewModel()
    {
        this.Input = new ReactiveProperty<string>("�����l");
        this.Output = this.Input
            .Select(s => s != null ? s.ToUpper() : null)
            .ToReactiveProperty();
    }
```

```C#
    public ReactiveProperty<string> Output { get; private set; }

    public MainViewModel()
    {
        Model model = new Model();
        
		//M -> VM
		this.Output = this.model
			.ObserveProperty(x => x.output)
            .ToReactiveProperty();

		this.Age = model
			.ObserveProperty(x => x.Age) // Age�v���p�e�B���Ď�����IObservable�ɕϊ�
			.Select(x => x.ToString()) // LINQ�ŉ��H����
			.ToReactiveProperty(); // ReactiveProperty�ɕϊ�1

		//M <-> VM
        this.IO = this.model
            .ToReactivePropertyAsSynchronized(x => x.IO);

            this.Age = model
                .ToReactivePropertyAsSynchronized(
                    x => x.Age, // Age�v���p�e�B��
                    convert: x => x.ToString(), // M -> VM�̂Ƃ��͕�����ɕϊ�
                    convertBack: x => int.Parse(x)); // VM -> M�̎��ɂ�int�ɕϊ�

		//VM -> M only
            this.Name = ReactiveProperty.FromObject(
                model, // ���ƂɂȂ�Model���w�肵��
                x => x.Name); // �v���p�e�B���w�肷��

            this.Age = ReactiveProperty.FromObject(
                model, // ���ƂɂȂ�Model���w�肵��
                x => x.Age, // �v���p�e�B���w�肷��
                convert: x => x.ToString(), // M -> VM�̕ϊ�����
                convertBack: x => int.Parse(x)); // VM -> M�̕ϊ�����
    }
```

### ����

```C#
	public ReactiveProperty<int> Lhs { get; }
  	public ReactiveProperty<int> Rhs { get; }
  	public ReadOnlyReactiveProperty<int> Answer { get; }

  	this.Lhs = new ReactiveProperty<int>(0);
   	this.Rhs = new ReactiveProperty<int>(0);
  	this.Answer = this.Lhs.CombineLatest(this.Rhs, (x, y) => x + y)
                .ToReadOnlyReactiveProperty();
```

### ���͌���

```C#
           this.Name = model
                .ToReactivePropertyAsSynchronized(x => x.Name)
                .SetValidateNotifyError((IObservable<string> ox) => // ���͒l�̃X�g���[��
                    Observable.Merge(
                        ox.Where(x => string.IsNullOrWhiteSpace(x)).Select(_ => "Error"), // �󕶎��̂Ƃ��̓G���[���b�Z�[�W��Ԃ�
                        ox.Where(x => !string.IsNullOrWhiteSpace(x)).Select(_ => default(string))) // �󕶎��ȊO�̂Ƃ��̓G���[���Ȃ��̂�null
                );

this.NameError = Observable.Merge(
        this.Name.ObserveErrorChanged.Where(x => x == null).Select(_ => default(string)), // �G���[�̂Ȃ��Ƃ���null
        this.Name.ObserveErrorChanged.Where(x => x != null).Select(x => x.OfType<string>().FirstOrDefault()) // �G���[�̂���Ƃ��͍ŏ���string
    )
    .ToReactiveProperty();

this.Name = model
    .ToReactivePropertyAsSynchronized(x => x.Name)
    .SetValidateNotifyError((string x) =>
    {
        return string.IsNullOrWhiteSpace(x) ? "Error" : null;
    });

// ��`����
[Required(ErrorMessage = "Error!!")]
public ReactiveProperty<string> Name { get; private set; }
// �C���X�^���X������
this.Name = new ReactiveProperty<string>()
    .SetValidateAttribute(() => this.Name);

this.Name = model
    .ToReactivePropertyAsSynchronized(
        x => x.Name,
        ignoreValidationErrorValue: true) // ���؃G���[�̂���l��Model�ɓn���Ȃ�
    .SetValidateNotifyError((string x) => string.IsNullOrWhiteSpace(x) ? "Error" : null);
```
### ��n��

```this.Name = model.ObserveProperty(x => x.Name).ToReactiveProperty();```

�̂悤�ȃP�[�X�ł�Dispose���Ȃ��Ɨ\�����Ȃ����삷��\������B

```C#
	//A�p�^�[��
	private CompositeDisposable Disposable { get; } = new CompositeDisposable();
    public Main()
	{
		this.PropA = hogeObservable.ToReactiveProperty();
	    this.PropB = fugaObservable.ToReactiveProperty();
	    // Dispose���W�߂Ă���
	    this.Disposable.Add(this.PropA);
	    this.Disposable.Add(this.PropA);
	}
	public void Close()
	{
		this.Disposable.Dispose();
	}

	//B�p�^�[��
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

### ���̑�

```C#
            this.Output = this.Input
                .Delay(TimeSpan.FromSeconds(1)) // 1�b�ԑҋ@����
                .Select(x => x.ToUpper()) // �啶���ɕϊ�����
                .ToReactiveProperty(); // ReactiveProperty������
```

## Command

###��{

```C#
	public ReactiveCommand ClearCommand { get; private set; }
            
	this.ClearCommand = this.Input
      	.Select(x => !string.IsNullOrWhiteSpace(x)) // Input.Value���󂶂�Ȃ��Ƃ�
      	.ToReactiveCommand(); // ���s�\��Command�����
  	this.ClearCommand.Subscribe(_ => this.Input.Value = "")
```

### ����

```C#
        IsCheckedA = new ReactiveProperty<bool>();
        IsCheckedB = new ReactiveProperty<bool>();
        IsCheckedC = new ReactiveProperty<bool>();
 
        ExecCommand = new[] { IsCheckedA, IsCheckedB, IsCheckedC }
            .CombineLatestValuesAreAllTrue()
            .ToReactiveCommand();
 
        ExecCommand.Subscribe(_ => MessageBox.Show("����Ղ�I"));
```

