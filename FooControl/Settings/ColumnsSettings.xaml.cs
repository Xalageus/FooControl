using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.ApplicationModel.DataTransfer;
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
    public sealed partial class ColumnsSettings : Page
    {
        private ObservableCollection<string> allFields;
        private ObservableCollection<string> visibleFields;
        private SettingsPage settingsPage;
        private SettingsWidget settingsWidget;
        private ColumnSettingsStore store;
        private bool visibleReordering;

        public ColumnsSettings()
        {
            this.InitializeComponent();

            FoobarFieldsDicts dicts = new FoobarFieldsDicts();
            allFields = new ObservableCollection<string>();
            visibleFields = new ObservableCollection<string>();

            foreach(KeyValuePair<string, string> member in dicts.genericFields)
            {
                allFields.Add(member.Key);
            }

            allFieldsView.ItemsSource = allFields;
            visibleFieldsView.ItemsSource = visibleFields;
        }

        private async void allFieldsView_Drop(object sender, DragEventArgs e)
        {
            if (e.DataView.Contains(CustomDataFormats.VisibleColumn))
            {
                DragOperationDeferral def = e.GetDeferral();
                visibleReordering = false;
                string text = await e.DataView.GetDataAsync(CustomDataFormats.VisibleColumn) as string;
                string[] items = text.Split('\n');

                Point position = e.GetPosition(sender as ListView);

                foreach (string item in items)
                {
                    if (item.EndsWith('\r'))
                    {
                        allFields.Add(item.Remove(item.Length - 1));
                    }
                    else
                    {
                        allFields.Add(item);
                    }
                }

                e.AcceptedOperation = DataPackageOperation.Move;
                def.Complete();
            }
        }

        /// <summary>
        /// Reciving items from available columns to add to current columns
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void visibleFieldsView_Drop(object sender, DragEventArgs e)
        {
            if (e.DataView.Contains(CustomDataFormats.AvailableColumn))
            {
                DragOperationDeferral def = e.GetDeferral();
                //string text = await e.DataView.GetTextAsync();
                string text = await e.DataView.GetDataAsync(CustomDataFormats.AvailableColumn) as string;
                string[] items = text.Split('\n');

                Point position = e.GetPosition(sender as ListView);

                foreach(string item in items)
                {
                    if (item.EndsWith('\r'))
                    {
                        visibleFields.Add(item.Remove(item.Length - 1));
                    }
                    else
                    {
                        visibleFields.Add(item);
                    }
                }

                e.AcceptedOperation = DataPackageOperation.Move;
                def.Complete();
            }
        }

        /// <summary>
        /// Preparing items to drag from available columns to current columns.
        /// Data is sent as an IList(string)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void allFieldsView_DragItemsStarting(object sender, DragItemsStartingEventArgs e)
        {
            StringBuilder items = new StringBuilder();

            foreach(var item in e.Items)
            {
                if(items.Length > 0)
                {
                    items.AppendLine();
                }

                items.Append(item as string);
            }

            //e.Data.SetText(items.ToString());
            e.Data.SetData(CustomDataFormats.AvailableColumn, items.ToString());
            e.Data.RequestedOperation = DataPackageOperation.Move;
        }

        /// <summary>
        /// After sending items to current columns, remove from available columns
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void allFieldsView_DragItemsCompleted(ListViewBase sender, DragItemsCompletedEventArgs args)
        {
            if(args.DropResult == DataPackageOperation.Move)
            {
                foreach (string item in args.Items)
                {
                    allFields.Remove(item);
                }

                SendModified();
            }
        }

        private void allFieldsView_DragEnter(object sender, DragEventArgs e)
        {
            e.DragUIOverride.Caption = "Hide";
            e.AcceptedOperation = e.DataView.Contains(CustomDataFormats.VisibleColumn) ? DataPackageOperation.Move : DataPackageOperation.None;
        }

        private void visibleFieldsView_DragItemsStarting(object sender, DragItemsStartingEventArgs e)
        {
            StringBuilder items = new StringBuilder();

            foreach (var item in e.Items)
            {
                if (items.Length > 0)
                {
                    items.AppendLine();
                }

                items.Append(item as string);
            }

            visibleReordering = true;
            e.Data.SetData(CustomDataFormats.VisibleColumn, items.ToString());
            e.Data.RequestedOperation = DataPackageOperation.Move;
        }

        private void visibleFieldsView_DragItemsCompleted(ListViewBase sender, DragItemsCompletedEventArgs args)
        {
            if (visibleReordering)
            {
                SendModified();
            }
            else if (args.DropResult == DataPackageOperation.Move)
            {
                foreach (string item in args.Items)
                {
                    visibleFields.Remove(item);
                }

                visibleReordering = true;
                SendModified();
            }
        }

        /// <summary>
        /// Check items that might drop onto current columns
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void visibleFieldsView_DragEnter(object sender, DragEventArgs e)
        {
            e.DragUIOverride.Caption = "Show";
            e.AcceptedOperation = e.DataView.Contains(CustomDataFormats.AvailableColumn) ? DataPackageOperation.Move : DataPackageOperation.None;
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

            store = (ColumnSettingsStore)param.store;
            UpdateLists();

            base.OnNavigatedTo(e);
        }

        private void UpdateLists()
        {
            visibleFields = new ObservableCollection<string>(store.columns);
            visibleFieldsView.ItemsSource = visibleFields;

            foreach(string field in visibleFields)
            {
                allFields.Remove(field);
            }
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            SendSettings();

            base.OnNavigatingFrom(e);
        }

        private void SendModified()
        {
            if(settingsPage == null)
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
            store.columns = visibleFields.ToList<string>();

            if (settingsPage == null)
            {
                settingsWidget.ReceiveSettings(store);
            }
            else
            {
                settingsPage.ReceiveSettings(store);
            }
        }

        private void allFieldsView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(allFieldsView.SelectedItems.Count > 0)
            {
                buttonShowColumn.IsEnabled = true;
            }
            else
            {
                buttonShowColumn.IsEnabled = false;
            }
        }

        private void visibleFieldsView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (visibleFieldsView.SelectedItems.Count > 0)
            {
                buttonHideColumn.IsEnabled = true;
            }
            else
            {
                buttonHideColumn.IsEnabled = false;
            }
        }

        private void buttonShowColumn_Click(object sender, RoutedEventArgs e)
        {
            foreach(string col in allFieldsView.SelectedItems)
            {
                visibleFields.Add(col);
                allFields.Remove(col);
            }

            SendModified();
        }

        private void buttonHideColumn_Click(object sender, RoutedEventArgs e)
        {
            foreach (string col in visibleFieldsView.SelectedItems)
            {
                allFields.Add(col);
                visibleFields.Remove(col);
            }

            SendModified();
        }
    }
}
