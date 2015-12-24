using SeriesUpdater.Context;
using SeriesUpdater.Internal;
using SeriesUpdater.Properties;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace SeriesUpdater
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            checkForOtherInstance();
            InitializeComponent();

            Variables.NotifyIcon = this.notifyIcon;
            Variables.MainForm = this;

            RunOnStartupToolStripMenuItem.Checked = Context.Settings.IsStartupItem();
            WireAllControls(this);
        }

        #region Form Events
        private void Form1_Load(object sender, EventArgs e)
        {
            IO.ReadSeries();

            if (Variables.IsFirst)
            {
                if (!Context.Settings.IsStartupItem()
                    && Notifications.ShowQuestion("Do you want to start Series Updater with Windows?",
                        "Start with Windows") == DialogResult.Yes)
                {
                    RunOnStartupToolStripMenuItem.Checked = Context.Settings.SetAutorun(true);
                }

                if (!Directory.Exists(Variables.DataFolderPath))
                {
                    Directory.CreateDirectory(Variables.DataFolderPath);
                }
            }

            updateSeries();

            System.Timers.Timer refreshTimer = new System.Timers.Timer();
            refreshTimer.Elapsed += refreshTimer_Elapsed;
            refreshTimer.Interval = 30 * 60 * 1000;
            refreshTimer.Enabled = true;

            notifyIcon.MouseUp += notifyIcon_MouseClick;
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            Hide();
        }

        private void Form1_Deactivate(object sender, EventArgs e)
        {
            if (!Variables.IsAddFormOpened)
            {
                Internal.Variables.LastDeactivateTick = Environment.TickCount;
                Hide();
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            notifyIcon.Visible = false;
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            Internal.Variables.IsAddFormOpened = true;
            AddForm addForm = new AddForm();
            addForm.FormClosed += addForm_FormClosed;
            addForm.ShowDialog();
        }

        private void addForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (Internal.Variables.IsAddedSeries)
            {
                applyData(true);
                placeForm(true, false);
            }
        }

        private void deleteImage_Click(object sender, EventArgs e)
        {
            Deactivate -= Form1_Deactivate;
            if (Notifications.ShowQuestion("Are you sure you want to remove this series from the list?",
                "Remove series") == DialogResult.Yes)
            {
                PictureBox deleteImage = (PictureBox)sender;
                int id = Convert.ToInt32(deleteImage.Name.Split('_')[1]);
                Label nameLabel = (Label)Controls.Find("name_" + id, true)[0];
                Internal.Variables.SeriesList.Remove(Internal.Variables.SeriesList.Where(x => x.Name == nameLabel.Text).FirstOrDefault());

                applyData(false);
                Context.IO.WriteSeries();
            }

            Deactivate += Form1_Deactivate;
        }

        private void newTextBox_LostFocus(object sender, EventArgs e)
        {
            if (!Internal.Variables.IsKeyDownFired)
            {
                editLastViewedLabel(sender);
            }

            else
            {
                Internal.Variables.IsKeyDownFired = false;
            }
        }

        void newTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Internal.Variables.IsKeyDownFired = true;
                editLastViewedLabel(sender);
            }
        }

        void lastViewedLabel_Click(object sender, EventArgs e)
        {
            editLastViewedLabel(sender);
        }

        private void constrols_Click(object sender, EventArgs e)
        {
            InvokeOnClick(this, EventArgs.Empty);
        }

        void refreshTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Application.OpenForms[0].Invoke(new Action(() => updateSeries()));
        }
        #endregion

        #region NotifyIcon Events
        private void notifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            placeForm(false, true);

            if (e.Button == MouseButtons.Left)
            {
                if (Environment.TickCount - Internal.Variables.LastDeactivateTick > 250)
                {
                    Show();
                    Activate();
                }
            }
        }
        #endregion

        #region Contextmenu events
        private void notifyIconControlManagementMenu_MouseUp(object sender, MouseEventArgs e)
        {
            notifyIconContextMenu.Hide();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void autorunStripMenuItem_Click(object sender, EventArgs e)
        {
            RunOnStartupToolStripMenuItem.Checked = Context.Settings.SetAutorun();
        }

        private void notificationContextMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.SendNotifications = sendNotificationsToolStripMenuItem.Checked;
            Properties.Settings.Default.Save();
        }

        private void customLanguageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string message = "To change the language of the display title of series, first you have to set it up in your IMDB account (https://secure.imdb.com/register-imdb/siteprefs), then you can log in here using your Google credentials. Do you want to log in now?";

            if (Notifications.ShowQuestion(message, "Change IMDB language") == DialogResult.Yes)
            {
                LoginForm loginForm = new LoginForm();
                loginForm.ShowDialog();
            }
        }
        #endregion

        #region Functions
        void checkForOtherInstance()
        {
            if (Process.GetProcessesByName(Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location)).Length > 1)
            {
                Process.GetCurrentProcess().Kill();
            }
        }

        void placeForm(bool isFormClosed, bool isNotifyIconClicked)
        {
            int xPosition = 1700;

            if (isNotifyIconClicked)
            {
                xPosition = Cursor.Position.X;
            }

            if (xPosition + (Width / 2) <= Screen.PrimaryScreen.WorkingArea.Width - 10 && !isFormClosed)
            {
                Left = xPosition - (Width / 2);
            }

            else
            {
                Left = Screen.PrimaryScreen.WorkingArea.Width - Width - 10;
            }

            Top = Screen.PrimaryScreen.WorkingArea.Height - Height;
        }

        void updateSeries()
        {
            if (Internal.Variables.SeriesList.Count > 0)
            {
                string text = "Series Updater is updating information about your series. Please wait until the process finishes.";
                Context.Notifications.ShowNotification(text, "Updating Series...", 3000);
            }

            else
            {
                string text = "Click on this icon to open Series Updater, add and later delete series.";
                Context.Notifications.ShowNotification(text, "Series Updater", 3000);
            }

            if (Internal.Variables.SeriesList.Count > 0)
            {
                Internal.WebRequests.GetLatestEpisodes();
                Context.IO.WriteSeries();
            }

            applyData(false);
            applySettings();
            updateLabels(false);

            if (Internal.Variables.SeriesList.Count > 0)
            {
                string text = "Series information is downloaded successfully. You can open the program now.";
                Context.Notifications.ShowNotification(text, "Update successful", 3000);
            }

            Context.Notifications.ShowComingSeries(false);
        }

        void applyData(bool isAdd)
        {
            if (Controls.Find("seriesTable", true).Length != 0 && !isAdd)
            {
                TableLayoutPanel seriesTable = (TableLayoutPanel)Controls.Find("seriesTable", true)[0];
                Controls.Remove(seriesTable);
                applyData(isAdd);
                return;
            }

            if (Internal.Variables.SeriesList.Count > 0)
            {
                Label deleteLabel = Context.Controls.CreateLabel("deleteLabel", "", true);
                Label nameHeaderLabel = Context.Controls.CreateLabel("nameHeaderLabel", "Name", true);
                Label lastViewedHeaderLabel = Context.Controls.CreateLabel("lastViewedHeaderLabel", "Last Viewed", true);
                Label lastEpisodeHeaderLabel = Context.Controls.CreateLabel("lastEpisodeHeaderLabel", "Newest", true);

                TableLayoutPanel seriesTable = Context.Controls.CreateTableLayoutPanel(deleteLabel, nameHeaderLabel,
                    lastViewedHeaderLabel, lastEpisodeHeaderLabel);
                Application.OpenForms[0].Controls.Add(seriesTable);
            }

            int forStart = 0;

            if (isAdd)
            {
                forStart = Internal.Variables.SeriesList.Count - 1;
            }

            else
            {
                forStart = 0;
            }

            for (int i = forStart; i < Internal.Variables.SeriesList.Count; i++)
            {
                Label nameLabel = Context.Controls.CreateLabel("name_" + Internal.Variables.SeriesList[i].Id,
                    Internal.Variables.SeriesList[i].Name, false);
                Label lastViewedLabel = Context.Controls.CreateLabel("lastViewed_" + Internal.Variables.SeriesList[i].Id,
                    Internal.Variables.SeriesList[i].LastViewed.ToString(), false);
                lastViewedLabel.Cursor = Cursors.Hand;
                lastViewedLabel.Click += lastViewedLabel_Click;

                Label lastEpLabel = new Label();
                if (Internal.Variables.SeriesList[i].LastEpisode != null)
                {
                    lastEpLabel = Context.Controls.CreateLabel("lastEp_" + Internal.Variables.SeriesList[i].Id,
                        Internal.Variables.SeriesList[i].LastEpisode.ToString(), false);
                }

                else
                {
                    lastEpLabel = Context.Controls.CreateLabel("lastEp_" + Internal.Variables.SeriesList[i].Id, "", false);
                }

                PictureBox deleteImage = Context.Controls.CreatePictureBox("delete_" + Internal.Variables.SeriesList[i].Id,
                    Properties.Resources.delete1, 0, 0, 10, true);
                deleteImage.Click += deleteImage_Click;

                TableLayoutPanel seriesTable = (TableLayoutPanel)Controls.Find("seriesTable", true)[0];
                seriesTable.Controls.Add(deleteImage, 0, i + 1);
                seriesTable.Controls.Add(nameLabel, 1, i + 1);
                seriesTable.Controls.Add(lastViewedLabel, 2, i + 1);
                seriesTable.Controls.Add(lastEpLabel, 3, i + 1);

                if (Internal.Variables.SeriesList[i].LastEpisode.SeasonNumber > Internal.Variables.SeriesList[i].LastViewed.SeasonNumber
                    || (Internal.Variables.SeriesList[i].LastEpisode.SeasonNumber == Internal.Variables.SeriesList[i].LastViewed.SeasonNumber
                        && Internal.Variables.SeriesList[i].LastEpisode.EpisodeNumber > Internal.Variables.SeriesList[i].LastViewed.EpisodeNumber))
                {
                    lastEpLabel.Font = new Font(lastEpLabel.Font, FontStyle.Bold);
                    lastEpLabel.Width = lastEpLabel.PreferredWidth;

                    /*
                    PictureBox newPicture = ControlManagement.NewControls.createPictureBox("new_" + MainProgram.Variables.seriesList[i].id,
                        SeriesUpdater.Properties.Resources.uj_másolat, createSeriesTable.Left + lastEpLabel.Left + lastEpLabel.Width + 2,
                        createSeriesTable.Top + lastEpLabel.Top + 1, 10);
                    newPicture.BringToFront();
                     */
                }
            }

            placeForm(false, false);
        }

        void applySettings()
        {
            Properties.Settings applicationSettings = Properties.Settings.Default;
            sendNotificationsToolStripMenuItem.Checked = applicationSettings.SendNotifications;

            bool isToBeAutorun = applicationSettings.RunOnStartup;
            bool isCurrentlyAutorun = Context.Settings.IsStartupItem();
            if (isToBeAutorun && !isCurrentlyAutorun) Context.Settings.SetAutorun(true);
        }

        void editLastViewedLabel(object sender)
        {
            TableLayoutPanel seriesTable = (TableLayoutPanel)Controls.Find("seriesTable", true)[0];

            try
            {
                Label currLastViewedLabel = (Label)sender;
                int id = Convert.ToInt32(currLastViewedLabel.Name.Split('_')[1]);
                string text = currLastViewedLabel.Text;

                seriesTable.Controls.Remove(currLastViewedLabel);

                TextBox newTextBox = Context.Controls.CreateTextBox("lastViewedBox_" + id, text);
                newTextBox.KeyDown += newTextBox_KeyDown;
                newTextBox.LostFocus += newTextBox_LostFocus;
                seriesTable.Controls.Add(newTextBox, 2, id + 1);

                newTextBox.Focus();
            }

            catch
            {
                TextBox currLastViewedTextBox = new TextBox();

                foreach (Control currControl in seriesTable.Controls)
                {
                    if (currControl.GetType() == typeof(TextBox))
                    {
                        currLastViewedTextBox = (TextBox)currControl;
                    }
                }

                int id = Convert.ToInt32(currLastViewedTextBox.Name.Split('_')[1]);
                string text = currLastViewedTextBox.Text;

                if (!Episode.IsValidEpisodeString(text))
                {
                    Variables.IsAddFormOpened = true;
                    Notifications.ShowError("The format of the given episode number is incorrect. Please give a correct one.",
                    "Invalid episode number");
                    return;
                }

                seriesTable.Controls.Remove(currLastViewedTextBox);

                Label newLabel = Context.Controls.CreateLabel("lastViewed_" + id, text, false);
                newLabel.Cursor = Cursors.Hand;
                newLabel.Click += lastViewedLabel_Click;
                seriesTable.Controls.Add(newLabel, 2, id + 1);

                Internal.Variables.SeriesList[id].LastViewed = new Episode(text);
                Context.IO.WriteSeries();

                Control currLastEpLabel = Controls.Find("lastEp_" + id, true)[0];
                if (Internal.Variables.SeriesList[id].LastEpisode.SeasonNumber > Internal.Variables.SeriesList[id].LastViewed.SeasonNumber
                    || (Internal.Variables.SeriesList[id].LastEpisode.SeasonNumber == Internal.Variables.SeriesList[id].LastViewed.SeasonNumber
                        && Internal.Variables.SeriesList[id].LastEpisode.EpisodeNumber > Internal.Variables.SeriesList[id].LastViewed.EpisodeNumber))
                {
                    currLastEpLabel.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 238);
                }

                else if (Internal.Variables.SeriesList[id].LastEpisode.SeasonNumber < Internal.Variables.SeriesList[id].LastViewed.SeasonNumber
                    || (Internal.Variables.SeriesList[id].LastEpisode.SeasonNumber == Internal.Variables.SeriesList[id].LastViewed.SeasonNumber
                        && Internal.Variables.SeriesList[id].LastEpisode.EpisodeNumber < Internal.Variables.SeriesList[id].LastViewed.EpisodeNumber))
                {
                    Deactivate -= Form1_Deactivate;
                    MessageBox.Show(this, "The number of the last viewed episode has to be smaller than the one of the newest episode.", "Invalid episode number",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Show();
                    Deactivate += Form1_Deactivate;
                    editLastViewedLabel(newLabel);
                }

                else
                {
                    currLastEpLabel.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 238);
                }
            }
        }

        void updateLabels(bool updateLastEpisode)
        {
            if (updateLastEpisode)
            {
                for (int i = 0; i < Internal.Variables.SeriesList.Count; i++)
                {
                    Label currLabel = (Label)Controls.Find("lastEp_" + i, true)[0];
                    currLabel.Text = Internal.Variables.SeriesList[i].ToString();
                }
            }
        }

        private void WireAllControls(Control control)
        {
            foreach (Control currControl in control.Controls)
            {
                currControl.Click += constrols_Click;
                if (currControl.HasChildren)
                {
                    WireAllControls(currControl);
                }
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