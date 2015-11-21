using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SeriesUpdater.MainProgram
{
    class WebRequest
    {
        public static string RequestPage(string Url)
        {
            HttpWebRequest webRequest = (HttpWebRequest)System.Net.WebRequest.Create(Url);
            webRequest.Method = "GET";

            try
            {
                WebResponse webResponse = webRequest.GetResponse();

                string HTMLText;
                using (StreamReader seriesRequestReader = new StreamReader(webResponse.GetResponseStream(), System.Text.Encoding.UTF8))
                {
                    HTMLText = seriesRequestReader.ReadToEnd();
                }

                return HTMLText;
            }

            catch
            {
                MessageBox.Show("Az azonosítás sikertelen volt. Nem megfelelő az IMDB azonosító, vagy nincs kapcsolat a szerverrel.",
                    "Sikertelen azonosítás", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "";
            }
        }

        public static void GetLatestEpisodes()
        {
            foreach (Series currSeries in Variables.SeriesList)
            {
                string id = currSeries.ImdbId;
                string url = "http://www.imdb.com/title/" + "tt" + id + "/episodes";

                ProcessHTML.CurrNextAirDate = new DateTime();
                ProcessHTML.CurrNextDateIndex = 0;
                Episode newLastEpisode = ProcessHTML.GetLatestEpisodeFromHTML(id, RequestPage(url), false);

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
                if ((innerHTML = ProcessHTML.GetInnerHTMLByAttribute(startIndex, HTMLText, "result_text", "class")[0]) == null)
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

                    Variables.resultSeriesList.Add(currResult);
                }

                startIndex = Convert.ToInt32(ProcessHTML.GetInnerHTMLByAttribute(startIndex, HTMLText, "result_text", "class")[1]);
            }
        }

        public static string GetAirTimeByName(string Name)
        {
            string content = RequestPage("http://services.tvrage.com/feeds/fullschedule.php?country=US");
            string[] showData = ProcessHTML.GetInnerHTMLByAttribute(0, content, Name, "name");

            if (showData[0] == null) return default(DateTime).ToString("H:mm");

            int startIndex = content.IndexOf("attr=\"", content.LastIndexOf("<time", Convert.ToInt32(showData[1]))) + 6;
            int endIndex = content.IndexOf("\"", startIndex) + 1;
            string airTime = content.Substring(startIndex, endIndex - startIndex - 1);

            return Convert.ToDateTime(airTime).ToString("HH:mm");
        }

        public static string GetNameById(string ImdbId)
        {
            string requestURL = "http://www.omdbapi.com/?i=tt" + ImdbId + "&plot=short&r=json";
            string content = RequestPage(requestURL);

            JObject json = JObject.Parse(content);
            return json["Title"].ToString();
        }

        public static void login()
        {
            /*
            string imdbPage = requestPage("https://secure.imdb.com/oauth/login");

            string pattern = "onclick=\"window.open\\('(.*)',";
            List<string> links = new List<string>();
            foreach (Match match in Regex.Matches(imdbPage, pattern))
            {
                links.Add(match.Groups[1].Value);
            }

            OpenIdRelyingParty OIDRP = new OpenIdRelyingParty();
            var str = OIDRP.GetResponse();
            if (str != null)
            {
            }
            */
        }
    }
}
