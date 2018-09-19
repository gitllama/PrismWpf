using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PrismAutofac.Models
{
    public enum EnumTypes
    {
        A,
        B,
        C
    }

    public abstract class ModelBase : BindableBase
    {
        public string ModelName => this.GetType().FullName;

        WindowModelBase _WindowModelBase = new WindowModelBase();
        public WindowModelBase WindowModelBase { get => _WindowModelBase; set => SetProperty(ref _WindowModelBase, value); }


        bool _isChangeable = true;
        public bool isChangeable { get => _isChangeable; set => SetProperty(ref _isChangeable, value); }

        private string _Title = "Prism Autofac Application";
        public string Title { get => _Title; set { if (isChangeable) { SetProperty(ref _Title, value); } } }

        private Brush _BarColor = Brushes.AliceBlue;
        public Brush BarColor { get => _BarColor; set => SetProperty(ref _BarColor, value); }


        private int[] _Data = new int[] { 10, 20, 30, 20, 10 };
        public int[] Data { get => _Data; set => SetProperty(ref _Data, value); }

        private int _MaxValue = 0;
        public int MaxValue { get => _MaxValue; set => SetProperty(ref _MaxValue, value); }

        private int _MinValue = 2;
        public int MinValue { get => _MinValue; set => SetProperty(ref _MinValue, value); }

        private EnumTypes _EnumType = EnumTypes.A;
        public EnumTypes EnumType { get => _EnumType; set => SetProperty(ref _EnumType, value); }
    }


    public class WindowModelBase : BindableBase
    {
        public string ModelName => nameof(WindowModelBase);

        private double _Scale = 1;
        public double Scale { get => _Scale; set => SetProperty(ref _Scale, value); }

        private bool _AutoScale = true;
        public bool AutoScale { get => _AutoScale; set => SetProperty(ref _AutoScale, value); }
    }
}