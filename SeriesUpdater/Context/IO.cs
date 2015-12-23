using Newtonsoft.Json;
using SeriesUpdater.Internal;
using System.Collections.Generic;
using System.IO;

namespace SeriesUpdater.Context
{
    class IO
    {
        // Series
        public static void ReadSeries()
        {
            if (!File.Exists(Variables.SeriesDataFileName)) return;

            string jsonString = File.ReadAllText(Variables.SeriesDataFileName);
            Variables.SeriesList = JsonConvert.DeserializeObject<List<Series>>(jsonString);
        }

        public static void WriteSeries(string Name = null, string ImdbId = null)
        {
            string dataFolderPath = Variables.DataFolderPath;
            if (!Directory.Exists(dataFolderPath)) Directory.CreateDirectory(dataFolderPath);

            string jsonString = JsonConvert.SerializeObject(Variables.SeriesList);
            File.WriteAllText(Variables.SeriesDataFileName, jsonString);
        }
    }
}
