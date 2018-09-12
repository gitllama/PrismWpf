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

        private string _Title = "Prism Autofac Application";
        public string Title { get => _Title; set { if (isChangeable) { SetProperty(ref _Title, value); } } }

    }
}
