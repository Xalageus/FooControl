using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace FooControl.Settings
{
    public sealed partial class MediaServerSettings : Page
    {
        private SettingsPage settingsPage;
        private SettingsWidget settingsWidget;
        private ServerSettingsStore store;
        private ObservableCollection<ServerLoginSettings> loginSettings;

        public MediaServerSettings()
        {
            this.InitializeComponent();
        }

        private async void newServerButton_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog();
            dialog.Title = "New media server";
            dialog.PrimaryButtonText = "OK";
            dialog.CloseButtonText = "Cancel";
            dialog.DefaultButton = ContentDialogButton.Primary;
            dialog.Content = new NewServerPage();
            ContentDialogResult result = await dialog.ShowAsync();

            if(result == ContentDialogResult.Primary)
            {
                //serverListView.Items.Add((dialog.Content as NewServerPage).getHostData());
                loginSettings.Add((dialog.Content as NewServerPage).getHostData());

                SendModified();
            }
        }

        private void deleteServerButton_Click(object sender, RoutedEventArgs e)
        {

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

            store = (ServerSettingsStore)param.store;
            loginSettings = new ObservableCollection<ServerLoginSettings>(store.logins);

            //Set options on controls
            serverListView.ItemsSource = loginSettings;

            base.OnNavigatedTo(e);
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
            store.logins = loginSettings.ToList();

            if (settingsPage == null)
            {
                settingsWidget.ReceiveSettings(store);
            }
            else
            {
                settingsPage.ReceiveSettings(store);
            }
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            SendSettings();

            base.OnNavigatingFrom(e);
        }
    }
}
