using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Reflection;
using System.Windows;

namespace AVProcessing.ViewModel
{
    
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            ChangedContent = new RelayCommand<string>((x) => RegisteredUserControl(x));
        }

        private FrameworkElement content;

        public FrameworkElement Content
        {
            get { return content; }
            set { content = value; RaisePropertyChanged(); }
        }

        public RelayCommand<string> ChangedContent { get; set; }

        public void RegisteredUserControl(string controlstr)
        {
            Type type = Type.GetType("AVProcessing.UserControls." + controlstr);
       
            ConstructorInfo cti = type.GetConstructor(Type.EmptyTypes);
            Content = (FrameworkElement)cti.Invoke(null);
        }
    }
}