using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using PrismAutofac.Models;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace PrismAutofac.ViewModels
{
    public class ContentUserControlViewModel : BindableBase
    {
        private ModelBase model;

        public ReactiveProperty<Brush> BarColor { get; private set; }
        public ReactiveProperty<int[]> Data { get; private set; }
        public ReactiveProperty<int> MaxValue { get; private set; }
        public ReactiveProperty<int> MinValue { get; private set; }

        public ContentUserControlViewModel(ModelBase model)
        {
            this.model = model;

            BarColor = model.ObserveProperty(x => x.BarColor).ToReactiveProperty();
            Data = model.ObserveProperty(x => x.Data).ToReactiveProperty();
            MaxValue = model.ObserveProperty(x => x.MaxValue).ToReactiveProperty();
            MinValue = model.ObserveProperty(x => x.MinValue).ToReactiveProperty();
        }
    }
}
