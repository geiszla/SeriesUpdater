﻿using System;
using System.IO;
using System.Windows.Forms;

namespace SeriesUpdater
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        #region Events
        private void Form2_Load(object sender, EventArgs e)
        {
            MainProgram.Variables.isAddedSeries = false;
            this.ActiveControl = nameTextBox;
        }
        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void addButton_Click(object sender, EventArgs e)
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
                    MainProgram.ProcessData.convertEpisodeString(lastViewedEpisodeTextBox.Text);
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
            MainProgram.Variables.isAddFormOpened = false;
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            MainProgram.Variables.isSelectedSeries = false;

            MainProgram.Variables.searchQuery = nameTextBox.Text;

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
                string url = "http://www.imdb.com/title/" + "tt" + Convert.ToInt32(imdbIdTextBox.Text) + "/episodes";
                string HTMLText = MainProgram.WebRequest.requestPage(url);
                int[] latestEp = MainProgram.ProcessHTML.getLatestEpisodeFromHTML(imdbIdTextBox.Text, HTMLText, false);

                if (HTMLText != "")
                {
                    nameTextBox.Text = MainProgram.ProcessHTML.getNameFromHTML(HTMLText);
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
            if (MainProgram.Variables.isSelectedSeries)
            {
                imdbIdTextBox.Text = MainProgram.Variables.selectedSeries[0];
                nameTextBox.Text = MainProgram.Variables.selectedSeries[1];
                lastViewedEpisodeTextBox.Text = "S" + MainProgram.Variables.selectedLastEpisodes[0] + "E" + MainProgram.Variables.selectedLastEpisodes[1];
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
        #endregion

        #region Functions
        void addSeries()
        {
            Cursor.Current = Cursors.WaitCursor;

            Series newSeries = new Series(MainProgram.Variables.seriesList.Count, nameTextBox.Text, imdbIdTextBox.Text, MainProgram.ProcessData.convertEpisodeString(lastViewedEpisodeTextBox.Text), new int[2], new int[2], new DateTime());
            MainProgram.Variables.seriesList.Add(newSeries);

            MainProgram.WebRequest.getLatestEpisodes(true);

            if (MainProgram.Variables.seriesList[MainProgram.Variables.seriesList.Count - 1].lastEpisode[0] == 0)
            {
                return;
            }

            if (!Directory.Exists(MainProgram.Variables.dataPath))
            {
                Directory.CreateDirectory(MainProgram.Variables.dataPath);
            }

            Context.IO.writeSeries(newSeries.name, newSeries.imdbId);

            Cursor.Current = Cursors.Arrow;
            MainProgram.Variables.isAddedSeries = true;

            this.Close();
        }

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
    }
}
