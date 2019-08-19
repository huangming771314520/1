using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;
using System.Linq;
using System.Data;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class SYS_BOMService : ServiceBase<SYS_BOM>
    {
        protected override void OnAfterHandleExcel(ref DataTable dtSheet)
        {
            var service = new SYS_BOMService();
            foreach (DataRow item in dtSheet.Rows)
            {
                string dName = item["PartCode"].ToString();
                //if ((dName == "GB/T 27") || (dName == "GB/T 5783") || (dName == "GB/T 93") ||( dName == "GB/T 119"))
                //{
                //    item["ParentCode"] = "QS160R-585-F 30";

                //    //var row = dtSheet.Select("PartTypeCode='" + item["PPartTypeCode"] + "'");
                //    //if (row.Length > 0)
                //    //{
                //    //    item["PTypeName"] = row[0]["TypeName"];
                //    //}

                //}

                string isEnable = item["IsEnable"].ToString();
                if (isEnable == "否")
                {
                    item["IsEnable"] = 0;
                }
                else
                    item["IsEnable"] = 1;

                var PartCode = item["PartCode"].ToString();
                var Query = ParamDelete.Instance().AndWhere("PartCode", PartCode);
                //service.Delete(Query);
                var ParentCode = item["ParentCode"].ToString();
                var Query1 = ParamDelete.Instance().AndWhere("PartCode", PartCode);
                //service.Delete(Query);
                var VersionCode = item["VersionCode"].ToString();
                var Query2 = ParamDelete.Instance().AndWhere("PartCode", PartCode);
                service.Delete(Query);
            }
            base.OnAfterHandleExcel(ref dtSheet);
            //GetNoInventory();
        }

        //public string GetNoInventory()
        //{
        //    var noInventoryCount = ParamQuery.Instance().AndWhere("InventoryCode", null);
        //    var service = new SYS_BOMService();
        //    dynamic res = service.GetDynamicList().Count;
        //    string msg = "有" + res + "条数据需要维护存货编码";
        //    return msg;
        //}
        public List<dynamic> GetBom(string parentCode)
        {
            //            string sql = String.Format(@"WITH T AS  
            //(  
            //    SELECT A.ID,A.PartCode,A.PartName,A.ParentCode,A.PartQuantity,A.PartFigureCode,A.IsSelfMade,A.IsEnable,(SELECT P.ID FROM dbo.PRS_Process_BOM P WHERE P.PartCode=A.ParentCode) PID,CAST(id AS VARBINARY(MAX)) AS px   
            //    FROM dbo.PRS_Process_BOM AS A 
            //    WHERE NOT EXISTS(SELECT B.ID,A.PartCode,B.PartName,B.ParentCode,B.PartQuantity,B.PartFigureCode,B.IsEnable,(SELECT P.ID FROM dbo.PRS_Process_BOM P WHERE P.PartCode=B.ParentCode) PID 
            //    FROM dbo.PRS_Process_BOM B 
            //    WHERE B.id=(SELECT P.ID FROM dbo.PRS_Process_BOM P WHERE P.PartCode=A.ParentCode))  
            //    AND A.PartCode='{0}'
            //    UNION ALL   
            //    SELECT C.ID,C.PartCode,C.PartName,C.ParentCode,C.PartQuantity,C.PartFigureCode,C.IsSelfMade,C.IsEnable,(SELECT P.ID FROM dbo.PRS_Process_BOM P WHERE P.PartCode=C.ParentCode) PID,CAST(D.px+CAST(C.ID AS VARBINARY) AS VARBINARY(MAX))    
            //    FROM dbo.PRS_Process_BOM AS C 
            //        JOIN T AS D 
            //           ON (SELECT P.ID FROM dbo.PRS_Process_BOM P WHERE P.PartCode=C.ParentCode)=D.ID  
            //)  
            //SELECT T.*,R.TotalStock,SP.InventoryName ParentName FROM T
            //LEFT JOIN (select distinct PartCode,max(InventoryCode) InventoryCode from SYS_Part group by PartCode) P on T.PartCode=P.PartCode
            //LEFT JOIN  SYS_Part SP on T.ParentCode=SP.PartCode
            //LEFT JOIN (select InventoryCode,sum(TotalStock) TotalStock from WMS_BN_RealStock group by InventoryCode) R ON  P.InventoryCode=R.InventoryCode
            //ORDER BY px    ", parentCode);
            string sql = String.Format(@"WITH T
AS (
SELECT C.*          
    FROM dbo.PRS_Process_BOM AS C
      WHERE C.PartCode='{0}'
	  UNION ALL
SELECT c.*
    FROM dbo.PRS_Process_BOM AS C
      inner  JOIN T ON c.ParentCode=t.PartCode     
	)
SELECT T.*,R.TotalStock,SP.InventoryName ParentName FROM T
LEFT JOIN (select distinct PartCode,max(InventoryCode) InventoryCode from SYS_Part group by PartCode) P on T.PartCode=P.PartCode
LEFT JOIN  SYS_Part SP on T.ParentCode=SP.PartCode
LEFT JOIN (select InventoryCode,sum(TotalStock) TotalStock from WMS_BN_RealStock group by InventoryCode) R ON  P.InventoryCode=R.InventoryCode
    ", parentCode);

            return db.Sql(sql).QueryMany<dynamic>();
        }

        public List<dynamic> GetBom2(string pCode,string cCode,int pID)
        {
            string sql = String.Format(@"
WITH T
AS (
SELECT C.*          
    FROM dbo.Get_ProcessBomBlanking('{0}','{1}',{2}) AS C
      WHERE C.PartCode='{0}'
	  UNION ALL
SELECT c.*
    FROM dbo.Get_ProcessBomBlanking('{0}','{1}',{2}) AS C
      inner  JOIN T ON c.ParentCode=t.PartCode     
	)
SELECT T.*,R.TotalStock,SP.InventoryName ParentName FROM T
LEFT JOIN (select distinct PartCode,max(InventoryCode) InventoryCode from SYS_Part group by PartCode) P on T.PartCode=P.PartCode
LEFT JOIN  SYS_Part SP on T.ParentCode=SP.PartCode
LEFT JOIN (select InventoryCode,sum(TotalStock) TotalStock from WMS_BN_RealStock group by InventoryCode) R ON  P.InventoryCode=R.InventoryCode
    ", pCode, cCode, pID);

            return db.Sql(sql).QueryMany<dynamic>();
        }

        public List<dynamic> GetBoomTree(string PartCode, string ContractCode, string ProductName)
        {
            var list = new PMS_BN_PartFileService().GetBom(PartCode);
            var notShowList = new MES_BN_MatchingTableDetailService().GetNotShowBom(PartCode, ContractCode, ProductName, "1");
            foreach (var item in notShowList)
            {
                var part = (from p in list where p.PartCode == item select p).FirstOrDefault();
                if (part != null)
                {
                    list.Remove(part);
                }
            }
            List<dynamic> list2 = new List<dynamic>();
            foreach (var item in list)
            {
                list2.Add(new { id = item.PartCode, pid = item.ParentCode, text = item.PartCode + " " + item.PartName });

            }
            return list2;
        }
        public List<dynamic> GetBoomTree2(string PartCode)
        {
            var list = new PMS_BN_PartFileService().GetBom(PartCode);
            List<dynamic> list2 = new List<dynamic>();
            foreach (var item in list)
            {
                list2.Add(new { id = item.PartCode, pid = item.ParentCode, text = item.PartName });

            }
            return list2;
        }
        public void updatePart(string partCode, string pareName, string id, string weight)
        {
            string sql = string.Format("update SYS_Part set PartCode='{0}' ,PartName='{1}',Weight = '{3}' where ID='{2}'", partCode, pareName, id, weight);
            var res = db.Sql(sql).Execute();
        }
        public List<dynamic> getPart(string pareName)
        {
            string sql = "select * from dbo.SYS_Part where InventoryName like '%" + pareName + "%'";
            var res = db.Sql(sql).QueryMany<dynamic>();
            return res;
        }

        public List<dynamic> GetMaterialDetailByPartCode(string partCode)
        {
            string sql = string.Format(@"
WITH subqry (PartCode, PartName, ParentCode, PartQuantity)
AS (SELECT PartCode,
           PartName,
           ParentCode,
           PartQuantity
    FROM dbo.SYS_BOM
    WHERE PartCode = '{0}'
    UNION ALL
    SELECT t.PartCode,
           t.PartName,
           t.ParentCode,
           t.PartQuantity
    FROM dbo.SYS_BOM t,
         subqry s
    WHERE t.ParentCode = s.PartCode)
SELECT *
FROM subqry;
", partCode);
            return db.Sql(sql).QueryMany<dynamic>();
        }


        public dynamic GetBOMsByPCode(string partCode)
        {
            //            string sql = string.Format(@"
            //SELECT * 
            //FROM fn_GETBOM(N'{0}')  ORDER BY px

            //", partCode);

            /*            string sql = string.Format(@"
            SELECT tbA.*,tbB.[Weight],tbB.Totalweight,tbB.InventoryCode,tbB.InventoryName,tbB.VersionCode,tbB.Remark 
            FROM (SELECT * FROM fn_GETBOM(N'{0}') WHERE IsSelfMade = 1) tbA LEFT JOIN dbo.SYS_BOM tbB
            ON tbB.ID = tbA.ID ORDER BY tbA.px
            ", partCode);*/

            string sql = string.Format(@"
with cte as
(
select * from SYS_BOM where isnull(PartFigureCode,'')='{0}'
union all
select temp.* from cte inner join SYS_BOM temp on cte.PartCode=temp.ParentCode
)
select * from cte union select * from SYS_BOM where PartFigureCode='{0}'
", partCode);

            List<dynamic> result = db.Sql(sql).QueryMany<dynamic>();
            return result as dynamic;
        }

    }

    public class SYS_BOM : ModelBase
    {
        [Identity]
        [PrimaryKey]
        public int ID { get; set; }
        public string PartFigureCode { get; set; }
        public string PartCode { get; set; }
        public string PartName { get; set; }
        public string InventoryCode { get; set; }
        public string InventoryName { get; set; }
        public int? PartQuantity { get; set; }
        public string ParentCode { get; set; }
        public string Weight { get; set; }
        public string Totalweight { get; set; }
        public string MaterialCode { get; set; }
        public string MaterialInventoryCode { get; set; }
        public string MaterialInventoryName { get; set; }
        public int? MaterialQuantity { get; set; }
        public string VersionCode { get; set; }
        public int? IsEnable { get; set; }
        public string IsSelfMade { get; set; }
        public string PartType { get; set; }
        public string Remark { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }

        /// <summary>
		/// 排序
		/// </summary>
		public int? Sort { get; set; }
    }


    public class SYS_BOM_Expand : SYS_BOM
    {
        public int Level { get; set; }

        public int TempPartQuantityAll { get; set; }
    }

}
