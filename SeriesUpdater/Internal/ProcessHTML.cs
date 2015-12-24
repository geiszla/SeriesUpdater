using HtmlAgilityPack;
using SeriesUpdater.Context;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace SeriesUpdater.Internal
{
    class ProcessHtml
    {
        #region Regex
        public static Tuple<string, int> GetInnerHtmlByAttribute(string AttributeValue, string Attribute,
            string HtmlText, int SearchStartIndex = 0)
        {
            // Get tag name
            string regexString = @"<([a-z0-9]+) " + Attribute + "=\"" + AttributeValue;
            Regex tagRegex = new Regex(regexString);

            Match tagMatch = tagRegex.Match(HtmlText, SearchStartIndex);
            if (!tagMatch.Success) return null;

            string tagName = tagMatch.Groups[1].Value;
            int tagIndex = tagMatch.Index;

            // Get inner HTML by tag name
            int startIndex = HtmlText.IndexOf('>', tagIndex) + 1;
            int endIndex = HtmlText.IndexOf("</" + tagName, startIndex);

            string innerHtml = HtmlText.Substring(startIndex, endIndex - startIndex);

            return new Tuple<string, int>(innerHtml, endIndex + tagName.Length + 2);
        }

        public static string GetNameFromHtml(string HtmlText)
        {
            string innerHtml = GetInnerHtmlByAttribute("parent", "class", HtmlText).Item1;

            Match match = Regex.Match(innerHtml, "\'url\'>(.*)</a>", RegexOptions.IgnoreCase);
            string name = match.Groups[1].Value;

            return name;
        }
        #endregion

        #region Html Agility Pack
        public static Episode[] GetEpisodes(string ImdbId, bool IsAdd)
        {
            string url = "http://www.imdb.com/title/" + "tt" + ImdbId + "/episodes";
            string htmlText = WebRequests.RequestPage(url);
            
            return GetEpisodesFromHtml(ImdbId, htmlText, IsAdd);
        }

        public static Episode[] GetEpisodesFromHtml(string imdbId, string HtmlText, bool isAdd)
        {
            if (HtmlText == "") return null;
            Tuple<DateTime, int, HtmlNode>[] airDateTuples = getAirDates(imdbId, HtmlText);
            Episode lastEpisode = getEpisodeNumberByAirDate(airDateTuples[0]);

            if (airDateTuples[1].Item1 == default(DateTime))
            {
                string url = "http://www.imdb.com/title/" + "tt" + imdbId + "/episodes?season=" + lastEpisode.SeasonNumber;
                string htmlText = WebRequests.RequestPage(url);

                airDateTuples = getAirDates(imdbId, htmlText, true);
            }

            Episode nextEpisode = getEpisodeNumberByAirDate(airDateTuples[1]);
            
            Series currSeries = Variables.SeriesList.Where(x => x.ImdbId == imdbId).FirstOrDefault();
            if (isAdd)
            {
                Variables.SelectedSeries.NextEpisode = nextEpisode;
                Variables.SelectedSeries.DateKnown = airDateTuples[1].Item2;
            }

            else if (currSeries != null)
            {
                currSeries.LastEpisode = lastEpisode;
                currSeries.NextEpisode = nextEpisode;
                currSeries.DateKnown = airDateTuples[1].Item2;
            }

            return new Episode[] { lastEpisode, nextEpisode  };
        }

        static Tuple<DateTime, int, HtmlNode>[] getAirDates(string imdbId, string htmlText, bool isNextOnly = false)
        {
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(htmlText);

            HtmlNodeCollection resultNodes = htmlDocument.DocumentNode
                .SelectNodes("//*[contains(concat(' ', normalize-space(@class), ' '), ' airdate ')]");

            // [0] = last episode, [1] = next episode
            int[] dateKnowns = new int[2];
            int[] nodeIndexes = new int[2];
            DateTime[] airDates = new DateTime[2];

            Tuple<DateTime, int> previousDateTuple = parseDate(resultNodes[0].InnerHtml);
            for (int i = 1; i < resultNodes.Count; i++)
            {
                DateTime previousAirDate = previousDateTuple.Item1;

                Tuple<DateTime, int> currDateTuple = parseDate(resultNodes[i].InnerHtml);
                DateTime currAirDate = currDateTuple.Item1;
                int dateKnown = currDateTuple.Item2;

                DateTime now = DateTime.Now;
                if (Episode.IsNext(previousAirDate, currAirDate, now))
                {
                    airDates[1] = previousAirDate;
                    nodeIndexes[1] = i;
                    dateKnowns[1] = dateKnown;
                    break;
                }

                if (!isNextOnly)
                {
                    if (Episode.IsLastAndNext(previousAirDate, currAirDate, now))
                    {
                        airDates[0] = previousAirDate;
                        nodeIndexes[0] = i - 1;
                        dateKnowns[0] = dateKnown;

                        airDates[1] = currAirDate;
                        nodeIndexes[1] = i;
                        dateKnowns[1] = dateKnown;
                        break;
                    }

                    else if (i == resultNodes.Count - 1)
                    {
                        airDates[0] = currAirDate;
                        nodeIndexes[0] = i - 1;
                        dateKnowns[0] = dateKnown;
                    }
                }

                previousDateTuple = currDateTuple;
            }

            Tuple<DateTime, int, HtmlNode> lastEpisodeTuple = new Tuple<DateTime, int, HtmlNode>(airDates[0], dateKnowns[0], resultNodes[nodeIndexes[0]]);
            Tuple<DateTime, int, HtmlNode> nextEpisodeTuple = new Tuple<DateTime, int, HtmlNode>(airDates[1], dateKnowns[1], resultNodes[nodeIndexes[1]]);

            return new Tuple<DateTime, int, HtmlNode>[] { lastEpisodeTuple, nextEpisodeTuple };
        }

        static Episode getEpisodeNumberByAirDate(Tuple<DateTime, int, HtmlNode> airDateTuple)
        {
            HtmlNodeCollection episodeNodes = airDateTuple.Item3.SelectNodes("ancestor::*[contains(concat(' ', normalize-space(@class), ' '), ' list_item ')]//*[contains(concat(' ', normalize-space(@class), ' '), ' hover-over-image ')]");
            if (episodeNodes.Count == 0) return new Episode();

            Episode currEpisode = new Episode();
            foreach (HtmlNode currEpisodeNode in episodeNodes)
            {
                string currEpisodeString = currEpisodeNode.InnerHtml;
                if (Episode.IsValidEpisodeString(currEpisodeString))
                {
                    currEpisode = new Episode(currEpisodeString);
                    currEpisode.AirDate = airDateTuple.Item1;
                }
            }

            return currEpisode;
        }
        #endregion

        #region Helper Functions
        static Tuple<DateTime, int> parseDate(string innerHtml)
        {
            DateTime airDate;
            int dateKnown;
            try
            {
                airDate = Convert.ToDateTime(innerHtml);
                dateKnown = 3;
            }

            catch
            {
                airDate = Convert.ToDateTime(innerHtml.Split('\n')[1] + ".12.31");
                dateKnown = 1;
            }

            return new Tuple<DateTime, int>(airDate, dateKnown);
        }
        #endregion
    }
}
