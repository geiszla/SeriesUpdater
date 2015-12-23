using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using SeriesUpdater.Context;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace SeriesUpdater.Internal
{
    class WebRequests
    {
        public static string RequestPage(string Url)
        {
            WebRequest webRequest = WebRequest.Create(Url);
            webRequest.Method = "GET";

            try
            {
                string responseHTML;
                using (WebResponse webResponse = webRequest.GetResponse())
                using (StreamReader responseReader = new StreamReader(webResponse.GetResponseStream(), Encoding.UTF8))
                {
                    responseHTML = responseReader.ReadToEnd();
                }

                return responseHTML;
            }

            catch
            {
                Notifications.ShowError("Couldn't get series information. IMDB id may be invalid or there is no connection to the server.",
                    "Couldn't get series information");
                return "";
            }
        }

        public static void GetLatestEpisodes()
        {
            foreach (Series currSeries in Variables.SeriesList)
            {
                Episode newLastEpisode = ProcessHTML.GetLatestAndNextEpisode(currSeries.ImdbId, false);

                if (newLastEpisode != currSeries.LastEpisode)
                {
                    currSeries.NotificationSent = 0;
                    currSeries.LastEpisode = newLastEpisode;
                }
            }
        }

        public static void SearchForSeries(string Query)
        {
            HtmlWeb htmlWeb = new HtmlWeb();
            HtmlDocument htmlDocument = htmlWeb.Load("http://www.imdb.com/find?" + "q=" + Query + "&s=tt&ttype=tv");

            HtmlNodeCollection resultNodes = htmlDocument.DocumentNode
                .SelectNodes("//*[contains(concat(' ', normalize-space(@class), ' '), ' result_text ')]");

            int nodeNumber = 0;
            for (int i = 0; i < 10; i++)
            {
                ResultSeries currResult = new ResultSeries();
                string innerHTML = resultNodes[nodeNumber].InnerHtml;

                Regex propertyRegex = new Regex("/tt([0-9]+).*>(.*)<.*\\(([0-9]+)\\).*\\((.*)\\)(?:.*>\"(.*)\"<)?");
                GroupCollection propertyGroups = propertyRegex.Match(innerHTML).Groups;

                if (propertyGroups[4].Value == "TV Movie" || propertyGroups[4].Value == "TV Short")
                {
                    i--;
                }

                else
                {
                    currResult.id = propertyGroups[1].Value;
                    currResult.name = propertyGroups[2].Value;
                    currResult.startYear = propertyGroups[3].Value;
                    currResult.type = propertyGroups[4].Value;
                    currResult.aka = propertyGroups.Count > 5 ? propertyGroups[5].Value : "";

                    Variables.ResultSeriesList.Add(currResult);
                }

                if (nodeNumber == resultNodes.Count - 1)
                {
                    break;
                }

                nodeNumber++;
            }

            return;
        }

        // Get time
        public static string GetAirTimeByName(string Name)
        {
            string content = RequestPage("http://services.tvrage.com/feeds/fullschedule.php?country=US");
            Tuple<string, int> showData = ProcessHTML.GetInnerHTMLByAttribute(Name, "name", content);

            if (showData == null) return default(DateTime).ToString("H:mm");

            int startIndex = content.IndexOf("attr=\"", content.LastIndexOf("<time", showData.Item2)) + 6;
            int endIndex = content.IndexOf("\"", startIndex) + 1;
            string airTime = content.Substring(startIndex, endIndex - startIndex - 1);

            return Convert.ToDateTime(airTime).ToString("HH:mm");
        }

        public static string GetNameById(string ImdbId) // Not used
        {
            string requestURL = "http://www.omdbapi.com/?i=tt" + ImdbId + "&plot=short&r=json";
            string content = RequestPage(requestURL);

            JObject json = JObject.Parse(content);
            return json["Title"].ToString();
        }

        public static void login() // Not used
        {
            //string imdbPage = requestPage("https://secure.imdb.com/oauth/login");

            //string pattern = "onclick=\"window.open\\('(.*)',";
            //List<string> links = new List<string>();
            //foreach (Match match in Regex.Matches(imdbPage, pattern))
            //{
            //    links.Add(match.Groups[1].Value);
            //}

            //OpenIdRelyingParty OIDRP = new OpenIdRelyingParty();
            //var str = OIDRP.GetResponse();
            //if (str != null)
            //{
            //}
        }
    }
}
