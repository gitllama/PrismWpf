using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows.Input;
using System.Xml;
using System.Linq;
using Xceed.Wpf.AvalonDock;
using Xceed.Wpf.AvalonDock.Layout;
using Xceed.Wpf.AvalonDock.Layout.Serialization;
using Prism.Mvvm;
using System.Windows;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using System.Windows.Data;

namespace AvalonDockUtil
{


    public abstract class WorkspaceBase : BindableBase
    {
        protected abstract void InitializeTools();

        //protected DocumentContent GetDocumentByContentId(String contentId)
        //{
        //    return Documents.FirstOrDefault(d => d.ContentId == contentId);
        //}

        public void LoadLayout(DockingManager dockManager)
        {
            InitializeTools();
            return;
            // backup default layout
            using (var ms = new MemoryStream())
            {
                var serializer = new XmlLayoutSerializer(dockManager);
                serializer.Serialize(ms);
                m_defaultLayout = ms.ToArray();
            }

            // load file
            Byte[] bytes;
            try
            {
                bytes = System.IO.File.ReadAllBytes(LayoutFile);
            }
            catch (FileNotFoundException ex)
            {
                return;
            }

            // restore layout
            LoadLayoutFromBytes(dockManager, bytes);
        }

        String LayoutFile
        {
            get
            {
                return System.IO.Path.ChangeExtension(
                    Environment.GetCommandLineArgs()[0]
                    , ".AvalonDock.config"
                    );
            }
        }
        Byte[] m_defaultLayout;

        public void DefaultLayout(DockingManager dockManager)
        {
            LoadLayoutFromBytes(dockManager, m_defaultLayout);
        }



        bool LoadLayoutFromBytes(DockingManager dockManager, Byte[] bytes)
        {
            InitializeTools();

            var serializer = new XmlLayoutSerializer(dockManager);

            serializer.LayoutSerializationCallback += MatchLayoutContent;

            try
            {
                using (var stream = new MemoryStream(bytes))
                {
                    serializer.Deserialize(stream);
                }

                RestoreDocumentsFromBytes(bytes);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        void MatchLayoutContent(object o, LayoutSerializationCallbackEventArgs e)
        {
            //var contentId = e.Model.ContentId;

            //if (e.Model is LayoutAnchorable)
            //{
            //    // Tool Windows
            //    foreach (var tool in Tools)
            //    {
            //        if (tool.ContentId == contentId)
            //        {
            //            e.Content = tool;
            //            return;
            //        }
            //    }

            //    // Unknown
            //    ErrorDialog(new Exception("unknown ContentID: " + contentId));
            //    return;
            //}

            //if (e.Model is LayoutDocument)
            //{
            //    // load済みを探す
            //    foreach (var document in Documents)
            //    {
            //        if (document.ContentId == contentId)
            //        {
            //            e.Content = document;
            //            return;
            //        }
            //    }

            //    // Document
            //    {
            //        var document = NewDocument();
            //        Documents.Add(document);
            //        document.ContentId = contentId;
            //        e.Content = document;
            //    }

            //    return;
            //}

            //ErrorDialog(new Exception("Unknown Model: " + e.Model.GetType()));
            //return;
        }

        protected abstract void ModifySerializedXml(XmlDocument doc);
        protected abstract void RestoreDocumentsFromBytes(Byte[] bytes);

        public void SaveLayout(DockingManager dockManager)
        {
            var serializer = new XmlLayoutSerializer(dockManager);
            var doc = new XmlDocument();
            using (var stream = new MemoryStream())
            {
                serializer.Serialize(stream);
                stream.Position = 0;
                doc.Load(stream);
            }

            ModifySerializedXml(doc);

            using (var stream = new FileStream(LayoutFile, FileMode.Create))
            {
                doc.Save(stream);
            }
        }

        public InteractionRequest<INotification> NotificationRequest { get; set; } = new InteractionRequest<INotification>();
        protected void InfoDialog(String message)
        {
            NotificationRequest.Raise(new Notification { Content = message, Title = "Info" });
        }

        protected void ErrorDialog(Exception ex)
        {
            NotificationRequest.Raise(new Notification { Content = ex.Message, Title = "Error" });
        }

        //    protected bool ConfirmDialog(String text, String title)
        //    {
        //        var message = new ConfirmationMessage(text, title
        //                    , MessageBoxImage.Question, MessageBoxButton.YesNo, "Confirm");
        //        Messenger.Raise(message);
        //        return message.Response.HasValue && message.Response.Value;
        //    }


        //    #region OpeningFileSelectionMessage
        //    protected String[] OpenDialog(String title, bool multiSelect = false)
        //    {
        //        return OpenDialog(title, "すべてのファイル(*.*)|*.*", multiSelect);
        //    }
        //    protected String[] OpenDialog(String title, String filter, bool multiSelect)
        //    {
        //        var message = new OpeningFileSelectionMessage("Open")
        //        {
        //            Title = title,
        //            Filter = filter,
        //            MultiSelect = multiSelect,
        //        };
        //        Messenger.Raise(message);
        //        return message.Response;
        //    }
        //    #endregion

        //    #region SavingFileSelectionMessage
        //    protected String SaveDialog(String title, string filename)
        //    {
        //        var message = new SavingFileSelectionMessage("Save")
        //        {
        //            Title = title,
        //            FileName = String.IsNullOrEmpty(filename) ? "list.txt" : filename,
        //        };
        //        Messenger.Raise(message);
        //        return message.Response != null ? message.Response[0] : null;
        //    }
        //    #endregion
        //}
    }

    public class DocumentContent : BindableBase
    {
        private Guid _Guid;
        [ContentProperty, ReadOnly(true)]
        public Guid Guid { get => _Guid; set => SetProperty(ref _Guid, value,()=>RaisePropertyChanged("ContentId")); }
        [ContentProperty, ReadOnly(true)]
        public string ContentId { get => _Guid.ToString(); set => Guid = Guid.Parse(value); }

        private string _Title;
        [ContentProperty, ReadOnly(true)]
        public string Title { get => _Title; set => SetProperty(ref _Title, value); }

        public DocumentContent() : this(Guid.NewGuid().ToString())
        {
            Title = "Untitled";
        }

        public DocumentContent(String contentId)
        {
            _Guid = Guid.Parse(contentId);
        }

        public virtual void Close() => throw new Exception();
    }

    public class ToolContent : BindableBase
    {
        private string _ContentId;
        [ContentProperty]
        public string ContentId { get => _ContentId; set => SetProperty(ref _ContentId, value);}

        private string _Title;
        [ContentProperty]
        public string Title { get => _Title; set => SetProperty(ref _Title, value); }

        private Visibility _Visibility = Visibility.Hidden;
        [ContentProperty(BindingMode = BindingMode.TwoWay)]
        public Visibility Visibility { get => _Visibility; set => SetProperty(ref _Visibility, value); }

        public bool IsVisible
        {
            get => _Visibility == System.Windows.Visibility.Visible;
            set
            {
                if (IsVisible == value) return;
                Visibility = value ? Visibility.Visible : Visibility.Hidden;
                RaisePropertyChanged("IsVisible");
            }
        }

        public ToolContent(String contentId, String title = null)
        {
            _ContentId = contentId;
            Title = String.IsNullOrEmpty(title) ? contentId : title;
        }
    }
}
