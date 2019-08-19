using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using Zephyr.Areas.Helpers;
using Zephyr.Core;
using System.Linq;
using sysnum = System.Numerics;
using System.Text.RegularExpressions;
using Zephyr.Areas.Areas.Mms.Common;
using Zephyr.Data;
using Zephyr.Utils.NPOI.SS.Formula.Functions;
using Zephyr.Utils.ExcelLibrary.BinaryFileFormat;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class SYS_PartService : ServiceBase<SYS_Part>
    {
        public dynamic CreateInventoryCode(dynamic data, string referenceCode)
        {
            dynamic result = new { status = 1, msg = "提交成功!" };
            //var sys_part_list = new SYS_PartService().GetModelList();
            string msg = "提交成功！";
            //var inventorycodeList = sys_part_list.Select(a => a.InventoryCode).ToList();
            try
            {
                foreach (dynamic item in data)
                {
                    string InventoryCode = item.InventoryCode;
                    string InventoryName = item.InventoryName;
                    string Model = item.Model;
                    string Spec = item.Spec;
                    string PartType = item.PartType;
                    int ID = item.ID;
                    string warehouseCode = item.WarehouseCode;
                    string warehouseName = item.WarehouseName;
                    string quantityUnit = item.QuantityUnit;
                    var querySql = string.Format(@"SELECT * FROM dbo.SYS_Part WHERE InventoryCode = '{0}'", InventoryCode);
                    var listA = db.Sql(string.Format(@"SELECT * FROM dbo.SYS_Part WHERE InventoryCode = '{0}'", InventoryCode)).QueryMany<SYS_Part>();

                    if (listA.Count == 0)
                    {
                        //db.Insert("SYS_Part")
                        //    .Column("InventoryCode", InventoryCode)
                        //    .Column("InventoryName", InventoryName)
                        //    .Column("Model", Model).Column("Spec", Spec).Column("IsEnable", 1).ExecuteReturnLastId<int>();
                        //db.Update(PartType).Column("New_InventoryCode", InventoryCode).Column("IsEnable", 1).Where("ID=" + ID).Execute(); ;

                        var model = db.Sql(string.Format(@"SELECT * FROM dbo.SYS_Part WHERE InventoryCode = '{0}'", referenceCode)).QuerySingle<SYS_Part>();

                        string sqlA = WinFormClientService.GetInsertSQL(new SYS_Part()
                        {
                            InventoryCode = InventoryCode,
                            InventoryName = InventoryName,
                            Model = Model,
                            Spec = Spec,
                            IsEnable = 1,
                            PartTypeID = model.PartTypeID,
                            PurchaseAdvanceTime = model.PurchaseAdvanceTime,
                            //QuantityUnit = model.QuantityUnit,
                            QuantityUnit = quantityUnit,
                            IsSelfMade = model.IsSelfMade,
                            IsSupplyMade = model.IsSupplyMade,
                            IsCastforgeMatch = model.IsCastforgeMatch,
                            IsElectroHydraulicMatch = model.IsElectroHydraulicMatch,
                            IsMarketMatch = model.IsMarketMatch,
                            IsOutsouceWeiding = model.IsOutsouceWeiding,
                            WarehouseCode = warehouseCode,
                            WarehouseName = warehouseName,
                        });

                        Dictionary<string, object> whereB = new Dictionary<string, object>();
                        switch (PartType)
                        {
                            case "PRS_BoardCreateMate":
                                whereB.Add("New_InventoryName", InventoryName);
                                whereB.Add("New_Model", Model);
                                break;
                            case "PRS_Process_BOM_Blanking":
                                whereB.Add("New_PartName", InventoryName);
                                whereB.Add("New_Model", Model);
                                break;
                            case "MES_WorkshopPurchaseDetail":
                                whereB.Add("MeterialName", InventoryName);
                                whereB.Add("WriteModel", Model);
                                break;
                            default: break;
                        }
                        //whereB.Add("New_PartName", InventoryName);
                        //whereB.Add("New_Model", Model);
                        string sqlB = WinFormClientService.GetUpdateSQL(PartType, whereB, new
                        {
                            New_InventoryCode = InventoryCode,
                            IsEnable = 1
                        });

                        db.UseTransaction(true);
                        bool numA = db.Sql(sqlA).Execute() > 0;
                        bool numB = db.Sql(sqlB).Execute() > 0;

                        if (numA && numB)
                        {
                            db.Commit();
                        }
                        else
                        {
                            db.Rollback();
                        }
                    }
                    else
                    {
                        //db.Update(PartType).Column("New_InventoryCode", InventoryCode).Column("IsEnable", 1).Where("ID=" + ID).Execute();
                        msg = "提交成功:" + "但部分存货编码重复";
                        //result = new { status = 1, msg =  "提交成功:" + "但部分存货编码重复" };
                    }
                }

            }
            catch (Exception ex)
            {
                msg = "提交失败:" + ex.Message;
                result = new { status = 1, msg = "提交失败:" + ex.Message };
            }
            result = new { status = 1, msg = msg };
            return result;
        }

        protected override void OnAfterHandleExcel(ref DataTable dtSheet)
        {
            var service = new SYS_PartService();
            var prsbomServices = new PRS_Process_BOMService();
            foreach (DataRow item in dtSheet.Rows)
            {
                var PartCode = item["InventoryCode"].ToString();

                List<PRS_Process_BOM> part = prsbomServices.GetModelList(ParamQuery.Instance().AndWhere("InventoryCode", PartCode).AndWhere("IsEnable", 1));
                if (part.Count > 0)
                {
                    item["PartCode"] = part[part.Count - 1].PartCode;
                    item["PartName"] = part[part.Count - 1].PartName;
                    item["Weight"] = part[part.Count - 1].Weight;

                    var Query = ParamDelete.Instance().AndWhere("InventoryCode", PartCode);
                    service.Delete(Query);
                }


                string dName = item["WarehouseCode"].ToString();
                string sql = string.Format(@"select WarehouseName from SYS_BN_Warehouse where WarehouseCode='{0}'", dName);
                string hID = db.Sql(sql).QuerySingle<string>();
                item["WarehouseName"] = hID;

                string isSelfMade = item["IsSelfMade"].ToString();
                if (isSelfMade == "否")
                {
                    item["IsSelfMade"] = 0;
                }
                else
                    item["IsSelfMade"] = 1;


                string isSupplyMade = item["IsSupplyMade"].ToString();
                if (isSupplyMade == "否")
                {
                    item["IsSupplyMade"] = 0;
                }
                else
                    item["IsSupplyMade"] = 1;

                string isCastforgeMatch = item["IsCastforgeMatch"].ToString();
                if (isCastforgeMatch == "否")
                {
                    item["IsCastforgeMatch"] = 0;
                }
                else
                    item["IsCastforgeMatch"] = 1;

                string isOutsouceWeiding = item["IsOutsouceWeiding"].ToString();
                if (isOutsouceWeiding == "否")
                {
                    item["IsOutsouceWeiding"] = 0;
                }
                else
                    item["IsOutsouceWeiding"] = 1;

                string isElectroHydraulicMatch = item["IsElectroHydraulicMatch"].ToString();
                if (isElectroHydraulicMatch == "否")
                {
                    item["IsElectroHydraulicMatch"] = 0;
                }
                else
                    item["IsElectroHydraulicMatch"] = 1;


                string isMarketMatch = item["IsMarketMatch"].ToString();
                if (isMarketMatch == "否")
                {
                    item["IsMarketMatch"] = 0;
                }
                else
                    item["IsMarketMatch"] = 1;

                string isEnable = item["IsEnable"].ToString();
                if (isEnable == "否")
                {
                    item["IsEnable"] = 0;
                }
                else
                    item["IsEnable"] = 1;
                //dName = item["PartTypeCode"].ToString();
                //sql = string.Format(@"select PartTypeCode from SYS_PartType where TypeName='{0}'", dName);
                //hID = db.Sql(sql).QuerySingle<string>();
                //item["PartTypeCode"] = hID;


            }
            base.OnAfterHandleExcel(ref dtSheet);

        }


        public List<dynamic> GetSysPartByCodeAndType(string partCode, string partType)
        {
            string where = string.Empty;
            if (!string.IsNullOrEmpty(partCode))
            {
                where += string.Format(" AND tbA.PartCode LIKE '%{0}%' ", partCode);
            }
            if (!string.IsNullOrEmpty(partType))
            {
                where += string.Format(" AND tbA.PartTypeID LIKE '%{0}%' ", partType);
            }

            string sql = string.Format(@"
SELECT tbA.*,tbB.TypeName
FROM dbo.SYS_Part AS tbA
    LEFT JOIN dbo.SYS_PartType AS tbB
        ON tbA.PartTypeID = tbB.PartTypeCode
WHERE 1 = 1 {0}
", where);

            using (var db = Db.Context("Mms"))
            {
                return db.Sql(sql).QueryMany<dynamic>();
            }
        }

        public dynamic GetUpdatePartBarCodeByPartCode(string partCode)
        {
            if (string.IsNullOrEmpty(partCode))
            {
                return new
                {
                    Result = false,
                    BarCode = ""
                };
            }

            string sqlA = string.Format(@"
SELECT *
FROM dbo.SYS_Part
WHERE PartCode = '{0}'
      AND
      (
          CorrespondingBarcode IS NULL
          OR CorrespondingBarcode = ''
      );
", partCode);

            using (var db = Db.Context("Mms"))
            {
                if (db.Sql(sqlA).QueryMany<dynamic>().Count > 0)
                {
                    string sqlB = string.Format(@"
DECLARE @barcode VARCHAR(50) =
        (
            SELECT RIGHT(REPLICATE('0', 12) +
                         (
                             SELECT CONVERT(VARCHAR(50), CONVERT(BIGINT, ISNULL(MAX(CorrespondingBarcode), '0')) + 1)
                             FROM dbo.SYS_Part
                         ), 12)
        );
UPDATE dbo.SYS_Part
SET CorrespondingBarcode = @barcode
WHERE PartCode = '{0}'
      AND
      (
          CorrespondingBarcode IS NULL
          OR CorrespondingBarcode = ''
      );
SELECT @barcode;
", partCode);
                    return new
                    {
                        Result = true,
                        BarCode = db.Sql(sqlB).QuerySingle<string>()
                    };
                }
                else
                {
                    return new
                    {
                        Result = false,
                        BarCode = ""
                    };
                }
            }

        }

        public dynamic GetQuantityUnitList()
        {
            string sql = @"SELECT DISTINCT QuantityUnit FROM dbo.SYS_Part WHERE QuantityUnit IS NOT NULL AND QuantityUnit <> ''";
            return db.Sql(sql).QueryMany<dynamic>();
        }

        public dynamic GetSimBomByPName(string partName, int page, int rows)
        {
            int maxSimilarity = 4;
            int startIndex = (page - 1) * rows;
            int endIndex = page * rows;

            using (var db = Db.Context("Mms"))
            {
                string sqlA = "SELECT ID as PartID,SimHash FROM dbo.SYS_Part WHERE IsEnable = 1 AND SimHash IS NOT NULL";

                List<PartTemp> GetParts = db.Sql(sqlA).QueryMany<PartTemp>();

                partName = Regex.Replace(partName, @"\s", "");
                partName = Regex.Replace(partName, @"[^\u4e00-\u9fa5].*?", "");
                var simhash = new SimhashHelper.SimHash(partName);

                for (int i = 0; i < GetParts.Count; i++)
                {
                    GetParts[i].Similarity = simhash.HammingDistance(GetParts[i].SimHash);
                }

                GetParts.RemoveAll(item => item.Similarity > maxSimilarity);
                GetParts.Sort((a, b) => a.Similarity.CompareTo(b.Similarity));

                int totalNum = GetParts.Count % rows == 0 ? GetParts.Count / rows : (GetParts.Count / rows + 1);
                int totalRows = GetParts.Count - (totalNum - 1) * rows;
                List<PartTemp> list = totalNum.Equals(0) ? new List<PartTemp>() : GetParts.GetRange(startIndex, page.Equals(totalNum) ? totalRows : rows);

                List<int> partIDs = new List<int>();

                list.ForEach(item =>
                {
                    partIDs.Add(item.PartID);
                });

                //               string sqlB = string.Format(@" SELECT t1.*,t2.TypeName FROM dbo.SYS_Part t1 
                //inner join SYS_PartType t2 
                //on t1.parttypeID=t2.PartTypeCode
                //and t1.IsEnable=1 WHERE t1.ID IN ({0})", string.Join(",", partIDs));
                string sqlB = string.Format(@"
SELECT *
FROM dbo.SYS_Part tbA
    LEFT JOIN dbo.SYS_PartType tbB
        ON tbA.PartTypeID = tbB.PartTypeCode
           AND tbB.IsEnable = 1
WHERE tbA.IsEnable = 1
      AND tbA.ID IN ( {0} );
", string.Join(",", partIDs));

                List<dynamic> sYS_Parts = partIDs.Count.Equals(0) ? new List<dynamic>() : db.Sql(sqlB).QueryMany<dynamic>();

                List<dynamic> dynamics = new List<dynamic>();

                list.ForEach(item =>
                {
                    dynamic model = sYS_Parts.SingleOrDefault(i => i.ID.Equals(item.PartID));
                    if (model == null)
                        return;
                    dynamics.Add(new
                    {
                        ID = model.ID,
                        InventoryCode = model.InventoryCode,
                        InventoryName = model.InventoryName,
                        Spec = model.Spec,
                        Model = model.Model,
                        PartTypeID = model.PartTypeID,
                        TypeName = model.TypeName,
                        Unit = model.QuantityUnit,
                        Similarity = item.Similarity
                    });
                });

                return new
                {
                    total = GetParts.Count,
                    rows = dynamics
                };
            }
        }

        public dynamic GetPartListByKeyWord(string InventoryCode, string InventoryName, string Spec, string Model, int page, int rows)
        {
            int startIndex = (page - 1) * rows + 1;
            int endIndex = page * rows;

            using (var db = Db.Context("Mms"))
            {
                string where = string.Empty;

                if (!string.IsNullOrEmpty(InventoryCode))
                {
                    where += string.Format(@" and InventoryCode like '%{0}%' ", InventoryCode);
                }
                if (!string.IsNullOrEmpty(InventoryName))
                {
                    where += string.Format(@" and InventoryName like '%{0}%' ", InventoryName);
                }
                if (!string.IsNullOrEmpty(Spec))
                {
                    where += string.Format(@" and Spec like '%{0}%' ", Spec);
                }
                if (!string.IsNullOrEmpty(Model))
                {
                    where += string.Format(@" and Model like '%{0}%' ", Model);
                }

                //               string sqlA = string.Format(@"SELECT count(t1.ID) FROM dbo.SYS_Part t1 
                //inner join SYS_PartType t2 
                //on t1.PartTypeID=t2.PartTypeCode
                //and t1.IsEnable=1", where);
                //               string sqlB = string.Format(@"SELECT * FROM (SELECT t1.*,TypeName, DENSE_RANK() OVER(ORDER BY t1.id) [row] FROM dbo.SYS_Part t1 inner join SYS_PartType t2 
                //on t1.parttypeID=t2.PartTypeCode
                //and t1.IsEnable=1 where 1=1 {0}) tb WHERE tb.row BETWEEN {1} AND {2}", where, startIndex, endIndex);

                string sqlA = string.Format(@"SELECT COUNT(ID) FROM dbo.SYS_Part WHERE IsEnable = 1 {0}", where);
                string sqlB = string.Format(@"
SELECT *
FROM
(
    SELECT tbA.*,
           tbB.TypeName,
           DENSE_RANK() OVER (ORDER BY tbA.ID) [row]
    FROM dbo.SYS_Part tbA
        LEFT JOIN dbo.SYS_PartType tbB
            ON tbA.PartTypeID = tbB.PartTypeCode
               AND tbB.IsEnable = 1
    WHERE tbA.IsEnable = 1
          {0}
) tbTemp
WHERE tbTemp.[row]
BETWEEN {1} AND {2};
", where, startIndex, endIndex);

                int total = db.Sql(sqlA).QuerySingle<int>();
                List<dynamic> sYS_Parts = db.Sql(sqlB).QueryMany<dynamic>();

                List<dynamic> dynamics = new List<dynamic>();

                sYS_Parts.ForEach(item =>
                {
                    dynamics.Add(new
                    {
                        ID = item.ID,
                        InventoryCode = item.InventoryCode,
                        InventoryName = item.InventoryName,
                        Spec = item.Spec,
                        Model = item.Model,
                        Unit = item.QuantityUnit
                    });
                });

                return new
                {
                    total = total,
                    rows = dynamics
                };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="partID">SYS_Part主键ID</param>
        /// <param name="weight">PRS_Process_BOM的Weight字段</param>
        /// <param name="PartName">PRS_Process_BOM的PartName字段</param>
        /// <param name="PartCode">PRS_Process_BOM的PartCode字段</param>
        /// <param name="BomID">PRS_Process_BOM主键ID</param>
        /// <param name="PartICode">SYS_Part的InventoryCode</param>
        /// <param name="PartIName">SYS_Part的InventoryName</param>
        /// <returns></returns>
        public dynamic GetUpdatePart(string partID, string weight, string PartName, string PartCode, string BomID, string PartICode, string PartIName, string Model, string wCode, string wName, string qUnit)
        {

            using (var db = Db.Context("Mms"))
            {
                db.UseTransaction(true);

                string sqlC = string.Format(@"select * from SYS_Part where InventoryCode='{0}'", PartICode);
                int nC = db.Sql(sqlC).QueryMany<dynamic>().Count();
                if (nC == 0)
                {
                    partID = db.Insert("SYS_Part").Column("InventoryCode", PartICode).Column("InventoryName", PartIName).Column("Model", Model).ExecuteReturnLastId<int>().ToString();
                }

                string sqlA = string.Format(@"UPDATE dbo.SYS_Part SET Weight = '{0}',PartCode='{1}',PartName='{2}',WarehouseCode='{3}',WarehouseName='{4}',QuantityUnit='{5}' WHERE InventoryCode='{6}'", weight, PartCode, PartName, wCode ?? "", wName ?? "", qUnit ?? "", PartICode);
                string sqlB = string.Format(@"UPDATE dbo.PRS_Process_BOM SET InventoryCode = '{0}',InventoryName = '{1}' WHERE PartCode ='{2}' ", PartICode, PartIName, PartCode);

                int nA = db.Sql(sqlA).Execute();
                int nB = db.Sql(sqlB).Execute();

                bool result = (nA > 0) && (nB > 0);

                if (result)
                {
                    db.Commit();
                }
                else
                {
                    db.Rollback();
                }

                return new
                {
                    result = result
                };
            }
        }

        /// <summary>
        /// 自动绑定BOM&Part
        /// </summary>
        /// <param name="PartFCode"></param>
        /// <returns></returns>
        public dynamic GetSelfMotionUpdatePart(string PartFCode)
        {
            using (var db = Db.Context("Mms"))
            {
                //此SQL差两查询条件 InventoryCode、InventoryName 判断为空、由于 fn_GETBOM 该方法未查出该字段，待修改
                //string sqlA = string.Format(@"SELECT * FROM fn_GETBOM(N'{0}') WHERE IsSelfMade = 1 ORDER BY px", PartFCode);

                //已拼接SQL修改
                /*                string sqlA = string.Format(@"
                SELECT tbA.*,tbB.[Weight],tbB.Totalweight,tbB.InventoryCode,tbB.InventoryName,tbB.VersionCode,tbB.Remark 
                FROM (SELECT * FROM fn_GETBOM(N'{0}') WHERE IsSelfMade = 1) tbA LEFT JOIN dbo.PRS_Process_BOM tbB
                ON tbB.ID = tbA.ID WHERE (tbB.InventoryCode IS NULL OR tbB.InventoryCode = '') ORDER BY tbA.px
                ", PartFCode);*/

                string sqlA = string.Format(@"
with cte as
(
select * from PRS_Process_BOM where isnull(PartFigureCode,'')='{0}'
union all
select temp.* from cte inner join PRS_Process_BOM temp on cte.PartCode=temp.ParentCode
)
select * from cte union select * from PRS_Process_BOM where PartFigureCode='{0}'
", PartFCode);

                List<dynamic> GetData = db.Sql(sqlA).QueryMany<dynamic>().Where(item =>
                {
                    bool a = string.IsNullOrEmpty(item.InventoryCode);
                    bool b = Convert.ToInt32(item.IsSelfMade ?? 0) == 1;
                    return a && b;
                }).ToList();

                if (GetData.Count <= 0)
                {
                    return new
                    {
                        result = false
                    };
                }

                string sqlB = string.Format(@"SELECT * FROM dbo.PRS_Process_BOM WHERE IsEnable = 1");
                List<PRS_Process_BOM> GetBOMs = db.Sql(sqlB).QueryMany<PRS_Process_BOM>();

                List<dynamic> partCodes = GetData.Select(item =>
                {
                    return "'" + item.PartCode + "'";
                }).ToList();
                string whereC = string.Format(" AND PartCode in ({0})", string.Join(",", partCodes));

                string sqlC = string.Format(@"SELECT * FROM dbo.SYS_Part WHERE IsEnable = 1 {0}", whereC);
                List<SYS_Part> GetParts = db.Sql(sqlC).QueryMany<SYS_Part>();

                long maxICode = db.Sql("SELECT SUBSTRING(ISNULL(MAX(InventoryCode),'030501000000000000'),7,12) FROM dbo.SYS_Part WHERE IsEnable = 1 AND InventoryCode LIKE '030501%'").QuerySingle<long>();

                DateTime newDT = DateTime.Now;

                foreach (var item in GetData)
                {
                    var bom = GetBOMs.SingleOrDefault(s => s.ID.Equals(item.ID));

                    var parts = GetParts.Where(s =>
                    {
                        bool a = s.PartCode == null ? false : true;
                        bool b = s.PartCode.Equals(item.PartCode);
                        return a && b;
                    }).ToList();

                    //无则新增
                    if (parts.Count <= 0)
                    {
                        SYS_Part partModel = new SYS_Part()
                        {
                            InventoryCode = "030501" + (++maxICode).ToString().PadLeft(12, '0'),
                            InventoryName = bom.PartName,

                            Weight = bom.Weight,
                            PartCode = bom.PartCode,
                            PartName = bom.PartName,
                            Model = bom.PartFigureCode,
                            IsSelfMade = true,
                            IsEnable = 1,

                            CreatePerson = MmsHelper.GetUserName(),
                            CreateTime = newDT,
                            ModifyPerson = MmsHelper.GetUserName(),
                            ModifyTime = newDT
                        };

                        string sqlD = string.Format(@"UPDATE dbo.PRS_Process_BOM SET InventoryCode = '{0}',InventoryName = '{1}' WHERE ID ={2} ", partModel.InventoryCode, partModel.InventoryName, bom.ID);

                        db.UseTransaction(true);

                        int tempA = db.Insert("SYS_Part", partModel).AutoMap(x => x.ID).Execute();
                        int tempB = db.Sql(sqlD).Execute();

                        if ((tempA > 0) && (tempB > 0))
                        {
                            db.Commit();
                        }
                        else
                        {
                            db.Rollback();
                        }

                    }
                    //有则绑定
                    else
                    {
                        var result = GetUpdatePart(parts[0].ID.ToString(), bom.Weight, bom.PartName, bom.PartCode, bom.ID.ToString(), parts[0].InventoryCode, parts[0].InventoryName, "", null, null, null);
                    }

                }

                return new
                {
                    result = true
                };
            }
        }

        public dynamic GetSelfMotionUpdatePart2(string pIDs)
        {
            try
            {
                List<int> pIDList = pIDs.Split(',').Select(item => { return Convert.ToInt32(item); }).ToList();



                throw new Exception("错误啊:" + string.Join("&", pIDList));
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


        public long GetSysPartMaxICode(string key)
        {
            try
            {
                if (key.Length.Equals(6))
                {
                    string sql = string.Format(@"
SELECT SUBSTRING(ISNULL(MAX(InventoryCode), '{0}000000000000'), 7, 12)
FROM dbo.SYS_Part
WHERE IsEnable = 1
      AND InventoryCode LIKE '{0}%'
", key);
                    long maxICode = db.Sql(sql).QuerySingle<long>();
                    return maxICode;
                }
                else
                {
                    return -1;
                }
            }
            catch
            {
                return -1;
            }
        }

        public string GetSysPartAutoMaxICode(string key)
        {
            try
            {
                if (key.Length.Equals(6))
                {
                    string sql = string.Format(@"
SELECT SUBSTRING(ISNULL(MAX(InventoryCode), '{0}000000000000'), 7, 12)
FROM dbo.SYS_Part
WHERE IsEnable = 1
      AND InventoryCode LIKE '{0}%'
", key);
                    long maxICode = db.Sql(sql).QuerySingle<long>();
                    return string.Format(@"{0}{1}", key, (maxICode + 1).ToString().PadLeft(12, '0'));
                }
                else
                {
                    return string.Empty;
                }
            }
            catch
            {
                return string.Empty;
            }
        }


        public class PartTemp
        {
            /// <summary>
            /// 零件ID
            /// </summary>
            public int PartID { get; set; }

            /// <summary>
            /// 零件编码
            /// </summary>
            public string PartCode { get; set; }

            /// <summary>
            /// hash值
            /// </summary>
            public string SimHash { get; set; }

            /// <summary>
            /// 相似度
            /// </summary>
            public int Similarity { get; set; }
        }

    }

    public class SYS_Part : ModelBase
    {
        [Identity]
        [PrimaryKey]
        #region
        /*
        public int ID { get; set; }
        public string InventoryCode { get; set; }
        public string InventoryName { get; set; }
        public string PartCode { get; set; }
        public string PartName { get; set; }
        public int? MinStock { get; set; }
        public int? MaxStock { get; set; }
        public string PartTypeID { get; set; }
        public string Spec { get; set; }
        public string Model { get; set; }
        public string Weight { get; set; }
        public int? SafetyStock { get; set; }
        public int? PurchaseAdvanceTime { get; set; }
        public int? EconomicBatchQuantity { get; set; }
        public int? MinPackQuantity { get; set; }
        public int? EnconanmicOrderQuantity { get; set; }
        public string QuantityUnit { get; set; }
        public string FigureCode { get; set; }
        public string QualityCode { get; set; }
        public string CorrespondingBarcode { get; set; }
        public int? IsSelfMade { get; set; }
        public int? IsSupplyMade { get; set; }
        public int? IsCastforgeMatch { get; set; }
        public int? IsOutsouceWeiding { get; set; }
        public int? IsElectroHydraulicMatch { get; set; }
        public int? IsMarketMatch { get; set; }
        public string WarehouseCode { get; set; }
        public string WarehouseName { get; set; }
        public int? IsEnable { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }

        public string SimHash { get; set; }
        */
        #endregion
        /// <summary>
		/// 零件信息ID
		/// </summary>
		public int ID { get; set; }

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
        public int? MaxStock { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? MinStock { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Weight { get; set; }

        /// <summary>
        /// 零件编码
        /// </summary>
        public string PartCode { get; set; }

        /// <summary>
        /// 零件名称
        /// </summary>
        public string PartName { get; set; }

        /// <summary>
        /// 零件类型ID
        /// </summary>
        public string PartTypeID { get; set; }

        /// <summary>
        /// 规格
        /// </summary>
        public string Spec { get; set; }

        /// <summary>
        /// 型号
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// 安全库存
        /// </summary>
        public int? SafetyStock { get; set; }

        /// <summary>
        /// 请购提前期
        /// </summary>
        public int? PurchaseAdvanceTime { get; set; }

        /// <summary>
        /// 经济批量
        /// </summary>
        public int? EconomicBatchQuantity { get; set; }

        /// <summary>
        /// 最小包装量
        /// </summary>
        public int? MinPackQuantity { get; set; }

        /// <summary>
        /// 紧急采购批量
        /// </summary>
        public int? EnconanmicOrderQuantity { get; set; }

        /// <summary>
        /// 数量单位
        /// </summary>
        public string QuantityUnit { get; set; }

        /// <summary>
        /// 图号
        /// </summary>
        public string FigureCode { get; set; }

        /// <summary>
        /// 质量批号
        /// </summary>
        public string QualityCode { get; set; }

        /// <summary>
        /// 对应条码
        /// </summary>
        public string CorrespondingBarcode { get; set; }

        /// <summary>
        /// 是否自制件
        /// </summary>
        public bool? IsSelfMade { get; set; }

        /// <summary>
        /// 是否采购供应
        /// </summary>
        public bool? IsSupplyMade { get; set; }

        /// <summary>
        /// 是否铸锻配套
        /// </summary>
        public bool? IsCastforgeMatch { get; set; }

        /// <summary>
        /// 是否外协焊接
        /// </summary>
        public bool? IsOutsouceWeiding { get; set; }

        /// <summary>
        /// 是否电液配套
        /// </summary>
        public bool? IsElectroHydraulicMatch { get; set; }

        /// <summary>
        /// 是否门市配套
        /// </summary>
        public bool? IsMarketMatch { get; set; }

        /// <summary>
        /// 仓库ID
        /// </summary>
        public string WarehouseCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string WarehouseName { get; set; }

        /// <summary>
        /// 是否可用
        /// </summary>
        public int? IsEnable { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatePerson { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        public string ModifyPerson { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? ModifyTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SimHash { get; set; }
    }
}
