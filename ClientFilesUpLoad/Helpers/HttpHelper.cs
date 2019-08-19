using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Text;

namespace ClientFilesUpLoad.Helpers
{
    public class HttpHelper
    {
        /// <summary>
        /// Get请求 - 根据url获取JSON
        /// </summary>
        /// <param name="url">URL地址</param>
        /// <param name="timeOut">超时时间</param>
        /// <returns></returns>
        public static string GetJSON(string url, int timeOut = 1000 * 30)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Timeout = timeOut;
            request.Method = "get";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// Post请求 - 根据url获取JSON
        /// </summary>
        /// <param name="url">URL地址</param>
        /// <param name="obj">参数 - 匿名对象</param>
        /// <param name="timeOut">超时时间</param>
        /// <returns></returns>
        public static string PostJSON(string url, object obj, int timeOut = 1000 * 30)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Accept = @"application/json, text/javascript, */*; q=0.01";
            request.ContentType = @"application/json";
            request.Timeout = timeOut;
            request.Method = "post";
            string strParment = JsonConvert.SerializeObject(obj);
            byte[] postData = Encoding.UTF8.GetBytes(strParment.ToString());
            request.ContentLength = postData.Length;
            Stream newStream = request.GetRequestStream();
            newStream.Write(postData, 0, postData.Length);
            newStream.Close();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// Get请求 - 根据url获取JSON并序列化成对象
        /// </summary>
        /// <typeparam name="T">要序列化成的对象</typeparam>
        /// <param name="url">URL地址</param>
        /// <param name="timeOut">超时时间</param>
        /// <returns></returns>
        public static T GetTByUrl<T>(string url, int timeOut = 1000 * 30)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Timeout = timeOut;
            request.Method = "get";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string json = string.Empty;
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                json = reader.ReadToEnd();
            }
            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// Post请求 - 根据url获取JSON并序列化成对象
        /// </summary>
        /// <typeparam name="T">要序列化成的对象</typeparam>
        /// <param name="url">URL地址</param>
        /// <param name="obj">参数 - 匿名对象</param>
        /// <param name="timeOut">超时时间</param>
        /// <returns></returns>
        public static T PostTByUrl<T>(string url, object obj, int timeOut = 1000 * 30)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Accept = @"application/json, text/javascript, */*; q=0.01";
            request.ContentType = @"application/json";
            request.Timeout = timeOut;
            request.Method = "post";
            string strParameter = JsonConvert.SerializeObject(obj);
            byte[] postData = Encoding.UTF8.GetBytes(strParameter.ToString());
            request.ContentLength = postData.Length;
            Stream newStream = request.GetRequestStream();
            newStream.Write(postData, 0, postData.Length);
            newStream.Close();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string json = string.Empty;
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                json = reader.ReadToEnd();
            }
            return JsonConvert.DeserializeObject<T>(json);
        }

    }
}
