using System;
using System.Xml;
using AvalonDockUtil;
using Prism.Mvvm;
using Reactive.Bindings;
using Xceed.Wpf.AvalonDock;
using Reactive.Bindings.Interactivity;
using Autofac;
using PrismAutofacAvalonDock.Models;
using System.Collections.ObjectModel;
using Reactive.Bindings.Extensions;

namespace PrismAutofacAvalonDock.ViewModels
{
    public class MainWindowViewModel : WorkspaceBase
    {
        private string _title = "Prism Autofac Application";
        public string Title{ get => _title; set => SetProperty(ref _title, value); }

        public ReactiveCommand<object> LoadedCommand { get; private set; }
        public ReactiveCommand<object> UnloadedCommand { get; private set; }

        public ReactiveCommand<object> DocumentClosingCommand { get; private set; }

        //Model
        Model model;


        //SubWindowの管理
        public ReactiveProperty<ObservableCollection<DocumentContent>> Documents { get; private set; }
        public ReactiveProperty<ObservableCollection<ToolContent>> Tools { get; private set; }
        public ReactiveProperty<DocumentContent> ActiveDocument { get; private set; }
        public ReactiveCommand<object> DocumentOpenCommand { get; private set; }
        public ReactiveCommand<string> DocumentClosedCommand { get; private set; }

        //Modelとの連携


        public MainWindowViewModel()
        {
            //Modelの登録
            model = App.Container.Resolve<Model>();


            Documents = model.ObserveProperty(x => x.Documents).ToReactiveProperty();
            Tools = model.ObserveProperty(x => x.Tools).ToReactiveProperty();
            ActiveDocument = model.ToReactivePropertyAsSynchronized(x => x.ActiveDocument);


            LoadedCommand = new ReactiveCommand();
            LoadedCommand.Subscribe(_ =>
            {
                LoadLayout(null);
            });
            UnloadedCommand = new ReactiveCommand<object>();
            UnloadedCommand.Subscribe(x =>
            {
                SaveLayout((DockingManager)x);
            });
            DocumentClosingCommand = new ReactiveCommand<object>();
            DocumentClosingCommand.Subscribe(x =>
            {
                SaveLayout((DockingManager)x);
            });

            DocumentOpenCommand = new ReactiveCommand();
            DocumentOpenCommand.Subscribe(_ =>
            {
                var i = new Document();
                model.Add(i);
            });
            DocumentClosedCommand = new ReactiveCommand<string>();
            DocumentClosedCommand.Subscribe(x => model.Remove(x as string));

        }

        //public override DocumentContent NewDocument()
        //{
        //    //return new UriListDocument(this.Messenger);
        //    return new Document();
        //}

        protected override void InitializeTools()
        {
            model.Tools.Clear();
            model.Tools.Add(new PropertyTool());
            model.Tools.Add(new EditorTool());
            model.Tools.Add(new OutputTool());

            model.Add(new Document());
        }

        protected override void RestoreDocumentsFromBytes(byte[] bytes)
        {
            // 独自にxmlを解析する
            //using (var stream = new System.IO.MemoryStream(bytes))
            //{
            //    var doc = new XmlDocument();
            //    doc.Load(stream);
            //    // ContentIDが"Document"のIDを探す
            //    var nodes = doc.GetElementsByTagName("LayoutDocument");
            //    for (int i = 0; i < nodes.Count; ++i)
            //    {
            //        var node = nodes[i];

            //        object document = null;// GetDocumentByContentId(node.Attributes["ContentId"].Value) as UriListDocument;
            //        if (document != null)
            //        {
            //            var viewModel = document.ViewModel;
            //            foreach (XmlAttribute attrib in node.Attributes)
            //            {
            //                if (attrib.Name == "FilePath")
            //                {
            //                    viewModel.Path = attrib.Value;
            //                    viewModel.Load();
            //                }
            //            }
            //        }
            //    }
            //}
        }

        protected override void ModifySerializedXml(XmlDocument doc)
        {
            throw new NotImplementedException();
        }
    }

    //public class UnloadedConverter : ReactiveConverter<EventArgs, object>
    //{

    //    protected override IObservable<string> Convert(IObservable<EventArgs> source)
    //    {
    //        return source
    //            .Select(_ => new OpenFileDialog())
    //            .Do(x => x.Filter = "*.*|*.*")
    //            .Where(x => x.ShowDialog() == true) // Show dialog
    //            .Select(x => x.FileName); // convert to string
    //    }
    //}
}
