using SeriesUpdater.Context;
using SeriesUpdater.Internal;
using System;
using System.Data;
using System.Windows.Forms;

namespace SeriesUpdater
{
    public partial class SearchForm : Form
    {
        public SearchForm()
        {
            InitializeComponent();
        }

        #region Events
        private void Form3_Load(object sender, EventArgs e)
        {
            Visible = false;

            if (Variables.SearchQuery != "")
            {
                searchBox.Text = Internal.Variables.SearchQuery;
                startSearch();
            }

            Visible = true;
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            if (searchBox.Text == "")
            {
                Notifications.ShowError("Please give a keyword to search for.",
                    "No search keyword given");
            }

            else
            {
                startSearch();
            }
        }

        private void selectButton_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            string imdbId = resultTable.SelectedRows[0].Cells[0].Value.ToString();
            string name = resultTable.SelectedRows[0].Cells[1].Value.ToString();

            Variables.SelectedSeries = new Series(name, imdbId, new Episode(), new Episode(), new DateTime(), 3);

            string id = resultTable.SelectedRows[0].Cells[0].Value.ToString();            
            Variables.SelectedSeries.LastEpisode = Internal.ProcessHTML.GetLatestAndNextEpisode(id, true);

            if (Variables.SelectedSeries.LastEpisode.SeasonNumber != 0)
            {
                Variables.IsSelectedSeries = true;
                Close();
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void searchBox_TextChanged(object sender, EventArgs e)
        {
            if (searchBox.Text == "")
            {
                searchButton.Enabled = false;
            }

            else
            {
                searchButton.Enabled = true;
            }
        }

        private void searchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                searchButton_Click(new Object(), new EventArgs());
            }
        }
        #endregion

        #region Functions
        void startSearch()
        {
            Variables.ResultSeriesList.Clear();
            Cursor.Current = Cursors.WaitCursor;

            WebRequests.SearchForSeries(searchBox.Text);
            DataTable seriesTable = ProcessData.CreateTable();

            if (seriesTable.Rows.Count == 0)
            {
                Notifications.ShowError("No series found. Please try using another keyword.",
                    "No series found");
                selectButton.Enabled = false;
            }

            else
            {
                resultTable.DataSource = seriesTable;

                resultTable.Columns[0].Width = 25;
                resultTable.Columns[1].Width = 70;
                resultTable.Columns[2].Width = 145;
                resultTable.Columns[3].Width = 105;
                resultTable.Columns[4].Width = 45;
                resultTable.Columns[4].Width = 45;

                selectButton.Enabled = true;
            }

            Cursor.Current = Cursors.Arrow;
        }
        #endregion
    }

    #region Classes
    public class ResultSeries
    {
        public string id;
        public string name;
        public string aka;
        public string startYear;
        public string type;
    }
    #endregion
}