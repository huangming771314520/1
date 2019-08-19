namespace Zephyr.Data.Providers.Common.Builders
{
    internal class InsertBuilderSqlGenerator
    {
        //#region 原始
        //public string GenerateSql(IDbProvider provider, string parameterPrefix, BuilderData data)
        //{
        //    var insertSql = "";
        //    var valuesSql = "";
        //    foreach (var column in data.Columns)
        //    {
        //        if (insertSql.Length > 0)
        //        {
        //            insertSql += ",";
        //            valuesSql += ",";
        //        }

        //        insertSql += provider.EscapeColumnName(column.ColumnName);
        //        valuesSql += parameterPrefix + column.ParameterName;
        //    }

        //    var sql = string.Format("insert into {0}({1}) values({2})",
        //                                data.ObjectName,
        //                                insertSql,
        //                                valuesSql);

        //    return sql;
        //}
        //#endregion

        #region 李文祥修改
        public string GenerateSql(IDbProvider provider, string parameterPrefix, BuilderData data)
        {
            var insertSql = "";
            var valuesSql = "";
            foreach (var column in data.Columns)
            {
                if (column.Value!=null)
                {
                    if (column.Value.ToString()!="")
                    {
                        if (insertSql.Length > 0)
                        {
                            insertSql += ",";
                            valuesSql += "',";
                        }

                        insertSql += provider.EscapeColumnName(column.ColumnName);
                        valuesSql += "'" + column.Value;
                    }
                    
                }
                
            }
            valuesSql = valuesSql + "'";
            var sql = string.Format("insert into {0}({1}) values({2})",
                                        data.ObjectName,
                                        insertSql,
                                        valuesSql);

            return sql;
        }
        #endregion

    }
}
