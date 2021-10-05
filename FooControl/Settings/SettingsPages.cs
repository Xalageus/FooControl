using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace FooControl.Settings
{
    static class SettingsPages
    {
        public static readonly Dictionary<string, Type> Pages = new Dictionary<string, Type>()
        {
            { "General", typeof(GeneralSettings) },
            { "Media Server", typeof(MediaServerSettings) },
            { "Columns", typeof(ColumnsSettings) },
            { "About", typeof(AboutPage) }
        };
    }
}
