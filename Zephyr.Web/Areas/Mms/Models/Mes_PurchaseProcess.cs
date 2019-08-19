using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class Mes_PurchaseProcessService : ServiceBase<Mes_PurchaseProcess>
    {
        public int AuditBillCode(string mainID, out string msg)
        {
            msg = string.Empty;
            var rowsAffected = 0;
            string sql = String.Format(@"  select * FROM Mes_PurchaseProcess  where ID='{0}' and IsEnable=1", mainID);
            //if sql.Contains
            Mes_PurchaseProcess PurchaseProcess = db.Sql(sql).QuerySingle<Mes_PurchaseProcess>();
            //领料日期
            if (!string.IsNullOrEmpty(PurchaseProcess.PickMaterialDate.ToString()))
            {
                PurchaseProcess.PickMaterialState = 1;
            }
            //实际交货日期 ActualFinishDate
            if (!string.IsNullOrEmpty(PurchaseProcess.ActualFinishDate.ToString()))
            {
                PurchaseProcess.ActualFinishState = 1;
            }
            //开票时间 InvoiceTime
            if (!string.IsNullOrEmpty(PurchaseProcess.InvoiceTime.ToString()))
            {
                PurchaseProcess.InvoiceState = 1;
            }
            if (!string.IsNullOrEmpty(PurchaseProcess.UnqualityQuantity))
            {
                PurchaseProcess.UnqualityQuantityState = 1;
            }
            //整改次数 RectificationNumber
            if (!string.IsNullOrEmpty(PurchaseProcess.RectificationNumber.ToString()))
            {
                PurchaseProcess.RectificationState = 1;
            }
            try
            {
                rowsAffected = db.Update<Mes_PurchaseProcess>("Mes_PurchaseProcess", PurchaseProcess)
     .AutoMap(x => x.ID)
     .Where(x => x.ID)
     .Execute();
                //rowsAffected = db.Update<Mes_PurchaseProcess>("Mes_PurchaseProcess", PurchaseProcess).Where("ID",mainID).Execute();
                msg = "确认成功";
                return rowsAffected;
            }
            catch(Exception ex)
            {
                msg = "确认失败，请检查数据";
                return 0;
            }
        }
    }

    public class Mes_PurchaseProcess : ModelBase
    {
        [Identity]
        [PrimaryKey]   
        public int ID { get; set; }
        public string ContractCode { get; set; }
        public int? ProductID { get; set; }
        public string UserCode { get; set; }
        public string SaleMan { get; set; }
        public string InventoryCode { get; set; }
        public string InventoryName { get; set; }
        public string Model { get; set; }
        public string MaterialCode { get; set; }
        public int? Quantity { get; set; }
        public int? ArrivalQuantity { get; set; }
        public string SupplierCode { get; set; }
        public string PurchaseCode { get; set; }

        public DateTime? PrchaseDate { get; set; }
        public DateTime? PickMaterialDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public DateTime? ActualFinishDate { get; set; }
        public string CurrentState { get; set; }
        public string UnqualityQuantity { get; set; }
        public int? RectificationNumber { get; set; }
        public string Remark { get; set; }
        public int? IsEnable { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }
        /// <summary>
        /// 计划日期
        /// </summary>
        public DateTime? PlanArrivelDate { get; set; }
        /// <summary>
        /// 签约日期
        /// </summary>
        public DateTime? SigningTime { get; set; }
        /// <summary>
        /// 开票金额
        /// </summary>
        public decimal? InvoiceAmount { get; set; }
        /// <summary>
        /// 开票日期
        /// </summary>
        public DateTime? InvoiceTime { get; set; }
        /// <summary>
        /// 领料日期状态
        /// </summary>
        public int? PickMaterialState { get; set; }
        /// <summary>
        /// 实到状态
        /// </summary>
        public int? ActualFinishState { get; set; }
        /// <summary>
        /// 开票金额，开票日期状态
        /// </summary>
        public int? InvoiceState { get; set; }
        /// <summary>
        /// 不合格数量状态
        /// </summary>
        public int? UnqualityQuantityState { get; set; }
        /// <summary>
        /// 整改次数状态
        /// </summary>
        public int? RectificationState { get; set; }
    }
}
