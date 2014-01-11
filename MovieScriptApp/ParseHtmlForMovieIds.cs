using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieScriptApp
{
    public class ParseHtmlForMovieIds
    {
        public static void Parse(string html, string fileToWriteTo)
        {
            MovieEntities db = new MovieEntities();
            List<string> movieIds = new List<string>();
            for (int i = 0; i < html.Length; i++ )
            {
                if(html[i] == 't')
                {
                    if((i + 2) < html.Length && html[i+1] == 't' && Char.IsDigit(html[i + 2]))
                    {
                        var movieId = html.Substring(i, 9);
                        bool isCorrectMovieId = true;

                        for (int j = 2; j < movieId.Length; j++ )
                        {
                            if(!Char.IsDigit(movieId[j]))
                            {
                                isCorrectMovieId = false;
                                break;
                            }
                        }

                            if (isCorrectMovieId && !db.Movies.Any(s => s.ImdbID == movieId))
                            {
                                if(!movieIds.Contains(movieId))
                                    movieIds.Add(movieId);
                            }
                    }
                }
            }
            ParseForMovieTitleWithSlashTitle(html, movieIds);
            WriteOnlyUniqueStringsToFile(fileToWriteTo, movieIds);
                //System.IO.File.WriteAllLines(filetowritedownloadeddatato, tempString);
        }

        private static void ParseForMovieTitleWithSlashTitle(string html, List<string> movieIds)
        {
            StringBuilder sb = new StringBuilder();
            int index = 0;
            do
            {
                index = html.IndexOf("title/", index);
                if (index != -1)
                {
                    
                    sb.Append(html.Substring(index+6,9));
                    var temp = html.Substring(index + 6, 9);
                    if (!movieIds.Contains(temp))
                        movieIds.Add(temp);
                    index++;
                }
            } while (index != -1);

            string repeats = sb.ToString();
        }



        private static void WriteOnlyUniqueStringsToFile(string fileName, List<string> movieIdsToWrite)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(fileName))
            {
                foreach (string line in movieIdsToWrite)
                {
                        file.WriteLine(line);
                }
            }
        }

        public static string filetowritedownloadeddatato { get; set; }

        public static IEnumerable<string> tempString { get; set; }
    }
}
