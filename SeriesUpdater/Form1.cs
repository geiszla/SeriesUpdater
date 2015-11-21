using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace SeriesUpdater
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            checkForOtherInstance();
            InitializeComponent();

            MainProgram.Variables.notifyIcon = this.notifyIcon;
            MainProgram.Variables.mainForm = this;

            RunOnStartupToolStripMenuItem.Checked = Context.Settings.IsStartupItem();
            WireAllControls(this);
        }

        #region Form Events
        private void Form1_Load(object sender, EventArgs e)
        {
            Context.IO.ReadSettings();
            Context.IO.ReadSeries();

            if (MainProgram.Variables.isFirst)
            {
                if (!Context.Settings.IsStartupItem() && MessageBox.Show("Szeretné, hogy a program automatikusan elinduljon a Windows indításakor?",
                    "Indítás a Windowszal", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    RunOnStartupToolStripMenuItem.Checked = Context.Settings.SetAutorun(true);
                }

                if (!Directory.Exists(MainProgram.Variables.DataFolderPath))
                {
                    Directory.CreateDirectory(MainProgram.Variables.DataFolderPath);
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
            if (!MainProgram.Variables.isAddFormOpened)
            {
                MainProgram.Variables.lastDeactivateTick = Environment.TickCount;
                Hide();
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Context.IO.WriteSettings();
            notifyIcon.Visible = false;
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            MainProgram.Variables.isAddFormOpened = true;
            Form2 addForm = new Form2();
            addForm.FormClosed += addForm_FormClosed;
            addForm.ShowDialog();
        }

        private void addForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (MainProgram.Variables.isAddedSeries)
            {
                applyData(true);
                placeForm(true, false);
            }
        }

        private void deleteImage_Click(object sender, EventArgs e)
        {
            Deactivate -= Form1_Deactivate;
            if (MessageBox.Show("Biztosan törölni akarod ezt a sorozatot?", "Sorozat törlése",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                PictureBox deleteImage = (PictureBox)sender;
                int id = Convert.ToInt32(deleteImage.Name.Split('_')[1]);
                Label nameLabel = (Label)Controls.Find("name_" + id, true)[0];
                MainProgram.Variables.SeriesList.Remove(MainProgram.Variables.SeriesList.Where(x => x.Name == nameLabel.Text).FirstOrDefault());

                applyData(false);
                Context.IO.WriteSeries();
            }

            Deactivate += Form1_Deactivate;
        }

        private void newTextBox_LostFocus(object sender, EventArgs e)
        {
            if (!MainProgram.Variables.keyDownFired)
            {
                editLastViewedLabel(sender);
            }

            else
            {
                MainProgram.Variables.keyDownFired = false;
            }
        }

        void newTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                MainProgram.Variables.keyDownFired = true;
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
                if (Environment.TickCount - MainProgram.Variables.lastDeactivateTick > 250)
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
            RunOnStartupToolStripMenuItem.Checked = Context.Settings.SetAutorun(false);
        }

        private void notificationContextMenuItem_Click(object sender, EventArgs e)
        {
            Context.Settings.ChangeSettings(0, sendNotificationsToolStripMenuItem.Checked.ToString());
        }

        private void customLanguageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string message = "Ahhoz hogy megváltoztasd a megjelenő IMDB címek nyelvét, először be kell ezt állítanod a saját IMDB fiókodban (https://secure.imdb.com/register-imdb/siteprefs) a címek nyelvét, ezután itt be kell jelentkezned a Google fiókodat használva. Szeretnél most bejelentkezni?";

            if (MessageBox.Show(message, "IMDB Nyelv módosítás", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                Form4 loginForm = new Form4();
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

            Top = Screen.PrimaryScreen.WorkingArea.Height - Height - 10;
        }

        void updateSeries()
        {
            if (MainProgram.Variables.SeriesList.Count > 0)
            {
                string text = "Frissítés folyamatban. A frissítés alatt nem tudod megnyitni a programot.";
                Context.Notification.ShowNotification("Frissítés", text, 3000);
            }

            else
            {
                string text = "Az ikonra kattintva nyithatja meg a programot, adhat hozzá, illetve a későbbiekben törölhet sorozatokat.";
                Context.Notification.ShowNotification("Sorozat figyelő", text, 3000);
            }

            if (MainProgram.Variables.SeriesList.Count > 0)
            {
                MainProgram.WebRequest.GetLatestEpisodes();
                Context.IO.WriteSeries();
            }

            applyData(false);
            applySettings();
            updateLabels(false);

            if (MainProgram.Variables.SeriesList.Count > 0)
            {
                string text = "Sikeresen frissültek a legújabb epizódok. Most már megnyithatja a programot.";
                Context.Notification.ShowNotification("Sikeres frissítés", text, 3000);
            }

            Context.Notification.GetComingSeries(false);
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

            if (MainProgram.Variables.SeriesList.Count > 0)
            {
                Label deleteLabel = Context.Controls.CreateLabel("deleteLabel", "", true);
                Label nameHeaderLabel = Context.Controls.CreateLabel("nameHeaderLabel", "Név", true);
                Label lastViewedHeaderLabel = Context.Controls.CreateLabel("lastViewedHeaderLabel", "Legutóbb megtekintett", true);
                Label lastEpisodeHeaderLabel = Context.Controls.CreateLabel("lastEpisodeHeaderLabel", "Legújabb", true);

                TableLayoutPanel seriesTable = Context.Controls.CreateTableLayoutPanel(deleteLabel, nameHeaderLabel,
                    lastViewedHeaderLabel, lastEpisodeHeaderLabel);
                Application.OpenForms[0].Controls.Add(seriesTable);
            }

            int forStart = 0;

            if (isAdd)
            {
                forStart = MainProgram.Variables.SeriesList.Count - 1;
            }

            else
            {
                forStart = 0;
            }

            for (int i = forStart; i < MainProgram.Variables.SeriesList.Count; i++)
            {
                Label nameLabel = Context.Controls.CreateLabel("name_" + MainProgram.Variables.SeriesList[i].Id,
                    MainProgram.Variables.SeriesList[i].Name, false);
                Label lastViewedLabel = Context.Controls.CreateLabel("lastViewed_" + MainProgram.Variables.SeriesList[i].Id,
                    MainProgram.Variables.SeriesList[i].LastViewed.ToString(), false);
                lastViewedLabel.Cursor = Cursors.Hand;
                lastViewedLabel.Click += lastViewedLabel_Click;

                Label lastEpLabel = new Label();
                if (MainProgram.Variables.SeriesList[i].LastEpisode != null)
                {
                    lastEpLabel = Context.Controls.CreateLabel("lastEp_" + MainProgram.Variables.SeriesList[i].Id,
                        MainProgram.Variables.SeriesList[i].LastEpisode.ToString(), false);
                }

                else
                {
                    lastEpLabel = Context.Controls.CreateLabel("lastEp_" + MainProgram.Variables.SeriesList[i].Id, "", false);
                }

                PictureBox deleteImage = Context.Controls.CreatePictureBox("delete_" + MainProgram.Variables.SeriesList[i].Id,
                    Properties.Resources.delete1, 0, 0, 10, true);
                deleteImage.Click += deleteImage_Click;

                TableLayoutPanel seriesTable = (TableLayoutPanel)Controls.Find("seriesTable", true)[0];
                seriesTable.Controls.Add(deleteImage, 0, i + 1);
                seriesTable.Controls.Add(nameLabel, 1, i + 1);
                seriesTable.Controls.Add(lastViewedLabel, 2, i + 1);
                seriesTable.Controls.Add(lastEpLabel, 3, i + 1);

                if (MainProgram.Variables.SeriesList[i].LastEpisode.SeasonNumber > MainProgram.Variables.SeriesList[i].LastViewed.SeasonNumber
                    || (MainProgram.Variables.SeriesList[i].LastEpisode.SeasonNumber == MainProgram.Variables.SeriesList[i].LastViewed.SeasonNumber
                        && MainProgram.Variables.SeriesList[i].LastEpisode.EpisodeNumber > MainProgram.Variables.SeriesList[i].LastViewed.EpisodeNumber))
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
            foreach (string[] option in Context.Settings.GlobalSettings)
            {
                ToolStripMenuItem currOption = (ToolStripMenuItem)notifyIconContextMenu.Items.Find(option[0] + "ToolStripMenuItem", true)[0];

                if (option[0] == "RunOnStartup")
                {
                    if (Convert.ToBoolean(option[1]) && !Context.Settings.IsStartupItem())
                    {
                        Context.Settings.SetAutorun(true);
                    }

                    else if (!Convert.ToBoolean(option[1]) && Context.Settings.IsStartupItem())
                    {
                        Context.Settings.SetAutorun(false);
                    }
                }
            }
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

                seriesTable.Controls.Remove(currLastViewedTextBox);

                Label newLabel = Context.Controls.CreateLabel("lastViewed_" + id, text, false);
                newLabel.Cursor = Cursors.Hand;
                newLabel.Click += lastViewedLabel_Click;
                seriesTable.Controls.Add(newLabel, 2, id + 1);

                MainProgram.Variables.SeriesList[id].LastViewed = new Episode(text);
                Context.IO.WriteSeries();

                Control currLastEpLabel = Controls.Find("lastEp_" + id, true)[0];
                if (MainProgram.Variables.SeriesList[id].LastEpisode.SeasonNumber > MainProgram.Variables.SeriesList[id].LastViewed.SeasonNumber
                    || (MainProgram.Variables.SeriesList[id].LastEpisode.SeasonNumber == MainProgram.Variables.SeriesList[id].LastViewed.SeasonNumber
                        && MainProgram.Variables.SeriesList[id].LastEpisode.EpisodeNumber > MainProgram.Variables.SeriesList[id].LastViewed.EpisodeNumber))
                {
                    currLastEpLabel.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 238);
                }

                else if (MainProgram.Variables.SeriesList[id].LastEpisode.SeasonNumber < MainProgram.Variables.SeriesList[id].LastViewed.SeasonNumber
                    || (MainProgram.Variables.SeriesList[id].LastEpisode.SeasonNumber == MainProgram.Variables.SeriesList[id].LastViewed.SeasonNumber
                        && MainProgram.Variables.SeriesList[id].LastEpisode.EpisodeNumber < MainProgram.Variables.SeriesList[id].LastViewed.EpisodeNumber))
                {
                    Deactivate -= Form1_Deactivate;
                    MessageBox.Show(this, "A legutoljára látott epizód számának kisebbnek kell lennie, mint az utoljára megjelent epizód.", "Érvénytelen epizód",
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
                for (int i = 0; i < MainProgram.Variables.SeriesList.Count; i++)
                {
                    Label currLabel = (Label)Controls.Find("lastEp_" + i, true)[0];
                    currLabel.Text = MainProgram.Variables.SeriesList[i].ToString();
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