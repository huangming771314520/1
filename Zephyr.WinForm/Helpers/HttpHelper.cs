using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.WinForm.Helpers
{
    public class HttpHelper
    {
        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="request">请求信息</param>
        /// <returns></returns>
        public static string GetResponse(HttpWebRequest request)
        {
            request.Method = "get";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="request">请求信息</param>
        /// <param name="obj">参数</param>
        /// <returns></returns>
        public static string PostResponse(HttpWebRequest request, object obj)
        {
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
        /// 根据url获取JSON
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetJSON(string url)
        {
            return GetResponse((HttpWebRequest)WebRequest.Create(url));
        }

        public static string PostJSON(string url, object obj)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Accept = @"application/json, text/javascript, */*; q=0.01";
            request.ContentType = @"application/json";

            return PostResponse(request, obj);
        }

    }
}
