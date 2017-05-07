using AvalonDockUtil;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismAutofacAvalonDock.ViewModels
{
    public class EditorTool : ToolContent
    {
        public ReactiveProperty<object> obj { get; private set; }

        public EditorTool() : base("EditorTool")
        {

        }
    }
}
