using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Areas.Areas.Mms.Common;
using Zephyr.Core;
using System.Linq;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class WMS_BN_TransfersMainService : ServiceBase<WMS_BN_TransfersMain>
    {
        public string GetBillCodeByID(string ID)
        {
            var pQuery = ParamQuery.Instance()
                .Select("BillCode")
                .AndWhere("ID", ID);
            var list = base.GetModelList(pQuery);
            if (list.Count > 0)
            {
                return list[0].BillCode;
            }
            else
                return string.Empty;
        }
        public int PostBuild(string billCode, out string msg)
        {
            msg = string.Empty;
          int rowsAffected=0;
            db.UseTransaction(true);
            string sql = string.Format(@"select a.BillCode,a.Remark, a.InWarehouseCode,a.InWarehouseName,a.OutWarehouseCode,a.OutWarehouseName,
b.InventoryCode,b.InventoryName,b.ConfirmNum,b.Unit from WMS_BN_TransfersMain a
 left join WMS_BN_TransfersDetail b on a.BillCode=b.BillCode where a.IsEnable=1 and
b.IsEnable=1 and b.BillCode='{0}'",billCode);
            var list = db.Sql(sql).QueryMany<dynamic>();
           if(list.Count<=0)
           {
                msg = "没有数据！";
                return 0;
           }
           //foreach (var item in list)
           //{
                WMS_BN_BillMain master = new WMS_BN_BillMain();
                var sc = new sys_codeService().Getsys_codeByTypeAndID("BillType",7);
                master.BillCode = MmsHelper.GetOrderNumber("WMS_BN_BillMain", "BillCode", sc.Description, "", "");
                master.BillType = 7;
                master.WarehouseCode = list[0].OutWarehouseCode;
                master.WarehouseName = list[0].OutWarehouseName;
                master.Remark = list[0].Remark;
                master.CreateTime = DateTime.Now;
                master.CreatePerson = MmsHelper.GetUserName();
            master.ApproveState = "1";
            
                rowsAffected = db.Sql(@"insert into WMS_BN_BillMain (ID,BillCode,BillType,WarehouseCode,WarehouseName,Remark,ApproveState,
CreatePerson,CreateTime) values(((select MAX(id)+1 from WMS_BN_BillMain)),@0,@1,@2,@3,@4,@5,@6,@7)", master.BillCode, master.BillType,
    master.WarehouseCode, master.WarehouseName, master.Remark, master.ApproveState, master.CreatePerson, master.CreateTime).Execute();
                if (rowsAffected < 0)
                {
                    db.Rollback();
                    return rowsAffected;
                }
                //var detailList = (from p in list where p.OutWarehouseID == item.OutWarehouseID select p).ToList();
                //var detailList = (from p in list where p.OutWarehouseID == item.OutWarehouseID select p).ToList();
              
                foreach (var it in list)
                {
                    WMS_BN_BillDetail detail = new WMS_BN_BillDetail();
                    detail.PBillCode = billCode;
                    detail.BillCode = master.BillCode;
                    detail.InventoryCode = it.InventoryCode;
                    detail.InventoryName = it.InventoryName;

                    detail.MateNum = it.ConfirmNum;
                    detail.ConfirmNum = it.ConfirmNum;

                    rowsAffected = db.Insert<WMS_BN_BillDetail>("WMS_BN_BillDetail", detail)
         .AutoMap(x => x.ID)
         .Execute();
                    if (rowsAffected < 0)
                    {
                        db.Rollback();
                        return rowsAffected;
                    }
                }
               
                sc = new sys_codeService().Getsys_codeByTypeAndID("BillType", 6);
                master.BillCode = MmsHelper.GetOrderNumber("WMS_BN_BillMain", "BillCode", sc.Description, "", "");
                master.BillType = 6;
                master.WarehouseCode = list[0].InWarehouseCode;
                master.WarehouseName = list[0].InWarehouseName;
                master.Remark = list[0].Remark;
                master.CreateTime = DateTime.Now;
                master.CreatePerson = MmsHelper.GetUserName();
                master.ApproveState = "1";
                rowsAffected = db.Sql(@"insert into WMS_BN_BillMain (ID,BillCode,BillType,WarehouseCode,WarehouseName,Remark,ApproveState,
CreatePerson,CreateTime) values(((select MAX(id)+1 from WMS_BN_BillMain)),@0,@1,@2,@3,@4,@5,@6,@7)", master.BillCode, master.BillType,
    master.WarehouseCode, master.WarehouseName, master.Remark, master.ApproveState, master.CreatePerson, master.CreateTime).Execute();
                if (rowsAffected < 0)
                {
                    db.Rollback();
                    return rowsAffected;
                }
                //detailList = (from p in list where p.InWarehouseID == item.InWarehouseID select p).ToList();
                foreach (var it in list)
                {
                    WMS_BN_BillDetail detail = new WMS_BN_BillDetail();
                    detail.PBillCode = billCode;
                    detail.BillCode = master.BillCode;
                    detail.InventoryCode = it.InventoryCode;
                    detail.InventoryName = it.InventoryName;

                    detail.MateNum = it.ConfirmNum;
                    detail.ConfirmNum = it.ConfirmNum;

                    rowsAffected = db.Insert<WMS_BN_BillDetail>("WMS_BN_BillDetail", detail)
         .AutoMap(x => x.ID)
         .Execute();
                    if (rowsAffected < 0)
                    {
                        db.Rollback();
                        return rowsAffected;
                    }
                }
            //}
           sql = string.Format(@"update WMS_BN_TransfersMain set ApproveState='2',ApprovePerson='{0}',ApproveTime='{1}' where BillCode='{2}'", MmsHelper.GetUserName(), DateTime.Now, billCode);
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

    public class WMS_BN_TransfersMain : ModelBase
    {
        [Identity]
        [PrimaryKey]   
        public int ID { get; set; }
        public string BillCode { get; set; }
        public string ContractCode { get; set; }
        public string ProjectName { get; set; }
        public string OutWarehouseCode { get; set; }
        public string OutWarehouseName { get; set; }
        public string InWarehouseCode { get; set; }
        public string InWarehouseName { get; set; }
        public int? ApproveState { get; set; }
        public string ApprovePerson { get; set; }
        public DateTime? ApproveTime { get; set; }
        public string Remark { get; set; }
        public int? IsEnable { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }
    }
}
