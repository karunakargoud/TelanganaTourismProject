using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TouristPlacesProject.Models;

namespace TouristPlacesProject.DAL
{
    public class TouristContext:DbContext
    {
        public TouristContext() : base()
        {

        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TouristPlaceType>().ToTable("TouristPlaceType");
            modelBuilder.Entity<TouristPlace>().ToTable("TouristPlace");
            modelBuilder.Entity<Users>().ToTable("Users");

        }
        public DbSet<TouristPlaceType> touristPlaceTypes { get;set; }
        public DbSet<TouristPlace> touristPlaces { get;set; }  

        public DbSet<Users> users { get;set; }
    }
}