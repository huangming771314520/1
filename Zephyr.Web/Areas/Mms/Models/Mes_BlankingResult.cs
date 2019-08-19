using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;
using System.Linq;
using Zephyr.Areas.Areas.Mms.Common;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class Mes_BlankingResultService : ServiceBase<Mes_BlankingResult>
    {
        public void PostCreateBoard(out bool result, dynamic data)
        {
            try
            {
                string SavantCode = data.SavantCode;
                var Item = new PRS_BlankingResultService().GetModelList(ParamQuery.Instance().AndWhere("SavantCode", SavantCode));
                var ItemDetail = new PRS_BlankingPlateDetailService().GetModelList(ParamQuery.Instance().AndWhere("MainID", string.Join(",", Item.Select(a => a.ID)), Cp.In));
                var list = from master in Item
                           join detail in ItemDetail on master.ID equals detail.MainID
                           select new
                           {
                               MasterID = master.ID,
                               PartBlankingQuntity = master.PartBlankingQuntity,
                               DetailID = detail.ID,
                               PlateSize = detail.PlateSize
                           };
                new Mes_BlankingResultService()
                    .Delete(ParamDelete
                        .Instance()
                        .AndWhere("BlankingResultID", string.Join(",", Item.Select(a => a.ID)), Cp.In));

                var BoardCode = MmsHelper.GetOrderNumber("Mes_BlankingResult", "BoardCode", "XJB", "", "");
                string PreCode = BoardCode.Substring(0, BoardCode.Length - 3);
                int StartNumber = Convert.ToInt32(BoardCode.Substring(BoardCode.Length - 3));

                foreach (var item in list)
                {
                    int count = item.PartBlankingQuntity;
                    for (int i = 0; i < count; i++)
                    {
                        BoardCode = PreCode + StartNumber.ToString().PadLeft(3, '0');
                        StartNumber++;
                        db.UseTransaction(true);
                        db.Insert("Mes_BlankingResult")
                            .Column("BlankingResultID", item.MasterID)
                            .Column("IsEnable", 1)
                            .Column("CreateTime", DateTime.Now)
                            .Column("CreatePerson", MmsHelper.GetUserCode())
                            .Column("BiankingSize", item.PlateSize.ToString())
                            .Column("IsBlanking", 0)
                            .Column("BlankingDetailID", item.DetailID)
                            //.Column("BlankingType",1)
                            .Column("BoardCode", BoardCode).Execute();
                    }
                }
                db.Commit();
                result = true;
            }
            catch
            {
                db.Rollback();
                result = false;
            }
        }
    }

    public class Mes_BlankingResult : ModelBase
    {
        [Identity]
        [PrimaryKey]
        public int ID { get; set; }
        public int? BlankingResultID { get; set; }
        public int? IsEnable { get; set; }
        public DateTime? CreateTime { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? ModifyTime { get; set; }
        public string ModifyPerson { get; set; }
        public string BiankingSize { get; set; }
        public int? MainID { get; set; }
        public string BlankingCode { get; set; }
        public int? IsBlanking { get; set; }
        public string BoardCode { get; set; }
        public int? BlankingDetailID { get; set; }
    }
}
