﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class MovieEntities : DbContext
    {
        public MovieEntities()
            : base("name=MovieEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Country> Countries { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<MovieCountry> MovieCountries { get; set; }
        public DbSet<MoviePersonRole> MoviePersonRoles { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<PosterInfo> PosterInfoes { get; set; }
        public DbSet<Role> Roles { get; set; }
    }
}
