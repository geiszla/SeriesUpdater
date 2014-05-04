using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeriesUpdater.Context
{
    class Notification
    {
        public static void showNotification(string title, string text, int timeout)
        {
            NotifyIcon notifyIcon = MainProgram.Variables.notifyIcon;

            notifyIcon.BalloonTipTitle = title;
            notifyIcon.BalloonTipText = text;
            notifyIcon.ShowBalloonTip(timeout);
        }

        public static void showNewEpisodeNotification(string when, List<Series> comingSeries)
        {
            if (comingSeries.Count == 1)
            {
                string text = when + " új rész jelenik meg a következő sorozatból: " + comingSeries[0].name;
                showNotification("Új epizód", text, 5000);
            }

            else if (comingSeries.Count > 1)
            {
                string title = "Új epizód (" + comingSeries.Count + ")";
                string text = when + " új részek jelennek meg a következő sorozatokból: ";

                for (int i = 0; i < comingSeries.Count; i++)
                {
                    text += comingSeries[i].name;

                    if (i != comingSeries.Count - 1)
                    {
                        text += ", ";
                    }

                    else
                    {
                        text += ".";
                    }
                }

                showNotification(title, text, 5000);
            }
        }

        public static void getComingSeries()
        {
            List<Series> todaySeries = new List<Series>();
            List<Series> tomorrowSeries = new List<Series>();

            for (int i = 0; i < MainProgram.Variables.seriesList.Count; i++)
            {
                int days = MainProgram.Variables.seriesList[i].nextEpisodeAirDate.DayOfYear - DateTime.Now.DayOfYear;

                if (DateTime.Now.Year >= MainProgram.Variables.seriesList[i].nextEpisodeAirDate.Year)
                {
                    if (days == 0)
                    {
                        todaySeries.Add(MainProgram.Variables.seriesList[i]);
                    }

                    else if (days == 1)
                    {
                        tomorrowSeries.Add(MainProgram.Variables.seriesList[i]);
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
    }
}
