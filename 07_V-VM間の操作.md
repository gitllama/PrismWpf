# MVVMっぽい V <-> VM 間処理

Contexet更新イベントつかんで、V <-> VMでFunc,Actionでつなぐ方法もある。

まじめにバインドしたいならカスタムコントロール作ってプロパティ公開した方が  
良さげ。

## V -> Mへ

### ReavtiveProperty + EventTrigger

```xaml
        xmlns:i="http://schemas.microsoft.com/expression/2010/interactivity"
        xmlns:r="clr-namespace:Reactive.Bindings.Interactivity;assembly=ReactiveProperty.NET45"
        
        <Canvas>
            <i:Interaction.Triggers>
                    <i:EventTrigger EventName="MouseMove">
                        <r:EventToReactiveProperty ReactiveProperty="{Binding MouseMove}">
                            <vm:MouseEventToPointConverter/>
                        </r:EventToReactiveProperty>
                    </i:EventTrigger>
                    <i:EventTrigger EventName="MouseMove">
                        <r:EventToReactive ReactiveProperty="{Binding MouseMove}"/>
                    </i:EventTrigger>
            </i:Interaction.Triggers>
```
```C#
    public class MainWindowViewModel : BindableBase
    {
        public ReactiveProperty<string> Title { get; set; }
        public ReactiveProperty<MouseEventArgs> MouseMove { get; private set; }
        public ReactiveProperty<Tuple<int, int>> MouseMove { get; private set; }
        
        public MainWindowViewModel()
        {
            MouseMove = new ReactiveProperty<MouseEventArgs>(mode: ReactivePropertyMode.RaiseLatestValueOnSubscribe);
            Title = MouseMove.Select(x => $"{x?.GetPosition(null).X ?? 0},{x?.GetPosition(null).Y ?? 0}").ToReactiveProperty();
        
            MouseMove = new ReactiveProperty<Tuple<int, int>>(mode: ReactivePropertyMode.RaiseLatestValueOnSubscribe);
            Title = MouseMove.Select(x => $"{x?.Item1 ?? 0},{x?.Item2 ?? 0}").ToReactiveProperty();
        }
    }

    public class MouseEventToPointConverter : ReactiveConverter<dynamic, Tuple<int, int>>
    {
        protected override IObservable<Tuple<int, int>> OnConvert(IObservable<dynamic> source)
        {
            return source
                .Select(x => x.GetPosition(null))
                .Select(x => Tuple.Create((int)x.X, (int)x.Y));
        }
    }
```

## VM -> Vへ

### Prism.Events + Messenger

```C#
    using Prism.Events;
    
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            Messenger.Instance
            .GetEvent<PubSubEvent<double>>().Subscribe(
                d => MessageBox.Show(d.ToString()));
        }
    }
    
    public class Messenger : EventAggregator
    {
        private static Messenger _instance;
        public static Messenger Instance {get => _instance ?? (_instance = new Messenger()); }
    }
```
```C#
    public class MainWindowViewModel : BindableBase
    {
        public ReactiveProperty<MouseEventArgs> MouseMove { get; private set; }

        public MainWindowViewModel()
        {
            MouseMove = new ReactiveProperty<MouseEventArgs>(mode: ReactivePropertyMode.RaiseLatestValueOnSubscribe);
            MouseMove.Subscribe(
                _ => Messenger.Instance
                    .GetEvent<PubSubEvent<double>>().Publish(1.0));
        }
    }  
```

## V <-> VM

### Behavior

```xaml
        <Canvas>
            <i:Interaction.Behaviors>
                <vm:MouseMoveBehavior Position="{Binding MouseMove.Value, Mode=TwoWay}"/>
            </i:Interaction.Behaviors>
```
```C#
    public class MainWindowViewModel : BindableBase
    {
        public ReactiveProperty<string> Title { get; set; }
        public ReactiveProperty<Point> MouseMove { get; private set; }

        public MainWindowViewModel()
        {
            MouseMove = new ReactiveProperty<Point>(mode: ReactivePropertyMode.RaiseLatestValueOnSubscribe);
            Title = MouseMove2.Select(x => $"{x.X},{x.Y}").ToReactiveProperty();
        }

    }

    // マウスの相対座標を取得するためには
    // e.GetPosition((Canvas)sender)
    // が必要。
    // コードビハインドではなくVMに記述するために、
    // Behavior自作してプロパティ公開を使用します。
    public class MouseMoveBehavior : Behavior<Canvas>
    {
        public Point Position
        {
            get { return (Point)GetValue(PositionProperty); }
            set { SetValue(PositionProperty, value); }
        }
        public static readonly DependencyProperty PositionProperty = DependencyProperty.Register(
            "Position", 
            typeof(Point), 
            typeof(MouseMoveBehavior), 
            new UIPropertyMetadata(null));

        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.MouseMove += AssociatedObject_MouseMove;
        }
        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.MouseMove -= AssociatedObject_MouseMove;
        }

        private void AssociatedObject_MouseMove(object sender, MouseEventArgs e)
        {
            Position = e.GetPosition((Canvas)sender);
        }
    }
```
