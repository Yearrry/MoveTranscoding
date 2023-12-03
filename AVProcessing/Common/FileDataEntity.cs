using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVProcessing.Common
{
    public class FileDataEntity:ViewModelBase
    {
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value;RaisePropertyChanged(); }
        }

        private string size;

        public string Size
        {
            get { return size; }
            set { size = value; RaisePropertyChanged(); }
        }
    }
}
