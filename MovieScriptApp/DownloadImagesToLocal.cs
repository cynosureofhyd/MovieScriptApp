using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieScriptApp
{
    public class DownloadImagesToLocal
    {
        public static async void Download()
        {
            string serverPath = @"C:\Users\PrashMaya\Pictures\MyMovieRecommendation\";
            string localFilenameImdb = @"C:\Users\PrashMaya\Pictures\MyMovieRecommendation\{0}.jpg";
            string localFilenameCover = @"C:\Users\PrashMaya\Pictures\MyMovieRecommendation\Cover-{0}.jpg";
            var db = new MyMovieEntities();


            var directoryName = @"C:\Users\PrashMaya\Pictures\MyMovieRecommendation";
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(directoryName);
            int count1 = dir.GetFiles().Length;
            int i = 0;
            foreach (var poster in db.PosterInfoes)
            {
                using (MyWebClient client = new MyWebClient())
                {
                    try
                    {
                        if (poster.Imdb != null)
                        {
                            var imdbId = db.Movies.Where(m => m.ID == poster.MovieId).ToList();
                            //imdbId = db.Movies.Where(s => s.ID == poster.MovieId).ToList();
                            int count = imdbId.Count();
                            serverPath = string.Format(serverPath, imdbId.First().ImdbID);
                         
                            localFilenameImdb = string.Format(localFilenameImdb, imdbId.First().ImdbID);
                           
                            localFilenameCover = string.Format(localFilenameCover, imdbId.First().ImdbID);

                            //if (!Directory.Exists(serverPath))
                            //    Directory.CreateDirectory(serverPath);
                            if (!File.Exists(localFilenameImdb))
                            client.DownloadFile((poster.Imdb), localFilenameImdb);
                            if (!File.Exists(localFilenameCover))
                            client.DownloadFile((poster.Cover), localFilenameCover);
                            i++;
                            localFilenameImdb = @"C:\Users\PrashMaya\Pictures\MyMovieRecommendation\{0}.jpg";
                            //serverPath = @"C:\Users\PrashMaya\Pictures\{0}\";
                        }
                        //client.DownloadFile(poster.Cover, localFilenameCover);
                    }
                    catch (Exception ex)
                    {
                        i++;
                        continue;
                    }
                }
            }
        }
    }
}
