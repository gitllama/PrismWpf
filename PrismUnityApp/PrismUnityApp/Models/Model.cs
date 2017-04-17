using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PrismUnityApp.Models
{
    public class Model : BindableBase
    {
        public Model()
        {

        }

        private double _Scale = 1;
        public double Scale
        {
            get { return _Scale; }
            set { SetProperty(ref _Scale, value); }
        }

        private BitmapScalingMode _ScalingMode;
        public BitmapScalingMode ScalingMode
        {
            get { return _ScalingMode; }
            set { SetProperty(ref _ScalingMode, value); }
        }
    }

    public class ImageModel : BindableBase
    {

        private string _FileName;
        public string FileName { get => _FileName; set => SetProperty(ref _FileName, value); }

        public int[] raw;

        private int _Width;
        public int Width { get => _Width; set => SetProperty(ref _Width, value); }

        private int _Height;
        public int Height { get => _Height; set => SetProperty(ref _Height, value); }

        private WriteableBitmap _Bitmap;
        public WriteableBitmap Bitmap { get => _Bitmap; set => SetProperty(ref _Bitmap, value); }

        public void ReadFile(FileTypes t,string value)
        {
            FileName = value;
            Width = t.Width;
            Height = t.Height;

            Bitmap = new WriteableBitmap(new BitmapImage(new Uri(value, UriKind.Relative)));
        }
    }

    public class ReadingImageModel : BindableBase
    {

        public enum Types
        {
            Byte,
            UShort,
            Short,
            Uint,
            Int,
            Single,
            Double,
            BMP
        }

        private Types _Type;
        public Types Type { get => _Type; set => SetProperty(ref _Type, value); }

        private int _OffsetByte;
        public int OffsetByte { get => _OffsetByte; set => SetProperty(ref _OffsetByte, value); }

        private int _Width;
        public int Width { get => _Width; set => SetProperty(ref _Width, value); }

        private int _Height;
        public int Height { get => _Height; set => SetProperty(ref _Height, value); }


        public bool Read(string path)
        {
            if (!System.IO.File.Exists(path)) return false;


            return true;
        }


        public void ReadBMP()
        {

        }

    }

    public static class RegexFile
    {
        public static FileTypes Match(string yaml, string path)
        {
            Dictionary<string, FileTypes> a = new Dictionary<string, FileTypes>();
            using (var sr = new System.IO.StreamReader(yaml))
            {
                var deserializer = new YamlDotNet.Serialization.Deserializer();
                a = deserializer.Deserialize<Dictionary<string, FileTypes>>(sr);
            }

            foreach(var i in a)
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(path, i.Value.Regex))
                {
                    return i.Value;
                }
            }
            return null;
        }
    }

    public class FileTypes
    {
        public enum Types
        {
            Byte,
            UShort,
            Short,
            Uint,
            Int,
            Single,
            Double,
            BMP
        }

        public enum EndianTypes
        {
            Little,
            Big
        }

        public string Regex { get; private set; } = "^.+$";
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int Offset { get; private set; }
        public Types Type { get; private set; } = Types.BMP;
        public EndianTypes Endian { get; private set; } = EndianTypes.Little;

    }
}
