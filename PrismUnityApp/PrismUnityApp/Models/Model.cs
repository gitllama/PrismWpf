using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace PrismUnityApp.Models
{
    public class Model : BindableBase
    {
        public Model()
        {

        }

        private double _Scale = 1;
        public double Scale
        {
            get { return _Scale; }
            set { SetProperty(ref _Scale, value); }
        }

        private BitmapScalingMode _ScalingMode;
        public BitmapScalingMode ScalingMode
        {
            get { return _ScalingMode; }
            set { SetProperty(ref _ScalingMode, value); }
        }
    }
}
