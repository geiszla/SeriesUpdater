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

        public static void showNewEpisodeNotification(string day, List<Series> comingSeries)
        {
            if (comingSeries.Count == 1)
            {
                string time = comingSeries[0].nextEpisodeAirDate.ToString("HH:mm");
                string text = day + (time != "00:00" ? time + "-kor" : "") + "új rész jelenik meg a(z) " + comingSeries[0].name + " című sorozatból.";
                showNotification("Új epizód", text, 30000);
            }

            else if (comingSeries.Count > 1)
            {
                string title = "Új epizód (" + comingSeries.Count + ")";
                string text = day + "új rész jelenik meg a(z) ";

                for (int i = 0; i < comingSeries.Count; i++)
                {
                    text += comingSeries[i].name + " (" + comingSeries[i].nextEpisodeAirDate.ToString("HH:mm") + ")";

                    if (i == comingSeries.Count - 2)
                    {
                        text += " és a(z) ";
                    }

                    else if (i != comingSeries.Count - 1)
                    {
                        text += ", ";
                    }
                }

                text += " című sorozatokból.";
                showNotification(title, text, 30000);
            }
        }

        public static void getComingSeries(bool isAdd)
        {
            List<Series> todaySeries = new List<Series>();
            List<Series> tomorrowSeries = new List<Series>();

            for (int i = 0; i < MainProgram.Variables.seriesList.Count; i++)
            {
                if (MainProgram.Variables.seriesList[i].dateKnown >= 3)
                {
                    int days = MainProgram.Variables.seriesList[i].nextEpisodeAirDate.DayOfYear - DateTime.Now.DayOfYear;

                    if (DateTime.Now.Year >= MainProgram.Variables.seriesList[i].nextEpisodeAirDate.Year)
                    {
                        if (MainProgram.Variables.seriesList[i].notificationSent < 2 && days == 0)
                        {
                            todaySeries.Add(MainProgram.Variables.seriesList[i]);
                            MainProgram.Variables.seriesList[i].notificationSent = 2;
                        }

                        else if (MainProgram.Variables.seriesList[i].notificationSent == 0 && days == 1)
                        {
                            tomorrowSeries.Add(MainProgram.Variables.seriesList[i]);
                            MainProgram.Variables.seriesList[i].notificationSent = 1;
                        }
                    }
                }
            }

            Context.IO.writeSeries();
            showNewEpisodeNotification("A mai napon ", todaySeries);
            showNewEpisodeNotification("Holnap ", tomorrowSeries);
        }
    }
}
