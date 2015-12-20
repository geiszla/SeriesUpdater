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

            if (Internal.Variables.SearchQuery != "")
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

            Internal.Variables.SelectedSeries = new Series(name, imdbId, new Episode(), new Episode(), new DateTime(), 3);

            string id = resultTable.SelectedRows[0].Cells[0].Value.ToString();            
            Internal.Variables.SelectedSeries.LastEpisode = Internal.ProcessHTML.GetLatestAndNextEpisode(id, true);

            if (Internal.Variables.SelectedSeries.LastEpisode.SeasonNumber != 0)
            {
                Internal.Variables.IsSelectedSeries = true;
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
            Internal.Variables.ResultSeriesList.Clear();
            Cursor.Current = Cursors.WaitCursor;

            Internal.WebRequests.SearchForSeries(searchBox.Text);
            DataTable seriesTable = Internal.ProcessData.CreateTable();

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