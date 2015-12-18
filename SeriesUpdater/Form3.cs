using System;
using System.Data;
using System.Windows.Forms;

namespace SeriesUpdater
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        #region Events
        private void Form3_Load(object sender, EventArgs e)
        {
            Visible = false;

            if (MainProgram.Variables.searchQuery != "")
            {
                searchBox.Text = MainProgram.Variables.searchQuery;
                startSearch();
            }

            Visible = true;
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            if (searchBox.Text == "")
            {
                MessageBox.Show("Please give a keyword to search for.", "No search keyword given", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            MainProgram.Variables.selectedSeries = new Series(name, imdbId, new Episode(), new Episode(), new DateTime(), 3);

            string id = resultTable.SelectedRows[0].Cells[0].Value.ToString();
            string url = "http://www.imdb.com/title/" + "tt" + id + "/episodes";

            MainProgram.ProcessHTML.CurrNextAirDate = new DateTime();
            MainProgram.ProcessHTML.CurrNextDateIndex = 0;
            MainProgram.Variables.selectedSeries.LastEpisode = MainProgram.ProcessHTML.GetLatestEpisodeFromHTML(id, MainProgram.WebRequest.RequestPage(url), true);

            if (MainProgram.Variables.selectedSeries.LastEpisode.SeasonNumber != 0)
            {
                MainProgram.Variables.isSelectedSeries = true;
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
            MainProgram.Variables.resultSeriesList.Clear();
            Cursor.Current = Cursors.WaitCursor;

            MainProgram.WebRequest.SearchForSeries(searchBox.Text);
            DataTable seriesTable = MainProgram.ProcessData.createTable();

            if (seriesTable.Rows.Count == 0)
            {
                MessageBox.Show("No series found. Please try using another keyword.",
                    "No series found", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            else
            {
                resultTable.DataSource = seriesTable;

                for (int i = 0; i < resultTable.Columns.Count; i++)
                {
                    resultTable.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                }
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