using System;
using System.Threading.Tasks;

namespace LiveLines.Api.Songs;

public interface ISongService
{
    Task<Guid> AddSong(string spotifySongId);
}