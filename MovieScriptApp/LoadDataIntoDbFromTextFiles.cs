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

            string englishMovieApis = @"C:\Users\PrashMaya\Documents\Movies\HindiMovieMovieApi";
            string inputpath = "C:\\Users\\PrashMaya\\Desktop\\IMDBMovieTitleIds-0-2500.txt";
            string inputfolder = "C:\\Users\\PrashMaya\\My Documents\\First2500MoviesIMDB\\Movie{0}.txt";
            inputpath = String.Format(inputfolder, 1);


            foreach (string file in Directory.EnumerateFiles(englishMovieApis, "*.txt"))
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
            Movie savedMovie = null;
            try
            {
                MyMovieEntities db = new MyMovieEntities();

                string imdbId = obj[0]["imdb_id"];
                //if (imdbId == "tt0025037")
                //if(!db.Movies.Select(m => m.ImdbID == imdbId).First())
                //if (db.Movies.Count() > 0 || !db.Movies.Select(m => m.ImdbID == imdbId).First())
                {
                    string movieId = obj[0]["imdb_id"];
                    Movie movie = new Movie();
                    var testIfMovie = db.Movies.Where(mi => mi.ImdbID == movieId);
                    //Movie containsMovie = db.Movies.Where(m => m.ImdbID == movieId).ToList<Movie>().First();
                    if (testIfMovie.Count() == 0)
                    {
                        Int64 existingMovieId = 0;
                        if (db.Movies.Count() > 0)
                            existingMovieId = db.Movies.Count();

                        //movie.ID = existingMovieId + 1;
                        movie.PlotDetailed = obj[0]["plot"] == null ? null : obj[0]["plot"]; ;
                        movie.ImdbID = obj[0]["imdb_id"] == null ? null : obj[0]["imdb_id"]; ;
                        movie.PlotSimple = obj[0]["plot_simple"] == null ? null : obj[0]["plot_simple"]; ;
                        var tempruntime = obj[0]["runtime"] == null ? null : obj[0]["runtime"];
                        if (tempruntime != null)
                            movie.Runtime = ConvertRuntime(tempruntime.ToString());
                        movie.Rated = obj[0]["rated"] == null ? null : obj[0]["rated"]; ;
                        movie.ImdbUrl = obj[0]["imdb_url"] == null ? null : obj[0]["imdb_url"];
                        if (obj[0]["also_known_as"] != null)
                            movie.AKA = obj[0]["also_known_as"][0] == null ? null : obj[0]["also_known_as"][0];
                        movie.IMDBRating = obj[0]["rating"] == null ? null : obj[0]["rating"];
                        Int64? releaseDate = obj[0]["release_date"] == null ? null : obj[0]["release_date"];
                        DateTime? dtTime = null;
                        if (releaseDate != null)
                        {
                            int year = Int32.Parse(releaseDate.ToString().Substring(0, 4));
                            if (year != 0)
                            {
                                int month = Int32.Parse(releaseDate.ToString().Substring(4, 2));
                                if (month == 0)
                                    month = 1;

                                int day = Int32.Parse(releaseDate.ToString().Substring(6, 2));
                                if (day == 0)
                                    day = 1;
                                dtTime = new DateTime(year, month, day);
                            }
                        }
                        movie.ReleaseDate = dtTime;
                        movie.Title = obj[0]["title"] == null ? null : obj[0]["title"];
                        movie.RatingCount = obj[0]["rating_count"] == null ? null : obj[0]["rating_count"];


                        // If movie is not saved but genre is saved for some reason


                        Genres(obj, db, movie);
                        db.Movies.Add(movie);
                        db.SaveChanges();
                    }

                    if (movie.ImdbID != null)
                        savedMovie = db.Movies.Where(m => m.ImdbID == movie.ImdbID).ToList<Movie>().Count() > 0 ? db.Movies.Where(m => m.ImdbID == movie.ImdbID).ToList<Movie>().First() : null;
                    if (savedMovie != null)
                    {
                        savedMovie = db.Movies.Where(m => m.ImdbID == movieId).ToList<Movie>().First();

                        if (savedMovie.MovieLanguages.Count() == 0 || savedMovie.MovieLanguages.Select(s => s.Language == null).ToList<bool>().First())
                            MovieLanguage(obj, db, savedMovie);

                        if (savedMovie.MoviePersonRoles.Count() == 0)
                            MoviePersonRole(obj, db, savedMovie);

                        var ListOfGenre = obj[0]["genres"] == null ? null : obj[0]["genres"];
                        Poster poster = new Poster();
                        if (obj[0]["poster"] != null && savedMovie.PosterInfoes.Count == 0)
                        {
                            poster.imdb = obj[0]["poster"]["imdb"] == null ? null : obj[0]["poster"]["imdb"];
                            poster.cover = obj[0]["poster"]["cover"] == null ? null : obj[0]["poster"]["cover"];
                            PosterInfo posterInfo = new PosterInfo();
                            posterInfo.Imdb = poster.imdb;
                            posterInfo.Cover = poster.cover;
                            posterInfo.MovieId = savedMovie.ID;
                            posterInfo.ImdbID = savedMovie.ImdbID;
                            posterInfo.MovieId = savedMovie.ID;
                            db.PosterInfoes.Add(posterInfo);
                            //db.PosterInfoes.Add(posterInfo);
                            db.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var testSavedMovie = savedMovie;
                string inputfolder = "C:\\Users\\PrashMaya\\My Documents\\Exceptions\\Movie{0}.txt";
                var temp = savedMovie == null ? null : savedMovie.ImdbID;
                string inputfile = String.Format(inputfolder, temp);
                StringBuilder textToWrite = new StringBuilder(temp);
                textToWrite.Append(ex.ToString());
                System.IO.File.WriteAllText(inputfile, textToWrite.ToString());

                Console.WriteLine(ex.Message);
            }
        }

        private static void MovieLanguage(dynamic obj, MyMovieEntities db, Movie savedMovie)
        {
            if (obj[0]["language"] != null)
            {
                for (int i = 0; i < obj[0]["language"].Count; i++)
                {
                    string languageName = obj[0]["language"][i];
                    var language = db.Languages.Where(l => l.Name == languageName).ToList<Language>();
                    if (language.Count() == 0)
                    {
                        Language newlanguage = new Language();
                        newlanguage.Name = languageName;
                        db.Languages.Add(newlanguage);
                        db.SaveChanges();
                        language = db.Languages.Where(l => l.Name == languageName).ToList<Language>();
                    }
                    MovieLanguage movieLanguage = new MovieLanguage();
                    movieLanguage.MovieId = savedMovie.ID;
                    movieLanguage.LanguageId = language.First().ID;
                    db.MovieLanguages.Add(movieLanguage);
                    db.SaveChanges();
                }
            }
        }

        public static void MoviePersonRole(dynamic obj, MyMovieEntities db, Movie movie)
        {
            string actorName = string.Empty;
            List<Role> actorRole = db.Roles.Where(a => a.Description == "Actor").ToList<Role>();

            if (obj[0]["actors"] != null)
            {
                for (int i = 0; i < obj[0]["actors"].Count; i++)
                {
                    MoviePersonRole moviePersonRole = new MovieScriptApp.MoviePersonRole();
                    actorName = obj[0]["actors"][i];
                    AddPersons(obj, db, movie, actorName, moviePersonRole, actorRole);
                }
            }
            if (obj[0]["writers"] != null)
            {
                for (int i = 0; i < obj[0]["writers"].Count; i++)
                {
                    actorRole = db.Roles.Where(a => a.Description == "Writer").ToList<Role>();
                    MoviePersonRole moviePersonRole = new MovieScriptApp.MoviePersonRole();
                    actorName = obj[0]["writers"][i];
                    AddPersons(obj, db, movie, actorName, moviePersonRole, actorRole);
                }
            }
            if (obj[0]["directors"] != null)
            {
                for (int i = 0; i < obj[0]["directors"].Count; i++)
                {
                    actorRole = db.Roles.Where(a => a.Description == "Director").ToList<Role>();
                    MoviePersonRole moviePersonRole = new MovieScriptApp.MoviePersonRole();
                    actorName = obj[0]["directors"][i];
                    AddPersons(obj, db, movie, actorName, moviePersonRole, actorRole);
                }
            }
        }

        private static void AddPersons(dynamic obj, MyMovieEntities db, Movie movie, string actorName, MoviePersonRole moviePersonRole, List<Role> roles)
        {
            Person person = new Person();
            string firstName = SplitNames(actorName).First();
            string lastName = SplitNames(actorName).Last();
            person.FirstName = firstName;
            person.MiddleName = ReturnMiddleName(actorName);
            person.LastName = lastName;
            person.FullName = actorName;
            person.Gender = "Test";
            db.People.Add(person);
            db.SaveChanges();

            db.MoviePersonRoles.Add(new MoviePersonRole()
            {
                MovieId = movie.ID,
                RoleId = roles.First().ID,
                PersonId = person.ID
            });

            db.SaveChanges();
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
            for (int i = 1; i < words.Count() - 1; i++)
            {
                sb.Append(words[i]);
            }
            return sb.ToString();
        }

        public static void Genres(dynamic genres, MyMovieEntities db, Movie movie)
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
                        if (db.Genres.Select(g => g.Name == genrename).ToList<bool>().First())
                        {
                            Genre alreadyExistsGenre = db.Genres.Where(g => g.Name == genrename).ToList<Genre>().First();
                            movie.Genres.Add(alreadyExistsGenre);
                            db.SaveChanges();
                        }
                        else
                        {
                            movie.Genres.Add(new Genre()
                                {
                                    Name = genrename
                                });
                            db.SaveChanges();
                        }
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
                if (result.Length == 3)
                    break;
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
            MyMovieEntities db = new MyMovieEntities();
            db.Roles.Add(role);
            db.SaveChanges();
        }

    }
}
