using AvalonDockUtil;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace PrismAutofacAvalonDock.Models
{
    public class Model : WorkspaceBaseModel
    {
        public Dictionary<string, WriteableBitmap> Images;

        public int _test;
        public int test { get => _test; set => SetProperty(ref _test, value); }

        public bool _isLinkage = true;
        public bool isLinkage { get => _isLinkage; set => SetProperty(ref _isLinkage, value); }

        private double _Scale = 1;
        public double Scale{ get => _Scale; set => SetProperty(ref _Scale, value); }

        double _ScrollBarV;
        public double ScrollBarV { get => _ScrollBarV; set => SetProperty(ref _ScrollBarV, value); }
        double _ScrollBarH;
        public double ScrollBarH { get => _ScrollBarH; set => SetProperty(ref _ScrollBarH, value); }


        public Model()
        {

        }
    }
}
