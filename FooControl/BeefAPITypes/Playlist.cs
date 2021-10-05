using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FooControl.BeefAPITypes
{
    public class PlaylistsRoot
    {
        public Playlist[] Playlists { get; set; }
    }

    public class Playlist
    {
        public string ID { get; set; }
        public int Index { get; set; }
        public string Title { get; set; }
        public bool IsCurrent { get; set; }
        public int ItemCount { get; set; }
        public float TotalTime { get; set; }
    }
}
