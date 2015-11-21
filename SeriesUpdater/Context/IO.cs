using System;
using System.Collections.Generic;
using System.IO;

namespace SeriesUpdater.Context
{
    class IO
    {
        public static void ReadSeries()
        {
            if (!File.Exists(MainProgram.Variables.SeriesDataFileName)) return;

            using (StreamReader dataReader = new StreamReader(MainProgram.Variables.SeriesDataFileName))
            {
                int whileCount = 0;
                while (dataReader.Peek() > -1)
                {
                    string[] currRow = dataReader.ReadLine().Split(';');
                    if (currRow.Length <= 1) continue;

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
                    MainProgram.Variables.SeriesList.Add(currSeries);

                    whileCount++;
                }
            }
        }

        public static void WriteSeries()
        {
            using (StreamWriter dataWriter = new StreamWriter(MainProgram.Variables.SeriesDataFileName, false))
            {
                foreach (Series currSeries in MainProgram.Variables.SeriesList)
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

        public static void WriteSeries(string name, string imdbId)
        {
            using (StreamWriter dataWriter = new StreamWriter(MainProgram.Variables.SeriesDataFileName, true))
            {
                List<Series> seriesList = MainProgram.Variables.SeriesList;
                Series lastSeries = seriesList[seriesList.Count - 1];

                string lastViewed = lastSeries.LastViewed.ToString();
                string lastEpisode = lastSeries.LastEpisode.ToString();
                string nextEpisode = lastSeries.NextEpisode.ToString();

                string nextAirDate = lastSeries.NextEpisodeAirDate.ToString();
                int dateKnown = lastSeries.DateKnown;
                int notificationSent = lastSeries.NotificationSent;

                dataWriter.WriteLine("{0};{1};{2};{3};{4};{5};{6};{7}", name, imdbId, lastViewed, lastEpisode,
                    nextEpisode, nextAirDate, dateKnown, notificationSent);
            }
        }

        public static void ReadSettings()
        {
            if (!File.Exists(MainProgram.Variables.SettingsFileName)) return;

            using (StreamReader settingsReader = new StreamReader(MainProgram.Variables.SettingsFileName))
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

        public static void WriteSettings()
        {
            using (StreamWriter settingsWriter = new StreamWriter(MainProgram.Variables.SettingsFileName, false))
            {
                foreach (string[] option in Settings.GlobalSettings)
                {
                    settingsWriter.WriteLine(option[0] + "=" + option[1]);
                }
            }
        }

        public static void WriteSettings(string optionName, string optionValue)
        {
            using (StreamWriter settingsWriter = new StreamWriter(MainProgram.Variables.SettingsFileName, false))
            {
                settingsWriter.WriteLine(optionName + "=" + optionValue);
            }
        }
    }
}
