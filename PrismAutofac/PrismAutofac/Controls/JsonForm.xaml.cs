using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;


namespace PrismAutofac.Controls
{
    /// <summary>
    /// HistogramUserControl.xaml の相互作用ロジック
    /// </summary>
    public partial class JsonForm : UserControl, IDisposable
    {

        #region MyRegion

        public static readonly DependencyProperty DataProperty = DependencyProperty.Register(
            nameof(Data),
            typeof(object),
            typeof(JsonForm),
            new PropertyMetadata(null, dataPropertyChanged));
        public object Data { get => (object)GetValue(DataProperty); set => SetValue(DataProperty, value); }

        public static readonly DependencyProperty JsonProperty = DependencyProperty.Register(
            nameof(Json),
            typeof(string),
            typeof(JsonForm),
            new PropertyMetadata(null, ViewListPropertyChanged));
        public string Json { get => (string)GetValue(JsonProperty); set => SetValue(JsonProperty, value); }

        private static void dataPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = d as JsonForm;

            if (e.OldValue is INotifyPropertyChanged oldValue)
            {
                oldValue.PropertyChanged -= obj.JsonForm_PropertyChanged;
            }
            if (e.NewValue is INotifyPropertyChanged newValue)
            {
                newValue.PropertyChanged += obj.JsonForm_PropertyChanged;
                JsonForm.UpdateChild(obj, obj.Json, obj.Json);
            }
        }
        private static void ViewListPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = d as JsonForm;

            var oldList = e.OldValue as string;
            var newList = e.NewValue as string;
            JsonForm.UpdateChild(obj, oldList, newList);
        }

        #endregion


        #region Constructor/IDisposable

        public JsonForm()
        {
            InitializeComponent();
            (this.Content as FrameworkElement).DataContext = this;
        }

        private bool disposedValue = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: マネージ状態を破棄します (マネージ オブジェクト)。
                    if (Data is INotifyPropertyChanged a)
                        a.PropertyChanged -= JsonForm_PropertyChanged;
                }

                disposedValue = true;
            }
        }

        void IDisposable.Dispose()
        {
            Dispose(true);
        }

        #endregion

        #region Event

        private void JsonForm_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var value = Data.GetPropertyValue(e.PropertyName);
            switch (FindName(e.PropertyName))
            {
                case TextBox a:
                    a.Text = value.ToString();
                    break;
                case MahApps.Metro.Controls.NumericUpDown a:
                    a.Value = (dynamic)value;
                    break;
                //case MahApps.Metro.Controls.NumericUpDown a:
                //    a.Value = (double)value;
                //    break;
            }
        }

        private static void UpdateChild(JsonForm obj, string oldList, string newList)
        {
            if (obj?.Data == null) return;
            if (newList == null) return;

            var oldSet = oldList == null ? new ChildList() : JsonConvert.DeserializeObject<ChildList>(oldList);
            var newSet = JsonConvert.DeserializeObject<ChildList>(newList);

            foreach (var c in oldSet.children)
            {
                obj.UnregisterName(c.name);
            }

            var stackPanel = obj.FindName("target") as StackPanel;
            stackPanel.Children.Clear();

            obj.AddUIElement(newSet);
        }

        #endregion

 
        private void AddUIElement(ChildList c)
        {
            var stackPanel = FindName("target") as StackPanel;
            foreach (var item in c.children)
            {
                var dock = new DockPanel()
                {
                    Margin = new Thickness(3),
                    VerticalAlignment = VerticalAlignment.Bottom
                };
                var label = new Label()
                {
                    Content = item.name,
                    Width = c.TitleWidth,
                    VerticalAlignment = VerticalAlignment.Center
                };
                DockPanel.SetDock(label, Dock.Left);
                dock.Children.Add(label);

                var value = Data.GetPropertyValue(item.name);
                switch (item.childType)
                {
                    case ChildType.TextBox:
                        {
                            var child = new TextBox()
                            {
                                Name = item.name,
                                Text = (string)value
                            };
                            child.TextChanged += TxtBox_TextChanged;
                            RegisterName(child.Name, child);
                            dock.Children.Add(child);
                        }
                        break;
                    case ChildType.ComboBox:
                        {
                            var child = new ComboBox()
                            {
                                Name = item.name,
                                SelectedValue = ((Enum)value).ToString(),
                                ItemsSource = item.ComboItems
                            };
                            child.SelectionChanged += Child_SelectionChanged;
                            RegisterName(child.Name, child);
                            dock.Children.Add(child);
                        }
                        break;
                    case ChildType.NumericUpDown:
                        {
                            var child = new MahApps.Metro.Controls.NumericUpDown()
                            {
                                Name = item.name,
                                Value = (int)value
                            };
                            child.ValueChanged +=(s,e)=> Data.SetPropertyValue(((MahApps.Metro.Controls.NumericUpDown)s).Name, (int)((MahApps.Metro.Controls.NumericUpDown)s).Value);
                            RegisterName(child.Name, child);
                            dock.Children.Add(child);
                        }
                        break;
                    case ChildType.Slider:
                        {
                            var child = new MahApps.Metro.Controls.NumericUpDown()
                            {
                                Name = item.name,
                                Value = (int)value,
                                Maximum = item.Maximum,
                                Minimum = item.Minimum,
                                Width = 70,
                                HideUpDownButtons = true
                            };
                            DockPanel.SetDock(child, Dock.Left);
                            child.ValueChanged += (s, e) => Data.SetPropertyValue(
                                ((MahApps.Metro.Controls.NumericUpDown)s).Name,
                                (int)(((MahApps.Metro.Controls.NumericUpDown)s).Value ?? 0.0));
                            dock.Children.Add(child);


                            var slider = new Slider()
                            {
                                Value = (int)value,
                                Maximum = item.Maximum,
                                Minimum = item.Minimum,
                                Margin = new Thickness(3,0,3,0)
                            };
                            slider.SetBindingElement(Slider.ValueProperty, child, "Value");
                            dock.Children.Add(slider);

                            RegisterName(child.Name, child);
                        }
                        break;
                    
                }
                stackPanel.Children.Add(dock);
            }
            //txtBox.Unloaded += TxtBox_Unloaded;
            //Enum.GetNames(v.GetType()).ToList()

        }

        private void Child_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var name = ((ComboBox)sender).Name;
            var value = ((ComboBox)sender).SelectedValue;
            var type = Data.GetPropertyValue(name).GetType(); ;
            var property = Data.GetType().GetProperty(name);
            property.SetValue(Data, Enum.Parse(type, (string)value));
        }

        private void TxtBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var name = ((TextBox)sender).Name;
            var value = ((TextBox)sender).Text;

            var property = Data.GetType().GetProperty(name);
            property.SetValue(Data, value);
        }

        private void NumericUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            var name = ((MahApps.Metro.Controls.NumericUpDown)sender).Name;
            var value = ((MahApps.Metro.Controls.NumericUpDown)sender).Value;

            
        }



    }


    public class ChildList
    {
        public int TitleWidth = 70;
        public ChildState[] children = new ChildState[] { };
    }
    public class ChildState
    {
        public string name;
        public ChildType childType;
        public double Maximum = 1;
        public double Minimum = 0;
        public string[] ComboItems = new string[] { };
    }
    public enum ChildType
    {
        TextBox,
        NumericUpDown,
        Slider,
        ComboBox
    }


    public static class JsonFormExtention
    {

        public static object GetPropertyValue(this object src, string propertyName)
        {
            var property = src.GetType().GetProperty(propertyName);
            var value = property.GetValue(src);
            return value;
        }

        public static void SetPropertyValue<T>(this object src, string propertyName, T value)
        {
            var property = src.GetType().GetProperty(propertyName);
            property.SetValue(src, value);
        }

        public static void SetBinding(Control ctrl, DependencyProperty prop, string path, BindingMode mode)
        {
            Binding bi = new Binding(path);
            bi.Mode = mode;
            ctrl.SetBinding(prop, bi);
        }

        public static void SetBindingElement(this Control ctrl, DependencyProperty strlProp, Control src, string srcPath)
        {
            Binding bi = new Binding(srcPath)
            {
                Source = src,
                Mode = BindingMode.TwoWay
            };
            ctrl.SetBinding(strlProp, bi);
        }


    }
}
