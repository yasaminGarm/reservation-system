namespace BeanSceneWebApp.Data
{
    public class Status
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public List<Reservation> Reservations { get; set; }
    }
}
