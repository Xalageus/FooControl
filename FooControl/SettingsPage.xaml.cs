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
    public sealed partial class SettingsPage : Page
    {
        private MainSettingsController controller;
        private MainPage mainPage = null;

        public SettingsPage()
        {
            this.InitializeComponent();

            pagesView.ItemsSource = SettingsPages.Pages.Keys;
        }

        private void pagesView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PageParameter param = new PageParameter();
            param.settingsPage = this;

            SettingsPageParams metaSettings = controller.GetSettingsParams(SettingsPages.Pages[pagesView.SelectedItem as string]);
            param.store = metaSettings.store;

            ShowHideSaveButton(metaSettings.showSaveButton);

            settingsFrame.Navigate(SettingsPages.Pages[pagesView.SelectedItem as string], param);
        }

        private void ShowHideSaveButton(bool show)
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

        public void SetModified()
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

        protected async override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            if (saveButton.IsEnabled)
            {
                e.Cancel = true;
                base.OnNavigatingFrom(e);

                ContentDialog dialog = new ContentDialog();
                dialog.Title = "Save settings?";
                dialog.PrimaryButtonText = "Save";
                dialog.SecondaryButtonText = "Don't save";
                dialog.CloseButtonText = "Cancel";
                dialog.DefaultButton = ContentDialogButton.Primary;
                ContentDialogResult result = await dialog.ShowAsync();

                switch (result)
                {
                    case ContentDialogResult.None:
                        mainPage.RestoreNavigationButton();
                        break;
                    case ContentDialogResult.Primary:
                        await saveSettings();
                        mainPage.NavigationGoBack();
                        break;
                    case ContentDialogResult.Secondary:
                        saveButton.IsEnabled = false;
                        mainPage.NavigationGoBack();
                        break;
                    default:
                        mainPage.RestoreNavigationButton();
                        break;
                }
            }
            else
            {
                base.OnNavigatingFrom(e);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            mainPage = e.Parameter as MainPage;

            base.OnNavigatedTo(e);
        }
    }
}
