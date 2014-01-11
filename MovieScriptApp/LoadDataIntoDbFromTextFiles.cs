using MovieScriptApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MovieScriptApp
{
    public class LoadDataIntoDbFromTextFiles
    {
        public static void LoadData()
        {
            //AddSampleRoleData();

            string teluguMovieApis = @"C:\Users\PrashMaya\Documents\Movies\TeluguMovieMovieApi";
            string inputpath = "C:\\Users\\PrashMaya\\Desktop\\IMDBMovieTitleIds-0-2500.txt";
            string inputfolder = "C:\\Users\\PrashMaya\\My Documents\\First2500MoviesIMDB\\Movie{0}.txt";
            inputpath = String.Format(inputfolder, 1);


            foreach (string file in Directory.EnumerateFiles(teluguMovieApis, "*.txt"))
            {
                    string text = File.ReadAllText(file);
                    dynamic obj = ConvertToObj(text);
                    LoadDataIntoDb(obj, 0);
            }

            //for (int i = 240; i < 2451; i++)
            //{
            //    inputpath = String.Format(inputfolder, i);
            //    string text = System.IO.File.ReadAllText(@inputpath);
            //    if (text.Length > 10)
            //    {
            //        dynamic obj = ConvertToObj(text);
            //        LoadDataIntoDb(obj, i);
            //    }
            //}

            HashSet<string> keys = new HashSet<string>();
            string[] lines = System.IO.File.ReadAllLines(@inputpath);
        }

        private static void LoadDataIntoDb(dynamic obj, int path)
        {
            try
            {
                MovieEntities db = new MovieEntities();

                string imdbId = obj[0]["imdb_id"];

                //if(!db.Movies.Select(m => m.ImdbID == imdbId).First())
                if (db.Movies.Count() == 0 || !db.Movies.Select(m => m.ImdbID == imdbId).First())
                {
                    Int64 existingMovieId = 0;
                    if (db.Movies.Count() > 0)
                        existingMovieId = db.Movies.Count();
                    Movie movie = new Movie();
                    movie.ID = existingMovieId + 1;
                    movie.PlotDetailed = obj[0]["plot"] == null ? null : obj[0]["plot"]; ;
                    movie.ImdbID = obj[0]["imdb_id"] == null ? null : obj[0]["imdb_id"]; ;
                    movie.PlotSimple = obj[0]["plot_simple"] == null ? null : obj[0]["plot_simple"]; ;
                    var tempruntime = obj[0]["runtime"] == null ? null : obj[0]["runtime"]; ;
                    movie.Runtime = ConvertRuntime(tempruntime.ToString());
                    movie.Rated = obj[0]["rated"] == null ? null : obj[0]["rated"]; ;
                    movie.ImdbUrl = obj[0]["imdb_url"] == null ? null : obj[0]["imdb_url"]; ;
                    movie.AKA = obj[0]["also_known_as"][0] == null ? null : obj[0]["also_known_as"][0]; 
                    movie.IMDBRating = obj[0]["rating"] == null ? null : obj[0]["rating"];
                    Int64? releaseDate = obj[0]["release_date"] == null ? null : obj[0]["release_date"];
                    DateTime? dtTime = null;
                    if(releaseDate != null)
                        dtTime = new DateTime(Int32.Parse(releaseDate.ToString().Substring(0, 4)), Int32.Parse(releaseDate.ToString().Substring(4, 2)), Int32.Parse(releaseDate.ToString().Substring(6, 2)));
                    movie.ReleaseDate = dtTime;
                    movie.Title = obj[0]["title"] == null ? null : obj[0]["title"]; ;
                    Genres(obj, db);


                    db.Movies.Add(movie);
                    db.SaveChanges();
                    var savedMovie = db.Movies.Where(m => m.ImdbID == movie.ImdbID).Distinct();

                    MovieLanguage(obj, db, savedMovie);

                    MoviePersonRole(obj, db);

                     
                    var ListOfGenre = obj[0]["genres"] == null ? null : obj[0]["genres"];
                    Poster poster = new Poster();
                    poster.imdb = obj[0]["poster"]["imdb"] == null ? null : obj[0]["poster"]["imdb"];
                    poster.cover = obj[0]["poster"]["cover"] == null ? null : obj[0]["poster"]["cover"];
                    PosterInfo posterInfo = new PosterInfo();
                    posterInfo.Imdb = poster.imdb;
                    posterInfo.Cover = poster.cover;
                    posterInfo.MovieId = savedMovie.First().ID;
                    db.PosterInfoes.Add(posterInfo);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                string inputfolder = "C:\\Users\\PrashMaya\\My Documents\\Exceptions\\Movie{0}.txt";
                string inputfile = String.Format(inputfolder, path);
                System.IO.File.WriteAllText(inputfile, ex.ToString());
                Console.WriteLine(ex.Message);
            }
        }

        private static void MovieLanguage(dynamic obj, MovieEntities db, IQueryable<Movie> savedMovie)
        {
            if(obj[0]["language"]!= null)
            {
                for(int i = 0; i < obj[0]["language"].Count; i++)
                {
                    
                }
            }
            throw new NotImplementedException();
        }

        public static void MoviePersonRole(dynamic obj, MovieEntities db)
        {
            string actorName = string.Empty;
            var actor = db.Roles.Where(a => a.Description == "Actor");
            
            for (int i = 0; i < obj[0]["actors"].Count; i++)
            {
                Person person = new Person();
                //person.
                MoviePersonRole moviePersonRole = new MovieScriptApp.MoviePersonRole();
                
                actorName = obj[0]["actors"][i];
                string firstName = SplitNames(actorName).First();
                string lastName = SplitNames(actorName).Last();
                person.FirstName = firstName;
                person.MiddleName = ReturnMiddleName(actorName);
                person.LastName = lastName;

                db.People.Add(person);
                db.SaveChanges();
                var personId = db.People.Select(s => s == person).Distinct();
                
                
                //per.FirstName = 
            }
        }


        public static List<string> SplitNames(string name)
        {
            string[] words = name.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
            return words.ToList();
        }

        public static string ReturnMiddleName(string name)
        {
            string[] words = name.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
            StringBuilder sb = new StringBuilder();
            for(int i = 1; i < words.Count() -1; i++)
            {
                sb.Append(words[i]);
            }
            return sb.ToString();
        }

        public static void Genres(dynamic genres, MovieEntities db)
        {
            Genre genre = new Genre();

            string genrename = "";
            if (genres[0]["genres"] != null)
            {
                for (int i = 0; i < genres[0]["genres"].Count; i++)
                {
                    genrename = genres[0]["genres"][i];
                    if (!String.IsNullOrWhiteSpace(genrename))
                    {
                        var g = db.Genres.Where(gen => gen.Name == genrename).Distinct();
                    }
                }
            }
        }

        public static int ConvertRuntime(string runtime)
        {
            string result = "";
            for (int i = 0; i < runtime.Length; i++)
            {
                if (char.IsDigit(runtime[i]))
                    result += runtime[i];
            }
            return Int32.Parse(result); ;
        }

        private static void ProcessDynamic(dynamic obj)
        {

        }

        public static dynamic ConvertToObj(string str)
        {
            dynamic moreInfo = JsonConvert.DeserializeObject(str);
            string temp = moreInfo[0]["plot"];
            return moreInfo;
        }

        public static void Process(string filetowritedownloadeddatato, string movieId)
        {
            //System.IO.File.WriteAllLines(@"C:\Users\PrashMaya\Desktop\WriteFirst50Lines.txt", titleIds.ToArray());
            using (var client = new HttpClient())
            {
                movieId = "tt" + movieId;
                string url = "http://mymovieapi.com/?ids={0}&type=json&plot=full&episode=1&lang=en-US&aka=simple&release=simple&business=0&tech=0";
                string anotherurl = String.Format(url, movieId);
                client.BaseAddress = new Uri(anotherurl);
                HttpResponseMessage anotherresponse = client.GetAsync(anotherurl).Result;

                object obj = JsonConvert.DeserializeObject<object>(anotherresponse.Content.ReadAsStringAsync().Result);

                dynamic moreInfo = JsonConvert.DeserializeObject(obj.ToString());
                string temp = moreInfo[0]["plot"];
            }
        }


        private static void AddSampleRoleData()
        {
            Role role = new Role();
            role.Description = "Director";
            MovieEntities db = new MovieEntities();
            db.Roles.Add(role);
            db.SaveChanges();
        }

    }
}
