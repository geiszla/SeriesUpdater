using System;
using System.Text.RegularExpressions;

namespace SeriesUpdater
{
    class Episode
    {
        public int SeasonNumber;
        public int EpisodeNumber;

        public Episode() { }
        public Episode(string EpisodeString)
        {
            Regex episodeRegex = new Regex("S([0-9]+)E([0-9]+)");
            GroupCollection regexGroups = episodeRegex.Match(EpisodeString).Groups;

            SeasonNumber = Convert.ToInt32(regexGroups[1].Value);
            EpisodeNumber = Convert.ToInt32(regexGroups[2].Value);
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

        public static bool IsValidEpisodeString(string String)
        {
            Regex episodeRegex = new Regex("S([0-9]+)E([0-9]+)");
            return episodeRegex.IsMatch(String);
        }
    }
}
