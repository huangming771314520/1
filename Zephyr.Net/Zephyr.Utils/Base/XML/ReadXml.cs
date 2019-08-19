using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Zephyr.Utils
{
    public partial class ReadXml
    {
        public static string getXmlElementValue(XElement element, string name)
        {
            return element.Element(name) == null ? string.Empty : element.Element(name).Value;
        }

        public static string getXmlElementAttr(XElement element, string attri, string defaultStr = "")
        {
            return element.Attribute(attri) == null ? defaultStr : element.Attribute(attri).Value;
        }
    }
}
