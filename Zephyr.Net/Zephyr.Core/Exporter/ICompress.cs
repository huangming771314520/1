/*************************************************************************
 * 文件名称 ：ICompress.cs                          
 * 描述说明 ：文件压缩接口
 * 
 * 创建信息 : create by shiruizhi@qq.com on 2013 (施睿智)
 * 修订信息 : 
**************************************************************************/

using System.IO;

namespace Zephyr.Core
{
    public interface ICompress
    {
        string Suffix(string orgSuffix);
        Stream Compress(Stream fileStream,string fullName);
    }
}
