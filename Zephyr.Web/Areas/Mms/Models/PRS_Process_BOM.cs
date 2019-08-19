using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;
using System.Linq;
using System.Dynamic;
using Zephyr.Areas.Areas.Mms.Common;
using Zephyr.Utils.ExcelLibrary.BinaryFileFormat;
using System.IO;
using Zephyr.Areas.CommonWrap;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class PRS_Process_BOMService : ServiceBase<PRS_Process_BOM>
    {
        public List<SYS_BOM> GetBom(string parentCode, string versionCode)
        {
            List<SYS_BOM> boomlist = new List<SYS_BOM>();
            string sql = String.Format(@"select * from SYS_BOM where PartCode='{0}' and VersionCode='{1}'", parentCode, versionCode);
            var list = db.Sql(sql).QueryMany<SYS_BOM>();
            while (list.Count != 0)
            {
                boomlist.AddRange(list);
                var part = "(";
                foreach (var item in list)
                {
                    part += "'" + item.PartCode + "',";
                }
                part = part.Remove(part.Length - 1, 1);
                part += ")";
                sql = String.Format(@"select tab1.*  from SYS_BOM tab1 inner join (
    select PartCode,max(VersionCode) as VersionCode from SYS_BOM where ParentCode in {0}  group by PartCode ) tab2 
     on tab1.PartCode=tab2.PartCode and tab1.VersionCode=tab2.VersionCode where ParentCode in {0}", part);
                list = db.Sql(sql).QueryMany<SYS_BOM>();
            }
            return boomlist;
        }

        public int GetUpdateProcessIsSelfMade(string id, string value)
        {

            string sql = string.Format("UPDATE dbo.PRS_Process_BOM SET IsSelfMade='{0}' WHERE PartCode='{1}'", value, id);
            int res = db.Sql(sql).Execute();
            if (res > 0)
                return 1;
            else
            {
                return 0;
            }
        }

        public dynamic GetUpdateProcessIsSelfMadeAndIsEnable(string id, string selfMadeValue, string enableValue)
        {
            List<string> sqlValue = new List<string>();

            if (!string.IsNullOrEmpty(selfMadeValue))
            {
                sqlValue.Add(string.Format(@" IsSelfMade='{0}' ", selfMadeValue));
            }
            if (!string.IsNullOrEmpty(enableValue))
            {
                sqlValue.Add(string.Format(@" IsEnable='{0}' ", enableValue));
            }

            if (sqlValue.Count <= 0)
            {
                return 0;
            }
            else
            {
                string sql = string.Format(@"UPDATE dbo.PRS_Process_BOM SET {0} WHERE PartCode='{1}'", string.Join(",", sqlValue), id);
                return db.Sql(sql).Execute() > 0;
            }
        }


        public dynamic GetUpdateProcessIsSelfMade2(string pCode, int selfMadeValue)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                var processBom = db.Sql(string.Format(@"SELECT * FROM dbo.PRS_Process_BOM WHERE IsEnable = 1 AND PartCode = '{0}'", pCode)).QuerySingle<PRS_Process_BOM>();

                if (processBom != null)
                {
                    if (new int[] { 0, 1 }.Contains(selfMadeValue))
                    {
                        if (processBom.IsSelfMade == "1")
                        {
                            sb.Append(WinFormClientService.GetUpdateSQL(nameof(PRS_Process_BOM), new KeyValuePair<string, object>("PartCode", pCode), new
                            {
                                IsSelfMade = selfMadeValue,
                                ModifyPerson = MmsHelper.GetUserName(),
                                ModifyTime = DateTime.Now
                            }));
                            //sb.Append(string.Format(@"UPDATE dbo.PRS_Process_BOM SET InventoryCode = NULL,InventoryName = NULL WHERE IsEnable = 1 AND PartCode = '{0}';", pCode));
                        }
                        else
                        {
                            sb.Append(WinFormClientService.GetUpdateSQL(nameof(PRS_Process_BOM), new KeyValuePair<string, object>("PartCode", pCode), new
                            {
                                IsSelfMade = selfMadeValue,
                                ModifyPerson = MmsHelper.GetUserName(),
                                ModifyTime = DateTime.Now
                            }));

                            sb.Append(string.Format(@"UPDATE dbo.PRS_Process_BOM SET InventoryCode = NULL,InventoryName = NULL WHERE IsEnable = 1 AND PartCode = '{0}';", pCode));

                        }

                        //sb.Append(string.Format(@"UPDATE dbo.SYS_Part SET PartCode = NULL,PartName = NULL WHERE InventoryCode = '{0}';", processBom.InventoryCode));
                    }
                    else
                    {
                        throw new Exception("参数错误！");
                    }
                }

                string sql = sb.ToString();

                db.UseTransaction(true);
                bool result = db.Sql(sql).Execute() > 0;
                if (result)
                {
                    db.Commit();
                }
                else
                {
                    db.Rollback();
                }

                return new ResultModel()
                {
                    Result = result,
                    Msg = result ? "设置成功！" : "设置失败！"
                };
            }
            catch (Exception ex)
            {
                return new ResultModel()
                {
                    Result = false,
                    Msg = "设置失败！" + ex.Message
                };
            }
        }
        /// <summary>
        /// 配套表编辑下料数量
        /// </summary>
        /// <param name="pCode"></param>
        /// <param name="selfMadeValue"></param>
        /// <returns></returns>
        public dynamic GetUpdateProcessMaterialNum(string pCode, int blankingNum, int purchaseNum, int restructNum)
        {
            try
            {
                string sql = WinFormClientService.GetUpdateSQL(nameof(PRS_Process_BOM_Blanking), new KeyValuePair<string, object>("PartCode", pCode), new
                {
                    BlankingNum = blankingNum,
                    PurchaseNum = purchaseNum,
                    RestructNum = restructNum
                });

                return db.Sql(sql).Execute();
            }
            catch
            {
                return 0;
            }
        }


        public List<PRS_Process_BOM> GetPRS_Process_BOM(string parentCode)
        {
            //List<PRS_Process_BOM> boomlist = new List<PRS_Process_BOM>();
            //string sql = String.Format(@"select * from PRS_Process_BOM where PartCode='{0}'  ", parentCode);
            //var list = db.Sql(sql).QueryMany<PRS_Process_BOM>();
            //while (list.Count != 0)
            //{
            //    boomlist.AddRange(list);
            //    var part = "(";
            //    foreach (var item in list)
            //    {
            //        part += "'" + item.PartCode + "',";
            //    }
            //    part = part.Remove(part.Length - 1, 1);
            //    part += ")";
            //    sql = String.Format(@"select * from PRS_Process_BOM where ParentCode in {0}  ", part);
            //    list = db.Sql(sql).QueryMany<PRS_Process_BOM>();
            //}
            //return boomlist;

            #region
            /*
            string sqlA = string.Format(@"
DECLARE @pCode VARCHAR(50) = '{0}';
WITH cte AS
(
  SELECT ID,PartCode,ParentCode FROM dbo.PRS_Process_BOM WHERE IsEnable = 1 and ISNULL(ParentCode,'')=@pCode
  UNION ALL
  SELECT b.ID,b.PartCode,b.ParentCode FROM cte p JOIN dbo.PRS_Process_BOM b ON b.ParentCode=p.PartCode AND b.IsEnable = 1
)
SELECT * FROM cte UNION SELECT ID,PartCode,ParentCode FROM dbo.PRS_Process_BOM WHERE ISNULL(PartCode,'')=@pCode
", parentCode);
            */
            #endregion

            string sqlA = string.Format(@"
DECLARE @pCode VARCHAR(50) = '{0}';
WITH cte AS
(
  SELECT ID,PartCode,ParentCode FROM dbo.PRS_Process_BOM WHERE ISNULL(ParentCode,'')=@pCode
  UNION ALL
  SELECT b.ID,b.PartCode,b.ParentCode FROM cte p JOIN dbo.PRS_Process_BOM b ON b.ParentCode=p.PartCode
)
SELECT * FROM cte UNION SELECT ID,PartCode,ParentCode FROM dbo.PRS_Process_BOM WHERE ISNULL(PartCode,'')=@pCode
", parentCode);

            var ids = db.Sql(sqlA).QueryMany<dynamic>();
            List<int> idList = new List<int>();
            if (ids.Count <= 0)
            {
                return new List<PRS_Process_BOM>();
            }
            else
            {
                ids.ForEach(item =>
                {
                    idList.Add(Convert.ToInt32(item.ID));
                });

                //string sqlB = string.Format(@"SELECT * FROM dbo.PRS_Process_BOM WHERE IsEnable=1 AND ID IN ({0})", string.Join(",", idList));
                string sqlB = string.Format(@"SELECT * FROM dbo.PRS_Process_BOM WHERE ID IN ({0})", string.Join(",", idList));

                return db.Sql(sqlB).QueryMany<PRS_Process_BOM>();
            }
        }

        public int Insert(SYS_BOM model)
        {
            var rowsAffected = model.ID = db.Insert<SYS_BOM>("SYS_BOM", model)
     .AutoMap(x => x.ID)
     .ExecuteReturnLastId<int>();
            return rowsAffected;
        }
        public int Update(SYS_BOM model)
        {
            var rowsAffected = db.Update<SYS_BOM>("SYS_BOM", model)
     .AutoMap(x => x.ID)
     .Where(x => x.ID)
     .Execute();
            return rowsAffected;
        }

        public PRS_Process_BOM SelectPRS_Process_BOM(string id)
        {
            string sql = string.Format(@"select * from PRS_Process_BOM where id={0}", id);
            var s = db.Sql(sql).QuerySingle<PRS_Process_BOM>();
            return s;
        }
        //集合克隆
        /// <summary>
        /// 克隆转化集合
        /// </summary>
        /// <typeparam name="TIn">克隆的原对象</typeparam>
        /// <typeparam name="TOut">克隆的后生成对象</typeparam>
        /// <param name="tIn">需要克隆数据</param>
        /// <returns></returns>
        public static TOut TransReflection<TIn, TOut>(TIn tIn)
        {
            TOut tOut = Activator.CreateInstance<TOut>();
            var tInType = tIn.GetType();
            foreach (var itemOut in tOut.GetType().GetProperties())
            {
                var itemIn = tInType.GetProperty(itemOut.Name);
                if (itemIn != null)
                {
                    itemOut.SetValue(tOut, itemIn.GetValue(tIn));
                }
            }
            return tOut;
        }
        public List<dynamic> GetGYBOMTree(string PartCode, string VersionCode)
        {
            var GyBOMList = GetPRS_Process_BOM(PartCode);
            if (GyBOMList.Count > 0)
            {
                List<dynamic> list3 = new List<dynamic>();
                foreach (var item in GyBOMList)
                {
                    var d = item.IsEnable == 1 ? "" : " 无效";
                    list3.Add(new { id = item.PartCode, pid = item.ParentCode, text = item.PartFigureCode + " " + item.PartName + d + "<span style='display:none'>&" + item.ID + "&</span>" });
                }
                return list3;
            }
            else
                return null;
        }
        /// <summary>
        /// 自制外购界面 查询树状表方法
        /// </summary>
        /// <param name="PartCode"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<dynamic> GetGYBomTreeGrid(string PartCode)
        {
            var GyBOMList = GetPRS_Process_BOM(PartCode);

            PRS_Process_BOM bom = new PRS_Process_BOM();
            PMS_BN_ProjectDetail product = new PMS_BN_ProjectDetail();
            try
            {
                bom = new PRS_Process_BOMService().GetModel(ParamQuery.Instance().AndWhere("IsEnable", 1).AndWhere("PartCode", PartCode));
                product = new PMS_BN_ProjectDetailService().GetModel(ParamQuery.Instance().AndWhere("IsEnable", 1).AndWhere("ID", bom.ProductID));

                if (GyBOMList.Count > 0)
                {
                    List<dynamic> dynamics = new List<dynamic>();
                    GyBOMList.ForEach(item =>
                    {
                        dynamics.Add(new
                        {
                            PartCode = item.PartCode,
                            ParentCode = item.ParentCode,
                            ID = item.ID,
                            MaterialCode = item.MaterialCode,
                            Text = item.PartFigureCode + " " + item.PartName,
                            PartFigureCode = item.PartFigureCode,
                            PartName = item.PartName,
                            PartQuantity = item.PartQuantity,
                            IsSelfMade = item.IsSelfMade,
                            IsEnable = item.IsEnable,
                            BlankingTotal = item.PartQuantity == null ? 0 : item.PartQuantity * product.Quantity,
                            BlankingNum = item.BlankingNum ?? (item.PartQuantity == null ? 0 : item.PartQuantity * product.Quantity),
                            Quantity = product.Quantity,
                            IsCrux = item.IsCrux,
                            MateType = item.MateType
                        });
                    });
                    return dynamics;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 生产（下料数量）页面 查询树状表方法
        /// </summary>
        /// <param name="PartCode"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<dynamic> GetProduceNumBomTreeGrid(string PartCode, string ContractCode, int ProductID)
        {
            //var GyBOMList = GetPRS_Process_BOM(PartCode);
            var GyBOMList = TreeNodeManage.GetTreeNodeList_Pro<PRS_Process_BOM_Blanking>(
                 TreeNodeManage.Instance()
                 .SetNode("PartCode")
                 .SetParentNode("ParentCode", PartCode)
                 .SetTableName("PRS_Process_BOM_Blanking")
                 .SetWhereSql("where temp.IsEnable=1 and temp.ContractCode='" + ContractCode + "' and temp.ProductID=" + ProductID)
                 .SetOrderBy("order by ID"));

            PRS_Process_BOM_Blanking bom = new PRS_Process_BOM_Blanking();
            PMS_BN_ProjectDetail product = new PMS_BN_ProjectDetail();
            try
            {
                bom = new PRS_Process_BOM_BlankingService().GetModel(ParamQuery.Instance().AndWhere("IsEnable", 1).AndWhere("PartCode", PartCode));
                product = new PMS_BN_ProjectDetailService().GetModel(ParamQuery.Instance().AndWhere("IsEnable", 1).AndWhere("ID", bom.ProductID));

                if (GyBOMList.Count > 0)
                {
                    List<dynamic> dynamics = new List<dynamic>();
                    GyBOMList.ForEach(item =>
                    {
                        dynamics.Add(new
                        {
                            PartCode = item.PartCode,
                            ParentCode = item.ParentCode,
                            ID = item.ID,
                            MaterialCode = item.MaterialCode,
                            Text = item.PartFigureCode + " " + item.PartName,
                            PartFigureCode = item.PartFigureCode,
                            PartName = item.PartName,
                            PartQuantity = item.PartQuantity,
                            IsSelfMade = item.IsSelfMade,
                            IsEnable = item.IsEnable,
                            BlankingTotal = item.PartQuantity == null ? 0 : item.PartQuantity * product.Quantity,
                            BlankingNum = item.BlankingNum ?? (item.PartQuantity == null ? 0 : item.PartQuantity * product.Quantity),
                            Quantity = product.Quantity,
                            PurchaseNum = item.PurchaseNum,
                            RestructNum = item.RestructNum
                        });
                    });
                    return dynamics;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public dynamic GetAutoUpdateBlankingNum(string partCode)
        {
            try
            {
                //var GyBOMList = GetPRS_Process_BOM(partCode).Where(item => item.BlankingNum == null).ToList();
                var GyBOMList = TreeNodeManage.GetTreeNodeList_Pro<PRS_Process_BOM_Blanking>(
                  TreeNodeManage.Instance()
                  .SetNode("PartCode")
                  .SetParentNode("ParentCode", partCode)
                  .SetTableName("PRS_Process_BOM_Blanking")
                  .SetWhereSql("where temp.BlankingNum is null")
                  .SetOrderBy("order by ID"));

                if (GyBOMList.Count.Equals(0))
                {
                    return new ResultModel()
                    {
                        Result = true,
                        Msg = "无需更新数据！"
                    };
                }

                //PRS_Process_BOM bom = new PRS_Process_BOMService().GetModel(ParamQuery.Instance().AndWhere("IsEnable", 1).AndWhere("PartCode", partCode));
                PRS_Process_BOM_Blanking bom = new PRS_Process_BOM_BlankingService().GetModel(ParamQuery.Instance().AndWhere("IsEnable", 1).AndWhere("PartCode", partCode));
                PMS_BN_ProjectDetail product = new PMS_BN_ProjectDetailService().GetModel(ParamQuery.Instance().AndWhere("IsEnable", 1).AndWhere("ID", bom.ProductID));

                StringBuilder sb = new StringBuilder();

                foreach (var item in GyBOMList)
                {
                    if (item.IsSelfMade.Equals("1"))
                    {
                        sb.Append(WinFormClientService.GetUpdateSQL(nameof(PRS_Process_BOM_Blanking), new KeyValuePair<string, object>("ID", item.ID), new
                        {
                            BlankingNum = item.PartQuantity == null ? 0 : item.PartQuantity * (product.Quantity ?? 0),
                            PurchaseNum = 0
                        }));
                    }
                    else
                    {
                        sb.Append(WinFormClientService.GetUpdateSQL(nameof(PRS_Process_BOM_Blanking), new KeyValuePair<string, object>("ID", item.ID), new
                        {
                            BlankingNum = 0,
                            PurchaseNum = item.PartQuantity == null ? 0 : item.PartQuantity * (product.Quantity ?? 0)
                        }));
                    }
                }

                string sql = sb.ToString();
                bool result = db.Sql(sql).Execute() > 0;

                return new ResultModel()
                {
                    Result = result,
                    Msg = result ? "更新成功！" : "更新失败！"
                };

            }
            catch (Exception ex)
            {
                return new ResultModel()
                {
                    Result = false,
                    Msg = ex.Message
                };
            }
        }

        public List<dynamic> GetPlanKitting_ConfigPage_BomTreeGrid(string PartCode)
        {
            var GyBOMList = GetPRS_Process_BOM(PartCode).Where(item => item.IsSelfMade != "0").ToList();
            var pprList = db.Sql(@"SELECT DISTINCT PartCode,ApproveState,ProcessModelType FROM dbo.MES_BN_ProductProcessRoute WHERE IsEnable = 1").QueryMany<MES_BN_ProductProcessRoute>();

            if (GyBOMList.Count > 0)
            {
                List<dynamic> dynamics = new List<dynamic>();
                GyBOMList.ForEach(item =>
                {
                    var ppr_A = pprList.Where(i =>
                    {
                        bool a = i.PartCode == null ? false : i.PartCode.Equals(item.PartCode);
                        bool b = i.ProcessModelType == null ? false : i.ProcessModelType.Equals("1");
                        return a && b;
                    }).ToList();
                    var ppr_B = pprList.Where(i =>
                    {
                        bool a = i.PartCode == null ? false : i.PartCode.Equals(item.PartCode);
                        bool b = i.ProcessModelType == null ? false : i.ProcessModelType.Equals("2");
                        return a && b;
                    }).ToList();
                    var ppr_C = pprList.Where(i =>
                    {
                        bool a = i.PartCode == null ? false : i.PartCode.Equals(item.PartCode);
                        bool b = i.ProcessModelType == null ? false : i.ProcessModelType.Equals("3");
                        return a && b;
                    }).ToList();
                    var ppr_D = pprList.Where(i =>
                    {
                        bool a = i.PartCode == null ? false : i.PartCode.Equals(item.PartCode);
                        bool b = i.ProcessModelType == null ? false : i.ProcessModelType.Equals("4");
                        return a && b;
                    }).ToList();

                    dynamics.Add(new
                    {
                        PartCode = item.PartCode,
                        ParentCode = item.ParentCode,
                        ID = item.ID,
                        PartFigureCode = item.PartFigureCode,
                        PartName = item.PartName,
                        Blanking = item.Blanking,
                        Blanking_ApproveState = ppr_A.Count.Equals(0) ? "-1" : ppr_A[0].ApproveState ?? "-1",
                        Welding = item.Welding,
                        Welding_ApproveState = ppr_B.Count.Equals(0) ? "-1" : ppr_B[0].ApproveState ?? "-1",
                        Machining = item.Machining,
                        Machining_ApproveState = ppr_C.Count.Equals(0) ? "-1" : ppr_C[0].ApproveState ?? "-1",
                        Assembling = item.Assembling,
                        Assembling_ApproveState = ppr_D.Count.Equals(0) ? "-1" : ppr_D[0].ApproveState ?? "-1",
                        IsEnable = item.IsEnable
                    });
                });
                return dynamics;
            }
            else
            {
                return null;
            }
        }

        public List<dynamic> GetPlanKitting_ReportPage_BomTreeGrid(string PartCode)
        {
            var GyBOMList = GetPRS_Process_BOM(PartCode);
            var RealStockSumList = db.Sql(@"SELECT InventoryCode,SUM(ISNULL(RealStock,0)) AS SumRealStock FROM dbo.WMS_BN_RealStock WHERE IsEnable = 1 GROUP BY InventoryCode;").QueryMany<dynamic>();
            var ProductPurchaseDetailInfoList = db.Sql(@"
SELECT tbTemp.ContractCode,tbTemp.InventoryCode,MAX(tbTemp.PurchaseDate) AS PurchaseDate 
FROM (SELECT tbMain.ContractCode,tbDetail.InventoryCode,tbDetail.PurchaseDate 
FROM dbo.APS_ProductPurchaseMain tbMain INNER JOIN dbo.APS_ProductPurchaseDetail tbDetail
ON tbMain.ID = tbDetail.MainID AND tbDetail.IsEnable = 1 WHERE tbMain.IsEnable = 1) tbTemp
GROUP BY tbTemp.ContractCode,tbTemp.InventoryCode
").QueryMany<dynamic>();

            if (GyBOMList.Count > 0)
            {
                List<dynamic> dynamics = new List<dynamic>();
                GyBOMList.ForEach(item =>
                {
                    var cCode = item.ContractCode;
                    var iCode = item.InventoryCode;

                    var RealStockSumItem = RealStockSumList.SingleOrDefault(i => i.InventoryCode == iCode);

                    var ProductPurchaseDetailInfoItem = ProductPurchaseDetailInfoList.SingleOrDefault(i =>
                    {
                        bool a = i.ContractCode == cCode;
                        bool b = i.InventoryCode == iCode;
                        return a && b;
                    });

                    dynamics.Add(new
                    {
                        PartCode = item.PartCode,
                        ParentCode = item.ParentCode,
                        ID = item.ID,
                        PartFigureCode = item.PartFigureCode,
                        PartName = item.PartName,
                        PartQuantity = item.PartQuantity ?? 0,
                        RealStockSum = RealStockSumItem == null ? 0 : RealStockSumItem.SumRealStock,
                        PurchaseDate = ProductPurchaseDetailInfoItem == null ? "" : ProductPurchaseDetailInfoItem.PurchaseDate,
                        Blanking = item.Blanking,
                        Welding = item.Welding,
                        Machining = item.Machining,
                        Assembling = item.Assembling,
                        IsEnable = item.IsEnable
                    });
                });
                return dynamics;
            }
            else
            {
                return null;
            }
        }

        public dynamic GetBlueprintPageBomTreeGrid(string partCode)
        {
            #region
            /*
            var GyBOMList = GetPRS_Process_BOM(PartCode);
            var partFileList = db.Sql(string.Format(@"SELECT * FROM [dbo].[PMS_BN_PartFile] WHERE IsEnable = 1")).QueryMany<PMS_BN_PartFile>();
            var processFigureList = db.Sql(@"SELECT tbA.* FROM dbo.PRS_ProcessFigure tbA RIGHT JOIN (SELECT MAX(ID) AS maxID,PartCode FROM dbo.PRS_ProcessFigure WHERE IsEnable = 1 GROUP BY PartCode) tbB ON tbA.ID = tbB.maxID").QueryMany<PRS_ProcessFigure>();

            var sources = (from tbA in GyBOMList
                           join tbB in partFileList
                           on new
                           {
                               cCode = tbA.ContractCode,
                               pID = (tbA.ProductID ?? -1).ToString(),
                               pCode = tbA.PartCode
                           } equals new
                           {
                               cCode = tbB.ContractCode,
                               pID = tbB.ProjectDetailID,
                               pCode = tbB.PartCode
                           } into tempTB
                           from tb in tempTB.DefaultIfEmpty()
                           select new
                           {
                               PartCode = tbA.PartCode,
                               ParentCode = tbA.ParentCode,
                               ID = tbA.ID,
                               MaterialCode = tbA.MaterialCode,
                               Text = tbA.PartFigureCode + " " + tbA.PartName,
                               PartFigureCode = tbA.PartFigureCode,
                               PartName = tbA.PartName,
                               PartQuantity = tbA.PartQuantity,
                               IsSelfMade = tbA.IsSelfMade,
                               IsEnable = tbA.IsEnable,
                               FileName = tb == null ? "" : tb.FileName,
                               DisplayFileText = tb == null ? "" : string.IsNullOrEmpty(tb.DocName) ? "" : tb.DocName.Split('_')[3],
                               WebFilePath = tb == null ? "" : string.IsNullOrEmpty(tb.FileAddress) ? "" : tb.FileAddress.Substring(tb.FileAddress.IndexOf("Upload") - 1).Replace('\\', '/')
                           }
                           into tbData
                           join tbC in processFigureList
                           on tbData.PartCode equals tbC.PartCode
                           into tbD
                           from tbE in tbD.DefaultIfEmpty()
                           select new
                           {
                               PartCode = tbData.PartCode,
                               ParentCode = tbData.ParentCode,
                               ID = tbData.ID,
                               MaterialCode = tbData.MaterialCode,
                               Text = tbData.Text,
                               PartFigureCode = tbData.PartFigureCode,
                               PartName = tbData.PartName,
                               PartQuantity = tbData.PartQuantity,
                               IsSelfMade = tbData.IsSelfMade,
                               IsEnable = tbData.IsEnable,
                               FileName = tbData.FileName,
                               DisplayFileText = tbData.DisplayFileText,
                               WebFilePath = tbData.WebFilePath,
                               WebDownFileName = tbE == null ? "" : tbE.FileName,
                               WebDownFilePath = tbE == null ? "" : string.IsNullOrEmpty(tbE.FileAddress) ? "" : tbE.FileAddress.Substring(tbE.FileAddress.IndexOf("Upload") - 1).Replace('\\', '/')
                           }).ToList();

            if (sources.Count > 0)
            {
                return sources;
            }
            else
            {
                return null;
            }
            */
            #endregion

            string sql = string.Format(@"
DECLARE @partCode VARCHAR(50) = '{0}';

SELECT tbA.ID,
       tbA.PartCode,
       tbA.ParentCode,
       tbA.MaterialCode,
       (tbA.PartFigureCode + ' ' + tbA.PartName) [Text],
       tbA.PartFigureCode,
       tbA.PartName,
       tbA.PartQuantity,
       tbA.IsSelfMade,
       tbA.IsEnable,
       tbB.PFile_DocNames,
       tbB.PFile_FileNames,
       tbB.PFile_FileAddress,
       tbC.PFigure_DocNames,
       tbC.PFigure_FileNames,
       tbC.PFigure_FileAddress
FROM dbo.Get_ProcessBOM(@partCode) tbA
    LEFT JOIN
    (
        SELECT PFile.PartCode,
               PFile.ContractCode,
               PFile.ProjectDetailID,
               PFile_DocNames = (STUFF(
                                 (
                                     SELECT '|' + DocName
                                     FROM dbo.PMS_BN_PartFile
                                     WHERE IsEnable = 1
                                           AND PartCode = PFile.PartCode
                                     FOR XML PATH('')
                                 ),
                                 1,
                                 1,
                                 ''
                                      )
                                ),
               PFile_FileNames = (STUFF(
                                  (
                                      SELECT '|' + [FileName]
                                      FROM dbo.PMS_BN_PartFile
                                      WHERE IsEnable = 1
                                            AND PartCode = PFile.PartCode
                                      FOR XML PATH('')
                                  ),
                                  1,
                                  1,
                                  ''
                                       )
                                 ),
               PFile_FileAddress = (STUFF(
                                    (
                                        SELECT '|' + FileAddress
                                        FROM dbo.PMS_BN_PartFile
                                        WHERE IsEnable = 1
                                              AND PartCode = PFile.PartCode
                                        FOR XML PATH('')
                                    ),
                                    1,
                                    1,
                                    ''
                                         )
                                   )
        FROM dbo.PMS_BN_PartFile PFile
        WHERE IsEnable = 1
        GROUP BY PFile.PartCode,
                 PFile.ContractCode,
                 PFile.ProjectDetailID
    ) tbB
        ON tbA.ContractCode = tbB.ContractCode
           AND tbA.ProductID = tbB.ProjectDetailID
           AND tbA.PartCode = tbB.PartCode
    LEFT JOIN
    (
        SELECT PFigure.PartCode,
               PFigure_DocNames = (STUFF(
                                   (
                                       SELECT '|' + DocName
                                       FROM dbo.PRS_ProcessFigure
                                       WHERE IsEnable = 1
                                             AND PartCode = PFigure.PartCode
                                       FOR XML PATH('')
                                   ),
                                   1,
                                   1,
                                   ''
                                        )
                                  ),
               PFigure_FileNames = (STUFF(
                                    (
                                        SELECT '|' + [FileName]
                                        FROM dbo.PRS_ProcessFigure
                                        WHERE IsEnable = 1
                                              AND PartCode = PFigure.PartCode
                                        FOR XML PATH('')
                                    ),
                                    1,
                                    1,
                                    ''
                                         )
                                   ),
               PFigure_FileAddress = (STUFF(
                                      (
                                          SELECT '|' + FileAddress
                                          FROM dbo.PRS_ProcessFigure
                                          WHERE IsEnable = 1
                                                AND PartCode = PFigure.PartCode
                                          FOR XML PATH('')
                                      ),
                                      1,
                                      1,
                                      ''
                                           )
                                     )
        FROM dbo.PRS_ProcessFigure PFigure
        WHERE IsEnable = 1
        GROUP BY PFigure.PartCode
    ) tbC
        ON tbA.PartCode = tbC.PartCode;
", partCode);

            List<dynamic> list = db.Sql(sql).QueryMany<dynamic>();

            list.ForEach(item =>
            {
                string PFilePaths = item.PFile_FileAddress;
                string PFigurePaths = item.PFigure_FileAddress;

                if (!string.IsNullOrEmpty(PFilePaths))
                {
                    List<string> arrA = PFilePaths.Split('|').ToList();
                    List<string> arrA_Temp = new List<string>();

                    arrA.ForEach(i =>
                    {
                        //if (File.Exists(i))
                        //{
                        arrA_Temp.Add(i);
                        //}
                        //else
                        //{
                        //    arrA_Temp.Add(string.Empty);
                        //}
                    });

                    item.PFile_FileAddress = string.Join("|", arrA_Temp);
                }

                if (!string.IsNullOrEmpty(PFigurePaths))
                {
                    List<string> arrB = PFigurePaths.Split('|').ToList();
                    List<string> arrB_Temp = new List<string>();

                    arrB.ForEach(i =>
                    {
                        //if (File.Exists(i))
                        //{
                        arrB_Temp.Add(i);
                        //}
                        //else
                        //{
                        //    arrB_Temp.Add(string.Empty);
                        //}
                    });

                    item.PFigure_FileAddress = string.Join("|", arrB_Temp);
                }

            });

            return list;
        }

        public dynamic GetCoverProcessBomPageBomTreeGrid(string partCode)
        {
            return db.Sql(string.Format(@"SELECT * FROM dbo.Get_ProcessBOM('{0}')", partCode)).QueryMany<PRS_Process_BOM>();
        }

        public dynamic GetBOMsByPCode(string partCode)
        {
            string sql = string.Format(@"
WITH cte
AS (SELECT *
    FROM PRS_Process_BOM
    WHERE ISNULL(PartFigureCode, '') = '{0}'
    UNION ALL
    SELECT temp.*
    FROM cte
        INNER JOIN PRS_Process_BOM temp
            ON cte.PartCode = temp.ParentCode)
SELECT *
FROM cte
UNION
SELECT *
FROM PRS_Process_BOM
WHERE PartFigureCode = '{0}';
", partCode);

            List<dynamic> result = db.Sql(sql).QueryMany<dynamic>().Where(item => item.IsSelfMade != "1").ToList();
            return result as dynamic;
        }

        public dynamic GetBOMsByPCode_Two(string partCode)
        {
            string sql = string.Format(@"
WITH cte
AS (SELECT *
    FROM PRS_Process_BOM
    WHERE ISNULL(PartFigureCode, '') = '{0}'
    UNION ALL
    SELECT temp.*
    FROM cte
        INNER JOIN PRS_Process_BOM temp
            ON cte.PartCode = temp.ParentCode)
SELECT *
FROM cte
UNION
SELECT *
FROM PRS_Process_BOM
WHERE PartFigureCode = '{0}';
", partCode);
            //所有的外购件
            List<dynamic> data = db.Sql(sql).QueryMany<dynamic>().Where(item => item.IsSelfMade != "1").ToList();

            var InventoryCodeList = data.Where(m => !string.IsNullOrEmpty(m.InventoryCode)).Select(item => "'" + item.InventoryCode + "'").ToList();

            if (InventoryCodeList.Count > 0)
            {
                string InventoryCodes = string.Join(",", InventoryCodeList);
                //外购件同步数据后的part数据
                var parts = db.Sql(string.Format(@" SELECT InventoryCode,Model,QuantityUnit,PartCode FROM dbo.SYS_Part WHERE InventoryCode IN ({0});", InventoryCodes)).QueryMany<dynamic>();

                var result = (from tbA in data
                              join tbB in parts
                              on tbA.InventoryCode equals tbB.InventoryCode
                              into tbC
                              from tbTemp in tbC.DefaultIfEmpty()
                              select new
                              {
                                  ID = tbA.ID,
                                  PartCode = tbA.PartCode,
                                  PartFigureCode = tbA.PartFigureCode,
                                  PartName = tbA.PartName,
                                  PartQuantity = tbA.PartQuantity,
                                  MaterialCode = tbA.MaterialCode,
                                  //ParentFigureCode = tbA.ParentFigureCode,
                                  Weight = tbA.Weight,
                                  Totalweight = tbA.Totalweight,
                                  InventoryCode = tbA.InventoryCode,
                                  InventoryName = tbA.InventoryName,
                                  MaterialInventoryCode = tbA.MaterialInventoryCode,
                                  MaterialInventoryName = tbA.MaterialInventoryName,
                                  MaterialQuantity = tbA.MaterialQuantity,
                                  //VersionCode = tbA.VersionCode,
                                  Remark = "",
                                  IsEnable = tbA.IsEnable,
                                  Model = tbTemp == null ? "" : tbTemp.Model,
                                  QuantityUnit = tbTemp == null ? "" : tbTemp.QuantityUnit
                              }).ToList();

                return result;
            }
            else
            {
                return data;
            }
        }

        public List<dynamic> GetProcessCardEditBomTreeGrid(string PartCode, string ContractCode)
        {
            var GyBOMList = GetPRS_Process_BOM(PartCode).Where(item => item.IsSelfMade != "0").ToList();

            if (GyBOMList.Count > 0)
            {
                #region
                /*
                 * var ppRouteData = db.Sql(string.Format("SELECT ContractCode,ProjectName,FigureCode,ProcessModelType FROM dbo.MES_BN_ProductProcessRoute WHERE IsEnable = 1 AND ContractCode = '{0}'", ContractCode)).QueryMany<MES_BN_ProductProcessRoute>();

                List<dynamic> dynamics = new List<dynamic>();
                GyBOMList.ForEach(item =>
                {
                    dynamics.Add(new
                    {
                        PartCode = item.PartCode,
                        ParentCode = item.ParentCode,
                        ID = item.ID,
                        Text = item.PartFigureCode + " " + item.PartName,
                        FigureCode = item.PartFigureCode,

                        XLNum = ppRouteData.Where(i =>
                        {
                            bool a = i.ProcessModelType == null ? false : i.ProcessModelType.Equals("1");
                            bool b = i.FigureCode == null ? false : i.FigureCode.Equals(item.PartFigureCode);
                            return a && b;
                        }).ToList().Count(),
                        HJNum = ppRouteData.Where(i =>
                        {
                            bool a = i.ProcessModelType == null ? false : i.ProcessModelType.Equals("2");
                            bool b = i.FigureCode == null ? false : i.FigureCode.Equals(item.PartFigureCode);
                            return a && b;
                        }).ToList().Count(),
                        JJGNum = ppRouteData.Where(i =>
                        {
                            bool a = i.ProcessModelType == null ? false : i.ProcessModelType.Equals("3");
                            bool b = i.FigureCode == null ? false : i.FigureCode.Equals(item.PartFigureCode);
                            return a && b;
                        }).ToList().Count(),
                        ZPNum = ppRouteData.Where(i =>
                        {
                            bool a = i.ProcessModelType == null ? false : i.ProcessModelType.Equals("4");
                            bool b = i.FigureCode == null ? false : i.FigureCode.Equals(item.PartFigureCode);
                            return a && b;
                        }).ToList().Count(),
                    }); 
                });
                return dynamics;
                */
                #endregion

                List<dynamic> dynamics = new List<dynamic>();
                GyBOMList.ForEach(item =>
                {
                    dynamics.Add(new
                    {
                        PartCode = item.PartCode,
                        ParentCode = item.ParentCode,
                        ID = item.ID,
                        Text = item.PartFigureCode + " " + item.PartName,
                        FigureCode = item.PartFigureCode,

                        XLNum = item.Blanking ?? 0,
                        HJNum = item.Welding ?? 0,
                        JJGNum = item.Machining ?? 0,
                        ZPNum = item.Assembling ?? 0
                    });
                });

                return dynamics;
            }
            else
            {
                return null;
            }
        }

        public dynamic GetUpdateProcessStatistical(string pCode, int type)
        {
            try
            {
                string sqlA = string.Format(@"SELECT ISNULL(COUNT(ID),0) FROM dbo.MES_BN_ProductProcessRoute WHERE IsEnable = 1 AND PartCode = '{0}' AND ProcessModelType = '{1}'", pCode, type);
                int count = db.Sql(sqlA).QuerySingle<int>();

                string typeName = string.Empty;

                switch (type)
                {
                    case 1: typeName = "Blanking"; break;
                    case 2: typeName = "Welding"; break;
                    case 3: typeName = "Machining"; break;
                    case 4: typeName = "Assembling"; break;
                    default: throw new Exception("参数错误！");
                }

                string sqlB = string.Format(@"UPDATE dbo.PRS_Process_BOM SET {0} = {1} WHERE PartCode = '{2}'", typeName, count, pCode);

                return new
                {
                    Result = db.Sql(sqlB).Execute() > 0
                };
            }
            catch (Exception ex)
            {
                return new
                {
                    Result = false,
                    Msg = ex.Message
                };
            }
        }

        public void GetUpdateGYBOMTree(string PartCode, string VersionCode, string ContractCode, string ProductID)
        {
            string sqlA = string.Format(@"
WITH T
AS (SELECT C.*,
           1 AS [level]
    FROM dbo.V_GetSysBomByNewVersion AS C
    WHERE C.PartCode = '{0}'
    UNION ALL
    SELECT C.*,
           T.[level] + 1 AS [level]
    FROM dbo.V_GetSysBomByNewVersion AS C
        INNER JOIN T
            ON C.ParentCode = T.PartCode)
SELECT *
FROM T
ORDER BY T.[level];
", PartCode);

            List<SYS_BOM_Expand> sysBomList = db.Sql(sqlA).QueryMany<SYS_BOM_Expand>();

            if (sysBomList.Count <= 0)
            {
                return;
            }

            int maxLevel = sysBomList.Max(item => item.Level);

            for (var i = 1; i <= maxLevel; i++)
            {
                var dataSource = sysBomList.Where(item => item.Level.Equals(i)).ToList();

                if (i.Equals(1))
                {
                    dataSource[0].TempPartQuantityAll = (dataSource[0].PartQuantity ?? 1) * 1;
                }
                else
                {
                    for (int j = 0; j < dataSource.Count; j++)
                    {
                        var parModel = sysBomList.Single(item => item.PartCode == dataSource[j].ParentCode);
                        dataSource[j].TempPartQuantityAll = (dataSource[j].PartQuantity ?? 1) * parModel.TempPartQuantityAll;
                    }
                }
            }

            using (db.UseTransaction(true))
            {
                try
                {
                    foreach (var item in sysBomList)
                    {
                        PRS_Process_BOM s = TransReflection<SYS_BOM, PRS_Process_BOM>(item);
                        s.PartQuantityAll = item.TempPartQuantityAll;

                        var sql = string.Format("select count(*) from PRS_Process_BOM where isnull(ParentCode,'')='{0}' and PartCode='{1}'", item.ParentCode, item.PartCode);
                        var isex = db.Sql(sql).QuerySingle<int>();
                        if (isex > 0)
                        {
                            var d = 0;
                            if (string.IsNullOrEmpty(item.ParentCode))
                            {
                                string updateSql = string.Format(@"
UPDATE dbo.PRS_Process_BOM
SET
PartFigureCode = '{0}',
PartName = '{1}',
PartQuantity= '{2}',
Weight = '{3}',
IsSelfMade = '{9}',
PartQuantityAll = '{10}',
MaterialCode = '{4}',
MaterialInventoryCode = '{5}',
MaterialInventoryName = '{6}',
MaterialQuantity = '{7}' WHERE PartCode = '{8}'
  AND ParentCode IS NULL ", item.PartFigureCode, item.PartName, item.PartQuantity, item.Weight, item.MaterialCode, item.MaterialInventoryCode, item.MaterialInventoryName, item.MaterialQuantity, item.PartCode, item.IsSelfMade, item.TempPartQuantityAll);
                                d = db.Sql(updateSql).Execute();
                            }
                            else
                            {
                                d = db.Update("PRS_Process_BOM")
                                   .Column("PartFigureCode", item.PartFigureCode)
                                   .Column("PartName", item.PartName)
           // .Column("InventoryCode", item.InventoryCode)
           // .Column("InventoryName", item.InventoryName)
           .Column("PartQuantity", item.PartQuantity)
           .Column("Weight", item.Weight)
           .Column("Totalweight", item.Totalweight)
           .Column("IsSelfMade", item.IsSelfMade)
           .Column("PartQuantityAll", item.TempPartQuantityAll)
           .Column("MaterialCode", item.MaterialCode)
           .Column("MaterialInventoryCode", item.MaterialInventoryCode)
           .Column("MaterialInventoryName", item.MaterialInventoryName)
           .Column("MaterialQuantity", item.MaterialQuantity).Column("IsEnable", item.IsEnable)
                                   // .Column("IsSelfMade", item.IsSelfMade)
                                   .Where("ParentCode", item.ParentCode).Where("PartCode", item.PartCode).Execute();
                            }

                            if (d == 0)
                            {
                                var sql2 = string.Format("update PRS_Process_BOM set isEnable={1} where PartCode = '{0}' and (ParentCode is null or ParentCode ='')", item.PartCode, item.IsEnable);
                                db.Sql(sql2).Execute();
                            }

                        }
                        else
                        {
                            s.ContractCode = ContractCode;
                            s.ProductID = Convert.ToInt16(ProductID);
                            db.Insert<PRS_Process_BOM>("PRS_Process_BOM", s)
           .AutoMap(x => x.ID)
           .ExecuteReturnLastId<int>();
                        }
                    }

                    StringBuilder sbSQL = new StringBuilder();

                    foreach (var item in sysBomList)
                    {
                        var sql = string.Format("update PRS_Process_BOM set ContractCode='{0}',ProductID='{1}' where PartCode = '{2}';\r\n", ContractCode, ProductID, item.PartCode);

                        var sql2 = string.Format("UPDATE PRS_Process_BOM SET IsCrux=IsSelfMade where ContractCode='{0}' and ProductID='{1}' and PartCode = '{2}';\r\n", ContractCode, ProductID, item.PartCode);

                        sbSQL.Append(sql);

                        sbSQL.Append(sql2);
                    }

                    if (!string.IsNullOrEmpty(sbSQL.ToString()))
                    {
                        db.Sql(sbSQL.ToString()).Execute();
                    }
                    db.Commit();
                }
                catch (Exception ex)
                {
                    db.Rollback();

                }
            }


            #region 备份
            /*
        //            string sqlGetBom = string.Format(@"
        //DECLARE @pCode VARCHAR(50) = '{0}';
        //DECLARE @pFigureCode VARCHAR(50) = (SELECT PartFigureCode FROM dbo.SYS_BOM WHERE PartCode = @pCode);
        //SELECT tbA.* FROM dbo.SYS_BOM tbA RIGHT JOIN dbo.fn_GETBOM(@pFigureCode) tbB ON tbA.ID = tbB.ID
        //", PartCode);

        string sqlGetBom = string.Format(@"SELECT * FROM dbo.Get_SYSBOM('{0}') ORDER BY Sort;", PartCode);

        var list = db.Sql(sqlGetBom).QueryMany<SYS_BOM>();

        //var list = GetBom(PartCode, VersionCode);

        //var notShowList = new MES_BN_MatchingTableDetailService().GetNotShowBom(PartCode, ContractCode, ProductName, "1");
        //foreach (var item in notShowList)
        //{
        //    var part = (from p in list where p.PartCode == item select p).FirstOrDefault();
        //    if (part != null)
        //    {
        //        list.Remove(part);

        //    }
        //}
        using (db.UseTransaction(true))
        {
            try
            {

                foreach (var item in list)
                {
                    PRS_Process_BOM s = TransReflection<SYS_BOM, PRS_Process_BOM>(item);

                    var sql = string.Format("select count(*) from PRS_Process_BOM where isnull(ParentCode,'')='{0}' and PartCode='{1}'", item.ParentCode, item.PartCode);
                    var isex = db.Sql(sql).QuerySingle<int>();
                    if (isex > 0)
                    {
                        var d = 0;
                        if (string.IsNullOrEmpty(item.ParentCode))
                        {
                            string updateSql = string.Format(@"
UPDATE dbo.PRS_Process_BOM
SET
PartFigureCode = '{0}',
PartName = '{1}',
PartQuantity= '{2}',
Weight = '{3}',
IsSelfMade = '{9}',
MaterialCode = '{4}',
MaterialInventoryCode = '{5}',
MaterialInventoryName = '{6}',
MaterialQuantity = '{7}' WHERE PartCode = '{8}'
  AND ParentCode IS NULL ", item.PartFigureCode, item.PartName, item.PartQuantity, item.Weight, item.MaterialCode, item.MaterialInventoryCode, item.MaterialInventoryName, item.MaterialQuantity, item.PartCode,item.IsSelfMade);
                            d = db.Sql(updateSql).Execute();
                        }
                        else
                        {
                            d = db.Update("PRS_Process_BOM")
                               .Column("PartFigureCode", item.PartFigureCode)
                               .Column("PartName", item.PartName)
       // .Column("InventoryCode", item.InventoryCode)
       // .Column("InventoryName", item.InventoryName)
       .Column("PartQuantity", item.PartQuantity)
       .Column("Weight", item.Weight)
       .Column("Totalweight", item.Totalweight)
       .Column("IsSelfMade", item.IsSelfMade)
       .Column("MaterialCode", item.MaterialCode)
       .Column("MaterialInventoryCode", item.MaterialInventoryCode)
       .Column("MaterialInventoryName", item.MaterialInventoryName)
       .Column("MaterialQuantity", item.MaterialQuantity).Column("IsEnable", item.IsEnable)
                               // .Column("IsSelfMade", item.IsSelfMade)
                               .Where("ParentCode", item.ParentCode).Where("PartCode", item.PartCode).Execute();
                        }

                        if (d == 0)
                        {
                            var sql2 = string.Format("update PRS_Process_BOM set isEnable={1} where PartCode = '{0}' and (ParentCode is null or ParentCode ='')", item.PartCode, item.IsEnable);
                            db.Sql(sql2).Execute();
                        }

                    }
                    else
                    {
                        s.ContractCode = ContractCode;
                        s.ProductID = Convert.ToInt16(ProductID);
                        db.Insert<PRS_Process_BOM>("PRS_Process_BOM", s)
       .AutoMap(x => x.ID)
       .ExecuteReturnLastId<int>();
                    }
                }

                if (list.Count <= 0)
                {
                    throw new Exception();
                }

                StringBuilder sbSQL = new StringBuilder();

                foreach (var item in list)
                {
                    var sql = string.Format("update PRS_Process_BOM set ContractCode='{0}',ProductID='{1}' where PartCode = '{2}';\r\n", ContractCode, ProductID, item.PartCode);

                    var sql2 = string.Format("UPDATE PRS_Process_BOM SET IsCrux=IsSelfMade where ContractCode='{0}' and ProductID='{1}' and PartCode = '{2}';\r\n", ContractCode, ProductID, item.PartCode);

                    sbSQL.Append(sql);

                    sbSQL.Append(sql2);
                }

                if (!string.IsNullOrEmpty(sbSQL.ToString()))
                {
                    db.Sql(sbSQL.ToString()).Execute();
                }
                db.Commit();
            }
            catch (Exception ex)
            {
                db.Rollback();

            }
        }
        */
            #endregion

        }

        public dynamic GetPBOMModel(string id, string ParentCode)
        {
            dynamic expObj = new ExpandoObject();
            expObj.child = SelectPRS_Process_BOM(id);
            if (ParentCode != null)
            {
                string sql = string.Format(@"select top 1 * from PRS_Process_BOM where PartCode=(select ParentCode from PRS_Process_BOM where id={0})", id);
                expObj.parent = db.Sql(sql).QuerySingle<PRS_Process_BOM>();
            }
            return expObj;
        }
        public dynamic GetProcessRouteModel(string ContractCode, string PartCode, string ProcessModelType)
        {
            string sql = string.Format(@"select * from MES_BN_ProductProcessRoute where ContractCode='{0}' and PartCode='{1}' and ProcessModelType='{2}' order by ProcessLineCode", ContractCode, PartCode, ProcessModelType);
            return db.Sql(sql).QueryMany<MES_BN_ProductProcessRoute>();
        }
        public int Insert(PRS_Process_BOM model)
        {
            model.CreateTime = DateTime.Now;
            model.CreatePerson = MmsHelper.GetUserCode();
            var rowsAffected = model.ID = db.Insert<PRS_Process_BOM>("PRS_Process_BOM", model)
     .AutoMap(x => x.ID)
     .ExecuteReturnLastId<int>();
            return rowsAffected;
        }
        public int Update(PRS_Process_BOM model)
        {
            var sql = String.Format(@"select * from PRS_Process_BOM where ID = '{0}'", model.ID);
            var mist = db.Sql(sql).QuerySingle<PRS_Process_BOM>();

            mist.ModifyTime = DateTime.Now;
            mist.ModifyPerson = MmsHelper.GetUserCode();
            mist.PartFigureCode = model.PartFigureCode;
            mist.PartCode = model.PartCode;
            mist.PartName = model.PartName;
            mist.Weight = model.Weight;
            mist.ParentCode = model.ParentCode;
            mist.IsEnable = model.IsEnable;
            mist.PartQuantity = model.PartQuantity;
            mist.IsSelfMade = model.IsSelfMade;
            var rowsAffected = db.Update<PRS_Process_BOM>("PRS_Process_BOM", mist)
     .AutoMap(x => x.ID)
     .Where(x => x.ID)
     .Execute();
            return rowsAffected;
        }
    }

    public class PRS_Process_BOM : ModelBase
    {
        [Identity]
        [PrimaryKey]

        /// <summary>
		/// 
		/// </summary>
		public int ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PartFigureCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PartCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PartName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string InventoryCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string InventoryName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? PartQuantity { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ParentCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Weight { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Totalweight { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string MaterialInventoryCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string MaterialInventoryName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? MaterialQuantity { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string IsSelfMade { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? IsEnable { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CreatePerson { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ModifyPerson { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? ModifyTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ApproveState { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ApprovePerson { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ApproveDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ApproveRemark { get; set; }

        /// <summary>
        /// 定料材料
        /// </summary>
        public string SetMateName { get; set; }

        /// <summary>
        /// 定料数量
        /// </summary>
        public double? SetMateNum { get; set; }

        /// <summary>
        /// 到位尺寸
        /// </summary>
        public string InPlanceSize { get; set; }

        /// <summary>
        /// 下料尺寸
        /// </summary>
        public string BlankingSize { get; set; }

        /// <summary>
        /// 材料类型:板材、棒材、其他
        /// </summary>
        public int? MateType { get; set; }

        /// <summary>
        /// 下料方式:数控切割、锯、手工切割
        /// </summary>
        public int? BlankingType { get; set; }

        /// <summary>
        /// 规格_新
        /// </summary>
        public string New_Specs { get; set; }

        /// <summary>
        /// 型号_新
        /// </summary>
        public string New_Model { get; set; }

        /// <summary>
        /// 名称_新
        /// </summary>
        public string New_PartName { get; set; }

        /// <summary>
        /// 材料参数：厚度或直径
        /// </summary>
        public string MateParamValue { get; set; }

        /// <summary>
        /// 合同编号
        /// </summary>
        public string ContractCode { get; set; }

        /// <summary>
        /// 产品ID
        /// </summary>
        public int? ProductID { get; set; }

        /// <summary>
        /// 零件编号(新)
        /// </summary>
        public string New_InventoryCode { get; set; }

        /// <summary>
        /// 工艺下料
        /// </summary>
        public int? Blanking { get; set; }

        /// <summary>
        /// 工艺焊接
        /// </summary>
        public int? Welding { get; set; }

        /// <summary>
        /// 工艺机加
        /// </summary>
        public int? Machining { get; set; }

        /// <summary>
        /// 工艺装配
        /// </summary>
        public int? Assembling { get; set; }

        /// <summary>
        /// 需求下料数量
        /// </summary>
        public int? BlankingTotal { get; set; }

        /// <summary>
        /// 下料数量
        /// </summary>
        public int? BlankingNum { get; set; }

        /// <summary> 
		/// 外购数量 
		/// </summary> 
		public int? PurchaseNum { get; set; }

        /// <summary> 
        /// 改制数量 
        /// </summary> 
        public int? RestructNum { get; set; }

        /// <summary>
		/// 排序
		/// </summary>
		public int? Sort { get; set; }
        public int? IsCrux { get; set; }

        public int? PartQuantityAll { get; set; }
    }
}
