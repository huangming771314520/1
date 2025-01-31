﻿using System;
using System.Data;
using System.Data.Common;

namespace Zephyr.Data
{
	public interface IDbProvider
	{
		IDbConnection CreateConnection();
		bool SupportsMultipleResultsets { get; }
		bool SupportsMultipleQueries { get; }
		bool SupportsOutputParameters { get; }
		bool SupportsStoredProcedures { get; }
		bool RequiresIdentityColumn { get; }
		string GetParameterName(string parameterName);
		string GetSelectBuilderAlias(string name, string alias);
		string GetSqlForSelectBuilder(SelectBuilderData data);
		string GetSqlForInsertBuilder(BuilderData data);
		string GetSqlForUpdateBuilder(BuilderData data);
		string GetSqlForDeleteBuilder(BuilderData data);
		string GetSqlForStoredProcedureBuilder(BuilderData data);
		DataTypes GetDbTypeForClrType(Type clrType);
		object ExecuteReturnLastId<T>(IDbCommand command, string identityColumnName);
		void OnCommandExecuting(IDbCommand command);
		string EscapeColumnName(string name);
    }
}
