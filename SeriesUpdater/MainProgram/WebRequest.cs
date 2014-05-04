using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SeriesUpdater.MainProgram
{
    class WebRequest
    {
        public static string requestPage(string url)
        {
            HttpWebRequest myRequest = (HttpWebRequest)System.Net.WebRequest.Create(url);
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

        public static void getLatestEpisodes(bool isAdd)
        {
            int forStart;

            if (isAdd)
            {
                forStart = MainProgram.Variables.seriesList.Count - 1;
            }

            else
            {
                forStart = 0;
            }

            for (int i = 0; i < MainProgram.Variables.seriesList.Count; i++)
            {
                string id = MainProgram.Variables.seriesList[i].imdbId;
                string url = "http://www.imdb.com/title/" + "tt" + id + "/episodes";
                MainProgram.Variables.seriesList[i].lastEpisode = MainProgram.ProcessHTML.getLatestEpisodeFromHTML(id, MainProgram.WebRequest.requestPage(url), true);
            }
        }

        public static void searchForSeries(string query)
        {
            string url = "http://www.imdb.com/find?" + "q=" + query + "&s=tt&ttype=tv&ref_=fn_tv";
            string HTMLText = requestPage(url);

            int startIndex = 0;
            string innerHTML = "";
            for (int i = 0; i < 10; i++)
            {
                if ((innerHTML = MainProgram.ProcessHTML.getInnerHTMLByClassOrId(startIndex, HTMLText, "result_text", "class")[0]) == null)
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

                    MainProgram.Variables.resultSeriesList.Add(currResult);
                }

                startIndex = Convert.ToInt32(MainProgram.ProcessHTML.getInnerHTMLByClassOrId(startIndex, HTMLText, "result_text", "class")[1]);
            }
        }
    }
}
