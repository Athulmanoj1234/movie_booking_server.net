using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using movie_booking.Models;
using movie_booking.Models.Ttheatre;

namespace movie_booking.data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Admin> Admins { get; set; }   
        public DbSet<MovieInfo> MovieInfos { get; set; }
        public DbSet<DirectorInfo> DirectorInfos { get; set; }
        public DbSet<MovieInfoDirector> MoviesInfoDirectors { get; set; }
        public DbSet<ActorInfo> ActorInfos { get; set; }
        public DbSet<MovieInfoActor> MoviesInfoActors { get; set; }
        public DbSet<WriterInfo> WriterInfos { get; set; }
        public DbSet<MovieInfoWriter> MoviesInfoWriters { get; set; }
        public DbSet<TheatreInfo> TheatreInfos { get; set; }
        public DbSet<Screen> Screens { get; set; }
        public DbSet<TheatreLocation> TheatreLocations { get; set; }
        public DbSet<TheatreSeat> TheatreSeats { get; set; }
        public DbSet<ShowsList> showsLists { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //movies-director

            modelBuilder.Entity<MovieInfoDirector>()
                .HasKey(md => new { md.MovieInfoId, md.DirectorId });   // "The combination of MovieInfoId and DirectorId is the primary key."  "Each (Movie, Director) pair must be unique in the table."

            modelBuilder.Entity<MovieInfoDirector>()
                .HasOne(md => md.MovieInfo)
                .WithMany(mi => mi.MovieInfoDirectors)
                .HasForeignKey(mi => mi.MovieInfoId);

            modelBuilder.Entity<MovieInfoDirector>()
                .HasOne(md => md.DirectorInfo)
                .WithMany(di => di.MoviesInfoDirectors)
                .HasForeignKey(md => md.DirectorId);

            //movies-writers

            modelBuilder.Entity<MovieInfoWriter>()
                .HasKey(md => new { md.MovieInfoId, md.WriterId });

            modelBuilder.Entity<MovieInfoWriter>()
                .HasOne(mw => mw.MovieInfo)
                .WithMany(mi => mi.MovieInfoWriters)
                .HasForeignKey(mi => mi.MovieInfoId);

            modelBuilder.Entity<MovieInfoWriter>()
                .HasOne(mw => mw.WriterInfo)
                .WithMany(wi => wi.MoviesInfoWriters)
                .HasForeignKey(mw => mw.WriterId);

            //movies-actors

            modelBuilder.Entity<MovieInfoActor>()
                .HasKey(ma => new { ma.MovieInfoId, ma.ActorId });

            modelBuilder.Entity<MovieInfoActor>()
                .HasKey(ma => new { ma.MovieInfoId, ma.ActorId });

            modelBuilder.Entity<MovieInfoActor>()
                .HasOne(ma => ma.MovieInfo)
                .WithMany(mi => mi.MovieInfoActors)
                .HasForeignKey(ma => ma.MovieInfoId);

            modelBuilder.Entity<MovieInfoActor>()
                .HasOne(ma => ma.ActorInfo)
                .WithMany(ai => ai.MoviesInfoActors)
                .HasForeignKey(ma => ma.ActorId);


            //individual movie-director, actor, writer relationships
            modelBuilder.Entity<MovieInfo>()
                .HasMany(mi => mi.DirectorInfo)
                .WithMany(di => di.MovieInfo);

            modelBuilder.Entity<MovieInfo>()
                .HasMany(mi => mi.WriterInfo)
                .WithMany(wi => wi.MovieInfo);

            modelBuilder.Entity<MovieInfo>()
                .HasMany(mi => mi.ActorInfo)
                .WithMany(ai => ai.MovieInfo);

            // for theatres
            modelBuilder.Entity<TheatreInfo>()
                .HasMany(ti => ti.Screen)
                .WithOne(s => s.TheatreInfo);

            modelBuilder.Entity<TheatreLocation>()
                .HasOne(tl => tl.TheatreInfo)
                .WithOne(ti => ti.TheatreLocation);

            modelBuilder.Entity<Screen>()
                .HasMany(s => s.TheatreSeats)
                .WithOne(ts => ts.Screen);

            modelBuilder.Entity<ShowsList>()
                .HasOne(sl => sl.Screen)
                .WithMany(s => s.ShowsLists);

            modelBuilder.Entity<TheatreSeat>()
                .HasOne(ts => ts.Screen)
                .WithMany(s => s.TheatreSeats);

        }





    }
}
