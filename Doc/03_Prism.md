# PrismWpf

https://github.com/PrismLibrary/Prism-Samples-Wpf

## Property Notifies

```cs
  private string _title = "Prism Unity Application";
  public string Title { get => _title; set => SetProperty(ref _title, value); }

  public string Title { get => _title; set => SetProperty(ref _title, value,"Title"); }

  private string _title = "Prism Unity Application";
  public string Title { get => _title; set => SetProperty(ref _title, value,()=>onChanged(value)); }

  public void Method()
  {
    RaisePropertyChanged("Title");
  }
```

```cs
  public void ViewModel()
  {
    model.PropertyChanged += Model_PropertyChanged;
  }
  private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
  {
    if(e.PropertyName == "Title"){ }
  }
```

## Command

```cs
  public DelegateCommand ToUpperCommand { get; }
  public DelegateCommandSampleViewModel()
  {
    this.ToUpperCommand = new DelegateCommand(() =>
    {
      this.Output = this.Input.ToUpper();
    },
    () => !string.IsNullOrWhiteSpace(this.Input));

    this.ToUpperCommand.ObservesProperty(() => this.Input);
  }
```

## PopupWindowAction

```cs
    <i:Interaction.Triggers>
        <prism:InteractionRequestTrigger SourceObject="{Binding NotificationRequest}">
            <prism:PopupWindowAction IsModal="True" CenterOverAssociatedObject="True" />
        </prism:InteractionRequestTrigger>
    </i:Interaction.Triggers>
```

```cs
  public InteractionRequest<INotification> NotificationRequest { get; set; }
  public DelegateCommand NotificationCommand { get; set; }

  public MainWindowViewModel()
  {
    NotificationRequest = new InteractionRequest<INotification>();
    NotificationCommand = new DelegateCommand(RaiseNotification);
  }

  void RaiseNotification()
  {
    NotificationRequest.Raise(new Notification { Content = "Notification Message", Title = "Notification" }, r => Title = "Notified");
  }
```

## コードビハインド

### Bindingを強制的に評価

```cs
  private void Button_Click(object sender, RoutedEventArgs e)
  {
    var expression = this.e.GetBindingExpression(TextBox.TextProperty);
    expression.UpdateTarget();
  }
```
