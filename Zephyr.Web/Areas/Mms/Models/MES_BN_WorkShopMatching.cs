using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zephyr.Areas.Areas.Mms.Common;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class MES_BN_WorkShopMatchingService : ServiceBase<MES_BN_WorkShopMatching>
    {
        public bool PostCreateWorkShopMatchingData(List<PRS_Process_BOM> data, string ParentCode, string OperationType)
        {
            try
            {
                db.UseTransaction(true);
                if (OperationType == "add")
                {
                    data.ForEach(item =>
                    {
                        MES_BN_WorkShopMatching model = new MES_BN_WorkShopMatching();
                        model.PartCode = item.PartCode;
                        model.RootPartCode = ParentCode;
                        model.ProcessType = "1";
                        model.WorkShopCode = "";
                        model.CreateTime = DateTime.Now;
                        model.CreatePerson = MmsHelper.GetUserCode();
                        model.ParentCode = item.ParentCode;
                        model.ContractCode = item.ContractCode;
                        model.IsEnable = 1;
                        int result = db.Insert<MES_BN_WorkShopMatching>("MES_BN_WorkShopMatching", model).AutoMap(a => a.ID).ExecuteReturnLastId<int>();
                    });
                }
                else if (OperationType == "update")
                {
                    var UpdateList = new List<int>();
                    new MES_BN_WorkShopMatchingService().Update(ParamUpdate.Instance()
                        .Update("MES_BN_WorkShopMatching")
                        .Column("IsEnable", 0)
                        .AndWhere("RootPartCode", ParentCode));

                    var MES_BN_WorkShopMatchingList = new MES_BN_WorkShopMatchingService().GetModelList(ParamQuery.Instance().AndWhere("RootPartCode", ParentCode));

                    data.ForEach(item =>
                    {
                        var PCode = item.ParentCode;//父级编码
                        var PartCode = item.PartCode;//子级编码
                        var IsExist = MES_BN_WorkShopMatchingList.Where(a => a.ParentCode == PCode && a.PartCode == PartCode);
                        if (IsExist.Count() > 0)
                        {
                            int ID = IsExist.FirstOrDefault().ID;
                            UpdateList.Add(ID);
                        }
                        else
                        {
                            MES_BN_WorkShopMatching model = new MES_BN_WorkShopMatching();
                            model.PartCode = item.PartCode;
                            model.RootPartCode = ParentCode;
                            model.ProcessType = "1";
                            model.WorkShopCode = "";
                            model.CreateTime = DateTime.Now;
                            model.CreatePerson = MmsHelper.GetUserCode();
                            model.ParentCode = item.ParentCode;
                            model.ContractCode = item.ContractCode;
                            model.IsEnable = 1;
                            int result = db.Insert<MES_BN_WorkShopMatching>("MES_BN_WorkShopMatching", model)
                            .AutoMap(a => a.ID).ExecuteReturnLastId<int>();
                        }
                    });

                    if (UpdateList.Count > 0)
                    {
                        string UpdateStr = string.Join(",", UpdateList);
                        db.Sql($"update MES_BN_WorkShopMatching set IsEnable=1 where ID in({UpdateStr})").Execute();
                    }
                }
                db.Commit();
                return true;
            }
            catch (Exception ex)
            {
                db.Rollback();
                return false;
            }
        }

        public bool PostCreateBlankingBomData(List<PRS_Process_BOM> PRS_Process_BOMList, List<PRS_Process_BOM_Blanking> PRS_Process_BOM_BlankingList, string ParentCode, string ContractCode, int ProductID, string OperationType)
        {
            try
            {
                db.UseTransaction(true);
                var PRS_Process_BOM_BlankingIDStr = string.Join(",", PRS_Process_BOM_BlankingList.Select(a => a.ID));
                db.Sql($"update PRS_Process_BOM_Blanking set IsEnable=0 where ID in({PRS_Process_BOM_BlankingIDStr})");
                var UpdateList = new List<int>();

                StringBuilder sb = new StringBuilder();

                PRS_Process_BOMList.ForEach(item =>
                {
                    item.ProductID = ProductID;
                    item.ContractCode = ContractCode;
                    var IsExist = PRS_Process_BOM_BlankingList.Where(a => a.ParentCode == item.ParentCode && a.PartCode == item.PartCode);
                    if (IsExist.Count() > 0)
                    {
                        //UpdateList.Add(IsExist.FirstOrDefault().ID);
                        var ID = IsExist.FirstOrDefault().ID;
                        var IsSelfMade = item.IsSelfMade;
                        db.Update("PRS_Process_BOM_Blanking")
                        .Column("IsEnable", "1")
                        .Column("IsSelfMade", IsSelfMade)
                        .Where("ID", ID).Execute();
                    }
                    else
                    {
                        //db.Insert("PRS_Process_BOM_Blanking", item)
                        //.AutoMap(a => a.ID)
                        //.ExecuteReturnLastId<int>();

                        sb.Append(WinFormClientService.GetInsertSQL(new PRS_Process_BOM_Blanking()
                        {
                            ApproveDate = item.ApproveDate,
                            ApprovePerson = item.ApprovePerson,
                            ApproveRemark = item.ApproveRemark,
                            ApproveState = item.ApproveState,
                            Assembling = item.Assembling,
                            Blanking = item.Blanking,
                            BlankingNum = item.BlankingNum,
                            BlankingSize = item.BlankingSize,
                            BlankingTotal = item.BlankingTotal,
                            BlankingType = item.BlankingType,
                            ContractCode = item.ContractCode,
                            CreatePerson = item.CreatePerson,
                            CreateTime = item.CreateTime,
                            InPlanceSize = item.InPlanceSize,
                            InventoryCode = item.InventoryCode,
                            IsCrux = item.IsCrux,
                            InventoryName = item.InventoryName,
                            IsEnable = item.IsEnable,
                            IsSelfMade = item.IsSelfMade,
                            Machining = item.Machining,
                            MateParamValue = item.MateParamValue,
                            MaterialCode = item.MaterialCode,
                            MaterialInventoryName = item.MaterialInventoryName,
                            MaterialInventoryCode = item.MaterialInventoryCode,
                            MaterialQuantity = item.MaterialQuantity,
                            MateType = item.MateType,
                            ModifyTime = item.ModifyTime,
                            ModifyPerson = item.ModifyPerson,
                            New_PartName = item.New_PartName,
                            New_Specs = item.New_Specs,
                            New_Model = item.New_Model,
                            New_InventoryCode = item.New_InventoryCode,
                            PartName = item.PartName,
                            PurchaseNum = item.PurchaseNum,
                            RestructNum = item.RestructNum,
                            SetMateName = item.SetMateName,
                            ParentCode = item.ParentCode,
                            SetMateNum = item.SetMateNum,
                            PartCode = item.PartCode,
                            PartFigureCode = item.PartFigureCode,
                            PartQuantity = item.PartQuantityAll,//**********//
                            PartQuantityAll = item.PartQuantityAll,
                            ProductID = item.ProductID,
                            Sort = item.Sort,
                            Totalweight = item.Totalweight,
                            Weight = item.Weight,
                            Welding = item.Welding
                        }));

                    }
                });

                var insertSQL = sb.ToString();
                if (!string.IsNullOrEmpty(insertSQL))
                {
                    db.Sql(insertSQL).Execute();
                }
                //if (UpdateList.Count > 0)
                //{
                //    db.Sql($"update PRS_Process_BOM_Blanking set IsEnable=1 where ID in({string.Join(",", UpdateList)})");
                //}
                db.Commit();
                return true;
            }
            catch (Exception ex)
            {
                db.Rollback();
                return false;
            }
        }
    }

    public class MES_BN_WorkShopMatching : ModelBase
    {
        [Identity]
        [PrimaryKey]
        public int ID { get; set; }
        public string PartCode { get; set; }
        public string ProcessType { get; set; }
        public string WorkShopCode { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }
        public string RootPartCode { get; set; }

        public string ParentCode { get; set; }
        public string ContractCode { get; set; }
        public int? IsEnable { get; set; }
    }
}
