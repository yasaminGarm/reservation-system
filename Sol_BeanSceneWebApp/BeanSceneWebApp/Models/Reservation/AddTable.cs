namespace BeanSceneWebApp.Models.Reservation
{
    public class AddTable
    {
        public int ReservationId { get; set; }
        public int SittingId { get; set; }
        public List<Models.Reservation.Tables> Tables { get; set; }
    }
}
