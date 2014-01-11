using ConsoleAppScriptForMovie;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieScriptApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileToWriteTo = @"C:\Users\PrashMaya\Documents\MovieIds\{0}-{1}.txt";
            string hindiMoviesfileToWriteTo = @"C:\Users\PrashMaya\Documents\HindiMovieIds\{0}-{1}.txt";
            string teluguMoviesfileToWriteTo = @"C:\Users\PrashMaya\Documents\TeluguMovieIds\{0}-{1}.txt";
            string hindiImdbUrlStringFormat = "http://www.imdb.com/search/title?at=0&languages=hi%7C1&sort=moviemeter,asc&start={0}&title_type=feature";
            string teluguImdbUrlStringFormat = "http://www.imdb.com/search/title?at=0&languages=te%7C1&sort=moviemeter,asc&start={0}&title_type=feature";
            string ImdbUrlStringFormat = "http://www.imdb.com/search/title?at=0&languages=en%7C1&sort=moviemeter,asc&start={0}&title_type=feature";

            //for (int i = 400; i < 3200; )
            //{
            //    string localFileToWriteTo = String.Format(teluguMoviesfileToWriteTo, i, i + 50);
            //    string localImdbUrlStringFormat = String.Format(teluguImdbUrlStringFormat, i + 1);
            //    string html = DownloadWebPageContent.Download(localImdbUrlStringFormat);
            //    ParseHtmlForMovieIds.Parse(html, localFileToWriteTo);
            //    i = i + 50;
            //}

            string englishMovieFolderPath = @"C:\Users\PrashMaya\Documents\TeluguMovieIds\";


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
                        string localFileDownloadedMovieInfo = String.Format(teluguFileDownloadedMovieInfo, line);
                        // Use a tab to indent each line of the file.
                        MyMovieApi.Process(localFileDownloadedMovieInfo, line);
                        //Console.WriteLine("\t" + line);
                        
                    }
                }
            }

            //string fileDownloadedMovieInfo = @"C:\Users\PrashMaya\Documents\EnglishMovieMovieApi\{0}.txt";
            //fileToWriteTo = String.Format(fileToWriteTo, 2400, 2450);
            //// Example #2 
            //// Read each line of the file into a string array. Each element 
            //// of the array is one line of the file. 
            //string[] lines = System.IO.File.ReadAllLines(fileToWriteTo);
            //int i = 0; 
            
            //foreach (string line in lines)
            //{
            //    string localFileDownloadedMovieInfo = String.Format(fileDownloadedMovieInfo, i);
            //    // Use a tab to indent each line of the file.
            //    MyMovieApi.Process(localFileDownloadedMovieInfo, line);
            //    Console.WriteLine("\t" + line);
            //    ++i;
            //}




            
        }
    }
}
