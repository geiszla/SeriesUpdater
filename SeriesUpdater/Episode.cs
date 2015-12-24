using System;
using System.Text.RegularExpressions;

namespace SeriesUpdater
{
    class Episode
    {
        public int SeasonNumber;
        public int EpisodeNumber;
        public DateTime AirDate;

        public Episode() { }
        public Episode(string EpisodeString)
        {
            Regex episodeRegex = new Regex("S([0-9]+)E([0-9]+)");
            Match regexMatch = episodeRegex.Match(EpisodeString);
            
            if (!regexMatch.Success)
            {
                episodeRegex = new Regex("S([0-9]+), Ep([0-9]+)");
                regexMatch = episodeRegex.Match(EpisodeString);
            }
            
            SeasonNumber = Convert.ToInt32(regexMatch.Groups[1].Value);
            EpisodeNumber = Convert.ToInt32(regexMatch.Groups[2].Value);
        }

        public Episode(int SeasonNumber, int EpisodeNumber)
        {
            this.SeasonNumber = SeasonNumber;
            this.EpisodeNumber = EpisodeNumber;
        }

        public override string ToString()
        {
            return "S" + SeasonNumber + "E" + EpisodeNumber;
        }

        public static bool IsValidEpisodeString(string EpisodeString)
        {
            Regex episodeRegex = new Regex("S([0-9]+)E([0-9]+)");
            if (!episodeRegex.IsMatch(EpisodeString))
            {
                episodeRegex = new Regex("S([0-9]+), Ep([0-9]+)");
                return episodeRegex.IsMatch(EpisodeString);
            }

            return true;
        }

        public static bool IsLastAndNext(DateTime FirstAirDate, DateTime SecondAirDate, DateTime Now)
        {
            if (FirstAirDate.Year > Now.Year) return false;

            int nowDayOfYear = Now.DayOfYear;
            return FirstAirDate.DayOfYear < nowDayOfYear && FirstAirDate.DayOfYear >= nowDayOfYear;
        }

        public static bool IsNext(DateTime CurrAirDate, DateTime NextAirDate, DateTime Now)
        {
            if (CurrAirDate.Year < Now.Year) return false;

            int nowDayOfYear = Now.DayOfYear;
            return CurrAirDate.DayOfYear >= nowDayOfYear && NextAirDate.DayOfYear >= nowDayOfYear;
        }
    }
}
