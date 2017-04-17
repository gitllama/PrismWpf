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
            using (var sr = new StreamReader("EditerConfig.yaml"))
            {
                var deserializer = new Deserializer();
                var hoge = deserializer.Deserialize<TextEditorOptions>(sr);
                Option = hoge;
            }

            //初期値の設定
            var globals = new Globals { P = new Pixel(10,10) };


            Document.Text = File.ReadAllText("Script.csx");
            
            RunCommand = new DelegateCommand(async () =>
            {
                try
                {
                    var state = await CSharpScript.RunAsync(
                        Document.Text,
                        ScriptOptions.Default.WithImports("System.Math")
                        , globals: globals);

                    foreach (var variable in state.Variables)
                    {
                        Result += $"{variable.Name} = {variable.Value} of type {variable.Type}\r\n";
                    }
                }
                catch(Exception e)
                {
                    Result += e.ToString();
                }
            });
        }
    }

    public class Globals
    {
        public Pixel P;
    }

    public class Pixel
    {
        public int[] P;
        public int Width;
        public int Height;

        public int this[int i] { get => P[i]; set => P[i] = value; }
        public int this[int x, int y] { get => P[x + y * Width]; set => P[x + y * Width] = value; }


        public Pixel(int width,int height)
        {
            Width = width;
            Height = height;
            P = new int[Width * Height];
        }
    }
}
