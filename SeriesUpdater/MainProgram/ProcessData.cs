using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace SeriesUpdater.MainProgram
{
    class ProcessData
    {
        int getLabelId(object sender)
        {
            Label currLabel = (Label)sender;

            int currId = Convert.ToInt32(currLabel.Name.Substring(currLabel.Name.LastIndexOf('_') + 1));

            return currId;
        }

        public static int[] convertEpisodeString(string inputSeriesString)
        {
            int[] outputInt = new int[2];
            outputInt[0] = Convert.ToInt32(inputSeriesString.Substring(inputSeriesString.IndexOf('S') + 1, inputSeriesString.IndexOf('E') - inputSeriesString.IndexOf('S') - 1));
            outputInt[1] = Convert.ToInt32(inputSeriesString.Substring(inputSeriesString.IndexOf('E') + 1));

            return outputInt;
        }

        public static DataTable createTable()
        {
            DataTable seriesTable = new DataTable();
            seriesTable.Columns.Add("IMDB id", typeof(string));
            seriesTable.Columns.Add("Név", typeof(string));
            seriesTable.Columns.Add("Másképpen", typeof(string));
            seriesTable.Columns.Add("Év", typeof(string));
            seriesTable.Columns.Add("Típus", typeof(string));

            for (int i = 0; i < MainProgram.Variables.resultSeriesList.Count; i++)
            {
                List<ResultSeries> resultSeriesList = MainProgram.Variables.resultSeriesList;
                seriesTable.Rows.Add(resultSeriesList[i].id, resultSeriesList[i].name, resultSeriesList[i].aka, resultSeriesList[i].startYear, resultSeriesList[i].type);
            }

            return seriesTable;
        }

        public static int getElementFromListByName(string name)
        {
            int elementNumber = -1;
            for (int i = 0; i < MainProgram.Variables.seriesList.Count; i++)
            {
                if (MainProgram.Variables.seriesList[i].name == name)
                {
                    elementNumber = i;
                }
            }

            return elementNumber;
        }
    }
}
