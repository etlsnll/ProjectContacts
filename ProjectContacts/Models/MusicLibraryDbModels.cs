using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ProjectContacts.Models
{
    public class MusicLibraryContextDbFactory : IDesignTimeDbContextFactory<MusicLibraryContext>
    {
        MusicLibraryContext IDesignTimeDbContextFactory<MusicLibraryContext>.CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MusicLibraryContext>();
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
            optionsBuilder.UseSqlServer<MusicLibraryContext>(configuration.GetConnectionString("MusicLibraryDB"));

            return new MusicLibraryContext(optionsBuilder.Options);
        }
    }


    public class MusicLibraryContext : DbContext
    {
        public MusicLibraryContext(DbContextOptions<MusicLibraryContext> options) : base(options)
        { }
        
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Track> Tracks { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<PlaylistTrack> PlaylistTracks { get; set; }

    }
    
    public class Track
    {
        public int TrackId { get; set; }
        public int? DiscNum { get; set; }
        public int? TrackNum { get; set; }
        public string Title { get; set; }

        public int ArtistId { get; set; }
        public Artist Artist { get; set; }

        public int AlbumId { get; set; }
        public Album Album { get; set; }
    }
    public class Album
    {
        public int AlbumId { get; set; }
        public string Title { get; set; }
        public int? Year { get; set; }
        public string AlbumArtist { get; set; }

        public List<Track> Tracks { get; set; }
    }

    public class Artist
    {
        public int ArtistId { get; set; }
        public string Title { get; set; }
    }

    public class Playlist
    {
        public int PlaylistId { get; set; }
        public string Title { get; set; }
        public List<PlaylistTrack> PlaylistTracks { get; set; }
    }

    public class PlaylistTrack
    {
        public int PlaylistTrackId { get; set; }
        public int TrackNum { get; set; }

        public int PlaylistId { get; set; }
        public Playlist Playlist { get; set; }

        public int TrackId { get; set; }
        public Track Track { get; set; }
    }
}
