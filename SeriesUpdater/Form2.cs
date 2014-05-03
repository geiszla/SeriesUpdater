using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;

namespace SeriesUpdater
{
    public partial class Form2 : Form
    {
        #region Public variables
        
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
            Variables.PublicVariables.isAddedSeries = false;
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
            Variables.PublicVariables.isAddFormOpened = false;
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            Variables.PublicVariables.isSelectedSeries = false;

            Variables.PublicVariables.searchQuery = nameTextBox.Text;

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
                string HTMLText = WebRequests.Core.requestImdb(Convert.ToInt32(imdbIdTextBox.Text), "");
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
            if (Variables.PublicVariables.isSelectedSeries)
            {
                int id = Convert.ToInt32(Variables.PublicVariables.selectedSeries[0]);

                imdbIdTextBox.Text = Variables.PublicVariables.selectedSeries[0];
                nameTextBox.Text = Variables.PublicVariables.selectedSeries[1];
                lastViewedEpisodeTextBox.Text = "S" + Variables.PublicVariables.selectedLastEpisodes[0] + "E" + Variables.PublicVariables.selectedLastEpisodes[1];
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

            Series newSeries = new Series(Variables.PublicVariables.seriesList.Count, nameTextBox.Text, Convert.ToInt32(imdbIdTextBox.Text), Form1.convertSeriesString(lastViewedEpisodeTextBox.Text), new int[2], new int[2], new DateTime());
            Variables.PublicVariables.seriesList.Add(newSeries);

            Form1.getLatestEps(true);

            if (Variables.PublicVariables.seriesList[Variables.PublicVariables.seriesList.Count - 1].lastEpisode[0] == 0)
            {
                return;
            }

            if (!Directory.Exists(Variables.PublicVariables.dataPath))
            {
                Directory.CreateDirectory(Variables.PublicVariables.dataPath);
            }

            StreamWriter addData = new StreamWriter(Variables.PublicVariables.dataPath + @"\series.dat", true);

            string name = newSeries.name;
            int imdbId = Convert.ToInt32(newSeries.imdbId);
            string lastViewed = "S" + Variables.PublicVariables.seriesList[Variables.PublicVariables.seriesList.Count - 1].lastViewed[0] + "E" + Variables.PublicVariables.seriesList[Variables.PublicVariables.seriesList.Count - 1].lastViewed[1];
            string lastEpisode = "S" + Variables.PublicVariables.seriesList[Variables.PublicVariables.seriesList.Count - 1].lastEpisode[0] + "E" + Variables.PublicVariables.seriesList[Variables.PublicVariables.seriesList.Count - 1].lastEpisode[1];
            string nextEpisode = "S" + Variables.PublicVariables.seriesList[Variables.PublicVariables.seriesList.Count - 1].nextEpisode[0] + "E" + Variables.PublicVariables.seriesList[Variables.PublicVariables.seriesList.Count - 1].nextEpisode[1];
            string nextAirDate = Variables.PublicVariables.seriesList[Variables.PublicVariables.seriesList.Count - 1].nextEpisodeAirDate.ToString("d");

            addData.WriteLine("{0};{1};{2};{3};{4};{5}", name, imdbId, lastViewed, lastEpisode, nextEpisode, nextAirDate);
            addData.Close();
            Cursor.Current = Cursors.Arrow;
            Variables.PublicVariables.isAddedSeries = true;

            this.Close();
        }
        #endregion

        #region Subfunctions
        void checkEmptyBoxes()
        {
            if (imdbIdTextBox.Text != "")
            {
                fillButton.Enabled = true;

                if (nameTextBox.Text != "" && imdbIdTextBox.Text != "" && lastViewedEpisodeTextBox.Text != "")
                {
                    addButton.Enabled = true;
                }

                else
                {
                    addButton.Enabled = false;
                }
            }

            else
            {
                fillButton.Enabled = false;
                addButton.Enabled = false;
            }
        }

        protected override void WndProc(ref Message message)
        {
            const int WM_NCHITTEST = 0x0084;

            if (message.Msg == WM_NCHITTEST)
                return;

            base.WndProc(ref message);
        }
        #endregion

        private void nameTextBox_TextChanged(object sender, EventArgs e)
        {
            checkEmptyBoxes();
        }

        private void imdbIdTextBox_TextChanged(object sender, EventArgs e)
        {
            checkEmptyBoxes();
        }

        private void lastViewedEpisodeTextBox_TextChanged(object sender, EventArgs e)
        {
            checkEmptyBoxes();
        }
    }
}
