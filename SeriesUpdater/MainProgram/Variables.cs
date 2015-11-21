using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SeriesUpdater.MainProgram
{
    class Variables
    {
        public static Form mainForm;
        public static NotifyIcon notifyIcon;

        public static int lastDeactivateTick;
        public static bool isAddFormOpened = false;
        public static List<Series> SeriesList = new List<Series>();

        public static string DataFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\SeriesUpdater";
        public static string ExecutableFileName = DataFolderPath + @"\SeriesUpdater.exe";
        public static string SettingsFileName = DataFolderPath + @"\settings.dat";
        public static string SeriesDataFileName = DataFolderPath + @"\series.dat";

        public static bool isFirst = Context.Settings.IsFirstCheck();
        public static bool isAddedSeries = false;
        public static bool keyDownFired = false;

        public static Series selectedSeries;
        public static bool isSelectedSeries = false;
        public static string searchQuery = "";

        public static List<ResultSeries> resultSeriesList = new List<ResultSeries>();
    }
}
