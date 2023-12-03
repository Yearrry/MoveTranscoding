using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AVProcessing.ViewModel
{
    public class TranscodingViewModel:ViewModelBase
    {
        public TranscodingViewModel()
        {
            Transcoding_ChangedContent = new RelayCommand<string>((x) => RegisteredContent(x));
        }

        private FrameworkElement content;

        public FrameworkElement Content
        {
            get { return content; }
            set { content = value; RaisePropertyChanged(); }
        }

        public RelayCommand<string> Transcoding_ChangedContent { get; set; }

        public void RegisteredContent(string controlname)
        {
            Type type = Type.GetType("AVProcessing.UserControls.Transcodings." + controlname);

            ConstructorInfo cti = type.GetConstructor(Type.EmptyTypes);

            Content = (FrameworkElement)cti.Invoke(null);
        }
    }
}
