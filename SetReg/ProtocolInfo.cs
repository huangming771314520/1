using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace SetReg
{
    /// <summary>
    /// URL 协议信息
    /// </summary>
    [Serializable]
    public class ProtocolInfo
    {
        /// <summary>
        /// 注册表中的节点名称
        /// </summary>
        [XmlAttribute]
        public string NodeName { get; set; }

        /// <summary>
        /// 程序名称
        /// </summary>
        [XmlAttribute]
        public string ProgramName { get; set; }

        /// <summary>
        /// 协议名称
        /// </summary>
        [XmlAttribute]
        public string ProtocolName { get; set; }

        /// <summary>
        /// 获取文件中的协议配置信息
        /// </summary>
        /// <param name="filepath">文件名称</param>
        /// <returns>协议信息</returns>
        public static List<ProtocolInfo> GetProtocolInfo(string filepath)
        {
            List<ProtocolInfo> protocolInfos = new List<ProtocolInfo>();
            using (FileStream stream = new FileStream(filepath, FileMode.Open, FileAccess.Read))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<ProtocolInfo>));
                protocolInfos = serializer.Deserialize(stream) as List<ProtocolInfo>;
            }
            return protocolInfos;
        }

        /// <summary>
        /// 保存协议信息到配置文件中
        /// </summary>
        /// <param name="protocolInfos">协议信息</param>
        public static void Save(List<ProtocolInfo> protocolInfos)
        {
            string file = "ProtocolInfo.xml";

            XmlSerializer serializer = new XmlSerializer(typeof(List<ProtocolInfo>));

            try
            {
                using (FileStream stream = new FileStream(file, FileMode.Create, FileAccess.Write))
                {
                    serializer.Serialize(stream, protocolInfos);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
        }
    }
}
