using System.Configuration;


namespace SpodIglyMVC.Infrastructure
{
    public class AppConfig
    {
        private static readonly string _genreIconsFolderRelative = ConfigurationManager.AppSettings["GenreIconsFolder"];
        public static string GenreIconsFolderRelative
        {
            get
            {
                return _genreIconsFolderRelative;
            }
        }

        private static readonly string _coverIconsFolder = ConfigurationManager.AppSettings["CoverIconsFolder"];
        public static string CoverIconsFolder
        {
            get
            {
                return _coverIconsFolder;
            }
        }
    }
}