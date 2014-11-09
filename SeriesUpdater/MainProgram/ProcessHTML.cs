using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SeriesUpdater.MainProgram
{
    class ProcessHTML
    {
        static DateTime currNextAirDate = new DateTime();

        public static void getInnerHTMLByTagName(int startSearchIndex, string HTMLText, string tagName)
        {

        }

        public static string[] getInnerHTMLByAttribute(int startSearchIndex, string HTMLText, string attributeValue, string attribute)
        {
            int startIndex = HTMLText.IndexOf('>', HTMLText.IndexOf(attribute + "=\"" + attributeValue + "\"", startSearchIndex) + 1);
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

        public static int[] getEpisodeByDateIndex(string HTMLText, int dateIndex)
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
            string innerHTML = getInnerHTMLByAttribute(0, HTMLText, "parent", "class")[0];

            Match match = Regex.Match(innerHTML, "\'url\'>(.*)</a>", RegexOptions.IgnoreCase);
            string name = match.Groups[1].Value;

            return name;
        }

        public static int[] getLatestEpisodeFromHTML(string id, string HTMLText, bool isAdd)
        {
            if (HTMLText == "")
            {
                return new int[] { 0, 0 };
            }

            string seasonName = MainProgram.ProcessHTML.getInnerHTMLByAttribute(0, HTMLText, "episode_top", "id")[0];

            if (seasonName == null)
            {
                MessageBox.Show("Ennek a sorozatnak nincsenek epizódjai. Kérem válasszon egy másikat!", "Érvénytelen sorozat", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new int[] { 0, 0 };
            }

            string previousSeasonNumber = "?season=" + Convert.ToString(Convert.ToInt32(seasonName.Substring(seasonName.IndexOf("Season&nbsp;") + 12)) - 1);
            string nextSeasonNumber = "?season=" + Convert.ToString(Convert.ToInt32(seasonName.Substring(seasonName.IndexOf("Season&nbsp;") + 12)) + 1);

            int startIndex = 0;
            DateTime latestAirDate = new DateTime();
            DateTime nextAirDate = currNextAirDate;
            int latestDateIndex = 0;
            int nextDateIndex = 0;
            string innerHTML = "";
            while ((innerHTML = MainProgram.ProcessHTML.getInnerHTMLByAttribute(startIndex, HTMLText, "airdate", "class")[0]) != default(string))
            {
                int endIndex = Convert.ToInt32(MainProgram.ProcessHTML.getInnerHTMLByAttribute(startIndex, HTMLText, "airdate", "class")[1]);
                DateTime airDate = new DateTime();
                try
                {
                    airDate = Convert.ToDateTime(innerHTML);
                }
                catch
                {
                    airDate = Convert.ToDateTime(innerHTML.Split('\n')[1] + ".12.31");
                }

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

                startIndex = Convert.ToInt32(MainProgram.ProcessHTML.getInnerHTMLByAttribute(startIndex, HTMLText, "airdate", "class")[1]);
            }

            if (nextAirDate == default(DateTime))
            {
                string url = "http://www.imdb.com/title/" + "tt" + id + "/episodes" + nextSeasonNumber;
                getLatestEpisodeFromHTML(id, MainProgram.WebRequest.requestPage(url), isAdd);
            }

            else
            {
                currNextAirDate = nextAirDate;
            }

            if (isAdd)
            {
                for (int i = 0; i < MainProgram.Variables.seriesList.Count; i++)
                {
                    if (MainProgram.Variables.seriesList[i].imdbId == Convert.ToString(id))
                    {
                        MainProgram.Variables.seriesList[i].nextEpisodeAirDate = nextAirDate;
                        MainProgram.Variables.seriesList[i].nextEpisode = MainProgram.ProcessHTML.getEpisodeByDateIndex(HTMLText, nextDateIndex);
                    }
                }
            }

            if (latestAirDate == default(DateTime))
            {
                string url = "http://www.imdb.com/title/" + "tt" + id + "/episodes" + previousSeasonNumber;
                return getLatestEpisodeFromHTML(id, MainProgram.WebRequest.requestPage(url), false);
            }

            else
            {
                return MainProgram.ProcessHTML.getEpisodeByDateIndex(HTMLText, latestDateIndex);
            }
        }
    }
}
