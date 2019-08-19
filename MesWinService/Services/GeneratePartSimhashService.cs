using log4net;
using MesWinService.Helpers;
using MesWinService.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using Timer = System.Timers.Timer;

namespace MesWinService.Services
{
    public class GeneratePartSimhashService
    {
        private readonly Timer timer = null;
        private readonly ILog log = LogManager.GetLogger(typeof(GeneratePartSimhashService));
        private readonly int interval = Convert.ToInt32(ConfigurationManager.AppSettings["Interval"].ToString());
        private int toIndex = Convert.ToInt32(ConfigurationManager.AppSettings["ExecuteNum"].ToString());
        private List<BomModel> bomData = new List<BomModel>();
        private List<string> sqlList = new List<string>();
        private string simHash = string.Empty;
        private int count = 0;
        private int num = 0;

        private bool isRun = false;

        public GeneratePartSimhashService()
        {
            timer = new Timer(1000 * interval) { AutoReset = true };
            timer.Elapsed += (sender, eventArgs) =>
            {
                try
                {
                    if (!isRun)
                    {
                        isRun = true;
                    }
                    else
                    {
                        return;
                    }

                    bomData = GetBomData();

                    if (bomData.Count > 0)
                    {
                        sqlList.Clear();
                        count = bomData.Count;
                        num = 0;

                        log.InfoFormat("检测到需要计算相似度的数据有{0}条...", count);
                    }

                    foreach (var item in bomData)
                    {
                        item.Name = Regex.Replace(item.Name, @"\s", "");
                        item.Name = Regex.Replace(item.Name, @"[^\u4e00-\u9fa5].*?", "");
                        simHash = new SimhashHelper.SimHash(item.Name).StrSimHash.ToString();
                        sqlList.Add(string.Format("update SYS_Part set SimHash = '{0}' where ID = {1};", simHash, item.ID));
                    }

                    while (num < count)
                    {
                        if ((num + toIndex) > count)
                        {
                            toIndex = count - num;
                        }

                        List<string> list = sqlList.Skip(num).Take(toIndex).ToList();

                        DBHelper.ExecuteCommand(string.Join("\r\n", list));

                        num += toIndex;
                    }

                    isRun = false;
                }
                catch (Exception ex)
                {
                    log.Info("出现错误！", ex);
                }
            };
        }

        public void Start()
        {
            log.Info("服务启动！");
            timer.Start();
        }

        public void Stop()
        {
            log.Info("服务暂停！");
            timer.Stop();
        }

        private List<BomModel> GetBomData()
        {
            List<BomModel> result = new List<BomModel>();

            string sql = "SELECT ID,(ISNULL(InventoryName,'')+ISNULL(Spec,'')+ISNULL(Model,'')) AS Name,SimHash FROM dbo.SYS_Part WHERE SimHash IS NULL";

            DataTable dt = DBHelper.GetDataTable(sql);

            foreach (DataRow dr in dt.Rows)
            {
                result.Add(new BomModel()
                {
                    ID = Convert.ToInt32(dr["ID"]),
                    Name = dr["Name"].ToString()
                });
            }

            return result;
        }
    }
}
