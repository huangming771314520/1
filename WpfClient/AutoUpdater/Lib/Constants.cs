using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Ezhu.AutoUpdater
{
    public class Constants
    {
        private static string remoteUrl;

        public static string RemoteUrl
        {
            get
            {
                if (string.IsNullOrWhiteSpace(remoteUrl))
                {
                    remoteUrl = getXMLAttr("RootLocal/ServerDownUrl", "url");
                }
                return remoteUrl;
            }
            set
            {
                remoteUrl = value;
            }
        }


        private static string getXMLAttr(string xPathNode, string attrName)
        {
            string Path = AppDomain.CurrentDomain.BaseDirectory + "LocalVersion.xml";
            XmlDocument doc = new XmlDocument();
            XmlReaderSettings settings = new XmlReaderSettings
            {
                IgnoreComments = true//忽略文档里面的注释
            };
            XmlReader reader = XmlReader.Create(Path, settings);
            doc.Load(reader);

            return ((XmlElement)doc.SelectSingleNode(xPathNode)).GetAttribute(attrName).ToString();
        }


    }
}