using Prism.Mvvm;
using Prism.Events;
using Prism.Interactivity.InteractionRequest;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using Autofac;
using Autofac.Builder;

namespace PrismAutofac.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Prism Autofac Application";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public ReactiveProperty<string> text { get; private set; }
        public ReactiveProperty<object> obj { get; private set; }

        public MainWindowViewModel()
        {
            var model = App.modelcontainer.Resolve<Models.Model>();

            obj = new ReactiveProperty<object>((object)model);

            this.text = model.ObserveProperty(x => x.Text).ToReactiveProperty();
        }
    }
}
