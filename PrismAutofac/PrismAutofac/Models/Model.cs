using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismAutofac.Models
{
    public class Model : BindableBase
    {
        private string _text = "Prism Unity Application";
        public string Text { get => _text; set => SetProperty(ref _text, value); }
    }
}
