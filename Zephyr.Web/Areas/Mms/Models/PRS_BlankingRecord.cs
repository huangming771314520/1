using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using Zephyr.Areas.Areas.Mms.Common;
using Zephyr.Core;
using Zephyr.Utils.ExcelLibrary.BinaryFileFormat;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class PRS_BlankingRecordService : ServiceBase<PRS_BlankingRecord>
    {
        public int AuditBillCode(string mainID, out string msg)
        {
            msg = string.Empty;
            var rowsAffected = 0;
            string sql = String.Format(@"  select BillState FROM PRS_BlankingRecord  where ID='{0}'", mainID);
            var state = db.Sql(sql).QuerySingle<int>();
            if (state == 2)
            {
                msg = "单据已审核";
                return 0;
            }
            else
            {
                try
                {
                    using (db.UseTransaction(true))
                    {
                        int isFalse = 0;
                        sql = string.Format(@"update PRS_BlankingRecord set BillState=2 where ID='{0}'", mainID);
                        rowsAffected = db.Sql(sql).Execute();
                        if (rowsAffected > 0)
                        {
                            msg = "生产请购单据审核成功";
                            var pQuery = ParamQuery.Instance().AndWhere("IsEnable", 1).AndWhere("MainID", mainID);
                            List<PRS_BlankingResult> resList = new PRS_BlankingResultService().GetModelList(pQuery);
                            foreach (var result in resList)
                            {
                                for (int i = 0; i < result.PartBlankingQuntity; i++)
                                {
                                    pQuery = ParamQuery.Instance().AndWhere("IsEnable", 1).AndWhere("MainID", result.ID);
                                    List<PRS_BlankingPlateDetail> plateList = new PRS_BlankingPlateDetailService().GetModelList(pQuery);
                                    foreach (var plate in plateList)
                                    {
                                        int res = db.Insert("Mes_BlankingResult")
                                          .Column("BlankingResultID", plate.MainID)
                                          .Column("BlankingQuantity", 1)
                                          .Column("IsEnable", 1)
                                          .Column("Ispicking", 0)
                                          .Column("BiankingSize", plate.PlateSize)
                                          .Column("CreateTime", DateTime.Now)
                                          .Column("CreatePerson", MmsHelper.GetUserName())
                                          .Column("ModifyTime", DateTime.Now)
                                          .Column("ModifyPerson", MmsHelper.GetUserName()).Execute();
                                        if (res <= 0)
                                        {
                                            isFalse++;
                                        }
                                    }
                                }

                            }
                            //return rowsAffected;

                        }
                        else
                        {
                            msg = "生产请购单据审核失败,请先保存数据后审核";
                            db.Rollback();
                            return 0;
                        }
                        if (isFalse > 0)
                        {
                            db.Rollback();
                            return 0;
                        }
                        else
                        {
                            db.Commit();
                            msg = "生产请购单据审核成功";
                            return 1;
                        }
                    }
                }
                catch
                {
                    msg = "生产请购单据审核失败";
                    return 0;
                }


            }
        }

        public ResultModel GetSavantManageData()
        {
            try
            {
                string sql = string.Format(@"
SELECT 
tbA.*,
tbB.SetMateName,--定料材料
tbB.New_PartName,--材料名称
tbb.MateType,--材料类型
tbB.MateParamValue,--材料参数：厚度或直径
tbB.PartFigureCode,--零件图号
tbB.PartName,--零件名称
tbB.InPlanceSize,--到位尺寸
ISNULL((SELECT SUM(ISNULL(BlankingNum,0)) FROM dbo.MES_SavantAndPBom WHERE SavantCode = tbA.SavantCode),0) AS PartTotalNum--零件总数
FROM dbo.MES_SavantManage tbA LEFT JOIN dbo.PRS_Process_BOM_Blanking tbB
ON tbA.ProcessBomID = tbB.ID AND tbB.IsEnable = 1
WHERE tbA.IsEnable = 1
");

                return new ResultModel()
                {
                    Result = true,
                    Data = db.Sql(sql).QueryMany<dynamic>()
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

        public ResultModel GetSavantAndPBomData(string savantCode)
        {
            try
            {
                string sql = string.Format(@"
SELECT 
tbA.*,
tbB.ContractCode,--合同编号
tbC.ProjectName,--工程项目
tbD.ProductName,--产品
tbB.PartFigureCode,--零件图号
tbB.PartName,--零件名称
tbB.MaterialCode,--设计材料
tbB.BlankingNum AS pbom_BlankingNum,--下料数量
(SELECT SUM(ISNULL(BlankingNum,0)) FROM dbo.MES_SavantAndPBom WHERE ProcessBomID = tbA.ProcessBomID) - ISNULL(tbA.BlankingNum,0) AS AlreadyBlankingNum,--已下料数量
tbB.SetMateName,--定料材料
tbB.New_PartName,--材料名称
tbB.MateType,--材料类型
tbB.MateParamValue,--材料参数：厚度或直径
tbB.InPlanceSize--到位尺寸
FROM dbo.MES_SavantAndPBom tbA LEFT JOIN dbo.PRS_Process_BOM_Blanking tbB
ON tbA.ProcessBomID = tbB.ID AND tbB.IsEnable = 1 LEFT JOIN dbo.PMS_BN_Project tbC
ON tbB.ContractCode = tbC.ContractCode AND tbC.IsEnable = 1 LEFT JOIN dbo.PMS_BN_ProjectDetail tbD
ON tbB.ProductID = tbD.ID
WHERE tbA.SavantCode = '{0}'
", savantCode);

                return new ResultModel()
                {
                    Result = true,
                    Data = db.Sql(sql).QueryMany<dynamic>()
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

        public ResultModel GetDelSavantAndPBomData(string ids)
        {
            try
            {
                db.UseTransaction(true);

                string sCode = db.Sql(string.Format(@"SELECT SavantCode FROM dbo.MES_SavantAndPBom WHERE ID = {0}", ids.Split(',')[0])).QuerySingle<string>();

                string sqlA = string.Format(@"DELETE dbo.MES_SavantAndPBom WHERE ID IN ({0})", ids);
                int numA = db.Sql(sqlA).Execute();

                string sqlB = string.Format(@"
UPDATE dbo.MES_SavantManage
SET SpareMateNum = 1,
    SplitNum =
    (
        SELECT SUM(BlankingNum)
        FROM dbo.MES_SavantAndPBom
        WHERE SavantCode = '{0}'
    );
", sCode);
                int numB = db.Sql(sqlB).Execute();

                bool result = numA > 0 && numB > 0;

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
                    Msg = result ? @"删除成功！" : @"删除失败！"
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

        public dynamic AddSavantAndPBomData(string ids, string savantCode)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                DateTime newDT = DateTime.Now;

                ids.Split(',').ToList().ForEach(item =>
                {
                    string[] arr = item.Split('|');

                    sb.Append(WinFormClientService.GetInsertSQL(new MES_SavantAndPBom()
                    {
                        SavantCode = savantCode,
                        ProcessBomID = Convert.ToInt32(arr[0]),
                        BlankingNum = Convert.ToInt32(arr[1]),
                        CreatePerson = MmsHelper.GetUserName(),
                        CreateTime = newDT,
                        ModifyPerson = MmsHelper.GetUserName(),
                        ModifyTime = newDT
                    }));
                });

                sb.Append(string.Format(@"
UPDATE dbo.MES_SavantManage
SET SpareMateNum = 1,
    SplitNum =
    (
        SELECT SUM(BlankingNum)
        FROM dbo.MES_SavantAndPBom
        WHERE SavantCode = '{0}'
    );
", savantCode));

                string sql = sb.ToString();

                return new ResultModel()
                {
                    Result = db.Sql(sql).Execute() > 0,
                    Msg = @"新增成功！"
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

        public ResultModel UpdateSavantAndPBomNum(long id, int num)
        {
            try
            {
                MES_SavantAndPBom modelA = db.Sql(string.Format(@"SELECT * FROM MES_SavantAndPBom WHERE ID = {0}", id)).QuerySingle<MES_SavantAndPBom>();

                if (modelA == null)
                {
                    throw new Exception("查不到 MES_SavantAndPBom 完整数据");
                }
                else
                {
                    PRS_Process_BOM_Blanking modelB = db.Sql(string.Format(@"SELECT * FROM dbo.PRS_Process_BOM_Blanking WHERE IsEnable = 1 AND ID = {0}", modelA.ProcessBomID)).QuerySingle<PRS_Process_BOM_Blanking>();

                    if (modelB == null)
                    {
                        throw new Exception("查不到 PRS_Process_BOM_Blanking 完整数据");
                    }
                    else if (modelB.BlankingNum == null)
                    {
                        throw new Exception("没有维护数据");
                    }
                    else
                    {
                        int alreadyBlankingNum = db.Sql(string.Format(@"SELECT tbTemp.AlreadyBlankingNum FROM (SELECT ProcessBomID,SUM(ISNULL(BlankingNum,0)) AS AlreadyBlankingNum FROM dbo.MES_SavantAndPBom WHERE ID NOT IN ({0}) GROUP BY ProcessBomID) tbTemp WHERE tbTemp.ProcessBomID = {1}", modelA.ID, modelA.ProcessBomID)).QuerySingle<int>();

                        if (num >= 0 && num <= (modelB.BlankingNum - alreadyBlankingNum))
                        {
                            string sql = WinFormClientService.GetUpdateSQL(nameof(MES_SavantAndPBom), new KeyValuePair<string, object>("ID", id), new
                            {
                                BlankingNum = num,
                                ModifyPerson = MmsHelper.GetUserName(),
                                ModifyTime = DateTime.Now
                            });

                            return new ResultModel()
                            {
                                Result = db.Sql(sql).Execute() > 0,
                                Msg = @"保存成功！"
                            };
                        }
                        else
                        {
                            throw new Exception("无效的数量，请重新填写！");
                        }
                    }

                }
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

        public ResultModel UpdateSavantManageData(dynamic data)
        {
            try
            {
                if (Convert.ToInt32(data.PartTotalNum) % Convert.ToInt32(data.SpareMateNum == null ? 1 : data.SpareMateNum) != 0)
                {
                    throw new Exception(@"切分数量不为整数，请重新填写！");
                }

                string sql = WinFormClientService.GetUpdateSQL(nameof(MES_SavantManage), new KeyValuePair<string, object>("ID", data.ID), new
                {
                    SpareMateSize = data.SpareMateSize,
                    SpareMateNum = data.SpareMateNum,
                    //SplitNum = data.SplitNum,
                    SplitNum = data.PartTotalNum / ((int?)data.SpareMateNum ?? 1),
                    Remark = data.Remark
                });

                return new ResultModel()
                {
                    Result = db.Sql(sql).Execute() > 0,
                    Msg = @"保存成功！"
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

        public ResultModel SaveBlankingPlanData(dynamic data)
        {
            try
            {
                //List<int> productIDs = new List<int>();
                //List<int> pbomIDs = new List<int>();
                //
                //foreach (var item in data)
                //{
                //    productIDs.Add(data[0].product_ID);
                //    pbomIDs.Add(data[0].pbom_ID);
                //}
                //
                //List<PMS_BN_ProjectDetail> listA = db.Sql(string.Format(@"SELECT * FROM dbo.PMS_BN_ProjectDetail WHERE IsEnable = 1 AND ID IN ({0})", string.Join(",", productIDs))).QueryMany<PMS_BN_ProjectDetail>();
                //List<PRS_Process_BOM> listB = db.Sql(string.Format(@"SELECT * FROM dbo.PRS_Process_BOM WHERE IsEnable = 1 AND ID IN ({0})", string.Join(",", pbomIDs))).QueryMany<PRS_Process_BOM>();

                StringBuilder sb = new StringBuilder();

                string savantCode = MmsHelper.GetOrderNumber("MES_SavantManage", "SavantCode", "ZJJ", "", "");
                DateTime newDT = DateTime.Now;

                int partTotalNum = 0;
                foreach (var item in data)
                {
                    //PMS_BN_ProjectDetail product = listA.Single(i => i.ID.Equals(item.product_ID));
                    //PRS_Process_BOM pbom = listB.Single(i => i.ID.Equals(item.pbom_ID));

                    //零件总数
                    partTotalNum += ((int?)item.BlankingNum ?? 0);

                    sb.Append(WinFormClientService.GetInsertSQL(new MES_SavantAndPBom()
                    {
                        SavantCode = savantCode,
                        ProcessBomID = item.pbom_ID,
                        BlankingNum = item.BlankingNum,
                        CreatePerson = MmsHelper.GetUserName(),
                        CreateTime = newDT,
                        ModifyPerson = MmsHelper.GetUserName(),
                        ModifyTime = newDT
                    }));
                }

                string iCode = new SYS_PartService().GetSysPartAutoMaxICode("030502");

                sb.Append(WinFormClientService.GetInsertSQL(new MES_SavantManage()
                {
                    SavantCode = savantCode,
                    ProcessBomID = data[0].pbom_ID,
                    SpareMateSize = data[0].BlankingSize,
                    SpareMateNum = partTotalNum,//备料数量
                    IsEnable = 1,
                    CreatePerson = MmsHelper.GetUserName(),
                    CreateTime = newDT,
                    ModifyPerson = MmsHelper.GetUserName(),
                    ModifyTime = newDT,
                    SplitNum = 1,//切分数量
                    InventoryCode = iCode
                }));

                sb.Append(WinFormClientService.GetInsertSQL(new SYS_Part()
                {
                    InventoryCode = iCode,
                    InventoryName = data[0].SetMateName,
                    Model = data[0].BlankingSize,
                    IsEnable = 1,
                    CreatePerson = MmsHelper.GetUserName(),
                    CreateTime = newDT,
                    ModifyPerson = MmsHelper.GetUserName(),
                    ModifyTime = newDT
                }));

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
                    Msg = result ? @"操作成功！" : @"操作失败！"
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


        public ResultModel GetBlankingProcessCardData(string cCode, string productName, string pFigure, string pName, string savantCode)
        {
            try
            {
                string where = string.Empty;

                if (!string.IsNullOrEmpty(cCode))
                {
                    where += $" AND tableB.ContractCode LIKE '%{cCode}%'";
                }
                if (!string.IsNullOrEmpty(productName))
                {
                    where += $" AND MAX(tableB.ProductName) LIKE '%{productName}%'";
                }
                if (!string.IsNullOrEmpty(pFigure))
                {
                    where += $" AND MAX(tableA.PartFigureCode) LIKE '%{pFigure}%'";
                }
                if (!string.IsNullOrEmpty(pName))
                {
                    where += $" AND MAX(tableA.PartName) LIKE '%{pName}%'";
                }
                if (!string.IsNullOrEmpty(savantCode))
                {
                    where += $" AND tableA.SavantCode LIKE '%{savantCode}%'";
                }

                #region SQL语句

                string sql = string.Format(@"
SELECT tableB.ContractCode,
       MAX(tableB.ProjectName) AS ProjectName,
       MAX(tableB.ProductName) AS ProductName,
       MAX(tableA.PartFigureCode) AS PartFigureCode,
       MAX(tableA.PartName) AS PartName,
       tableA.SavantCode,
       MAX(tableA.PartCode) AS PartCode,
       tableB.ProductID
FROM
(
    SELECT tbA.ID AS SavantManageID,
           tbA.SavantCode,
           tbA.ProcessBomID,
           tbB.PartCode,       --零件编码
           tbB.PartFigureCode, --零件图号
           tbB.PartName        --零件名称
    FROM dbo.MES_SavantManage tbA
        LEFT JOIN dbo.PRS_Process_BOM_Blanking tbB
            ON tbA.ProcessBomID = tbB.ID
               AND tbB.IsEnable = 1
    WHERE tbA.IsEnable = 1
) tableA
    LEFT JOIN
    (
        SELECT tbA.ID AS SavantPBomID,
               tbA.SavantCode,
               tbA.ProcessBomID,
               tbB.ContractCode,    --合同编号
               tbC.ProjectID,       --工程项目ID
               tbC.ProjectName,     --工程项目
               tbD.ID AS ProductID, --产品
               tbD.ProductName      --产品
        FROM dbo.MES_SavantAndPBom tbA
            LEFT JOIN dbo.PRS_Process_BOM_Blanking tbB
                ON tbA.ProcessBomID = tbB.ID
                   AND tbB.IsEnable = 1
            LEFT JOIN dbo.PMS_BN_Project tbC
                ON tbB.ContractCode = tbC.ContractCode
                   AND tbC.IsEnable = 1
            LEFT JOIN dbo.PMS_BN_ProjectDetail tbD
                ON tbB.ProductID = tbD.ID
    ) tableB
        ON tableA.SavantCode = tableB.SavantCode
GROUP BY tableA.SavantCode,
         tableB.ContractCode,
         tableB.ProductID
HAVING 1 = 1 {0}
", where);

                #endregion

                return new ResultModel()
                {
                    Result = true,
                    Data = db.Sql(sql).QueryMany<dynamic>()
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

    }

    public class PRS_BlankingRecord : ModelBase
    {
        [Identity]
        [PrimaryKey]
        public int ID { get; set; }
        public string ContractCode { get; set; }
        public string PartCode { get; set; }
        public string FigureCode { get; set; }
        public string PartName { get; set; }
        public int? SingleQuantity { get; set; }
        public int? TotalQuantity { get; set; }
        public int? BlankedQuantity { get; set; }
        public int? NoBlankingQuantity { get; set; }
        public string MaterialCode { get; set; }
        public string InPlanceSize { get; set; }
        public string BlankingSize { get; set; }
        public string Process { get; set; }
        public string BatchNumber { get; set; }
        public string Remark { get; set; }
        public int? IsEnable { get; set; }
        public DateTime? CreateTime { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? ModifyTime { get; set; }
        public string ModifyPerson { get; set; }
        public string InventoryCode { get; set; }
        /// <summary>
        /// 单据状态
        /// </summary>
        public string BillState { get; set; }


    }
}
