using System;
using System.Drawing;
using System.Windows.Forms;

namespace SeriesUpdater
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            MainProgram.Variables.notifyIcon = this.notifyIcon;
            MainProgram.Variables.mainForm = this;

            autorunStripMenuItem.Checked = Context.Settings.isStartupItem();
        }

        #region Form Events
        private void Form1_Load(object sender, EventArgs e)
        {
            Context.IO.readSeries();

            if (MainProgram.Variables.isFirst)
            {
                if (!Context.Settings.isStartupItem() && MessageBox.Show("Szeretné, hogy a program automatikusan elinduljon a Windows indításakor?", "Indítás a Windowszal", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    autorunStripMenuItem.Checked = Context.Settings.setAutorun(true);
                }
            }

            if (MainProgram.Variables.seriesList.Count > 0)
            {
                string text = "Frissítés folyamatban. A frissítés alatt nem tudod megnyitni a programot.";
                Context.Notification.showNotification("Frissítés", text, 3000);
            }

            else
            {
                string text = "Az ikonra kattintva nyithatja meg a programot, adhat hozzá, illetve a későbbiekben törölhet sorozatokat.";
                Context.Notification.showNotification("Sorozat figyelő", text, 3000);
            }

            if (MainProgram.Variables.seriesList.Count > 0)
            {
                MainProgram.WebRequest.getLatestEpisodes(false);
            }

            applyData(false);
            updateLabels(false);

            if (MainProgram.Variables.seriesList.Count > 0)
            {
                string text = "Sikeresen frissültek a legújabb epizódok. Most már megnyithatja a programot.";
                Context.Notification.showNotification("Sikeres frissítés", text, 3000);
            }

            Context.Notification.getComingSeries();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void Form1_Deactivate(object sender, EventArgs e)
        {
            if (!MainProgram.Variables.isAddFormOpened)
            {
                MainProgram.Variables.lastDeactivateTick = Environment.TickCount;
                this.Hide();
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
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
                placeForm(true);
            }
        }

        private void deleteImage_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Biztosan törölni akarod ezt a sorozatot?", "Sorozat törlése", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                PictureBox deleteImage = (PictureBox)sender;
                int id = Convert.ToInt32(deleteImage.Name.Split('_')[1]);
                MainProgram.Variables.seriesList.Remove(MainProgram.Variables.seriesList[id]);
                Controls.Remove(Controls.Find("delete_" + (MainProgram.Variables.seriesList.Count), true)[0]);

                TableLayoutPanel seriesTable = (TableLayoutPanel)Controls.Find("createSeriesTable", true)[0];
                Controls.Remove(seriesTable);

                applyData(false);
                Context.IO.writeSeries();
            }
        }
        #endregion

        #region NotifyIcon Events
        private void notifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            placeForm(false);

            if (e.Button == MouseButtons.Left)
            {
                if (Environment.TickCount - MainProgram.Variables.lastDeactivateTick > 250)
                {
                    this.Show();
                    this.Activate();
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
            this.Close();
        }

        private void autorunStripMenuItem_Click(object sender, EventArgs e)
        {
            autorunStripMenuItem.Checked = Context.Settings.setAutorun(false);
        }
        #endregion

        #region Functions
        void placeForm(bool isFormClosed)
        {
            if (Cursor.Position.X + (this.Width / 2) <= Screen.PrimaryScreen.WorkingArea.Width - 10 && !isFormClosed)
            {
                this.Left = Cursor.Position.X - (this.Width / 2);
            }

            else
            {
                this.Left = Screen.PrimaryScreen.WorkingArea.Width - this.Width - 10;
            }

            Top = Screen.PrimaryScreen.WorkingArea.Height - Height - 10;
        }

        void applyData(bool isAdd)
        {
            if (MainProgram.Variables.seriesList.Count > 0)
            {
                try
                {
                    TableLayoutPanel oldTable = (TableLayoutPanel)Controls.Find("seriesTable", true)[0];
                    oldTable.Controls.Clear();
                    oldTable.RowStyles.Clear();
                }
                catch { }

                Label nameHeaderLabel = Context.Controls.createLabel("nameHeaderLabel", "Név", true);
                Label lastViewedHeaderLabel = Context.Controls.createLabel("lastViewedHeaderLabel", "Legutóbb megtekintett", true);
                Label lastEpisodeHeaderLabel = Context.Controls.createLabel("lastEpisodeHeaderLabel", "Legújabb", true);

                TableLayoutPanel seriesTable = Context.Controls.createTableLayoutPanel(nameHeaderLabel, lastViewedHeaderLabel, lastEpisodeHeaderLabel);
                Application.OpenForms[0].Controls.Add(seriesTable);
            }

            int forStart = 0;

            if (isAdd)
            {
                forStart = MainProgram.Variables.seriesList.Count - 1;
            }

            else
            {
                forStart = 0;
            }

            for (int i = forStart; i < MainProgram.Variables.seriesList.Count; i++)
            {
                Label nameLabel = Context.Controls.createLabel("name_" + MainProgram.Variables.seriesList[i].id, MainProgram.Variables.seriesList[i].name, false);
                Label lastViewedLabel = Context.Controls.createLabel("lastViewed_" + MainProgram.Variables.seriesList[i].id, "S" + MainProgram.Variables.seriesList[i].lastViewed[0] + "E" + MainProgram.Variables.seriesList[i].lastViewed[1], false);
                lastViewedLabel.Cursor = Cursors.Hand;
                lastViewedLabel.Click += lastViewedLabel_Click;

                Label lastEpLabel = new Label();
                if (MainProgram.Variables.seriesList[i].lastEpisode != default(int[]))
                {
                    lastEpLabel = Context.Controls.createLabel("lastEp_" + MainProgram.Variables.seriesList[i].id, "S" + MainProgram.Variables.seriesList[i].lastEpisode[0] + "E" + MainProgram.Variables.seriesList[i].lastEpisode[1], false);
                }

                else
                {
                    lastEpLabel = Context.Controls.createLabel("lastEp_" + MainProgram.Variables.seriesList[i].id, "", false);
                }

                TableLayoutPanel seriesTable = (TableLayoutPanel)Controls.Find("seriesTable", true)[0];
                seriesTable.Controls.Add(nameLabel, 0, i + 1);
                seriesTable.Controls.Add(lastViewedLabel, 1, i + 1);
                seriesTable.Controls.Add(lastEpLabel, 2, i + 1);

                if (MainProgram.Variables.seriesList[i].lastEpisode[0] > MainProgram.Variables.seriesList[i].lastViewed[0] || (MainProgram.Variables.seriesList[i].lastEpisode[0] == MainProgram.Variables.seriesList[i].lastViewed[0] && MainProgram.Variables.seriesList[i].lastEpisode[1] > MainProgram.Variables.seriesList[i].lastViewed[1]))
                {
                    lastEpLabel.Font = new Font(lastEpLabel.Font, FontStyle.Bold | FontStyle.Underline);
                    lastEpLabel.Width = lastEpLabel.PreferredWidth;

                    /*
                    PictureBox newPicture = ControlManagement.NewControls.createPictureBox("new_" + MainProgram.Variables.seriesList[i].id, SeriesUpdater.Properties.Resources.uj_másolat, createSeriesTable.Left + lastEpLabel.Left + lastEpLabel.Width + 2, createSeriesTable.Top + lastEpLabel.Top + 1, 10);
                    newPicture.BringToFront();
                     */
                }

                Control currRowLabel = Controls.Find("name_" + MainProgram.Variables.seriesList[i].id, true)[0];
                PictureBox deleteImage = Context.Controls.createPictureBox("delete_" + MainProgram.Variables.seriesList[i].id, SeriesUpdater.Properties.Resources.delete1, 2, currRowLabel.Top + seriesTable.Top, 15, true);
                deleteImage.Click += deleteImage_Click;
                Controls.Add(deleteImage);
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

                TextBox newTextBox = Context.Controls.createTextBox("lastViewedBox_" + id, text);
                newTextBox.KeyDown += newTextBox_KeyDown;
                newTextBox.LostFocus += newTextBox_LostFocus;
                seriesTable.Controls.Add(newTextBox, 1, id + 1);

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

                Label newLabel = Context.Controls.createLabel("lastViewed_" + id, text, false);
                newLabel.Cursor = Cursors.Hand;
                newLabel.Click += lastViewedLabel_Click;
                seriesTable.Controls.Add(newLabel, 1, id + 1);

                MainProgram.Variables.seriesList[id].lastViewed = MainProgram.ProcessData.convertEpisodeString(text);

                Control currLastEpLabel = Controls.Find("lastEp_" + id, true)[0];
                if (MainProgram.Variables.seriesList[id].lastEpisode[0] > MainProgram.Variables.seriesList[id].lastViewed[0] || (MainProgram.Variables.seriesList[id].lastEpisode[0] == MainProgram.Variables.seriesList[id].lastViewed[0] && MainProgram.Variables.seriesList[id].lastEpisode[1] > MainProgram.Variables.seriesList[id].lastViewed[1]))
                {
                    currLastEpLabel.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(238)));
                }

                else if (MainProgram.Variables.seriesList[id].lastEpisode[0] < MainProgram.Variables.seriesList[id].lastViewed[0] || (MainProgram.Variables.seriesList[id].lastEpisode[0] == MainProgram.Variables.seriesList[id].lastViewed[0] && MainProgram.Variables.seriesList[id].lastEpisode[1] < MainProgram.Variables.seriesList[id].lastViewed[1]))
                {
                    this.Deactivate -= Form1_Deactivate;
                    MessageBox.Show("A legutoljára látott epizód számának kisebbnek kell lennie, mint az utoljára megjelent epizód.", "Érvénytelen epizód", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Deactivate += Form1_Deactivate;
                    editLastViewedLabel(newLabel);
                }

                else
                {
                    currLastEpLabel.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(238)));
                }
            }
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

        void updateLabels(bool updateLastEpisode)
        {
            if (updateLastEpisode)
            {
                for (int i = 0; i < MainProgram.Variables.seriesList.Count; i++)
                {
                    Label currLabel = (Label)Controls.Find("lastEp_" + i, true)[0];
                    currLabel.Text = "S" + MainProgram.Variables.seriesList[i].lastEpisode[0] + "E" + MainProgram.Variables.seriesList[i].lastEpisode[0];
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

    #region Classes
    public class Series
    {
        public int id;
        public string name;
        public int imdbId;
        public int[] lastViewed;
        public int[] lastEpisode;
        public int[] nextEpisode;
        public DateTime nextEpisodeAirDate;

        public Series(int id, string name, int imdbId, int[] lastViewed, int[] lastEpisode, int[] nextEpisode, DateTime nextEpisodeAirDate)
        {
            this.id = id;
            this.name = name;
            this.imdbId = imdbId;
            this.lastViewed = lastViewed;
            this.lastEpisode = lastEpisode;
            this.nextEpisode = nextEpisode;
            this.nextEpisodeAirDate = nextEpisodeAirDate;
        }
    }
    #endregion
}