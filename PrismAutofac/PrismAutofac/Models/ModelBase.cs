using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismAutofac.Models
{
    public abstract class ModelBase : BindableBase
    {
        bool _isChangeable = true;
        public bool isChangeable { get => _isChangeable; set => SetProperty(ref _isChangeable, value); }

        private string _text = "Prism Unity Application";
        public string Text { get => _text; set { if (isChangeable) { SetProperty(ref _text, value); } } }

    }
}
