using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ProjectContacts.Models
{
    public class MusicRepository : IMusicRepository
    {
        private MusicLibraryContext _dbContext;

        public MusicRepository(MusicLibraryContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<TrackInfo> GetAllTracks(int pageNum, int pageSize)
        {
            var result = _dbContext.Tracks.OrderBy(t => t.Album.Title)
                                           .ThenBy(t => t.DiscNum ?? 1)
                                           .ThenBy(t => t.TrackNum ?? 1)
                                           .Skip((pageNum - 1) * pageSize)
                                           .Take(pageSize)
                                           .Select(t => new TrackInfo
                                            {
                                                TrackId = t.TrackId,
                                                Album = t.Album.Title,
                                                AlbumArtist = t.Album.AlbumArtist,
                                                Artist = t.Artist.Title,
                                                Year = t.Album.Year,
                                                Title = t.Title,
                                                TrackNum = t.TrackNum
                                            });
            return result;
        }

        public int AllTracksCount
        {
            get
            {
                var result = _dbContext.Tracks.Count();
                return result;
            }
        }

        public int AddPlayList(string title)
        { 
            var pl = new Playlist() { Title = title };
            _dbContext.Playlists.Add(pl);
            _dbContext.SaveChanges();
            return pl.PlaylistId;
        }

        public TrackInfo PlayListAddTrack(int playlistId, int trackId, int trackNumInPlaylist)
        {
            var p = _dbContext.Playlists.FirstOrDefault(x => x.PlaylistId == playlistId);
            // Use eager loading of track related objects by using Include(), as we need them a bit later:
            var t = _dbContext.Tracks.Include(x => x.Album).Include(x => x.Artist).FirstOrDefault(x => x.TrackId == trackId);
            TrackInfo track = null;
            if (p != null && t != null)
            {
                var plt = new PlaylistTrack
                {
                    Track = t,
                    Playlist = p,
                    TrackNum = trackNumInPlaylist
                };
                _dbContext.PlaylistTracks.Add(plt);
                _dbContext.SaveChanges();
                track = new TrackInfo
                {
                    TrackId = plt.PlaylistTrackId,
                    Album = plt.Track.Album.Title,
                    AlbumArtist = plt.Track.Album.AlbumArtist,
                    Artist = plt.Track.Artist.Title,
                    Year = plt.Track.Album.Year,
                    Title = plt.Track.Title,
                    TrackNum = plt.TrackNum
                };
            }
            return track;
        }

        public IEnumerable<TrackInfo> PlayListDeleteTrack(int id, int trackId)
        {
            var trk = _dbContext.PlaylistTracks.FirstOrDefault(x => x.PlaylistTrackId == trackId && x.PlaylistId == id);
            if (trk != null)
            {
                var trackNum = trk.TrackNum;
                _dbContext.PlaylistTracks.Remove(trk);
                _dbContext.PlaylistTracks.Where(x => x.PlaylistId == id && x.TrackNum > trackNum).ToList()
                                         .ForEach(x =>
                                         {
                                             x.TrackNum = x.TrackNum - 1;
                                         });
                _dbContext.SaveChanges();

                return GetPlaylistTracks(id);

            }
            return null;
        }

        public bool DeletePlayList(int id)
        {
            var pl = _dbContext.Playlists.FirstOrDefault(x => x.PlaylistId == id);
            if (pl != null)
            {
                _dbContext.PlaylistTracks.Where(x => x.PlaylistId == id).ToList().ForEach(t => _dbContext.Remove(t));
                _dbContext.Remove(pl);
                _dbContext.SaveChanges();
                return true;
            }
            return false;
        }

        public IEnumerable<TrackInfo> PlayListMoveTrack(int id, int trackId, bool moveUp)
        {
            var trk = _dbContext.PlaylistTracks.FirstOrDefault(x => x.PlaylistTrackId == trackId && x.PlaylistId == id);
            var offset = (moveUp ? 1 : -1);
            if (trk != null)
            {
                var trkAbove = _dbContext.PlaylistTracks.FirstOrDefault(x => x.TrackNum == (trk.TrackNum - offset) && x.PlaylistId == id);
                if (trkAbove != null)
                {
                    trk.TrackNum -= offset;
                    trkAbove.TrackNum += offset;
                }
                _dbContext.SaveChanges();

                return GetPlaylistTracks(id);
            }
            return null;
        }

        public int AllPlaylistsCount
        {
            get
            {
                var result = _dbContext.Playlists.Count();
                return result;
            }
        }

        public IEnumerable<PlaylistSummary> GetAllPlaylists(int pageNum, int pageSize)
        {
            var result = _dbContext.Playlists.OrderBy(p => p.Title)
                                             .Skip((pageNum - 1) * pageSize)
                                             .Take(pageSize)
                                             .Select(p => new PlaylistSummary
                                             {
                                                 Id = p.PlaylistId,
                                                 Name = p.Title,
                                                 NumTracks = p.PlaylistTracks.Count()
                                             });
            return result;
        }

        /// <summary>
        /// Get data about a playlist
        /// </summary>
        /// <param name="id">The ID of the list to retrieve</param>
        /// <returns>A model of the data, or null if not found</returns>
        public PlaylistDetails GetPlaylist(int id)
        {
            PlaylistDetails result = null;
            var p = _dbContext.Playlists.FirstOrDefault(x => x.PlaylistId == id);

            if (p != null)
            {
                result = new PlaylistDetails
                {
                    Id = p.PlaylistId,
                    Name = p.Title                    
                };
                if (_dbContext.PlaylistTracks.Any(x => x.PlaylistId == p.PlaylistId))
                    result.Tracks = GetPlaylistTracks(p.PlaylistId);
                else
                    result.Tracks = new List<TrackInfo>(); // Empty list
            }

            return result;
        }

        private IEnumerable<TrackInfo> GetPlaylistTracks(int playlistId)
        {
            return _dbContext.PlaylistTracks.Where(x => x.PlaylistId == playlistId)
                                            .OrderBy(x => x.TrackNum)
                                            .ThenBy(x => x.Track.Title)
                                            .Select(t => new TrackInfo
                                            {
                                                TrackId = t.PlaylistTrackId,
                                                Album = t.Track.Album.Title,
                                                AlbumArtist = t.Track.Album.AlbumArtist,
                                                Artist = t.Track.Artist.Title,
                                                Year = t.Track.Album.Year,
                                                Title = t.Track.Title,
                                                TrackNum = t.TrackNum
                                            });
        }

        public IEnumerable<TrackInfo> SearchTracks(string title, string artist, string album, int maxTake)
        {
            var result = _dbContext.Tracks.OrderBy(t => t.Title)
                                           .ThenBy(t => t.Artist.Title)
                                           .ThenBy(t => t.Album.Title)
                                           .Where(t => String.IsNullOrEmpty(title) || EF.Functions.Like(t.Title, "%" + title + "%"))
                                           .Where(t => String.IsNullOrEmpty(artist) || EF.Functions.Like(t.Artist.Title, "%" + artist + "%"))
                                           .Where(t => String.IsNullOrEmpty(album) || EF.Functions.Like(t.Album.Title, "%" + album + "%"))
                                           .Take(maxTake)
                                           .Select(t => new TrackInfo
                                           {
                                               TrackId = t.TrackId,
                                               Album = t.Album.Title,
                                               AlbumArtist = t.Album.AlbumArtist,
                                               Artist = t.Artist.Title,
                                               Year = t.Album.Year,
                                               Title = t.Title,
                                               TrackNum = t.TrackNum
                                           });
            return result;
        }

        public PlaylistDetails UpdatePlayListTitle(int id, string title)
        {
            var pl = _dbContext.Playlists.FirstOrDefault(x => x.PlaylistId == id);
            if (pl != null)
            {
                pl.Title = title;
                _dbContext.SaveChanges();
                return GetPlaylist(id);
            }
            return null;
        }
    }
}
