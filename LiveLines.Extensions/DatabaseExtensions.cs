using System;
using System.Data.Common;

namespace Extensions;

public static class DatabaseExtensions
{
    public static void AddParam<T>(this DbCommand command, string paramName, T value)
    {
        var parameter = command.CreateParameter();
        parameter.ParameterName = paramName;

        parameter.Value = (object?) value ?? DBNull.Value;

        command.Parameters.Add(parameter);
    }

    public static T Get<T>(this DbDataReader reader, string paramName)
    {
        return (T) reader[paramName];
    }
}