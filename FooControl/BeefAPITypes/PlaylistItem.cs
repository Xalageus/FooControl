using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FooControl.BeefAPITypes
{
    public class PlaylistItemsRoot
    {
        public PlaylistItems PlaylistItems { get; set; }
    }

    public class PlaylistItems
    {
        public int Offset { get; set; }
        public int TotalCount { get; set; }
        public PlaylistItem[] Items { get; set; }
        public string[] ColumnHeaders;

        public void setColumnHeaders(string[] headers)
        {
            ColumnHeaders = headers;
        }
    }

    public class PlaylistItem
    {
        public string[] Columns { get; set; }
    }
}
