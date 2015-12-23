using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SeriesUpdater.Internal
{
    class Variables
    {
        // Global
        public const string ApplicationName = "Series Updater";
        public static List<Series> SeriesList = new List<Series>();
        
        // MainForm
        public static Form MainForm;
        public static NotifyIcon NotifyIcon;

        public static int LastDeactivateTick;
        public static bool IsAddFormOpened = false;

        // AddForm
        public static bool IsAddedSeries = false;
        public static bool IsKeyDownFired = false;

        // SearchForm
        public static Series SelectedSeries;
        public static bool IsSelectedSeries = false;
        public static string SearchQuery = "";

        public static List<ResultSeries> ResultSeriesList = new List<ResultSeries>();

        // IO
        public static string DataFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\SeriesUpdater";
        public static string ExecutableFileName = DataFolderPath + @"\SeriesUpdater.exe";
        public static string SettingsFileName = DataFolderPath + @"\settings.dat";
        public static string SeriesDataFileName = DataFolderPath + @"\series.json";

        // Settings
        public static bool IsFirst = Context.Settings.IsFirst();
    }
}
