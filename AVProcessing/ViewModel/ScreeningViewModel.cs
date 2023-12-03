using AVProcessing.Common;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVProcessing.ViewModel
{
    public class ScreeningViewModel:ViewModelBase
    {
        CustomFileOptions CustomOption;
        public ScreeningViewModel()
        {
            CustomOption = new CustomFileOptions();
            SelectSourceCommand = new RelayCommand(Screen_OpenFileDialog);
        }

        private string playSource = "";

        public string PlaySource
        {
            get { return playSource; }
            set { playSource = value; RaisePropertyChanged(); }
        }

        // 视频时长
        private string movieTime;

        public string MovieTime
        {
            get { return movieTime; }
            set { movieTime = value;RaisePropertyChanged(); }
        }


        public void Screen_OpenFileDialog()
        {
            OpenFileDialog file = CustomOption.OpenFileDialog_p("视频文件|*.mp4;*.flv;*.ts;*.m3u8;*.mkv;");

            if (file == null) return;

            PlaySource = file.FileName;

            MovieTime = CustomOption.GetMediaTimeLen(file.FileName);
        }

        public RelayCommand SelectSourceCommand { get; set; }
    }
}
