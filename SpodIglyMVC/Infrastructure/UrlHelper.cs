using System.IO;
using System.Web.Mvc;

namespace SpodIglyMVC.Infrastructure
{
    public static class UrlHelpers
    {
        public static string GenreIconPath(this UrlHelper helper, string genreIconFileName)
        {
            string genreIconFolder = AppConfig.GenreIconsFolderRelative;
            string path = Path.Combine(genreIconFolder, genreIconFileName);
            string absoluthePath = helper.Content(path);
            return absoluthePath;
        }

        public static string CoverIconFile(this UrlHelper helper, string coverIconFileName)
        {
            string genreIconFolder = AppConfig.CoverIconsFolder;
            string path = Path.Combine(genreIconFolder, coverIconFileName);
            string absoluthePath = helper.Content(path);
            return absoluthePath;
        }
    }
}