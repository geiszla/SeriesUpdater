using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SeriesUpdater.Context
{
    class Notification
    {
        public static void ShowNotification(string Title, string Text, int Timeout)
        {
            NotifyIcon notifyIcon = MainProgram.Variables.notifyIcon;

            notifyIcon.BalloonTipTitle = Title;
            notifyIcon.BalloonTipText = Text;
            notifyIcon.ShowBalloonTip(Timeout);
        }

        public static void ShowNewEpisodeNotification(string DayString, List<Series> UpcomingSeries)
        {
            if (UpcomingSeries.Count == 1)
            {
                string showTime = UpcomingSeries[0].NextEpisodeAirDate.ToString("HH:mm");
                string notificationText = DayString + (showTime != "00:00" ? showTime + "-kor" : "")
                    + "új rész jelenik meg a(z) " + UpcomingSeries[0].Name + " című sorozatból.";
                ShowNotification("Új epizód", notificationText, 30000);
            }

            else if (UpcomingSeries.Count > 1)
            {
                string notificationTitle = "Új epizód (" + UpcomingSeries.Count + ")";
                string notificationText = DayString + " új rész jelenik meg a(z) ";

                for (int i = 0; i < UpcomingSeries.Count; i++)
                {
                    notificationText += UpcomingSeries[i].Name
                        + " (" + UpcomingSeries[i].NextEpisodeAirDate.ToString("HH:mm") + ")";

                    if (i == UpcomingSeries.Count - 2)
                    {
                        notificationText += " és a(z) ";
                    }

                    else if (i != UpcomingSeries.Count - 1)
                    {
                        notificationText += ", ";
                    }
                }

                notificationText += " című sorozatokból.";
                ShowNotification(notificationTitle, notificationText, 30000);
            }
        }

        public static void GetComingSeries(bool IsAdd)
        {
            if (!Convert.ToBoolean(Settings.GlobalSettings[0][1])) return;

            List<Series> todaySeries = new List<Series>();
            List<Series> tomorrowSeries = new List<Series>();

            foreach (Series currSeries in MainProgram.Variables.SeriesList)
            {
                if (currSeries.DateKnown < 3 || DateTime.Now.Year < currSeries.NextEpisodeAirDate.Year) continue;

                int remainingDays = currSeries.NextEpisodeAirDate.DayOfYear - DateTime.Now.DayOfYear;
                if (currSeries.NotificationSent < 2 && remainingDays == 0)
                {
                    todaySeries.Add(currSeries);
                    currSeries.NotificationSent = 2;
                }

                else if (currSeries.NotificationSent == 0 && remainingDays == 1)
                {
                    tomorrowSeries.Add(currSeries);
                    currSeries.NotificationSent = 1;
                }
            }

            IO.WriteSeries();
            ShowNewEpisodeNotification("A mai napon", todaySeries);
            ShowNewEpisodeNotification("Holnap", tomorrowSeries);
        }
    }
}
