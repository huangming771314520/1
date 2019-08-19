using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SetReg
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var a1 = Environment.CurrentDirectory;
                Console.WriteLine(a1);
                List<ProtocolInfo> protocalInfos = ProtocolInfo.GetProtocolInfo(string.Format("{0}\\ProtocolInfo.xml", Environment.CurrentDirectory));
                if (protocalInfos == null || protocalInfos.Count == 0)
                    Console.WriteLine("未获取协议的配置信息！请确保配置文件 ProtocolInfo.xml 在当前目录下。");
                string nodeName = protocalInfos[0].NodeName;
                string programName = protocalInfos[0].ProgramName;
                string programFullPath = string.Format("{0}\\{1}", Environment.CurrentDirectory, programName);

                RegistryKey key = Registry.ClassesRoot;
                //if (!key.GetSubKeyNames().Contains(nodeName))//判断是否存在该注册表，不存在则添加注册表
                //{
                    RegistryKey software = key.CreateSubKey(protocalInfos[0].NodeName);
                    software.SetValue("URL Protocol", programFullPath);
                    software.SetValue("", protocalInfos[0].ProtocolName);

                    RegistryKey softwareDefaultIcon = software.CreateSubKey("DefaultIcon");
                    softwareDefaultIcon.SetValue("", string.Format("{0},{1}", programFullPath, 1));

                    RegistryKey softwareShell = software.CreateSubKey("shell");
                    softwareShell = softwareShell.CreateSubKey("open");
                    softwareShell = softwareShell.CreateSubKey("command");
                    softwareShell.SetValue("", string.Format("\"{0}\" \"%{1}\"", programFullPath, 1));
                //}
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            Console.ReadLine();
        }
    }
}
