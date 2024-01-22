using BeanSceneWebApp.Data;

namespace BeanSceneWebApp.Models.Reservation
{
    public class Index
    {
        public List<Data.Reservation> Reservations {get; set;} = new List<Data.Reservation>();
        public string PersonSearch { get; set; }
        public int StatusIdSearch { get; set; }

    }
}
