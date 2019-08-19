using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;
using Zephyr.Core;
using Zephyr.Data;
using Zephyr.Models;
using Zephyr.Areas.Mms.Controllers;

namespace Zephyr.Areas.Areas.Mms.Common
{
    [Module("Mms")]
    public class MmsService : ServiceBase<ModelBase>
    {
        //审核表单
        public int Audit(string tableName, string Object_ID, string status, string comment)
        {
            db.UseTransaction(true);

            var userName = MmsHelper.GetUserName();
            var pUpdate = ParamUpdate.Instance()
                .Update(tableName)
                .Column("ApproveState", status)
                .Column("ApproveRemark", comment)
                .Column("ApprovePerson", userName)
                .Column("ApproveDate", DateTime.Now)
                .AndWhere("Object_ID", Object_ID);

            var rowsAffected = BuilderParse(pUpdate).Execute();

            if (rowsAffected <= 0)
            {
                db.Rollback();
                return rowsAffected;
            }

            switch (tableName) //财务过账
            {
                default:
                    break;
            }

            if (rowsAffected < 0)
            {
                db.Rollback();
                return rowsAffected;
            }

            db.Commit();
            return rowsAffected;
        }

        //审核出库
        public string AuditOutStorage(string Object_ID, string UnitCategoryTypeID)
        {

            var db = Db.Context("Mms");
            string outMsg = "";
            using (db)
            {
                var document = db.StoredProcedure("proc_OutMasterById")
                 .ParameterOut("outMsg", DataTypes.String, 1000)
                 .Parameter("Object_Id", Object_ID)
                 .Parameter("UnitCategoryTypeID", Convert.ToInt32(UnitCategoryTypeID));
                document.Execute();
                outMsg = document.ParameterValue<string>("outMsg");
            }
            return outMsg;
        }

        //审核入库
        public string AuditInStorage(string Object_ID)
        {

            var db = Db.Context("Mms");
            string documentNo = "";
            using (db)
            {
                var document = db.StoredProcedure("proc_InMasterById")
                 .ParameterOut("outMsg", DataTypes.String, 1000)
                 .Parameter("Object_Id", Object_ID);
                document.Execute();
                documentNo = document.ParameterValue<string>("outMsg");
            }
            return documentNo;
        }

        //新增审核
        public int Audit(string tableName, EditEventArgs data)
        {
            db.UseTransaction(true);

            var userName = MmsHelper.GetUserName();
            var pUpdate = ParamUpdate.Instance()
                .Update(tableName)
                .Column("ApproveState", data.form["status"].ToString())
                .Column("ApproveRemark", data.form["comment"].ToString())
                .Column("ApprovePerson", userName)
                .Column("ApproveDate", DateTime.Now)
                .AndWhere("[Object_ID]", data.form["Object_ID"].ToString());

            var rowsAffected = BuilderParse(pUpdate).Execute();

            if (rowsAffected <= 0)
            {
                db.Rollback();
                return rowsAffected;
            }

            switch (tableName) //财务过账
            {
                default:
                    break;
            }

            if (rowsAffected < 0)
            {
                db.Rollback();
                return rowsAffected;
            }

            db.Commit();
            return rowsAffected;
        }


        public static List<dynamic> GetReceiveSrcBillTypeList()
        {
            var result = new List<dynamic>();
            result.Add(new { value = "receive", text = "收料单" });
            result.Add(new { value = "return", text = "退货单" });
            result.Add(new { value = "direct", text = "直入直出单" });
            result.Add(new { value = "rentin", text = "租赁进场单" });

            return result;
        }

        public static List<dynamic> GetSendSrcBillTypeList()
        {
            var result = new List<dynamic>();
            result.Add(new { value = "send", text = "发料单" });
            result.Add(new { value = "direct", text = "直入直出单" });
            result.Add(new { value = "refund", text = "退库单" });
            result.Add(new { value = "rentin", text = "租赁进场单" });
            result.Add(new { value = "lossReport", text = "报损单" });
            result.Add(new { value = "transfer", text = "调拨单" });

            return result;
        }

        public static List<dynamic> GetAccountSrcBillTypeList()
        {
            var result = new List<dynamic>();
            result.Add(new { value = "receive", text = "收料单" });
            result.Add(new { value = "send", text = "发料单" });
            result.Add(new { value = "direct", text = "直入直出单" });
            result.Add(new { value = "refund", text = "退库单" });
            result.Add(new { value = "return", text = "退货单" });
            result.Add(new { value = "lossReport", text = "报损单" });
            result.Add(new { value = "transfer", text = "调拨单" });
            result.Add(new { value = "rentin", text = "租赁进场单" });

            return result;
        }


        public int WMS_OutAndIn(string BillCode, string typeID, out string msg)
        {
            msg = string.Empty;
            db.UseTransaction(true);
            var rowsAffected = 0;
            string sql = String.Format(@"    select d.PBillCode,m.ApproveState,m.BillCode,d.InventoryCode,d.InventoryName,d.Specs as Model,m.SupplierCode,m.SupplierName,
            d.MateNum,d.ConfirmNum,d.BatchCode,d.Unit,d.AccountabilityCode,d.PackageCode,m.WarehouseCode,m.WarehouseName,m.DepartmentName
            from WMS_BN_BillDetail as d
            inner join WMS_BN_BillMain as m on d.BillCode=m.BillCode
            where m.BillCode='{0}'", BillCode);
            
            var list = db.Sql(sql).QueryMany<dynamic>();
            if (list.Count <= 0)
            {
                msg = "没有明细数据！";
                return 0;
            }
            if (list[0].ApproveState == "2")
            {
                msg = "已提交的数据不能重复提交！";
                return 0;
            }
            if (typeID == "1")
            {
                if (string.IsNullOrEmpty(list[0].SupplierCode) || string.IsNullOrEmpty(list[0].WarehouseCode))
                {
                    msg = "供应商或收货仓库不能为空！";
                    return 0;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(list[0].WarehouseCode))
                {
                    msg = "库位不能为空！";
                    return 0;
                }
            }

            sql = String.Format(@"select * from WMS_BN_RealStock where (WarehouseCode='{0}')", list[0].WarehouseCode);
            List<WMS_BN_RealStock> realStockList = db.Sql(sql).QueryMany<WMS_BN_RealStock>();

            foreach (var item in list)
            {
                if (string.IsNullOrEmpty(item.InventoryCode))
                {
                    msg = "物料不存在！";
                    return 0;
                }
                if (typeID == "1" || typeID == "6" || typeID == "4" || typeID == "3" || typeID=="8"||typeID=="10")
                {
                    var realStock = (from p in realStockList where p.WarehouseCode == item.WarehouseCode && p.BatchCode == item.BatchCode && p.InventoryCode == item.InventoryCode select p).FirstOrDefault();

                    if (realStock == null)//库位没有该物料
                    {
                        WMS_BN_RealStock stock = new WMS_BN_RealStock();
                        stock.InventoryCode = item.InventoryCode;
                        stock.InventoryName = item.InventoryName;
                        stock.RealStock = item.ConfirmNum;
                        stock.TotalStock = item.ConfirmNum;
                        stock.WarehouseCode = item.WarehouseCode;
                        stock.WarehouseName = item.WarehouseName;
                        stock.BatchCode = item.BatchCode;
                        stock.Unit = item.Unit;
                        stock.Model = item.Model;
                        stock.CreateTime = DateTime.Now;
                        stock.CreatePerson = MmsHelper.GetUserName();
                        stock.IsEnable = 1;
                        rowsAffected = stock.ID = db.Insert<WMS_BN_RealStock>("WMS_BN_RealStock", stock)
     .AutoMap(x => x.ID)
     .ExecuteReturnLastId<int>();

                        if (rowsAffected <= 0)
                        {
                            db.Rollback();
                            return rowsAffected;
                        }

                    }
                    else
                    {
                        realStock.RealStock = realStock.RealStock + item.ConfirmNum;
                        realStock.TotalStock = realStock.TotalStock + item.ConfirmNum;
                        //realStock.ModifyTime = DateTime.Now;
                        //realStock.ModifyPerson = MmsHelper.GetUserName();
                        //product.Modify_Time = DateTime.Now;
                        rowsAffected = db.Update<WMS_BN_RealStock>("WMS_BN_RealStock", realStock)
     .AutoMap(x => x.ID)
     .Where(x => x.ID)
     .Execute();

                    }
                }
                if (typeID == "2" || typeID == "5"||typeID == "7")
                {
                    var realStock = (from p in realStockList where p.WarehouseCode == item.WarehouseCode && p.BatchCode == item.BatchCode && p.InventoryCode == item.InventoryCode select p).FirstOrDefault();
                    if (realStock == null) //库位没有该物料
                    {
                        msg = "物料不存在！,无法进行出库操作";
                        return 0;
                    }
                    else
                    {
                        realStock.RealStock = realStock.RealStock - item.ConfirmNum;
                        realStock.TotalStock = realStock.TotalStock - item.ConfirmNum;
                        //product.Modify_Time = DateTime.Now;
                        rowsAffected = db.Update<WMS_BN_RealStock>("WMS_BN_RealStock", realStock)
     .AutoMap(x => x.ID)
     .Where(x => x.ID)
     .Execute();
                        if (realStock.RealStock < 0 || realStock.TotalStock < 0)
                        {
                            msg = "物料出库数量超过库存，请输入正确数量！";
                            return 0;
                        }
                        else
                        {
                            if (rowsAffected < 0)
                            {
                                db.Rollback();
                                return rowsAffected;
                            }
                        }
                    }
                }
            }

            sql = string.Format(@"update WMS_BN_BillMain set ApproveState='{0}',ApprovePerson='{1}',ApproveDate='{2}' where BillCode='{3}' and ApproveState='1'", "2", MmsHelper.GetUserName(), DateTime.Now.ToString(), list[0].BillCode);
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

       
    }
}