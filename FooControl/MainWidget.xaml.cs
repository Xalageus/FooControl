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
using Microsoft.Gaming.XboxGameBar;

namespace FooControl
{
    public sealed partial class MainWidget : Page
    {
        private XboxGameBarWidget widget = null;

        public MainWidget()
        {
            this.InitializeComponent();

            rootFrame.Navigate(typeof(MainFrame));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // In our example we pass the XboxGameBarWidget through the Navigate event when creating and showing the parent widget for the first time, your implementation may differ.

            // Here we store the parameter in a member variable "widget":
            widget = e.Parameter as XboxGameBarWidget;

            // Hook up the settings clicked event
            widget.SettingsClicked += Widget_SettingsClicked;
        }

        private async void Widget_SettingsClicked(XboxGameBarWidget sender, object args)
        {
            // if necessary pre-configure any required data needed by the settings widget prior to activation
            // ...

            await widget.ActivateSettingsAsync();
        }
    }
}
