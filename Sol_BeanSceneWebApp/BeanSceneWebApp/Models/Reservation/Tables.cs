using BeanSceneWebApp.Data;

namespace BeanSceneWebApp.Models.Reservation
{
    public class Tables
    {

        public int ReservationId { get; set; }

        public string Information { get; set; }

        public List<Area> Areas { get; set; }

        public List<int> TableIds { get; set; }

        public int Id { get; set; }
        public string Name { get; set; }

        public int Occupied { get; set; }

        
    }
}
