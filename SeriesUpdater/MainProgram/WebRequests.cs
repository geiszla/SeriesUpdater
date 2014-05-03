using System.IO;
using System.Net;
using System.Windows.Forms;

namespace SeriesUpdater.MainProgram
{
    class WebRequests
    {
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
    }
}
