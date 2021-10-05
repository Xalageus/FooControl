using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FooControl.BeefAPITypes;
using RestSharp;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using Newtonsoft.Json.Linq;

namespace FooControl
{
    class BeefwebAPI
    {
        private Uri hostApi { get; set; }
        private RestClient client;

        public BeefwebAPI(string host, int port)
        {
            this.hostApi = new Uri("http://" + host + ":" + port.ToString() + "/api/");
            this.client = new RestClient(this.hostApi);
        }

        public Player getPlayer()
        {
            RestRequest request = new RestRequest("player", DataFormat.Json);
            IRestResponse response = client.Get(request);

            Player player;
            //Newtonsoft.Json.Serialization.ITraceWriter traceWriter = new Newtonsoft.Json.Serialization.MemoryTraceWriter();
            try
            {
                //player = JsonConvert.DeserializeObject<PlayerRoot>(response.Content, new JsonSerializerSettings { TraceWriter = traceWriter }).Player;
                player = JsonConvert.DeserializeObject<PlayerRoot>(response.Content).Player;
            }
            catch
            {
                //System.Diagnostics.Debug.WriteLine(traceWriter);
                return null;
            }

            return player;
        }

        public Playlist[] getPlaylists()
        {
            RestRequest request = new RestRequest("playlists", DataFormat.Json);
            IRestResponse response = client.Get(request);

            Playlist[] playlists;
            //Newtonsoft.Json.Serialization.ITraceWriter traceWriter = new Newtonsoft.Json.Serialization.MemoryTraceWriter();
            try
            {
                //playlists = JsonConvert.DeserializeObject<PlaylistsRoot>(response.Content, new JsonSerializerSettings { TraceWriter = traceWriter }).Playlists;
                playlists = JsonConvert.DeserializeObject<PlaylistsRoot>(response.Content).Playlists;
            }
            catch
            {
                //System.Diagnostics.Debug.WriteLine(traceWriter);
                return null;
            }

            return playlists;
        }

        public PlaylistItems getPlaylistItems(int playlistID, int offset, int count, string[] columns)
        {
            RestRequest request = new RestRequest("playlists/{playlistId}/items/{range}", DataFormat.Json);
            request.AddUrlSegment("playlistId", playlistID);
            request.AddUrlSegment("range", offset + ":" + count);
            request.AddParameter("columns", String.Join(",", new List<string>(columns).ConvertAll(i => i.ToString()).ToArray()));
            IRestResponse response = client.Get(request);

            PlaylistItems items;
            //Newtonsoft.Json.Serialization.ITraceWriter traceWriter = new Newtonsoft.Json.Serialization.MemoryTraceWriter();
            try
            {
                //items = JsonConvert.DeserializeObject<PlaylistItemsRoot>(response.Content, new JsonSerializerSettings { TraceWriter = traceWriter }).PlaylistItems;
                items = JsonConvert.DeserializeObject<PlaylistItemsRoot>(response.Content).PlaylistItems;
                items.setColumnHeaders(columns);
            }
            catch
            {
                //System.Diagnostics.Debug.WriteLine(traceWriter);
                return null;
            }

            return items;
        }

        public ObservableCollection<SongItem> convertToSongItems(PlaylistItems items)
        {
            ObservableCollection<SongItem> songs = new ObservableCollection<SongItem>();
            
            foreach(PlaylistItem item in items.Items)
            {
                SongItem song = new SongItem();
                
                for(int i = 0; i < items.ColumnHeaders.Length; i++)
                {
                    //There has to be a better way to do this...
                    switch (items.ColumnHeaders[i])
                    {
                        case GenericFields.Album:
                            song.Album = item.Columns[i];
                            break;
                        case GenericFields.AlbumArtist:
                            song.AlbumArtist = item.Columns[i];
                            break;
                        case GenericFields.Artist:
                            song.Artist = item.Columns[i];
                            break;
                        case GenericFields.Bitrate:
                            song.Bitrate = item.Columns[i];
                            break;
                        case GenericFields.Channels:
                            song.Channels = item.Columns[i];
                            break;
                        case GenericFields.Codec:
                            song.Codec = item.Columns[i];
                            break;
                        case GenericFields.CodecProfile:
                            song.CodecProfile = item.Columns[i];
                            break;
                        case GenericFields.Date:
                            song.Date = item.Columns[i];
                            break;
                        case GenericFields.DirectoryName:
                            song.DirectoryName = item.Columns[i];
                            break;
                        case GenericFields.DiscNumber:
                            song.DiscNumber = item.Columns[i];
                            break;
                        case GenericFields.Filename:
                            song.Filename = item.Columns[i];
                            break;
                        case GenericFields.FilenameExt:
                            song.FilenameExt = item.Columns[i];
                            break;
                        case GenericFields.Filesize:
                            song.Filesize = item.Columns[i];
                            break;
                        case GenericFields.FilesizeNatural:
                            song.FilesizeNatural = item.Columns[i];
                            break;
                        case GenericFields.Genre:
                            song.Genre = item.Columns[i];
                            break;
                        case GenericFields.LastModified:
                            song.LastModified = item.Columns[i];
                            break;
                        case GenericFields.Length:
                            song.Length = item.Columns[i];
                            break;
                        case GenericFields.LengthSeconds:
                            song.LengthSeconds = item.Columns[i];
                            break;
                        case GenericFields.Path:
                            song.Path = item.Columns[i];
                            break;
                        case GenericFields.PathSort:
                            song.PathSort = item.Columns[i];
                            break;
                        case GenericFields.ReplayGainAlbumGain:
                            song.ReplayGainAlbumGain = item.Columns[i];
                            break;
                        case GenericFields.ReplayGainAlbumPeak:
                            song.ReplayGainAlbumPeak = item.Columns[i];
                            break;
                        case GenericFields.ReplayGainAlbumPeakDB:
                            song.ReplayGainAlbumPeakDB = item.Columns[i];
                            break;
                        case GenericFields.ReplayGainTrackGain:
                            song.ReplayGainTrackGain = item.Columns[i];
                            break;
                        case GenericFields.ReplayGainTrackPeak:
                            song.ReplayGainTrackPeak = item.Columns[i];
                            break;
                        case GenericFields.ReplayGainTrackPeakDB:
                            song.ReplayGainTrackPeakDB = item.Columns[i];
                            break;
                        case GenericFields.SampleRate:
                            song.SampleRate = item.Columns[i];
                            break;
                        case GenericFields.SubSong:
                            song.SubSong = item.Columns[i];
                            break;
                        case GenericFields.Title:
                            song.Title = item.Columns[i];
                            break;
                        case GenericFields.TotalDiscs:
                            song.TotalDiscs = item.Columns[i];
                            break;
                        case GenericFields.TotalTracks:
                            song.TotalTracks = item.Columns[i];
                            break;
                        case GenericFields.TrackArtist:
                            song.TrackArtist = item.Columns[i];
                            break;
                        case GenericFields.TrackNumber:
                            song.TrackNumber = item.Columns[i];
                            break;
                        default:
                            break;
                    }
                }

                songs.Add(song);
            }

            return songs;
        }

        public void playPlaylistItem(int playlistID, int index)
        {
            RestRequest request = new RestRequest("player/play/{playlistId}/{index}");
            request.AddUrlSegment("playlistId", playlistID);
            request.AddUrlSegment("index", index);
            client.Post(request);
        }

        public byte[] getArtwork(int playlistID, int index)
        {
            RestRequest request = new RestRequest("artwork/{playlistId}/{index}");
            request.AddUrlSegment("playlistId", playlistID);
            request.AddUrlSegment("index", index);
            IRestResponse response = client.Get(request);

            //check if we have an image
            //otherwise return null
            if(response != null)
            {
                if (!isValidJSON(response.Content))
                {
                    return response.RawBytes;
                }
            }

            return null;
        } 

        private bool isValidJSON(string data)
        {
            try
            {
                JToken obj = JToken.Parse(data);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public SongHolder getSongFromPlaylist(int playlistID, int index)
        {
            RestRequest request = new RestRequest("playlists/{playlistId}/items/{range}", DataFormat.Json);
            request.AddUrlSegment("playlistId", playlistID);
            request.AddUrlSegment("range", index + ":" + 1);
            string[] columns = new string[] { GenericFields.Title, GenericFields.Artist, GenericFields.Album, GenericFields.AlbumArtist, GenericFields.Date, GenericFields.TrackArtist, GenericFields.TrackNumber, GenericFields.Length, GenericFields.LengthSeconds };
            request.AddParameter("columns", String.Join(",", new List<string>(columns).ConvertAll(i => i.ToString()).ToArray()));
            IRestResponse response = client.Get(request);

            PlaylistItems items;
            try
            {
                items = JsonConvert.DeserializeObject<PlaylistItemsRoot>(response.Content).PlaylistItems;
                items.setColumnHeaders(columns);
            }
            catch
            {
                return null;
            }

            SongHolder songData = new SongHolder(convertToSongItems(items)[0], items, playlistID);
            return songData;
        }

        public void setPlaybackPosition(float position)
        {
            RestRequest request = new RestRequest("player", Method.POST);
            request.AddParameter("position", position, ParameterType.QueryString);
            client.Execute(request);
        }

        public void playPauseToggle()
        {
            RestRequest request = new RestRequest("player/pause/toggle");
            client.Post(request);
        }

        public void playPrevious()
        {
            RestRequest request = new RestRequest("player/previous");
            client.Post(request);
        }

        public void playNext()
        {
            RestRequest request = new RestRequest("player/next");
            client.Post(request);
        }

        public void playRandom()
        {
            RestRequest request = new RestRequest("player/play/random");
            client.Post(request);
        }

        public void stopPlayer()
        {
            RestRequest request = new RestRequest("player/stop");
            client.Post(request);
        }

        public void setVolume(float volume)
        {
            RestRequest request = new RestRequest("player", Method.POST);
            request.AddParameter("volume", volume, ParameterType.QueryString);
            client.Execute(request);
        }

        public void toggleMute(Player data)
        {
            RestRequest request = new RestRequest("player", Method.POST);

            if (data.Volume.IsMuted)
            {
                request.AddParameter("isMuted", "false", ParameterType.QueryString);
            }
            else
            {
                request.AddParameter("isMuted", "true", ParameterType.QueryString);
            }

            client.Execute(request);
        }

        public void setPlaybackMode(int mode)
        {
            RestRequest request = new RestRequest("player", Method.POST);
            request.AddParameter("playbackMode", mode, ParameterType.QueryString);
            client.Execute(request);
        }
    }
}
