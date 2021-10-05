using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FooControl.Settings
{
    public class DefaultSettings : SettingsStore
    {
        public DefaultSettings()
        {
            //Default General Settings
            generalSettings.songHighlightColor = Windows.UI.Color.FromArgb(255, 146, 188, 146); //DarkGreen
            generalSettings.highlightPlaylist = false;
            generalSettings.playlistHighlightColor = Windows.UI.Color.FromArgb(255, 146, 188, 146); //DarkGreen
            generalSettings.volumeCurve = VolumeCurveType.Better;
            generalSettings.showNetworkIndicators = true;
            generalSettings.locallyUpdateSongTime = true;
            generalSettings.heartbeatInterval = 5;
            generalSettings.spacebarTogglesPlayback = false;

            //Default Server Settings
            serverSettings.logins.Add(new ServerLoginSettings("127.0.0.1", "8880"));
            serverSettings.currentServer = 0;

            //Default Column Settings
            columnsSettings.columns.Add("Title");
            columnsSettings.columns.Add("Artist");
            columnsSettings.columns.Add("Album");
            columnsSettings.columns.Add("Length");
        }
    }
}
