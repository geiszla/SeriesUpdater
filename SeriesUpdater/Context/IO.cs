using System;
using System.IO;
using System.Windows.Forms;

namespace SeriesUpdater.Context
{
    class IO
    {
        public static void readSeries()
        {
            if (File.Exists(MainProgram.Variables.dataPath + @"\series.dat"))
            {
                StreamReader readData = new StreamReader(MainProgram.Variables.dataPath + @"\series.dat");

                int whileCount = 0;
                while (readData.Peek() > -1)
                {
                    string[] currRow = readData.ReadLine().Split(';');

                    if (currRow.Length > 1)
                    {
                        int id = whileCount;
                        string name = currRow[0];
                        string imdbId = currRow[1];
                        int[] lastViewed = MainProgram.ProcessData.convertEpisodeString(currRow[2]);
                        int[] lastEpisode = MainProgram.ProcessData.convertEpisodeString(currRow[3]);
                        int[] nextEpisode = MainProgram.ProcessData.convertEpisodeString(currRow[4]);
                        DateTime nextEpisodeAirDate = Convert.ToDateTime(currRow[5]);
                        int dateKnown = Convert.ToInt32(currRow[6]);
                        int notificationSent = Convert.ToInt32(currRow[7]);
                        Series currSeries = new Series(id, name, imdbId, lastViewed, lastEpisode, nextEpisode, nextEpisodeAirDate, dateKnown, notificationSent);
                        MainProgram.Variables.seriesList.Add(currSeries);
                    }

                    whileCount++;
                }

                readData.Close();
            }
        }

        public static void writeSeries()
        {
            StreamWriter writeData = new StreamWriter(MainProgram.Variables.dataPath + @"\series.dat", false);

            for (int i = 0; i < MainProgram.Variables.seriesList.Count; i++)
            {
                string name = MainProgram.Variables.seriesList[i].name;
                string imdbId = MainProgram.Variables.seriesList[i].imdbId;
                string lastViewed = "S" + MainProgram.Variables.seriesList[i].lastViewed[0] + "E" + MainProgram.Variables.seriesList[i].lastViewed[1];
                string lastEpisode = "S" + MainProgram.Variables.seriesList[i].lastEpisode[0] + "E" + MainProgram.Variables.seriesList[i].lastEpisode[1];
                string nextEpisode = "S" + MainProgram.Variables.seriesList[i].nextEpisode[0] + "E" + MainProgram.Variables.seriesList[i].nextEpisode[1];
                DateTime nextAirDate = MainProgram.Variables.seriesList[i].nextEpisodeAirDate;
                int dateKnown = MainProgram.Variables.seriesList[i].dateKnown;
                int notificationSent = MainProgram.Variables.seriesList[i].notificationSent;

                writeData.WriteLine("{0};{1};{2};{3};{4};{5};{6};{7}", name, imdbId, lastViewed, lastEpisode, nextEpisode, nextAirDate, dateKnown, notificationSent);
            }

            writeData.Close();
        }

        public static void writeSeries(string name, string imdbId)
        {
            StreamWriter writeData = new StreamWriter(MainProgram.Variables.dataPath + @"\series.dat", true);

            string lastViewed = "S" + MainProgram.Variables.seriesList[MainProgram.Variables.seriesList.Count - 1].lastViewed[0] + "E" + MainProgram.Variables.seriesList[MainProgram.Variables.seriesList.Count - 1].lastViewed[1];
            string lastEpisode = "S" + MainProgram.Variables.seriesList[MainProgram.Variables.seriesList.Count - 1].lastEpisode[0] + "E" + MainProgram.Variables.seriesList[MainProgram.Variables.seriesList.Count - 1].lastEpisode[1];
            string nextEpisode = "S" + MainProgram.Variables.seriesList[MainProgram.Variables.seriesList.Count - 1].nextEpisode[0] + "E" + MainProgram.Variables.seriesList[MainProgram.Variables.seriesList.Count - 1].nextEpisode[1];
            
            string nextAirDate = MainProgram.Variables.seriesList[MainProgram.Variables.seriesList.Count - 1].nextEpisodeAirDate.ToString();
            int dateKnown = MainProgram.Variables.seriesList[MainProgram.Variables.seriesList.Count - 1].dateKnown;
            int notificationSent = MainProgram.Variables.seriesList[MainProgram.Variables.seriesList.Count - 1].notificationSent;

            writeData.WriteLine("{0};{1};{2};{3};{4};{5};{6};{7}", name, imdbId, lastViewed, lastEpisode, nextEpisode, nextAirDate, dateKnown, notificationSent);
            writeData.Close();
        }

        public static void readSettings()
        {
            if (File.Exists(MainProgram.Variables.dataPath + @"\settings.dat"))
            {
                StreamReader readSettings = new StreamReader(MainProgram.Variables.dataPath + @"\settings.dat");

                while (readSettings.Peek() > -1)
                {
                    string[] currRow = readSettings.ReadLine().Split('=');

                    foreach (string[] option in Context.Settings.settings)
                    {
                        if (currRow[0] == option[0])
                        {
                            option[1] = currRow[1];
                        }
                    }
                }

                readSettings.Close();
            }
        }

        public static void writeSettings()
        {
            StreamWriter writeSettings = new StreamWriter(MainProgram.Variables.dataPath + @"\settings.dat", false);

            foreach (string[] option in Context.Settings.settings)
            {
                writeSettings.WriteLine(option[0] + "=" + option[1]);
            }

            writeSettings.Close();
        }

        public static void writeSettings(string optionName, string optionValue)
        {
            StreamWriter writeSettings = new StreamWriter(MainProgram.Variables.dataPath + @"\settings.dat", false);
            writeSettings.WriteLine(optionName + "=" + optionValue);
            writeSettings.Close();
        }
    }
}
