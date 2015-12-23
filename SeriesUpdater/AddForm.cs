using SeriesUpdater.Internal;
using System;
using System.Windows.Forms;

namespace SeriesUpdater
{
    public partial class AddForm : Form
    {
        public AddForm()
        {
            InitializeComponent();
        }

        #region Events
        private void Form2_Load(object sender, EventArgs e)
        {
            placeForm();
            Variables.IsAddedSeries = false;
            ActiveControl = nameTextBox;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            if (nameTextBox.Text == "")
            {
                MessageBox.Show("Please give the series a name.", "No name given", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            else if (imdbIdTextBox.Text == "")
            {
                MessageBox.Show("Please give a valid IMDB ID of the series or search by name from the field above.", "No IMDB ID given", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            else if (lastViewedEpisodeTextBox.Text == "")
            {
                MessageBox.Show("Please give the last episode you watched from this series.", "No last viewed episode given", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            else
            {
                try
                {
                    Convert.ToInt32(imdbIdTextBox.Text);
                }

                catch
                {
                    MessageBox.Show("The IMDB ID can only contain numbers. Please check the given value.", "Invalid IMDB ID", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!Episode.IsValidEpisodeString(lastViewedEpisodeTextBox.Text))
                {
                    MessageBox.Show("Format of the given last viewed episode is invalid. Please give a valid value, eg. S05E13", "Invalid last viewed episode", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                addSeries();
                Context.Notifications.ShowComingSeries(true);
            }
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            Variables.IsAddFormOpened = false;
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            Variables.IsSelectedSeries = false;

            Variables.SearchQuery = nameTextBox.Text;

            SearchForm searchForm = new SearchForm();
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
                    MessageBox.Show("The IMDB ID can only contain numbers. Please check the given value.",
                        "Invalid IMDB ID", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Cursor.Current = Cursors.WaitCursor;
                string url = "http://www.imdb.com/title/" + "tt" + Convert.ToInt32(imdbIdTextBox.Text) + "/episodes";
                string HTMLText = WebRequests.RequestPage(url);
                Episode latestEp = ProcessHTML.GetEpisodesFromHTML(imdbIdTextBox.Text, HTMLText, false);

                if (HTMLText != "")
                {
                    nameTextBox.Text = ProcessHTML.GetNameFromHTML(HTMLText);
                    lastViewedEpisodeTextBox.Text = latestEp.ToString();
                }
            }

            else
            {
                MessageBox.Show("Please give a valid IMDB ID of the desired series.",
                    "Empty IMDB ID", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void searchForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (Variables.IsSelectedSeries)
            {
                imdbIdTextBox.Text = Variables.SelectedSeries.ImdbId;
                nameTextBox.Text = Variables.SelectedSeries.Name;
                lastViewedEpisodeTextBox.Text = Variables.SelectedSeries.LastEpisode.ToString();
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
        void placeForm()
        {
            Top = Screen.PrimaryScreen.WorkingArea.Height - Height - 10;
        }

        void addSeries()
        {
            string imdbId = imdbIdTextBox.Text;
            bool isFound = false;
            foreach (Series currSeries in Variables.SeriesList)
            {
                if (imdbId == currSeries.ImdbId)
                {
                    isFound = true;
                }
            }

            if (!isFound)
            {
                Cursor.Current = Cursors.WaitCursor;

                Series newSeries = new Series(Variables.SeriesList.Count, nameTextBox.Text, imdbId,
                    new Episode(lastViewedEpisodeTextBox.Text), new Episode(), new Episode(), new DateTime(), 3, 0);
                Variables.SeriesList.Add(newSeries);

                if (Variables.SelectedSeries.ImdbId == newSeries.ImdbId)
                {
                    newSeries.LastEpisode = Variables.SelectedSeries.LastEpisode;
                    newSeries.NextEpisode = Variables.SelectedSeries.NextEpisode;
                    newSeries.NextEpisodeAirDate = Variables.SelectedSeries.NextEpisodeAirDate;
                }

                if (Variables.SeriesList[Variables.SeriesList.Count - 1].LastEpisode.SeasonNumber == 0)
                {
                    return;
                }

                Context.IO.WriteSeries(newSeries.Name, newSeries.ImdbId);

                Cursor.Current = Cursors.Arrow;
                Variables.IsAddedSeries = true;

                Close();
            }

            else
            {
                MessageBox.Show("This series is already in the list. Please select another one.",
                    "Series already in list", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
