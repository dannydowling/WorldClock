// File: Services/DataService.cs - For saving and loading data
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;


namespace WorldClock
{
    public class DataService
    {
        private const string DATA_FILE = "worldclocks.xml";

        public void SaveData(TimeZoneService timeZoneService, EventService eventService)
        {
            try
            {
                var appData = new ApplicationData();

                // Save time zones
                foreach (var timeZone in timeZoneService.GetAllTimeZones())
                {
                    appData.TimeZones.Add(new SerializableTimeZone(timeZone));
                }

                // Save events
                foreach (var scheduledEvent in eventService.GetAllEvents())
                {
                    appData.Events.Add(new SerializableEvent(scheduledEvent));
                }

                // Serialize and save to file
                XmlSerializer serializer = new XmlSerializer(typeof(ApplicationData));
                using (TextWriter writer = new StreamWriter(GetDataFilePath()))
                {
                    serializer.Serialize(writer, appData);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving data: {ex.Message}", "Save Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public bool LoadData(TimeZoneService timeZoneService, EventService eventService)
        {
            string filePath = GetDataFilePath();
            if (!File.Exists(filePath))
                return false;

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ApplicationData));
                using (TextReader reader = new StreamReader(filePath))
                {
                    var appData = (ApplicationData)serializer.Deserialize(reader);

                    // Clear existing data
                    timeZoneService.ClearTimeZones();
                    eventService.ClearEvents();

                    // Load time zones
                    foreach (var serializableTimeZone in appData.TimeZones)
                    {
                        timeZoneService.AddTimeZoneModel(serializableTimeZone.ToTimeZoneModel());
                    }

                    // Load events
                    foreach (var serializableEvent in appData.Events)
                    {
                        eventService.AddEvent(serializableEvent.ToScheduledEvent());
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Load Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private string GetDataFilePath()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string appFolder = Path.Combine(appDataPath, "WorldClocks");

            // Create directory if it doesn't exist
            if (!Directory.Exists(appFolder))
                Directory.CreateDirectory(appFolder);

            return Path.Combine(appFolder, DATA_FILE);
        }
    }
}