using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SeriesUpdater.MainProgram
{
    class ProcessHTML
    {
        public static DateTime CurrNextAirDate = new DateTime();
        public static int CurrNextDateIndex = 0;

        public static string[] GetInnerHTMLByAttribute(int StartSearchIndex, string HTMLText, string AttributeValue, string Attribute)
        {
            int startIndex = HTMLText.IndexOf('>', HTMLText.IndexOf(Attribute + "=\"" + AttributeValue + "\"", StartSearchIndex) + 1);
            int startTagIndex = HTMLText.LastIndexOf('<', startIndex);
            string tagName = HTMLText.Substring(startTagIndex + 1, HTMLText.IndexOf(' ', startTagIndex) - startTagIndex - 1);

            int endIndex = HTMLText.IndexOf("</" + tagName + ">", startIndex + 1);
            if (endIndex == -1) return new string[2];

            string innerHTML = HTMLText.Substring(startIndex + 1, endIndex - startIndex - 1);

            string[] returnValues = new string[2];
            returnValues[0] = innerHTML;
            returnValues[1] = Convert.ToString(endIndex);

            return returnValues;
        }

        public static Episode GetEpisodeByDateIndex(string HTMLText, int DateIndex)
        {
            int startIndex = HTMLText.IndexOf("<div>", HTMLText.LastIndexOf("<img", DateIndex)) + 4;
            int endIndex = HTMLText.IndexOf("</div>", HTMLText.IndexOf("<div>", HTMLText.LastIndexOf("<img", DateIndex)) + 1);
            string innerHTML = HTMLText.Substring(startIndex + 1, endIndex - startIndex - 1);

            int seasonNumber = Convert.ToInt32(innerHTML.Split(',')[0].Substring(innerHTML.Split(',')[0].IndexOf('S') + 1));
            int episodeNumber = Convert.ToInt32(innerHTML.Split(',')[1].Substring(innerHTML.Split(',')[1].IndexOf("Ep") + 2));

            return new Episode(seasonNumber, episodeNumber);
        }

        public static string GetNameFromHTML(string HTMLText)
        {
            string innerHTML = GetInnerHTMLByAttribute(0, HTMLText, "parent", "class")[0];

            Match match = Regex.Match(innerHTML, "\'url\'>(.*)</a>", RegexOptions.IgnoreCase);
            string name = match.Groups[1].Value;

            return name;
        }

        public static Episode GetLatestEpisodeFromHTML(string id, string HTMLText, bool isAdd)
        {
            if (HTMLText == "") return new Episode();

            string seasonName = GetInnerHTMLByAttribute(0, HTMLText, "episode_top", "id")[0];
            if (seasonName == null)
            {
                MessageBox.Show("This series doesn't have any episodes. Please choose another one!", "Invalid series", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new Episode();
            }

            string previousSeasonNumber = "?season=" + Convert.ToString(Convert.ToInt32(seasonName.Substring(seasonName.IndexOf("Season&nbsp;") + 12)) - 1);

            int startIndex = 0;
            DateTime latestAirDate = new DateTime();
            DateTime nextAirDate = CurrNextAirDate;
            int nextDateIndex = CurrNextDateIndex;
            int dateKnown = 3;
            int latestDateIndex = 0;
            string innerHTML = "";

            while ((innerHTML = GetInnerHTMLByAttribute(startIndex, HTMLText, "airdate", "class")[0]) != default(string))
            {
                int endIndex = Convert.ToInt32(GetInnerHTMLByAttribute(startIndex, HTMLText, "airdate", "class")[1]);
                DateTime airDate = new DateTime();
                if (innerHTML.Trim() != "")
                {
                    try
                    {
                        airDate = Convert.ToDateTime(innerHTML);
                    }

                    catch
                    {
                        airDate = Convert.ToDateTime(innerHTML.Split('\n')[1] + ".12.31");
                        dateKnown = 1;
                    }
                }

                if (airDate < DateTime.Now)
                {
                    latestAirDate = airDate;
                    latestDateIndex = endIndex;
                }

                else if (nextAirDate == null || airDate < nextAirDate)
                {
                    nextAirDate = airDate;
                    nextDateIndex = endIndex;
                    break;
                }

                startIndex = Convert.ToInt32(GetInnerHTMLByAttribute(startIndex, HTMLText, "airdate", "class")[1]);
            }

            if (nextAirDate != default(DateTime))
            {
                if (CurrNextAirDate != nextAirDate)
                {
                    if (!isAdd)
                    {
                        foreach (Series currSeries in Variables.SeriesList)
                        {
                            if (currSeries.ImdbId == id)
                            {
                                currSeries.NextEpisodeAirDate = nextAirDate;
                                currSeries.DateKnown = dateKnown;
                                currSeries.NextEpisode = GetEpisodeByDateIndex(HTMLText, nextDateIndex);
                            }
                        }
                    }

                    else
                    {
                        Variables.selectedSeries.NextEpisodeAirDate = nextAirDate;
                        Variables.selectedSeries.DateKnown = dateKnown;
                        Variables.selectedSeries.NextEpisode = GetEpisodeByDateIndex(HTMLText, nextDateIndex);
                    }
                }

                CurrNextAirDate = nextAirDate;
                CurrNextDateIndex = nextDateIndex;
            }

            if (latestAirDate == null)
            {
                string url = "http://www.imdb.com/title/" + "tt" + id + "/episodes" + previousSeasonNumber;
                return GetLatestEpisodeFromHTML(id, WebRequest.RequestPage(url), isAdd);
            }

            else
            {
                /*
                for (int i = 0; i < MainProgram.Variables.seriesList.Count; i++)
                {
                    if (MainProgram.Variables.seriesList[i].imdbId == id)
                    {
                        string name = MainProgram.WebRequest.getNameById(MainProgram.Variables.seriesList[i].imdbId);
                        string dateTime = MainProgram.WebRequest.getAirTimeByName(name);

                        if (Convert.ToDateTime(dateTime) != default(DateTime))
                        {
                            TimeSpan time = TimeSpan.Parse(dateTime);
                            MainProgram.Variables.seriesList[i].nextEpisodeAirDate += time;
                            MainProgram.Variables.seriesList[i].dateKnown = 6;
                        }
                    }
                }
                 */

                return GetEpisodeByDateIndex(HTMLText, latestDateIndex);
            }
        }
    }
}
