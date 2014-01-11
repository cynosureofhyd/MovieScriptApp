using System;
using System.Collections.Generic;
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
            string ImdbUrlStringFormat = "http://www.imdb.com/search/title?at=0&languages=en%7C1&sort=moviemeter,asc&start={0}&title_type=feature";

            for(int i = 21050; i < 50000; )
            {
                string localFileToWriteTo = String.Format(fileToWriteTo, i, i + 50);
                string localImdbUrlStringFormat = String.Format(ImdbUrlStringFormat, i + 1);
                string html = DownloadWebPageContent.Download(localImdbUrlStringFormat);
                ParseHtmlForMovieIds.Parse(html, localFileToWriteTo);
                i = i + 50;
            }


            //string ImdbUrl = "http://www.imdb.com/search/title?at=0&languages=en%7C1&sort=moviemeter,asc&start=2401&title_type=feature";
            //string html = DownloadWebPageContent.Download(ImdbUrl);
            //ParseHtmlForMovieIds.Parse(html, fileToWriteTo);

            //ImdbUrl = "http://www.imdb.com/search/title?at=0&languages=en%7C1&sort=moviemeter,asc&start=2451&title_type=feature";
            //html = DownloadWebPageContent.Download(ImdbUrl);
            //ParseHtmlForMovieIds.Parse(html, fileToWriteTo);
        }
    }
}
