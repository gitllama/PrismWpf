using Prism.Commands;
using Prism.Mvvm;
using PrismAutofac.Models;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PrismAutofac.ViewModels
{
    public class ContentViewModel : BindableBase
    {
        public ReactiveProperty<WriteableBitmap> Image { get; private set; }
        public ReactiveProperty<double> Scale { get; private set; }
        public ReactiveProperty<bool> AutoScale { get; private set; }

        public ContentViewModel(ModelBase model)
        {
            Image = new ReactiveProperty<WriteableBitmap>(
                new WriteableBitmap(64, 48, 96, 96, PixelFormats.Bgr24, null)
            );

            Scale = model.WindowModelBase.ObserveProperty(x => x.Scale).ToReactiveProperty();
            AutoScale = model.WindowModelBase.ObserveProperty(x => x.AutoScale).ToReactiveProperty();
        }
    }
}
