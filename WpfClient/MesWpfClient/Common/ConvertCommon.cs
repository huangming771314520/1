using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesWpfClient.Common
{
    public static class ConvertCommon
    {
        /// <summary>
        /// 将list转换成ObservableCollection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static ObservableCollection<T> ToObservable<T>(List<T> list) where T : class
        {
            ObservableCollection<T> result = new ObservableCollection<T>();
            list.ForEach(item => result.Add(item));
            return result;
        }

        /// <summary>
        /// ASCII码转字符
        /// </summary>
        /// <param name="asciiCode">ASCII码</param>
        /// <returns>返回字符</returns>
        public static string ToStringByASCII(int asciiCode)
        {
            if (asciiCode >= 0 && asciiCode <= 255)
            {
                ASCIIEncoding asciiEncoding = new ASCIIEncoding();
                byte[] byteArray = new byte[] { (byte)asciiCode };
                string strCharacter = asciiEncoding.GetString(byteArray);
                return strCharacter;
            }
            else
            {
                return "";
            }
        }

    }
}
