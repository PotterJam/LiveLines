using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using Extensions;
using LiveLines.Api.Database;
using LiveLines.Api.Lines;
using LiveLines.Api.Users;

namespace LiveLines.Lines
{
    public class LinesStore : ILinesStore
    {
        private readonly IDatabaseCommandExecutor _dbExecutor;

        public LinesStore(IDatabaseCommandExecutor dbExecutor)
        {
            _dbExecutor = dbExecutor;
        }

        public async Task<IEnumerable<Line>> GetLines(User user)
        {
            return await _dbExecutor.ExecuteCommand(async cmd =>
            {
                cmd.AddParam("@userid", user.InternalId);

                cmd.CommandText = @"
                    SELECT id, body, created_at
                    FROM lines
                    WHERE user_id = @userid
                    ORDER BY created_at DESC;";

                var reader = await cmd.ExecuteReaderAsync();
                var lines = new List<Line>();
                
                while (await reader.ReadAsync())
                {
                    var line = ReadLine(reader);
                    lines.Add(line);
                }

                return lines;
            });
        }

        public async Task<Line> CreateLine(User user, string body)
        {
            return await _dbExecutor.ExecuteCommand(async cmd =>
            {
                cmd.AddParam("@userid", user.InternalId);
                cmd.AddParam("@body", body);

                cmd.CommandText = @"
                    INSERT INTO lines (user_id, body)
                    VALUES (@userid, @body)
                    RETURNING id;";

                var id = (int?) await cmd.ExecuteScalarAsync();

                if (id == null)
                    throw new LinesStoreException($"Tried to create line for user {user.InternalId}, id not returned");

                return await GetLine(user, id.Value);
            });
        }

        private async Task<Line> GetLine(User user, int lineId)
        {
            return await _dbExecutor.ExecuteCommand(async cmd =>
            {
                cmd.AddParam("@lineid", lineId);
                cmd.AddParam("@userid", user.InternalId);

                cmd.CommandText = @"
                    SELECT id, body, created_at
                    FROM lines
                    WHERE id = @lineid
                        AND user_id = @userid
                    LIMIT 1;";

                var reader = await cmd.ExecuteReaderAsync();

                if (!await reader.ReadAsync())
                    throw new LinesStoreException($"Couldn't get line {lineId} for user {user.InternalId}");

                return ReadLine(reader);
            });
        }
        
        private Line ReadLine(DbDataReader reader)
        {
            var id = reader.Get<int>("id");
            var body = reader.Get<string>("body");
            var createdAt = reader.Get<DateTime>("created_at");

            return new Line(id, body, createdAt);
        }
    }
}