using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zephyr.Areas.Areas.Mms.Common;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class MES_MaterialPickMainService : ServiceBase<MES_MaterialPickMain>
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

        public dynamic InsertPickingBill(dynamic InsertPickingBill)
        {

            return "";


        }

        public int GetDelete2(string id)
        {
            string sql = "update MES_MaterialPickMain set IsEnable=0 where id=" + id;
            db.Sql(sql).Execute();
            return 1;
        }

        public dynamic GetMaterialDetailByPCode(string ppdID)
        {
            var data = new WinFormClientService().GetPickMaterialDetailByPCode(ppdID);
            //var data = new WinFormClientService().GetMaterialDetailByPCode(ppdID, partCode); 
            MES_MaterialPickMainService pickMainServices = new MES_MaterialPickMainService();
            if(!data.Result)
            {
                return new
                {
                    Result = false,
                    Msg = data.Msg
                };
            }
            var warehouseList = data.Data.listmain;
            var winform = new WinFormClientService();
            var list = data.Data.listDetail;
            try
            {
                using (db.UseTransaction(true))
                {
                    for (int i = 0; i < warehouseList.Count - 1; i++)
                    {
                        string deststring = warehouseList[i].WarehouseCode;

                        if (string.IsNullOrEmpty(deststring))
                        {
                            throw new Exception(@"未查到" + warehouseList[i].InventoryName + "的仓库信息！");
                        }
                        for (int j = i + 1; j < warehouseList.Count; j++)
                        {
                            if (deststring.CompareTo(warehouseList[j].WarehouseCode) == 0)
                            {
                                warehouseList.RemoveAt(j);
                                j--;
                                continue;
                            }
                        }
                    }
                    int maxID = pickMainServices.GetModelList().Count > 0 ?
                 pickMainServices.GetModelList().Max(a => a.ID) + 1 : 1;

                    for (int t = 0; t < warehouseList.Count; t++)
                    {
                        SYS_BN_User user = new SYS_BN_UserService().GetModel(ParamQuery.Instance().AndWhere("IsEnable", 1).AndWhere("UserCode", MmsHelper.GetUserCode()));
                        MES_MaterialPickMain pickMain = new MES_MaterialPickMain();
                        pickMain.BillCode = MmsHelper.GetOrderNumber("MES_MaterialPickMain", "BillCode", "CJLL", "", "");
                        pickMain.ID = maxID;
                        pickMain.PickingDate = DateTime.Now;
                        pickMain.ProjectName = warehouseList[0].ProjectName;
                        pickMain.ContractCode = warehouseList[0].ContractCode;
                        pickMain.BillState = 1;
                        pickMain.IsEnable = 1;
                        pickMain.DepartmentID = user.DepartmentCode;
                        pickMain.DepartmentName = user.DepartmentName;
                        pickMain.WarehouseCode = warehouseList[t].WarehouseCode;
                        pickMain.WarehouseName = warehouseList[t].WarehouseName;
                        pickMain.CreateTime = DateTime.Now;
                        pickMain.CreatePerson = MmsHelper.GetUserName();
                        pickMain.ModifyPerson = MmsHelper.GetUserName();
                        pickMain.ModifyTime = DateTime.Now;
                        pickMain.ProductID = warehouseList[t].ProductID;
                        pickMain.ProductName = warehouseList[t].ProductName;
                        //pickMain.ProductName = productName;
                        //int res = db.Insert<MES_MaterialPickMain>("MES_MaterialPickMain", pickMain).Execute();
                        int res = db.Sql(WinFormClientService.GetInsertSQL(pickMain)).Execute();
                        maxID++;
                        foreach (var item in list)
                        {
                            if (item.WarehouseCode.Equals(warehouseList[t].WarehouseCode))
                            {
                                MES_MaterialPickDetail pickDetail = new MES_MaterialPickDetail();
                                pickDetail.InventoryCode = item.InventoryCode;
                                pickDetail.InventoryName = item.InventoryName;
                                pickDetail.MainID = pickMain.ID;
                                pickDetail.Model = item.Model;
                                pickDetail.ContractCode = item.ContractCode;
                                pickDetail.ProjectName = item.ProjectName;
                                pickDetail.RequiredQuantity = item.InventoryQuantity;
                                pickDetail.Unit = item.Unit;
                                pickDetail.IsEnable = 1;
                                pickDetail.CreateTime = DateTime.Now;
                                pickDetail.CreatePerson = MmsHelper.GetUserName();
                                pickDetail.ModifyPerson = MmsHelper.GetUserName();
                                pickDetail.ModifyTime = DateTime.Now;
                                res = db.Insert("MES_MaterialPickDetail", pickDetail).AutoMap(a => a.ID).ExecuteReturnLastId<int>();
                            }


                        }
                    }
                    int aaa = 1;
                    db.Commit();
                    return new ResultModel
                    {
                        Result = true,
                        Msg = "生成领料单成功"
                    };
                }
            }
            catch (Exception ex)
            {
                db.Rollback();
                return new 
                {
                    Result = false,
                    Msg = ex.Message
                };
            }


           

        }
    }

    public class MES_MaterialPickMain : ModelBase
    {
        [PrimaryKey]   
        public int ID { get; set; }
        public string BillCode { get; set; }
        public string ContractCode { get; set; }
        public string ProjectName { get; set; }
        public string DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public string WarehouseCode { get; set; }
        public string WarehouseName { get; set; }
        public DateTime? PickingDate { get; set; }
        public int? BillState { get; set; }
        public int? IsEnable { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }

        /// <summary> 
        /// 产品表ID 
        /// </summary> 
        public int? ProductID { get; set; }

        /// <summary> 
        /// 产品名称 
        /// </summary> 
        public string ProductName { get; set; }

    }
}
