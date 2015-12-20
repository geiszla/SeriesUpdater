using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Forms;

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
                using (StreamReader responseReader = new StreamReader(webResponse.GetResponseStream(), System.Text.Encoding.UTF8))
                {
                    responseHTML = responseReader.ReadToEnd();
                }

                return responseHTML;
            }

            catch
            {
                MessageBox.Show("Couldn't get series information. IMDB id may be invalid or there is no connection to the server.",
                    Variables.ApplicationName + ": Couldn't get series information", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            string url = "http://www.imdb.com/find?" + "q=" + Query + "&s=tt&ttype=tv&ref_=fn_tv";
            string HTMLText = RequestPage(url);

            int startIndex = 0;
            string innerHTML = "";
            for (int i = 0; i < 10; i++)
            {
                if ((innerHTML = ProcessHTML.GetInnerHTMLByAttribute("result_text", "class", HTMLText, startIndex).Item1) == null)
                {
                    break;
                }

                ResultSeries currResult = new ResultSeries();

                Match typeMatch = Regex.Match(innerHTML, @"\(([a-z| |-]*)\)", RegexOptions.IgnoreCase);
                currResult.type = typeMatch.Groups[1].Value;

                if (currResult.type == "TV Movie" || currResult.type == "TV Short")
                {
                    i--;
                }

                else
                {
                    Match idMatch = Regex.Match(innerHTML, @"/title/tt(.*)\/\?", RegexOptions.IgnoreCase);
                    currResult.id = idMatch.Groups[1].Value;

                    Match nameMatch = Regex.Match(innerHTML, "<(a href=)+.*>(.*)</a", RegexOptions.IgnoreCase);
                    currResult.name = nameMatch.Groups[2].Value;

                    Match akaMatch = Regex.Match(innerHTML, "<i>\"(.*)\"</i>", RegexOptions.IgnoreCase);
                    currResult.aka = akaMatch.Groups[1].Value;

                    Match yearMatch = Regex.Match(innerHTML, @"> \(([0-9]*)\)", RegexOptions.IgnoreCase);
                    currResult.startYear = yearMatch.Groups[1].Value;

                    Variables.ResultSeriesList.Add(currResult);
                }

                startIndex = Convert.ToInt32(ProcessHTML.GetInnerHTMLByAttribute("result_text", "class", HTMLText, startIndex).Item2);
            }
        }

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
