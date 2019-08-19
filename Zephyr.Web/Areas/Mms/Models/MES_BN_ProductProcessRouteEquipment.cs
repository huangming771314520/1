using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class MES_BN_ProductProcessRouteEquipmentService : ServiceBase<MES_BN_ProductProcessRouteEquipment>
    {
        public void SaveEQP(string eqpids, string gyid)
        {
            eqpids = eqpids.Remove(eqpids.Length - 1, 1);
            string sql = string.Format(@"
delete MES_BN_ProductProcessRouteEquipment where ProcessRouteID ='{0}' 
insert into MES_BN_ProductProcessRouteEquipment(ProcessRouteID,EquipmentCode,EquipmentName,epqID)
select '{0}',EquipmentCode,EquipmentName,ID from SYS_Equipment where id in({1})", gyid, eqpids);
            db.Sql(sql).Execute();
        }
        public int Insert(MES_BN_ProductProcessRouteEquipment model)
        {
            var rowsAffected = model.ID = db.Insert<MES_BN_ProductProcessRouteEquipment>("MES_BN_ProductProcessRouteEquipment", model)
     .AutoMap(x => x.ID)
     .ExecuteReturnLastId<int>();
            return rowsAffected;
        }
        public int Update(MES_BN_ProductProcessRouteEquipment model)
        {
            var rowsAffected = db.Update<MES_BN_ProductProcessRouteEquipment>("MES_BN_ProductProcessRouteEquipment", model)
     .AutoMap(x => x.ID)
     .Where(x => x.ID)
     .Execute();
            return rowsAffected;
        }
    }

    public class MES_BN_ProductProcessRouteEquipment : ModelBase
    {
        [Identity]
        [PrimaryKey]   
        public int ID { get; set; }
        public string epqID { get; set; }
        public int ProcessRouteID { get; set; }
        public string EquipmentCode { get; set; }
        public string EquipmentName { get; set; }
        public string EquitmentType { get; set; }
        public string AffiliatedWorkshopID { get; set; }
        public string EquiptmentParms { get; set; }
        public string EquipmentState { get; set; }
        public int? IsEnable { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }
    }
}
