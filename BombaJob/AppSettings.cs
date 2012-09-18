using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Diagnostics;

namespace BombaJob
{
    public sealed class AppSettings
    {
        private static volatile AppSettings instance;
        private static object syncRoot = new Object(); 

        private AppSettings()
        {
        }

        public static AppSettings Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new AppSettings();
                    }
                }
                return instance;
            }
        }

        public static string AppName = "BombaJob";

        public static string DBConnectionString = "Data Source=isostore:/BombaJob.sdf";

        public static string ServicesURL = "http://www.bombajob.bg/_mob_service.php";
        public static bool InDebug = true;

		public static bool ConfPrivateData = true;
		public static bool ConfGeoLocation = false;
        public static bool ConfInitSync = true;
        public static bool ConfOnlineSearch = true;
		public static bool ConfInAppEmail = false;
        public static bool ConfShowCategories = true;
        public static bool ConfShowBanners = true;

		public static string TwitterOAuthConsumerKey = "OVvHQ1wio8LZklS5mRUuA";
		public static string TwitterOAuthConsumerSecret = "zZm0RsfzkLpF3FYnxcM3BDZdxHA6sPLoPiTcBvohUEo";
		public static string FacebookAppID = "162884250446512";
        public static string FacebookAppSecret = "a082d8bbc8e98cf63f8a1711ccbafe82";

        public static void LogThis(params string[] logs)
        {
            if (AppSettings.InDebug)
                Debugger.Log(3, "Warning", "[____BombaJob-Log] " + string.Join(" ", logs));
        }
    }
}
