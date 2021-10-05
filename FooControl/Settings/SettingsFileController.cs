using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace FooControl.Settings
{
    class SettingsFileController
    {
        private static string settingsFilename = "settings.xml";

        public SettingsFileController()
        {

        }

        public async Task<bool> CheckIfSettingsFileExistsAsync()
        {
            try
            {
                StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync(settingsFilename);
            }
            catch (FileNotFoundException)
            {
                return false;
            }

            return true;
        }

        public async Task<SettingsStore> LoadSettingsFile()
        {
            StorageFile file;

            try
            {
                file = await ApplicationData.Current.LocalFolder.GetFileAsync(settingsFilename);
            }
            catch (FileNotFoundException)
            {
                return null;
            }

            string rawXml = await FileIO.ReadTextAsync(file);
            TextReader reader = new StringReader(rawXml);
            return await ParseSettingsFile(reader);
        }

        public async Task<bool> SaveSettingsFile(SettingsStore store)
        {
            StorageFile file;

            try
            {
                file = await ApplicationData.Current.LocalFolder.GetFileAsync(settingsFilename);
            }
            catch (FileNotFoundException)
            {
                return false;
            }

            XmlDocument doc = WriteSettingsDocument(store);

            await FileIO.WriteTextAsync(file, doc.OuterXml);
            return true;
        }

        public async Task<SettingsStore> CreateSettingsFile()
        {
            StorageFile file;

            try
            {
                file = await ApplicationData.Current.LocalFolder.CreateFileAsync(settingsFilename);
            }
            catch (FileNotFoundException)
            {
                return null;
            }

            SettingsStore store = new DefaultSettings();
            XmlDocument doc = WriteSettingsDocument(store);

            await FileIO.WriteTextAsync(file, doc.OuterXml);
            return store;
        }

        private async Task<SettingsStore> ParseSettingsFile(TextReader data)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SettingsStore));
            SettingsStore store;
            try
            {
                store = serializer.Deserialize(data) as SettingsStore;
            }
            catch (Exception)
            {
                //File is corrupted
                //TODO: Report this to user
                await DeleteSettingsFile();
                return await CreateSettingsFile();
            }

            return store;
        }

        private async Task DeleteSettingsFile()
        {
            StorageFile file;

            try
            {
                file = await ApplicationData.Current.LocalFolder.GetFileAsync(settingsFilename);
                await file.DeleteAsync();
            }
            catch
            {
                
            }
        }

        private XmlDocument WriteSettingsDocument(SettingsStore store)
        {
            TextWriter data = new StringWriter();
            XmlSerializer serializer = new XmlSerializer(typeof(SettingsStore));
            serializer.Serialize(data, store);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(data.ToString());

            return xmlDoc;
        }
    }
}
