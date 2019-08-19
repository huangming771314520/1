using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Zephyr.Utils
{
    public partial class ReadExcel
    {
        /// <summary>    
        /// 把数据从Excel装载到DataTable    
        /// </summary>    
        /// <param name="pathName">带路径的Excel文件名</param>    
        /// <param name="sheetName">工作表名</param>
        /// <param name="tbContainer">将数据存入的DataTable</param>
        /// <returns></returns> 
        public static DataTable ExcelToDataTable(string pathName)
        {
            DataTable tbContainer = new DataTable();
            string strConn = string.Empty;
            FileInfo file = new FileInfo(pathName);
            if (!file.Exists)
            {
                throw new Exception("文件不存在!");
            }
            string extension = file.Extension;
            switch (extension)
            {
                case ".xls":
                    strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + pathName + ";Extended Properties='Excel 8.0;HDR=No;IMEX=1;'";
                    break;
                case ".xlsx":
                    strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + pathName + ";Extended Properties='Excel 12.0;HDR=No;IMEX=1;'";
                    break;
                default:
                    strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + pathName + ";Extended Properties='Excel 8.0;HDR=No;IMEX=1;'";
                    break;
            }
            OleDbConnection cnnxls = new OleDbConnection(strConn);//链接Excel
            cnnxls.Open();
            DataTable sheetsName = cnnxls.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "Table" }); //得到所有sheet的名字
            string sheetName = sheetsName.Rows[0][2].ToString(); //得到第一个sheet的名字
            OleDbDataAdapter oda = new OleDbDataAdapter(string.Format("select * from [{0}]", sheetName), cnnxls); //读取Excel里面有 表Sheet1
            DataSet ds = new DataSet();//将Excel里面有表内容装载到内存表中！
            oda.Fill(tbContainer);
            tbContainer.Rows.RemoveAt(0);
            return tbContainer;
        }

        #region 获取指定目录的所有文件和子目录
        public static List<string> GetDirectoryFiles(string TargetDir)
        {
            TargetDir = HttpContext.Current.Server.MapPath(TargetDir);
            List<FileInfo> list = GetAllFilesByDir(TargetDir).Where(li => li.Extension.ToLower().Equals(".xml")).ToList();
            List<string> FilePathList = new List<string>();
            if (list.Count > 0)
            {
                foreach (FileInfo fi in list)
                {
                    FilePathList.Add(fi.FullName.ToString());
                }
            }
            return FilePathList;
        }

        /// <summary>  
        /// 获得指定目录下的所有文件  
        /// </summary>  
        /// <param name="path"></param>  
        /// <returns></returns>  
        public static List<FileInfo> GetFilesByDir(string path)
        {
            DirectoryInfo di = new DirectoryInfo(path);

            //找到该目录下的文件  
            FileInfo[] fi = di.GetFiles();

            //把FileInfo[]数组转换为List  
            List<FileInfo> list = fi.ToList<FileInfo>();
            return list;
        }

        /// <summary>  
        /// 获得指定目录及其子目录的所有文件  
        /// </summary>  
        /// <param name="path"></param>  
        /// <returns></returns>  
        public static List<FileInfo> GetAllFilesByDir(string path)
        {
            DirectoryInfo dir = new DirectoryInfo(path);

            //找到该目录下的文件  
            FileInfo[] fi = dir.GetFiles();

            //把FileInfo[]数组转换为List  
            List<FileInfo> list = fi.ToList<FileInfo>();

            //找到该目录下的所有目录里的文件  
            DirectoryInfo[] subDir = dir.GetDirectories();
            foreach (DirectoryInfo d in subDir)
            {
                List<FileInfo> subList = GetFilesByDir(d.FullName);
                foreach (FileInfo subFile in subList)
                {
                    list.Add(subFile);
                }
            }
            return list;
        }

        #endregion
    }
}
