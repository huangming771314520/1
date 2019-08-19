using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Areas.Areas.Mms.Common;
using Zephyr.Core;
using System.Linq;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class WMS_BN_TakeStockMainService : ServiceBase<WMS_BN_TakeStockMain>
    {
        public List<dynamic> GetData(string tockType)
        {
            if (tockType == "1")
            {
                var list = db.Sql(@"select t1.InventoryCode,MAX( t1.InventoryName) InventoryName,t2.WarehouseCode,MAX( t2.WarehouseName) WarehouseName,isnull( SUM( t2.RealStock),0) TakeStockNum from SYS_Part as t1
  inner join  WMS_BN_RealStock as t2 on t1.InventoryCode=t2.InventoryCode
  group by t1.InventoryCode,t2.WarehouseCode
  ").QueryMany<dynamic>();

                string InventoryCode = "";
                string InventoryName = "";
                foreach (var item in list)
                {
                    if (item.InventoryCode == InventoryCode)
                    {
                        item.InventoryCode = "";
                        item.InventoryName = "";
                    }
                    else
                    {
                        InventoryCode = item.InventoryCode;
                        InventoryName = item.InventoryName;
                    }
                }
                return list;
            }
            else
            {
                var list = db.Sql(@"select t2.WarehouseCode,MAX( t2.WarehouseName) WarehouseName,t1.InventoryCode,MAX( t1.MateName) MateName,isnull( SUM( t2.RealStock),0) TakeStockNum from WMS_BD_Material as t1
  inner join  WMS_BN_RealStock as t2 on t1.InventoryCode=t2.InventoryCode
  group by t1.InventoryCode,t2.WarehouseCode").QueryMany<dynamic>();

                string WarehouseCode = "";
                string WarehouseName = "";
                foreach (var item in list)
                {
                    if (item.WarehouseCode == WarehouseCode)
                    {
                        item.WarehouseCode = "";
                        item.WarehouseName = "";
                    }
                    else
                    {
                        WarehouseCode = item.WarehouseCode;
                        WarehouseName = item.WarehouseName;
                    }
                }
                return list;
            }
        }
        public WMS_BN_TakeStockMain GetModelById(int id)
        {

            var info = db.Sql(string.Format(@"SELECT  max(BillCode)BillCode FROM WMS_BN_TakeStockMain A WHERE A.ID={0} ", id)).QuerySingle<WMS_BN_TakeStockMain>();
            return info;
        }
        public string GetMaxBillCode(int year, int month)
        {
            var result = "PDDH" + DateTime.Now.ToString("yyyyMMdd");// +"0000".PadLeft(4, '0');

            var bilcode = db.Sql(string.Format(@"SELECT ISNULL(BillCode,'0001')BillCode FROM (SELECT  max(BillCode)BillCode FROM WMS_BN_TakeStockMain A WHERE A.TakeStockYear={0} AND A.TakeStockMonth={1})A", year, month)).QuerySingle<string>();
            return string.IsNullOrEmpty(bilcode) ? (result + "0001") : (result + (Convert.ToInt32(bilcode) + 1).ToString().PadLeft(4, '0'));
        }
        public int GetTakeStock(int year, int month)
        {
            return db.Sql(string.Format(@"select count(*) from WMS_BN_TakeStockMain where TakeStockYear='{0}' and TakeStockMonth='{1}'", year, month)).QuerySingle<int>();
        }
        //自动生成出入库单
        public int PostBuild(string billCode, out string msg)
        {
            msg = string.Empty;
            var rowsAffected = 0;
            string sql = String.Format(@"    select MAX(t1.BillState) as BillState,t2.WarehouseCode,max(t2.WarehouseName) as WarehouseName,t2.InventoryCode,max(t2.InventoryName) as InventoryName,sum(t2.DValue) as DValue from WMS_BN_TakeStockMain as t1
inner join WMS_BN_TakeStockDetail as t2 on t1.BillCode=t2.BillCode where t1.BillCode='{0}' 
group by t2.WarehouseCode,t2.Inventorycode", billCode);
            var list = db.Sql(sql).QueryMany<dynamic>();
            var res = list[1].DValue;
            if (list[0].BillState == 2)
            {
                msg = "已锁定的数据不能重复提交！";
                return 0;
            }
            db.UseTransaction(true);
            foreach (var item in list)
            {
                res = item.DValue;
                WMS_BN_BillMain master = new WMS_BN_BillMain();
                if (item.DValue > 0)
                {
                    master.BillType = 6;
                    master.WarehouseCode = item.WarehouseCode;
                    master.WarehouseName = item.WarehouseName;
                    master.Remark = "盘盈入库";
                }
                else if (item.DValue < 0)
                {
                    master.BillType = 7;
                    master.WarehouseCode = item.WarehouseCode;
                    master.WarehouseName = item.WarehouseName;
                    master.Remark = "盘亏出库";
                }
                else
                    continue;
                var sc = new sys_codeService().Getsys_codeByTypeAndID("BillType", Convert.ToInt16(master.BillType));
                master.BillCode = MmsHelper.GetOrderNumber("WMS_BN_BillMain", "BillCode", sc.Description, "", "");
                master.CreateTime = DateTime.Now;
                master.CreatePerson = MmsHelper.GetUserName();

                rowsAffected = db.Sql(@"insert into WMS_BN_BillMain (BillCode,BillType,WarehouseCode,WarehouseName,Remark,
CreatePerson,CreateTime) values(@0,@1,@2,@3,@4,@5,@6)", master.BillCode, master.BillType,
    master.WarehouseCode, master.WarehouseName, master.Remark, master.CreatePerson, master.CreateTime).Execute();
                if (rowsAffected < 0)
                {
                    db.Rollback();
                    return rowsAffected;
                }

                var detailList = (from p in list where p.WarehouseCode == item.WarehouseCode select p).ToList();
                foreach (var it in detailList)
                {
                    WMS_BN_BillDetail detail = new WMS_BN_BillDetail();
                    detail.PBillCode = billCode;
                    detail.BillCode = master.BillCode;
                    detail.InventoryCode = it.InventoryCode;
                    detail.InventoryName = it.InventoryName;

                    detail.MateNum = Math.Abs(item.DValue);
                    detail.ConfirmNum = Math.Abs(item.DValue);

                    rowsAffected = db.Insert<WMS_BN_BillDetail>("WMS_BN_BillDetail", detail)
         .AutoMap(x => x.ID)
         .Execute();
                    if (rowsAffected < 0)
                    {
                        db.Rollback();
                        return rowsAffected;
                    }
                }
            }
            sql = string.Format(@"update WMS_BN_TakeStockMain set BillState='2' where BillCode='{0}'", billCode);
            rowsAffected = db.Sql(sql).Execute();

            if (rowsAffected < 0)
            {
                db.Rollback();
                return rowsAffected;
            }
            msg = "操作成功！";
            db.Commit();

            return rowsAffected;
        }

        //自动生成盘点单
        public int PostBuildPD(int year, int month, int tackStockType, out string msg)
        {
            msg = string.Empty;
            var rowsAffected = 0;
            string sql = String.Format(@"select * from WMS_BN_TakeStockMain where TakeStockYear='{0}' and TakeStockMonth='{1}'", year, month);
            var isExit = db.Sql(sql).QuerySingle<int>();
            if (isExit > 0)
            {
                msg = "一个月只能生成一次盘点！";
                return 0;
            }
            var list = db.Sql(@"select t1.InventoryCode,MAX( t1.InventoryName) InventoryName,t2.WarehouseCode,MAX( t2.WarehouseName) WarehouseName,isnull( SUM( t2.RealStock),0) TakeStockNum from SYS_Part as t1
  inner join  WMS_BN_RealStock as t2 on t1.InventoryCode=t2.InventoryCode
  group by t1.InventoryCode,t2.WarehouseCode
  ").QueryMany<dynamic>();
            if (list.Count <= 0)
            {
                msg = "没有数据！";
                return 0;
            }

            db.UseTransaction(true);
            WMS_BN_TakeStockMain master = new WMS_BN_TakeStockMain();
            master.BillCode = MmsHelper.GetOrderNumber("WMS_BN_TakeStockMain", "BillCode", "PDDH", "", "");
            master.TakeStockYear = year;
            master.TakeStockMonth = month;
            master.BillState = 1;
            master.TakeStockType = tackStockType;
            master.CreateTime = DateTime.Now;
            master.CreatePerson = MmsHelper.GetUserName();

            rowsAffected = db.Sql(@"insert into WMS_BN_TakeStockMain (BillCode,TakeStockYear,TakeStockMonth,TakeStockType,BillState,CreateTime,CreatePerson) 
values(@0,@1,@2,@3,@4,@5,@6)", master.BillCode, master.TakeStockYear,
master.TakeStockMonth, master.TakeStockType, master.BillState, master.CreateTime, master.CreatePerson).Execute();

            if (rowsAffected < 0)
            {
                db.Rollback();
                return rowsAffected;
            }
            sql = String.Format(@"select * from WMS_BN_TakeStockMain where BillCode = '{0}'", master.BillCode);
            dynamic next = db.Sql(sql).QuerySingle<dynamic>();//
            foreach (var item in list)
            {
                WMS_BN_TakeStockDetail detail = new WMS_BN_TakeStockDetail();
                detail.BillCode = next.BillCode;
                detail.InventoryCode = item.InventoryCode;
                detail.InventoryName = item.InventoryName;
                detail.WarehouseCode = item.WarehouseCode;
                detail.WarehouseName = item.WarehouseName;
                detail.TakeStockNum = item.TakeStockNum;


                rowsAffected = db.Insert<WMS_BN_TakeStockDetail>("WMS_BN_TakeStockDetail", detail)
     .AutoMap(x => x.ID)
     .Execute();
                if (rowsAffected < 0)
                {
                    db.Rollback();
                    return rowsAffected;
                }
            }

            msg = "操作成功！";
            db.Commit();
            return rowsAffected;
        }
    }

    public class WMS_BN_TakeStockMain : ModelBase
    {
        
        [PrimaryKey]
        public int ID { get; set; }
        public string BillCode { get; set; }
        public int? TakeStockYear { get; set; }
        public int? TakeStockMonth { get; set; }
        public int? TakeStockType { get; set; }
        public int? BillState { get; set; }
        public string Remark { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }
        public int? IsEnable { get; set; }
    }
}
