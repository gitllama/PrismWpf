using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace PrismAutofac.ViewModels
{
	public class TreeUserControlViewModel : BindableBase
	{
        public ObservableCollection<TreeSource> TreeRoot { get; set; }

        public TreeUserControlViewModel()
        {
            TreeRoot = new ObservableCollection<TreeSource>();
            var item1 = new TreeSource() { Text = "Item1", IsExpanded = true };
            var item11 = new TreeSource() { Text = "Item1-1", IsExpanded = true };
            var item12 = new TreeSource() { Text = "Item1-2", IsExpanded = true };
            var item2 = new TreeSource() { Text = "Item2", IsExpanded = false };
            var item21 = new TreeSource() { Text = "Item2-1", IsExpanded = true };
            TreeRoot.Add(item1);
            TreeRoot.Add(item2);
            item1.Add(item11);
            item1.Add(item12);
            item2.Add(item21);
        }
    }

    public class TreeSource : INotifyPropertyChanged
    {
        private bool _IsExpanded = true;
        private string _Text = "";
        private TreeSource _Parent = null;
        private ObservableCollection<TreeSource> _Children = null;


        public bool IsExpanded
        {
            get { return _IsExpanded; }
            set { _IsExpanded = value; OnPropertyChanged("IsExpanded"); }
        }

        public string Text
        {
            get { return _Text; }
            set { _Text = value; OnPropertyChanged("Text"); }
        }

        public TreeSource Parent
        {
            get { return _Parent; }
            set { _Parent = value; OnPropertyChanged("Parent"); }
        }

        public ObservableCollection<TreeSource> Children
        {
            get { return _Children; }
            set { _Children = value; OnPropertyChanged("Childen"); }
        }



        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name)
        {
            if (null == this.PropertyChanged) return;
            this.PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        public void Add(TreeSource child)
        {
            if (null == Children) Children = new ObservableCollection<TreeSource>();
            child.Parent = this;
            Children.Add(child);
        }
    }
}
