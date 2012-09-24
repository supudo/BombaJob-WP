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
using System.Text.RegularExpressions;

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

		public static string TwitterOAuthConsumerKey = "";
		public static string TwitterOAuthConsumerSecret = "";
		public static string FacebookAppID = "";
        public static string FacebookAppSecret = "";

        public static string DateTimeFormat = "dd-MM-yyyy HH:mm:ss";

        public static int OffersPerPage = 20;

        public static void LogThis(params string[] logs)
        {
            if (AppSettings.InDebug)
                Debugger.Log(3, "Warning", "[____BombaJob-Log] " + string.Join(" ", logs));
        }

        public static string DoLongDate(DateTime dt)
        {
            if (dt == null)
                return "";
            string date = "";
            date += dt.ToString("HH:mm");
            date += " ";
            date += AppResources.ResourceManager.GetString("weekday_" + ((int)dt.DayOfWeek + 1));
            date += ", ";
            date += dt.Day;
            date += " ";
            date += AppResources.ResourceManager.GetString("monthFull_" + dt.Month);
            return date;
        }

        public static string DoShortDate(DateTime dt)
        {
            if (dt == null)
                return "";
            string date = "";
            date += dt.ToString("HH:mm");
            date += " ";
            date += AppResources.ResourceManager.GetString("weekday_" + ((int)dt.DayOfWeek + 1));
            date += ", ";
            date += dt.Day;
            date += " ";
            date += AppResources.ResourceManager.GetString("monthShort_" + dt.Month);
            return date;
        }

        public static string Hyperlinkify(string strvar)
        {
            string final = strvar;

            Regex regex = new Regex(@"<nolink>(.*?)</nolink>",
                          RegexOptions.IgnoreCase | RegexOptions.Singleline |
                          RegexOptions.CultureInvariant |
                          RegexOptions.IgnorePatternWhitespace |
                          RegexOptions.Compiled);

            MatchCollection theMatches = regex.Matches(strvar);

            for (int index = 0; index < theMatches.Count; index++)
                final = final.Replace(theMatches[index].ToString(), theMatches[index].ToString().Replace(".", "[[[pk:period]]]"));

            regex = new Regex(@"<a(.*?)</a>", RegexOptions.IgnoreCase |
                    RegexOptions.Singleline | RegexOptions.CultureInvariant |
                    RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);

            theMatches = regex.Matches(final);

            for (int index = 0; index < theMatches.Count; index++)
            {
                final = final.Replace(theMatches[index].ToString(),
                        theMatches[index].ToString().Replace(".", "[[[pk:period]]]"));
            }

            final = Regex.Replace(final, @"(?<=\d)\.(?=\d)", "[[[pk:period]]]");

            Regex tags = new Regex(@"([a-zA-Z0-9\:/\-]*[a-zA-Z0-9\-_]\" +
                         @".[a-zA-Z0-9\-_][a-zA-Z0-9\-_][a-zA-Z0-9\?\" +
                         @"=&#_\-/\.]*[^<>,;\.\s\)\(\]\[\""])");

            final = tags.Replace(final, "<a href=\"http://$&\">$&</a>");
            final = final.Replace("http://https://", "https://");
            final = final.Replace("http://http://", "http://");

            final = final.Replace("[[[pk:period]]]", ".");
            final = final.Replace("<nolink>", "");
            final = final.Replace("</nolink>", "");

            return final;
        }
    }
}
