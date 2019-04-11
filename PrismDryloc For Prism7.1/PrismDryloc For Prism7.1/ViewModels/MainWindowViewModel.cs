using Prism.Mvvm;
using Prism.Regions;
using System;

namespace PrismDryloc_For_Prism7._1.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Prism Application";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public MainWindowViewModel(Models.ModelBase model, IRegionManager regionManager)
        {
            Console.Write("in VM : ");
            Console.WriteLine(model.a);
        }
    }
}
