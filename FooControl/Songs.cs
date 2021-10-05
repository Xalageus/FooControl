using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FooControl.BeefAPITypes;

namespace FooControl
{
    class SongItem
    {
        public string Title { get; set; }
        public string Artist { get; set; }
        public string AlbumArtist { get; set; }
        public string TrackArtist { get; set; }
        public string Album { get; set; }
        public string Date { get; set; }
        public string Genre { get; set; }
        public string TrackNumber { get; set; }
        public string TotalTracks { get; set; }
        public string DiscNumber { get; set; }
        public string TotalDiscs { get; set; }
        public string Codec { get; set; }
        public string CodecProfile { get; set; }
        public string Filename { get; set; }
        public string FilenameExt { get; set; }
        public string DirectoryName { get; set; }
        public string Path { get; set; }
        public string SubSong { get; set; }
        public string PathSort { get; set; }
        public string Length { get; set; }
        public string LengthSeconds { get; set; }
        public string Bitrate { get; set; }
        public string Channels { get; set; }
        public string SampleRate { get; set; }
        public string ReplayGainTrackGain { get; set; }
        public string ReplayGainAlbumGain { get; set; }
        public string ReplayGainTrackPeak { get; set; }
        public string ReplayGainAlbumPeak { get; set; }
        public string ReplayGainTrackPeakDB { get; set; }
        public string ReplayGainAlbumPeakDB { get; set; }
        public string Filesize { get; set; }
        public string FilesizeNatural { get; set; }
        public string LastModified { get; set; }

        public SongItem()
        {
            Title = String.Empty;
            Artist = String.Empty;
            AlbumArtist = String.Empty;
            TrackArtist = String.Empty;
            Album = String.Empty;
            Date = String.Empty;
            Genre = String.Empty;
            TrackNumber = String.Empty;
            TotalTracks = String.Empty;
            DiscNumber = String.Empty;
            TotalDiscs = String.Empty;
            Codec = String.Empty;
            CodecProfile = String.Empty;
            Filename = String.Empty;
            FilenameExt = String.Empty;
            DirectoryName = String.Empty;
            Path = String.Empty;
            SubSong = String.Empty;
            PathSort = String.Empty;
            Length = String.Empty;
            LengthSeconds = String.Empty;
            Bitrate = String.Empty;
            Channels = String.Empty;
            SampleRate = String.Empty;
            ReplayGainTrackGain = String.Empty;
            ReplayGainAlbumGain = String.Empty;
            ReplayGainTrackPeak = String.Empty;
            ReplayGainAlbumPeak = String.Empty;
            ReplayGainTrackPeakDB = String.Empty;
            ReplayGainAlbumPeakDB = String.Empty;
            Filesize = String.Empty;
            FilesizeNatural = String.Empty;
            LastModified = String.Empty;
        }
    }

    class SongHolder
    {
        public SongItem song;
        public int songIndex;
        public int playlistTotal;
        public int playlistIndex;

        public SongHolder(SongItem song, PlaylistItems items, int playlistIndex)
        {
            this.song = song;
            songIndex = items.Offset;
            playlistTotal = items.TotalCount;
            this.playlistIndex = playlistIndex;
        }

        public SongHolder(SongItem song, int songIndex, int playlistTotal, int playlistIndex)
        {
            this.song = song;
            this.songIndex = songIndex;
            this.playlistTotal = playlistTotal;
            this.playlistIndex = playlistIndex;
        }
    }
}
