using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectContacts.Models
{
    // All models used for data in and out of web API that aren't native DB models go here:

    public class TrackInfo
    {
        public int TrackId { get; set; }
        public string Album { get; set; }
        public string AlbumArtist { get; set; }
        public string Artist { get; set; }
        public int? Year { get; set; }
        public string Title { get; set; }
        public int? TrackNum { get; set; }
    }

    public class PlaylistInfo
    {
        public string Name { get; set; }
    }

    public class PlaylistSummary : PlaylistInfo
    {
        public int? Id { get; set; }
        public int NumTracks { get; set; }
    }

    public class PlaylistDetails : PlaylistSummary
    {
        IEnumerable<TrackInfo> tracks;

        public IEnumerable<TrackInfo> Tracks
        {
            get { return tracks; }
            set
            {
                tracks = value;
                NumTracks = tracks.Count();
            }
        }
    }
}
