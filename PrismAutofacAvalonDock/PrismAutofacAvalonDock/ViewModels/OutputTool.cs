using Autofac;
using AvalonDockUtil;
using PrismAutofacAvalonDock.Models;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System;

namespace PrismAutofacAvalonDock.ViewModels
{
    public class OutputTool : ToolContent
    {
        Model model;
        string _Output;
        public string Output { get => _Output; set => SetProperty(ref _Output, value); }

        public OutputTool() : base("Output")
        {
            model = App.Container.Resolve<Model>();
            model.Output += (x) =>
             {
                 Output += $"{x}{System.Environment.NewLine}";
                 RaisePropertyChanged(nameof(Output));
             };
        }
    }
}