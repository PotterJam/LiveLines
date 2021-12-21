using System.Data.Common;

namespace Extensions
{
    public static class DatabaseExtensions
    {
        public static void AddParam<T>(this DbCommand command, string paramName, T value)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = paramName;
            parameter.Value = value;

            command.Parameters.Add(parameter);
        }
    }
}