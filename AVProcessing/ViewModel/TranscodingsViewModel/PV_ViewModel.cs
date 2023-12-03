using AVProcessing.Common;
using AVProcessing.Common.DosOptions;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;

namespace AVProcessing.ViewModel.TranscodingsViewModel
{
    public class PV_ViewModel:ViewModelBase
    {
        //视频转换 通用 不改变编码
        string movieChange = "             ffmpeg -i {0} -vcodec copy -acodec copy {1}";
        //m4s 音视频合成
        string movieCompound_m4s = "                ffmpeg -i {0} -i {1} -vcodec copy -acodec copy -f {2} {3}";

        DosHelper dosHelper;

        CustomFileOptions customOption;
        public PV_ViewModel()
        {
            dosHelper = new DosHelper();
            customOption = new CustomFileOptions();

            OpenDirectory = new RelayCommand(PV_OpenDirectoryDialog);
            OpenFile = new RelayCommand(PV_OpenFileDialog);
            OpenVideo_m4s = new RelayCommand(PV_OpenVideo_M4s);
            OpenRadio_m4s = new RelayCommand(PV_OpenAudio_M4s);
            OpenDirectory_m4s = new RelayCommand(PV_Openm4sDirectory);
            CheckRelativePath_m4s = new RelayCommand(Checkm4sRelativePath);
            SingleFileChange = new RelayCommand<string>((x) => { PV_SingleFormatChange(x); });
            DirectoryFileChange = new RelayCommand<string>((x) => { PV_DirectoryFormatChange(x); });
            Singlem4sCompound = new RelayCommand<string>((x) => { SingleCompound_m4s(x); });
            Directorym4sCompound = new RelayCommand<string>((x) => { DirectoryCompound_m4s(x); });

            ResultFile = new ObservableCollection<FileDataEntity>();
            M4s_resultFile = new ObservableCollection<FileDataEntity>();
            av_m4s = new List<(string, string)>();

        }

        private string oldFileName;

        public string OldFileName
        {
            get { return oldFileName; }
            set { oldFileName = value; RaisePropertyChanged(); }
        }

        private string newFileName;

        public string NewFileName
        {
            get { return newFileName; }
            set { newFileName = value; RaisePropertyChanged(); }
        }

        #region 单个m4s 合成

        private string videoName_m4s;

        public string VideoName_m4s
        {
            get { return videoName_m4s; }
            set { videoName_m4s = value; RaisePropertyChanged(); }
        }

        private string audioName_m4s;

        public string AudioName_m4s
        {
            get { return audioName_m4s; }
            set { audioName_m4s = value; RaisePropertyChanged(); }
        }

        private string singleCompoundPath_m4s;

        public string SingleCompoundPath_m4s
        {
            get { return singleCompoundPath_m4s; }
            set { singleCompoundPath_m4s = value; RaisePropertyChanged(); }
        }


        #endregion

        #region 多个 m4s 合成

        private string av_DirectoryRelativepath;
        public string Av_DirectoryRelativepath
        {
            get { return av_DirectoryRelativepath; }
            set { av_DirectoryRelativepath = value; RaisePropertyChanged(); }
        }


        private string oldm4sDirectoryPath;
        public string Oldm4sDirectoryPath
        {
            get { return oldm4sDirectoryPath; }
            set { oldm4sDirectoryPath = value; RaisePropertyChanged(); }
        }


        private string newDirectoryPath_m4s;
        public string NewDirectoryPath_m4s
        {
            get { return newDirectoryPath_m4s; }
            set { newDirectoryPath_m4s = value; RaisePropertyChanged(); }
        }


        private string newFileName_m4s;
        public string NewFileName_m4s
        {
            get { return newFileName_m4s; }
            set { newFileName_m4s = value; RaisePropertyChanged(); }
        }

        #endregion



        private string oldDirectoryName;

        public string OldDirectoryName
        {
            get { return oldDirectoryName; }
            set { oldDirectoryName = value; RaisePropertyChanged(); }
        }

        private string oldDirectoryDetailName;

        public string OldDirectoryDetailName
        {
            get { return oldDirectoryDetailName;}
            set { oldDirectoryDetailName = value; RaisePropertyChanged(); }
        }

        //有几个m4s 外文件  提示字符串
        private string m4slistDirectoryCountStr;

        public string M4slistDirectoryCountStr
        {
            get { return m4slistDirectoryCountStr; }
            set { m4slistDirectoryCountStr = value; RaisePropertyChanged(); }
        }

        //有几个 video.m4s、radio.m4s 提示字符串
        private string avm4sCountStr;

        public string Avm4sCountStr
        {
            get { return avm4sCountStr; }
            set { avm4sCountStr = value; RaisePropertyChanged(); }
        }

        //多视频转化新目录
        private string newDirectory;

        public string NewDirectory
        {
            get { return newDirectory; }
            set { newDirectory = value; RaisePropertyChanged(); }
        }

        //单视频转化新目录
        private string newFilename_Directory;

        public string NewFilename_Directory
        {
            get { return newFilename_Directory; }
            set { newFilename_Directory = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<FileDataEntity> resultFile;

        public ObservableCollection<FileDataEntity> ResultFile
        {
            get { return resultFile; }
            set { resultFile = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<FileDataEntity> m4s_resultFile;

        public ObservableCollection<FileDataEntity> M4s_resultFile
        {
            get { return m4s_resultFile; }
            set { m4s_resultFile = value; RaisePropertyChanged(); }
        }

        public FileInfo[] DirectoryArray { get; set; } = new FileInfo[] { };

        public DirectoryInfo[] m4sDirectoryArray { get; set; }

        public List<(string, string)> av_m4s { get; set; } 


        #region 视频转化
        public async void PV_OpenDirectoryDialog()
        {
            string directoryname = await customOption.OpenDirectory_p();

            if (string.IsNullOrWhiteSpace(directoryname)) return;

            OldDirectoryName = directoryname;

            DirectoryInfo videoDirectory = new DirectoryInfo(directoryname);

            FileInfo[] videoDirectory_f = videoDirectory.GetFiles();

            DirectoryArray = videoDirectory_f;

            OldDirectoryDetailName += "找到" + videoDirectory_f.Length + "个 " + videoDirectory_f[0].Extension + " 文件" + "\r\n \r\n";

           await  Task.Run(async() =>
            {
                for (int i = 0; i < videoDirectory_f.Length; i++)
                {
                    OldDirectoryDetailName += videoDirectory_f[i].FullName + "\r\n";
                    await Task.Delay(100);
                }
            });
        }
        public void PV_OpenFileDialog()
        {
            OpenFileDialog file = customOption.OpenFileDialog_p("视频文件|*.mp4;*.flv;*.ts;*.m3u8;*.mkv;");

            if (file == null) return;

            OldFileName = file.FileName;
        }
        public void PV_OpenVideo_M4s()
        {
            OpenFileDialog file = customOption.OpenFileDialog_p("视频文件|*.m4s;");

            if (file == null) return;

            VideoName_m4s = file.FileName;
        }
        public void PV_OpenAudio_M4s()
        {
            OpenFileDialog file = customOption.OpenFileDialog_p("音频文件|*.m4s;");

            if (file == null) return;

            AudioName_m4s = file.FileName;
        }
        public async void PV_Openm4sDirectory()
        {
            string m4sDirectorypath = await customOption.OpenDirectory_p();

            if (m4sDirectorypath == null) return;

            Oldm4sDirectoryPath = m4sDirectorypath;

            DirectoryInfo m4sDirectory = new DirectoryInfo(m4sDirectorypath);

            DirectoryInfo m4sParentDirectory = new DirectoryInfo(m4sDirectory.Parent.FullName);

            DirectoryInfo[] m4sDirectory_list = m4sParentDirectory.GetDirectories();

            m4sDirectoryArray = m4sDirectory_list;

            M4slistDirectoryCountStr = "找到 " + m4sDirectory_list.Length + " 个文件";
        }
        public async void PV_SingleFormatChange(string format)
        {
            string newFilepath;

            if (!NewFileName.Contains(format)) newFilepath = string.Format("{0}.{1}", NewFileName, format);
            else newFilepath = NewFileName;

            await SingleFormatChange(string.Format(movieChange, OldFileName, newFilepath),newFilepath, ResultFile); 
        }
        public async void PV_DirectoryFormatChange(string format)
        {
            ResultFile.Clear();

            if (!NewFilename_Directory.Contains(format)) NewFilename_Directory = string.Format("{0}.{1}", NewFilename_Directory, format);

            DirectoryInfo _newDirectory = new DirectoryInfo(NewDirectory);

            if (!_newDirectory.Exists) Directory.CreateDirectory(NewDirectory);

            int count = 1;

            foreach (FileInfo item in DirectoryArray)
            {
                string newname = NewFilename_Directory.Replace(string.Format(".{0}",format), string.Format("_{0}.{1}", count, format));

                count += 1;

                await SingleFormatChange(string.Format(movieChange, item.FullName, string.Format("{0}\\{1}", NewDirectory, newname)), 
                    string.Format("{0}\\{1}", NewDirectory, newname),
                    ResultFile);
            }
        }
        #endregion 

        #region m4s音视频合成

        public async void SingleCompound_m4s(string format)
        {
            if (!SingleCompoundPath_m4s.Contains(format)) SingleCompoundPath_m4s = string.Format("{0}.{1}", SingleCompoundPath_m4s, format);

            string ffmpegCommand = string.Format(movieCompound_m4s, VideoName_m4s, AudioName_m4s, format,SingleCompoundPath_m4s);

            await SingleFormatChange(ffmpegCommand, SingleCompoundPath_m4s, M4s_resultFile);
        }

        public async void DirectoryCompound_m4s(string format)
        {
            M4s_resultFile.Clear();

            if (!NewFileName_m4s.Contains(format)) NewFileName_m4s = string.Format("{0}.{1}", NewFileName_m4s, format);

            DirectoryInfo _newDirectory = new DirectoryInfo(NewDirectoryPath_m4s);

            if (!_newDirectory.Exists) Directory.CreateDirectory(NewDirectoryPath_m4s);

            int count = 1;

            foreach ((string,string) item in av_m4s)
            {
                string newname = NewFileName_m4s.Replace(string.Format(".{0}", format), string.Format("_{0}.{1}", count, format));

                count += 1;

                string newpath = string.Format("{0}\\{1}", NewDirectoryPath_m4s, newname);

                string ffmpegCommand = string.Format(movieCompound_m4s, item.Item1, item.Item2, format, newpath);

                await SingleFormatChange(ffmpegCommand, newpath, M4s_resultFile);
            }
        }

        #endregion

        public async Task SingleFormatChange(string ffmpegCommand,string newPath, ObservableCollection<FileDataEntity> result)
        {
            if (await dosHelper.WriteCommand(ffmpegCommand))
            {
                await Task.Delay(500);

                FileInfo newFile = new FileInfo(newPath);

                if (!newFile.Exists) return;

                result.Add(new FileDataEntity
                {
                    Name = newFile.Name,
                    Size = string.Format("{0}mb", (newFile.Length / (1024 * 1024)).ToString())
                });
            }
        }
        public void Checkm4sRelativePath()
        {
            if (string.IsNullOrWhiteSpace(Av_DirectoryRelativepath)) return;

            if (m4sDirectoryArray.Length == 0 || m4sDirectoryArray == null) return;

            int canSynthetic = 0;

            foreach (DirectoryInfo item in m4sDirectoryArray)
            {
                string audioFilePath = string.Format(@"{0}\{1}\audio.m4s",item.FullName,Av_DirectoryRelativepath);
                string videoFilePath = string.Format(@"{0}\{1}\video.m4s",item.FullName,Av_DirectoryRelativepath);

                FileInfo audioInfo = new FileInfo(audioFilePath);
                FileInfo videoInfo = new FileInfo(videoFilePath);

                if (audioInfo.Exists && videoInfo.Exists)
                {
                    canSynthetic += 1;
                    av_m4s.Add((audioInfo.FullName, videoInfo.FullName));
                }
            }

            Avm4sCountStr = "可合成 " + canSynthetic + " 个视频";
        }
        
        public RelayCommand OpenDirectory { get; set; }
        public RelayCommand OpenFile { get; set; }
        public RelayCommand OpenVideo_m4s { get; set; }
        public RelayCommand OpenRadio_m4s { get; set; }
        public RelayCommand OpenDirectory_m4s { get; set; }
        public RelayCommand CheckRelativePath_m4s { get; set; }
        public RelayCommand<string> SingleFileChange { get; set; }
        public RelayCommand<string> DirectoryFileChange { get; set; }
        public RelayCommand<string> Singlem4sCompound { get; set; }
        public RelayCommand<string> Directorym4sCompound { get; set; }
    }
}
