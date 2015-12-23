using System.Collections.Generic;
using System.Data;

namespace SeriesUpdater.Internal
{
    class ProcessData
    {
        public static DataTable CreateTable()
        {
            DataTable seriesTable = new DataTable();
            seriesTable.Columns.Add("#", typeof(int));
            seriesTable.Columns.Add("IMDB id", typeof(string));
            seriesTable.Columns.Add("Name", typeof(string));
            seriesTable.Columns.Add("AKA", typeof(string));
            seriesTable.Columns.Add("Year", typeof(string));
            seriesTable.Columns.Add("Type", typeof(string));

            for (int i = 0; i < Variables.ResultSeriesList.Count; i++)
            {
                ResultSeries currSeries = Variables.ResultSeriesList[i];

                seriesTable.Rows.Add(i + 1, currSeries.id, currSeries.name, currSeries.aka,
                    currSeries.startYear, currSeries.type);
            }

            return seriesTable;
        }
    }
}
