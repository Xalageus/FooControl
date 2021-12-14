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

namespace FooControl.Settings
{
    public sealed partial class NewServerPage : Page
    {
        public NewServerPage()
        {
            this.InitializeComponent();
        }

        public ServerLoginSettings getHostData()
        {
            if(usernameTB.Text == "")
            {
                return new ServerLoginSettings(ipAddressTB.Text, portTB.Text);
            }
            else
            {
                return new ServerLoginSettings(ipAddressTB.Text, portTB.Text, usernameTB.Text, passwordTB.Text);
            }
        }
    }
}
