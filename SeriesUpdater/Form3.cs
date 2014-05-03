using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
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

            if (Variables.PublicVariables.searchQuery != "")
            {
                searchBox.Text = Variables.PublicVariables.searchQuery;
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
            Variables.PublicVariables.selectedSeries[0] = resultTable.SelectedRows[0].Cells[0].Value.ToString();
            Variables.PublicVariables.selectedSeries[1] = resultTable.SelectedRows[0].Cells[1].Value.ToString();

            int id = Convert.ToInt32(resultTable.SelectedRows[0].Cells[0].Value);
            Variables.PublicVariables.selectedLastEpisodes = Form1.getLatestEp(id, WebRequests.Core.requestImdb(id, ""), false);

            if (Variables.PublicVariables.selectedLastEpisodes[0] != 0)
            {
                Variables.PublicVariables.isSelectedSeries = true;
                this.Close();
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region Main functions
        void startSearch()
        {
            Variables.PublicVariables.resultSeriesList.Clear();
            Cursor.Current = Cursors.WaitCursor;

            searchForSeries(searchBox.Text);
            DataTable seriesTable = CreateTable();

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

        public void searchForSeries(string query)
        {
            string url = "http://www.imdb.com/find?" + "q=" + query + "&s=tt&ttype=tv&ref_=fn_tv";

            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
            myRequest.Method = "GET";
            WebResponse myResponse = myRequest.GetResponse();

            StreamReader sr = new StreamReader(myResponse.GetResponseStream(), System.Text.Encoding.UTF8);
            string HTMLText = sr.ReadToEnd();
            sr.Close();
            myResponse.Close();

            int startIndex = 0;
            string innerHTML = "";
            for (int i = 0; i < 10; i++)
            {
                if ((innerHTML = Form1.getInnerHTMLByClassOrId(startIndex, HTMLText, "result_text", "class")[0]) == null)
                {
                    break;
                }

                ResultSeries currResult = new ResultSeries();

                Match typeMatch = Regex.Match(innerHTML, @"\(([a-z| |-]*)\)", RegexOptions.IgnoreCase);
                currResult.type = typeMatch.Groups[1].Value;

                if (currResult.type == "TV Movie" || currResult.type == "TV Short")
                {
                    i--;
                }

                else
                {
                    Match idMatch = Regex.Match(innerHTML, @"/title/tt(.*)\/\?", RegexOptions.IgnoreCase);
                    currResult.id = Convert.ToInt32(idMatch.Groups[1].Value);

                    Match nameMatch = Regex.Match(innerHTML, "<(a href=)+.*>(.*)</a", RegexOptions.IgnoreCase);
                    currResult.name = nameMatch.Groups[2].Value;

                    Match akaMatch = Regex.Match(innerHTML, "<i>\"(.*)\"</i>", RegexOptions.IgnoreCase);
                    currResult.aka = akaMatch.Groups[1].Value;

                    Match yearMatch = Regex.Match(innerHTML, @"> \(([0-9]*)\)", RegexOptions.IgnoreCase);
                    currResult.startYear = yearMatch.Groups[1].Value;

                    Variables.PublicVariables.resultSeriesList.Add(currResult);
                }

                startIndex = Convert.ToInt32(Form1.getInnerHTMLByClassOrId(startIndex, HTMLText, "result_text", "class")[1]);
            }
        }

        static DataTable CreateTable()
        {
            DataTable seriesTable = new DataTable();
            seriesTable.Columns.Add("IMDB id", typeof(int));
            seriesTable.Columns.Add("Név", typeof(string));
            seriesTable.Columns.Add("Másképpen", typeof(string));
            seriesTable.Columns.Add("Év", typeof(string));
            seriesTable.Columns.Add("Típus", typeof(string));

            for (int i = 0; i < Variables.PublicVariables.resultSeriesList.Count; i++)
            {
                List<ResultSeries> resultSeriesList = Variables.PublicVariables.resultSeriesList;
                seriesTable.Rows.Add(resultSeriesList[i].id, resultSeriesList[i].name, resultSeriesList[i].aka, resultSeriesList[i].startYear, resultSeriesList[i].type);
            }

            return seriesTable;
        }
        #endregion

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