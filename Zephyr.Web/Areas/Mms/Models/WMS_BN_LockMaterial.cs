using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class WMS_BN_LockMaterialService : ServiceBase<WMS_BN_LockMaterial>
    {
        public int ConfirmLockState(WMS_BN_LockMaterial info, out string msg)
        {
            msg = "";
            db.UseTransaction(true);
            var rowsAffected = 0;
            rowsAffected = db.Update<WMS_BN_LockMaterial>("WMS_BN_LockMaterial", info).AutoMap(x => x.ID).Where(x => x.ID).Execute();
            if (rowsAffected < 0)
            {
                db.Rollback();
                return rowsAffected;
            }

            var sql = String.Format(@"select InventoryCode from SYS_Part where InventoryCode = '{0}'", info.InventoryCode);
            sql = String.Format(@"select * from WMS_BN_RealStock where InventoryCode = '{0}' and WarehouseCode='{1}'", info.InventoryCode, info.WarehouseCode);
            WMS_BN_RealStock product = db.Sql(sql).QuerySingle<WMS_BN_RealStock>();
            if (product == null)
            {
                msg = "该仓库物料不存在！";
                return 0;
            }
            if (info.LockState == 1)
            {
                product.RealStock = product.RealStock - info.LockQuantity;
                product.LockStock = product.LockStock + info.LockQuantity;
            }
            else if (info.LockState == 2)
            {
                product.RealStock = product.RealStock + info.LockQuantity;
                product.LockStock = product.LockStock - info.LockQuantity;
            }
            product.ModifyTime = DateTime.Now;

            rowsAffected = db.Update<WMS_BN_RealStock>("WMS_BN_RealStock", product).AutoMap(x => x.ID).Where(x => x.ID).Execute();

            if (rowsAffected < 0)
            {
                db.Rollback();
                return rowsAffected;
            }

            db.Commit();
            return rowsAffected;
        }
    }

    public class WMS_BN_LockMaterial : ModelBase
    {
        [Identity]
        [PrimaryKey]
        public int ID { get; set; }
        public string InventoryCode { get; set; }
        public string InventoryName { get; set; }
        public float LockQuantity { get; set; }
        public string WarehouseCode { get; set; }
        public string WarehouseName { get; set; }
        public string MaterialCategoryNum { get; set; }
        public string MaterialCategoryName { get; set; }
        public int? LockState { get; set; }
        public string LockDescription { get; set; }
        public string UnLockDescription { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }
        public int? IsEnable { get; set; }
    }
}
