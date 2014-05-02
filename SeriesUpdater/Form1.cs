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

namespace SeriesUpdater
{
    public partial class Form1 : Form
    {
        #region Public variables
        int lastDeactivateTick;
        public static bool isAddFormOpened = false;
        public static List<Series> seriesList = new List<Series>();
        public static string dataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\SeriesUpdater";
        public static bool isFirst = isFirstCheck();
        public static bool isAddedSeries = false;
        #endregion

        #region Initialization
        public Form1()
        {
            InitializeComponent();
        }
        #endregion

        #region Form Events
        private void Form1_Load(object sender, EventArgs e)
        {
            readSeries();

            if (isFirst)
            {
                if (!IsStartupItem() && MessageBox.Show("Szeretné, hogy a program automatikusan elinduljon a Windows indításakor?", "Indítás a Windowszal", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    setAutorun(true);
                }
            }

            if (seriesList.Count > 0)
            {
                notifyIcon.ShowBalloonTip(3000);
            }

            else
            {
                notifyIcon.BalloonTipTitle = "Sorozat figyelő";
                notifyIcon.BalloonTipText = "Az ikonra kattintva nyithatja meg a programot, adhat hozzá, illetve a későbbiekben törölhet sorozatokat.";
                notifyIcon.ShowBalloonTip(3000);
            }

            if (seriesList.Count > 0)
            {
                getLatestEps(false);
            }

            applyData(false);
            updateData(false);

            if (seriesList.Count > 0)
            {
                notifyIcon.BalloonTipTitle = "Sikeres frissítés";
                notifyIcon.BalloonTipText = "Sikeresen frissültek a legújabb epizódok. Most már megnyithatja a programot.";
                notifyIcon.ShowBalloonTip(3000);
            }

            nextEpNotification();
        }

        void notifyIcon_BalloonTipClicked(object sender, EventArgs e)
        {
            placeForm(false);
            this.Show();
            this.Activate();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void Form1_Deactivate(object sender, EventArgs e)
        {
            if (!isAddFormOpened)
            {
                lastDeactivateTick = Environment.TickCount;
                this.Hide();
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            notifyIcon.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            isAddFormOpened = true;
            Form2 addForm = new Form2();
            addForm.FormClosed += addForm_FormClosed;
            addForm.ShowDialog();
        }

        private void addForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (isAddedSeries)
            {
                applyData(true);
                placeForm(true);
            }
        }
        #endregion

        #region NotifyIcon Events
        private void notifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            placeForm(false);

            if (e.Button == MouseButtons.Left)
            {
                if (Environment.TickCount - lastDeactivateTick > 250)
                {
                    this.Show();
                    this.Activate();
                }
            }
        }
        #endregion

        #region Context menu events
        private void notifyIconContextMenu_MouseUp(object sender, MouseEventArgs e)
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

            this.Top = Screen.PrimaryScreen.WorkingArea.Height - this.Height - 10;
        }

        public static void readSeries()
        {
            if (File.Exists(dataPath + @"\series.dat"))
            {
                StreamReader readData = new StreamReader(dataPath + @"\series.dat");

                int whileCount = 0;
                while (readData.Peek() > -1)
                {
                    string[] currRow = readData.ReadLine().Split(';');

                    if (currRow.Length > 1)
                    {
                        Series currSeries = new Series();
                        currSeries.id = whileCount;
                        currSeries.name = currRow[0];
                        currSeries.englishName = currRow[1];
                        currSeries.lastViewed = convertSeriesString(currRow[2]);
                        currSeries.lastEpisode = convertSeriesString(currRow[3]);
                        currSeries.nextEpisode = convertSeriesString(currRow[4]);
                        currSeries.nextEpisodeAirDate = Convert.ToDateTime(currRow[5]);
                        Form1.seriesList.Add(currSeries);
                    }

                    whileCount++;
                }

                readData.Close();
            }
        }

        void applyData(bool isAdd)
        {
            if (seriesList.Count > 0)
            {
                seriesTable.Visible = true;
            }

            int forStart = 0;

            if (isAdd)
            {
                forStart = seriesList.Count - 1;
            }

            else
            {
                forStart = 0;
            }

            for (int i = forStart; i < seriesList.Count; i++)
            {
                Label nameLabel = new Label();
                nameLabel.Text = seriesList[i].name;
                nameLabel.Name = "name_" + seriesList[i].id;
                nameLabel.Width = nameLabel.PreferredWidth;
                nameLabel.Anchor = AnchorStyles.Top;
                nameLabel.AutoEllipsis = true;
                nameLabel.MaximumSize = new System.Drawing.Size(150, 50);

                Label lastViewedLabel = new Label();
                lastViewedLabel.Text = "S" + seriesList[i].lastViewed[0] + "E" + seriesList[i].lastViewed[1];
                lastViewedLabel.Name = "lastViewed_" + seriesList[i].id;
                lastViewedLabel.Width = lastViewedLabel.PreferredWidth;
                lastViewedLabel.Anchor = AnchorStyles.Top;
                lastViewedLabel.MaximumSize = new System.Drawing.Size(150, 50);

                Label lastEpLabel = new Label();
                if (seriesList[i].lastEpisode != default(int[]))
                {
                    lastEpLabel.Text = "S" + seriesList[i].lastEpisode[0] + "E" + seriesList[i].lastEpisode[1];
                }
                else
                {
                    lastEpLabel.Text = "";
                }
                lastEpLabel.Name = "lastEp_" + seriesList[i].id;
                lastEpLabel.Width = lastEpLabel.PreferredWidth;
                lastEpLabel.Anchor = AnchorStyles.Top;
                lastEpLabel.MaximumSize = new System.Drawing.Size(150, 50);

                seriesTable.Controls.Add(nameLabel, 0, i + 1);
                seriesTable.Controls.Add(lastViewedLabel, 1, i + 1);
                seriesTable.Controls.Add(lastEpLabel, 2, i + 1);

                if (seriesList[i].lastEpisode[0] > seriesList[i].lastViewed[0] || (seriesList[i].lastEpisode[0] == seriesList[i].lastViewed[0] && seriesList[i].lastEpisode[1] > seriesList[i].lastViewed[1]))
                {

                    lastEpLabel.Font = new Font(lastEpLabel.Font, FontStyle.Bold | FontStyle.Underline);
                    lastEpLabel.Width = lastEpLabel.PreferredWidth;

                    /*
                    PictureBox newPicture = new PictureBox();
                    newPicture.Name = "new_" + seriesList[i].id;
                    newPicture.Image = SeriesUpdater.Properties.Resources.uj_másolat;
                    newPicture.Anchor = (AnchorStyles.Top | AnchorStyles.Left);
                    newPicture.Height = 10;
                    newPicture.Width = 10;
                    newPicture.Top = seriesTable.Top + lastEpLabel.Top + 1;
                    newPicture.Left = seriesTable.Left + lastEpLabel.Left + lastEpLabel.Width + 2;

                    Controls.Add(newPicture);
                    newPicture.BringToFront();
                     */
                }

                PictureBox deleteImage = new PictureBox();
                deleteImage.Name = "delete_" + seriesList[i].id;
                deleteImage.Image = SeriesUpdater.Properties.Resources.delete_icon_hi3;
                deleteImage.Height = 10;
                deleteImage.Width = 10;
                deleteImage.Left = 5;
                deleteImage.Cursor = Cursors.Hand;

                var currRowLabel = this.Controls.Find("name_" + seriesList[i].id, true)[0];
                deleteImage.Top = currRowLabel.Top + seriesTable.Top + 2;
                deleteImage.Visible = false;

                Controls.Add(deleteImage);
            }
        }

        void updateData(bool updateLastEpisode)
        {
            if (updateLastEpisode)
            {
                for (int i = 0; i < seriesList.Count; i++)
                {
                    Label currLabel = (Label)Controls.Find("lastEp_" + i, true)[0];
                    currLabel.Text = "S" + seriesList[i].lastEpisode[0] + "E" + seriesList[i].lastEpisode[0];
                }
            }
        }

        public static void getLatestEps(bool isAdd)
        {
            int forStart;

            if (isAdd)
            {
                forStart = seriesList.Count - 1;
            }

            else
            {
                forStart = 0;
            }

            for (int i = 0; i < seriesList.Count; i++)
            {
                int id = Convert.ToInt32(seriesList[i].englishName);
                seriesList[i].lastEpisode = getLatestEp(id, requestImdb(id, ""), true);
            }
        }

        void nextEpNotification()
        {
            List<Series> todaySeries = new List<Series>();
            List<Series> tomorrowSeries = new List<Series>();

            for (int i = 0; i < seriesList.Count; i++)
            {
                int days = seriesList[i].nextEpisodeAirDate.DayOfYear - DateTime.Now.DayOfYear;

                if (DateTime.Now.Year >= seriesList[i].nextEpisodeAirDate.Year)
                {
                    if (days == 0)
                    {
                        todaySeries.Add(seriesList[i]);
                    }

                    else if (days == 1)
                    {
                        tomorrowSeries.Add(seriesList[i]);
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
                if (!File.Exists(dataPath + @"\SeriesUpdater.exe") && MessageBox.Show("Szeretné, ha a programról készülne egy másolat? Így a file törlése esetén is lehetséges a Windows indításakor való futtatás.", "Indítás a Windowszal", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    File.Copy(Application.ExecutablePath.ToString(), dataPath + @"\SeriesUpdater.exe");
                    registryKey.SetValue("SeriesUpdater", dataPath + @"\SeriesUpdater.exe");
                }

                else
                {
                    registryKey.SetValue("SeriesUpdater", Application.ExecutablePath.ToString());
                }

                autorunStripMenuItem.Checked = true;
            }

            else
            {
                registryKey.DeleteValue("SeriesUpdater", false);
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

        public static string requestImdb(int id, string seasonNumber)
        {
            string url = "http://www.imdb.com/title/" + "tt" + id + "/episodes" + seasonNumber;

            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
            myRequest.Method = "GET";

            try
            {
                WebResponse myResponse = myRequest.GetResponse();
                StreamReader sr = new StreamReader(myResponse.GetResponseStream(), System.Text.Encoding.UTF8);
                string HTMLText = sr.ReadToEnd();
                sr.Close();
                myResponse.Close();

                return HTMLText;
            }

            catch
            {
                MessageBox.Show("Az azonosítás sikertelen volt. Nem megfelelő az IMDB azonosító, vagy nincs kapcsolat a szerverrel.", "Sikertelen azonosítás", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "";
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
                MessageBox.Show("Ennek a sorozatnak nincsenek részei, kérem válasszon egy másikat.", "Érvénytelen sorozat", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                for (int i = 0; i < seriesList.Count; i++)
                {
                    if (seriesList[i].englishName == id.ToString())
                    {
                        if (nextAirDate == default(DateTime))
                        {
                            seriesList[i].nextEpisodeAirDate = nextAirDate;
                            seriesList[i].nextEpisode = new int[] { 0, 0 };
                        }

                        else
                        {
                            seriesList[i].nextEpisodeAirDate = nextAirDate;
                            seriesList[i].nextEpisode = getEpisodeByDateIndex(HTMLText, nextDateIndex);
                        }
                    }
                }
            }

            if (latestAirDate == default(DateTime))
            {
                return getLatestEp(id, requestImdb(id, previousSeasonNumber), false);
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

            if (registryKey.GetValue("SeriesUpdater") == null)
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
            if (Directory.Exists(dataPath))
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
        public string englishName;
        public int[] lastViewed;
        public int[] lastEpisode;
        public int[] nextEpisode;
        public DateTime nextEpisodeAirDate;
    }
    #endregion
}