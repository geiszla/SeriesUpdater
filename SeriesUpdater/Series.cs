using System;

namespace SeriesUpdater
{
    class Series
    {
        public int Id;
        public string Name;
        public string ImdbId;
        public Episode LastViewed;
        public Episode LastEpisode;
        public Episode NextEpisode;
        public DateTime NextEpisodeAirDate;
        public int DateKnown;
        public int NotificationSent = 0;

        public Series(int Id, string Name, string ImdbId, Episode LastViewed, Episode LastEpisode, Episode NextEpisode, DateTime NextEpisodeAirDate, int DateKnown, int NotificationSent)
        {
            this.Id = Id;
            this.Name = Name;
            this.ImdbId = ImdbId;
            this.LastViewed = LastViewed;
            this.LastEpisode = LastEpisode;
            this.NextEpisode = NextEpisode;
            this.NextEpisodeAirDate = NextEpisodeAirDate;
            this.DateKnown = DateKnown;
            this.NotificationSent = NotificationSent;
        }

        public Series(string Name, string ImdbId, Episode LastEpisode, Episode NextEpisode, DateTime NextEpisodeAirDate, int DateKnown)
        {
            this.Name = Name;
            this.ImdbId = ImdbId;
            this.LastEpisode = LastEpisode;
            this.NextEpisode = NextEpisode;
            this.NextEpisodeAirDate = NextEpisodeAirDate;
            this.DateKnown = DateKnown;
        }
    }
}
