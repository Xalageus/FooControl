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
using Windows.UI.Core;

namespace FooControl
{
    public sealed partial class MainPage : Page
    {
        private SystemNavigationManager currentView;

        public MainPage()
        {
            this.InitializeComponent();

            currentView = SystemNavigationManager.GetForCurrentView();
            rootFrame.Navigate(typeof(MainFrame), this);
        }

        public void openSettings()
        {
            rootFrame.Navigate(typeof(SettingsPage), this);

            RestoreNavigationButton();
        }

        private void MainPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (rootFrame.CanGoBack)
            {
                NavigationGoBack();

                e.Handled = true;
            }
        }

        public void NavigationGoBack()
        {
            if (rootFrame.CanGoBack)
            {
                rootFrame.GoBack();

                currentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
                SystemNavigationManager.GetForCurrentView().BackRequested -= MainPage_BackRequested;
            }
        }

        public void RestoreNavigationButton()
        {
            SystemNavigationManager.GetForCurrentView().BackRequested += MainPage_BackRequested;
            currentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
        }
    }
}
