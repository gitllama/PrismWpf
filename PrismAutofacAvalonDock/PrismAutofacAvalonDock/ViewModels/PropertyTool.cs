using Autofac;
using AvalonDockUtil;
using PrismAutofacAvalonDock.Models;
using Reactive.Bindings;

namespace PrismAutofacAvalonDock.ViewModels
{
    public class PropertyTool : ToolContent
    {
        public ReactiveProperty<object> obj { get; private set; }

        public PropertyTool() : base("Property")
        {
            var buf = App.Container.Resolve<Model>();
            obj = new ReactiveProperty<object>((object)buf);
        }
    }
}