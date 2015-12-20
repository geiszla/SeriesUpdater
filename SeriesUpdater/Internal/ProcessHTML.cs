using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SeriesUpdater.Internal
{
    class ProcessHTML
    {
        public static Tuple<string, int> GetInnerHTMLByAttribute(string AttributeValue, string Attribute,
            string HTMLText, int SearchStartIndex = 0)
        {
            string regexString = @"<[a-z0-9]+ " + Attribute + "=\"" + AttributeValue + @".*?>([^<]*)";
            Regex innerHTMLRegex = new Regex(regexString);

            if (!innerHTMLRegex.IsMatch(HTMLText, SearchStartIndex)) return null;

            Match regexMatch = innerHTMLRegex.Match(HTMLText, SearchStartIndex);
            string innerHTML = regexMatch.Groups[1].Value;

            return new Tuple<string, int>(innerHTML, regexMatch.Index + innerHTML.Length);
        }

        public static Episode GetEpisodeByDateIndex(int DateIndex, string HTMLText)
        {
            Stopwatch stopWatch = Stopwatch.StartNew();

            Regex episodeRegex = new Regex("<div>(S[^<]*)");
            string episodeString = episodeRegex.Match(HTMLText, DateIndex, HTMLText.Length).Groups[1].Value;

            stopWatch.Stop();
            long firstElapsed = stopWatch.Elapsed.Ticks;
            
            return new Episode(episodeString);
        }

        public static string GetNameFromHTML(string HTMLText)
        {
            string innerHTML = GetInnerHTMLByAttribute("parent", "class", HTMLText).Item1;

            Match match = Regex.Match(innerHTML, "\'url\'>(.*)</a>", RegexOptions.IgnoreCase);
            string name = match.Groups[1].Value;

            return name;
        }

        public static Episode GetLatestAndNextEpisode(string ImdbId, bool IsAdd)
        {
            string url = "http://www.imdb.com/title/" + "tt" + ImdbId + "/episodes";
            string HTMLText = WebRequests.RequestPage(url);

            return GetEpisodesFroHTML(ImdbId, HTMLText, IsAdd);
        }

        public static Episode GetEpisodesFroHTML(string imdbId, string HTMLText, bool isAdd,
            DateTime currNextAirDate = new DateTime(), int currNextDateIndex = 0)
        {
            if (HTMLText == "") return null;

            // Get season number
            string seasonName = GetInnerHTMLByAttribute("episode_top", "id", HTMLText).Item1;
            if (seasonName == null)
            {
                MessageBox.Show("This series doesn't have any episodes. Please choose another one.",
                    "Invalid series", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            // Get latest episode
            DateTime latestAirDate = new DateTime();
            int latestDateIndex = 0;

            DateTime nextAirDate = currNextAirDate;
            int nextDateIndex = currNextDateIndex;

            int startIndex = 0;
            int dateKnown = 3;
            Tuple<string, int> innerHTMLTuple;
            while ((innerHTMLTuple = GetInnerHTMLByAttribute("airdate", "class", HTMLText, startIndex)) != null)
            {
                if (innerHTMLTuple.Item1.Trim() == "") continue;

                DateTime currAirDate;
                try
                {
                    currAirDate = Convert.ToDateTime(innerHTMLTuple.Item1);
                }

                catch
                {
                    currAirDate = Convert.ToDateTime(innerHTMLTuple.Item1.Split('\n')[1] + ".12.31");
                    dateKnown = 1;
                }

                if (currAirDate < DateTime.Now)
                {
                    latestAirDate = currAirDate;
                    latestDateIndex = innerHTMLTuple.Item2;
                }

                else if (nextAirDate == null || currAirDate < nextAirDate)
                {
                    nextAirDate = currAirDate;
                    nextDateIndex = innerHTMLTuple.Item2;
                    break;
                }

                startIndex = GetInnerHTMLByAttribute("airdate", "class", HTMLText, startIndex).Item2;
            }

            // Change next air date
            if (nextAirDate != default(DateTime))
            {
                if (currNextAirDate != nextAirDate)
                {
                    Episode nextEpisode = GetEpisodeByDateIndex(nextDateIndex, HTMLText);

                    if (isAdd)
                    {
                        Variables.SelectedSeries.NextEpisodeAirDate = nextAirDate;
                        Variables.SelectedSeries.DateKnown = dateKnown;
                        Variables.SelectedSeries.NextEpisode = nextEpisode;
                    }

                    else
                    {
                        Series currSeries = Variables.SeriesList.Where(x => x.ImdbId == imdbId).FirstOrDefault();

                        currSeries.NextEpisodeAirDate = nextAirDate;
                        currSeries.DateKnown = dateKnown;
                        currSeries.NextEpisode = nextEpisode;
                    }
                }

                currNextAirDate = nextAirDate;
                currNextDateIndex = nextDateIndex;
            }

            // Return latest air date
            if (latestAirDate == default(DateTime))
            {
                Regex seasonRegex = new Regex("Season&nbsp;([0-9]+)");
                string previousSeasonNumber = Convert.ToString(Convert.ToInt32(seasonRegex.Match(seasonName).Groups[1].Value) - 1);

                string url = "http://www.imdb.com/title/" + "tt" + imdbId + "/episodes" + previousSeasonNumber;
                return GetEpisodesFroHTML(imdbId, WebRequests.RequestPage(url), isAdd, currNextAirDate, currNextDateIndex);
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

                return GetEpisodeByDateIndex(latestDateIndex, HTMLText);
            }
        }
    }
}
