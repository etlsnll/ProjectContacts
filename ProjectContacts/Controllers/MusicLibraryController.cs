using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjectContacts.Models;

namespace ProjectContacts.Controllers
{
    [Produces("application/json")]
    [Route("api/MusicLibrary")]
    public class MusicLibraryController : Controller
    {
        private IMusicRepository _musicRepository;
        private readonly ILogger _logger;

        public MusicLibraryController(IMusicRepository musicRepository,
                                      ILogger<MusicLibraryController> logger)
        {
            _musicRepository = musicRepository;
            _logger = logger;
        }

        // GET: api/MusicLibrary/CountAllTracks
        [HttpGet("[action]")]
        public int CountAllTracks()
        {
            var num = _musicRepository.AllTracksCount;
            //_logger.LogInformation("Count All tracks method called - counted {0} tracks", num);
            return num;
        }

        /// <summary>
        /// GET: api/MusicLibrary/AllTracks (server side paged results)
        /// </summary>
        /// <param name="pageNum">Page number to get</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>List of music track info classes</returns>
        [HttpGet("[action]")]
        public IEnumerable<TrackInfo> AllTracks(int pageNum, int pageSize)
        {
            if (pageNum < 1)
                throw new ArgumentException("Value must be greater than 0", "pageNum");
            if (pageSize < 1)
                throw new ArgumentException("Value must be greater than 0", "pageSize");

            return _musicRepository.GetAllTracks(pageNum, pageSize);
        }

        /// <summary>
        /// POST: api/MusicLibrary/AddPlayList
        /// </summary>
        /// <param name="name">The name of the new playlist</param>
        [HttpPost("[action]")]
        public int AddPlayList([FromBody]PlaylistInfo pl)
        {
            return _musicRepository.AddPlayList(pl.Name);
        }

        // GET: api/MusicLibrary/CountAllPlaylists
        [HttpGet("[action]")]
        public int CountAllPlaylists()
        {
            var num = _musicRepository.AllPlaylistsCount;
            return num;
        }

        /// <summary>
        /// GET: api/MusicLibrary/AllPlaylists (server side paged results)
        /// </summary>
        /// <param name="pageNum">Page number to get</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>List of music playlist info classes</returns>
        [HttpGet("[action]")]
        public IEnumerable<PlaylistSummary> AllPlaylists(int pageNum, int pageSize)
        {
            if (pageNum < 1)
                throw new ArgumentException("Value must be greater than 0", "pageNum");
            if (pageSize < 1)
                throw new ArgumentException("Value must be greater than 0", "pageSize");

            return _musicRepository.GetAllPlaylists(pageNum, pageSize);
        }

        // GET: api/MusicLibrary/GetPlaylist/5
        [HttpGet("[action]/{id}")]
        public PlaylistDetails Playlist(int id)
        {
            var pl = _musicRepository.GetPlaylist(id);
            if (pl == null)
                _logger.LogError("Playlist with ID {} requested but not found in DB", id);
            return pl;
        }

        /// <summary>
        /// GET: api/MusicLibrary/SearchTracks (server side paged results)
        /// </summary>
        /// <param name="title">keywords to search track title</param>
        /// <param name="artist">keywords to search track artist</param>
        /// <param name="album">keywords to search track album</param>
        /// <returns>List of top 50 music tracks that match the search terms</returns>
        [HttpGet("[action]")]
        public IEnumerable<TrackInfo> SearchTracks(string title, string artist, string album)
        {
            return _musicRepository.SearchTracks(title, artist, album, 50);
        }

        /// <summary>
        /// PUT: api/MusicLibrary/UpdatePlayListTitle/5
        /// </summary>
        /// <param name="id">ID of playlist to update</param>
        /// <param name="playlist">Playlist model object containing new title</param>
        /// <returns>Updated playlist model</returns>
        [HttpPut("[action]/{id}")]
        public PlaylistSummary UpdatePlayListTitle(int id, [FromBody]PlaylistDetails playlist)
        {
            var pl = _musicRepository.UpdatePlayListTitle(id, playlist.Name);
            if (pl == null)
                _logger.LogError("Playlist with ID {0} requested but not found in DB", id);
            return pl;
        }


        /// <summary>
        /// PUT: api/MusicLibrary/PlayListAddTrack/7
        /// </summary>
        /// <param name="id">ID of playlist to update</param>
        /// <param name="info">Track model object for track to add</param>
        /// <returns>Model for new track in playlist</returns>
        [HttpPut("[action]/{id}")]
        public TrackInfo PlayListAddTrack(int id, [FromBody]TrackInfo info)
        {
            var t = _musicRepository.PlayListAddTrack(id, info.TrackId, info.TrackNum ?? 0);
            if (t == null)
                _logger.LogError("Track with ID {0} failed to be added to playlist ID {1} in DB", info.TrackId, id);

            return t;
        }

        /// <summary>
        /// DELETE: api/MusicLibrary/PlayList/4/DeleteTrack/12
        /// </summary>
        /// <param name="id">ID of playlist to update</param>
        /// <param name="trackId">ID of track in playlist to remove</param>
        /// <returns>Updated list of tracks for the playlist</returns>
        [HttpDelete("PlayList/{id}/DeleteTrack/{trackId}")]
        public IEnumerable<TrackInfo> PlayListDeleteTrack(int id, int trackId)
        {
            var tracks = _musicRepository.PlayListDeleteTrack(id, trackId);
            if (tracks == null)
                _logger.LogError("Track with ID {0} failed to be deleted from playlist ID {1} in DB", trackId, id);

            return tracks;
        }

        /// <summary>
        /// DELETE: api/MusicLibrary/DeletePlayList/4
        /// </summary>
        /// <param name="id">ID of playlist to delete</param>
        /// <returns>True if successful</returns>
        [HttpDelete("[action]/{id}")]
        public bool DeletePlayList(int id)
        {
            var result = _musicRepository.DeletePlayList(id);
            if (!result)
                _logger.LogError("Playlist with ID {0} failed to be deleted in DB", id);

            return result;
        }

        /// <summary>
        /// PUT: api/MusicLibrary/PlayListMoveTrackUp/7
        /// </summary>
        /// <param name="id">ID of playlist to update</param>
        /// <param name="info">Track model object for track to move</param>
        /// <returns>Updated list of tracks for the playlist</returns>
        [HttpPut("[action]/{id}")]
        public IEnumerable<TrackInfo> PlayListMoveTrackUp(int id, [FromBody]TrackInfo info)
        {
            var tracks = _musicRepository.PlayListMoveTrack(id, info.TrackId, true);
            if (tracks == null)
                _logger.LogError("Track with ID {0} failed to be moved up in playlist ID {1} in DB", info.TrackId, id);

            return tracks;
        }

        /// <summary>
        /// PUT: api/MusicLibrary/PlayListMoveTrackDown/56
        /// </summary>
        /// <param name="id">ID of playlist to update</param>
        /// <param name="info">Track model object for track to move</param>
        /// <returns>Updated list of tracks for the playlist</returns>
        [HttpPut("[action]/{id}")]
        public IEnumerable<TrackInfo> PlayListMoveTrackDown(int id, [FromBody]TrackInfo info)
        {
            var tracks = _musicRepository.PlayListMoveTrack(id, info.TrackId, false);
            if (tracks == null)
                _logger.LogError("Track with ID {0} failed to be moved down in playlist ID {1} in DB", info.TrackId, id);

            return tracks;
        }
    }
}
