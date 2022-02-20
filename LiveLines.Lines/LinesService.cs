using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using LiveLines.Api;
using LiveLines.Api.Lines;
using LiveLines.Api.Songs;
using LiveLines.Api.Users;

namespace LiveLines.Lines;

public class LinesService : ILinesService
{
    private readonly ILinesStore _linesStore;
    private readonly ISongService _songService;

    public LinesService(ILinesStore linesStore, ISongService songService)
    {
        _linesStore = linesStore;
        _songService = songService;
    }

    public async Task<IEnumerable<Line>> GetLines(LoggedInUser loggedInUser)
    {
        return await _linesStore.GetLines(loggedInUser);
    }

    public async Task<Line> CreateLine(LoggedInUser loggedInUser, LineToCreate lineToCreate)
    {
        // TODO: will need to validate the song id input (don't send an invalid input to the below)
        // perhaps in the song table have a flag for songs that didn't get populated properly so we can display it in the frontend

        Guid? songId = lineToCreate.SpotifySongId != null
            ? await _songService.AddSong(lineToCreate.SpotifySongId)
            : null;

        return await _linesStore.CreateLine(loggedInUser, lineToCreate.Body, songId, lineToCreate.ForYesterday);
    }

    public async Task<IEnumerable<LineOperation>> GetLineOperations(LoggedInUser loggedInUser)
    {
        var lineOperations = new Collection<LineOperation>();

        DateOnly today = DateOnly.FromDateTime(DateTime.Today);

        try
        {
            await _linesStore.GetLineForDate(loggedInUser, today);
            lineOperations.Add(LineOperation.PostToday);
        }
        catch (LinesStoreException)
        {
            if (DateTime.Now.Hour < 12)
            {
                try
                {
                    await _linesStore.GetLineForDate(loggedInUser, today.AddDays(-1));
                    lineOperations.Add(LineOperation.PostYesterday);
                }
                catch (LinesStoreException)
                {
                    // no line yesterday
                }
            }
        }

        return lineOperations;
    }
}