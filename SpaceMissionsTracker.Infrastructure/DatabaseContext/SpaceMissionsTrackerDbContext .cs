using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SpaceMissionsTracker.Core.Entities;
using SpaceMissionsTracker.Core.Identity;

namespace SpaceMissionsTracker.Infrastructure.DatabaseContext
{
    public class SpaceMissionsTrackerDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public SpaceMissionsTrackerDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Agency> Agencies { get; set; }
        public DbSet<Astronaut> Astronauts { get; set; }
        public DbSet<Rocket> Rockets { get; set; }
        public DbSet<Mission> Missions { get; set; }
        public DbSet<MissionAstronaut> MissionAstronauts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Agency>().ToTable("Agencies");


            modelBuilder.Entity<Agency>().HasData(
                new Agency { Id = 1, Name = "NASA", Country = "USA", Founded = 1958 },
                new Agency { Id = 2, Name = "ESA", Country = "Europe", Founded = 1975 },
                new Agency { Id = 3, Name = "Roscosmos", Country = "Russia", Founded = 1992 },
                new Agency { Id = 4, Name = "SpaceX", Country = "USA", Founded = 2002 }
            );

            modelBuilder.Entity<Astronaut>().HasData(
                new Astronaut { Id = 1, Name = "Neil Armstrong", Nationality = "USA", BirthYear = 1930 },
                new Astronaut { Id = 2, Name = "Buzz Aldrin", Nationality = "USA", BirthYear = 1930 },
                new Astronaut { Id = 3, Name = "Yuri Gagarin", Nationality = "USSR", BirthYear = 1934 },
                new Astronaut { Id = 4, Name = "Sally Ride", Nationality = "USA", BirthYear = 1951 }
            );

            modelBuilder.Entity<Rocket>().HasData(
            new Rocket { Id = 1, Name = "Saturn V", AgencyId = 1, FirstLaunch = 1967 },
            new Rocket { Id = 2, Name = "Falcon 9", AgencyId = 4, FirstLaunch = 2010 },
            new Rocket { Id = 3, Name = "Soyuz", AgencyId = 3, FirstLaunch = 1966 }
            );

            modelBuilder.Entity<Mission>().HasData(
                new Mission { Id = 1, Name = "Apollo 11", RocketId = 1, LaunchDate = new DateTime(1969, 7, 16), Status = "Completed", Destination = "Moon" },
                new Mission { Id = 2, Name = "Vostok 1", RocketId = 3, LaunchDate = new DateTime(1961, 4, 12), Status = "Completed", Destination = "Earth Orbit" },
                new Mission { Id = 3, Name = "Artemis I", RocketId = 1, LaunchDate = new DateTime(2022, 11, 16), Status = "Completed", Destination = "Moon Orbit" }
            );

            //--------------------------------

            modelBuilder.Entity<MissionAstronaut>()
                .HasKey(ma => new { ma.MissionId, ma.AstronautId });


            modelBuilder.Entity<MissionAstronaut>()
                .HasOne(ma => ma.Mission)
                .WithMany(m => m.MissionAstronauts)
                .HasForeignKey(ma => ma.MissionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MissionAstronaut>()
                .HasOne(ma => ma.Astronaut)
                .WithMany(a => a.MissionAstronauts)
                .HasForeignKey(ma => ma.AstronautId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MissionAstronaut>().HasData(
                new MissionAstronaut { MissionId = 1, AstronautId = 1 },
                new MissionAstronaut { MissionId = 1, AstronautId = 2 },
                new MissionAstronaut { MissionId = 2, AstronautId = 3 }
            );
        }

    }
}
