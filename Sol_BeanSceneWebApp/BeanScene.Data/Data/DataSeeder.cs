using Microsoft.EntityFrameworkCore;

namespace BeanSceneWebApp.Data
{
    public class DataSeeder
    {



        public DataSeeder(ModelBuilder mb)
        {


            mb.Entity<Resturant>().HasData(
            new Resturant { Id = 1, Name = "BeanScene" }


          );

            mb.Entity<Status>().HasData(
               new Status { Id = 1, Name = "Pending" },
               new Status { Id = 2, Name = "Confirmed" },
               new Status { Id = 3, Name = "Seated" },
               new Status { Id = 4, Name = "Completed" },
               new Status { Id = 5, Name = "Canceled" }
           );

        

            mb.Entity<Area>().HasData(
                new Area { Id = 1, Name = "Main Dinning", ResturantId = 1 },
                 new Area { Id = 2, Name = "Outside", ResturantId = 1 },
                  new Area { Id = 3, Name = "Balcony", ResturantId = 1 }

            );


            mb.Entity<SittingType>().HasData(
              new SittingType { Id = 1, Name = "Breakfast" },
               new SittingType { Id = 2, Name = "Lunch" },
                new SittingType { Id = 3, Name = "Dinner" },
                new SittingType { Id = 4, Name = "None" }

          );

      


            mb.Entity<ProductCategory>().HasData(
             new ProductCategory { Id = 1, Name = "Appetizer " },
              new ProductCategory { Id = 2, Name = "Main" },
               new ProductCategory { Id = 3, Name = "Dessert" },
                 new ProductCategory { Id = 4, Name = "Beverage" },
                  new ProductCategory { Id = 5, Name = "KidesMenu" }

         );


            mb.Entity<ReservationOrigion>().HasData(
                new ReservationOrigion { Id = 1, Name = "Online", Description = "Online Reservation" },
                new ReservationOrigion { Id = 2, Name = "Via Phone", Description = "Staff Create a Reservation" },
                new ReservationOrigion { Id = 3, Name = "Via Email", Description = "Staff Create a Reservation" },
                new ReservationOrigion { Id = 4, Name = "Walked in", Description = "Staff Create a Reservation" }

                );


            var rTable = new List<Table>();
            for (int i = 1; i < 11; i++)
            {
                var table = new Table
                {
                    Id = i,
                    Name = $"M{i}",
                    AreaId = 1

                };
                rTable.Add(table);
            }
            var rTable2 = new List<Table>();
            for (int i = 1; i < 11; i++)
            {
                var table = new Table
                {
                    Id = i + 10,

                    Name = $"O{i}",
                    AreaId = 2

                };
                rTable2.Add(table);
            }
            var rTable3 = new List<Table>();
            for (int i = 1; i < 11; i++)
            {
                var table = new Table
                {
                    Id = i + 20,
                    Name = $"B{i}",
                    AreaId = 3

                };
                rTable3.Add(table);
            }


            mb.Entity<Table>().HasData(rTable);
            mb.Entity<Table>().HasData(rTable2);

            mb.Entity<Table>().HasData(rTable3);

        }

    }
}
