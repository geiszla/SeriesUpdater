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
            this.Visible = false;

            if (MainProgram.Variables.searchQuery != "")
            {
                searchBox.Text = MainProgram.Variables.searchQuery;
                startSearch();
            }

            this.Visible = true;
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            if (searchBox.Text == "")
            {
                MessageBox.Show("A kereséshez kérem adjon meg egy kulcsszót!", "Üres mező", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            else
            {
                startSearch();
            }
        }

        private void selectButton_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            MainProgram.Variables.selectedSeries[0] = resultTable.SelectedRows[0].Cells[0].Value.ToString();
            MainProgram.Variables.selectedSeries[1] = resultTable.SelectedRows[0].Cells[1].Value.ToString();

            int id = Convert.ToInt32(resultTable.SelectedRows[0].Cells[0].Value);
            string url = "http://www.imdb.com/title/" + "tt" + id + "/episodes";
            MainProgram.Variables.selectedLastEpisodes = MainProgram.ProcessHTML.getLatestEpisodeFromHTML(id, MainProgram.WebRequest.requestPage(url), false);

            if (MainProgram.Variables.selectedLastEpisodes[0] != 0)
            {
                MainProgram.Variables.isSelectedSeries = true;
                this.Close();
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
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

            MainProgram.WebRequest.searchForSeries(searchBox.Text);
            DataTable seriesTable = MainProgram.ProcessData.createTable();

            if (seriesTable.Rows.Count == 0)
            {
                MessageBox.Show("Nincs találat. Kérem próbálja újra más kulcsszóval!", "Nincs találat", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        public int id;
        public string name;
        public string aka;
        public string startYear;
        public string type;
    }
    #endregion
}