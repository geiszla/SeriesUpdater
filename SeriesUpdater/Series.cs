using Newtonsoft.Json;
using System;

namespace SeriesUpdater
{
    class Series
    {
        [JsonIgnore]
        public int Id;

        public string Name;
        public string ImdbId;
        public Episode LastViewed;
        public Episode LastEpisode;
        public Episode NextEpisode;
        public int DateKnown; // 0 = not known, 1 = year known, 2 = month known, 3 = day known, 4 = time known
        public int NotificationSent = 0; // 0 = not sent, 1 = tomorrow sent, 2 = today sent

        public Series() { }
        public Series(int Id, string Name, string ImdbId, Episode LastViewed, Episode LastEpisode, Episode NextEpisode, DateTime NextEpisodeAirDate, int DateKnown, int NotificationSent)
        {
            this.Id = Id;
            this.Name = Name;
            this.ImdbId = ImdbId;
            this.LastViewed = LastViewed;
            this.LastEpisode = LastEpisode;
            this.NextEpisode = NextEpisode;
            this.NextEpisode.AirDate = NextEpisodeAirDate;
            this.DateKnown = DateKnown;
            this.NotificationSent = NotificationSent;
        }

        public Series(string Name, string ImdbId, Episode LastEpisode, Episode NextEpisode, DateTime NextEpisodeAirDate, int DateKnown)
        {
            this.Name = Name;
            this.ImdbId = ImdbId;
            this.LastEpisode = LastEpisode;
            this.NextEpisode = NextEpisode;
            this.NextEpisode.AirDate = NextEpisodeAirDate;
            this.DateKnown = DateKnown;
        }
    }
}
