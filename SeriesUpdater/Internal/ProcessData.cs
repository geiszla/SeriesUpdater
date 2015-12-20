using System.Collections.Generic;
using System.Data;

namespace SeriesUpdater.Internal
{
    class ProcessData
    {
        public static DataTable CreateTable()
        {
            DataTable seriesTable = new DataTable();
            seriesTable.Columns.Add("IMDB id", typeof(string));
            seriesTable.Columns.Add("Name", typeof(string));
            seriesTable.Columns.Add("AKA", typeof(string));
            seriesTable.Columns.Add("Year", typeof(string));
            seriesTable.Columns.Add("Type", typeof(string));

            foreach (ResultSeries currSeries in Variables.ResultSeriesList)
            {
                List<ResultSeries> resultSeriesList = Variables.ResultSeriesList;
                seriesTable.Rows.Add(currSeries.id, currSeries.name, currSeries.aka,
                    currSeries.startYear, currSeries.type);
            }

            return seriesTable;
        }
    }
}
