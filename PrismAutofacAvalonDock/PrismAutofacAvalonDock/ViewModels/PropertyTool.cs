using Autofac;
using AvalonDockUtil;
using PrismAutofacAvalonDock.Models;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;

namespace PrismAutofacAvalonDock.ViewModels
{
    public class PropertyTool : ToolContent
    {
        Model model;
        public ReactiveProperty<object> obj { get; private set; }

        //SubWindowの管理
        public ReactiveProperty<List<string>> Documents { get; private set; }
        public ReactiveProperty<DocumentContent> ActiveDocument { get; private set; }

        public ReactiveProperty<int> SelectedDoc { get; private set; }

        public PropertyTool() : base("Property")
        {
            model = App.Container.Resolve<Model>();

            Documents = model.ObserveProperty(x => x.Documents)
                .Select(x => x.ToList())
                .Select(x=>
                {
                    var j = x.Select(n => n.ContentId).ToList();
                    j.Insert(0, "All");
                    return j;
                }).ToReactiveProperty();

            ActiveDocument = model.ToReactivePropertyAsSynchronized(x => x.ActiveDocument).ToReactiveProperty();

            SelectedDoc = new ReactiveProperty<int>(0);


            obj = new ReactiveProperty<object>((object)model);
            obj = SelectedDoc.Select(x => x == 0 ? (object)model : (object)model.Documents[x - 1]).ToReactiveProperty();
        }
    }
}