using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace FooControl.Settings
{
    class MainSettingsController
    {
        SettingsStore settingsStore;
        SettingsStore heldSettingsStore;
        SettingsFileController fileController;

        private MainSettingsController(SettingsFileController fileController, SettingsStore settingsStore)
        {
            this.fileController = fileController;
            this.settingsStore = settingsStore;
            heldSettingsStore = settingsStore;
        }

        public static async Task<MainSettingsController> CreateAsync()
        {
            SettingsFileController fileController = new SettingsFileController();
            SettingsStore store = await LoadSettings(fileController);

            return new MainSettingsController(fileController, store);
        }

        private static async Task<SettingsStore> LoadSettings(SettingsFileController fc)
        {
            if (await fc.CheckIfSettingsFileExistsAsync())
            {
                return await fc.LoadSettingsFile();
            }
            else
            {
                return await fc.CreateSettingsFile();
            }
        }

        private async Task<SettingsStore> LoadSettings()
        {
            if (await fileController.CheckIfSettingsFileExistsAsync())
            {
                return await fileController.LoadSettingsFile();
            }
            else
            {
                return await fileController.CreateSettingsFile();
            }
        }

        private async Task SaveSettings()
        {
            await fileController.SaveSettingsFile(settingsStore);
        }

        public SettingsPageParams GetSettingsParams(Type page)
        {
            SettingsPageParams settingsPageParams = new SettingsPageParams();

            if(page == typeof(GeneralSettings))
            {
                settingsPageParams.store = heldSettingsStore.generalSettings;
                settingsPageParams.showSaveButton = true;
            }else if(page == typeof(MediaServerSettings))
            {
                settingsPageParams.store = heldSettingsStore.serverSettings;
                settingsPageParams.showSaveButton = true;
            }else if(page == typeof(ColumnsSettings))
            {
                settingsPageParams.store = heldSettingsStore.columnsSettings;
                settingsPageParams.showSaveButton = true;
            }else if(page == typeof(AboutPage))
            {
                settingsPageParams.store = new SpecificStore();
                settingsPageParams.showSaveButton = false;
            }
            else
            {
                settingsPageParams.store = new SpecificStore();
                settingsPageParams.showSaveButton = false;
            }

            return settingsPageParams;
        }

        public void HoldSettings(SpecificStore store)
        {
            switch (store)
            {
                case GeneralSettingsStore t1:
                    heldSettingsStore.generalSettings = (GeneralSettingsStore)store;
                    break;
                case ServerSettingsStore t2:
                    heldSettingsStore.serverSettings = (ServerSettingsStore)store;
                    break;
                case ColumnSettingsStore t3:
                    heldSettingsStore.columnsSettings = (ColumnSettingsStore)store;
                    break;
                default:
                    break;
            }
        }

        public void AskSendSettings(Page page, Type pageType)
        {
            if (pageType == typeof(GeneralSettings))
            {
                ((GeneralSettings)page).SendSettings();
            }
            else if (pageType == typeof(MediaServerSettings))
            {
                ((MediaServerSettings)page).SendSettings();
            }
            else if (pageType == typeof(ColumnsSettings))
            {
                ((ColumnsSettings)page).SendSettings();
            }
        }

        public async Task UpdateSettings()
        {
            settingsStore = heldSettingsStore;
            await SaveSettings();
        }
    }

    public class PageParameter
    {
        public SettingsPage settingsPage { get; set; }
        public SettingsWidget settingsWidget { get; set; }
        public SpecificStore store { get; set; }
    }

    public class SettingsPageParams
    {
        public SpecificStore store { get; set; }
        public bool showSaveButton { get; set; }
    }
}
