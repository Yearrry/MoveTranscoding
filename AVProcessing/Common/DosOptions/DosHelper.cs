using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AVProcessing.Common.DosOptions
{
    public class DosHelper
    {
        Process p;

        public string ErrorResult { get; set; }
        public string OutputResult { get; set; }
        public string Writestr { get; set; }


        public DosHelper()
        {
            //p = new Process();

            //DosStart();
        }

        ~DosHelper()
        {
            //if (p.HasExited) return;

            //p.WaitForExit();
            //p.Close();
            //p.Kill();
        }

        public void DosStart()
        {
            //设置要启动的应用程序
            p.StartInfo.FileName = "cmd.exe";

            p.StartInfo.Arguments = "";
            //是否使用操作系统shell启动
            p.StartInfo.UseShellExecute = false;
            // 接受来自调用程序的输入信息
            p.StartInfo.RedirectStandardInput = true;
            //输出信息
            p.StartInfo.RedirectStandardOutput = true;
            // 输出错误
            p.StartInfo.RedirectStandardError = true;
            //不显示程序窗口
            p.StartInfo.CreateNoWindow = true;


            p.Start();
        }

        public Task<bool> WriteCommand(string command)
        {
            return Task.Run(async () =>
            {
                await write(command);
                return true;
            });
        }

        public Task write(string command)
        {
            Writestr += command + "\r\n";
           
            using(Process p = new Process())
            {
                //设置要启动的应用程序
                p.StartInfo.FileName = "cmd.exe";

                p.StartInfo.Arguments = "";
                //是否使用操作系统shell启动
                p.StartInfo.UseShellExecute = false;
                // 接受来自调用程序的输入信息
                p.StartInfo.RedirectStandardInput = true;
                //输出信息
                p.StartInfo.RedirectStandardOutput = true;
                // 输出错误
                p.StartInfo.RedirectStandardError = true;
                //不显示程序窗口
                p.StartInfo.CreateNoWindow = true;


                p.Start();

                p.StandardInput.WriteLine(command);
            }

            return Task.CompletedTask;
        }

        public async Task<bool> WriteCommandGetErrorResult(string command)
        {
            p.StandardInput.WriteLine(command);

            return await Task.Run(async () =>
            {
                StreamReader error = new StreamReader(p.StandardError.BaseStream, Encoding.UTF8);

                if (error.EndOfStream) return false;

                while (error.EndOfStream)
                {
                    try
                    {
                        ErrorResult += error.ReadLine() + "\r\n";
                        await Task.Delay(50);
                    }
                    catch (System.Exception e)
                    {

                        throw e;
                    }
                }
                return true;
            });
        }

        public Task<bool> GetErrorResult()
        {
            return Task.Run(() =>
            {
                StreamReader error = new StreamReader(p.StandardError.BaseStream, Encoding.Default);

                if (error.EndOfStream) return false;

                while (error.EndOfStream)
                {
                    try
                    {
                        ErrorResult += error.ReadLine() + "\r\n";
                    }
                    catch (System.Exception e)
                    {

                        throw e;
                    }
                }
                return true;
            });
        }

        public Task<bool> GetOutputResult()
        {
            return Task.Run(async () =>
            {
                StreamReader error = new StreamReader(p.StandardOutput.BaseStream, Encoding.Default);
                if (error.EndOfStream) return false;

                while (error.EndOfStream)
                {
                    try
                    {
                        OutputResult += error.ReadLine() + "\r\n";
                        await Task.Delay(50);
                    }
                    catch (System.Exception e)
                    {

                        throw e;
                    }
                }
                OutputResult += "\r\n\r\n";
                return true;
            });
        }

        public Task CloseDos()
        {
            p.WaitForExit();

            p.Close();

            p.Kill();
            return Task.CompletedTask;
        }
    }
}
