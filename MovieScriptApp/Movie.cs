//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MovieScriptApp
{
    using System;
    using System.Collections.Generic;
    
    public partial class Movie
    {
        public Movie()
        {
            this.MoviePersonRoles = new HashSet<MoviePersonRole>();
            this.PosterInfoes = new HashSet<PosterInfo>();
            this.Genres = new HashSet<Genre>();
            this.Languages = new HashSet<Language>();
            this.Countries = new HashSet<Country>();
        }
    
        public long ID { get; set; }
        public string Title { get; set; }
        public Nullable<System.DateTime> ReleaseDate { get; set; }
        public Nullable<int> Director { get; set; }
        public string Language { get; set; }
        public string Producer { get; set; }
        public string PlotSimple { get; set; }
        public string PlotDetailed { get; set; }
        public string AKA { get; set; }
        public string ImdbUrl { get; set; }
        public Nullable<int> Runtime { get; set; }
        public Nullable<decimal> IMDBRating { get; set; }
        public string Rated { get; set; }
        public string ImdbID { get; set; }
        public string MovieType { get; set; }
    
        public virtual ICollection<MoviePersonRole> MoviePersonRoles { get; set; }
        public virtual ICollection<PosterInfo> PosterInfoes { get; set; }
        public virtual ICollection<Genre> Genres { get; set; }
        public virtual ICollection<Language> Languages { get; set; }
        public virtual ICollection<Country> Countries { get; set; }
    }
}
