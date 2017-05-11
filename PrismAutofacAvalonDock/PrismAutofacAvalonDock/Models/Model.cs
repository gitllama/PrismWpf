using AvalonDockUtil;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

using Pixels;
using Pixels.Math;
using Pixels.Extend;
using Pixels.Stream;
using System.Windows.Media;
using System.Collections.ObjectModel;
using Prism.Commands;
using System.ComponentModel;
using System.IO;
using Microsoft.CodeAnalysis.Scripting;
using System.Reflection;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using System.Windows;
using YamlDotNet.Serialization;


namespace PrismAutofacAvalonDock.Models
{
    public abstract class WorkspaceBaseModel : BindableBase
    {
        ObservableCollection<DocumentContent> _Documents;
        public ObservableCollection<DocumentContent> Documents
        {
            get
            {
                if (_Documents == null) _Documents = new ObservableCollection<DocumentContent>();
                return _Documents;
            }
        }

        ObservableCollection<ToolContent> _Tools;
        public ObservableCollection<ToolContent> Tools
        {
            get
            {
                if (_Tools == null) _Tools = new ObservableCollection<ToolContent>();
                return _Tools;
            }
        }

        private DocumentContent _ActiveDocument;
        public DocumentContent ActiveDocument { get => _ActiveDocument; set => SetProperty(ref _ActiveDocument, value); }

        public virtual DocumentContent NewDocument() => new DocumentContent();

        public WorkspaceBaseModel()
        {
        }

        public virtual string Add<T>(T content) where T : DocumentContent
        {
            Documents.Add(content);
            RaisePropertyChanged("Documents");

            return content.ContentId;
        }
        public virtual void Remove(string id)
        {
            var i = Documents.Where(x => x.ContentId == id).First();
            if (i != null)
            {
                i.Close();
                Documents.Remove(i);
            }
        }
    }

    public class Model : WorkspaceBaseModel
    {
        public Dictionary<string, string> FileNames;
        public Dictionary<string, Pixel<float>> raws;
        public Dictionary<string, WriteableBitmap> Images;


        public bool _isLinkage = true;
        public bool isLinkage { get => _isLinkage; set => SetProperty(ref _isLinkage, value); }



        //読み込みブロック


        //Scriptingブロック
        string _Script = File.ReadAllText("Default.csx");
        [Category("Read"), ReadOnly(true)]
        public string Script { get => _Script; set => SetProperty(ref _Script, value); }

        private Rect _Rect;
        [ReadOnly(true), Description("you can get it in Script")]
        public Rect Rect { get => _Rect; set => SetProperty(ref _Rect, value); }

        //現像ブロック

        int _Offset = 0;
        [Category(nameof(Developing)), Description("image = (raw - Offset) * (255 / Depth)")]
        public int Offset { get => _Offset; set => SetProperty(ref _Offset, value, () => DevelopingAll()); }
        int _Depth = 255;
        [Category(nameof(Developing)), Description("image = (raw - Offset) * (255 / Depth)")]
        public int Depth { get => _Depth; set => SetProperty(ref _Depth, value, () => DevelopingAll()); }

        public enum ColorType
        {
            Mono,
            GR,
            RG,
            GB,
            BG,
        }
        ColorType _Color = ColorType.Mono;
        [Category(nameof(Developing)), Description("")]
        public ColorType Color { get => _Color; set => SetProperty(ref _Color, value, () => DevelopingAll()); }

        //表示ブロック

        private BitmapScalingMode _ScalingMode;
        [Category("Canvas"), Description("")]
        public BitmapScalingMode ScalingMode { get => _ScalingMode; set => SetProperty(ref _ScalingMode, value); }

        Point _ScrollBar;
        [Category("Canvas"), Description("")]
        public Point ScrollBar { get => _ScrollBar; set => SetProperty(ref _ScrollBar, value); }

        private double _Scale = 1;
        [Category("Canvas"), Description("")]
        public double Scale { get => _Scale; set => SetProperty(ref _Scale, value); }

        private Brush _Background = Brushes.WhiteSmoke;
        [Category("Canvas"), Description("")]
        public Brush Background { get => _Background; set => SetProperty(ref _Background, value); }

        //描画
        private Brush _BrushL = Brushes.Red;
        public Brush BrushL { get => _BrushL; set => SetProperty(ref _BrushL, value); }
        private Brush _BrushR = Brushes.Blue;
        public Brush BrushR { get => _BrushR; set => SetProperty(ref _BrushR, value); }


        //pixeloffsetmode
        //結果

        public Action<string> Output { get; set; }


        //メソッド

        public Model()
        {
            FileNames = new Dictionary<string, string>();
            raws = new Dictionary<string, Pixel<float>>();
            Images = new Dictionary<string, WriteableBitmap>();
        }

        public override string Add<T>(T content)
        {
            var i = base.Add(content);
            FileNames[i] = "null";
            raws[i] = null;
            Images[i] = null;
            return i;
        }

        public override void Remove(string id)
        {
            FileNames.Remove(id);
            raws.Remove(id);
            Images.Remove(id);
            base.Remove(id);
        }

        public void ReadFile(string filename,string key = null)
        {
            Output($"ReadFile : {filename} to {key}");

            //Auto読み込み

            FileNames[key] = filename;

            //raws[key] = PixelFactory.Create<float>(2256, 1178).Read(filename, 0, typeof(int));

            var dic = (new Deserializer()).Deserialize<List<Pixels.PixelFormat>>(File.ReadAllText("PixelFormat.yaml"));
            raws[key] = PixelFactory.Create<float>(dic, filename);

            RaisePropertyChanged(nameof(FileNames));
        }

        public void ScriptRun(string contentId) => RunAsync(contentId, Script);

        public async void RunAsync(string id,string command)
        {
            try
            {
                Output("-----Script Run-----");
                var globals = new Globals() { raw = raws[id],Rect = Rect };

                var ssr = ScriptSourceResolver.Default.WithBaseDirectory(Environment.CurrentDirectory);
                var smr = ScriptMetadataResolver.Default.WithBaseDirectory(Environment.CurrentDirectory);
                var state = await CSharpScript.RunAsync(
                    command,
                    ScriptOptions.Default
                    .WithSourceResolver(ssr)
                    .WithMetadataResolver(smr)
                    .WithReferences(Assembly.GetEntryAssembly())
                    .WithImports(new string[]
                    {
                        "System",
                        "System.Collections.Generic",
                        "System.Linq",
                        "System.Math",
                        "System.IO",
                        "Pixels",
                        "Pixels.Math",
                        "Pixels.Stream",
                        "Pixels.Extend",
                    }),
                   globals: globals);

                
                foreach (var variable in state.Variables)
                    Output($"  Result : {variable.Name} = {variable.Value} of type {variable.Type}");

                //再代入した場合参照先が変わるので書き戻しが必要
                raws[id] = globals.raw;
            }
            catch (Exception e)
            {
                Output($"{e.ToString()}");
            }
        }

        public void DevelopingAll()
        {
            foreach(var i in raws.Keys)
            {
                Developing(i);
            }
            //isLinkなら全部
            //var buf = raws[key].Clone();

            //buf.SubSelf(Offset).DivSelf(Depth);

            //Images[key] = buf.ToMono();
            //RaisePropertyChanged(nameof(Images));
        }

        public void Developing(string id)
        {
            Output($"Developing {id}");

            var buf = raws[id].Clone();

            buf.SubSelf(Offset).MulSelf((float)(255.0/Depth));
            Images[id] =
                Color == ColorType.Mono ? buf.ToMono() :
                Color == ColorType.GR ? buf.ToColorGR() :
                Color == ColorType.RG ? buf.ToColorRG() :
                Color == ColorType.GB ? buf.ToColorGB() :
                Color == ColorType.BG ? buf.ToColorBG() : buf.ToMono();
            
            RaisePropertyChanged(nameof(Images));
        }

    }

    public class Globals
    {
        public Pixel<float> raw;
        public Rect Rect;
    }

}

//var buf = new WriteableBitmap(new BitmapImage(new Uri(
//    @".png",
//    UriKind.Relative)));