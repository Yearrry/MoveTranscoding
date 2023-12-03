using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Threading.Tasks;

namespace AVProcessing.Common
{
    public class CustomFileOptions
    {
        public OpenFileDialog OpenFileDialog_p(string filter, string openpath = "")
        {
            OpenFileDialog fileDialog = new OpenFileDialog();

            bool? code = true;

            fileDialog.Filter = filter;

            if (!string.IsNullOrEmpty(openpath))
            {
                fileDialog.InitialDirectory = openpath;
            }

            if (fileDialog.ShowDialog() != code) return null;

            return fileDialog;
        }
        public Task<string> OpenDirectory_p()
        {
            CommonOpenFileDialog fileDialog = new CommonOpenFileDialog();
            fileDialog.IsFolderPicker = true;
            CommonFileDialogResult openCode = fileDialog.ShowDialog();

            if (Convert.ToInt32(openCode) != 1) return Task.FromResult("");

            return Task.FromResult(fileDialog.FileName);
        }

        /// <summary>
        /// 获取视频文件的播放时长
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string GetMediaTimeLen(string path)
        {
            try
            {

                Shell32.Shell shell = new Shell32.ShellClass();
                //文件路径
                Shell32.Folder folder = shell.NameSpace(path.Substring(0, path.LastIndexOf("\\")));
                //文件名称
                Shell32.FolderItem folderitem = folder.ParseName(path.Substring(path.LastIndexOf("\\") + 1));

                string showMsg = "";

                //在win2003上应使用folder.getdetailsof(folderitem, 21) ，
                //而在vista上应使用folder.getdetailsof(folderitem, 27) ，主要是因为不同系统下文件属性索引顺序不同造成。

                showMsg = folder.GetDetailsOf(folderitem, 27);

                return showMsg;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
    }
}
