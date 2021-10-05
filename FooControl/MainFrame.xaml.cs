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
using FooControl.BeefAPITypes;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage.Streams;

namespace FooControl
{
    public sealed partial class MainFrame : Page
    {
        private BeefwebAPI api;
        private int numPages = 0;
        private int lastNumPages = -1;
        private ObservableCollection<SongItem> currentSongs;
        private Player playerData;
        private SongHolder currentSong;
        private int loadedPlaylist = -2;
        private bool sliderManipulateHold = false;
        DispatcherTimer updateTimer;
        DispatcherTimer autoTimer;
        private MainPage mainPage = null;

        public MainFrame()
        {
            this.InitializeComponent();
            
            Window.Current.CoreWindow.KeyUp += CoreWindow_KeyUp;

            api = new BeefwebAPI("127.0.0.1", 8880);

            generateIdleSongData();

            autoTimer = new DispatcherTimer();
            autoTimer.Tick += autoTimerUpdate;
            autoTimer.Interval = new TimeSpan(0, 0, 1);

            updatePlayerData();

            if (playerData == null)
            {
                statusText.Text = "Could not connect to foobar2000!";
            }
            else
            {
                statusText.Text = "Connected to " + playerData.Info.Title + " " + playerData.Info.Version;

                Playlist[] playlists = api.getPlaylists();
                foreach (Playlist pl in playlists)
                {
                    playlistView.Items.Add(pl.Title);
                }

                updateTimer = new DispatcherTimer();
                updateTimer.Tick += timedUpdatePlayerData;
                updateTimer.Interval = new TimeSpan(0, 0, 5);
                updateTimer.Start();
            }
        }

        private void timedUpdatePlayerData(object sender, object e)
        {
            updatePlayerData();

            //If loaded numPages changed since last timer call, call changeSelectedSongColor.
            //Easy workaround in UWP for no event on ItemsSource changed.
            //Possibility for an exception/crash if this gets called on the exact millisecond that ItemsSource gets changed.
            //Hopefully there is a better way to do this...
            if (numPages != lastNumPages)
            {
                lastNumPages = numPages;
                changeSelectedSongColor();
            }
        }

        private void autoTimerUpdate(object sender, object e)
        {
            playerData.ActiveItem.Position = playerData.ActiveItem.Position + 1;

            if (!sliderManipulateHold)
            {
                songTimeSlider.Value = playerData.ActiveItem.Position;
                songCurrentPosition.Text = playerData.ActiveItem.convertPositionToFormattedTime();
            }
        }

        private void updatePlayerData()
        {
            playerData = api.getPlayer();

            if(playerData != null)
            {
                //Update song data
                if (playerData.ActiveItem.Index != currentSong.songIndex || playerData.ActiveItem.PlaylistIndex != currentSong.playlistIndex)
                {
                    if (playerData.ActiveItem.Index == -1)
                    {
                        generateIdleSongData();
                        setSongControls();
                        setAlbumArt(null);
                        songTimeSlider.Value = 0;
                        songTimeSlider.Maximum = 100;
                        songTimeSlider.IsEnabled = false;
                        songCurrentDuration.Text = "0:00";
                    }
                    else
                    {
                        currentSong = api.getSongFromPlaylist(playerData.ActiveItem.PlaylistIndex, playerData.ActiveItem.Index);
                        setSongControls();
                        changeSelectedSongColor();
                        setAlbumArt(api.getArtwork(playerData.ActiveItem.PlaylistIndex, playerData.ActiveItem.Index));
                        songTimeSlider.IsEnabled = true;
                        songTimeSlider.Maximum = playerData.ActiveItem.Duration;
                        songCurrentDuration.Text = playerData.ActiveItem.convertDurationToFormattedTime();
                    }
                }

                //Update song time
                if (!sliderManipulateHold)
                {
                    songTimeSlider.Value = playerData.ActiveItem.Position;
                    songCurrentPosition.Text = playerData.ActiveItem.convertPositionToFormattedTime();
                }

                //Update play/pause
                if (playerData.isPaused() || playerData.isStopped())
                {
                    playPauseButton.Content = PlayerIcons.Play;
                    ToolTip tooltip = new ToolTip();
                    tooltip.Content = "Play";
                    ToolTipService.SetToolTip(playPauseButton, tooltip);

                    if (autoTimer.IsEnabled)
                    {
                        autoTimer.Stop();
                    }
                }
                else
                {
                    playPauseButton.Content = PlayerIcons.Pause;
                    ToolTip tooltip = new ToolTip();
                    tooltip.Content = "Pause";
                    ToolTipService.SetToolTip(playPauseButton, tooltip);

                    if (!autoTimer.IsEnabled)
                    {
                        autoTimer.Start();
                    }
                }

                //Update volume data
                if (!sliderManipulateHold)
                {
                    VolumeCurve curve = new VolumeCurve(true, VolumeCurveType.Better);
                    volumeSlider.Value = curve.revertVolumeCurve(playerData.Volume.Value);
                }
                
                if (playerData.Volume.IsMuted)
                {
                    muteButton.Content = PlayerIcons.VolumeMute;
                }
                else
                {
                    switch(playerData.Volume.Value)
                    {
                        case float n when (n >= -100 && n < -30):
                            muteButton.Content = PlayerIcons.Volume0;
                            break;

                        case float n when (n >= -30 && n < -15):
                            muteButton.Content = PlayerIcons.Volume1;
                            break;

                        case float n when (n >= -15 && n < -5):
                            muteButton.Content = PlayerIcons.Volume2;
                            break;

                        case float n when (n > -5):
                            muteButton.Content = PlayerIcons.Volume3;
                            break;

                        default:
                            muteButton.Content = PlayerIcons.Volume3;
                            break;
                    }
                }
            }
        }

        private void changeSelectedSongColor()
        {
            clearSongColors();

            if (currentSong.playlistIndex == loadedPlaylist && currentSong.songIndex < playlistSongView.Items.Count)
            {
                ListViewItem container = playlistSongView.ContainerFromIndex(currentSong.songIndex) as ListViewItem;
                container.Background = (SolidColorBrush)Resources["SongPlaying"];
            }
        }

        private void setSongControls()
        {
            songTitle.Text = currentSong.song.Title;
            songArtist.Text = currentSong.song.Artist;
        }

        private void generateIdleSongData()
        {
            SongItem song = new SongItem();
            song.Title = "Playback stopped.";
            currentSong = new SongHolder(song, -1, 0, -1);
        }

        private async void setAlbumArt(byte[] rawImage)
        {
            if(rawImage != null)
            {
                BitmapImage bitmap = new BitmapImage();

                using (InMemoryRandomAccessStream ms = new InMemoryRandomAccessStream())
                {
                    await ms.WriteAsync(rawImage.AsBuffer());
                    ms.Seek(0);
                    await bitmap.SetSourceAsync(ms);
                }

                albumArtwork.Source = bitmap;
            }
            else
            {
                albumArtwork.Source = null;
            }
        }

        private void playlistView_ItemClick(object sender, ItemClickEventArgs e)
        {
            currentSongs = new ObservableCollection<SongItem>();
            numPages = 0;
            lastNumPages = -1;
            updatePlaylistSongs(playlistView.Items.IndexOf(e.ClickedItem), numPages);
            loadedPlaylist = playlistView.Items.IndexOf(e.ClickedItem);
        }

        private void updatePlaylistSongs(int playlist, int page)
        {
            PlaylistItems items = api.getPlaylistItems(playlist, page * 50, 50, new string[] { GenericFields.Title, GenericFields.Artist, GenericFields.Album, GenericFields.Length });
            ObservableCollection<SongItem> songs = currentSongs;

            if (items == null)
            {
                statusText.Text = "Could not connect to foobar2000!";
            }
            else
            {
                ObservableCollection<SongItem> newSongs = api.convertToSongItems(items);

                foreach (SongItem song in newSongs)
                {
                    songs.Add(song);
                }

                playlistSongView.ItemsSource = songs;
                currentSongs = songs;
                playlistSongHeadersScroller.Visibility = Visibility.Visible;
            }
        }

        private void playlistSongView_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            selectSong();
        }

        private void playlistSongScroller_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            ScrollViewer viewer = sender as ScrollViewer;

            if (viewer.VerticalOffset == viewer.ScrollableHeight)
            {
                numPages++;
                updatePlaylistSongs(playlistView.SelectedIndex, numPages);
            }

            playlistSongHeadersScroller.ChangeView(viewer.HorizontalOffset, 0, 1);
        }

        private void clearSongColors()
        {
            for (int i = 0; i < playlistSongView.Items.Count; i++)
            {
                ListViewItem container = playlistSongView.ContainerFromIndex(i) as ListViewItem;
                container.Background = null;
            }
        }

        private void selectSong()
        {
            api.playPlaylistItem(playlistView.SelectedIndex, playlistSongView.SelectedIndex);
            updatePlayerData();
        }

        private void playlistSongView_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if(e.Key == Windows.System.VirtualKey.Enter)
            {
                selectSong();
            }
        }

        void CoreWindow_KeyUp(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs e)
        {
            //Check if A button on a gamepad is pressed while a playlistSongView item is focused/selected
            if(playlistSongView.Items.Count > 0 && playlistSongView.SelectedIndex != -1)
            {
                bool playlistSongViewFocused = (playlistSongView.ContainerFromIndex(playlistSongView.SelectedIndex) as ListViewItem).FocusState == FocusState.Keyboard;
                if (playlistSongViewFocused && e.VirtualKey == Windows.System.VirtualKey.GamepadA)
                {
                    selectSong();
                }
            }
            
            if(e.VirtualKey == Windows.System.VirtualKey.GamepadX)
            {
                playPauseButton.Focus(FocusState.Programmatic);
            }
        }

        private void songTimeSlider_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            api.setPlaybackPosition((float)songTimeSlider.Value);
            sliderManipulateHold = false;
        }

        private void playPauseButton_Click(object sender, RoutedEventArgs e)
        {
            api.playPauseToggle();
            updatePlayerData();
        }

        private void previousButton_Click(object sender, RoutedEventArgs e)
        {
            api.playPrevious();
            updatePlayerData();
        }

        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            api.playNext();
            updatePlayerData();
        }

        private void randomNextButton_Click(object sender, RoutedEventArgs e)
        {
            api.playRandom();
            updatePlayerData();
        }

        private void stopButton_Click(object sender, RoutedEventArgs e)
        {
            api.stopPlayer();
            updatePlayerData();
        }

        private void songTimeSlider_Tapped(object sender, TappedRoutedEventArgs e)
        {
            api.setPlaybackPosition((float)songTimeSlider.Value);
            sliderManipulateHold = false;
        }

        private void volumeSlider_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            VolumeCurve curve = new VolumeCurve(true, VolumeCurveType.Better);
            api.setVolume((float)curve.calcVolumeCurve(volumeSlider.Value));
            sliderManipulateHold = false;
        }

        private void volumeSlider_Tapped(object sender, TappedRoutedEventArgs e)
        {
            VolumeCurve curve = new VolumeCurve(true, VolumeCurveType.Better);
            api.setVolume((float)curve.calcVolumeCurve(volumeSlider.Value));
            sliderManipulateHold = false;
        }

        private void muteButton_Click(object sender, RoutedEventArgs e)
        {
            api.toggleMute(playerData);
            updatePlayerData();
        }

        private void playbackModeButton_Click(object sender, RoutedEventArgs e)
        {
            if (!playbackModePopup.IsOpen)
            {
                playbackModeView.ItemsSource = playerData.PlayBackModes;
                playbackModeView.SelectedIndex = playerData.PlaybackMode;
                playbackModePopup.IsOpen = true;
            }
        }

        private void playbackModeView_ItemClick(object sender, ItemClickEventArgs e)
        {
            api.setPlaybackMode(playbackModeView.Items.IndexOf(e.ClickedItem));
            playbackModeView.ItemsSource = null;
            playbackModePopup.IsOpen = false;

            updatePlayerData();
        }

        private void songTimeSlider_PreviewKeyUp(object sender, KeyRoutedEventArgs e)
        {
            api.setPlaybackPosition((float)songTimeSlider.Value);
            sliderManipulateHold = false;
        }

        private void volumeSlider_PreviewKeyUp(object sender, KeyRoutedEventArgs e)
        {
            VolumeCurve curve = new VolumeCurve(true, VolumeCurveType.Better);
            api.setVolume((float)curve.calcVolumeCurve(volumeSlider.Value));
            sliderManipulateHold = false;
        }

        private void songTimeSlider_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            sliderManipulateHold = true;
        }

        private void volumeSlider_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            sliderManipulateHold = true;
        }

        private void songTimeSlider_PreviewKeyDown(object sender, KeyRoutedEventArgs e)
        {
            sliderManipulateHold = true;
        }

        private void volumeSlider_PreviewKeyDown(object sender, KeyRoutedEventArgs e)
        {
            sliderManipulateHold = true;
        }

        private void settingsButton_Click(object sender, RoutedEventArgs e)
        {
            if (mainPage != null)
            {
                mainPage.openSettings();
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            mainPage = e.Parameter as MainPage;

            if(mainPage != null)
            {
                settingsButton.Visibility = Visibility.Visible;
            }

            base.OnNavigatedTo(e);
        }
    }
}
