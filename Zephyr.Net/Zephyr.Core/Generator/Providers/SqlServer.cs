using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Zephyr.Data;

namespace Zephyr.Core.Generator
{
    //取得表的方法
    public class SqlServerGen : ISqlGen
    {
        //取得表名，返回字段TableName即可
        public string SqlGetTableNames()
        {
            return "SELECT Name as TableName FROM sys.tables order by Name";
        }

        //取得表结构，返回字段ColumnName、SqlTypeName、MaxLength、IsNullable、IsIdentity、IsPrimaryKey、Description即可
        public string SqlGetTableSchemas(string tableName)
        {
            var sql = String.Format(@"
SELECT	sy.name						AS ColumnName, 
		type_name(sy.system_type_id)	AS SqlTypeName,
		sy.max_length					AS MaxLength,
		sy.is_nullable					AS IsNullable,
		sy.is_identity					AS IsIdentity,
		(case when exists(select 1  
						 from   syscolumns 
						 join   sysindexkeys on syscolumns.id  =sysindexkeys.id and syscolumns.colid=sysindexkeys.colid and syscolumns.id=sy.object_id 
						 join   sysindexes   on syscolumns.id  =sysindexes.id   and sysindexkeys.indid=sysindexes.indid   
						 join   sysobjects   on sysindexes.name=sysobjects.name and sysobjects.xtype= 'PK '
						 where syscolumns.name = sy.name) then 1 else 0 end) AS IsPrimaryKey,
		(select value from sys.extended_properties where sys.extended_properties.major_id = sy.object_id and sys.extended_properties.minor_id = sy.column_id) as Description
FROM sys.columns sy    
WHERE sy.object_id = object_id('{0}')
ORDER BY sy.column_id", tableName);

            return sql;
        }

        //只要返回字段column_name即可
        public string SqlGetTableKeys(string tableName)
        {
            return String.Format("sp_pkeys '{0}'", tableName);
        }
    }
}
