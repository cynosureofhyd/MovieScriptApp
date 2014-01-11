using ConsoleAppScriptForMovie;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieScriptApp
{
    public class EnumerateDirectory
    {
        public static void Enumerate()
        {
            string englishMovieFolderPath = @"C:\Users\PrashMaya\Documents\TempEnglishMovieIds\";


            foreach (string file in Directory.EnumerateFiles(englishMovieFolderPath, "*.txt"))
            {
                //if (file == "36800-36850.txt")
                {
                    string[] lines = File.ReadAllLines(file);


                    string fileDownloadedMovieInfo = @"C:\Users\PrashMaya\Documents\EnglishMovieMovieApi\{0}.txt";
                    string teluguFileDownloadedMovieInfo = @"C:\Users\PrashMaya\Documents\TeluguMovieMovieApi\{0}.txt";
                    string hindiFileDownloadedMovieInfo = @"C:\Users\PrashMaya\Documents\HindiMovieMovieApi\{0}.txt";
                    foreach (string line in lines)
                    {
                        string localFileDownloadedMovieInfo = String.Format(fileDownloadedMovieInfo, line);
                        // Use a tab to indent each line of the file.
                        MyMovieApi.Process(localFileDownloadedMovieInfo, line);
                        //Console.WriteLine("\t" + line);

                    }
                }
            }
        }
    }
}
