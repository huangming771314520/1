using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Zephyr.Core;
using Zephyr.Utils.ExcelLibrary.BinaryFileFormat;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class PRS_ProcessFigureService : ServiceBase<PRS_ProcessFigure>
    {
        public void UpdateProcessFigureIsEnableByProcessBomID(int processBomID)
        {
            PRS_Process_BOM ppBom = new PRS_Process_BOMService().SelectPRS_Process_BOM(processBomID.ToString());

            Dictionary<string, object> where = new Dictionary<string, object>();
            where.Add("IsEnable", 1);
            where.Add("ContractCode", ppBom.ContractCode);
            where.Add("ProductID", ppBom.ProductID);
            where.Add("PartCode", ppBom.PartCode);

            string sql = WinFormClientService.GetUpdateSQL(nameof(PRS_ProcessFigure), where, new
            {
                IsEnable = 0
            });

            db.Sql(sql).Execute();
        }

        public dynamic UpdateProcessFigureByBlueprintUpload(dynamic dynamic)
        {
            try
            {
                string sqlPBom = string.Format(@"SELECT * FROM dbo.PRS_Process_BOM WHERE ID = {0}", dynamic.PBomID);
                var pBomModel = db.Sql(sqlPBom).QuerySingle<PRS_Process_BOM>();

                Dictionary<string, object> where = new Dictionary<string, object>();
                where.Add("IsEnable", 1);
                where.Add("ContractCode", pBomModel.ContractCode);
                where.Add("ProductID", pBomModel.ProductID);
                where.Add("PartCode", pBomModel.PartCode);
                string sqlA = WinFormClientService.GetUpdateSQL(nameof(PRS_ProcessFigure), where, new
                {
                    IsEnable = 0
                });

                StringBuilder sb = new StringBuilder();
                DateTime newDT = DateTime.Now;
                foreach (var item in dynamic.FileInfos)
                {
                    sb.Append(WinFormClientService.GetInsertSQL(new PRS_ProcessFigure()
                    {
                        ContractCode = pBomModel.ContractCode,
                        ProductID = pBomModel.ProductID,
                        PartCode = pBomModel.PartCode,
                        DocName = item.DocName,
                        FileName = item.FileName,
                        FileAddress = item.FileAddress,
                        IsEnable = 1,
                        CreateTime = newDT,
                        ModifyTime = newDT
                    }));
                }
                string sqlB = sb.ToString();

                db.UseTransaction(true);

                db.Sql(sqlA).Execute();
                bool result = db.Sql(sqlB).Execute() > 0;
                if (result)
                {
                    db.Commit();
                }
                else
                {
                    db.Rollback();
                }

                return new ResultModel()
                {
                    Result = result,
                    Msg = result ? "成功！" : "失败！"
                };
            }
            catch (Exception ex)
            {
                return new ResultModel()
                {
                    Result = false,
                    Msg = ex.Message
                };
            }
        }

        public class UpdateInfoEntity
        {
            public UpdateInfoEntity()
            {
                FileInfos = new List<FileInfoEntity>();
            }

            public string UserCode { get; set; }

            public string UserName { get; set; }

            public int PBomID { get; set; }

            public List<FileInfoEntity> FileInfos { get; set; }

            public class FileInfoEntity
            {
                public string DocName { get; set; }

                public string FileName { get; set; }

                public string FileAddress { get; set; }
            }
        }

    }

    public class PRS_ProcessFigure : ModelBase
    {
        [PrimaryKey]
        [Identity]

        /// <summary>
		/// 主键ID
		/// </summary>
		public long ID { get; set; }

        /// <summary>
        /// 合同号
        /// </summary>
        public string ContractCode { get; set; }

        /// <summary>
        /// 产品ID
        /// </summary>
        public int? ProductID { get; set; }

        /// <summary>
        /// 零件编号
        /// </summary>
        public string PartCode { get; set; }

        /// <summary>
        /// 文档名称
        /// </summary>
        public string DocName { get; set; }

        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 文件路径
        /// </summary>
        public string FileAddress { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public int? IsEnable { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatePerson { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        public string ModifyPerson { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? ModifyTime { get; set; }

        public byte[] FileContent { get; set; }
    }
}