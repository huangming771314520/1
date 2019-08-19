/*************************************************************************
 * 文件名称 ：IExport.cs                          
 * 描述说明 ：文件导出接口
 * 
 * 
 * 创建信息 : create by shiruizhi@qq.com on 2013 (施睿智)
 * 修订信息 : 
**************************************************************************/

using System.IO;

namespace Zephyr.Core
{
    public interface IExport
    {
        string suffix { get;}
 
        void MergeCell(int x1,int y1,int x2,int y2);
        void FillData(int x, int y,string field, object data);

        void Init(object data);
        Stream SaveAsStream();

        void SetHeadStyle(int x1, int y1, int x2, int y2);
        void SetRowsStyle(int x1, int y1, int x2, int y2);
    }
}
