using System;
using System.Collections.Generic;
using System.IO;

namespace SeriesUpdater.Context
{
    class IO
    {
        // Series
        public static void ReadSeries()
        {
            if (!File.Exists(Internal.Variables.SeriesDataFileName)) return;

            using (StreamReader dataReader = new StreamReader(Internal.Variables.SeriesDataFileName))
            {
                int whileCount = 0;
                while (dataReader.Peek() > -1)
                {
                    string[] currRow = dataReader.ReadLine().Split(';');
                    if (currRow.Length < 2) continue;

                    int id = whileCount;
                    string name = currRow[0];
                    string imdbId = currRow[1];
                    Episode lastViewed = new Episode(currRow[2]);
                    Episode lastEpisode = new Episode(currRow[3]);
                    Episode nextEpisode = new Episode(currRow[4]);
                    DateTime nextEpisodeAirDate = Convert.ToDateTime(currRow[5]);
                    int dateKnown = Convert.ToInt32(currRow[6]);
                    int notificationSent = Convert.ToInt32(currRow[7]);

                    Series currSeries = new Series(id, name, imdbId, lastViewed, lastEpisode,
                        nextEpisode, nextEpisodeAirDate, dateKnown, notificationSent);
                    Internal.Variables.SeriesList.Add(currSeries);

                    whileCount++;
                }
            }
        }

        public static void WriteSeries(string Name = null, string ImdbId = null)
        {
            initializeWrite();

            bool IsAppend = ImdbId != null;
            List<Series> seriesList = Internal.Variables.SeriesList;
            List<Series> selectedSeries = IsAppend ? new List<Series> { seriesList[seriesList.Count - 1] } : seriesList;

            using (StreamWriter dataWriter = new StreamWriter(Internal.Variables.SeriesDataFileName, IsAppend))
            {
                foreach (Series currSeries in selectedSeries)
                {
                    string name = currSeries.Name;
                    string imdbId = currSeries.ImdbId;
                    string lastViewed = currSeries.LastViewed.ToString();
                    string lastEpisode = currSeries.LastEpisode.ToString();
                    string nextEpisode = currSeries.NextEpisode.ToString();
                    DateTime nextAirDate = currSeries.NextEpisodeAirDate;
                    int dateKnown = currSeries.DateKnown;
                    int notificationSent = currSeries.NotificationSent;

                    dataWriter.WriteLine("{0};{1};{2};{3};{4};{5};{6};{7}", name, imdbId, lastViewed, lastEpisode,
                        nextEpisode, nextAirDate, dateKnown, notificationSent);
                }
            }
        }

        // Settings
        public static void ReadSettings()
        {
            if (!File.Exists(Internal.Variables.SettingsFileName)) return;

            using (StreamReader settingsReader = new StreamReader(Internal.Variables.SettingsFileName))
            {
                while (settingsReader.Peek() > -1)
                {
                    string[] currRow = settingsReader.ReadLine().Split('=');
                    foreach (string[] option in Settings.GlobalSettings)
                    {
                        if (currRow[0] == option[0]) option[1] = currRow[1];
                    }
                }
            }
        }

        public static void WriteSettings(string Name = null, string Value = null)
        {
            initializeWrite();

            bool IsAppend = Value == null && Name == null;
            List<string[]> settingsList = IsAppend ? new List<string[]>() { new string[] { Name, Value } }
                : Settings.GlobalSettings;

            using (StreamWriter settingsWriter = new StreamWriter(Internal.Variables.SettingsFileName, IsAppend))
            {
                foreach (string[] option in settingsList)
                {
                    settingsWriter.WriteLine(option[0] + "=" + option[1]);
                }
            }
        }

        // Helper
        static void initializeWrite()
        {
            string dataFolderPath = Internal.Variables.DataFolderPath;
            if (!Directory.Exists(dataFolderPath))
            {
                Directory.CreateDirectory(dataFolderPath);
            }
        }
    }
}
