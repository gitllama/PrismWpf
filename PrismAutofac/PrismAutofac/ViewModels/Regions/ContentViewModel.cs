using Prism.Commands;
using Prism.Mvvm;
using PrismAutofac.Models;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
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
            BitmapImage bmpImage = new BitmapImage();
            FileStream stream = File.OpenRead(".\\sample.jpg");
            bmpImage.BeginInit();
            bmpImage.CacheOption = BitmapCacheOption.OnLoad;
            bmpImage.StreamSource = stream;
            bmpImage.EndInit();
            stream.Close();

            var img = new WriteableBitmap(bmpImage);
            //var img = new WriteableBitmap(640, 480, 96, 96, PixelFormats.Bgr24, null)
            Image = new ReactiveProperty<WriteableBitmap>(img);

            Scale = model.WindowModelBase.ObserveProperty(x => x.Scale).ToReactiveProperty();
            AutoScale = model.WindowModelBase.ObserveProperty(x => x.AutoScale).ToReactiveProperty();
        }
    }
}
