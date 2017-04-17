using System;
using ICSharpCode.AvalonEdit.Document;
using Prism.Mvvm;
using Prism.Commands;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using ICSharpCode.AvalonEdit;
using System.IO;
using YamlDotNet.Serialization;

namespace ScriptingTest.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Prism Unity Application";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private TextDocument document = new TextDocument();
        public TextDocument Document
        {
            get { return document; }
            set { this.SetProperty(ref this.document, value); }
        }

        private string _Result;
        public string Result
        {
            get { return _Result; }
            set { SetProperty(ref _Result, value); }
        }

        private TextEditorOptions _Option = new TextEditorOptions();
        public TextEditorOptions Option
        {
            get { return _Option; }
            set { SetProperty(ref _Option, value); }
        }

        public DelegateCommand RunCommand { get; }

        public MainWindowViewModel()
        {
            //初期値の設定


            using (var sr = new StreamReader("EditerConfig.yaml"))
            {
                var deserializer = new Deserializer();
                var hoge = deserializer.Deserialize<TextEditorOptions>(sr);
                Option = hoge;
            }

            Document.Text = "int j = 0;\r\nfor(int i=0;i<4;i++)\r\n  j+=i;";

            var globals = new Globals { X = 1, Y = 2 };

            RunCommand = new DelegateCommand(async () =>
            {
                //var state = await CSharpScript.EvaluateAsync(
                //    Document.Text,
                //    ScriptOptions.Default.WithImports("System.Math")
                //    , globals: globals);

                var state = await CSharpScript.RunAsync(
                    Document.Text,
                    ScriptOptions.Default.WithImports("System.Math")
                    , globals: globals);

                foreach (var variable in state.Variables)
                {
                    Result += $"{variable.Name} = {variable.Value} of type {variable.Type}\r\n";
                }
            });
        }
    }

    public class Globals
    {
        public int X;
        public int Y;
    }
}
