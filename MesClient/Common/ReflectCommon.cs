using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesClient.Common
{
    public class ReflectCommon
    {
        public static object GetAttrValueByObj(string attrName, object obj)
        {
            try
            {
                Type type = obj.GetType();

                var attr = type.GetProperty(attrName);
                if (attr == null)
                {
                    return string.Empty;
                }
                else
                {
                    return attr.GetValue(obj, null);
                }
            }
            catch
            {
                return string.Empty;
            }
        }


    }
}
