using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FooControl.BeefAPITypes
{
    public class PlayerRoot
    {
        public Player Player { get; set; }
    }

    public class Player
    {
        public ActiveItem ActiveItem { get; set; }
        public Info Info { get; set; }
        public int PlaybackMode { get; set; }
        public IList<string> PlayBackModes { get; set; }
        public string PlaybackState { get; set; }
        public Volume Volume { get; set; }

        public bool isPlaying()
        {
            if(PlaybackState == "playing")
            {
                return true;
            }

            return false;
        }

        public bool isPaused()
        {
            if(PlaybackState == "paused")
            {
                return true;
            }

            return false;
        }

        public bool isStopped()
        {
            if(PlaybackState == "stopped")
            {
                return true;
            }

            return false;
        }
    }

    public class ActiveItem
    {
        public string PlaylistId { get; set; }
        public int PlaylistIndex { get; set; }
        public int Index { get; set; }
        public float Position { get; set; }
        public float Duration { get; set; }
        public IList<string> Columns { get; set; }

        public int getPositionInIntSeconds()
        {
            return (int)Position;
        }

        public int getPositionInIntMinutes()
        {
            TimeSpan t = TimeSpan.FromSeconds(Position);
            return t.Minutes;
        }

        public int getPositionInIntHours()
        {
            TimeSpan t = TimeSpan.FromSeconds(Position);
            return t.Hours;
        }

        public string convertPositionToFormattedTime()
        {
            TimeSpan t = TimeSpan.FromSeconds(Position);
            return convertToShortFormattedTime(t);
        }

        public int getDurationInIntSeconds()
        {
            return (int)Duration;
        }

        public int getDurationInIntMinutes()
        {
            TimeSpan t = TimeSpan.FromSeconds(Duration);
            return t.Minutes;
        }

        public int getDurationInIntHours()
        {
            TimeSpan t = TimeSpan.FromSeconds(Duration);
            return t.Hours;
        }

        public string convertDurationToFormattedTime()
        {
            TimeSpan t = TimeSpan.FromSeconds(Duration);
            return convertToShortFormattedTime(t);
        }

        private string convertToShortFormattedTime(TimeSpan t)
        {
            string shortForm = "";
            bool hasHours = false;

            if (t.Hours > 0)
            {
                hasHours = true;
                shortForm += string.Format("{0}:", t.Hours.ToString());
            }

            //Always show minutes and seconds
            if (hasHours && t.Minutes.ToString().Length == 1)
            {
                shortForm += string.Format("0{0}:", t.Minutes.ToString());
            }
            else
            {
                shortForm += string.Format("{0}:", t.Minutes.ToString());
            }

            if(t.Seconds.ToString().Length == 1)
            {
                shortForm += string.Format("0{0}", t.Seconds.ToString());
            }
            else
            {
                shortForm += t.Seconds.ToString();
            }

            return shortForm;
        }
    }

    public class Info
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string Version { get; set; }
        public string PluginVersion { get; set; }
    }

    public class Volume
    {
        public bool IsMuted { get; set; }
        public float Max { get; set; }
        public float Min { get; set; }
        public string Type { get; set; }
        public float Value { get; set; }
    }
}
