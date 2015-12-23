using SeriesUpdater.Internal;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SeriesUpdater.Context
{
    class Notifications
    {
        public static void ShowError(string Text, string Caption)
        {
            MessageBox.Show(Text, Variables.DisplayName + " Error: " + Caption,
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static DialogResult ShowQuestion(string Text, string Caption)
        {
            return MessageBox.Show(Text, Variables.DisplayName + ": " + Caption,
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        public static void ShowComingSeries(bool IsAdd)
        {
            if (Properties.Settings.Default.SendNotifications
                || Variables.SeriesList.Count == 0) return;

            List<Series> todaySeries = new List<Series>();
            List<Series> tomorrowSeries = new List<Series>();

            foreach (Series currSeries in Variables.SeriesList)
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

            if (todaySeries.Count != 0 || tomorrowSeries.Count != 0) IO.WriteSeries();
            ShowNewEpisodeNotification("today", todaySeries);
            ShowNewEpisodeNotification("tomorrow", tomorrowSeries);
        }

        public static void ShowNewEpisodeNotification(string DayString, List<Series> UpcomingSeries)
        {
            if (UpcomingSeries.Count == 0) return;

            string notificationTitle = null;
            string notificationText = null;

            if (UpcomingSeries.Count == 1)
            {
                string showTime = UpcomingSeries[0].NextEpisodeAirDate.ToString("HH:mm");
                notificationText = "A new episode of \"" + UpcomingSeries[0].Name + "\" is coming out "
                    + DayString + (showTime != "00:00" ? " at " + showTime : null) + ".";
            }

            else if (UpcomingSeries.Count > 1)
            {
                notificationTitle = "New episodes (" + UpcomingSeries.Count + ")";
                notificationText = "New episodes of \"";

                for (int i = 0; i < UpcomingSeries.Count; i++)
                {
                    notificationText += UpcomingSeries[i].Name
                        + "\" (" + UpcomingSeries[i].NextEpisodeAirDate.ToString("HH:mm") + ")";

                    if (i == UpcomingSeries.Count - 2)
                    {
                        notificationText += " and ";
                    }

                    else if (i != UpcomingSeries.Count - 1)
                    {
                        notificationText += ", ";
                    }
                }

                notificationText += " are coming out " + DayString + ".";
            }

            ShowNotification(notificationText, notificationTitle, 30000);
        }

        public static void ShowNotification(string Text, string Caption, int Timeout)
        {
            NotifyIcon notifyIcon = Variables.NotifyIcon;

            notifyIcon.BalloonTipTitle = Caption;
            notifyIcon.BalloonTipText = Text;
            notifyIcon.ShowBalloonTip(Timeout);
        }
    }
}
