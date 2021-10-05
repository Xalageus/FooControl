using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FooControl.Settings
{
    [XmlInclude(typeof(DefaultSettings))]
    public class SettingsStore
    {
        public GeneralSettingsStore generalSettings = new GeneralSettingsStore();
        public ServerSettingsStore serverSettings = new ServerSettingsStore();
        public ColumnSettingsStore columnsSettings = new ColumnSettingsStore();
    }

    public class SpecificStore
    {

    }

    public class GeneralSettingsStore : SpecificStore
    {
        public Windows.UI.Color songHighlightColor { get; set; }
        public bool highlightPlaylist { get; set; }
        public Windows.UI.Color playlistHighlightColor { get; set; }
        public VolumeCurveType volumeCurve { get; set; }
        public bool showNetworkIndicators { get; set; }
        public bool locallyUpdateSongTime { get; set; }
        public int heartbeatInterval { get; set; }
        public bool spacebarTogglesPlayback { get; set; }
    }

    public class ServerSettingsStore : SpecificStore
    {
        public List<ServerLoginSettings> logins = new List<ServerLoginSettings>();
        public int currentServer { get; set; }
    }

    public class ServerLoginSettings
    {
        public string hostAddress { get; set; }
        public string hostPort { get; set; }
        public string username { get; set; }
        public string password { get; set; }

        public ServerLoginSettings()
        {

        }

        public ServerLoginSettings(string hostAddress, string hostPort)
        {
            this.hostAddress = hostAddress;
            this.hostPort = hostPort;
        }

        public ServerLoginSettings(string hostAddress, string hostPort, string username, string password)
        {
            this.hostAddress = hostAddress;
            this.hostPort = hostPort;
            this.username = username;
            this.password = password;
        }
    }

    public class ColumnSettingsStore : SpecificStore
    {
        public List<string> columns = new List<string>();
    }
}
