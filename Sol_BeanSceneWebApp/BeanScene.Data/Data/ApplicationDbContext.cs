using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace BeanSceneWebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }


        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<ReservationOrigion> ReservationOrigions { get; set; }
        public DbSet<ReservationTable> ReservationTables { get; set; }
        public DbSet<Resturant> Resturants { get; set; }
        public DbSet<Sitting> Sittings { get; set; }
        public DbSet<SittingType> SittingTypes { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Table> Tables { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
      
        public DbSet<Area> Areas { get; set; }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            new DataSeeder(mb);
            base.OnModelCreating(mb);
            mb.Entity<Person>().Property(p => p.UserId)

                .HasMaxLength(450).IsRequired(false);

            mb.Entity<Sitting>()
                .HasMany(p => p.Reservations)
                .WithOne(r => r.Sitting)
                .OnDelete(DeleteBehavior.Restrict);



            mb.Entity<Person>()
                .HasMany(p => p.Reservations)
                .WithOne(r => r.Person)
            .OnDelete(DeleteBehavior.Restrict);


            mb.Entity<Person>().HasOne(p => p.User).WithOne().IsRequired(false);
            mb.Entity<Person>().HasIndex(p => p.UserId).IsUnique(true);



            //mb.Entity<Sitting>()
            //.HasOne(s => s.Schedule)
            //.WithMany(schedule => schedule.Sittings)
            //.HasForeignKey(s => s.ScheduleId)
            //.OnDelete(DeleteBehavior.Restrict);



          //  mb.Entity<SittingSchedule>()
          //    .HasMany(p => p.Sittings)
          //    .WithOne(r => r.SittingSchedule)
          //.OnDelete(DeleteBehavior.Restrict);




            //   mb.Entity<Schedule>()
            //  .Property(s => s.StartDate)
            //  .HasColumnType("date"); // Use the appropriate database date type


            //   mb.Entity<Schedule>()
            //.Property(e => e.EndDate)
            //.HasColumnType("date");





        }

    }
}