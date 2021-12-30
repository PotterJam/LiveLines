using System;
using System.Threading.Tasks;

namespace LiveLines.Api.Songs;

public interface ISongStore
{
    Task<Guid> AddSong(string spotifySongId);
}