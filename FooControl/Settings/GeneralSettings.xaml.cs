using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI;

namespace FooControl.Settings
{
    public sealed partial class GeneralSettings : Page
    {
        private SettingsPage settingsPage;
        private SettingsWidget settingsWidget;
        private GeneralSettingsStore store;
        private bool loaded = false;
        private IList<HBInterval> intervals;

        public GeneralSettings()
        {
            this.InitializeComponent();
        }

        private void createHBIntervalItems()
        {
            intervals = new List<HBInterval>();
            
            HBInterval def = new HBInterval("1 second", 1);
            intervals.Add(def);
            intervals.Add(new HBInterval("2 seconds", 2));
            intervals.Add(new HBInterval("5 seconds", 5));
            intervals.Add(new HBInterval("10 seconds", 10));
            intervals.Add(new HBInterval("15 seconds", 15));
            intervals.Add(new HBInterval("20 seconds", 20));
            intervals.DefaultIfEmpty(def);

            heartbeatIntervalComboBox.ItemsSource = intervals;
        }

        private void setHBInterval(int value)
        {
            heartbeatIntervalComboBox.SelectedItem = intervals.SingleOrDefault(x => x.value == value);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            PageParameter param = e.Parameter as PageParameter;
            if (param.settingsWidget == null)
            {
                settingsPage = param.settingsPage;
            }
            else
            {
                settingsWidget = param.settingsWidget;
            }

            store = (GeneralSettingsStore)param.store;

            //Set options on controls
            songHighlightColorPicker.SelectedColor = store.songHighlightColor;
            highlightPlaylistCB.IsChecked = store.highlightPlaylist;
            playlistHighlightColorPicker.SelectedColor = store.playlistHighlightColor;
            volumeCurveComboBox.ItemsSource = Enum.GetValues(typeof(VolumeCurveType));
            volumeCurveComboBox.SelectedItem = store.volumeCurve;
            showNetworkIndicatorsCB.IsChecked = store.showNetworkIndicators;
            localUpdateSongTimeCB.IsChecked = store.locallyUpdateSongTime;
            spacebarTogglesPlaybackCB.IsChecked = store.spacebarTogglesPlayback;

            createHBIntervalItems();
            setHBInterval(store.heartbeatInterval);

            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            SendSettings();

            base.OnNavigatingFrom(e);
        }

        private void SendModified()
        {
            if (settingsPage == null)
            {
                settingsWidget.setModified();
            }
            else
            {
                settingsPage.SetModified();
            }
        }

        public void SendSettings()
        {
            store.heartbeatInterval = ((HBInterval)heartbeatIntervalComboBox.SelectedItem).value;
            store.highlightPlaylist = highlightPlaylistCB.IsChecked.Value;
            store.locallyUpdateSongTime = localUpdateSongTimeCB.IsChecked.Value;
            store.playlistHighlightColor = playlistHighlightColorPicker.SelectedColor;
            store.showNetworkIndicators = showNetworkIndicatorsCB.IsChecked.Value;
            store.songHighlightColor = songHighlightColorPicker.SelectedColor;
            store.spacebarTogglesPlayback = spacebarTogglesPlaybackCB.IsChecked.Value;
            store.volumeCurve = (VolumeCurveType)volumeCurveComboBox.SelectedItem;

            if (settingsPage == null)
            {
                settingsWidget.ReceiveSettings(store);
            }
            else
            {
                settingsPage.ReceiveSettings(store);
            }
        }

        public void ColorPickerButton_ColorChanged(object sender, ColorChangedEventArgs e)
        {
            SendModified();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //Apparently the ColorPicker instance is still null during the constructor...
            songHighlightColorPicker.ColorPicker.ColorChanged += ColorPickerButton_ColorChanged;
            playlistHighlightColorPicker.ColorPicker.ColorChanged += ColorPickerButton_ColorChanged;

            loaded = true;
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            SendModified();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (loaded)
            {
                SendModified();
            }
        }
    }
}
