using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Areas.Areas.Mms.Common;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class PMS_ProductTaskService : ServiceBase<PMS_ProductTask>
    {
        public int Insert(List<PMS_ProductTask> modellist)
        {
            int isSuccess=0;
            using (db.UseTransaction(true) )
            {
                for (int i = 0; i < modellist.Count; i++)
                {
                    var rowsAffected = modellist[i].ID = db.Insert<PMS_ProductTask>("PMS_ProductTask", modellist[i])
       .AutoMap(x => x.ID)
       .ExecuteReturnLastId<int>();
                    if (rowsAffected <= 0)
                    {
                        isSuccess ++;
                        db.Rollback();
                    }
                }
                if(isSuccess<=0)
                {
                    db.Commit();
                }
            }
            return isSuccess;
        }
     //   public int Update(PMS_ProductTask model)
     //   {
     //       var rowsAffected = db.Update<PMS_ProductTask>("PMS_ProductTask", model)
     //.AutoMap(x => x.ID)
     //.Where(x => x.ID)
     //.Execute();
     //       return rowsAffected;
     //   }
        public int SaveData(List<PMS_ProductTask> model)
        {
            int result=0;
            if (model.Count > 0)
            {
                using (db.UseTransaction(true))
                {
                    foreach (var item in model)
                    {
                        if (item.ID <= 0)
                        {
                            //result = new PMS_ProductTaskService().Insert(item);
                            item.ProductTaskCode = MmsHelper.GetOrderNumber("PMS_ProductTask", "ProductTaskCode", "JYRW", "", "");
                            item.CreateTime = DateTime.Now;
                            item.CreatePerson = MmsHelper.GetUserCode();
                            item.ModifyTime = DateTime.Now;
                            item.ModifyPerson = MmsHelper.GetUserCode();
                            var rowsAffected = item.ID = db.Insert<PMS_ProductTask>("PMS_ProductTask", item)
     .AutoMap(x => x.ID)
     .ExecuteReturnLastId<int>();
                            if (rowsAffected <= 0)
                            {
                                result++;
                                db.Rollback();
                            }
                        }
                        else
                        {
                            string sql = String.Format(@"select * from PMS_ProductTask where ID='{0}' ", item.ID);
                            var single = db.Sql(sql).QuerySingle<PMS_ProductTask>();
                            item.CreatePerson = single.CreatePerson;
                            item.CreateTime = single.CreateTime;
                            item.ModifyPerson = MmsHelper.GetUserCode();
                            item.ModifyTime = DateTime.Now;
                            var rowsAffected = item.ID = db.Update<PMS_ProductTask>("PMS_ProductTask", item)
     .AutoMap(x => x.ID)
     .Where(x => x.ID)
     .Execute();
                            if (rowsAffected <= 0)
                            {
                                result++;
                                db.Rollback();
                            }
                        }
                    }
                    if (result <= 0)
                    {
                        db.Commit();
                    }
                   
                }

              
                  
            }
            return result;
        }
        

    }

    public class PMS_ProductTask : ModelBase
    {
        [Identity]
        [PrimaryKey]   
        public int ID { get; set; }
        public string ContractCode { get; set; }
        public string ProductID { get; set; }
        public string ProductTaskCode { get; set; }
        public int? TaskType { get; set; }
        public DateTime? TaskStartDate { get; set; }
        public DateTime? TaskFinishDate { get; set; }
        public int? TaskCycle { get; set; }
        public int? BillState { get; set; }
        public int? IsEnable { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }
    }
}
