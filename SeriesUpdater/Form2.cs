using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;

namespace SeriesUpdater
{
    public partial class Form2 : Form
    {
        #region Public variables
        public static string[] selectedSeries = new string[2];
        public static bool isSelectedSeries = false;
        public static string searchQuery = "";
        #endregion

        #region Initialization
        public Form2()
        {
            InitializeComponent();
        }
        #endregion

        #region Events
        private void Form2_Load(object sender, EventArgs e)
        {
            Form1.isAddedSeries = false;
            this.ActiveControl = nameTextBox;
        }
        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void acceptAddButton_Click(object sender, EventArgs e)
        {
            if (nameTextBox.Text == "")
            {
                MessageBox.Show("Kérem adjon egy nevet a felvett sorozatnak!", "Üres mező", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            else if (imdbIdTextBox.Text == "")
            {
                MessageBox.Show("A sorozat azonosításához kérem adja meg a megfelelő IMBD azonosítót!", "Üres mező", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            else if (lastViewedEpisodeTextBox.Text == "")
            {
                MessageBox.Show("A sorozat követéséhez kérem adja meg a legutoljára látott részt!", "Üres mező", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            else
            {
                try
                {
                    Convert.ToInt32(imdbIdTextBox.Text);
                }

                catch
                {
                    MessageBox.Show("Nem megfelelő az IMDB azonodító formátuma. A kód csak számokat tartalmazhat.", "Érvénytelen formátum", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                try
                {
                    Form1.convertSeriesString(lastViewedEpisodeTextBox.Text);
                }

                catch
                {
                    MessageBox.Show("Nem megfelelő a megadott \"legutoljára látott epizód\" formátuma.", "Érvénytelen formátum", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                addSeries();
            }
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            Form1.isAddFormOpened = false;
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            isSelectedSeries = false;

            searchQuery = nameTextBox.Text;

            Form3 searchForm = new Form3();
            searchForm.FormClosed += searchForm_FormClosed;
            searchForm.ShowDialog();
        }

        private void fillButton_Click(object sender, EventArgs e)
        {
            if (imdbIdTextBox.Text != "")
            {
                try
                {
                    Convert.ToInt32(imdbIdTextBox.Text);
                }

                catch
                {
                    MessageBox.Show("Nem megfelelő az IMDB azonodító formátuma. A kód csak számokat tartalmazhat.", "Érvénytelen formátum", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Cursor.Current = Cursors.WaitCursor;
                string HTMLText = Form1.requestImdb(Convert.ToInt32(imdbIdTextBox.Text), "");
                int[] latestEp = Form1.getLatestEp(Convert.ToInt32(imdbIdTextBox.Text), HTMLText, false);

                if (HTMLText != "")
                {
                    nameTextBox.Text = Form1.getNameFromHTML(HTMLText);
                    lastViewedEpisodeTextBox.Text = "S" + latestEp[0] + "E" + latestEp[1];
                }
            }

            else
            {
                MessageBox.Show("A sorozat azonosításához kérem adja meg a megfelelő IMDB azonosítót!", "Üres mező", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void searchForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (isSelectedSeries)
            {
                int id = Convert.ToInt32(selectedSeries[0]);
                int[] latestEp = Form1.getLatestEp(id, Form1.requestImdb(id, ""), false);

                if (latestEp[0] != 0)
                {
                    imdbIdTextBox.Text = selectedSeries[0];
                    nameTextBox.Text = selectedSeries[1];
                    lastViewedEpisodeTextBox.Text = "S" + latestEp[0] + "E" + latestEp[1];
                }
            }
        }

        private void nameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                searchButton_Click(new object(), new EventArgs());
            }
        }

        private void imdbIdTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                fillButton_Click(new object(), new EventArgs());
            }
        }
        #endregion

        #region Main functions
        public void addSeries()
        {
            Cursor.Current = Cursors.WaitCursor;

            Application.OpenForms[0].Height += 25;
            Application.OpenForms[0].Top -= 25;

            Series newSeries = new Series();
            newSeries.id = Form1.seriesList.Count;
            newSeries.name = nameTextBox.Text;
            newSeries.englishName = imdbIdTextBox.Text;
            newSeries.lastViewed = Form1.convertSeriesString(lastViewedEpisodeTextBox.Text);
            newSeries.lastEpisode = new int[2];
            Form1.seriesList.Add(newSeries);

            Form1.getLatestEps(true);

            if (Form1.seriesList[Form1.seriesList.Count - 1].lastEpisode[0] == 0)
            {
                return;
            }

            if (!Directory.Exists(Form1.dataPath))
            {
                Directory.CreateDirectory(Form1.dataPath);
            }

            StreamWriter addData = new StreamWriter(Form1.dataPath + @"\series.dat", true);

            string name = newSeries.name;
            string englishName = newSeries.englishName;
            string lastViewed = "S" + Form1.seriesList[Form1.seriesList.Count - 1].lastViewed[0] + "E" + Form1.seriesList[Form1.seriesList.Count - 1].lastViewed[1];
            string lastEpisode = "S" + Form1.seriesList[Form1.seriesList.Count - 1].lastEpisode[0] + "E" + Form1.seriesList[Form1.seriesList.Count - 1].lastEpisode[1];
            string nextEpisode = "S" + Form1.seriesList[Form1.seriesList.Count - 1].nextEpisode[0] + "E" + Form1.seriesList[Form1.seriesList.Count - 1].nextEpisode[1];
            string nextAirDate = Form1.seriesList[Form1.seriesList.Count - 1].nextEpisodeAirDate.ToString("d");

            addData.WriteLine("{0};{1};{2};{3};{4};{5}", name, englishName, lastViewed, lastEpisode, nextEpisode, nextAirDate);
            addData.Close();
            Cursor.Current = Cursors.Arrow;
            Form1.isAddedSeries = true;

            this.Close();
        }
        #endregion

        #region Subfunctions
        protected override void WndProc(ref Message message)
        {
            const int WM_NCHITTEST = 0x0084;

            if (message.Msg == WM_NCHITTEST)
                return;

            base.WndProc(ref message);
        }
        #endregion
    }
}
