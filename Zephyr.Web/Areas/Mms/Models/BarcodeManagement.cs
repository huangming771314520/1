using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;
using System.Linq;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class BarcodeManagementService : ServiceBase<BarcodeManagement>
    {
        public dynamic GetDepartmentList()
        {
            using (var db = Db.Context("Mms"))
            {
                string sql = string.Format(@"SELECT id,DepartmentName,DepartmentCode FROM SYS_BN_Department WHERE IsEnable = 1");
                return db.Sql(sql).QueryMany<dynamic>();
            }
        }

        /// <summary>
        /// 根据人员编码、人员类型查人员信息
        /// </summary>
        /// <param name="uName"></param>
        /// <param name="dCode"></param>
        /// <returns></returns>
        public dynamic GetUserList(string uName, string dCode, int page, int rows)
        {
            int startIndex = (page - 1) * rows + 1;
            int endIndex = page * rows;

            using (var db = Db.Context("Mms"))
            {
                string where = string.Empty;

                if (!string.IsNullOrEmpty(uName))
                {
                    where += string.Format(@" and UserName = '{0}'", uName);
                }
                if (!string.IsNullOrEmpty(dCode))
                {
                    where += string.Format(@" and DepartmentCode = '{0}'", dCode);
                }

                string sqlA = string.Format("SELECT COUNT(ID) FROM SYS_BN_User WHERE IsEnable = 1 {0}", where);
                string sqlB = string.Format("SELECT * FROM (SELECT *,DENSE_RANK() OVER(ORDER BY ID ASC) [row] FROM SYS_BN_User WHERE IsEnable = 1 {0}) tbTemp WHERE tbTemp.[row] BETWEEN {1} AND {2}", where, startIndex, endIndex);

                return new
                {
                    total = db.Sql(sqlA).QueryMany<int>(),
                    rows = db.Sql(sqlB).QueryMany<dynamic>()
                };

            }
        }

        /// <summary>
        /// 根据人员ID生成条码
        /// </summary>
        /// <param name="userIDs"></param>
        /// <returns></returns>
        public dynamic GetUpdateUserBarCodeByUID(List<int> userIDs)
        {
            if (userIDs.Count <= 0)
            {
                return new ResultModel
                {
                    Result = false,
                    Msg = @"未选择要生成条码的人员！"
                };
            }

            using (var db = Db.Context("Mms"))
            {
                //需要生成条码的ID
                string sqlA = string.Format(@"SELECT ID FROM dbo.SYS_BN_User WHERE ID IN ({0}) AND (UserBarcode IS NULL OR LEN(UserBarcode) = 0)", string.Join(",", userIDs));
                List<int> IDs = db.Sql(sqlA).QueryMany<int>();

                if (IDs.Count <= 0)
                {
                    return new ResultModel()
                    {
                        Result = false,
                        Msg = @"没有需要生成条码的人员！"
                    };
                }

                StringBuilder sb = new StringBuilder();

                string maxBarcode = db.Sql("SELECT MAX(ISNULL(UserBarcode,'USER00000000')) FROM dbo.SYS_BN_User").QuerySingle<string>();
                long maxCode = Convert.ToInt64(maxBarcode.Substring(4));

                foreach (int item in IDs)
                {
                    sb.Append(string.Format(@"
UPDATE dbo.SYS_BN_User
SET UserBarcode = '{0}'
WHERE ID = '{1}'
      AND
      (
          UserBarcode IS NULL
          OR UserBarcode = ''
      );
", "USER" + (++maxCode).ToString().PadLeft(8, '0'), item));
                }

                int tempN = db.Sql(sb.ToString()).Execute();

                return new ResultModel()
                {
                    Result = true
                };
            }
        }


        /// <summary>
        /// 根据零件编码、零件类型查零件信息
        /// </summary>
        /// <param name="partCode"></param>
        /// <param name="partType"></param>
        /// <returns></returns>
        public dynamic GetPartByPCodeAndPType(string pName, string pFigureNo, int page, int rows)
        {
            int startIndex = (page - 1) * rows + 1;
            int endIndex = page * rows;

            string where = string.Empty;
            if (!string.IsNullOrEmpty(pName))
            {
                where += string.Format(" AND PartName LIKE '%{0}%' ", pName);
            }
            if (!string.IsNullOrEmpty(pFigureNo))
            {
                where += string.Format(" AND PartFigureCode LIKE '%{0}%' ", pFigureNo);
            }

            string sqlA = string.Format(@"SELECT COUNT(ID) FROM dbo.SYS_MaterialBatch WHERE 1 = 1 {0}", where);
            string sqlB = string.Format(@"
SELECT *
FROM
(
    SELECT tbA.*,
           tbB.ProjectName,
           tbC.ProductName,
           DENSE_RANK() OVER (ORDER BY tbA.ID) [row]
    FROM dbo.SYS_MaterialBatch tbA
        LEFT JOIN dbo.PMS_BN_Project tbB
            ON tbB.ContractCode = tbA.ContractCode
               AND tbB.IsEnable = 1
        LEFT JOIN dbo.PMS_BN_ProjectDetail tbC
            ON tbA.ProductID = tbC.ID
               AND tbC.IsEnable = 1
    WHERE 1 = 1 {0}
) tbTemp
WHERE tbTemp.[row]
BETWEEN {1} AND {2};
", where, startIndex, endIndex);

            return new
            {
                total = db.Sql(sqlA).QueryMany<int>(),
                rows = db.Sql(sqlB).QueryMany<dynamic>()
            };
        }

        /// <summary>
        /// 根据零件ID生成条码
        /// </summary>
        /// <param name="partCode"></param>
        /// <returns></returns>
        public dynamic GetUpdatePartBarCodeByPID(List<int> partIDs)
        {
            if (partIDs.Count <= 0)
            {
                return new ResultModel
                {
                    Result = false,
                    Msg = @"未选择要生成条码的零件！"
                };
            }

            using (var db = Db.Context("Mms"))
            {
                //需要生成条码的ID
                string sqlA = string.Format(@"SELECT ID FROM dbo.SYS_Part WHERE ID IN ({0}) AND (CorrespondingBarcode IS NULL OR LEN(CorrespondingBarcode) = 0)", string.Join(",", partIDs));
                List<int> IDs = db.Sql(sqlA).QueryMany<int>();

                if (IDs.Count <= 0)
                {
                    return new ResultModel()
                    {
                        Result = false,
                        Msg = @"没有需要生成条码的零件！"
                    };
                }

                StringBuilder sb = new StringBuilder();

                long maxBarcode = db.Sql("SELECT MAX(CONVERT(BIGINT,ISNULL(CorrespondingBarcode,0))) FROM SYS_Part").QuerySingle<long>();

                foreach (int item in IDs)
                {
                    sb.Append(string.Format(@"
UPDATE dbo.SYS_Part
SET CorrespondingBarcode = '{0}'
WHERE ID = '{1}'
      AND
      (
          CorrespondingBarcode IS NULL
          OR CorrespondingBarcode = ''
      );
", (++maxBarcode).ToString().PadLeft(12, '0'), item));
                }

                int tempN = db.Sql(sb.ToString()).Execute();

                return new ResultModel()
                {
                    Result = true
                };
            }
        }





    }




    public class BarcodeManagement : ModelBase
    {

    }
}