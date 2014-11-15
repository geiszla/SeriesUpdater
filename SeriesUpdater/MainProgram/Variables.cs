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
        public static List<Series> seriesList = new List<Series>();
        public static string dataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\SeriesUpdater";
        public static bool isFirst = Context.Settings.isFirstCheck();
        public static bool isAddedSeries = false;
        public static bool keyDownFired = false;


        public static Series selectedSeries;
        public static bool isSelectedSeries = false;
        public static string searchQuery = "";

        public static List<ResultSeries> resultSeriesList = new List<ResultSeries>();
        public static string inputName = "";
    }
}
