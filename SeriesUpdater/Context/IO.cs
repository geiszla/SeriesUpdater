using System;
using System.IO;

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
                        Series currSeries = new Series(id, name, imdbId, lastViewed, lastEpisode, nextEpisode, nextEpisodeAirDate);
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
                string lastViewed = "S" + MainProgram.Variables.seriesList[MainProgram.Variables.seriesList.Count - 1].lastViewed[0] + "E" + MainProgram.Variables.seriesList[MainProgram.Variables.seriesList.Count - 1].lastViewed[1];
                string lastEpisode = "S" + MainProgram.Variables.seriesList[MainProgram.Variables.seriesList.Count - 1].lastEpisode[0] + "E" + MainProgram.Variables.seriesList[MainProgram.Variables.seriesList.Count - 1].lastEpisode[1];
                string nextEpisode = "S" + MainProgram.Variables.seriesList[MainProgram.Variables.seriesList.Count - 1].nextEpisode[0] + "E" + MainProgram.Variables.seriesList[MainProgram.Variables.seriesList.Count - 1].nextEpisode[1];
                DateTime nextAirDate = MainProgram.Variables.seriesList[i].nextEpisodeAirDate;

                writeData.WriteLine("{0};{1};{2};{3};{4};{5}", name, imdbId, lastViewed, lastEpisode, nextEpisode, nextAirDate);
            }

            writeData.Close();
        }

        public static void writeSeries(string name, string imdbId)
        {
            StreamWriter writeData = new StreamWriter(MainProgram.Variables.dataPath + @"\series.dat", true);

            string lastViewed = "S" + MainProgram.Variables.seriesList[MainProgram.Variables.seriesList.Count - 1].lastViewed[0] + "E" + MainProgram.Variables.seriesList[MainProgram.Variables.seriesList.Count - 1].lastViewed[1];
            string lastEpisode = "S" + MainProgram.Variables.seriesList[MainProgram.Variables.seriesList.Count - 1].lastEpisode[0] + "E" + MainProgram.Variables.seriesList[MainProgram.Variables.seriesList.Count - 1].lastEpisode[1];
            string nextEpisode = "S" + MainProgram.Variables.seriesList[MainProgram.Variables.seriesList.Count - 1].nextEpisode[0] + "E" + MainProgram.Variables.seriesList[MainProgram.Variables.seriesList.Count - 1].nextEpisode[1];
            string nextAirDate = MainProgram.Variables.seriesList[MainProgram.Variables.seriesList.Count - 1].nextEpisodeAirDate.ToString("d");

            writeData.WriteLine("{0};{1};{2};{3};{4};{5}", name, imdbId, lastViewed, lastEpisode, nextEpisode, nextAirDate);
            writeData.Close();
        }
    }
}
