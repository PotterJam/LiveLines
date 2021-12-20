using System;
using System.Data;
using System.Threading.Tasks;
using LiveLines.Api.Database;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace LiveLines.Database
{
    public class PostgresCommandExecutor : IDatabaseCommandExecutor
    {
        private readonly string  _connectionString;

        public PostgresCommandExecutor(IConfiguration configuration)
        {
            _connectionString = new NpgsqlConnectionStringBuilder
            {
                Database = configuration.GetValue<string>("DB_NAME"),
                Host = configuration.GetValue<string>("DB_HOST"),
                Port = configuration.GetValue<int>("DB_PORT"),
                Username = configuration.GetValue<string>("DB_USER_NAME"),
                Password = configuration.GetValue<string>("DB_PASSWORD"),
                MaxPoolSize = configuration.GetValue<int>("DB_MAX_POOL_SIZE")
            }.ToString();
        }

        public async Task ExecuteCommand(Func<IDbCommand, Task> action)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();
 
            using var command = conn.CreateCommand();
            await action(command);
        }
        
        public async Task<T> ExecuteCommand<T>(Func<IDbCommand, Task<T>> action)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();
 
            using var command = conn.CreateCommand();
            return await action(command);
        }
    }
}