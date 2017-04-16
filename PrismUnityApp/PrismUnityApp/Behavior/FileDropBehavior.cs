using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace PrismUnityApp.Behavior
{
    public class FileDropBehavior : Behavior<Grid>
    {

        /// <summary>
        /// ドロップされたときに実行するコマンドを取得または設定します。
        /// </summary>
        public ICommand TargetCommand
        {
            get { return (ICommand)GetValue(TargetCommandProperty); }
            set { SetValue(TargetCommandProperty, value); }
        }
        public static readonly DependencyProperty TargetCommandProperty = DependencyProperty.Register(
                "TargetCommand", 
                typeof(ICommand), 
                typeof(FileDropBehavior), 
                new PropertyMetadata(null));

        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.DragEnter += OnDragEnter;
            this.AssociatedObject.DragLeave += OnDragLeave;
            this.AssociatedObject.Drop += OnDrop;
        }
        protected override void OnDetaching()
        {
            this.AssociatedObject.DragEnter -= OnDragEnter;
            this.AssociatedObject.DragLeave -= OnDragLeave;
            this.AssociatedObject.Drop -= OnDrop;
            base.OnDetaching();
        }

        private void OnDragEnter(object sender, DragEventArgs e)
        {
            var backgroundBorder = sender as Border;
            //ChangeBrush(backgroundBorder, DescriptionTextBlock);
        }

        private void OnDragLeave(object sender, DragEventArgs e)
        {
            var backgroundBorder = sender as Border;
            //RestoreBrush(backgroundBorder, DescriptionTextBlock);
        }

        private void OnDrop(object sender, DragEventArgs e)
        {
            var backgroundBorder = sender as Border;
            //RestoreBrush(backgroundBorder, DescriptionTextBlock);

            NotifyDrop(e.Data);
        }


        /// <summary>
        /// ドロップされたファイルを通知します。
        /// </summary>
        /// <param name="data">ドロップイベントのデータ</param>
        private void NotifyDrop(IDataObject data)
        {
            if (TargetCommand == null) throw new InvalidOperationException("TargetCommand is null.");
            //TargetCommand.Execute(data);

            var files = data.GetData(DataFormats.FileDrop);
            if (files != null) TargetCommand.Execute(files);
        }

    }
}
