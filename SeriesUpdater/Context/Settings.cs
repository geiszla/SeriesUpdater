using System;
using System.IO;

namespace SeriesUpdater.Context
{
    class Settings
    {
        public static void readInput()
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
                        Series currSeries = new Series(whileCount, currRow[0], Convert.ToInt32(currRow[1]), Form1.convertSeriesString(currRow[2]), Form1.convertSeriesString(currRow[3]), Form1.convertSeriesString(currRow[4]), Convert.ToDateTime(currRow[5]));
                        MainProgram.Variables.seriesList.Add(currSeries);
                    }

                    whileCount++;
                }

                readData.Close();
            }
        }
    }
}
