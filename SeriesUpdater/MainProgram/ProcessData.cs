using System.Collections.Generic;
using System.Data;

namespace SeriesUpdater.MainProgram
{
    class ProcessData
    {
        public static DataTable createTable()
        {
            DataTable seriesTable = new DataTable();
            seriesTable.Columns.Add("IMDB id", typeof(string));
            seriesTable.Columns.Add("Name", typeof(string));
            seriesTable.Columns.Add("AKA", typeof(string));
            seriesTable.Columns.Add("Year", typeof(string));
            seriesTable.Columns.Add("Type", typeof(string));

            foreach (ResultSeries currSeries in Variables.resultSeriesList)
            {
                List<ResultSeries> resultSeriesList = Variables.resultSeriesList;
                seriesTable.Rows.Add(currSeries.id, currSeries.name, currSeries.aka,
                    currSeries.startYear, currSeries.type);
            }

            return seriesTable;
        }
    }
}
