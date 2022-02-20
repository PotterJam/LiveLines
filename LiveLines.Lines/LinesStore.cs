using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using Extensions;
using LiveLines.Api.Database;
using LiveLines.Api.Lines;
using LiveLines.Api.Users;

namespace LiveLines.Lines;

public class LinesStore : ILinesStore
{
    private readonly IDatabaseCommandExecutor _dbExecutor;

    public LinesStore(IDatabaseCommandExecutor dbExecutor)
    {
        _dbExecutor = dbExecutor;
    }

    public async Task<IEnumerable<Line>> GetLines(LoggedInUser loggedInUser)
    {
        return await _dbExecutor.ExecuteCommand(async cmd =>
        {
            cmd.AddParam("@userid", loggedInUser.InternalId);

            cmd.CommandText = @"
                    SELECT l.id, l.body, l.created_at, l.date_for, s.spotify_id
                    FROM lines l
                    LEFT JOIN songs s on s.id = l.song_id
                    WHERE l.user_id = @userid
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

    public async Task<IEnumerable<Line>> GetLinesAfter(LoggedInUser loggedInUser, DateOnly afterDate)
    {
        return await _dbExecutor.ExecuteCommand(async cmd =>
        {
            cmd.AddParam("@userId", loggedInUser.InternalId);
            cmd.AddParam("@afterDate", afterDate);

            cmd.CommandText = @"
                    SELECT l.id, l.body, l.created_at, l.date_for, s.spotify_id
                    FROM lines l
                    LEFT JOIN songs s on s.id = l.song_id
                    WHERE l.user_id = @userid
                    AND l.date_for >= @afterDate
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

    public async Task<Line> CreateLine(LoggedInUser loggedInUser, string body, Guid? songId)
    {
        return await _dbExecutor.ExecuteCommand(async cmd =>
        {
            cmd.AddParam("@userid", loggedInUser.InternalId);
            cmd.AddParam("@body", body);
            cmd.AddParam("@songId", songId);

            cmd.CommandText = @"
                    INSERT INTO lines (user_id, body, song_id)
                    VALUES (@userid, @body, @songId)
                    RETURNING id;";

            var guid = (Guid?) await cmd.ExecuteScalarAsync();

            if (guid == null)
                throw new LinesStoreException($"Tried to create line for user {loggedInUser.InternalId}, id not returned");

            return await GetLine(loggedInUser, guid.Value);
        });
    }

    private async Task<Line> GetLine(LoggedInUser loggedInUser, Guid lineId)
    {
        return await _dbExecutor.ExecuteCommand(async cmd =>
        {
            cmd.AddParam("@lineid", lineId);
            cmd.AddParam("@userid", loggedInUser.InternalId);

            cmd.CommandText = @"
                    SELECT l.id, l.body, l.created_at, l.date_for, s.spotify_id
                    FROM lines l
                    LEFT JOIN songs s on s.id = l.song_id
                    WHERE l.id = @lineid
                        AND l.user_id = @userid
                    LIMIT 1;";

            var reader = await cmd.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
                throw new LinesStoreException($"Couldn't get line {lineId} for user {loggedInUser.InternalId}");

            return ReadLine(reader);
        });
    }

    private Line ReadLine(DbDataReader reader)
    {
        var id = reader.Get<Guid>("id");
        var body = reader.Get<string>("body");
        var createdAt = reader.Get<DateTime>("created_at");
        var dateFor = reader.Get<DateOnly>("date_for");
        var spotifyId = reader.GetNullable<string?>("spotify_id"); 

        return new Line(id, body, spotifyId, createdAt, dateFor);
    }
}