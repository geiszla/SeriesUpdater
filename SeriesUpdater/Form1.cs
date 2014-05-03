using System;
using System.Windows;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Net;
using Microsoft.Win32;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading;
using System.Data;

namespace SeriesUpdater
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region Form Events
        private void Form1_Load(object sender, EventArgs e)
        {
            Variables.PublicVariables.isFirst = isFirstCheck();

            IO.Input.readInput();

            if (Variables.PublicVariables.isFirst)
            {
                if (!IsStartupItem() && MessageBox.Show("Szeretné, hogy a program automatikusan elinduljon a Windows indításakor?", "Indítás a Windowszal", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    setAutorun(true);
                }
            }

            if (Variables.PublicVariables.seriesList.Count > 0)
            {
                notifyIcon.ShowBalloonTip(3000);
            }

            else
            {
                notifyIcon.BalloonTipTitle = "Sorozat figyelő";
                notifyIcon.BalloonTipText = "Az ikonra kattintva nyithatja meg a programot, adhat hozzá, illetve a későbbiekben törölhet sorozatokat.";
                notifyIcon.ShowBalloonTip(3000);
            }

            if (Variables.PublicVariables.seriesList.Count > 0)
            {
                getLatestEps(false);
            }

            ControlManagement.ModifyControl.applyData(false);
            updateData(false);

            if (Variables.PublicVariables.seriesList.Count > 0)
            {
                notifyIcon.BalloonTipTitle = "Sikeres frissítés";
                notifyIcon.BalloonTipText = "Sikeresen frissültek a legújabb epizódok. Most már megnyithatja a programot.";
                notifyIcon.ShowBalloonTip(3000);
            }

            nextEpNotification();
        }

        void notifyIcon_BalloonTipClicked(object sender, EventArgs e)
        {
            ControlManagement.ModifyControl.placeForm(false);
            this.Show();
            this.Activate();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void Form1_Deactivate(object sender, EventArgs e)
        {
            if (!Variables.PublicVariables.isAddFormOpened)
            {
                Variables.PublicVariables.lastDeactivateTick = Environment.TickCount;
                this.Hide();
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            notifyIcon.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Variables.PublicVariables.isAddFormOpened = true;
            Form2 addForm = new Form2();
            addForm.FormClosed += addForm_FormClosed;
            addForm.ShowDialog();
        }

        private void addForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (Variables.PublicVariables.isAddedSeries)
            {
                ControlManagement.ModifyControl.applyData(true);
                ControlManagement.ModifyControl.placeForm(true);
            }
        }
        #endregion

        #region NotifyIcon Events
        private void notifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            ControlManagement.ModifyControl.placeForm(false);

            if (e.Button == MouseButtons.Left)
            {
                if (Environment.TickCount - Variables.PublicVariables.lastDeactivateTick > 250)
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
            setAutorun(false);
        }
        #endregion

        #region Main functions
        

        public static void deleteImage_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Biztosan törölni akarod ezt a sorozatot?", "Sorozat törlése", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Form thisForm = Application.OpenForms[0];

                PictureBox deleteImage = (PictureBox)sender;
                int id = Convert.ToInt32(deleteImage.Name.Split('_')[1]);
                Variables.PublicVariables.seriesList.Remove(Variables.PublicVariables.seriesList[id]);
                thisForm.Controls.Remove(deleteImage);

                TableLayoutPanel seriesTable = (TableLayoutPanel)thisForm.Controls.Find("createSeriesTable", true)[0];
                thisForm.Controls.Remove(seriesTable);

                ControlManagement.ModifyControl.applyData(false);
            }
        }

        void updateData(bool updateLastEpisode)
        {
            if (updateLastEpisode)
            {
                for (int i = 0; i < Variables.PublicVariables.seriesList.Count; i++)
                {
                    Label currLabel = (Label)Controls.Find("lastEp_" + i, true)[0];
                    currLabel.Text = "S" + Variables.PublicVariables.seriesList[i].lastEpisode[0] + "E" + Variables.PublicVariables.seriesList[i].lastEpisode[0];
                }
            }
        }

        public static void getLatestEps(bool isAdd)
        {
            int forStart;

            if (isAdd)
            {
                forStart = Variables.PublicVariables.seriesList.Count - 1;
            }

            else
            {
                forStart = 0;
            }

            for (int i = 0; i < Variables.PublicVariables.seriesList.Count; i++)
            {
                int id = Convert.ToInt32(Variables.PublicVariables.seriesList[i].imdbId);
                Variables.PublicVariables.seriesList[i].lastEpisode = getLatestEp(id, WebRequests.Core.requestImdb(id, ""), true);
            }
        }

        void nextEpNotification()
        {
            List<Series> todaySeries = new List<Series>();
            List<Series> tomorrowSeries = new List<Series>();

            for (int i = 0; i < Variables.PublicVariables.seriesList.Count; i++)
            {
                int days = Variables.PublicVariables.seriesList[i].nextEpisodeAirDate.DayOfYear - DateTime.Now.DayOfYear;

                if (DateTime.Now.Year >= Variables.PublicVariables.seriesList[i].nextEpisodeAirDate.Year)
                {
                    if (days == 0)
                    {
                        todaySeries.Add(Variables.PublicVariables.seriesList[i]);
                    }

                    else if (days == 1)
                    {
                        tomorrowSeries.Add(Variables.PublicVariables.seriesList[i]);
                    }
                }
            }

            showNewEpisodeNotification("A mai napon", todaySeries);

            Task newNotification = Task.Factory.StartNew(() =>
                {
                    Thread.Sleep(8000);
                    showNewEpisodeNotification("Holnap", tomorrowSeries);
                });
        }

        void setAutorun(bool addAnyway)
        {
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);

            if (!IsStartupItem() || addAnyway)
            {
                if (!File.Exists(Variables.PublicVariables.dataPath + @"\MainProgram.exe") && MessageBox.Show("Szeretné, ha a programról készülne egy másolat? Így a file törlése esetén is lehetséges a Windows indításakor való futtatás.", "Indítás a Windowszal", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    File.Copy(MainProgram.ExecutablePath.ToString(), Variables.PublicVariables.dataPath + @"\MainProgram.exe");
                    registryKey.SetValue("MainProgram", Variables.PublicVariables.dataPath + @"\MainProgram.exe");
                }

                else
                {
                    registryKey.SetValue("MainProgram", MainProgram.ExecutablePath.ToString());
                }

                autorunStripMenuItem.Checked = true;
            }

            else
            {
                registryKey.DeleteValue("MainProgram", false);
                autorunStripMenuItem.Checked = false;
            }
        }
        #endregion

        #region Subfunctions
        int getLabelId(object sender)
        {
            Label currLabel = (Label)sender;

            int currId = Convert.ToInt32(currLabel.Name.Substring(currLabel.Name.LastIndexOf('_') + 1));

            return currId;
        }

        public static string[] getInnerHTMLByClassOrId(int startSearchIndex, string HTMLText, string inputClassOrId, string classOrId)
        {
            int startIndex = HTMLText.IndexOf('>', HTMLText.IndexOf(classOrId + "=\"" + inputClassOrId + "\"", startSearchIndex) + 1);
            string tagName = HTMLText.Substring(HTMLText.LastIndexOf('<', startIndex) + 1, HTMLText.IndexOf(' ', HTMLText.LastIndexOf('<', startIndex)) - HTMLText.LastIndexOf('<', startIndex) - 1);
            int endIndex = HTMLText.IndexOf("</" + tagName + ">", startIndex + 1);
            if (endIndex != -1)
            {
                string innerHTML = HTMLText.Substring(startIndex + 1, endIndex - startIndex - 1);

                string[] returnValues = new string[2];
                returnValues[0] = innerHTML;
                returnValues[1] = Convert.ToString(endIndex);

                return returnValues;
            }

            else
            {
                return new string[2];
            }
        }

        public static int[] getLatestEp(int id, string HTMLText, bool isAdd)
        {
            if (HTMLText == "")
            {
                return new int[]{0, 0};
            }

            string seasonName = getInnerHTMLByClassOrId(0, HTMLText, "episode_top", "id")[0];

            if (seasonName == null)
            {
                MessageBox.Show("Ennek a sorozatnak nincsenek epizódjai. Kérem válasszon egy másikat!", "Érvénytelen sorozat", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new int[] { 0, 0 };
            }

            string previousSeasonNumber = "?season=" + Convert.ToString(Convert.ToInt32(seasonName.Substring(seasonName.IndexOf("Season&nbsp;") + 12)) - 1);
            string nextSeasonNumber = "?season=" + Convert.ToString(Convert.ToInt32(seasonName.Substring(seasonName.IndexOf("Season&nbsp;") + 12)) + 1);

            int startIndex = 0;
            DateTime latestAirDate = new DateTime();
            DateTime nextAirDate = new DateTime();
            int latestDateIndex = 0;
            int nextDateIndex = 0;
            string innerHTML = "";
            while ((innerHTML = getInnerHTMLByClassOrId(startIndex, HTMLText, "airdate", "class")[0]) != default(string))
            {
                int endIndex = Convert.ToInt32(getInnerHTMLByClassOrId(startIndex, HTMLText, "airdate", "class")[1]);
                DateTime airDate = Convert.ToDateTime(innerHTML);
                if (airDate < DateTime.Now)
                {
                    latestAirDate = airDate;
                    latestDateIndex = endIndex;
                }

                else
                {
                    nextAirDate = airDate;
                    nextDateIndex = endIndex;
                    break;
                }

                startIndex = Convert.ToInt32(getInnerHTMLByClassOrId(startIndex, HTMLText, "airdate", "class")[1]);
            }

            if (isAdd)
            {
                for (int i = 0; i < Variables.PublicVariables.seriesList.Count; i++)
                {
                    if (Variables.PublicVariables.seriesList[i].imdbId == id)
                    {
                        if (nextAirDate == default(DateTime))
                        {
                            Variables.PublicVariables.seriesList[i].nextEpisodeAirDate = nextAirDate;
                            Variables.PublicVariables.seriesList[i].nextEpisode = new int[] { 0, 0 };
                        }

                        else
                        {
                            Variables.PublicVariables.seriesList[i].nextEpisodeAirDate = nextAirDate;
                            Variables.PublicVariables.seriesList[i].nextEpisode = getEpisodeByDateIndex(HTMLText, nextDateIndex);
                        }
                    }
                }
            }

            if (latestAirDate == default(DateTime))
            {
                return getLatestEp(id, WebRequests.Core.requestImdb(id, previousSeasonNumber), false);
            }

            else
            {
                return getEpisodeByDateIndex(HTMLText, latestDateIndex);
            }
        }

        static int[] getEpisodeByDateIndex(string HTMLText, int dateIndex)
        {
            int startIndex = HTMLText.IndexOf("<div>", HTMLText.LastIndexOf("<img", dateIndex)) + 4;
            int endIndex = HTMLText.IndexOf("</div>", HTMLText.IndexOf("<div>", HTMLText.LastIndexOf("<img", dateIndex)) + 1);
            string innerHTML = HTMLText.Substring(startIndex + 1, endIndex - startIndex - 1);

            int[] episodeNumber = new int[2];
            episodeNumber[0] = Convert.ToInt32(innerHTML.Split(',')[0].Substring(innerHTML.Split(',')[0].IndexOf('S') + 1));
            episodeNumber[1] = Convert.ToInt32(innerHTML.Split(',')[1].Substring(innerHTML.Split(',')[1].IndexOf("Ep") + 2));

            return episodeNumber;
        }

        public static string getNameFromHTML(string HTMLText)
        {
            string innerHTML = getInnerHTMLByClassOrId(0, HTMLText, "parent", "class")[0];

            Match match = Regex.Match(innerHTML, "\'url\'>(.*)</a>", RegexOptions.IgnoreCase);
            string name = match.Groups[1].Value;

            return name;
        }

        public static int[] convertSeriesString(string inputSeriesString)
        {
            int[] outputInt = new int[2];
            outputInt[0] = Convert.ToInt32(inputSeriesString.Substring(inputSeriesString.IndexOf('S') + 1, inputSeriesString.IndexOf('E') - inputSeriesString.IndexOf('S') - 1));
            outputInt[1] = Convert.ToInt32(inputSeriesString.Substring(inputSeriesString.IndexOf('E') + 1));

            return outputInt;
        }

        void showNewEpisodeNotification(string when, List<Series> comingSeries)
        {
            if (comingSeries.Count == 1)
            {
                notifyIcon.BalloonTipTitle = "Új epizód";
                notifyIcon.BalloonTipText = when + " új rész jelenik meg a következő sorozatból: " + comingSeries[0].name;
                notifyIcon.ShowBalloonTip(5000);
            }

            else if (comingSeries.Count > 1)
            {
                notifyIcon.BalloonTipTitle = "Új epizód";
                notifyIcon.BalloonTipTitle += "(" + comingSeries.Count + ")";
                notifyIcon.BalloonTipText = when + " új részek jelennek meg a következő sorozatokból: ";

                for (int i = 0; i < comingSeries.Count; i++)
                {
                    notifyIcon.BalloonTipText += comingSeries[i].name;

                    if (i != comingSeries.Count - 1)
                    {
                        notifyIcon.BalloonTipText += ", ";
                    }

                    else
                    {
                        notifyIcon.BalloonTipText += ".";
                    }
                }

                notifyIcon.ShowBalloonTip(5000);
            }
        }

        protected override void WndProc(ref Message message)
        {
            const int WM_NCHITTEST = 0x0084;

            if (message.Msg == WM_NCHITTEST)
                return;

            base.WndProc(ref message);
        }

        bool IsStartupItem()
        {
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);

            if (registryKey.GetValue("MainProgram") == null)
            {
                autorunStripMenuItem.Checked = false;
                return false;
            }

            else
            {
                autorunStripMenuItem.Checked = true;
                return true;
            }
        }

        static bool isFirstCheck()
        {
            if (Directory.Exists(Variables.PublicVariables.dataPath))
            {
                return false;
            }

            else
            {
                return true;
            }
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