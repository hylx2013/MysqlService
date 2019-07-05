using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;
using System.IO;
using Newtonsoft.Json.Linq;

namespace Chenxi.Service.Services
{
    public class DataBaseBackUpService
    {
        public static void BackUp()
        {
            try
            {
                Loghelper.WriteLog("开始备份数据库");
                //mysqldump -uroot -p123456 mydb -d > /data/mysqlDump/mydb.sql

                var configStr = AppDomain.CurrentDomain.BaseDirectory + "/Config/DataBaseConfig.json";

                DataBaseConfig configObject;
                using (System.IO.StreamReader file = System.IO.File.OpenText(configStr))
                {

                    using (JsonTextReader reader = new JsonTextReader(file))
                    {
                        JObject o = (JObject)JToken.ReadFrom(reader);
                        configObject = o.ToObject<DataBaseConfig>();
                    }
                }
                if (configObject != null)
                {
                    string str = $"mysqldump ";
                    //如果没有配置数据库列表，则备份所有数据库
                    if (configObject.DataBase.Count > 0)
                    {
                        str += "--databases";
                        foreach (var item in configObject.DataBase)
                        {
                            str += $" {item}";
                        }
                    }
                    else
                    {
                        str += " -A -t";
                    }
                    str += $" >{configObject.BackPath}{DateTime.Now.ToString("yyyyMMddHHmmss")}.sql";


                    System.Diagnostics.Process p = new System.Diagnostics.Process();
                    p.StartInfo.FileName = "cmd.exe";
                    p.StartInfo.UseShellExecute = false;    //是否使用操作系统shell启动
                    p.StartInfo.RedirectStandardInput = true;//接受来自调用程序的输入信息
                    p.StartInfo.RedirectStandardOutput = true;//由调用程序获取输出信息
                    p.StartInfo.RedirectStandardError = true;//重定向标准错误输出
                    p.StartInfo.CreateNoWindow = true;//不显示程序窗口
                    p.Start();//启动程序

                    //向cmd窗口发送输入信息
                    p.StandardInput.WriteLine(str + "&exit");

                    p.StandardInput.AutoFlush = true;
                    //p.StandardInput.WriteLine("exit");
                    //向标准输入写入要执行的命令。这里使用&是批处理命令的符号，表示前面一个命令不管是否执行成功都执行后面(exit)命令，如果不执行exit命令，后面调用ReadToEnd()方法会假死
                    //同类的符号还有&&和||前者表示必须前一个命令执行成功才会执行后面的命令，后者表示必须前一个命令执行失败才会执行后面的命令



                    //获取cmd窗口的输出信息
                    string output = p.StandardOutput.ReadToEnd();

                    p.WaitForExit();//等待程序执行完退出进程
                    p.Close();



                }
            }
            catch (Exception e)
            {

                Loghelper.WriteLog("备份数据库异常",e);
            }
            
        }

    }

    public class DataBaseConfig
    {
        public string ServerAddress { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public string Port { get; set; }

        public List<string> DataBase { get; set; }

        public string BackPath { get; set; }
    }
}
