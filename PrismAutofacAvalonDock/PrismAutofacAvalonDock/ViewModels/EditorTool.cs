using Autofac;
using AvalonDockUtil;
using ICSharpCode.AvalonEdit.Document;
using PrismAutofacAvalonDock.Models;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismAutofacAvalonDock.ViewModels
{
    public class EditorTool : ToolContent
    {
        Model model;

        private TextDocument _doc = new TextDocument();
        public TextDocument doc { get => _doc; set => SetProperty(ref _doc, value); }

        public EditorTool() : base("EditorTool")
        {
            //Modelの登録
            model = App.Container.Resolve<Model>();
            doc.Text = model.Script;
            doc.TextChanged += TextDocument_TextChanged;
        }

        private void TextDocument_TextChanged(object sender, EventArgs e)
        {
            model.Script = doc.Text;
        }
    }
}
