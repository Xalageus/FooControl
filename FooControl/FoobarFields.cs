using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FooControl
{
    static class GenericFields
    {
        public const string Title = "%title%";
        public const string Artist = "%artist%";
        public const string AlbumArtist = "%album artist%";
        public const string TrackArtist = "%track artist%";
        public const string Album = "%album%";
        public const string Date = "%date%";
        public const string Genre = "%genre%";
        public const string TrackNumber = "%tracknumber%";
        public const string TotalTracks = "%totaltracks%";
        public const string DiscNumber = "%discnumber%";
        public const string TotalDiscs = "%totaldiscs%";
        public const string Codec = "%codec%";
        public const string CodecProfile = "%codec_profile%";
        public const string Filename = "%filename%";
        public const string FilenameExt = "%filename_ext%";
        public const string DirectoryName = "%directoryname%";
        public const string Path = "%path%";
        public const string SubSong = "%subsong%";
        public const string PathSort = "%path_sort%";
        public const string Length = "%length%";
        public const string LengthSeconds = "%length_seconds%";
        public const string Bitrate = "%bitrate%";
        public const string Channels = "%channels%";
        public const string SampleRate = "%samplerate%";
        public const string ReplayGainTrackGain = "%replaygain_track_gain%";
        public const string ReplayGainAlbumGain = "%replaygain_album_gain%";
        public const string ReplayGainTrackPeak = "%replaygain_track_peak%";
        public const string ReplayGainAlbumPeak = "%replaygain_album_peak%";
        public const string ReplayGainTrackPeakDB = "%replaygain_track_peak_db%";
        public const string ReplayGainAlbumPeakDB = "%replaygain_album_peak_db%";
        public const string Filesize = "%filesize%";
        public const string FilesizeNatural = "%filesize_natural%";
        public const string LastModified = "%last_modified%";
    }

    class FoobarFieldsDicts
    {
        public Dictionary<string, string> genericFields = new Dictionary<string, string>();

        public FoobarFieldsDicts()
        {
            MemberInfo[] gFields = typeof(GenericFields).GetMembers().Skip(4).ToArray();

            foreach (MemberInfo member in gFields)
            {
                genericFields.Add(member.Name, typeof(GenericFields).GetField(member.Name).GetValue(null).ToString());
            }
        }
    }
}
