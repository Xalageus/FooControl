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
using FooControl.Settings;
using System.Threading.Tasks;

namespace FooControl
{
    public sealed partial class SettingsWidget : Page
    {
        private MainSettingsController controller;

        public SettingsWidget()
        {
            this.InitializeComponent();

            pagesView.ItemsSource = SettingsPages.Pages.Keys;
        }

        private async void pagesView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (controller == null)
            {
                controller = await MainSettingsController.CreateAsync();
            }

            PageParameter param = new PageParameter();
            param.settingsWidget = this;

            SettingsPageParams metaSettings = controller.GetSettingsParams(SettingsPages.Pages[pagesView.SelectedItem as string]);
            param.store = metaSettings.store;

            showHideSaveButton(metaSettings.showSaveButton);

            settingsFrame.Navigate(SettingsPages.Pages[pagesView.SelectedItem as string], param);
        }

        private void showHideSaveButton(bool show)
        {
            if (show)
            {
                saveButton.Visibility = Visibility.Visible;
            }
            else
            {
                saveButton.Visibility = Visibility.Collapsed;
            }
        }

        public void setModified()
        {
            saveButton.IsEnabled = true;
        }

        public void ReceiveSettings(SpecificStore store)
        {
            controller.HoldSettings(store);
        }

        private async void saveButton_Click(object sender, RoutedEventArgs e)
        {
            await saveSettings();
        }

        private async Task saveSettings()
        {
            controller.AskSendSettings(settingsFrame.Content as Page, SettingsPages.Pages[pagesView.SelectedItem as string]);
            await controller.UpdateSettings();
            saveButton.IsEnabled = false;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.controller = await MainSettingsController.CreateAsync();
            pagesView.SelectedItem = pagesView.Items[0];
        }
    }
}
