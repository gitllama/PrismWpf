# PrismWpf

https://github.com/PrismLibrary/Prism-Samples-Wpf

## Property Notifies 

```C#
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

## Command

```C#
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

```XML
    <i:Interaction.Triggers>
        <prism:InteractionRequestTrigger SourceObject="{Binding NotificationRequest}">
            <prism:PopupWindowAction IsModal="True" CenterOverAssociatedObject="True" />
        </prism:InteractionRequestTrigger>
    </i:Interaction.Triggers>
```

```C#
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
